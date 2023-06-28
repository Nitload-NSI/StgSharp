using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix3x3 : IEquatable<matrix3x3>
    {
        [FieldOffset(0)]
        public vec3d row1;
        [FieldOffset(12)]
        public vec3d row2;
        [FieldOffset(24)]
        public vec3d row3;

        public matrix3x3(vec3d r0, vec3d r1, vec3d r2)
        {
            row1 = r0;
            row2 = r1;
            row3 = r2;
        }

        public matrix3x3(
            float a11, float a12, float a13,
            float a21, float a22, float a23,
            float a31, float a32, float a33
            )
        {
            row1 = new vec3d(a11, a12, a13);
            row2 = new vec3d(a21, a22, a23);
            row3 = new vec3d(a31, a32, a33);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row1, row2, row3);
        }

        bool IEquatable<matrix3x3>.Equals(matrix3x3 other)
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
                    && this.row3 == other.row3;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix3x3))
            {
                return false;
            }
            else
            {
                return this.Equals((matrix3x3)obj);
            }
        }

        public override string ToString()
        {
            return $"{row1}\n{row2}\n{row3}";
        }



        public static bool operator ==(matrix3x3 matrix1, matrix3x3 matrix2)
        {
            return matrix1.Equals(matrix2);
        }

        public static bool operator !=(matrix3x3 matrix1, matrix3x3 matrix2)
        {
            return !matrix1.Equals(matrix2);
        }

        public static matrix3x3 operator +(matrix3x3 matrix1, matrix3x3 matrix2)
        {
            return new matrix3x3(
                matrix1.row1.X + matrix2.row1.X,
                matrix1.row1.Y + matrix2.row1.Y,
                matrix1.row1.Z + matrix2.row1.Z,

                matrix1.row2.X + matrix2.row2.X,
                matrix1.row2.Y + matrix2.row2.Y,
                matrix1.row2.Z + matrix2.row2.Z,

                matrix1.row3.X + matrix2.row3.X,
                matrix1.row3.Y + matrix2.row3.Y,
                matrix1.row3.Z + matrix2.row3.Z
                );
        }

        public static matrix3x3 operator -(matrix3x3 matrix1, matrix3x3 matrix2)
        {
            return new matrix3x3(
                matrix1.row1.X - matrix2.row1.X,
                matrix1.row1.Y - matrix2.row1.Y,
                matrix1.row1.Z - matrix2.row1.Z,

                matrix1.row2.X - matrix2.row2.X,
                matrix1.row2.Y - matrix2.row2.Y,
                matrix1.row2.Z - matrix2.row2.Z,

                matrix1.row3.X - matrix2.row3.X,
                matrix1.row3.Y - matrix2.row3.Y,
                matrix1.row3.Z - matrix2.row3.Z
                );
        }



        public static matrix3x3 operator *(matrix3x3 array, float x)
        {
            return new matrix3x3(
                array.row1 * x,
                array.row2 * x,
                array.row3 * x
                );
        }

        public static matrix3x3 operator /(matrix3x3 array, float x)
        {
            return new matrix3x3(
                array.row1 / x,
                array.row2 / x,
                array.row3 / x
                );
        }
    }
}
