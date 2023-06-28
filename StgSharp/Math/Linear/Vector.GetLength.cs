namespace StgSharp.Math
{
    public static unsafe partial class Vector
    {
        public static float GetLength(this vec3d vec)
        {
            return Calc.Sqrt(
                vec.X * vec.X +
                vec.Y * vec.Y +
                vec.Z * vec.Z
                );
        }

        public static float GetLength(this vec2d vec)
        {
            return Calc.Sqrt(
                vec.X * vec.X +
                vec.Y * vec.Y
                );
        }

    }
}
