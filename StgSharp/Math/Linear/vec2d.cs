using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{

    /// <summary>
    /// A two dimension vector defined by two elements.
    /// Vec2Ds in StgSharp are defaultly usaed as colum vector. 
    /// </summary>

    [StructLayout(LayoutKind.Explicit, Size = 16,Pack = 16)]
    public struct Vec2d
    {

        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;

        [FieldOffset(0)]
        internal Vector4 vec;

        public Vec2d(float x, float y)
        {
            X = x; Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2d operator +(Vec2d vec1, Vec2d vec2)
        {
            return new Vec2d(
                vec1.X + vec2.X,
                vec1.Y + vec2.Y
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2d operator -(Vec2d vec1, Vec2d vec2)
        {
            return new Vec2d(
                vec1.X - vec2.X,
                vec1.Y - vec2.Y
                );
        }

        public static Vec2d operator *(Vec2d vec, float value)
        {
            return new Vec2d(
                vec.X * value,
                vec.Y * value
                );
        }

        public static Vec2d operator /(Vec2d vec, float value)
        {
            return new Vec2d(
                vec.X / value,
                vec.Y / value
                );
        }



    }
}
