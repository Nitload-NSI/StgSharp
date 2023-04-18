using Steamworks;
using StgSharp;
using StgSharp.Control;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class Line:IPlainGeometry
    {
        internal Point basePoint = default ;
        internal Point beginPoint = default ;
        internal Point endPoint = default;
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
        public virtual vec3d MoveBeginPoint(uint tick) 
            => movBegin.Invoke(tick);

        //计算线重点的方法，通过对线的派生的重写实现功能
        public virtual vec3d MoveEndPoint(uint tick) 
            => movEnd.Invoke(tick);

        internal override void OnRender(uint tick)
        {
            tick -= bornTick;
            beginPoint._position = refOrigin._position + MoveBeginPoint(tick);
            endPoint._position = refOrigin._position + MoveEndPoint(tick);
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
