// -----------------------------------------------------------------------------
// file="Rectangle"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;
using System.Numerics;

namespace StgSharp.Geometries
{
    public class Rectangle : Parallelogram
    {

        public Rectangle(
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
            Vec2 side0 = (this[0].Coord - this[1].Coord).XY;
            Vec2 side1 = (this[2].Coord - this[1].Coord).XY;
            if (side0.Cross(side1) == 0) {
                throw new ArgumentException();
            }
            #endif
        }

        public Rectangle(
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
            Vec2 side0 = (this[0].Coord - this[1].Coord).XY;
            Vec2 side1 = (this[2].Coord - this[1].Coord).XY;
            if (side0.Cross(side1) == 0) {
                throw new ArgumentException();
            }
            #endif
        }

        /// <summary>
        ///   Calculate movement to center
        /// </summary>
        /// <param _label="tick">
        ///   CurrentPipeline time tick
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public virtual Vec3 MovCenter(
                            uint tick
        )
        {
            return default(Vec3);
        }

    }
}
