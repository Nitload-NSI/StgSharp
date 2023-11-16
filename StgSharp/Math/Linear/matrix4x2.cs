using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace StgSharp.Math
{


    [StructLayout(LayoutKind.Explicit, Size = (16 + 8) * sizeof(float) + sizeof(bool), Pack = 16)]
    public struct Matrix4x2
    {
        [FieldOffset(0)] internal Mat2 mat;
        [FieldOffset(8 * sizeof(float))] internal Mat4 transpose;
        [FieldOffset((16 + 8) * sizeof(float))] internal bool isTransposed;


        public Matrix4x2(
            float a00, float a01,
            float a10, float a11,
            float a20, float a21,
            float a30, float a31
            )
        {
            mat.colum0 = new Vector4(a00, a10, a20, a30);
            mat.colum1 = new Vector4(a01, a11, a21, a31);
        }

        internal Matrix4x2(
            Vector4 c0,
            Vector4 c1
            )
        {
            mat.colum0 = c0;
            mat.colum1 = c1;
        }

        internal Matrix4x2(Mat2 mat)
        {
            this.mat = mat;
        }



        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if (rowNum > 3 || rowNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum > 1 || columNum < 0)
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
                if (rowNum > 3 || rowNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum > 1 || columNum < 0)
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
        public static unsafe Matrix4x2 operator +(Matrix4x2 left, Matrix4x2 right)
        {
            return new Matrix4x2(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum0 + right.mat.colum0
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x2 operator *(Matrix4x2 mat, float value)
        {
            return new Matrix4x2(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x2 operator /(Matrix4x2 mat, float value)
        {
            return new Matrix4x2(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix4x2 operator -(Matrix4x2 left, Matrix4x2 right)
        {
            return new Matrix4x2(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum0 - right.mat.colum0
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x2 operator *(Matrix4x2 left, Matrix2x2 right)
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
        public static Matrix4x3 operator *(Matrix4x2 left, Matrix2x3 right)
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
        public static Matrix4x4 operator *(Matrix4x2 left, Matrix2x4 right)
        {
            left.InternalTranspose();
            return new Matrix4x4(
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
        internal unsafe void InternalTranspose()
        {
            if (!isTransposed)
            {
                fixed (Mat2* source = &this.mat)
                fixed (Mat4* target = &this.transpose)
                {
                    InternalIO.Transpose2to4_internal(source, target);
                }
                isTransposed = true;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                mat.colum0.GetHashCode(),
                mat.colum1.GetHashCode()
                );
        }


    }
}
