﻿using Zeta.Common;

namespace Adventurer.Game.Exploration
{
    public interface INode
    {
        Vector3 NavigableCenter { get; }
        Vector2 NavigableCenter2D { get; }
        Vector2 Center { get; }
        Vector2 TopLeft { get; }
        Vector2 BottomLeft { get; }
        Vector2 TopRight { get; }
        Vector2 BottomRight { get; }
        int DynamicWorldId { get; }
        int LevelAreaId { get; }
        NodeFlags NodeFlags { get; set; }
        uint CustomFlags { get; set; }
        GridPoint GridPoint { get; set; }
    }
}
