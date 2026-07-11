// -----------------------------------------------------------------------------
// file="Polygon6"
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
            vertexMat = new Vec4[6];
        }

        public Polygon6(
               PlainCoordinate coordination,
               Vec2 vert0,
               Vec2 vert1,
               Vec2 vert2,
               Vec2 vert3,
               Vec2 vert4,
               Vec2 vert5,
               Vec2 vert6
        )
            : base(coordination)
        {
            vertexMat = new Vec4[6];
            vertexMat[0].reg = vert0.reg;
            vertexMat[1].reg = vert1.reg;
            vertexMat[2].reg = vert2.reg;
            vertexMat[3].reg = vert3.reg;
            vertexMat[4].reg = vert4.reg;
            vertexMat[5].reg = vert5.reg;
            vertexMat[6].reg = vert6.reg;
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}