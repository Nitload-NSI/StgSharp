using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    /// <summary>
    /// 计算位移失量，需要手动输入游戏刻以提供时间数据
    /// </summary>
    /// <param name="tickCounter"></param>
    /// <returns>表示位移量的三维矢量</returns>
    public delegate vec3d GetLocationHandler(uint tick);

    public static unsafe class GeometryOperation
    {
        public static vec3d DefualtMotion(uint tick)
        {
            return default(vec3d);
        }
    }

}
