using StgSharp.Math;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal static unsafe partial class InternalIO
    {

        #region det

        [DllImport(InternalIO.SSC_libname, EntryPoint = "det_mat3",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe float det_mat3(Mat3* mat);

        #endregion

        #region transpose

        [DllImport(InternalIO.SSC_libname, EntryPoint = "transpose3to4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose3to4_internal(Mat3* source, Mat4* target);

        [DllImport("StgsharpGra[hic", EntryPoint = "transpose3to4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose3to3_internal(Mat3* source, Mat3* target);

        [DllImport("StgsharpGra[hic", EntryPoint = "transpose3to2",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose3to2_internal(Mat3* source, Mat2* target);

        #endregion

    }

}


namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 12 * sizeof(float), Pack = 16)]
    public struct Mat3 : IEquatable<Mat3>, IMat
    {

        [FieldOffset(0)] internal Vector4 colum0;
        [FieldOffset(4 * sizeof(float))] internal Vector4 colum1;
        [FieldOffset(8 * sizeof(float))] internal Vector4 colum2;

        [FieldOffset(0 * sizeof(float))] internal float m00;
        [FieldOffset(1 * sizeof(float))] internal float m10;
        [FieldOffset(2 * sizeof(float))] internal float m20;
        [FieldOffset(3 * sizeof(float))] internal float m30;

        [FieldOffset(4 * sizeof(float))] internal float m01;
        [FieldOffset(5 * sizeof(float))] internal float m11;
        [FieldOffset(6 * sizeof(float))] internal float m21;
        [FieldOffset(7 * sizeof(float))] internal float m31;

        [FieldOffset(8 * sizeof(float))] internal float m02;
        [FieldOffset(9 * sizeof(float))] internal float m12;
        [FieldOffset(10 * sizeof(float))] internal float m22;
        [FieldOffset(11 * sizeof(float))] internal float m32;

        public override bool Equals(object? obj)
        {
            return obj is Mat3 mat && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Mat3 other)
        {
            return colum0.Equals(other.colum0) &&
                   colum1.Equals(other.colum1) &&
                   colum2.Equals(other.colum2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Mat3 left, Mat3 right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Mat3 left, Mat3 right)
        {
            return !(left == right);
        }
    }
}
