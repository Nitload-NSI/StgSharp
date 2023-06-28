using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct vec4d : IEquatable<vec4d>
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;
        [FieldOffset(8)]
        public float Z;
        [FieldOffset(12)]
        public float W;


        [FieldOffset(0)]
        public vec2d Point1;
        [FieldOffset(8)]
        public vec2d Point2;

        public vec4d(float a1, float a2, float a3, float a4)
        {
            Point1 = default(vec2d);
            Point2 = default(vec2d);

            X = a1;
            Y = a2;
            Z = a3;
            W = a4;
        }

        public vec4d(double a1, double a2, double a3, double a4)
        {
            Point1 = default(vec2d);
            Point2 = default(vec2d);

            X = (float)a1;
            Y = (float)a2;
            Z = (float)a3;
            W = (float)a4;
        }

        public vec4d(vec2d p1, vec2d p2)
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 0;

            Point1 = p1;
            Point2 = p2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(vec4d))
            {
                return false;
            }
            else
            {
                return this == (vec4d)obj;
            }
        }

        bool IEquatable<vec4d>.Equals(vec4d other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.X == other.X
                    && this.Y == other.Y
                    && this.Z == other.Z
                    && this.W == other.Z;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        public override string ToString()
        {
            return $"{X}\t{Y}\t{Z}\t{W}";
        }

        public static bool operator ==(vec4d vec1, vec4d vec2)
        {
            return vec1.Equals(vec2);
        }

        public static bool operator !=(vec4d vec1, vec4d vec2)
        {
            return !vec1.Equals(vec2);
        }

        public static vec4d operator +(vec4d vec1, vec4d vec2)
        {
            return new vec4d(
                vec1.X + vec2.X,
                vec1.Y + vec2.Y,
                vec1.Z + vec2.Z,
                vec1.W + vec2.W
                );
        }

        public static vec4d operator -(vec4d vec1, vec4d vec2)
        {
            return new vec4d(
                vec1.X - vec2.X,
                vec1.Y - vec2.Y,
                vec1.Z - vec2.Z,
                vec1.W - vec2.W
                );
        }

        public static float operator *(vec4d vec1, vec4d vec2)
        {
            return
                vec1.X * vec2.X
                + vec1.Y * vec2.Y
                + vec1.Z * vec2.Z
                + vec1.W * vec2.W;
        }

        public static vec4d operator *(vec4d vec, float x)
        {
            return new vec4d(vec.X * x, vec.Y * x, vec.Z * x, vec.W * x);
        }

        public static vec4d operator /(vec4d vec, float x)
        {
            return new vec4d(vec.X / x, vec.Y / x, vec.Z / x, vec.W / x);
        }

        public static implicit operator float[](vec4d vec)
        {
            return new float[] { vec.X, vec.Y, vec.Z, vec.W };
        }


    }
}
