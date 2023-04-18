using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Geometries
{
    public class Triangle :IPlainGeometry
    {
        internal Point vertex1;
        internal Point vertex2;
        internal Point vertex3;
        internal GetLocationHandler movVertex01Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movVertex02Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movVertex03Opearion = new GetLocationHandler(GeometryOperation.DefualtMotion);


        public override sealed Point RefPoint01=> vertex1;

        public override sealed Point RefPoint02 => vertex2;

        public override sealed Point RefPoint03 => vertex3;

        public GetLocationHandler MovVertex01Operation => this.movVertex01Opearion;
        public GetLocationHandler MovVertex02Operation => this.movVertex02Opearion;
        public GetLocationHandler MovVertex03Operation => this.movVertex03Opearion;

        public virtual vec3d MovVertex01(uint tick) => movVertex01Opearion.Invoke(tick);

        public virtual vec3d MovVertex02(uint tick) => movVertex02Opearion.Invoke(tick);

        public virtual vec3d MovVertex03(uint tick) => movVertex03Opearion.Invoke(tick);

        internal override sealed void OnRender(uint tick)
        {
            tick -= this.bornTick;
            vertex1._position = RefOrigin._position + MovVertex01(tick);
            vertex2._position = RefOrigin._position + MovVertex02(tick);
            vertex3._position = RefOrigin._position + MovVertex03(tick);

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
            return new Plain(this.vertex1,this.vertex2,this.vertex3);
        }

    }
}
