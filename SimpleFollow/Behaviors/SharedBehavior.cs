using System.Collections.Generic;
using System.Linq;
using SimpleFollow.Helpers;
using Zeta.Bot;
using Zeta.Bot.Settings;
using Zeta.Common;
using Zeta.TreeSharp;

namespace SimpleFollow.Behaviors
{
    internal class SharedComposites
    {
        private static List<Composite> _originalOutOfGameComposites;
        private static bool _outOfGameHookReplaced;

        internal static bool OutOfGameHookReplaced
        {
            get { return _outOfGameHookReplaced; }
            set { _outOfGameHookReplaced = value; }
        }

        private static Composite _followerBehavior;

        internal static void CheckReplaceOutOfGameHook()
        {
            // Plugin was diabled, replace hook to original
            if (!SimpleFollow.Enabled)
            {
                if (_originalOutOfGameComposites != null)
                {
                    Logr.Log("Replacing OutOfGame hook to original");
                    TreeHooks.Instance.ReplaceHook("OutOfGame", new PrioritySelector(_originalOutOfGameComposites.ToArray()));
                }
                _outOfGameHookReplaced = false;
                return;
            }

            if (ProfileManager.CurrentProfile != null)
            {
                if (SimpleFollow.IsFollower && !_outOfGameHookReplaced)
                {
                    Logr.Log("Replacing OutOfGame hook with Follower Behavior");
                    StoreOriginalOutOfGameComposite();
                    _followerBehavior = FollowerComposite.CreateBehavior();
                    TreeHooks.Instance.ReplaceHook("OutOfGame", _followerBehavior);
                    _outOfGameHookReplaced = true;
                }
                else if (!SimpleFollow.IsFollower && !_outOfGameHookReplaced)
                {
                    Logr.Log("Replacing OutOfGame hook with Leader Behavior");
                    if (SimpleFollow.Enabled)
                    {
                        StoreOriginalOutOfGameComposite();

                        if (_originalOutOfGameComposites != null)
                        {
                            // Reference the leader composite (since we're no longer a follower)
                            TreeHooks.Instance.ReplaceHook("OutOfGame", LeaderComposite.CreateOutOfGameBehavior(_originalOutOfGameComposites));
                            _outOfGameHookReplaced = true;

                        }
                    }
                }
            }
        }

        internal static void StoreOriginalOutOfGameComposite()
        {
            if (_originalOutOfGameComposites == null)
                _originalOutOfGameComposites = TreeHooks.Instance.Hooks.FirstOrDefault(h => h.Key == "OutOfGame").Value;
        }
    }
}