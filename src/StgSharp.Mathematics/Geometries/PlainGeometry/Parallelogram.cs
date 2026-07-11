// -----------------------------------------------------------------------------
// file="Parallelogram"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;

namespace StgSharp.Geometries
{
    public class Parallelogram : Polygon4
    {

        internal readonly Func<int, Vec3> movCenterOperation = GeometryOperation.DefaultMotion;

        internal Point center;

        public Parallelogram(
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y,
               float v3x,
               float v3y
        )
            : base(v0x, v0y, v1x, v1y, v2x, v2y, v3x, v3y)
        {
            #if DEBUG
            if (this[0].Coord + this[2].Coord != this[1].Coord + this[3].Coord)
            {
                DefaultLog.InternalWriteLog(
                    "Init of geometry item failed, because four vertices cannot form a rectangle.",
                    LogType.Warning);
            }
            #endif
        }

        public Parallelogram(
               PlainCoordinate coordination,
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y,
               float v3x,
               float v3y
        )
            : base(coordination, v0x, v0y, v1x, v1y, v2x, v2y, v3x, v3y)
        {
            #if DEBUG
            if (this[0].Coord + this[2].Coord != this[1].Coord + this[3].Coord)
            {
                DefaultLog.InternalWriteLog(
                    "Init of geometry item failed, because four vertices cannot form a rectangle.",
                    LogType.Warning);
            }
            #endif
        }

    }
}
