using StgSharp.Control;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    internal class Parallelogram : Polygon4
    {
        internal Point center;

        public override sealed Point RefPoint01 => center;
        public sealed override Point RefPoint02 => vertex01;
        public sealed override Point RefPoint03 => vertex02;

        internal readonly GetLocationHandler movCenterOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);

        public GetLocationHandler MovCenterOperation { get { return movCenterOperation; } }
        public GetLocationHandler MovVertex01Operation { get { return movVertex01Operation; } }
        public GetLocationHandler MovVertex02Operation { get { return movVertex02Operation; } }

        public virtual vec3d MovCenter(uint tick)
        {
            return movCenterOperation.Invoke(tick);
        }

        /// <summary>
        /// 移动平行四边形的第三个顶点，是一个多余的方法，调用会引发多余变量异常
        /// </summary>
        /// <param name="tick">当前游戏刻</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">几何体过定义</exception>
        public sealed override vec3d MovVertex03(uint tick)
        {
            throw new UnusedVertexException();
        }

        public sealed override vec3d MovVertex04(uint tick)
        {
            throw new UnusedVertexException();
        }

        internal override sealed void OnRender(uint tick)
        {
            uint nowTick = GameTimeLine.tickCounter._value;
            this.vertex01._position = RefOrigin._position + movVertex01Operation(nowTick);
            this.vertex02._position = RefOrigin._position + movVertex02Operation(nowTick);
            this.vertex03._position = 2 * center._position - vertex01._position;
            this.vertex04._position = 2 * center._position - vertex02._position;
        }

    }
}
