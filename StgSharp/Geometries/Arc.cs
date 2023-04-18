using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Geometries
{
    public class Arc : IPlainGeometry
    {
        internal Point center;
        internal Point beginPoint;
        internal Point endPoint;
        internal GetLocationHandler movCenterOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movBeginOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movEndOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);

        public override Point RefPoint01 => center;

        public override Point RefPoint02 => beginPoint;

        public override Point RefPoint03 => endPoint;

        public virtual vec3d MoveCenter(uint tick) => movCenterOperation.Invoke(tick);

        public virtual vec3d MovBegin(uint tick) => movBeginOperation.Invoke(tick);

        public virtual vec3d MovEnd(uint tick) => movEndOperation.Invoke(tick);

        internal override void OnRender(uint tick)
        {
            tick-= bornTick;
            center._position = refOrigin._position + MoveCenter(tick);
            beginPoint._position = refOrigin._position + MovBegin(tick);
            endPoint._position = refOrigin._position + MovEnd(tick);
        }

        public override Line[] GetAllSides() => throw new NotImplementedException();

        public override Plain GetPlain()
        {
            return new Plain(this.center,this.beginPoint,this.endPoint);
        }
    }
}
