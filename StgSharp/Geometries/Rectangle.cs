using StgSharp.Control;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class Rectangle : Polygon4
    {
        internal Point center;

        public override sealed Point RefPoint01=>center;

        public override sealed Point RefPoint02 => vertex01;

        public override sealed Point RefPoint03 => vertex02;

        

        /// <summary>
        /// 计算中心点相对于参考原点的位移
        /// </summary>
        /// <param name="tick">当前游戏刻</param>
        /// <returns></returns>
        public virtual vec3d MovCenter(uint tick) => default(vec3d);

        public sealed override vec3d MovVertex03(uint tick)
        {
            return this.center._position * 2 - this.vertex01._position;
        }

        public sealed override vec3d MovVertex04(uint tick)
        {
            return this.center._position * 2 - this.vertex02._position;
        }

        internal override sealed void OnRender(uint tick)
        {
            tick-=bornTick;
              this.MovCenter(tick);
        }

    }
}
