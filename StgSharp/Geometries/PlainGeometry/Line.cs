//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Line.cs"
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
    public class Line : PlainGeometry
    {

        internal Point basePoint;
        internal Point beginPoint;
        internal Point endPoint;
        internal GetLocationHandler movBegin;
        internal GetLocationHandler movEnd;

        internal Line(Vector4 begin, Vector4 end)
        {
            beginPoint = new Point(begin);
            endPoint = new Point(end);
        }

        public Line() { }

        public Line(Point begin, Point end)
        {
            this.beginPoint = begin;
            this.endPoint = end;
        }


        internal override int[] Indices => throw new Exception();

        public override Line[] GetAllSides()
        {
            throw new NotImplementedException();
        }

        public override Plain GetPlain()
        {
            throw new ToUpperDimensionException();
        }

        /// <summary>
        /// Move begin side of a line.
        /// It is recommended to override this function rather than
        /// loading an <see cref="GetLocationHandler"/> with same function.
        /// </summary>
        /// <param name="tick">Time passed since current line was created.</param>
        public virtual vec3d MoveBeginPoint(int tick)
        {
            return movBegin.Invoke(time);
        }

        /// <summary>
        /// Move end side of a line.
        /// It is recommended to override this function rather than
        /// loading an <see cref="GetLocationHandler"/> with same function.
        /// </summary>
        /// <param name="tick">Time passed since current line was created.</param>
        /// <returns></returns>
        public virtual vec3d MoveEndPoint(int tick)
        {
            return movEnd.Invoke(time);
        }

        internal override void UpdateCoordinate(int tick)
        {
            beginPoint.Position = coordinate.origin + MoveBeginPoint(time);
            endPoint.Position = coordinate.origin + MoveEndPoint(time);
        }

    }
}
