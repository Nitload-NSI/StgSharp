using StgSharp.Control;
using StgSharp.Math;

namespace StgSharp.Geometries
{


    public unsafe class Triangle : IPlainGeometry
    {

        internal Point vertex1 = new Point();
        internal Point vertex2 = new Point();
        internal Point vertex3 = new Point();
        internal GetLocationHandler movVertex01Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movVertex02Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movVertex03Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);

        public sealed override Point RefPoint01 => vertex1;
        public sealed override Point RefPoint02 => vertex2;
        public sealed override Point RefPoint03 => vertex3;

        public GetLocationHandler MovVertex01Operation => this.movVertex01Opearion;
        public GetLocationHandler MovVertex02Operation => this.movVertex02Opearion;
        public GetLocationHandler MovVertex03Operation => this.movVertex03Opearion;

        public virtual vec3d MovVertex01(uint tick) => movVertex01Opearion.Invoke(tick);

        public virtual vec3d MovVertex02(uint tick) => movVertex02Opearion.Invoke(tick);

        public virtual vec3d MovVertex03(uint tick) => movVertex03Opearion.Invoke(tick);

        public Triangle()
        {

            this.vertex1 = new Point();
            this.vertex2 = new Point();
            this.vertex3 = new Point();

            /**/
            TimeLine._currentPool._pointContainer.Add(vertex1);
            TimeLine._currentPool._pointContainer.Add(vertex2);
            TimeLine._currentPool._pointContainer.Add(vertex3);
            /**/


        }

        public Triangle(
            vec3d vertex01,
            vec3d vertex02,
            vec3d vertex03
            )
        {
            vertex1.Position = vertex01;
            vertex2.Position = vertex02;
            vertex3.Position = vertex03;
        }

        internal sealed override void OnRender(uint tick)
        {
            tick -= this.bornTick;
            vertex1.Position = RefOrigin.Position + MovVertex01(tick);
            vertex2.Position = RefOrigin.Position + MovVertex02(tick);
            vertex3.Position = RefOrigin.Position + MovVertex03(tick);
        }

        public override Line[] GetAllSides()
        {
            Line[] sides = new Line[3]
                {
                    new Line(vertex1,vertex2),
                    new Line(vertex2,vertex3),
                    new Line(vertex3,vertex1)
                };

            return sides;
        }

        public override Plain GetPlain()
        {
            return new Plain(this.vertex1, this.vertex2, this.vertex3);
        }

    }
}
