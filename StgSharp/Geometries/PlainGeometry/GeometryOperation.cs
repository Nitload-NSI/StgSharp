using StgSharp.Math;

namespace StgSharp.Geometries
{
    /// <summary>
    /// 计算位移失量，需要手动输入游戏刻以提供时间数据
    /// </summary>
    /// <param name="tick"></param>
    /// <returns>表示位移量的三维矢量</returns>
    public delegate Vec3d GetLocationHandler(uint tick);

    public static unsafe class GeometryOperation
    {
        public static Vec3d DefualtMotion(uint tick)
        {
            return default(Vec3d);
        }
    }

}
