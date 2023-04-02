using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct colum3 : IEquatable<colum3>
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;
        [FieldOffset(8)]
        public float Z;

        public colum3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        bool IEquatable<colum3>.Equals(colum3 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.X == other.X
                    && this.Y == other.Y
                    && this.Z == other.Z;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(colum3))
            {
                return false;
            }
            else
            {
                return this.Equals((colum3)obj);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public override string ToString()
        {
            return $"{X}\t{Y}\t{Z}";
        }

        public static bool operator ==(colum3 vec1, colum3 vec2)
        {
            return vec1.X == vec2.X
                && vec1.Y == vec2.Y
                && vec1.Z == vec2.Z;
        }

        public static bool operator !=(colum3 vec1, colum3 vec2)
        {
            return !(vec1.X == vec2.X
                && vec1.Y == vec2.Y
                && vec1.Z == vec2.Z);
        }

        public static colum3 operator +(colum3 vec1, colum3 vec2)
        {
            return new colum3(
                vec1.X + vec2.X,
                vec1.Y + vec2.Y,
                vec1.Z + vec2.Z
                );
        }

        public static colum3 operator -(colum3 vec1, colum3 vec2)
        {
            return new colum3(
                vec1.X - vec2.X,
                vec1.Y - vec2.Y,
                vec1.Z - vec2.Z
                );
        }

        public static float operator *(colum3 vec1, colum3 vec2)
        {
            return
                vec1.X * vec2.X
                + vec1.Y + vec2.Y
                + vec1.Z + vec2.Z;
        }

        public static colum3 operator *(colum3 vec, float x)
        {
            return new colum3(
                vec.X * x,
                vec.Y * x,
                vec.Z * x
                );
        }

        public static colum3 operator /(colum3 vec, float x)
        {
            return new colum3(
                vec.X / x,
                vec.Y / x,
                vec.Z / x
                );
        }
    }
}
