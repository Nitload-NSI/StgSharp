//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Polygon4.cs"
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
    public class Polygon4 : PlainGeometry
    {
        protected static int[] Indices = [0, 1, 2, 0, 2, 3];

        internal Polygon4():base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new vec4d[4];
        }

        internal Polygon4(vec4d[] rawCoord):
            base(PlainCoordinate.StandardPlainCoordination)
        {
            if (rawCoord.Length != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(rawCoord));
            }
            vertexMat = rawCoord;
        }

        public Polygon4(
            Point topLeft,
            Point topRight,
            Point bottomRight,
            Point bottomLeft
            ):base(PlainCoordinate.StandardPlainCoordination)
        {
            this[0] = topLeft;
            this[1] = topRight;
            this[2] = bottomRight;
            this[3] = bottomLeft;

        }

        public Polygon4(
            float v0x, float v0y,
            float v1x, float v1y,
            float v2x, float v2y,
            float v3x, float v3y
            ):base(PlainCoordinate.StandardPlainCoordination)
        {
            vertexMat = new vec4d[4];

            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
            this[3] = new Point(v3x, v3y, 0);
        }


        public Polygon4(CoordinationBase coordinate,
            PlainCoordinate coordination,
            Point topLeft,
            Point topRight,
            Point bottomRight,
            Point bottomLeft
            ) : base(coordinate)
        {
            this[0] = topLeft;
            this[1] = topRight;
            this[2] = bottomRight;
            this[3] = bottomLeft;

        }

        public Polygon4(
            PlainCoordinate coordination,
            float v0x, float v0y,
            float v1x, float v1y,
            float v2x, float v2y,
            float v3x, float v3y
            ) : base(coordination)
        {
            vertexMat = new vec4d[4];

            this[0] = new Point(v0x, v0y, 0);
            this[1] = new Point(v1x, v1y, 0);
            this[2] = new Point(v2x, v2y, 0);
            this[3] = new Point(v3x, v3y, 0);
        }

        public override ReadOnlySpan<int> VertexIndices => Indices;

    }
}
