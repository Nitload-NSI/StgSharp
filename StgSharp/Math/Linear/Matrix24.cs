//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Matrix24.cs"
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
    [StructLayout(LayoutKind.Explicit, Size = ((4 + 2) * 4 * sizeof(float)) + sizeof(bool), Pack = 16)]
    public struct Matrix24 : IMat
    {

        [FieldOffset(6 * 4 * sizeof(float))] internal bool isTransposed;
        [FieldOffset(4 * 4 * sizeof(float))] internal Mat2 transpose;

        [FieldOffset(0)] internal Mat4 mat;

        [FieldOffset(0)] public vec2d Colum0;
        [FieldOffset(16)] public vec2d Colum1;
        [FieldOffset(32)] public vec2d Colum2;
        [FieldOffset(48)] public vec2d Colum3;

        internal Matrix24(
            Vector4 c0,
            Vector4 c1,
            Vector4 c2,
            Vector4 c3
            )
        {
            mat.colum0 = c0;
            mat.colum1 = c1;
            mat.colum2 = c2;
            mat.colum3 = c3;
        }


        public Matrix24(
            float a00, float a01, float a02, float a03,
            float a10, float a11, float a12, float a13
            )
        {
            mat.colum0 = new Vector4(a00, a10, 0, 0);
            mat.colum1 = new Vector4(a01, a11, 0, 0);
            mat.colum2 = new Vector4(a02, a12, 0, 0);
            mat.colum3 = new Vector4(a03, a13, 0, 0);
        }

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if ((rowNum > 1) || (rowNum < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if ((columNum > 3) || (columNum < 0))
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
                if ((rowNum > 1) || (rowNum < 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if ((columNum > 3) || (columNum < 0))
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

        public override string ToString()
        {
            InternalTranspose();
            return $"{$"[{mat.m00},{mat.m01},{mat.m02},{mat.m03}],"}{$"[{mat.m10},{mat.m11},{mat.m12},{mat.m13}]"}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed (Mat4* source = &this.mat)
                {
                    fixed (Mat2* target = &this.transpose)
                    {
                        InternalIO.Transpose4to2_internal(source, target);
                    }
                }

                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix24 operator -(Matrix24 left, Matrix24 right)
        {
            return new Matrix24(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum1 - right.mat.colum1,
                left.mat.colum2 - right.mat.colum2,
                left.mat.colum3 - right.mat.colum3
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix24 operator *(Matrix24 mat, float value)
        {
            return new Matrix24(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value,
                mat.mat.colum2 * value,
                mat.mat.colum3 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix22 operator *(Matrix24 left, Matrix42 right)
        {
            left.InternalTranspose();
            return new Matrix22(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix23 operator *(Matrix24 left, Matrix43 right)
        {
            left.InternalTranspose();
            return new Matrix23(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix24 operator *(Matrix24 left, Matrix44 right)
        {
            left.InternalTranspose();
            return new Matrix24(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),
                Vector4.Dot(left.transpose.colum0, right.mat.colum3),

                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2),
                Vector4.Dot(left.transpose.colum1, right.mat.colum3)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix24 operator /(Matrix24 mat, float value)
        {
            return new Matrix24(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value,
                mat.mat.colum2 / value,
                mat.mat.colum3 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix24 operator +(Matrix24 left, Matrix24 right)
        {
            return new Matrix24(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum1 + right.mat.colum1,
                left.mat.colum2 + right.mat.colum2,
                left.mat.colum3 + right.mat.colum3
                );
        }



    }


}
