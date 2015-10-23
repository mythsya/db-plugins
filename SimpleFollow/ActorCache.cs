using System.Linq;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace SimpleFollow
{
    public class ActorCache
    {
        public int ActorSNO { get; private set; }
        public string Name { get; private set; }
        public Vector3 Position { get; private set; }
        public float Distance { get; private set; }
        public float Radius { get; private set; }
        public float RadiusDistance { get; private set; }

        public bool IsValid
        {
            get { return RActor != null && RActor.IsValid && RActor.CommonData != null && RActor.CommonData.IsValid; }
        }

        public DiaObject RActor { get; private set; }
        public int ACDGuid { get; private set; }
        public bool IsReturnPortal { get; private set; }
        public bool IsMonster { get; private set; }
        public int RActorGuid { get; private set; }

        public ActorCache()
        {

        }

        public ActorCache(DiaObject actor)
        {
            Update(actor);
        }

        public bool Update()
        {
            var newActor = ZetaDia.Actors.GetActorsOfType<DiaObject>(true, true).FirstOrDefault(o => o.RActorGuid == RActorGuid);

            if (newActor == null)
                return false;
            return Update(newActor);
        }

        public bool Update(DiaObject actor)
        {
            if (actor == null)
                return false;

            if (!actor.IsValid)
                return false;

            RActor = actor;
            RActorGuid = actor.RActorGuid;
            ACDGuid = actor.ACDGuid;
            ActorSNO = actor.ActorSNO;
            Name = actor.Name;
            Position = actor.Position;
            Distance = actor.Distance;
            Radius = actor.CollisionSphere.Radius;
            RadiusDistance = Distance - Radius;

            if (actor.ActorType == Zeta.Game.Internals.SNO.ActorType.Gizmo)
                IsReturnPortal = actor.ActorInfo.GizmoType == Zeta.Game.Internals.SNO.GizmoType.ReturnPortal;

            if (actor.ActorType == Zeta.Game.Internals.SNO.ActorType.Monster)
                IsMonster = true;

            return true;
        }

        public bool Interact()
        {
            if (RActor != null && IsValid)
                return RActor.Interact();
            return false;
        }
    }
}