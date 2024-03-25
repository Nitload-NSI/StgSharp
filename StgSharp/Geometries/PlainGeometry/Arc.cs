//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Arc.cs"
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

namespace StgSharp.Geometries
{
    /// <summary>
    /// Part of a circle. All points on a arc has the same distance to their center.
    /// </summary>
    public class Arc : PlainGeometry
    {

        internal Point beginPoint;
        internal Point center;
        internal Point endPoint;
        internal GetLocationHandler movBeginOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movCenterOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movEndOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);

        internal override int[] Indices => throw new NotImplementedException();

        public override Line[] GetAllSides()
        {
            throw new NotImplementedException();
        }

        public override Plain GetPlain()
        {
            return new Plain(this.center, this.beginPoint, this.endPoint);
        }

        public virtual vec3d MovBegin(int tick)
        {
            return movBeginOperation.Invoke(tick);
        }

        public virtual vec3d MoveCenter(int tick)
        {
            return movCenterOperation.Invoke(tick);
        }

        public virtual vec3d MovEnd(int tick)
        {
            return movEndOperation.Invoke(tick);
        }

        internal override void UpdateCoordinate(int tick)
        {
            tick = time;
            center.Position = this.coordinate.origin + MoveCenter(tick);
            beginPoint.Position = this.coordinate.origin + MovBegin(tick);
            endPoint.Position = this.coordinate.origin + MovEnd(tick);
        }

    }
}
