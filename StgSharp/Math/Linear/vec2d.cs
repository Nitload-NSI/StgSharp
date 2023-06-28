using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{

    [StructLayout(LayoutKind.Explicit)]
    public struct vec2d : IEquatable<vec2d>
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(sizeof(float))]
        public float Y;

        public vec2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(vec2d other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.X == other.X && this.Y == other.Y;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(vec2d))
            {
                return false;
            }
            else
            {
                return this.Equals((vec2d)obj);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{this.X}\t{this.Y}";
        }

        public static bool operator ==(vec2d vec1, vec2d vec2)
        {
            return vec1.X == vec2.X && vec1.Y == vec2.Y;
        }

        public static bool operator !=(vec2d vec1, vec2d vec2)
        {
            return !(vec1.X == vec2.X && vec1.Y == vec2.Y);
        }

        public static vec2d operator +(vec2d vec1, vec2d vec2)
        {
            return new vec2d(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static vec2d operator -(vec2d vec1, vec2d vec2)
        {
            return new vec2d(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        public static vec2d operator *(vec2d vec, float x)
        {
            return new vec2d(vec.X * x, vec.Y * x);
        }

        public static vec2d operator /(vec2d vec, float x)
        {
            return new vec2d(vec.X / x, vec.Y / x);
        }

        public static implicit operator float[](vec2d vec)
        {
            return new float[2] { vec.X, vec.Y };
        }

    }
}
