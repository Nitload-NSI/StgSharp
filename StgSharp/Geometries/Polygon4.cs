using StgSharp.Control;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class Polygon4 : IPlainGeometry
    {
        internal Point vertex01;
        internal Point vertex02;
        internal Point vertex03;
        internal Point vertex04;

        internal readonly GetLocationHandler movVertex01Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal readonly GetLocationHandler movVertex02Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal readonly GetLocationHandler movVertex03Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal readonly GetLocationHandler movVertex04Operation = new GetLocationHandler(GeometryOperation.DefualtMotion);

        public override Point RefPoint01 => vertex01;

        public override Point RefPoint02 => vertex02;

        public override Point RefPoint03 => vertex03;

        public GetLocationHandler MovVertex01Operaion { get { return movVertex01Operation; } }
        public GetLocationHandler MovVertex02Operaion { get { return movVertex02Operation; } }
        public GetLocationHandler MovVertex03Operaion { get { return movVertex03Operation; } }
        public GetLocationHandler MovVertex04Operaion { get { return movVertex04Operation; } }

        public override Line[] GetAllSides()
        {
            Line[] sides = new Line[4]
                {
                    new Line(vertex01,vertex02),
                    new Line(vertex02, vertex03),
                    new Line(vertex03, vertex04),
                    new Line(vertex04, vertex01)
                };

            sides[0] = new Line(vertex01,vertex02);
            sides[1] = new Line(vertex02,vertex03);
            sides[2] = new Line(vertex03,vertex04);
            sides[3] = new Line(vertex04,vertex01);

            return sides;
        }

        public override Plain GetPlain()
        {
            return new Plain(this.RefPoint01, this.RefPoint02, this.RefPoint03);
        }

        
        public virtual vec3d MovVertex01(uint tick) => movVertex01Operation.Invoke(tick);

        public virtual vec3d MovVertex02(uint tick) => movVertex01Operation.Invoke(tick);

        public virtual vec3d MovVertex03(uint tick) => movVertex01Operation.Invoke(tick);

        public virtual vec3d MovVertex04(uint tick) => movVertex01Operation.Invoke(tick);


        internal override void OnRender(uint tick)
        {
            uint nowTick = GameTimeLine.tickCounter._value;
            this.vertex01._position = RefOrigin._position + movVertex01Operation(nowTick);
            this.vertex02._position = RefOrigin._position + movVertex02Operation(nowTick);
            this.vertex03._position = RefOrigin._position + movVertex03Operation(nowTick);
            this.vertex04._position = RefOrigin._position + movVertex04Operation(nowTick);
        }
    }
}
