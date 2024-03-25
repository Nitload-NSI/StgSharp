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

using System.Numerics;

namespace StgSharp.Geometries
{


    public unsafe class Triangle : PlainGeometry
    {

        internal GetLocationHandler movVertex01Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movVertex02Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movVertex03Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);

        internal Matrix3x3 vertexMat;

        public Triangle()
        {
            vertexMat = new Matrix3x3();
        }

        public Triangle(
            vec3d vertex01,
            vec3d vertex02,
            vec3d vertex03
            )
        {
            vertexMat.mat.colum0 = vertex01.vec;
            vertexMat.mat.colum1 = vertex02.vec;
            vertexMat.mat.colum2 = vertex03.vec;
        }

        public Triangle(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z
            )
        {
            vertexMat.mat.colum0 = new Vector4(v0x, v0y, v0z, 0);
            vertexMat.mat.colum1 = new Vector4(v1x, v1y, v1z, 0);
            vertexMat.mat.colum2 = new Vector4(v2x, v2y, v2z, 0);
        }

        public GetLocationHandler MovVertex01Operation => this.movVertex01Opearion;
        public GetLocationHandler MovVertex02Operation => this.movVertex02Opearion;
        public GetLocationHandler MovVertex03Operation => this.movVertex03Opearion;

        public Mat3 VertexBuffer => vertexMat.mat;

        internal override sealed int[] Indices => new int[3] { 0, 1, 2 };

        public override Line[] GetAllSides()
        {
            Line[] sides = new Line[3]
                {
                    new Line(vertexMat.mat.colum0,vertexMat.mat.colum1),
                    new Line(vertexMat.mat.colum1,vertexMat.mat.colum2),
                    new Line(vertexMat.mat.colum2,vertexMat.mat.colum0)
                };

            return sides;
        }

        public override Plain GetPlain()
        {
            return new Plain(this.vertexMat.mat.colum0, this.vertexMat.mat.colum1, this.vertexMat.mat.colum2);
        }

        public virtual vec3d MovVertex01(int tick)
        {
            return movVertex01Opearion.Invoke(tick);
        }

        public virtual vec3d MovVertex02(int tick)
        {
            return movVertex02Opearion.Invoke(tick);
        }

        public virtual vec3d MovVertex03(int tick)
        {
            return movVertex03Opearion.Invoke(tick);
        }

        internal override sealed void UpdateCoordinate(int tick)
        {
            tick = time;
            vertexMat.mat.colum0 = coordinate.origin.vec + MovVertex01(time).vec;
            vertexMat.mat.colum1 = coordinate.origin.vec + MovVertex02(time).vec;
            vertexMat.mat.colum2 = coordinate.origin.vec + MovVertex03(time).vec;
        }

    }
}
