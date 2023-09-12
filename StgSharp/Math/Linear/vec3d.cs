using StgSharp.Controlling;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{

    [StructLayout(LayoutKind.Explicit,Size = 16,Pack = 16)]
    public struct Vec3d
    {
        [FieldOffset(0)]
        internal Vector4 vec;

        [FieldOffset(0)] public float X;
        [FieldOffset(4)] public float Y;
        [FieldOffset(8)] public float Z;

        public Vec3d(
            float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3d operator +(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X + vec2.X,
                vec1.Y + vec2.Y,
                vec1.Z + vec2.Z
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3d operator -(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X - vec2.X,
                vec1.Y - vec2.Y,
                vec1.Z - vec2.Z
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3d operator *(Vec3d vec, float value)
        {
            return new Vec3d(
                vec.X * value,
                vec.Y * value,
                vec.Z * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3d operator /(Vec3d vec, float value)
        {
            return new Vec3d(
                vec.X/value,
                vec.Y/value,
                vec.Z/value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float operator *(Vec3d vec1, Vec3d vec2)
        {
            return
                vec1.X * vec2.X +
                vec1.Y * vec2.Y +
                vec1.Z * vec2.Z;
        }

        public string ToString(string sample)
        {
            return $"{this.X}\t{this.Y}\t{this.Z}\t";
        }



    }
}
