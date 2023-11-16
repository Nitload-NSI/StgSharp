using StgSharp.Math;
using System.Numerics;

namespace StgSharp.Geometries
{

    /// <summary>
    /// A square
    /// </summary>
    public class Square : Rectangle
    {
        public Square(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z)
            : base(v0x, v0y, v0z,
                  v1x, v1y, v1z,
                  v2x, v2y, v2z,
                  v3x, v3y, v3z)
        {
        }

        public sealed override vec3d MoveVertex1(uint tick)
        {
            return this.center.Position * 2 - this.vertexMat.Colum0;
        }

    }
}