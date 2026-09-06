// -----------------------------------------------------------------------------
// file="Polygon6"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;
using Nitload.Mathematics.Numeric.Graphics;
using System;
using System.Numerics;

namespace Nitload.Geometries
{
    public class Polygon6 : PlainGeometry
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
            4,
            0,
            4,
            5
        ];

        public Polygon6()
            : base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new Vec3<float>[6];
        }

        public Polygon6(
               PlainCoordinate coordination,
               Vec2<float> vert0,
               Vec2<float> vert1,
               Vec2<float> vert2,
               Vec2<float> vert3,
               Vec2<float> vert4,
               Vec2<float> vert5,
               Vec2<float> vert6
        )
            : base(coordination)
        {
            vertexMat = new Vec3<float>[6];
            vertexMat[0].XY = vert0;
            vertexMat[1].XY = vert1;
            vertexMat[2].XY = vert2;
            vertexMat[3].XY = vert3;
            vertexMat[4].XY = vert4;
            vertexMat[5].XY = vert5;
            vertexMat[6].XY = vert6;
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
