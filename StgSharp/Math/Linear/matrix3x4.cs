using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix3x4 : IEquatable<matrix3x4>
    {
        [FieldOffset(0)]
        public vec4d row1;
        [FieldOffset(16)]
        public vec4d row2;
        [FieldOffset(32)]
        public vec4d row3;

        public matrix3x4(
           vec4d r1, vec4d r2, vec4d r3
            )
        {
            this.row1 = r1;
            this.row2 = r2;
            this.row3 = r3;
        }

        public matrix3x4(
            float a11, float a12, float a13, float a14,
            float a21, float a22, float a23, float a24,
            float a31, float a32, float a33, float a34
            )
        {
            row1 = new vec4d(a11, a12, a13, a14);
            row2 = new vec4d(a21, a22, a23, a24);
            row3 = new vec4d(a31, a32, a33, a34);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix3x4))
            {
                return false;
            }
            else
            {
                return this.Equals((matrix3x4)obj);
            }
        }

        bool IEquatable<matrix3x4>.Equals(matrix3x4 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.row1 == other.row1
                    && this.row2 == other.row2
                    && this.row3 == other.row3;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.row1, this.row2, this.row3);
        }

        public override string ToString()
        {
            return $"{this.row1}\n{this.row2}\n{this.row3}";
        }



    }
}
