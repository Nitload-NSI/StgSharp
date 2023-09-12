using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{

    [StructLayout(LayoutKind.Explicit, Size = 7*4*sizeof(float)+ sizeof(bool),Pack = 16)]
    public struct Matrix3x4
    {
        [FieldOffset(0)] internal Mat4 mat;
        [FieldOffset(4*4*sizeof(float))] internal Mat3 transpose;
        [FieldOffset(7 * 4 * sizeof(float))] internal bool isTransposed;

        public Matrix3x4(
            float a00, float a01, float a02, float a03,
            float a10, float a11, float a12, float a13,
            float a20, float a21, float a22, float a23
            )
        {
            mat.colum0 = new Vector4(a00, a10, a20, 0);
            mat.colum0 = new Vector4(a01, a11, a21, 0);
            mat.colum0 = new Vector4(a02, a12, a22, 0);
            mat.colum0 = new Vector4(a03, a13, a23, 0);
        }

        internal Matrix3x4(
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

        internal Matrix3x4(Mat4 mat)
        {
            this.mat = mat;
        }

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if (rowNum > 2 || rowNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum > 3 || columNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                InternalTranspose();
                fixed (float* p = &this.transpose.m00)
                {
                    ulong pbit = (ulong)p
                        + (ulong)sizeof(Vector4) * (ulong)rowNum
                        + (ulong)sizeof(float) * (ulong)columNum;
                    return *(float*)pbit;
                }
            }
            set
            {
                if (rowNum > 2 || rowNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum > 3 || columNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                InternalTranspose();
                fixed (float* p = &this.transpose.m00)
                {
                    ulong pbit = (ulong)p
                        + (ulong)sizeof(Vector4) * (ulong)rowNum
                        + (ulong)sizeof(float) * (ulong)columNum;
                    *(float*)pbit = value;
                }
                isTransposed = false;
            }
        }

        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed(Mat4* source = &this.mat)
                fixed(Mat3* target = &this.transpose)
                {
                    internalIO.Transpose4to3_internal(source, target);
                }
                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x4 operator +(Matrix3x4 left, Matrix3x4 right)
        {
            return new Matrix3x4(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum1 + right.mat.colum1,
                left.mat.colum2 + right.mat.colum2,
                left.mat.colum3 + right.mat.colum3
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x4 operator -(Matrix3x4 left, Matrix3x4 right)
        {
            return new Matrix3x4(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum1 - right.mat.colum1,
                left.mat.colum2 - right.mat.colum2,
                left.mat.colum3 - right.mat.colum3
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x4 operator *(Matrix3x4 mat, float value)
        {
            return new Matrix3x4(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value,
                mat.mat.colum2 * value,
                mat.mat.colum3 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x4 operator /(Matrix3x4 mat, float value)
        {
            return new Matrix3x4(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value,
                mat.mat.colum2 / value,
                mat.mat.colum3 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator *(Matrix3x4 left, Matrix4x2 right)
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
        public static Matrix3x3 operator *(Matrix3x4 left, Matrix4x3 right)
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
        public static Matrix3x4 operator *(Matrix3x4 left, Matrix4x4 right)
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




    }
}
