using System.Threading.Tasks;
using QuestTools.Helpers;
using Zeta.Bot;
using Zeta.Bot.Profile.Common;
using Zeta.TreeSharp;

namespace SimpleFollow.Behaviors
{
    class FollowerTakeWaypoint
    {
        public static Composite TakeWaypointBehavior()
        {
            return new ActionRunCoroutine(ret => TakeWaypointTask());
        }

        private static async Task<bool> TakeWaypointTask()
        {
            if (SimpleFollow.Leader.ProfileTagName != "UseWaypoint")
                return false;

            BotBehaviorQueue.Queue(new UseWaypointTag
            {
                WaypointNumber = SimpleFollow.Leader.ProfileWaypointNumber,
                X = SimpleFollow.Leader.Position.X,
                Y = SimpleFollow.Leader.Position.Y,
                Z = SimpleFollow.Leader.Position.Z,
            });
            return true;
        }
    }
}
