using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 6 * 4 * sizeof(float) + sizeof(bool), Pack = 16)]
    public struct Matrix3x3
    {
        [FieldOffset(0)] internal Mat3 mat;
        [FieldOffset(3 * 4 * sizeof(float))] internal Mat3 transpose;
        [FieldOffset(6 * 4 * sizeof(float))] internal bool isTransposed;
        [FieldOffset(0 * 4 * sizeof(float))] public vec3d Colum0;
        [FieldOffset(1 * 4 * sizeof(float))] public vec3d Colum1;
        [FieldOffset(2 * 4 * sizeof(float))] public vec3d Colum2;

        public Matrix3x3(
            float a00, float a01, float a02,
            float a10, float a11, float a12,
            float a20, float a21, float a22
            )
        {
            mat.colum0 = new Vector4(a00, a10, a20, 0);
            mat.colum1 = new Vector4(a01, a11, a21, 0);
            mat.colum2 = new Vector4(a02, a12, a22, 0);
        }

        internal Matrix3x3(
            Vector4 c0,
            Vector4 c1,
            Vector4 c2
            )
        {
            mat.colum0 = c0;
            mat.colum1 = c1;
            mat.colum2 = c2;
        }

        public unsafe float this[int rowNum, int columNum]
        {
            get
            {
                if (rowNum > 2 || rowNum < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columNum));
                }
                if (columNum > 2 || columNum < 0)
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
                if (columNum > 2 || columNum < 0)
                {
                    InternalIO.InternalWriteLog(
                        $"Attempt to write unused space in {GetType().Name} {nameof(mat)}",
                        LogType.Error
                        );
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
                fixed (Mat3* source = &this.mat, target = &this.transpose)
                {
                    InternalIO.Transpose3to3_internal(source, target);
                }
                isTransposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe float Det()
        {
            fixed (Mat3* mat = &this.mat)
            {
                return InternalIO.det_mat3(mat);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x3 operator +(Matrix3x3 left, Matrix3x3 right)
        {
            return new Matrix3x3(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum1 + right.mat.colum1,
                left.mat.colum2 + right.mat.colum2
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x3 operator -(Matrix3x3 left, Matrix3x3 right)
        {
            return new Matrix3x3(
                left.mat.colum0 - right.mat.colum0,
                left.mat.colum1 - right.mat.colum1,
                left.mat.colum2 + right.mat.colum2
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x3 operator *(Matrix3x3 mat, float value)
        {
            return new Matrix3x3(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value,
                mat.mat.colum2 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x3 operator /(Matrix3x3 mat, float value)
        {
            return new Matrix3x3(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value,
                mat.mat.colum2 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator *(Matrix3x3 left, vec3d right)
        {
            left.InternalTranspose();
            return new vec3d(
                Vector4.Dot(left.transpose.colum0, right.vec),
                Vector4.Dot(left.transpose.colum1, right.vec),
                Vector4.Dot(left.transpose.colum2, right.vec)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 operator *(Matrix3x3 left, Matrix3x2 right)
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
        public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right)
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
        public static Matrix3x4 operator *(Matrix3x3 left, Matrix3x4 right)
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
