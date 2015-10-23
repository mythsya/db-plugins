using System;
using System.Collections.Generic;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using SimpleFollow.Network;
using SimpleFollow.Party;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Bot.Profile;

namespace SimpleFollow
{
    public class SimpleFollow
    {
        public static Version PluginVersion = new Version(3, 0, 0);

        /// <summary>
        /// The leader message
        /// </summary>
        internal static Message Leader = new Message();

        /// <summary>
        /// The followers. Key is BattleTag.HashCode
        /// </summary>
        internal static Dictionary<int, Message> Followers = new Dictionary<int, Message>();

        /// <summary>
        /// The last changed world
        /// </summary>
        internal static DateTime LastChangedWorld = DateTime.MinValue;

        internal static DateTime LastJoinedGame = DateTime.MinValue;

        public static bool Enabled
        {
            get { return Plugin.Enabled; }
        }

        public static void Pulse()
        {
            try
            {
                LeaderService.CleanExpiredFollowers();
                ServiceBase.Communicate();

                LeaderService.PulseInbox();

                SharedComposites.CheckReplaceOutOfGameHook();
            }
            catch (Exception ex)
            {
                Logr.Log("Exception thrown on Pulse: {0}", ex.ToString());
            }

            GameUI.SafeCheckClickButtons();
        }


        private static bool _isFollower;
        private static DateTime _lastCheckedIsFollower = DateTime.MinValue;

        internal static bool IsLeader
        {
            get { return !IsFollower; }
        }

        internal static bool IsFollower
        {
            get
            {
                if (DateTime.UtcNow.Subtract(_lastCheckedIsFollower) < TimeSpan.FromMilliseconds(500))
                    return _isFollower;

                _lastCheckedIsFollower = DateTime.UtcNow;

                try
                {
                    if (ProfileManager.CurrentProfile != null)
                    {
                        Profile p = ProfileManager.CurrentProfile;
                        _isFollower = p.Name.ToLower().Contains("simplefollow");
                        return _isFollower;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Logr.Log("Exception reading IsFollower current profile: {0}", ex);
                    return false;
                }
            }
        }
    }
}