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

using System.Numerics;

namespace StgSharp.Geometries
{
    public class Polygon4 : PlainGeometry
    {

        internal readonly GetLocationHandler moveVertex0Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal readonly GetLocationHandler moveVertex1Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal readonly GetLocationHandler moveVertex2Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal readonly GetLocationHandler moveVertex3Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);

        internal Matrix24 vertexMat;

        internal Polygon4()
        {
        }

        public Polygon4(PartialPlainCoordinate coordinate,
            vec2d topLeft,
            vec2d topRight,
            vec2d bottomRight,
            vec2d bottomLeft,
            bool Orth
            )
        {
            this.coordinate = coordinate;
            vertexMat.Colum0 = topLeft;
            vertexMat.Colum1 = topRight;
            vertexMat.Colum2 = bottomRight;
            vertexMat.Colum3 = bottomLeft;

            Vector4 maxCoord = vertexMat.mat.colum0;
            Vector4 minCoord = vertexMat.mat.colum0;

            maxCoord = Vector4.Max(maxCoord, vertexMat.mat.colum1);
            maxCoord = Vector4.Max(maxCoord, vertexMat.mat.colum2);
            maxCoord = Vector4.Max(maxCoord, vertexMat.mat.colum3);

            minCoord = Vector4.Min(minCoord, vertexMat.mat.colum1);
            minCoord = Vector4.Min(minCoord, vertexMat.mat.colum2);
            minCoord = Vector4.Min(minCoord, vertexMat.mat.colum3);

            textureFrame.mat.colum1 = maxCoord;
            textureFrame.mat.colum3 = minCoord;
            textureFrame.Colum0 = new vec2d(minCoord.X, maxCoord.Y);
            textureFrame.Colum2 = new vec2d(maxCoord.X, minCoord.Y);
        }

        public Polygon4(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z
            )
        {
            vertexMat.mat.colum0 = new Vector4(v0x, v0y, v0z, 0);
            vertexMat.mat.colum1 = new Vector4(v1x, v1y, v1z, 0);
            vertexMat.mat.colum2 = new Vector4(v2x, v2y, v2z, 0);
            vertexMat.mat.colum3 = new Vector4(v3x, v3y, v3z, 0);
        }

        public GetLocationHandler MovVertex0Operation => moveVertex0Operation;
        public GetLocationHandler MovVertex1Operation => moveVertex1Operation;
        public GetLocationHandler MovVertex2Operation => moveVertex2Operation;
        public GetLocationHandler MovVertex3Operation => moveVertex3Operation;

        public Point Vertex0
        {
            get => new Point(vertexMat.mat.colum0);
            set => vertexMat.mat.colum0 = value.position.vec;
        }

        public Point Vertex1
        {
            get => new Point(vertexMat.mat.colum1);
            set => vertexMat.mat.colum1 = value.position.vec;
        }

        public Point Vertex2
        {
            get => new Point(vertexMat.mat.colum2);
            set => vertexMat.mat.colum1 = value.position.vec;
        }

        public Mat4 VertexBuffer => vertexMat.mat;



        internal override sealed int[] Indices => new int[6] { 0, 1, 2, 0, 2, 3 };

        internal Point Vertex3
        {
            get => new Point(vertexMat.mat.colum3);
            set => vertexMat.mat.colum1 = value.position.vec;
        }


        public override Line[] GetAllSides()
        {
            Line[] sides = new Line[4]
                {
                    new Line(Vertex0, Vertex1),
                    new Line(Vertex1, Vertex2),
                    new Line(Vertex2, Vertex3),
                    new Line(Vertex3, Vertex0)
                };

            return sides;
        }

        public override Plain GetPlain()
        {
            vertexMat.InternalTranspose();
            return new Plain(
                vertexMat.mat.colum0,
                vertexMat.mat.colum1,
                vertexMat.mat.colum2);
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
