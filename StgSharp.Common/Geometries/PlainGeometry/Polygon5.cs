//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Polygon5.cs"
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
using StgSharp.Mathematics;

using System;
using System.Numerics;

namespace StgSharp.Geometries
{
    public unsafe class Polygon5 : PlainGeometry
    {

        internal static int[] Indices = [0, 1, 2, 0, 2, 3, 0, 3, 4];

        public unsafe Polygon5()
            : base( PlainCoordinate.StandardPlainCoordination )
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
               float v4y )
            : base( PlainCoordinate.StandardPlainCoordination )
        {
            vertexMat = new Vec4[5];
            this[ 0 ] = new Point( v0x, v0y, 0 );
            this[ 1 ] = new Point( v1x, v1y, 0 );
            this[ 2 ] = new Point( v2x, v2y, 0 );
            this[ 3 ] = new Point( v3x, v3y, 0 );
            this[ 4 ] = new Point( v4x, v4y, 0 );
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
               float v4y )
            : base( coordination )
        {
            vertexMat = new Vec4[5];
            this[ 0 ] = new Point( v0x, v0y, 0 );
            this[ 1 ] = new Point( v1x, v1y, 0 );
            this[ 2 ] = new Point( v2x, v2y, 0 );
            this[ 3 ] = new Point( v3x, v3y, 0 );
            this[ 4 ] = new Point( v4x, v4y, 0 );
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
