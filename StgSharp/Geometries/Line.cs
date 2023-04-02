using Steamworks;
using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class Line:IGeometry
    {
        internal Point _basePoint;
        internal Point _beginPoint;
        internal Point _endPoint;
        internal GetLocationHandler _beginPointMov;
        internal GetLocationHandler _endPointMov;
        internal RenderMode _renderMode;
        internal readonly uint bornTick;

        public override Point BasePoint => _basePoint;

        public override Point RefPoint => _beginPoint;

        public override Point RefVec01 => _endPoint;
        public override Point RefVec02 => default;
        public override RenderMode renderMode => _renderMode;

        public Line(uint bornTime) : base(bornTime)
        {
            bornTick = bornTime;
        }

        public Line(Counter<uint> tickCounter) : base(tickCounter)
        {
            bornTick = tickCounter._value;
        }

        public override float SetArrayX()
        {
            throw new NotImplementedException();
        }

        public override float SetArrayY()
        {
            throw new NotImplementedException();
        }

        internal sealed override void CalcVec01(uint tick)
        {
            _beginPoint._position = _basePoint._position + MoveBeginPoint(tick);
        }

        internal sealed override void CalcVec02(vec2d vec, uint tick)
        {
            _endPoint._position = vec + MoveEndPoint(tick);
        }

        internal sealed override void CalcVec03(vec2d vec, uint tick)
        {
            throw new ArgumentException("Lines do not need the third parameter to define!");
        }

        public virtual vec2d MoveBeginPoint(uint tick) => default;

        public virtual vec2d MoveEndPoint(uint tick) => default;

        internal override void OnRender(uint tick)
        {
            tick -= bornTick;
            CalcVec01(tick);
            switch (_renderMode)
            {
                case RenderMode.BasePoint:
                    CalcVec02(_basePoint._position, tick);
                    break;
                case RenderMode.RefPoint:
                    CalcVec02(_beginPoint._position, tick);
                    break;
                default:
                    break;
            }
        }
    }
}
