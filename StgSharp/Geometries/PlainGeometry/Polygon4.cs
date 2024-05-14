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
using StgSharp.Controlling;
using StgSharp.Math;

using System;
using System.Numerics;

namespace StgSharp.Geometries
{
    public class Polygon4 : PlainGeometry
    {

        internal readonly Func<int,vec3d> moveVertex0Operation = GeometryOperation.DefaultMotion;
        internal readonly Func<int,vec3d> moveVertex1Operation = GeometryOperation.DefaultMotion;
        internal readonly Func<int,vec3d> moveVertex2Operation = GeometryOperation.DefaultMotion;
        internal readonly Func<int,vec3d> moveVertex3Operation = GeometryOperation.DefaultMotion;

        internal Mat4 vertexMat;

        internal Polygon4()
        {
        }

        public Polygon4(ICoord coordinate,
            vec2d topLeft,
            vec2d topRight,
            vec2d bottomRight,
            vec2d bottomLeft
            )
        {
            this.coordinate = coordinate;
            vertexMat.colum0 = topLeft.vec;
            vertexMat.colum1 = topRight.vec;
            vertexMat.colum2 = bottomRight.vec;
            vertexMat.colum3 = bottomLeft.vec;

            Vector4 maxCoord = vertexMat.colum0;
            Vector4 minCoord = vertexMat.colum0;

            maxCoord = Vector4.Max(maxCoord, vertexMat.colum1);
            maxCoord = Vector4.Max(maxCoord, vertexMat.colum2);
            maxCoord = Vector4.Max(maxCoord, vertexMat.colum3);

            minCoord = Vector4.Min(minCoord, vertexMat.colum1);
            minCoord = Vector4.Min(minCoord, vertexMat.colum2);
            minCoord = Vector4.Min(minCoord, vertexMat.colum3);
        }

        public Polygon4(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z
            )
        {
            vertexMat.colum0 = new Vector4(v0x, v0y, v0z, 0);
            vertexMat.colum1 = new Vector4(v1x, v1y, v1z, 0);
            vertexMat.colum2 = new Vector4(v2x, v2y, v2z, 0);
            vertexMat.colum3 = new Vector4(v3x, v3y, v3z, 0);
        }

        public Func<int,vec3d> MovVertex0Operation => moveVertex0Operation;
        public Func<int,vec3d> MovVertex1Operation => moveVertex1Operation;
        public Func<int,vec3d> MovVertex2Operation => moveVertex2Operation;
        public Func<int,vec3d> MovVertex3Operation => moveVertex3Operation;

        public Point Vertex0
        {
            get => new Point(vertexMat.colum0);
            set => vertexMat.colum0 = value.position.vec;
        }

        public Point Vertex1
        {
            get => new Point(vertexMat.colum1);
            set => vertexMat.colum1 = value.position.vec;
        }

        public Point Vertex2
        {
            get => new Point(vertexMat.colum2);
            set => vertexMat.colum1 = value.position.vec;
        }

        public Mat4 VertexBuffer => vertexMat;



        internal override sealed int[] Indices => new int[6] { 0, 1, 2, 0, 2, 3 };

        internal Point Vertex3
        {
            get => new Point(vertexMat.colum3);
            set => vertexMat.colum1 = value.position.vec;
        }


        public override Line[] GetAllSides()
        {
            Line[] sides =
                [
                    new Line(Vertex0, Vertex1),
                    new Line(Vertex1, Vertex2),
                    new Line(Vertex2, Vertex3),
                    new Line(Vertex3, Vertex0)
                ];

            return sides;
        }

        public override Plain GetPlain()
        {
            return new Plain(
                vertexMat.colum0,
                vertexMat.colum1,
                vertexMat.colum2);
        }

        public virtual vec3d MoveVertex0(int tick)
        {
            return moveVertex0Operation(tick);
        }

        public virtual vec3d MoveVertex1(int tick)
        {
            return moveVertex1Operation(tick);
        }

        public virtual vec3d MoveVertex2(int tick)
        {
            return moveVertex2Operation(tick);
        }

        public virtual vec3d MoveVertex3(int tick)
        {
            return moveVertex3Operation(tick);
        }

        internal override void UpdateCoordinate(int tick)
        {
        }

    }
}
