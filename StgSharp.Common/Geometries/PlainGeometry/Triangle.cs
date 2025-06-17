//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Triangle.cs"
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
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Numerics;

namespace StgSharp.Geometries
{
    public unsafe class Triangle : PlainGeometry
    {

        internal static int[] Indices = [0, 1, 2];

        public Triangle()
            : base( PlainCoordinate.StandardPlainCoordination )
        {
            vertexMat = new Vec4[3];
        }

        public Triangle( Point vertex01, Point vertex02, Point vertex03 )
            : base( PlainCoordinate.StandardPlainCoordination )
        {
            this[ 0 ] = vertex01;
            this[ 1 ] = vertex02;
            this[ 2 ] = vertex03;
        }

        public Triangle(
               PlainCoordinate coordination,
               Point vertex01,
               Point vertex02,
               Point vertex03 )
            : base( coordination )
        {
            this[ 0 ] = vertex01;
            this[ 1 ] = vertex02;
            this[ 2 ] = vertex03;
        }

        public Triangle( float v0x, float v0y, float v1x, float v1y, float v2x, float v2y )
            : base( PlainCoordinate.StandardPlainCoordination )
        {
            this[ 0 ] = new Point( v0x, v0y, 0 );
            this[ 1 ] = new Point( v1x, v1y, 0 );
            this[ 2 ] = new Point( v2x, v2y, 0 );
        }

        public Triangle(
               PlainCoordinate coordination,
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y )
            : base( coordination )
        {
            this[ 0 ] = new Point( v0x, v0y, 0 );
            this[ 1 ] = new Point( v1x, v1y, 0 );
            this[ 2 ] = new Point( v2x, v2y, 0 );
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
