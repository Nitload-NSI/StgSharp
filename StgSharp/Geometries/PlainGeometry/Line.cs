using StgSharp.Math;
using System;
using System.Numerics;

namespace StgSharp.Geometries
{
    public class Line : PlainGeometry
    {
        internal Point basePoint;
        internal Point beginPoint;
        internal Point endPoint;
        internal GetLocationHandler movBegin;
        internal GetLocationHandler movEnd;

        public override Point RefPoint0 => beginPoint;
        public override Point RefPoint1 => endPoint;
        public override Point RefPoint2 => default;

        internal override int[] Indices
        {
            get { throw new Exception(); }
        }

        public Line(Point begin, Point end)
        {
            this.beginPoint = begin;
            this.endPoint = end;
        }

        internal Line(Vector4 begin, Vector4 end)
        {
            beginPoint = new Point(begin);
            endPoint = new Point(end);
        }

        public Line() { }

        //计算线起点的方法，通过对线的派生的重写实现功能
        public virtual vec3d MoveBeginPoint(uint tick)
            => movBegin.Invoke(tick);

        //计算线重点的方法，通过对线的派生的重写实现功能
        public virtual vec3d MoveEndPoint(uint tick)
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
