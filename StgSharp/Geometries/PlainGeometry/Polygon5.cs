//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Polygon5.cs"
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
    public unsafe class Polygon5 : PlainGeometry
    {

        internal Point _refOrigin;
        internal Func<int,vec3d> movP01Operation = default;
        internal Func<int,vec3d> movP02Operation = default;
        internal Func<int,vec3d> movP03Operation = default;
        internal Func<int,vec3d> movP04Operation = default;
        internal Func<int,vec3d> movP05Operation = default;
        internal mat5 vertexMat;

        public unsafe Polygon5()
        {
        }

        public Polygon5(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z,
            float v4x, float v4y, float v4z
            )
        {
            vertexMat.vec0 = new Vector4(v0x, v0y, v0z, 0);
            vertexMat.vec1 = new Vector4(v1x, v1y, v1z, 0);
            vertexMat.vec2 = new Vector4(v2x, v2y, v2z, 0);
            vertexMat.vec3 = new Vector4(v3x, v3y, v3z, 0);
            vertexMat.vec4 = new Vector4(v4x, v4y, v4z, 0);
        }


        public Func<int,vec3d> MovP01Operation => movP01Operation;
        public Func<int,vec3d> MovP02Operation => movP02Operation;
        public Func<int,vec3d> MovP03Operation => movP03Operation;
        public Func<int,vec3d> MovP04Operation => movP04Operation;
        public Func<int,vec3d> MovP05Operation => movP05Operation;

        public Point Vertex0
        {
            get => new Point(vertexMat.vec0);
            set => vertexMat.vec0 = value.position.vec;
        }

        public Point Vertex1
        {
            get => new Point(vertexMat.vec1);
            set => vertexMat.vec1 = value.position.vec;
        }

        public Point Vertex2
        {
            get => new Point(vertexMat.vec2);
            set => vertexMat.vec2 = value.position.vec;
        }

        public Point Vertex3
        {
            get => new Point(vertexMat.vec3);
            set => vertexMat.vec3 = value.position.vec;
        }

        public Point Vertex4
        {
            get => new Point(vertexMat.vec4);
            set => vertexMat.vec4 = value.position.vec;
        }

        public mat5 VertexBuffer => vertexMat;

        internal override int[] Indices => new int[9] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };

        public override Line[] GetAllSides()
        {
            return new Line[5]
                {
                    new Line(Vertex0,Vertex1),
                    new Line(Vertex1,Vertex2),
                    new Line(Vertex2,Vertex3),
                    new Line(Vertex3,Vertex4),
                    new Line(Vertex4,Vertex0)
                };
        }

        public override Plain GetPlain()
        {
            return new Plain(Vertex0, Vertex1, Vertex2);
        }

        public virtual vec3d movVertex01(int tick)
        {
            return movP01Operation.Invoke(time);
        }

        public virtual vec3d movVertex02(int tick)
        {
            return movP02Operation.Invoke(time);
        }

        public virtual vec3d movVertex03(int tick)
        {
            return movP03Operation.Invoke(time);
        }

        public virtual vec3d movVertex04(int tick)
        {
            return movP04Operation.Invoke(time);
        }

        public virtual vec3d movVertex05(int tick)
        {
            return movP05Operation.Invoke(time);
        }


        internal override void UpdateCoordinate(int tick)
        {
            int nowTick = tick - BornTime;
            vertexMat.vec0 = (coordinate.Origin + movVertex01(nowTick)).vec;
            vertexMat.vec1 = (coordinate.Origin + movVertex02(nowTick)).vec;
            vertexMat.vec2 = (coordinate.Origin + movVertex03(nowTick)).vec;
            vertexMat.vec3 = (coordinate.Origin + movVertex04(nowTick)).vec;
            vertexMat.vec4 = (coordinate.Origin + movVertex05(nowTick)).vec;
        }

    }
}
