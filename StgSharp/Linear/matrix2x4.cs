using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{

    [StructLayout(LayoutKind.Explicit)]
    public struct matrix2x4 : IEquatable<matrix2x4>
    {
        [FieldOffset(0)]
        public vec4d row1;
        [FieldOffset(16)]
        public vec4d row2;

        public matrix2x4( vec4d r1, vec4d r2)
        {
            row1 = r1;
            row2 = r2;
        }

        public matrix2x4(
            float a11, float a12, float a13, float a14,
            float a21, float a22, float a23, float a24
            )
        {
            row1 = new vec4d(a11,a12,a13,a14);
            row2 = new vec4d(a21,a22,a23,a24);
        }

        public bool Equals(matrix2x4 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.row1 == other.row1
                    && this.row2 == other.row2;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix2x4))
            {
                return false;
            }
            else
            {
                return this.Equals((matrix2x4)obj);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.row1, this.row2);
        }

        public override string ToString()
        {
            return $"{this.row1}\n{this.row2}";
        }

        public static matrix2x4 operator +(matrix2x4 matrix1 ,matrix2x4 matrix2)
        {
            return new matrix2x4(
                matrix1.row1 + matrix2.row1,
                matrix1.row2 + matrix2.row2
                ) ;
        }

        public static matrix2x4 operator -(matrix2x4 matrix1, matrix2x4 matrix2)
        {
            return new matrix2x4(
                matrix1.row1 - matrix2.row1,
                matrix1.row2 - matrix2.row2
                );
        }

        public static matrix2x4 operator *(matrix2x4 array, float x)
        {
            return new matrix2x4(
                array.row1 * x,
                array.row2 * x
                );
        }

        public static matrix2x4 operator /(matrix2x4 array, float x)
        {
            return new matrix2x4(
                array.row1 / x,
                array.row2 / x
                );
        }

        
    }
}
