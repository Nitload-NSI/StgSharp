// -----------------------------------------------------------------------------
// file="Point"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Geometries
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Point
    {

        [FieldOffset(0)] private Vec3 coord;
        [FieldOffset(0)] internal Vector4 coordVec;

        internal Point(
                 Vec4 vector
        )
        {
            coordVec = vector.vec;
            coordVec.W = 1;
        }

        public Point(
               Vec3 coord
        )
        {
            Coord = coord;
            coordVec.W = 1;
        }

        public Point(
               float x,
               float y,
               float z
        )
        {
            coordVec = new Vector4(x, y, z, 1);
        }

        public Vec3 Coord
        {
            get => coord;
            set => coord = value;
        }

    }
}
