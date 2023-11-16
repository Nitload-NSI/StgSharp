using System.Numerics;
using System.Runtime.CompilerServices;

namespace StgSharp.Math
{
    public unsafe static partial class VectorCalc
    {
        public static vec3d To3D(Vec2d vec)
        {
            return new vec3d(vec.X, vec.Y, 0.0f);
        }

        public static Vec2d To2D(vec3d vec, CordinatePlain plain)
        {
            switch (plain)
            {
                case CordinatePlain.XY:
                    return new Vec2d(vec.X, vec.Y);
                    break;
                case CordinatePlain.YZ:
                    return new Vec2d(vec.Y, vec.Z);
                    break;
                case CordinatePlain.XZ:
                    return new Vec2d(vec.X, vec.Z);
                    break;
                default:
                    return default(Vec2d);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static vec3d To3D(Vector4 vector)
        {
            return new vec3d(vector);
        }
    }



}
