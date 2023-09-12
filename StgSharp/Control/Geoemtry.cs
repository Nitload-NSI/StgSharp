using StgSharp.Geometries;
using StgSharp.Math;

namespace StgSharp
{
    public static unsafe partial class Geometry
    {
        public static unsafe Triangle InitTriagnle<T>(
            Vec3d vertex1,
            Vec3d vertex2,
            Vec3d vertex3
            ) where T : Triangle
        {
            Triangle t = new Triangle();


            return t;
        }

    }
}
