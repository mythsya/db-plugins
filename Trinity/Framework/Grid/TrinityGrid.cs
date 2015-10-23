using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Trinity.Framework.Grid.Algorithms;
using Zeta.Common;
using Zeta.Game;

namespace Trinity.Framework.Grid
{
    public sealed class TrinityGrid : GridBase
    {
        private static readonly ConcurrentDictionary<int, Lazy<TrinityGrid>> WorldGrids = new ConcurrentDictionary<int, Lazy<TrinityGrid>>();
        public const float NodeBoxSize = 2.5f;
        private const int GRID_BOUNDS = 2500;

        public static TrinityGrid GetWorldGrid(int worldDynamicId)
        {
            return WorldGrids.GetOrAdd(worldDynamicId, new Lazy<TrinityGrid>(() => new TrinityGrid())).Value;
        }

        public static TrinityGrid Instance
        {
            get { return GetWorldGrid(ZetaDia.CurrentWorldDynamicId); }
        }

        public override float BoxSize
        {
            get { return NodeBoxSize; }
        }

        public override int GridBounds
        {
            get { return GRID_BOUNDS; }
        }

        public static void ResetAll()
        {
            WorldGrids.Clear();
        }

        public override bool CanRayCast(Vector3 from, Vector3 to)
        {
            return GetRayLine(from, to).Select(point => InnerGrid[point.X, point.Y]).All(node => node != null && node.NodeFlags.HasFlag(NodeFlags.AllowProjectile));
        }

        public override bool CanRayWalk(Vector3 from, Vector3 to)
        {
            return GetRayLine(from, to).Select(point => InnerGrid[point.X, point.Y]).All(node => node != null && node.NodeFlags.HasFlag(NodeFlags.AllowWalk));
        }

        private IEnumerable<GridPoint> GetRayLine(Vector3 from, Vector3 to)
        {
            var gridFrom = ToGridPoint(from);
            var gridTo = ToGridPoint(to);
            return Bresenham.GetPointsOnLine(gridFrom, gridTo);
        }

    }
}
