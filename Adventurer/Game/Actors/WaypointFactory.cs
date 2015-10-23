using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Zeta.Game;
using Zeta.Game.Internals.Actors.Gizmos;

namespace Adventurer.Game.Actors
{
    public static class WaypointFactory
    {
        public static readonly Dictionary<Act, int> ActHubs = new Dictionary<Act, int>
            {
                {Act.A1, 0},
                {Act.A2, 18},
                {Act.A3, 27},
                {Act.A4, 27},
                {Act.A5, 49}

            };

        public static int GetWaypointNumber(int levelAreaId)
        {
            return ZetaDia.Memory.CallInjected<int>(new IntPtr(0x0112DBC0),
                CallingConvention.Cdecl, levelAreaId);
        }

        public static bool NearWaypoint(int waypointNumber)
        {
            var gizmoWaypoint = ZetaDia.Actors.GetActorsOfType<GizmoWaypoint>().OrderBy(g => g.Distance).FirstOrDefault();
            if (gizmoWaypoint != null && gizmoWaypoint.IsFullyValid())
            {
                if (gizmoWaypoint.WaypointNumber == waypointNumber && gizmoWaypoint.Distance <= 500)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
