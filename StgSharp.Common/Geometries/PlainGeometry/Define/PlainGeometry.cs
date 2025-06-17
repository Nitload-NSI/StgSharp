//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PlainGeometry.cs"
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
using System.ComponentModel;

namespace StgSharp.Geometries
{
    /// <summary>
    ///   Plain geometry only contains straight sides.
    /// </summary>
    public abstract class PlainGeometry : IGeometry
    {

        protected Vec4[] vertexMat;
        internal CoordinationBase coordinate;

        internal PlainGeometry( CoordinationBase coordination )
        {
            coordinate = coordination;
        }

        public Point this[ int index ]
        {
            get => new Point( vertexMat[ index ] );
            set => vertexMat[ index ].vec = value.coordVec;
        }

        public int VertexCount => this.vertexMat.Length;

        public abstract ReadOnlySpan<int> VertexIndices { get; }

        #pragma warning disable CA1819 
        public Vec4[] VertexStream => vertexMat;

        public CoordinationBase Coordination => this.coordinate;
#pragma warning restore CA1819
    }
}