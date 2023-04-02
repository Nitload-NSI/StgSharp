using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Geometries
{
    public class Triangle :IGeometry
    {
        internal Point _basePoint;
        internal Point _beginPoint;
        internal Point _line1;
        internal Point _line2;
        internal RenderMode _renderMode;
        internal readonly uint bornTick;

        public override sealed Point BasePoint => _basePoint;

        public override sealed Point RefPoint=>_beginPoint;

        public override sealed Point RefVec01 => _line1;

        public override sealed Point RefVec02 => _line2;

        public override RenderMode renderMode => _renderMode;

        public Triangle(uint bornTime) : base(bornTime)
        {
            bornTick = bornTime;
        }

        public Triangle(Counter<uint> tickCounter) : base(tickCounter)
        {
            bornTick = tickCounter._value;
        }

        internal override void CalcVec01(uint tick)
        {
            _beginPoint._position = _basePoint._position+ MovBegin(tick);
        }

        internal override void CalcVec02(vec2d vec, uint tick)
        {
            _line1._position = vec + CalcSide1(tick);
        }

        internal override void CalcVec03(vec2d vec, uint tick)
        {
            _line2._position = vec + CalcSide2(tick);
        }

        public virtual vec2d MovBegin(uint tick) => default(vec2d);

        public virtual vec2d CalcSide1(uint tick)=> default(vec2d);

        public virtual vec2d CalcSide2(uint tick) => default(vec2d);

        internal override sealed void OnRender(uint tick)
        {
            CalcVec01(tick);
            switch (_renderMode)
            {
                case RenderMode.BasePoint:
                    CalcVec02(BasePoint._position,tick);
                    CalcVec03(BasePoint._position,tick);
                    break;
                case RenderMode.RefPoint:
                    CalcVec02(_beginPoint._position,tick);
                    CalcVec03(_beginPoint._position,tick);
                    break;
                default:
                    break;
            }
        }

        public override float SetArrayX()
        {
            throw new NotImplementedException();
        }

        public override float SetArrayY()
        {
            throw new NotImplementedException();
        }
    }
}
