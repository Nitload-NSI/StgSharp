namespace StgSharp.Math
{
    public static unsafe partial class Vector
    {
        public static float GetAngle(Vec2d vec1, Vec2d vec2)
        {
            return Calc.ACos(1
                //vec1.multiple(vec2)
                / (vec1.GetLength() * vec2.GetLength())
                );
        }

        public static float GetAngle(Vec3d vec1, Vec3d vec2)
        {
            return Calc.ACos(
                vec1 * vec2
                / (vec1.GetLength() * vec2.GetLength())
                );
        }

    }
}
