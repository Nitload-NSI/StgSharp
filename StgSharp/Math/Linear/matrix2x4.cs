using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = (4 + 2) * 4 * sizeof(float) + sizeof(bool), Pack = 16)]
    public struct Matrix2x4
    {
        [FieldOffset(0)] internal Mat4 mat;
        [FieldOffset(4 * 4 * sizeof(float))] internal Mat2 transpose;
        [FieldOffset(6 * 4 * sizeof(float))] internal bool isTransposed;
    
        public Matrix2x4(
            float a00, float a01,float a02,float a03,
            float a10, float a11,float a12,float a13
            )
        {
            mat.colum0 = new Vector4(a00, a10, 0, 0);
            mat.colum1 = new Vector4(a01, a11, 0, 0);
            mat.colum2 = new Vector4(a02, a12, 0, 0);
            mat.colum3 = new Vector4(a03, a13, 0, 0);
        }

        internal Matrix2x4(
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

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if (rowNum > 1 || rowNum < 0)
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
                if (rowNum > 1 || rowNum < 0)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed (Mat4* source = &this.mat)
                fixed (Mat2* target = &this.transpose)
                {
                    internalIO.Transpose4to2_internal(source, target);
                }
                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x4 operator +(Matrix2x4 left, Matrix2x4 right)
        {
            return new Matrix2x4(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum1 + right.mat.colum1,
                left.mat.colum2 + right.mat.colum2,
                left.mat.colum3 + right.mat.colum3
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x4 operator -(Matrix2x4 left, Matrix2x4 right)
        {
            return new Matrix2x4(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum1 - right.mat.colum1,
                left.mat.colum2 - right.mat.colum2,
                left.mat.colum3 - right.mat.colum3
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x4 operator *(Matrix2x4 mat, float value)
        {
            return new Matrix2x4(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value,
                mat.mat.colum2 * value,
                mat.mat.colum3 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x4 operator /(Matrix2x4 mat, float value)
        {
            return new Matrix2x4(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value,
                mat.mat.colum2 / value,
                mat.mat.colum3 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 operator *(Matrix2x4 left, Matrix4x2 right)
        {
            left.InternalTranspose();
            return new Matrix2x2(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                
                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x3 operator *(Matrix2x4 left, Matrix4x3 right)
        {
            left.InternalTranspose();
            return new Matrix2x3(
                Vector4.Dot(left.transpose.colum0, right.mat.colum0),
                Vector4.Dot(left.transpose.colum0, right.mat.colum1),
                Vector4.Dot(left.transpose.colum0, right.mat.colum2),
                
                Vector4.Dot(left.transpose.colum1, right.mat.colum0),
                Vector4.Dot(left.transpose.colum1, right.mat.colum1),
                Vector4.Dot(left.transpose.colum1, right.mat.colum2)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x4 operator *(Matrix2x4 left, Matrix4x4 right)
        {
            left.InternalTranspose();
            return new Matrix2x4(
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

        public override string ToString()
        {
            InternalTranspose();
            return $"[{mat.m00},{mat.m01},{mat.m02},{mat.m03}]," +
                $"[{mat.m10},{mat.m11},{mat.m12},{mat.m13}]";
        }



    }


}
