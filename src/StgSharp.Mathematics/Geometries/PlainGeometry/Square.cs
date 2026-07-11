// -----------------------------------------------------------------------------
// file="Square"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;

using System.Numerics;

namespace StgSharp.Geometries
{
    /// <summary>
    ///   A square
    /// </summary>
    public class Square : Rectangle
    {

        public Square(
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
            if (true) { }
        }

    }
}