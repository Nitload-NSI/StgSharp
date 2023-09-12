using StgSharp.Math;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal static unsafe partial class internalIO
    {
        #region add

        [DllImport(SSGC_libname, EntryPoint = "add_Mat2",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern Mat2* add_mat2(Mat2* left, Mat2* right);


        #endregion

        #region sub

        [DllImport(SSGC_libname, EntryPoint = "sub_mat2",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe Mat2* sub_mat2(Mat2* left, Mat2* right);

        #endregion

        #region transpose


        [DllImport(SSGC_libname, EntryPoint = "transmpose2to4",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void Transpose2to4_internal(Mat2* source, Mat4* target);

        [DllImport(SSGC_libname, EntryPoint = "transmpose2to3",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void Transpose2to3_internal(Mat2* source, Mat3* target);

        #endregion

        #region deinit mat2

        [DllImport(SSGC_libname, EntryPoint = "deinit_mat2",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void deinit_mat2(Mat2* mat);

        #endregion

    }
}

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 12 * sizeof(float), Pack = 16)]
    internal struct Mat2 : IEquatable<Mat2>
    {

        [FieldOffset(0)] internal Vector4 colum0;
        [FieldOffset(4 * sizeof(float))] internal Vector4 colum1;

        [FieldOffset(0 * sizeof(float))] internal float m00;
        [FieldOffset(1 * sizeof(float))] internal float m10;
        [FieldOffset(2 * sizeof(float))] internal float m20;
        [FieldOffset(3 * sizeof(float))] internal float m30;

        [FieldOffset(4 * sizeof(float))] internal float m01;
        [FieldOffset(5 * sizeof(float))] internal float m11;
        [FieldOffset(6 * sizeof(float))] internal float m21;
        [FieldOffset(7 * sizeof(float))] internal float m31;

        internal Mat2(
            Vector4 c0,
            Vector4 c1
            )
        {
            colum0 = c0;
            colum1 = c1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is Mat2 mat && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Mat2 other)
        {
            return colum0.Equals(other.colum0) &&
                   colum1.Equals(other.colum1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Mat2 left, Mat2 right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Mat2 left, Mat2 right)
        {
            return !(left == right);
        }
    }
}
