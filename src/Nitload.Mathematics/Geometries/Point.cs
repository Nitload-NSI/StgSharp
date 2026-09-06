// -----------------------------------------------------------------------------
// file="Point"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Nitload.Geometries
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Point
    {

        [FieldOffset(0)] private Vec3<float> coord;

        public Point(
               Vec3<float> vector
        )
        {
            coord = vector;
        }

        public Point(
               float x,
               float y,
               float z
        )
        {
            coord = new Vec3<float>(x, y, z);
        }

        public Vec3<float> Coord
        {
            readonly get => coord;
            set => coord = value;
        }

    }
}
