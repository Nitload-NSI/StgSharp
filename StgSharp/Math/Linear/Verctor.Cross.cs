namespace StgSharp.Math
{
    public static unsafe partial class Vector
    {

        public static float Cross(vec2d vec1, vec2d vec2)
        {
            return vec1.X * vec2.Y - vec1.Y * vec2.X;
        }

        public static vec3d Cross(vec3d vec1, vec3d vec2)
        {
            return new vec3d(
                vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                vec1.X * vec2.Z - vec1.Z * vec2.X,
                vec1.X * vec2.Y - vec1.Y * vec2.X
                );
        }
    }
}
