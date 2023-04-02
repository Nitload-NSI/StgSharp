using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public abstract class IGeometry:INode<IGeometry>
    {
        internal ContainerNode<IGeometry> _id;
        private readonly uint bornTick;

        public abstract Point BasePoint { get; }
        public abstract Point RefPoint{ get; }
        public abstract Point RefVec01{ get; }
        public abstract Point RefVec02{ get; }
        public uint BornTime => bornTick;

        public IGeometry(uint bornTime)
        {
            bornTick=bornTime;
        }

        public IGeometry(Counter<uint> MainTimer)
        {
            bornTick = MainTimer.Value;
        }

        internal abstract void CalcVec01(uint tick);
        internal abstract void CalcVec02(vec2d vec, uint tick);
        internal abstract void CalcVec03(vec2d vec, uint tick);
        internal abstract void OnRender(uint tick);
        
        public abstract RenderMode renderMode { get; }
        public abstract float SetArrayX();
        public abstract float SetArrayY();

        ContainerNode<IGeometry> INode<IGeometry>.ID => _id;

        void INode<IGeometry>.setID(ContainerNode<IGeometry> id)
        {
            if (_id != default)
            {
                throw new ArgumentException("Cannot set id twice");
            }
            else
            {
                _id = id;
            }
        }
    }


    public enum RenderMode
    {
        /// <summary>
        /// 坐标点将基于参考原点构建
        /// </summary>
        BasePoint,
        /// <summary>
        /// 坐标点将基于参考起点构建
        /// </summary>
        RefPoint
    }


}
