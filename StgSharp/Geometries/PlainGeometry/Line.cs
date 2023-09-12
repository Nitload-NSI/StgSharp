using StgSharp.Math;
using System;

namespace StgSharp.Geometries
{
    public class Line : IPlainGeometry
    {
        internal Point basePoint = new Point();
        internal Point beginPoint = new Point();
        internal Point endPoint = new Point();
        internal GetLocationHandler movBegin;
        internal GetLocationHandler movEnd;

        public override Point RefPoint01 => beginPoint;
        public override Point RefPoint02 => endPoint;
        public override Point RefPoint03 => default;

        public Line(Point begin, Point end)
        {
            this.beginPoint = begin;
            this.endPoint = end;
        }

        public Line() { }

        //计算线起点的方法，通过对线的派生的重写实现功能
        public virtual Vec3d MoveBeginPoint(uint tick)
            => movBegin.Invoke(tick);

        //计算线重点的方法，通过对线的派生的重写实现功能
        public virtual Vec3d MoveEndPoint(uint tick)
            => movEnd.Invoke(tick);

        internal override void OnRender(uint tick)
        {
            tick -= bornTick;
            beginPoint.Position = refOrigin.Position + MoveBeginPoint(tick);
            endPoint.Position = refOrigin.Position + MoveEndPoint(tick);
        }

        public override Line[] GetAllSides()
        {
            throw new NotImplementedException();
        }

        public override Plain GetPlain()
        {
            throw new ToUpperDimensionException();
        }
    }
}
