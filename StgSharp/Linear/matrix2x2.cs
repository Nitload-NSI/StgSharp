using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct matrix2x2:IEquatable<matrix2x2>
    {
        [FieldOffset(0)]
        public vec2d row1;
        [FieldOffset(8)]
        public vec2d row2;


        public matrix2x2(vec2d c0, vec2d c1)
        {
            row1 = c0;
            row2 = c1;
        }

        public matrix2x2(
            float a11,
            float a12,
            float a21,
            float a22
            )
        {
            row1 = new vec2d(a11, a12);
            row2 = new vec2d(a21, a22);
        }

        bool IEquatable<matrix2x2>.Equals(matrix2x2 other)
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix2x2))
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
            return HashCode.Combine(row1,row2);
        }

        public override string ToString()
        {
            return $"{row1}\n{row2}";
        }

        public static matrix2x2 operator +(matrix2x2 matrix1 , matrix2x2 matrix2)
        {
            return new matrix2x2(
                matrix1.row1 + matrix2.row1,
                matrix1.row2 + matrix2.row2
                );
        }

        public static matrix2x2 operator -(matrix2x2 matrix1, matrix2x2 matrix2)
        {
            return new matrix2x2(
                matrix1.row1 - matrix2.row1,
                matrix1.row2 - matrix2.row2
                );
        }

        

        
        
    }
}
