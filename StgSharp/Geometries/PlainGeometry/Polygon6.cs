//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Polygon6.cs"
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
    public class Polygon6 : PlainGeometry
    {

        internal Point _refOrigin;
        internal GetLocationHandler movVertex0Operation;
        internal GetLocationHandler movVertex1Operation;
        internal GetLocationHandler movVertex2Operation;
        internal GetLocationHandler movVertex3Operation;
        internal GetLocationHandler movVertex4Operation;
        internal GetLocationHandler movVertex5Operation;
        internal mat6 vertexMat;

        public GetLocationHandler MovVertex0Operation => movVertex0Operation;
        public GetLocationHandler MovVertex1Operation => movVertex1Operation;
        public GetLocationHandler MovVertex2Operation => movVertex2Operation;
        public GetLocationHandler MovVertex3Operation => movVertex3Operation;
        public GetLocationHandler MovVertex4Operation => movVertex4Operation;
        public GetLocationHandler MovVertex5Operation => movVertex5Operation;

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

        public Point Vertex5
        {
            get => new Point(vertexMat.vec5);
            set => vertexMat.vec5 = value.position.vec;
        }

        internal override int[] Indices => new int[12] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };

        public override Line[] GetAllSides()
        {
            return new Line[6]
                {
                    new Line(Vertex0,Vertex1),
                    new Line(Vertex1,Vertex2),
                    new Line(Vertex2,Vertex3),
                    new Line(Vertex3,Vertex4),
                    new Line(Vertex4,Vertex5),
                    new Line(Vertex5,Vertex0)
                };
        }

        public override Plain GetPlain()
        {
            return new Plain(Vertex0, Vertex1, Vertex2);
        }

        public virtual vec3d movVertex0(int tick)
        {
            return movVertex0Operation.Invoke(time);
        }

        public virtual vec3d movVertex1(int tick)
        {
            return movVertex1Operation.Invoke(time);
        }

        public virtual vec3d movVertex2(int tick)
        {
            return movVertex2Operation.Invoke(time);
        }

        public virtual vec3d movVertex3(int tick)
        {
            return movVertex3Operation.Invoke(time);
        }

        public virtual vec3d movVertex4(int tick)
        {
            return movVertex4Operation.Invoke(time);
        }

        public virtual vec3d movVertex5(int tick)
        {
            return movVertex5Operation.Invoke(time);
        }

        internal override void UpdateCoordinate(int tick)
        {
            int nowTick = tick - BornTime;
            Vector4 origin = coordinate.origin.vec;
            vertexMat.vec0 = origin + movVertex0(nowTick).vec;
            vertexMat.vec1 = origin + movVertex1(nowTick).vec;
            vertexMat.vec2 = origin + movVertex2(nowTick).vec;
            vertexMat.vec3 = origin + movVertex3(nowTick).vec;
            vertexMat.vec4 = origin + movVertex4(nowTick).vec;
            vertexMat.vec5 = origin + movVertex5(nowTick).vec;
        }

    }
}