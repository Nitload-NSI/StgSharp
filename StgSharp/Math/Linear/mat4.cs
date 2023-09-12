using StgSharp;
using StgSharp.Math;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal static unsafe partial class internalIO
    {

        #region transpose

        [DllImport(SSGC_libname, EntryPoint = "transpose4to4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose4to4_internal(Mat4* source, Mat4* target);

        [DllImport(SSGC_libname, EntryPoint = "transpose4to3",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose4to3_internal(Mat4* source, Mat3* target);

        [DllImport(SSGC_libname, EntryPoint = "transpose4to2",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose4to2_internal(Mat4* source, Mat2* target);


        #endregion

        #region det

        [DllImport(SSGC_libname, EntryPoint = "det_mat4",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe extern float det_mat4(Mat4* matPtr, Mat4* transpose);

        #endregion

        #region deinit_mat4


        [DllImport(SSGC_libname, EntryPoint = "deinit_mat4",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern void deinit_mat4(Mat4* mat);

        #endregion



    }
}
namespace StgSharp.Math
{


    [StructLayout(LayoutKind.Explicit, Size = 16 * sizeof(float))]
    internal struct Mat4 : IEquatable<Mat4>
    {
        [FieldOffset(0)] internal Vector4 colum0;
        [FieldOffset(16)] internal Vector4 colum1;
        [FieldOffset(32)] internal Vector4 colum2;
        [FieldOffset(48)] internal Vector4 colum3;

        [FieldOffset(0)] public float m00;
        [FieldOffset(4)] public float m10;
        [FieldOffset(8)] public float m20;
        [FieldOffset(12)] public float m30;

        [FieldOffset(16)] public float m01;
        [FieldOffset(20)] public float m11;
        [FieldOffset(24)] public float m21;
        [FieldOffset(28)] public float m31;

        [FieldOffset(32)] public float m02;
        [FieldOffset(36)] public float m12;
        [FieldOffset(40)] public float m22;
        [FieldOffset(44)] public float m32;

        [FieldOffset(48)] public float m03;
        [FieldOffset(52)] public float m13;
        [FieldOffset(56)] public float m23;
        [FieldOffset(60)] public float m33;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Mat4(
            Vector4 colum0,
            Vector4 colum1,
            Vector4 colum2,
            Vector4 colum3
            )
        {
            this.colum0 = colum0;
            this.colum1 = colum1;
            this.colum2 = colum2;
            this.colum3 = colum3;
        }

        public override bool Equals(object? obj)
        {
            return obj is Mat4 mat && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Mat4 other)
        {
            return colum0.Equals(other.colum0) &&
                   colum1.Equals(other.colum1) &&
                   colum2.Equals(other.colum2) &&
                   colum3.Equals(other.colum3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Mat4 left, Mat4 right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Mat4 left, Mat4 right)
        {
            return !(left == right);
        }
    }
}
