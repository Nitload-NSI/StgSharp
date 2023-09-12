namespace StgSharp.Math
{
    public unsafe static partial class Vector
    {
        public static Vec3d To3D(Vec2d vec)
        {
            return new Vec3d(vec.X, vec.Y, 0.0f);
        }

        public static Vec2d To2D(Vec3d vec, CordinatePlain plain)
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

    }



}
