using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    public static partial class VectorCalc
    {
        /*

        public static unsafe float Multiple(Vec3d vec1, Vec3d vec2)
        {
            IntPtr retPtr = internalIO.mulps_internal(vec1._Vector4Ptr,vec2._Vector4Ptr);

            Vector4 Vector4 = *(Vector4*)retPtr;
            Marshal.FreeHGlobal(retPtr);
            return
                Vector4.single_0 +
                Vector4.single_1 +
                Vector4.single_2;
        }

        public static unsafe float Multiple(Vec4d vec1,Vec3d vec2)
        {
            IntPtr retPtr = internalIO.mulps_internal(vec1._Vector4Ptr,vec2._Vector4Ptr);
            Vector4 Vector4 = *(Vector4*)retPtr;
            Marshal.FreeHGlobal(retPtr);
            return
                Vector4.single_0 +
                Vector4.single_1 +
                Vector4.single_2 +
                Vector4.single_3;
        }

        public static Matrix2x2 Multiple(Matrix2x2 matrix1, Matrix2x2 matrix2)
        {
            if (matrix2 == Matrix2x2.Unit)
            {
                return new Matrix2x2(
                    internalIO.load_cpy_internal(matrix1._colum0Ptr),
                    internalIO.load_cpy_internal(matrix1._colum1Ptr)
                    );
            }

            Matrix2x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix2x2 Multiple(Matrix2x3 matrix1, Matrix3x2 matrix2)
        {
            Matrix3x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix2x2 Multiple(Matrix2x4 matrix1, Matrix4x2 matrix2)
        {
            Matrix4x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix2x3 Multiple(Matrix2x2 matrix1, Matrix2x3 matrix2)
        {
            Matrix2x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x3(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix2x3 Multiple(Matrix2x3 matrix1, Matrix3x3 matrix2)
        {
            Matrix3x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x3(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix2x3 Multiple(Matrix2x4 matrix1, Matrix4x3 matrix2)
        {
            Matrix4x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x3(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix2x4 Multiple(Matrix2x2 matrix1, Matrix2x4 matrix2)
        {
            Matrix2x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x4(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum3Ptr)
                );
        }

        public static Matrix2x4 Multiple(Matrix2x3 matrix1, Matrix3x4 matrix2)
        {
            Matrix3x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x4(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum3Ptr)
                );
        }

        public static Matrix2x4 Multiple(Matrix2x4 matrix1, Matrix4x4 matrix2)
        {
            Matrix4x2 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix2x4(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum3Ptr)
                );
        }

        public static Matrix3x2 Multiple(Matrix3x2 matrix1, Matrix2x2 matrix2)
        {
            Matrix2x3 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr,matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr,matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr,matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr,matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr,matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr,matrix2._colum1Ptr)
                );
        }

        public static Matrix3x2 Multiple(Matrix3x3 matrix1, Matrix3x2 matrix2)
        {
            Matrix3x3 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix3x2 Multiple(Matrix3x4 matrix1, Matrix4x2 matrix2)
        {
            Matrix4x3 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix3x3 Multiple(Matrix3x2 matrix1, Matrix2x3 matrix2)
        {
            Matrix2x3 transpose = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x3(
                internalIO.mulps_sum_internal(transpose._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(transpose._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(transpose._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(transpose._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(transpose._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(transpose._colum1Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(transpose._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(transpose._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(transpose._colum2Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix3x3 Multiple(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            Matrix3x3 transpose = matrix2.Transpose(TransposeMode.Copy);

            return new Matrix3x3(
                internalIO.mulps_sum_internal(matrix1._colum0Ptr,transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._colum0Ptr,transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._colum0Ptr,transpose._colum2Ptr),

                internalIO.mulps_sum_internal(matrix1._colum1Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._colum1Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._colum1Ptr, transpose._colum2Ptr),

                internalIO.mulps_sum_internal(matrix1._colum2Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._colum2Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._colum2Ptr, transpose._colum2Ptr)
                );
        }

        public static Matrix3x3 Multiple(Matrix3x4 matrix1, Matrix4x3 matrix2)
        {
            Matrix3x4 transpose = matrix2.Transpose(TransposeMode.Copy);

            return new Matrix3x3(
                internalIO.mulps_sum_internal(matrix1._colum0Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._colum0Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._colum0Ptr, transpose._colum2Ptr),

                internalIO.mulps_sum_internal(matrix1._colum1Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._colum1Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._colum1Ptr, transpose._colum2Ptr),

                internalIO.mulps_sum_internal(matrix1._colum2Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._colum2Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._colum2Ptr, transpose._colum2Ptr)
                );
        }

        public static Matrix3x4 Multiple(Matrix3x2 matrix1, Matrix2x4 matrix2)
        {
            Matrix2x3 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x4(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum3Ptr)
                );
        }

        public static Matrix3x4 Multiple(Matrix3x3 matrix1, Matrix3x4 matrix2)
        {
            Matrix3x3 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x4(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum3Ptr)
                );
        }

        public static Matrix3x4 Multiple(Matrix3x4 matrix1, Matrix4x4 matrix2)
        {
            Matrix4x3 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix3x4(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum3Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum2Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum3Ptr)
                );
        }

        public static Matrix4x2 Multiple(Matrix4x2 matrix1, Matrix2x2 matrix2)
        {
            Matrix2x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix4x2 Multiple(Matrix4x3 matrix1, Matrix3x2 matrix2)
        {
            Matrix3x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix4x2 Multiple(Matrix4x4 matrix1, Matrix4x2 matrix2)
        {

            Matrix4x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x2(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),

                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum1Ptr)
                );
        }

        public static Matrix4x3 Multiple(Matrix4x2 matrix1, Matrix2x3 matrix2)
        {
            Matrix2x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x3(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix4x3 Multiple(Matrix4x3 matrix1, Matrix3x3 matrix2)
        {

            Matrix3x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x3(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix4x3 Multiple(Matrix4x4 matrix1, Matrix4x3 matrix2)
        {

            Matrix4x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x3(
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum0Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum1Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum2Ptr, matrix2._colum2Ptr),

                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum0Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum1Ptr),
                internalIO.mulps_sum_internal(trans._colum3Ptr, matrix2._colum2Ptr)
                );
        }

        public static Matrix4x4 Multiple(Matrix4x2 matrix1, Matrix2x4 matrix2)
        {
            Matrix2x4 transpose = matrix1.Transpose(TransposeMode.Copy);
            return new Matrix4x4(
                internalIO.mulps_sum_internal(matrix2._colum0Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum0Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum0Ptr, transpose._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum0Ptr, transpose._colum3Ptr),

                internalIO.mulps_sum_internal(matrix2._colum1Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum1Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum1Ptr, transpose._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum1Ptr, transpose._colum3Ptr),

                internalIO.mulps_sum_internal(matrix2._colum2Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum2Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum2Ptr, transpose._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum2Ptr, transpose._colum3Ptr),

                internalIO.mulps_sum_internal(matrix2._colum3Ptr, transpose._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum3Ptr, transpose._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum3Ptr, transpose._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum3Ptr, transpose._colum3Ptr)
                );
        }

        public static Matrix4x4 Multiple( Matrix4x3 matrix1, Matrix3x4 matrix2)
        {
            Matrix3x4 trans = matrix1.Transpose(TransposeMode.Copy);

            return new Matrix4x4(
                internalIO.mulps_sum_internal(matrix2._colum0Ptr,trans._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum0Ptr,trans._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum0Ptr,trans._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum0Ptr,trans._colum3Ptr),

                internalIO.mulps_sum_internal(matrix2._colum1Ptr,trans._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum1Ptr,trans._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum1Ptr,trans._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum1Ptr,trans._colum3Ptr),

                internalIO.mulps_sum_internal(matrix2._colum2Ptr,trans._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum2Ptr,trans._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum2Ptr,trans._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum2Ptr,trans._colum3Ptr),

                internalIO.mulps_sum_internal(matrix2._colum3Ptr,trans._colum0Ptr),
                internalIO.mulps_sum_internal(matrix2._colum3Ptr,trans._colum1Ptr),
                internalIO.mulps_sum_internal(matrix2._colum3Ptr,trans._colum2Ptr),
                internalIO.mulps_sum_internal(matrix2._colum3Ptr,trans._colum3Ptr)
                );
        }

        /*

        public static Matrix4x4 Multiple( Matrix4x4 matrix1, Matrix4x4 matrix2)
        {
            matrix1.InternalTranspose();

            return new Matrix4x4(
                internalIO.mulps_sum_internal(matrix1._transpose.colum0Ptr,matrix2._matMap.colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum0Ptr,matrix2._matMap.colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum0Ptr,matrix2._matMap.colum2Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum0Ptr,matrix2._matMap.colum3Ptr),

                internalIO.mulps_sum_internal(matrix1._transpose.colum1Ptr,matrix2._matMap.colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum1Ptr,matrix2._matMap.colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum1Ptr,matrix2._matMap.colum2Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum1Ptr,matrix2._matMap.colum3Ptr),

                internalIO.mulps_sum_internal(matrix1._transpose.colum2Ptr,matrix2._matMap.colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum2Ptr,matrix2._matMap.colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum2Ptr,matrix2._matMap.colum2Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum2Ptr,matrix2._matMap.colum3Ptr),

                internalIO.mulps_sum_internal(matrix1._transpose.colum3Ptr,matrix2._matMap.colum0Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum3Ptr,matrix2._matMap.colum1Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum3Ptr,matrix2._matMap.colum2Ptr),
                internalIO.mulps_sum_internal(matrix1._transpose.colum3Ptr,matrix2._matMap.colum3Ptr)
                );

        }
        

        /**/
    }
}
