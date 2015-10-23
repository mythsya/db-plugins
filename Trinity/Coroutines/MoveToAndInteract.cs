using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Trinity.Helpers;
using Zeta.Bot.Navigation;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Logger = Trinity.Technicals.Logger;

namespace TrinityCoroutines
{
    public class MoveToAndInteract
    {
        /// <summary>
        /// Moves to something and interacts with it
        /// </summary>
        /// <param name="obj">object to interact with</param>
        /// <param name="range">how close to get</param>
        /// <param name="interactLimit">maximum number of times to interact</param>
        public static async Task<bool> Execute(DiaObject obj, float range = -1f, int interactLimit = 5)
        {
            if (obj == null)
                return false;

            if (!obj.IsFullyValid())
                return false;

            if (interactLimit < 1) interactLimit = 5;
            if (range < 0) range = obj.CollisionSphere.Radius;

            if (obj.Position.Distance(ZetaDia.Me.Position) > range)
            {
                if (!await MoveTo.Execute(obj.Position, obj.Name))
                {
                    Logger.Log("MoveTo Failed for {0} ({1}) Distance={2}", obj.Name, obj.ActorSNO, obj.Distance);
                    return false;
                }                    
            }

            var distance = obj.Position.Distance(ZetaDia.Me.Position);
            if (distance <= range || distance - obj.CollisionSphere.Radius <= range)
            {
                for (int i = 1; i <= interactLimit; i++)
                {
                    Logger.LogVerbose("Interacting with {0} ({1}) Attempt={2}", obj.Name, obj.ActorSNO, i);
                    if (obj.Interact() && i > 1)
                        break;

                    await Coroutine.Sleep(500);
                    await Coroutine.Yield();
                }
            }

            // Better to be redundant than failing to interact.

            Navigator.PlayerMover.MoveTowards(obj.Position);
            await Coroutine.Sleep(500);
            obj.Interact();

            Navigator.PlayerMover.MoveStop();
            await Coroutine.Sleep(1000);
            await Interact(obj);
            return true;
        }

        /// <summary>
        /// Moves to a position, finds actor by Id and interacts with it
        /// </summary>
        /// <param name="actorId">id of actor to interact with</param>
        /// <param name="range">how close to get</param>
        /// <param name="position">position from which to interact</param>
        /// <param name="interactLimit">maximum number of times to interact</param>
        public static async Task<bool> Execute(Vector3 position, int actorId, float range = -1f, int interactLimit = 5)
        {
            if (position == Vector3.Zero)
                return false;

            if (interactLimit < 1) interactLimit = 5;
            if (range < 0) range = 2f;

            if (position.Distance(ZetaDia.Me.Position) > range)
            {
                if (!await MoveTo.Execute(position, position.ToString()))
                {
                    Logger.Log("MoveTo Failed for {0} Distance={1}", position, position.Distance(ZetaDia.Me.Position));
                    return false;
                }
            }

            var actor = ZetaDia.Actors.GetActorsOfType<DiaObject>(true).FirstOrDefault(a => a.ActorSNO == actorId);
            if (actor == null)
            {
                Logger.LogVerbose("Interaction Failed: Actor not found with Id={0}", actorId);
                return false;
            }

            var distance = position.Distance(ZetaDia.Me.Position);
            if (distance <= range || distance - actor.CollisionSphere.Radius <= range)
            {                    
                for (int i = 1; i <= interactLimit; i++)
                {
                    Logger.Log("Interacting with {0} ({1}) Attempt={2}", actor.Name, actor.ActorSNO, i);    
                    if (actor.Interact() && i > 1)
                        break;

                    await Coroutine.Sleep(100);
                    await Coroutine.Yield();
                }                
            }

            // Better to be redundant than failing to interact.

            Navigator.PlayerMover.MoveTowards(actor.Position);
            await Coroutine.Sleep(250);
            actor.Interact();

            Navigator.PlayerMover.MoveStop();
            await Coroutine.Sleep(1000);
            await Interact(actor);
            return true;
        }

        private static async Task<bool> Interact(DiaObject actor)
        {
            bool retVal = false;
            switch (actor.ActorType)
            {
                case ActorType.Gizmo:
                    switch (actor.ActorInfo.GizmoType)
                    {
                        case GizmoType.BossPortal:
                        case GizmoType.Portal:
                        case GizmoType.ReturnPortal:
                            retVal = ZetaDia.Me.UsePower(SNOPower.GizmoOperatePortalWithAnimation, actor.Position);
                            break;
                        default:
                            retVal = ZetaDia.Me.UsePower(SNOPower.Axe_Operate_Gizmo, actor.Position);
                            break;
                    }
                    break;
                case ActorType.Monster:
                    retVal = ZetaDia.Me.UsePower(SNOPower.Axe_Operate_NPC, actor.Position);
                    break;
            }

            // Doubly-make sure we interact
            actor.Interact();
            await Coroutine.Sleep(100);
            return retVal;
        }

    }
}
