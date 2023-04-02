using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix3x2:IEquatable<matrix3x2>
    {
        [FieldOffset(0)]
        public vec2d row1;
        [FieldOffset(8)]
        public vec2d row2;
        [FieldOffset(16)]
        public vec2d row3;

        public matrix3x2(vec2d r1,vec2d r2,vec2d r3)
        {
            row1= r1;
            row2= r2;
            row3= r3;
        }

        public matrix3x2(
            float a11, float a12,
            float a21, float a22,
            float a31, float a32
            )
        {
            row1 = new vec2d(a11, a12);
            row2 = new vec2d(a21, a22);
            row3 = new vec2d(a31, a32);
        }

        bool IEquatable<matrix3x2>.Equals(matrix3x2 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return
                    this.row1 == other.row1
                    && this.row2 == other.row2
                    && this.row3 == other.row3
                    ;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix3x2))
            {
                return false;
            }
            else
            {
                return this.Equals((matrix3x2)obj);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row1, row2, row3);
        }

        public override string ToString()
        {
            return $"{row1}\n{row2}\n{row3}";
        }

        public static matrix3x2 operator +(matrix3x2 matrix1, matrix3x2 matrix2)
        {
            return new matrix3x2(
                matrix1.row1 + matrix2.row1,
                matrix1.row2 + matrix2.row2,
                matrix1.row3 + matrix2.row3
                );
        }

        public static matrix3x2 operator -(matrix3x2 matrix1, matrix3x2 matrix2)
        {
            return new matrix3x2(
                matrix1.row1 - matrix2.row1,
                matrix1.row2 - matrix2.row2,
                matrix1.row3 - matrix2.row3
                );
        }

    }
}
