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

        internal Matrix3x4 vertexMat;

        internal Polygon4()
        {
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

        public GetLocationHandler MovVertex0Operaion { get { return moveVertex0Operation; } }
        public GetLocationHandler MovVertex1Operaion { get { return moveVertex1Operation; } }
        public GetLocationHandler MovVertex2Operaion { get { return moveVertex2Operation; } }
        public GetLocationHandler MovVertex3Operaion { get { return moveVertex3Operation; } }

        public override Point RefPoint0
        {
            get { return new Point(vertexMat.mat.colum0); }
        }

        public override Point RefPoint1
        {
            get { return new Point(vertexMat.mat.colum1); }
        }

        public override Point RefPoint2
        {
            get { return new Point(vertexMat.mat.colum2); }
        }

        internal sealed override int[] Indices
        {
            get
            {
                return new int[6] {
                    0,1,2,
                    0,2,3};
            }
        }

        public Mat4 VertexBuffer
        {
            get { return vertexMat.mat; }
        }

        public Point Vertex0
        {
            get => new Point(vertexMat.mat.colum0);
            set { vertexMat.mat.colum0 = value.position.vec; }
        }

        public Point Vertex1
        {
            get => new Point(vertexMat.mat.colum1);
            set { vertexMat.mat.colum1 = value.position.vec; }
        }

        public Point Vertex2
        {
            get => new Point(vertexMat.mat.colum2);
            set { vertexMat.mat.colum1 = value.position.vec; }
        }

        internal Point Vertex3
        {
            get => new Point(vertexMat.mat.colum3);
            set { vertexMat.mat.colum1 = value.position.vec; }
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

        public virtual vec3d MoveVertex0(uint tick)
            => moveVertex0Operation.Invoke(tick);

        public virtual vec3d MoveVertex1(uint tick)
            => moveVertex1Operation.Invoke(tick);

        public virtual vec3d MoveVertex2(uint tick)
            => moveVertex2Operation.Invoke(tick);

        public virtual vec3d MoveVertex3(uint tick)
            => moveVertex3Operation.Invoke(tick);


        internal override void OnRender(uint tick)
        {
            uint nowTick = TimeLine.tickCounter._value;
            this.vertexMat.Colum0 = RefOrigin.position + moveVertex1Operation(nowTick);
            this.vertexMat.Colum1 = RefOrigin.position + moveVertex1Operation(nowTick);
            this.vertexMat.Colum2 = RefOrigin.position + moveVertex2Operation(nowTick);
            this.vertexMat.Colum3 = RefOrigin.position + moveVertex3Operation(nowTick);
        }
    }
}
