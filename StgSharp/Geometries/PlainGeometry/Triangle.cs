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
        public sealed override Point RefPoint0 => new Point(vertexMat.mat.colum0);
        public sealed override Point RefPoint1 => new Point(vertexMat.mat.colum1);
        public sealed override Point RefPoint2 => new Point(vertexMat.mat.colum2);

        public Mat3 VertexBuffer
        {
            get { return vertexMat.mat; }
        }

        internal sealed override int[] Indices
        {
            get { return new int[3] { 0, 1, 2 }; }
        }

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

        public virtual vec3d MovVertex01(uint tick) => movVertex01Opearion.Invoke(tick);

        public virtual vec3d MovVertex02(uint tick) => movVertex02Opearion.Invoke(tick);

        public virtual vec3d MovVertex03(uint tick) => movVertex03Opearion.Invoke(tick);
        internal sealed override void OnRender(uint tick)
        {
            tick -= this.bornTick;
            vertexMat.mat.colum0 = RefOrigin.Position.vec + MovVertex01(tick).vec;
            vertexMat.mat.colum1 = RefOrigin.Position.vec + MovVertex02(tick).vec;
            vertexMat.mat.colum2 = RefOrigin.Position.vec + MovVertex03(tick).vec;
        }
    }
}
