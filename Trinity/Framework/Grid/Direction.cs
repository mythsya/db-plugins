﻿using System;

namespace Trinity.Framework.Grid
{
    [Flags]
    public enum Direction
    {
        North = 0,
        NorthEast = 1 << 1,
        East = 1 << 2,
        SouthEast = 1 << 3,
        South = 1 << 4,
        SouthWest = 1 << 5,
        West = 1 << 6,
        NorthWest = 1 << 7,
    }
}
