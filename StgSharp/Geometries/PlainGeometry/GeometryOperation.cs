using StgSharp.Math;

namespace StgSharp.Geometries
{
    /// <summary>
    /// Caculate the movement of a point.
    /// </summary>
    /// <param name="tick">The game tick represent time passed by</param>
    /// <returns>A 3d vector representing movement</returns>
    public delegate vec3d GetLocationHandler(uint tick);

    public static unsafe class GeometryOperation
    {
        public static vec3d DefualtMotion(uint tick)
        {
            return default(vec3d);
        }
    }

}
