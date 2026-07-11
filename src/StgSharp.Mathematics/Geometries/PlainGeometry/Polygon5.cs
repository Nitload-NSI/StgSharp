// -----------------------------------------------------------------------------
// file="Polygon5"
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
    public unsafe class Polygon5 : PlainGeometry
    {

        internal static int[] Indices = [
            0,
            1,
            2,
            0,
            2,
            3,
            0,
            3,
            4
        ];

        public unsafe Polygon5()
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new Vec4[5];
        }

        public Polygon5(
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y,
               float v3x,
               float v3y,
               float v4x,
               float v4y
        )
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new Vec4[5];
            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
            this[3] = new Point(v3x, v3y, 0);
            this[4] = new Point(v4x, v4y, 0);
        }

        public Polygon5(
               PlainCoordinate coordination,
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y,
               float v3x,
               float v3y,
               float v4x,
               float v4y
        )
            : base(coordination)
        {
            vertexMat = new Vec4[5];
            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
            this[3] = new Point(v3x, v3y, 0);
            this[4] = new Point(v4x, v4y, 0);
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
