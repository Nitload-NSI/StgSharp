using StgSharp.Math;
using System;

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
            tick -= bornTick;
            center.Position = refOrigin.Position + MoveCenter(tick);
            beginPoint.Position = refOrigin.Position + MovBegin(tick);
            endPoint.Position = refOrigin.Position + MovEnd(tick);
        }

        public override Line[] GetAllSides() => throw new NotImplementedException();

        public override Plain GetPlain()
        {
            return new Plain(this.center, this.beginPoint, this.endPoint);
        }
    }
}
