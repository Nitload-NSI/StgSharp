using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{

    /// <summary>
    /// 平面几何体。包含三角形，对称四边形，自由多边形等。最多可由16个点参数定义。
    /// </summary>
    public abstract class IPlainGeometry:INode<IPlainGeometry>
    {
        protected readonly uint bornTick;
        internal readonly Point refOrigin;

        public Point RefOrigin { get { return refOrigin; } }
        public abstract Point RefPoint01{ get; }
        public abstract Point RefPoint02{ get; }
        public abstract Point RefPoint03{ get; }
        public uint BornTime => bornTick;

        public IPlainGeometry(uint bornTime)
        {
            bornTick=bornTime;
        }

        public IPlainGeometry(Counter<uint> MainTimer)
        {
            bornTick = MainTimer.Value;
        }

        public IPlainGeometry() { }


        internal abstract void OnRender(uint tick);

        public abstract Line[] GetAllSides();

        public abstract Plain GetPlain();

    }


}
