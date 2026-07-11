// -----------------------------------------------------------------------------
// file="Polygon4"
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
    public class Polygon4 : PlainGeometry
    {

        protected static int[] Indices = [
            0,
            1,
            2,
            0,
            2,
            3
        ];

        internal Polygon4()
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new Vec4[4];
        }

        internal Polygon4(
                 Vec4[] rawCoord
        )
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            if (rawCoord.Length != 4) {
                throw new ArgumentOutOfRangeException(nameof(rawCoord));
            }
            vertexMat = rawCoord;
        }

        public Polygon4(
               Point topLeft,
               Point topRight,
               Point bottomRight,
               Point bottomLeft
        )
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            this[0] = topLeft;
            this[1] = topRight;
            this[2] = bottomRight;
            this[3] = bottomLeft;
        }

        public Polygon4(
               CoordinationBase coordinate,
               PlainCoordinate coordination,
               Point topLeft,
               Point topRight,
               Point bottomRight,
               Point bottomLeft
        )
            : base(coordinate)
        {
            this[0] = topLeft;
            this[1] = topRight;
            this[2] = bottomRight;
            this[3] = bottomLeft;
        }

        public Polygon4(
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y,
               float v3x,
               float v3y
        )
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new Vec4[4];

            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
            this[3] = new Point(v3x, v3y, 0);
        }

        public Polygon4(
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
            : base(coordination)
        {
            vertexMat = new Vec4[4];

            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
            this[3] = new Point(v3x, v3y, 0);
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
