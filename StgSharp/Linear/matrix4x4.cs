using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix4x4:IEquatable<matrix4x4>
    {
        [FieldOffset(0)]
        public vec4d row1;
        [FieldOffset(16)]
        public vec4d row2;
        [FieldOffset(32)]
        public vec4d row3;
        [FieldOffset(48)]
        public vec4d row4;

        public matrix4x4( vec4d r1,vec4d r2,vec4d r3,vec4d r4)
        {
            row1 = r1;
            row2 = r2;
            row3 = r3;
            row4 = r4;
        }

        public matrix4x4(
            float a11, float a12, float a13, float a14,
            float a21, float a22, float a23, float a24,
            float a31, float a32, float a33, float a34,
            float a41, float a42, float a43, float a44
            )
        {
            row1 = new vec4d(a11, a12, a13, a14);
            row2 = new vec4d(a21, a22, a23, a24);
            row3 = new vec4d(a31, a32, a33, a34);
            row4 = new vec4d(a41, a42, a43, a44);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        bool IEquatable<matrix4x4>.Equals(matrix4x4 other)
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

        public override int GetHashCode()
        {
            return HashCode.Combine(row1, row2, row3, row4);
        }

        public override string ToString()
        {
            return $"{this.row1}\n{this.row2}\n{this.row3}\n{this.row4}";
        }

        public static matrix4x4 operator +(matrix4x4 matrix1, matrix4x4 matrix2)
        {
            return new matrix4x4(
                matrix1.row1 + matrix2.row1,
                matrix1.row2 + matrix2.row2,
                matrix1.row3 + matrix2.row3,
                matrix1.row4 + matrix2.row4
                );
        }

        public static matrix4x4 operator -(matrix4x4 matrix1, matrix4x4 matrix2)
        {
            return new matrix4x4(
                matrix1.row1 - matrix2.row1,
                matrix1.row2 - matrix2.row2,
                matrix1.row3 - matrix2.row3,
                matrix1.row4 - matrix2.row4
                );
        }

        public static matrix4x4 operator *(matrix4x4 array, float x)
        {
            return new matrix4x4(
                array.row1 * x,
                array.row2 * x,
                array.row3 * x,
                array.row4 * x
                );
        }

        

    }

    
    
}
