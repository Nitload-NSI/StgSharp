//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Matrix44"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance;
using StgSharp.Mathematics.Numeric;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Graphic
{
    [StructLayout(LayoutKind.Explicit, Size = (2 * 16 * sizeof(float)) + sizeof(bool), Pack = 16)]
    public unsafe struct Matrix44 : IEquatable<Matrix44>, IMatrix<float>, IEnumerable<Vec4>
    {

        [FieldOffset(0 * sizeof(float))] internal Column column;

        [FieldOffset(0)] internal ColumnSet4 mat;
        [FieldOffset(0)] internal MatrixKernel<float> kernel;

        internal Matrix44(ColumnSet4 mat)
        {
            Unsafe.SkipInit(out this);
            this.mat = mat;
        }

        internal Matrix44(Vector4 vec0, Vector4 vec1, Vector4 vec2, Vector4 vec3)
        {
            Unsafe.SkipInit(out this);
            mat.colum0 = vec0;
            mat.colum1 = vec1;
            mat.colum2 = vec2;
            mat.colum3 = vec3;
        }

        public Matrix44()
        {
            Unsafe.SkipInit(out this);
        }

        public unsafe Matrix44(
                      float a00,
                      float a01,
                      float a02,
                      float a03,
                      float a10,
                      float a11,
                      float a12,
                      float a13,
                      float a20,
                      float a21,
                      float a22,
                      float a23,
                      float a30,
                      float a31,
                      float a32,
                      float a33)
        {
            Unsafe.SkipInit(out this);
            mat.colum0 = new Vector4(a00, a10, a20, a30);
            mat.colum1 = new Vector4(a01, a11, a21, a31);
            mat.colum2 = new Vector4(a02, a12, a22, a32);
            mat.colum3 = new Vector4(a03, a13, a23, a33);
        }

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
#if DEBUG
                if (rowNum is > 3 or < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum is > 3 or < 0) {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
#endif
                return kernel[rowNum + 4 * columNum];
            }
            set
            {
#if DEBUG
                if (rowNum is > 3 or < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum is > 3 or < 0) {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
#endif
                kernel[rowNum + 4 * columNum] = value;
            }
        }

        public Matrix44 Transpose
        {
            get
            {
                Matrix44 transpose = new();
                InternalTranspose(ref transpose);
                return transpose;
            }
        }

        public static Matrix44 Unit => new Matrix44(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        public static Matrix44 WNegativeUnit => new Matrix44(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0,
                                                             0, 0, -1);

        public static Matrix44 XNegativeUnit => new Matrix44(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0,
                                                             0, 0, 1);

        public static Matrix44 YNegativeUnit => new Matrix44(1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1, 0, 0,
                                                             0, 0, 1);

        public static Matrix44 ZNegativeUnit => new Matrix44(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0, 0,
                                                             0, 0, 1);

        public ReadOnlySpan<float> AsSpan()
        {
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<ColumnSet4, float>(ref mat), 16);
        }

        public override bool Equals(object? obj)
        {
            return obj is Matrix44 x && Equals(x);
        }

        public bool Equals(Matrix44 other)
        {
            return mat.Equals(other.mat);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(mat.colum0.GetHashCode(), mat.colum1.GetHashCode(),
                                    mat.colum2.GetHashCode(), mat.colum3.GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InternalTranspose(ref Matrix44 transpose)
        {
            fixed (MatrixKernel<float>* source = &this.kernel, target = &transpose.kernel) {
                NativeIntrinsic.Intrinsic.f32_buffer_transpose(source, target);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix44 operator -(Matrix44 left, Matrix44 right)
        {
            Matrix44 ret = new();
            NativeIntrinsic.Intrinsic.f32_buffer_sub(&left, &right, &ret, 1);
            return ret;
        }

        public static bool operator !=(Matrix44 left, Matrix44 right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix44 operator *(Matrix44 mat, float value)
        {
            return new Matrix44(mat.mat.colum0 * value, mat.mat.colum1 * value,
                                mat.mat.colum2 * value, mat.mat.colum3 * value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix44 operator *(Matrix44 left, Matrix44 right)
        {
            Matrix44 transpose = left.Transpose;
            return new Matrix44(Vector4.Dot(transpose.mat.colum0, right.mat.colum0),
                                Vector4.Dot(transpose.mat.colum0, right.mat.colum1),
                                Vector4.Dot(transpose.mat.colum0, right.mat.colum2),
                                Vector4.Dot(transpose.mat.colum0, right.mat.colum3),
                                Vector4.Dot(transpose.mat.colum1, right.mat.colum0),
                                Vector4.Dot(transpose.mat.colum1, right.mat.colum1),
                                Vector4.Dot(transpose.mat.colum1, right.mat.colum2),
                                Vector4.Dot(transpose.mat.colum1, right.mat.colum3),
                                Vector4.Dot(transpose.mat.colum2, right.mat.colum0),
                                Vector4.Dot(transpose.mat.colum2, right.mat.colum1),
                                Vector4.Dot(transpose.mat.colum2, right.mat.colum2),
                                Vector4.Dot(transpose.mat.colum2, right.mat.colum3),
                                Vector4.Dot(transpose.mat.colum3, right.mat.colum0),
                                Vector4.Dot(transpose.mat.colum3, right.mat.colum1),
                                Vector4.Dot(transpose.mat.colum3, right.mat.colum2),
                                Vector4.Dot(transpose.mat.colum3, right.mat.colum3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix44 operator +(Matrix44 left, Matrix44 right)
        {
            Matrix44 ret = new();
            NativeIntrinsic.Intrinsic.f32_buffer_add(&left, &right, &ret, 1);
            return ret;
        }

        public static bool operator ==(Matrix44 left, Matrix44 right)
        {
            return left.Equals(right);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<Vec4> IEnumerable<Vec4>.GetEnumerator()
        {
            for (int i = 0; i < 4; i++) {
                yield return column[i];
            }
        }

        [InlineArray(4)]
        internal struct Column
        {

            private Vec4 value;

        }

    }
}

