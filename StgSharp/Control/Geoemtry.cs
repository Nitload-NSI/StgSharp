using StgSharp.Geometries;
using StgSharp.Math;

namespace StgSharp
{
    public static unsafe partial class Geometry
    {
        public static unsafe Triangle InitTriagnle<T>(
            vec3d vertex1,
            vec3d vertex2,
            vec3d vertex3
            ) where T : Triangle
        {
            Triangle t = new Triangle();


            return t;
        }

    }
}
