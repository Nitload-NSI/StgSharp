using StgSharp.Math;
using System.Numerics;

namespace StgSharp.Geometries
{
    public class Rectangle : Parallelogram
    {
        internal Point center;

        public Rectangle(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z
            ) : base(
                v0x, v0y, v0z,
                v1x, v1y, v1z,
                v2x, v2y, v2z,
                v3x, v2y, v3z
                )
        {
            vec3d side0 = vertexMat.Colum0 - vertexMat.Colum1;
            vec3d side1 = vertexMat.Colum2 - vertexMat.Colum1;
            if (side0.Cross(side1) == Vec3d.Zero)
            {

            }
        }



        /// <summary>
        /// 计算中心点相对于参考原点的位移
        /// </summary>
        /// <param name="tick">当前游戏刻</param>
        /// <returns></returns>
        public virtual vec3d MovCenter(uint tick) => default(vec3d);



    }
}
