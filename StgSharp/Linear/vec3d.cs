using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct vec3d : IEquatable<vec3d>
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;
        [FieldOffset(8)]
        public float Z;

        public vec3d(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        bool IEquatable<vec3d>.Equals(vec3d other)
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
            else if (obj.GetType() != typeof(vec3d))
            {
                return false;
            }
            else
            {
                return this.Equals((vec3d)obj);
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

        public static bool operator ==(vec3d vec1,vec3d vec2)
        {
            return vec1.X == vec2.X 
                && vec1.Y == vec2.Y 
                && vec1.Z == vec2.Z;
        }

        public static bool operator !=(vec3d vec1, vec3d vec2)
        {
            return !(vec1.X == vec2.X 
                && vec1.Y == vec2.Y 
                && vec1.Z == vec2.Z);
        }

        public static vec3d operator +(vec3d vec1, vec3d vec2)
        {
            return new vec3d(
                vec1.X + vec2.X,
                vec1.Y + vec2.Y,
                vec1.Z + vec2.Z
                );
        }

        public static vec3d operator -(vec3d vec1, vec3d vec2)
        {
            return new vec3d(
                vec1.X - vec2.X,
                vec1.Y - vec2.Y,
                vec1.Z - vec2.Z
                );
        }

        public static float operator *(vec3d vec1, vec3d vec2)
        {
            return 
                vec1.X * vec2.X
                + vec1.Y + vec2.Y
                + vec1.Z + vec2.Z;
        }

        public static vec3d operator *(vec3d vec, float x)
        {
            return new vec3d(
                vec.X * x,
                vec.Y * x,
                vec.Z * x
                );
        }

        public static vec3d operator *( float x, vec3d vec)
        {
            return new vec3d(
                vec.X * x,
                vec.Y * x,
                vec.Z * x
                );
        }

        public static vec3d operator /(vec3d vec, float x)
        {
            return new vec3d(
                vec.X / x,
                vec.Y / x,
                vec.Z / x
                );
        }
    }
}
