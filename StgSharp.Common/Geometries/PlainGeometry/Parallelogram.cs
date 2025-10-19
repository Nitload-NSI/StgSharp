//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Parallelogram"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

using System;

namespace StgSharp.Geometries
{
    public class Parallelogram : Polygon4
    {

        internal readonly Func<int, Vec3> movCenterOperation =
            new Func<int, Vec3>(GeometryOperation.DefaultMotion);

        internal Point center;

        public Parallelogram(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y, float v3x, float v3y)
            : base(v0x, v0y, v1x, v1y, v2x, v2y, v3x, v3y)
        {
            #if DEBUG
            if (this[0].Coord + this[2].Coord != this[1].Coord + this[3].Coord)
            {
                InternalIO.InternalWriteLog(
                    "Init of geometry item failed, because four vertices cannot form a rectangle.",
                    LogType.Warning);
            }
            #endif
        }

        public Parallelogram(
               PlainCoordinate coordination,
               float v0x,
               float v0y,
               float v1x,
               float v1y,
               float v2x,
               float v2y,
               float v3x,
               float v3y)
            : base(coordination, v0x, v0y, v1x, v1y, v2x, v2y, v3x, v3y)
        {
            #if DEBUG
            if (this[0].Coord + this[2].Coord != this[1].Coord + this[3].Coord)
            {
                InternalIO.InternalWriteLog(
                    "Init of geometry item failed, because four vertices cannot form a rectangle.",
                    LogType.Warning);
            }
            #endif
        }

    }
}
