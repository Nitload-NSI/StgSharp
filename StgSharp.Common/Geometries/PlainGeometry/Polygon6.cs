//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Polygon6.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------

using StgSharp.Math;

using System;
using System.Numerics;

namespace StgSharp.Geometries
{
    public class Polygon6 : PlainGeometry
    {

        internal static int[] Indices = [0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5];

        public Polygon6()
            : base( PlainCoordinate.StandardPlainCoordination )
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
            Vec2 vert6 )
            : base( coordination )
        {
            vertexMat = new Vec4[6];
            vertexMat[ 0 ].vec = vert0.vec;
            vertexMat[ 1 ].vec = vert1.vec;
            vertexMat[ 2 ].vec = vert2.vec;
            vertexMat[ 3 ].vec = vert3.vec;
            vertexMat[ 4 ].vec = vert4.vec;
            vertexMat[ 5 ].vec = vert5.vec;
            vertexMat[ 6 ].vec = vert6.vec;
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}