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
using StgSharp.Controlling;
using StgSharp.Math;

using System;
using System.Numerics;

namespace StgSharp.Geometries
{


    public unsafe class Triangle : PlainGeometry
    {

        internal Func<int,vec3d> movVertex01Operation = GeometryOperation.DefaultMotion;
        internal Func<int,vec3d> movVertex02Operation = GeometryOperation.DefaultMotion;
        internal Func<int,vec3d> movVertex03Operation = GeometryOperation.DefaultMotion;

        internal Mat3 vertexMat;

        public Triangle()
        {
            vertexMat = new Mat3();
        }

        public Triangle(
            vec3d vertex01,
            vec3d vertex02,
            vec3d vertex03
            )
        {
            vertexMat.colum0 = vertex01.vec;
            vertexMat.colum1 = vertex02.vec;
            vertexMat.colum2 = vertex03.vec;
        }

        public Triangle(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z
            )
        {
            vertexMat.colum0 = new Vector4(v0x, v0y, v0z, 0);
            vertexMat.colum1 = new Vector4(v1x, v1y, v1z, 0);
            vertexMat.colum2 = new Vector4(v2x, v2y, v2z, 0);
        }

        public Func<int,vec3d> MovVertex01Operation => this.movVertex01Operation;
        public Func<int,vec3d> MovVertex02Operation => this.movVertex02Operation;
        public Func<int,vec3d> MovVertex03Operation => this.movVertex03Operation;

        public Mat3 VertexBuffer => vertexMat;

        internal override sealed int[] Indices => new int[3] { 0, 1, 2 };

        public override Line[] GetAllSides()
        {
            return
                [
                    new Line(vertexMat.colum0,vertexMat.colum1),
                    new Line(vertexMat.colum1,vertexMat.colum2),
                    new Line(vertexMat.colum2,vertexMat.colum0)
                ];
        }

        public override Plain GetPlain()
        {
            return new Plain(this.vertexMat.colum0, this.vertexMat.colum1, this.vertexMat.colum2);
        }

        public virtual vec3d MovVertex01(int tick)
        {
            return movVertex01Operation(tick);
        }

        public virtual vec3d MovVertex02(int tick)
        {
            return movVertex02Operation(tick);
        }

        public virtual vec3d MovVertex03(int tick)
        {
            return movVertex03Operation(tick);
        }

        internal override sealed void UpdateCoordinate(int tick)
        {
            tick = time;
            vertexMat.colum0 = coordinate.Origin.vec + MovVertex01(time).vec;
            vertexMat.colum1 = coordinate.Origin.vec + MovVertex02(time).vec;
            vertexMat.colum2 = coordinate.Origin.vec + MovVertex03(time).vec;
        }

    }
}
