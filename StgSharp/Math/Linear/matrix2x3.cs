using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix2x3 : IEquatable<matrix2x3>
    {
        [FieldOffset(0)]
        public vec3d row1;
        [FieldOffset(12)]
        public vec3d row2;

        public matrix2x3(vec3d r1, vec3d r2)
        {
            row1 = r1;
            row2 = r2;
        }

        public matrix2x3(
            float r11, float r12, float r13,
            float r21, float r22, float r23
            )
        {
            row1 = new vec3d(r11, r12, r13);
            row2 = new vec3d(r21, r22, r23);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix2x3))
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
            return HashCode.Combine(row1, row2);
        }

        public override string ToString()
        {
            return $"{row1}\n{row2}";
        }

        bool IEquatable<matrix2x3>.Equals(matrix2x3 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return
                    this.row1 == other.row1
                    && this.row2 == other.row2;
            }
        }

        public static matrix2x3 operator +(matrix2x3 matrix1, matrix2x3 matrix2)
        {
            return new matrix2x3(
                matrix1.row1 + matrix2.row1,
                matrix1.row2 + matrix2.row2
                );
        }

        public static matrix2x3 operator -(matrix2x3 matrix1, matrix2x3 matrix2)
        {
            return new matrix2x3(
                matrix1.row1 - matrix2.row1,
                matrix1.row2 - matrix2.row2
                );
        }

        public static matrix2x3 operator *(matrix2x3 array, float x)
        {
            return new matrix2x3(
                array.row1 * x,
                array.row2 * x
                );
        }

        public static matrix2x3 operator /(matrix2x3 array, float x)
        {
            return new matrix2x3(
                array.row1 / x,
                array.row2 / x
                );
        }



    }
}
