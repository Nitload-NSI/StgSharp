using StgSharp.Math;

namespace StgSharp.Geometries
{

    /// <summary>
    /// A square
    /// </summary>
    public class Square : Rectangle
    {
        public sealed override Vec3d MovVertex02(uint tick)
        {
            return this.center.Position * 2 - this.vertex01.Position;
        }

    }
}