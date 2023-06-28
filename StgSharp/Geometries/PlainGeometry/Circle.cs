using StgSharp.Math;
using System;

namespace StgSharp.Geometries
{
    public class Circle : IPlainGeometry
    {
        internal Point center;
        internal Point pointOnCircle;
        internal Point pointOnPlain;
        internal GetLocationHandler movCenterOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movBeginOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);
        internal GetLocationHandler movEndOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);


        public override Point RefPoint01 => center;

        public override Point RefPoint02 => pointOnCircle;

        public override Point RefPoint03 => pointOnPlain;

        public virtual vec3d MoveCenter(uint tick) => movCenterOperation.Invoke(tick);

        public virtual vec3d UpdateRadius(uint tick) => movBeginOperation.Invoke(tick);

        public virtual vec3d UpdatePlain(uint tick) => movEndOperation.Invoke(tick);

        public override Plain GetPlain()
        {
            return new Plain(center, pointOnCircle, pointOnPlain);
        }

        internal override void OnRender(uint tick)
        {
            tick -= bornTick;
            center.Position = refOrigin.Position + MoveCenter(tick);
            pointOnCircle.Position = refOrigin.Position + UpdateRadius(tick);
            pointOnPlain.Position = refOrigin.Position + UpdatePlain(tick);
        }

        public override Line[] GetAllSides()
        {
            throw new NotImplementedException();
        }
    }
}
