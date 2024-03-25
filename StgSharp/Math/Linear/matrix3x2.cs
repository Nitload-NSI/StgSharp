//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="matrix3x2.cs"
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
    [StructLayout(LayoutKind.Explicit, Size = (5 * 4 * sizeof(float)) + sizeof(bool), Pack = 16)]
    public struct Matrix3x2
    {

        [FieldOffset(0)] internal Mat2 mat;
        [FieldOffset(0)] internal vec3d colum0;
        [FieldOffset(4 * 4)] internal vec3d colum1;
        [FieldOffset(2 * 4 * sizeof(float))] internal Mat3 transpose;
        [FieldOffset(5 * 4 * sizeof(float))] internal bool isTransposed;

        internal Matrix3x2(
            Vector4 c0,
            Vector4 c1
            )
        {
            mat.colum0 = c0;
            mat.colum1 = c1;
        }


        public unsafe Matrix3x2(
            float a00, float a01,
            float a10, float a11,
            float a20, float a21
            )
        {
            mat.colum0 = new Vector4(a00, a10, a20, 0);
            mat.colum0 = new Vector4(a01, a11, a21, 0);
#if NET5_0_OR_GREATER
            Unsafe.SkipInit(out colum0);
            Unsafe.SkipInit(out colum1);
            Unsafe.SkipInit(out transpose);
#endif
        }

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if ((rowNum > 2) || (rowNum < 0))
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
                if ((rowNum > 2) || (rowNum < 0))
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

        public vec3d VerticalVecMultiply(Vec2d vec)
        {
            return (colum0 * vec.X) + (colum1 * vec.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed (Mat2* source = &this.mat)
                {
                    fixed (Mat3* target = &this.transpose)
                    {
                        InternalIO.Transpose2to3_internal(source, target);
                    }
                }

                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator -(Matrix3x2 left, Matrix3x2 right)
        {
            return new Matrix3x2(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum1 - right.mat.colum1
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator *(Matrix3x2 mat, float value)
        {
            return new Matrix3x2(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator *(Matrix3x2 left, Matrix2x2 right)
        {
            left.InternalTranspose();
            return new Matrix3x2(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),

                Vector4.Dot(left.transpose.colum2, right.mat.colum0),
                Vector4.Dot(left.transpose.colum2, right.mat.colum1)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x3 operator *(Matrix3x2 left, Matrix2x3 right)
        {
            left.InternalTranspose();
            return new Matrix3x3(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2),

                Vector4.Dot(left.transpose.colum2, right.mat.colum0),
                Vector4.Dot(left.transpose.colum2, right.mat.colum1),
                Vector4.Dot(left.transpose.colum2, right.mat.colum2)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x4 operator *(Matrix3x2 left, Matrix2x4 right)
        {
            left.InternalTranspose();
            return new Matrix3x4(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),
                Vector4.Dot(left.transpose.colum0, right.mat.colum3),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2),
                Vector4.Dot(left.transpose.colum1, right.mat.colum3),

                Vector4.Dot(left.transpose.colum2, right.mat.colum0),
                Vector4.Dot(left.transpose.colum2, right.mat.colum1),
                Vector4.Dot(left.transpose.colum2, right.mat.colum2),
                Vector4.Dot(left.transpose.colum2, right.mat.colum3)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator /(Matrix3x2 mat, float value)
        {
            return new Matrix3x2(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator +(Matrix3x2 left, Matrix3x2 right)
        {
            return new Matrix3x2(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum1 + right.mat.colum1
                );
        }


    }
}
