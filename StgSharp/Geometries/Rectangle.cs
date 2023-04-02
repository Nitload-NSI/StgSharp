using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class Rectangle : IGeometry
    {
        internal Point _basePoint;
        internal Point _center;
        internal Point _centerToCorner1;
        internal Point _centerToCorner2;
        internal RenderMode _renderMode;
        internal readonly uint bornTick;

        public override sealed Point BasePoint => _basePoint;
        public override sealed Point RefPoint=>_center;

        public override sealed Point RefVec01 => _centerToCorner1;

        public override sealed Point RefVec02 => _centerToCorner2;

        public override RenderMode renderMode => _renderMode;

        public Rectangle(uint bornTime) : base(bornTime)
        {
            bornTick = bornTime;
        }

        public Rectangle(Counter<uint> tickCounter) : base(tickCounter)
        {
            bornTick = tickCounter._value;
        }

        internal override sealed void OnRender(uint tick)
        {
            tick-=bornTick;
            CalcVec01(tick);
            switch (renderMode)
            {
                case RenderMode.BasePoint:
                    CalcVec02(_basePoint._position,tick);
                    CalcVec03(_basePoint._position,tick);
                    break;
                case RenderMode.RefPoint:
                    CalcVec02(_center._position, tick);
                    CalcVec03(_center._position, tick);
                    break;
                default:
                    break;
            }
        }

        public override sealed float SetArrayX()
        {
            throw new NotImplementedException();
        }

        public override sealed float SetArrayY()
        {
            throw new NotImplementedException();
        }

        internal override void CalcVec01(uint tick)
        {
            _center._position = _basePoint._position + MovCenter(tick);
        }

        internal override void CalcVec02(vec2d vec, uint tick)
        {
            _centerToCorner1._position = vec + UpdateCTC1(tick);
        }

        internal override void CalcVec03(vec2d vec, uint tick)
        {
            _centerToCorner2._position = vec + UpdateCTC2(tick);
            if (_centerToCorner1._position.GetLength() 
                == _centerToCorner2._position.GetLength())
            {
                throw new ArgumentException("Cannot form a triangle, " +
                    "two crossing lines are not equal!");
            }
        }

        public virtual vec2d MovCenter(uint tick)=>default(vec2d);

        public virtual vec2d UpdateCTC1(uint tick) => default(vec2d);

        public virtual vec2d UpdateCTC2(uint tick) => default(vec2d);
    }
}
