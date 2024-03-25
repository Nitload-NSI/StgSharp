//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="matrix4x4.cs"
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
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



namespace StgSharp.Math
{


    [StructLayout(LayoutKind.Explicit, Size = (2 * 16 * sizeof(float)) + sizeof(bool), Pack = 16)]
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {

        [FieldOffset(0)] internal Mat4 mat;
        [FieldOffset(64 * sizeof(float))] internal Mat4 transpose;
        [FieldOffset(2 * 64 * sizeof(float))] internal bool isTransposed;

        internal Matrix4x4(Mat4 mat)
        {
            this.mat = mat;
        }

        internal Matrix4x4(
            Vector4 vec0,
            Vector4 vec1,
            Vector4 vec2,
            Vector4 vec3
            )
        {
            mat.colum0 = vec0;
            mat.colum1 = vec1;
            mat.colum2 = vec2;
            mat.colum3 = vec3;
        }


        public unsafe Matrix4x4(
            float a00, float a01, float a02, float a03,
            float a10, float a11, float a12, float a13,
            float a20, float a21, float a22, float a23,
            float a30, float a31, float a32, float a33
            )
        {
            mat.colum0 = new Vector4(a00, a10, a20, a30);
            mat.colum0 = new Vector4(a01, a11, a21, a31);
            mat.colum0 = new Vector4(a02, a12, a22, a32);
            mat.colum0 = new Vector4(a03, a13, a23, a33);

        }

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if ((rowNum > 3) || (rowNum < 0))
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

                if ((rowNum > 3) || (rowNum < 0))
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe float Det()
        {
            InternalTranspose();
            fixed (Mat4* _this = &mat, _trans = &transpose)
            {
                return InternalIO.det_mat4(_this, _trans);
            }
        }

        public override bool Equals(object? obj)
        {
            return (obj is Matrix4x4 x) && Equals(x);
        }

        public bool Equals(Matrix4x4 other)
        {
            return mat.Equals(other.mat);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                mat.colum0.GetHashCode(),
                mat.colum1.GetHashCode(),
                mat.colum2.GetHashCode(),
                mat.colum3.GetHashCode()
                );
        }

        public override string ToString()
        {
            InternalTranspose();
            return $"{transpose.colum0}{transpose.colum1}{transpose.colum2}{transpose.colum3}";
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed (Mat4* source = &this.mat, target = &this.transpose)
                {
                    InternalIO.Transpose4to4_internal(source, target);
                }

                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right)
        {
            return new Matrix4x4(
                right.mat.colum0 - left.mat.colum0,
                right.mat.colum1 - left.mat.colum1,
                right.mat.colum2 - left.mat.colum2,
                right.mat.colum3 - left.mat.colum3
                );
        }

        public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix4x4 operator *(Matrix4x4 mat, float value)
        {
            return new Matrix4x4(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value,
                mat.mat.colum2 * value,
                mat.mat.colum3 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix4x2 operator *(Matrix4x4 left, Matrix4x2 right)
        {
            left.InternalTranspose();
            return new Matrix4x2(
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
        public static Matrix4x3 operator *(Matrix4x4 left, Matrix4x3 right)
        {
            left.InternalTranspose();
            return new Matrix4x3(
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
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            left.InternalTranspose();
            return new Matrix4x4(
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
                Vector4.Dot(left.transpose.colum2, right.mat.colum3),

                Vector4.Dot(left.transpose.colum3, right.mat.colum0),
                Vector4.Dot(left.transpose.colum3, right.mat.colum1),
                Vector4.Dot(left.transpose.colum3, right.mat.colum2),
                Vector4.Dot(left.transpose.colum3, right.mat.colum3)

                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
        {
            return new Matrix4x4(
                right.mat.colum0 + left.mat.colum0,
                right.mat.colum1 + left.mat.colum1,
                right.mat.colum2 + left.mat.colum2,
                right.mat.colum3 + left.mat.colum3
                );
        }

        public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
        {
            return left.Equals(right);
        }

    }



}
