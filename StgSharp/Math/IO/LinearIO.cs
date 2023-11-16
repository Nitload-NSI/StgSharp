using StgSharp.Math;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



namespace StgSharp
{
    internal static partial class InternalIO
    {
        [DllImport(InternalIO.SSC_libname, EntryPoint = "set_Vector4ptr_default",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr set_Vector4ptr_default_internal();


        #region deinit Vector4

        [DllImport(
            "msvcrt.dll",
            EntryPoint = "_aligned_free",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void deinit_internal(Vector4* ptr);

        [DllImport(
            "msvcrt.dll",
            EntryPoint = "_aligned_free",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void deinit_internal(Mat2* ptr);

        /*
        [DllImport(
            "msvcrt.dll",
            EntryPoint = "_aligned_free",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void deinit_internal(Mat3* ptr);
        */

        [DllImport(
            "msvcrt.dll",
            EntryPoint = "_aligned_free",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe void deinit_internal(Mat4* ptr);

        [DllImport(InternalIO.SSC_libname, EntryPoint = "deinitVector4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void deinitVector4_internal(ref Vector4 vec);

        [DllImport(InternalIO.SSC_libname, EntryPoint = "deinitVector4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void deinitVector4_internal(Vector4* vec);

        #endregion

        [DllImport(InternalIO.SSC_libname, EntryPoint = "deinit_matmapPtr_sse",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void deinitMatmapPtr_sse_internal(ref Vector4Map mapPtr);

        [DllImport(InternalIO.SSC_libname, EntryPoint = "deinit_matmap_sse",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern void deinitMatmap_sse_internal(ref Vector4Map mapPtr);

    }
}
