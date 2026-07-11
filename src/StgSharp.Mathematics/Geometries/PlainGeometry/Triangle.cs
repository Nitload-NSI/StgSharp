// -----------------------------------------------------------------------------
// file="Triangle"
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
    public unsafe class Triangle : PlainGeometry
    {

        internal static int[] Indices = [
            0,
            1,
            2
        ];

        public Triangle()
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new Vec4[3];
        }

        public Triangle(
               Point vertex01,
               Point vertex02,
               Point vertex03
        )
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            this[0] = vertex01;
            this[1] = vertex02;
            this[2] = vertex03;
        }

        public Triangle(
               PlainCoordinate coordination,
               Point vertex01,
               Point vertex02,
               Point vertex03
        )
            : base(coordination)
        {
            this[0] = vertex01;
            this[1] = vertex02;
            this[2] = vertex03;
        }

        public Triangle(
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y
        )
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
        }

        public Triangle(
               PlainCoordinate coordination,
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y
        )
            : base(coordination)
        {
            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
