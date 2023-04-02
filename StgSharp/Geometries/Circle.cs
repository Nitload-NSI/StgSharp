using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class Circle : IGeometry
    {
        internal Point _baseCenter;
        internal Point _center;
        internal Point _radiusVec;
        internal RenderMode _renderMode;
        internal readonly uint bornTick;

        public override Point BasePoint => _baseCenter;

        public override Point RefPoint => _center;

        public override Point RefVec01 => _radiusVec;

        public override Point RefVec02 => default;

        public override RenderMode renderMode => _renderMode;

        public Circle(uint bornTime) : base(bornTime)
        {
            bornTick = bornTime;
        }

        public Circle(Counter<uint> tickCounter) : base(tickCounter)
        {
            bornTick = tickCounter._value;
        }

        internal override void CalcVec01(uint tick)
        {
            _center._position = _baseCenter._position + MoveCenter(tick);
        }

        internal override void CalcVec02(vec2d vec, uint tick)
        {
            _radiusVec._position = vec + UpdateRadius(tick);
        }

        internal override void CalcVec03(vec2d vec, uint tick)
        {
            throw new NotImplementedException();
        }

        public virtual vec2d MoveCenter(uint tick) => default;

        public virtual vec2d UpdateRadius(uint tick) => default;

        

        public override float SetArrayX()
        {
            throw new NotImplementedException();
        }

        public override float SetArrayY()
        {
            throw new NotImplementedException();
        }

        internal override void OnRender(uint tick)
        {
            tick-=bornTick;
            CalcVec01(tick);
            switch (_renderMode)
            {
                case RenderMode.BasePoint:
                    CalcVec02(_baseCenter._position,tick);
                    CalcVec03(_baseCenter._position,tick);
                    break;
                case RenderMode.RefPoint:
                    CalcVec02(_center._position, tick);
                    CalcVec03(_center._position, tick);
                    break;
                default:
                    break;
            }
        }

    }
}
