using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using SimpleFollow.Party;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Bot.Logic;
using Zeta.Bot.Profile;

namespace SimpleFollow.Network
{
    internal class LeaderService : ServiceBase
    {
        internal static ConcurrentQueue<Message> Inbox = new ConcurrentQueue<Message>();

        /// <summary>
        /// This is a thread running on the leader bot to watch the current profile behavior, to speed up followers going to the right place at the right time
        /// </summary>
        internal static void BehaviorWatcher()
        {
            try
            {
                if (Enabled && !IsFollower)
                {
                    ProfileBehavior currentBehavior = ProfileManager.CurrentProfileBehavior;
                    if (currentBehavior != null)
                    {
                        if (currentBehavior.GetType().Name.ToLower().Contains("town"))
                        {
                            Leader.IsInTown = true;
                        }
                        if (currentBehavior.GetType().Name.ToLower().Contains("leavegame"))
                        {
                            Leader.IsInGame = false;
                        }

                        // The profile position for look-ahead
                        Leader.ProfilePosition = Message.GetProfilePosition();
                        Leader.ProfileActorSNO = Message.GetProfileActorSNO();
                    }

                    if (BrainBehavior.IsVendoring)
                    {
                        Leader.IsInTown = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logr.Log("Exception in BehaviorWatcher: {0}", ex);
            }
        }

        internal static void ResetLeaderService()
        {
            Host.Close();
            Host = null;
        }

        internal static void LeaderOutOfGameUpdate()
        {
            Leader = Message.GetMessage();
            if (Settings.Instance.DebugLogging)
                Logr.Debug("Leader OutOfGame: {0}", Leader);
        }


        /// <summary>
        /// Called by leader through Communicate()
        /// </summary>
        internal static void ServerUpdate()
        {
            if (!Enabled)
                return;
            // Need to initialize server service
            if (Host == null || (Host != null && Host.State != CommunicationState.Opened))
            {
                StartServer();
            }
            // Server service initialized, lets update
            if (Leader.GetMillisecondsSinceLastUpdate() >= 500)
            {
                Leader = Message.GetMessage();
                if (Followers.Any())
                    Leader.HighestTeamRiftKey = Followers.Min(f => f.Value.HighestLevelTieredRiftKey);
                SharedComposites.CheckReplaceOutOfGameHook();
            }
        }

        private static void StartServer()
        {
            try
            {
                var bindAddress = Settings.Instance.BindAddress;
                var serverPort = Settings.Instance.ServerPort;

                IPGlobalProperties igp = IPGlobalProperties.GetIPGlobalProperties();
                bool isAvailable =
                    igp.GetActiveTcpConnections().All(tcpi => tcpi.LocalEndPoint.Port != serverPort) &&
                    igp.GetActiveTcpListeners().All(l => l.Port != serverPort);

                // At this point, if isAvailable is true, we can proceed accordingly.
                if (isAvailable)
                {
                    ServerUri = new Uri("http://" + bindAddress + ":" + serverPort);

                    Logr.Log("Initializing Server Service @ {0}", ServerUri.AbsoluteUri);
                    Host = new ServiceHost(typeof (Server), new[] {ServerUri});
                    Host.AddServiceEndpoint(typeof (IFollowService), new BasicHttpBinding(), "Follow");
                    Host.Open();
                    Leader = Message.GetMessage();
                }
            }
            catch (Exception ex)
            {
                Logr.Log("ERROR: Could not initialize follow service. Do you already have a leader started?");
                Logr.Log(ex.ToString());

                Logr.Log("Disabling Plugin and Resetting Game Creation Wait Timers to default");
                Plugin.DisablePlugin();
            }
        }


        internal static void PulseInbox()
        {
            // Process Messages
            Queue<Message> messages = new Queue<Message>();

            try
            {
                lock (Inbox)
                    while (Inbox.ToList().Any())
                    {
                        Message msg;
                        if (Inbox.TryDequeue(out msg) && msg != null)
                            messages.Enqueue(msg);
                    }
            }
            catch (Exception ex)
            {
                Logr.Log(ex.ToString());
            }

            CleanExpiredFollowers();

            while (messages.ToList().Any())
            {
                var message = messages.Dequeue();

                if (message == null)
                    continue;

                try
                {
                    if (message.BattleTagHash != 0)
                    {
                        if (Followers.ContainsKey(message.BattleTagHash))
                            Followers[message.BattleTagHash] = message;
                        else
                            Followers.Add(message.BattleTagHash, message);
                    }

                    if (Settings.Instance.DebugLogging)
                        Logr.Debug("Received follower message: {0}", message);

                    if (Settings.Instance.UseHotSpots && message.HotSpot != null)
                        Trinity.SetTrinityHotSpot(message.HotSpot);

                    if (!message.IsInParty && !message.IsInGame && Leader.IsInGame)
                    {
                        Logr.Debug("Inviting {0} ({1}) to the party - Not in game. IsInGame={2} IsInParty={3} NumPartyMembers={4}",
                            message.ActorClass, message.BattleTagHash, message.IsInGame, message.IsInParty, message.NumPartymembers);
                        Social.Instance.CheckInvites();
                        continue;
                    }

                    if (!message.IsInParty && (message.GameId.FactoryId == 0 && message.GameId.High == 0 && message.GameId.Low == 0) && !Leader.IsInGame)
                    {
                        Logr.Debug("Inviting {0} ({1}) to the party - Needs Invite. IsInGame={2} IsInParty={3} NumPartyMembers={4}",
                            message.ActorClass, message.BattleTagHash, message.IsInGame, message.IsInParty, message.NumPartymembers);
                        Social.Instance.CheckInvites();
                        continue;
                    }

                    if (!Leader.GameId.Equals(message.GameId))
                    {
                        Logr.Debug("Inviting {0} ({1}) to the party - Incorrect Game. IsInGame={2} IsInParty={3} NumPartyMembers={4}",
                            message.ActorClass, message.BattleTagHash, message.IsInGame, message.IsInParty, message.NumPartymembers);
                        Social.Instance.CheckInvites();
                        continue;
                    }

                    if (Leader.GameId.Equals(message.GameId))
                    {
                        Social.Instance.CheckCloseSocialWindow();
                        continue;
                    }

                    Logr.Log("Message response: invalid/unknown state");
                }
                catch (Exception ex)
                {
                    Logr.Log("Exception receiving update from client!");
                    Logr.Log(ex.ToString());
                    return;
                }
            }
        }

        /// <summary>
        /// Remove followers who haven't updated in a while
        /// </summary>
        internal static void CleanExpiredFollowers()
        {
            foreach (var follower in Followers.Where(f => DateTime.UtcNow.Subtract(f.Value.LastTimeUpdated).TotalSeconds > 30).ToList())
            {
                Followers.Remove(follower.Key);
            }
        }
    }
}