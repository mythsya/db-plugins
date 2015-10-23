using System;
using Zeta.Common;
using Zeta.Game;

namespace Trinity.Framework.Grid
{
    public class TrinityNode : IEquatable<TrinityNode>
    {
        public Vector3 NavigableCenter { get; private set; }
        public Vector2 NavigableCenter2D { get; private set; }
        public Vector2 Center { get; private set; }
        public Vector2 TopLeft { get; private set; }
        public Vector2 BottomLeft { get; private set; }
        public Vector2 TopRight { get; private set; }
        public Vector2 BottomRight { get; private set; }
        public float Distance2DSqr { get { return NavigableCenter2D.DistanceSqr(ZetaDia.Me.Position.ToVector2()); } }
        public NodeFlags NodeFlags { get; set; }
        public uint CustomFlags { get; set; }
        public GridPoint GridPoint { get; set; }


        public TrinityNode(Vector3 center, float boxSize,  WorldSceneCell cell)
        {
            if (cell != null)
            {
                NodeFlags = (NodeFlags) cell.NavCellFlags;
            }

            var halfSize = (float)boxSize / 2;
            Center = center.ToVector2();
            TopLeft = Center + new Vector2(-(halfSize), -(halfSize));
            BottomLeft = Center + new Vector2(-(halfSize), halfSize);
            TopRight = Center + new Vector2(halfSize, -(halfSize));
            BottomRight = Center + new Vector2(halfSize, halfSize);
            NavigableCenter = center;
            NavigableCenter2D = Center;
        }

        public bool Equals(TrinityNode other)
        {
            return Center == other.Center;
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode();
        }

        public byte AStarValue { get; set; }

    }
}