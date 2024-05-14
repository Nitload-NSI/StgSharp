//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Matrix42.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace StgSharp.Math
{


    [StructLayout(LayoutKind.Explicit, Size = ((16 + 8) * sizeof(float)) + sizeof(bool), Pack = 16)]
    public struct Matrix42 : IMat
    {

        [FieldOffset((16 + 8) * sizeof(float))] internal bool isTransposed;

        [FieldOffset(0)] internal Mat2 mat;
        [FieldOffset(8 * sizeof(float))] internal Mat4 transpose;

        internal Matrix42(Mat2 mat)
        {
            this.mat = mat;
        }

        internal Matrix42(
            Vector4 c0,
            Vector4 c1
            )
        {
            mat.colum0 = c0;
            mat.colum1 = c1;
        }


        public Matrix42(
            float a00, float a01,
            float a10, float a11,
            float a20, float a21,
            float a30, float a31
            )
        {
            mat.colum0 = new Vector4(a00, a10, a20, a30);
            mat.colum1 = new Vector4(a01, a11, a21, a31);
        }



        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if ((rowNum > 3) || (rowNum < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if ((columNum > 1) || (columNum < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                InternalTranspose();
                fixed (float* p = &this.transpose.m00)
                {
                    ulong pbit = ((ulong)p)
                        + (((ulong)sizeof(Vector4)) * ((ulong)rowNum))
                        + (((ulong)sizeof(float)) * ((ulong)columNum));
                    return *(float*)pbit;
                }
            }
            set
            {
                if ((rowNum > 3) || (rowNum < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if ((columNum > 1) || (columNum < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                InternalTranspose();
                fixed (float* p = &this.transpose.m00)
                {
                    ulong pbit = ((ulong)p)
                        + (((ulong)sizeof(Vector4)) * ((ulong)rowNum))
                        + (((ulong)sizeof(float)) * ((ulong)columNum));
                    *(float*)pbit = value;
                }
                isTransposed = false;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                mat.colum0.GetHashCode(),
                mat.colum1.GetHashCode()
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed (Mat2* source = &this.mat)
                {
                    fixed (Mat4* target = &this.transpose)
                    {
                        InternalIO.Transpose2to4_internal(source, target);
                    }
                }

                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix42 operator -(Matrix42 left, Matrix42 right)
        {
            return new Matrix42(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum0 - right.mat.colum0
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix42 operator *(Matrix42 mat, float value)
        {
            return new Matrix42(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix42 operator *(Matrix42 left, Matrix22 right)
        {
            left.InternalTranspose();
            return new Matrix42(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),

                Vector4.Dot(left.transpose.colum2, right.mat.colum0),
                Vector4.Dot(left.transpose.colum2, right.mat.colum1),

                Vector4.Dot(left.transpose.colum3, right.mat.colum0),
                Vector4.Dot(left.transpose.colum3, right.mat.colum1)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix43 operator *(Matrix42 left, Matrix23 right)
        {
            left.InternalTranspose();
            return new Matrix43(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2),

                Vector4.Dot(left.transpose.colum2, right.mat.colum0),
                Vector4.Dot(left.transpose.colum2, right.mat.colum1),
                Vector4.Dot(left.transpose.colum2, right.mat.colum2),

                Vector4.Dot(left.transpose.colum3, right.mat.colum0),
                Vector4.Dot(left.transpose.colum3, right.mat.colum1),
                Vector4.Dot(left.transpose.colum3, right.mat.colum2)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix44 operator *(Matrix42 left, Matrix24 right)
        {
            left.InternalTranspose();
            return new Matrix44(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2),

                Vector4.Dot(left.transpose.colum2, right.mat.colum0),
                Vector4.Dot(left.transpose.colum2, right.mat.colum1),
                Vector4.Dot(left.transpose.colum2, right.mat.colum2),
                Vector4.Dot(left.transpose.colum2, right.mat.colum2),

                Vector4.Dot(left.transpose.colum3, right.mat.colum0),
                Vector4.Dot(left.transpose.colum3, right.mat.colum1),
                Vector4.Dot(left.transpose.colum3, right.mat.colum2),
                Vector4.Dot(left.transpose.colum3, right.mat.colum2)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix42 operator /(Matrix42 mat, float value)
        {
            return new Matrix42(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix42 operator +(Matrix42 left, Matrix42 right)
        {
            return new Matrix42(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum0 + right.mat.colum0
                );
        }


    }
}
