using StgSharp.Controlling;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    public static class Vec3d
    {
        public static vec3d Zero => new vec3d(0, 0, 0);
        public static vec3d Unit => new vec3d(1, 1, 1);
    }

    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct vec3d : IEquatable<vec3d>
    {
        [FieldOffset(0)]
        internal Vector4 vec;

        [FieldOffset(0)] public float X;
        [FieldOffset(4)] public float Y;
        [FieldOffset(8)] public float Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public vec3d Zero()
        {
            return new vec3d(0, 0, 0);
        }

        public vec3d(
            float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(vec3d right)
        {
            Vector4 vector4 = this.vec * right.vec;
            return vector4.X
                + vector4.Y
                + vector4.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public vec3d Cross(vec3d right)
        {
            return new vec3d(
                this.Y * right.Z - this.Z * right.Y,
                this.X * right.Z - this.Z * right.X,
                this.X * right.Y - this.Y * right.X
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal vec3d(Vector4 vector)
        {
            vec = vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator +(vec3d left, vec3d right)
        {
            return new vec3d(
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator -(vec3d left, vec3d right)
        {
            return new vec3d(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator *(vec3d vec, float value)
        {
            return new vec3d(
                vec.X * value,
                vec.Y * value,
                vec.Z * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator /(vec3d vec, float value)
        {
            return new vec3d(
                vec.X / value,
                vec.Y / value,
                vec.Z / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float operator *(vec3d left, vec3d right)
        {
            return
                left.X * right.X +
                left.Y * right.Y +
                left.Z * right.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(vec3d left, vec3d right)
        {
            return left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(vec3d left, vec3d right)
        {
            return left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z;
        }

        public string ToString(string sample)
        {
            return $"{this.X}\t{this.Y}\t{this.Z}\t";
        }

        public bool Equals(vec3d other)
        {
            return this.vec == other.vec;
        }

        public override bool Equals(object obj)
        {
            if (obj is vec3d)
            {
                return this == (vec3d)obj;
            }
            else
            {
                return false;
            }
        }

        public unsafe override int GetHashCode()
        {
            fixed (float* add = &X)
                return HashCode.Combine(X, Y, Z, (ulong)add);
        }

        public override string ToString()
        {
            return $"[{X},{Y},{Z}]";
        }
    }
}
