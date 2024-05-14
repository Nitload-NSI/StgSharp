//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Parallelogram.cs"
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
using StgSharp.Controlling;
using StgSharp.Math;

using System;

namespace StgSharp.Geometries
{
    public class Parallelogram : Polygon4
    {

        internal Point center;

        internal readonly Func<int,vec3d> movCenterOperation =
            new Func<int,vec3d>(GeometryOperation.DefaultMotion);

        public Parallelogram(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z
            ) : base(
                v0x, v0y, v0z,
                v1x, v1y, v1z,
                v2x, v2y, v2z,
                v3x, v3y, v3z
                )
        {
            if (
                vertexMat.colum0 + vertexMat.colum2 !=
                vertexMat.colum1 + vertexMat.colum3
                )
            {
                InternalIO.InternalWriteLog("Init of geometry item failed, because four vertices cannot form a rectangle.", LogType.Warning);
            }
        }

        public Func<int,vec3d> MovCenterOperation => movCenterOperation;
        public Func<int,vec3d> MovVertex01Operation => moveVertex0Operation;
        public Func<int,vec3d> MovVertex02Operation => moveVertex1Operation;

        public virtual vec3d MovCenter(int tick)
        {
            return movCenterOperation.Invoke(tick);
        }

        /// <summary>
        /// Move the third vertex. this is a useless method for Parallelograms.
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">Geometry over defined</exception>
        public override sealed vec3d MoveVertex2(int tick)
        {
            throw new UnusedVertexException();
        }

        /// <summary>
        /// Move the third vertex. This method is useless for Parallelograms.
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">Geometry over defined</exception>
        public override sealed vec3d MoveVertex3(int tick)
        {
            throw new UnusedVertexException();
        }

        internal override sealed void UpdateCoordinate(int tick)
        {
        }

    }
}
