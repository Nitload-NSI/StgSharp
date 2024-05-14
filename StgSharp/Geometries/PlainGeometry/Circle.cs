//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Circle.cs"
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
    /// A collection of all point on the same plain and has the same distance to a point
    /// </summary>
    public class Circle : PlainGeometry
    {

        internal Point center;
        internal Func<int,vec3d> movBeginOperation =  GeometryOperation.DefaultMotion;
        internal Func<int,vec3d> movCenterOperation = GeometryOperation.DefaultMotion;
        internal Func<int,vec3d> movEndOperation =    GeometryOperation.DefaultMotion;
        internal Point pointOnCircle;
        internal Point pointOnPlain;


        internal override int[] Indices => throw new NotImplementedException();

        public override Line[] GetAllSides()
        {
            throw new NotImplementedException();
        }

        public override Plain GetPlain()
        {
            return new Plain(center, pointOnCircle, pointOnPlain);
        }

        public virtual vec3d MoveCenter(int tick)
        {
            return movCenterOperation.Invoke(tick);
        }

        public virtual vec3d UpdatePlain(int tick)
        {
            return movEndOperation.Invoke(tick);
        }

        public virtual vec3d UpdateRadius(int tick)
        {
            return movBeginOperation.Invoke(tick);
        }


        internal override void UpdateCoordinate(int tick)
        {
            tick = time;
            center.Position = this.coordinate.Origin + MoveCenter(tick);
            pointOnCircle.Position = this.coordinate.Origin + UpdateRadius(tick);
            pointOnPlain.Position = this.coordinate.Origin + UpdatePlain(tick);
        }

    }
}
