using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix4x2:IEquatable<matrix4x2>
    {
        [FieldOffset(0)]
        public vec2d row1;
        [FieldOffset(8)]
        public vec2d row2;
        [FieldOffset(16)]
        public vec2d row3;
        [FieldOffset(24)]
        public vec2d row4;

        public matrix4x2(vec2d r1, vec2d r2, vec2d r3, vec2d r4)
        {
            row1 = r1;
            row2 = r2;
            row3 = r3;
            row4 = r4;
        }

        public matrix4x2(
            float a11, float a12,
            float a21, float a22,
            float a31, float a32,
            float a41, float a42
            )
        {
            row1 = new vec2d(a11, a12);
            row2 = new vec2d(a21, a22);
            row3 = new vec2d(a31, a32);
            row4 = new vec2d(a41, a42);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() == typeof(matrix4x2))
            {
                return false;
            }
            else
            {
                return this.Equals((matrix4x2)obj);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row1, row2, row3, row4);
        }

        public override string ToString()
        {
            return $"{row1}\n{row2}\n{row3}\n{row4}";
        }

        bool IEquatable<matrix4x2>.Equals(matrix4x2 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.row1 == other.row1
                    && this.row2 == other.row2
                    && this.row3 == other.row3
                    && this.row4 == other.row4;
            }
        }

        
    }
}
