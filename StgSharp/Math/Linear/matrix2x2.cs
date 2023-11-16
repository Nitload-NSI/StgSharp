using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 2 * 2 * 4 * sizeof(float) + sizeof(bool), Pack = 16)]
    public struct Matrix2x2 : IMat
    {
        [FieldOffset(0)]
        internal Mat2 mat;
        [FieldOffset(2 * 4 * sizeof(float))]
        internal Mat2 transpose;
        [FieldOffset(2 * 2 * 4 * sizeof(float))]
        internal bool isTransposed;

        public Matrix2x2(
            float a00, float a01,
            float a10, float a11
            )
        {
            mat.colum0 = new Vector4(a00, a10, 0, 0);
            mat.colum1 = new Vector4(a01, a11, 0, 0);
        }

        internal Matrix2x2(
            Vector4 c0,
            Vector4 c1
            )
        {
            mat.colum0 = c0;
            mat.colum1 = c1;
        }

        internal Matrix2x2(
            Mat2 mat
            )
        {
            this.mat = mat;
        }

        public Matrix2x2 Transpose
        {
            get
            {
                InternalTranspose();
                return new Matrix2x2(this.transpose);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InternalTranspose()
        {
            if (!isTransposed)
            {
                transpose = mat;
                float cache = transpose.m10;
                transpose.m10 = transpose.m01;
                transpose.m01 = cache;

                isTransposed = true;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Matrix2x2 operator +(Matrix2x2 left, Matrix2x2 right)
        {
            Mat2* refmat = InternalIO.add_mat2(&left.mat, &right.mat);
            Matrix2x2 ret = new Matrix2x2(*refmat);
            InternalIO.deinit_mat2(refmat);
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 operator -(Matrix2x2 left, Matrix2x2 right)
        {
            return new Matrix2x2(
                left.mat.colum0 + right.mat.colum0,
                left.mat.colum1 + right.mat.colum1
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 operator *(Matrix2x2 mat, float value)
        {
            return new Matrix2x2(
                mat.mat.colum0 * value,
                mat.mat.colum1 * value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 operator /(Matrix2x2 mat, float value)
        {
            return new Matrix2x2(
                mat.mat.colum0 / value,
                mat.mat.colum1 / value
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 operator *(Matrix2x2 left, Matrix2x2 right)
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
        public static Matrix2x3 operator *(Matrix2x2 left, Matrix2x3 right)
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
        public static Matrix2x4 operator *(Matrix2x2 left, Matrix2x4 right)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Det()
        {
            return mat.m00 * mat.m11 - mat.m10 * mat.m01;
        }



    }
}
