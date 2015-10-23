using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using SimpleFollow.Helpers;
using SimpleFollow.Network;
using SimpleFollow.Party;
using SimpleFollow.UI;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors.Gizmos;
using Zeta.TreeSharp;

namespace SimpleFollow.Behaviors
{
    /// <summary>
    /// Class LeaderComposite.
    /// </summary>
    public class LeaderComposite
    {
        /// <summary>
        /// The _original bot behavior
        /// </summary>
        private static List<Composite> _originalBotBehavior;

        /// <summary>
        /// The _leader behavior
        /// </summary>
        private static Composite _leaderBehavior;

        /// <summary>
        /// Gets the leader behavior.
        /// </summary>
        /// <value>The leader behavior.</value>
        public static Composite LeaderBehavior
        {
            get { return _leaderBehavior; }
            private set { _leaderBehavior = value; }
        }

        /// <summary>
        /// Creates the behavior.
        /// </summary>
        /// <returns>Composite.</returns>
        internal static Composite CreateBehavior()
        {
            return new ActionRunCoroutine(ret => LeaderInGameTask());
        }

        private static readonly Func<bool> ValidCheck = () => ZetaDia.IsInGame && !ZetaDia.IsLoadingWorld &&
                                                              ZetaDia.IsInTown && ZetaDia.Me != null && ZetaDia.Me.IsValid && Social.IsInParty && Social.IsPartyleader && !ZetaDia.Me.IsInCombat;

        private static readonly Func<bool> ValidCombatCheck = () => ZetaDia.IsInGame && !ZetaDia.IsLoadingWorld &&
                                                              ZetaDia.Me != null && ZetaDia.Me.IsValid && Social.IsInParty && !ZetaDia.Me.IsInCombat;

        private static readonly Func<bool> AllFollowersInGame = () => SimpleFollow.Followers.All(f => f.Value.IsInSameGame && f.Value.IsInGame);

        private static readonly Func<bool> AnyFollowersNotInTown = () => ZetaDia.IsInTown && SimpleFollow.Followers.Any(f => !f.Value.IsInTown);

        private static readonly Func<bool> AnyFollowersInCombat = () => ZetaDia.IsInTown && SimpleFollow.Followers.Any(f => f.Value.IsInCombat);

        /// <summary>
        /// Leaders the in game task.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal static async Task<bool> LeaderInGameTask()
        {
            if (ValidCombatCheck() && AnyFollowersInCombat())
            {
                SimpleFollow.Pulse();
                Logr.Log("A friend is in combat - to the rescue!");

                ZetaDia.Me.TeleportToPlayerByIndex(SimpleFollow.Followers.FirstOrDefault(f => f.Value.IsInCombat).Value.CPlayerIndex);
                await Coroutine.Sleep(200);
                await Coroutine.Yield();
            }

            // In Party but no followers connected - just wait!
            while (Settings.Instance.WaitForFollowers && ValidCheck() && Social.NumPartyMembers - 1 != SimpleFollow.Followers.Count)
            {
                SimpleFollow.Pulse();
                Logr.Log("Waiting for party members to connect to SimpleFollow server...");
                await Coroutine.Yield();
            }

            while (Settings.Instance.WaitForFollowers && ValidCheck() && !AllFollowersInGame())
            {
                SimpleFollow.Pulse();
                Logr.Log("Waiting for party members to join same game...");
                await Coroutine.Yield();
            }

            while (Settings.Instance.WaitForFollowers && ValidCheck() && AnyFollowersNotInTown())
            {
                SimpleFollow.Pulse();
                Logr.Log("Waiting for party members to come to town...");
                await Coroutine.Yield();
            }

            // Wait for followers to open greater rift
            while (ValidCheck() && ShouldFollowerOpenRift())
            {
                SimpleFollow.Pulse();
                Logr.Log("Waiting for follower to open rift...");
                await Coroutine.Yield();
            }
            return false;
        }

        private const int RiftPortalSNO = 345935; // X1_OpenWorld_LootRunPortal

        internal static bool ShouldFollowerOpenRift()
        {
            if (Player.IsGreaterRiftStarted)
                return false;

            if (ZetaDia.WorldType != Act.OpenWorld)
                return false;

            if (SimpleFollow.IsLeader)
            {
                // Current profile tag must be OpenRift / QTOpenRiftWrapper
                var currentBehavior = ProfileManager.CurrentProfileBehavior;
                if (currentBehavior != null && !currentBehavior.GetType().Name.Contains("OpenRift"))
                    return false;

                if (!SimpleFollow.Followers.Any())
                    return false;

                if (!SimpleFollow.Followers.Any(f => f.Value.HasRiftKeys))
                    return false;
            }

            // In regular rift quests
            if (ConditionParser.IsActiveQuestAndStep(337492, 1) ||
                ConditionParser.IsActiveQuestAndStep(337492, 3) ||
                ConditionParser.IsActiveQuestAndStep(337492, 13) ||
                ConditionParser.IsActiveQuestAndStep(337492, 16))
                return false;

            var portals = ZetaDia.Actors.GetActorsOfType<GizmoPortal>(true).Where(p => p.ActorSNO == RiftPortalSNO).ToList();
            if (portals.Any())
            {
                return false;
            }

            var allFollowersConnected = SimpleFollow.Followers.Count == Social.NumPartyMembers - 1;

            Logr.Debug("Follower Count={0}, NumPartyMembers={1}", SimpleFollow.Followers.Count, Social.NumPartyMembers);
            Logr.Debug("HighestTeamRiftKey={0} HighestLevelTieredRiftKey={1}", SimpleFollow.Leader.HighestTeamRiftKey, SimpleFollow.Leader.HighestLevelTieredRiftKey);

            var result = allFollowersConnected && SimpleFollow.Leader.HighestTeamRiftKey <= SimpleFollow.Leader.HighestLevelTieredRiftKey;
            Logr.Debug("RequestFollowerOpenRift?={0}", result);
            return result;
        }

        private static bool _hookReplaced;

        /// <summary>
        /// Replaces the bot behavior.
        /// </summary>
        internal static void ReplaceBotBehavior()
        {
            if (!Plugin.Enabled)
            {
                Logr.Log("Plugin not enabled - restoring original BotBehavior");
                RestoreOriginalBotBehavior();
                return;
            }
            if (SimpleFollow.IsFollower)
                return;

            const string hookName = "BotBehavior";

            if (!TreeHooks.Instance.Hooks.ContainsKey(hookName))
                return;

            if (_originalBotBehavior == null)
                _originalBotBehavior = TreeHooks.Instance.Hooks[hookName];

            _leaderBehavior = CreateBehavior();

            Logr.Log("Inserting into BotBehavior hook");
            TreeHooks.Instance.Hooks[hookName].Insert(0, _leaderBehavior);
            _hookReplaced = true;
        }

        /// <summary>
        /// Restores the original bot behavior.
        /// </summary>
        internal static void RestoreOriginalBotBehavior()
        {
            const string hookName = "BotBehavior";
            if (_originalBotBehavior != null && TreeHooks.Instance.Hooks.ContainsKey(hookName) && _hookReplaced)
            {
                Logr.Log("Replacing {0} hook to original", hookName);
                TreeHooks.Instance.ReplaceHook(hookName, new Sequence(_originalBotBehavior.ToArray()));
                _hookReplaced = false;
            }
        }

        /// <summary>
        /// Creates the out of game behavior.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns>Composite.</returns>
        internal static Composite CreateOutOfGameBehavior(List<Composite> children)
        {
            return new Sequence(
                new ActionRunCoroutine(ret => LeaderOutOfGameTask()),
                new Sequence(children.ToArray())
                );
        }

        /// <summary>
        /// Leader Out Of Game Coroutine
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal static async Task<bool> LeaderOutOfGameTask()
        {
            LeaderService.LeaderOutOfGameUpdate();
            SimpleFollow.Pulse();

            // Some safety checks
            if (ZetaDia.IsInGame)
                return true;

            if (ZetaDia.IsLoadingWorld)
                return true;

            if (!ZetaDia.Service.Hero.IsValid)
                return true;

            if (!ZetaDia.Service.Platform.IsConnected)
                return true;

            if (!ZetaDia.Service.IsValid || !ZetaDia.Service.Hero.IsValid)
                return true;

            // In Party but no followers connected - just wait!
            if (Settings.Instance.WaitForFollowers && Social.IsInParty && Social.IsPartyleader && Social.NumPartyMembers - 1 != SimpleFollow.Followers.Count)
            {
                Logr.Log("Waiting for followers to connect to SimpleFollow server...");
                return false;
            }
            if (!Social.IsPartyleader)
            {
                Logr.Log("Waiting to become party leader...");
                return false;
            }

            if (!SimpleFollow.Followers.Any())
                return true;

            var followersInGame = SimpleFollow.Followers.Where(f => DateTime.UtcNow.Subtract(f.Value.LastTimeInGame).TotalSeconds < 10).ToList();
            if (Social.IsInParty && followersInGame.Any() && !GameUI.PlayGameButton.IsEnabled)
            {
                Logr.Log("Waiting for {0} followers to leave game...", followersInGame.Count());
                await Coroutine.Sleep(500);
                return false;
            }

            return true;
        }
    }
}