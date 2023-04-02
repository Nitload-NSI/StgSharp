using Steamworks;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace StgSharp.Geometries
{
    public class Arc : IGeometry
    {
        internal Point _basePoint;
        internal Point _center;
        internal Point _radius;
        internal Point _angle;
        internal RenderMode _renderMode;
        internal readonly uint bornTick;

        public Arc(uint bornTime) : base(bornTime)
        {
            bornTick = bornTime;
        }

        public Arc(Counter<uint> tickCounter) : base(tickCounter)
        {
            bornTick = tickCounter._value;
        }

        public override Point BasePoint => _basePoint;

        public override Point RefPoint => _center;

        public override Point RefVec01 => _radius;

        public override Point RefVec02 => _angle;

        public override RenderMode renderMode => _renderMode;

        

        internal sealed override void CalcVec01(uint tick)
        {
            _center._position = _basePoint._position + MoveCenter(tick);
        }

        internal sealed override void CalcVec02(vec2d vec, uint tick)
        {
            _radius._position = vec + UpdateRadius(tick);

        }
        internal sealed override void CalcVec03(StgSharp.Math.vec2d vec, uint tick)
        {
            _angle._position = vec + UpdateAngle(tick);
        }

        public virtual vec2d MoveCenter(uint tick) => default;

        public virtual vec2d UpdateRadius(uint tick) => default;

        public virtual vec2d UpdateAngle(uint tick) => default;

        internal override void OnRender(uint tick)
        {
            tick-=bornTick;
            CalcVec01(tick);
            switch (_renderMode)
            {
                case RenderMode.BasePoint:
                    CalcVec02(_basePoint._position,tick);
                    CalcVec03(_basePoint._position,tick);
                    break;
                case RenderMode.RefPoint:
                    CalcVec02(_center._position,tick);
                    CalcVec03(_center._position, tick);
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
