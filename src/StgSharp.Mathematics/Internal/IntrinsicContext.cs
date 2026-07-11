// -----------------------------------------------------------------------------
// file="IntrinsicContext"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Numeric.Runtime;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Internal
{
    internal enum MatrixIntrinsicHandle : int
    {

        Add = 0,
        Fill = 1,
        ScalarMul = 2,
        Sub = 3,
        Transpose = 4,
        ClearPanel = 5,
        Fma = 6,
        PackFmaLeft = 7,
        PackFmaRight = 8,
        Pivot = 9,

    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct MatrixElementType
    {

        public const int IntrinsicCount = 0x0a;

        public static readonly MatrixElementType F32 = new(0x00000004);
        public static readonly MatrixElementType F64 = new(0x00010008);
        public static readonly MatrixElementType I08 = new(0x00020001);
        public static readonly MatrixElementType I16 = new(0x00030002);
        public static readonly MatrixElementType I32 = new(0x00040004);
        public static readonly MatrixElementType I64 = new(0x00050008);
        public static readonly MatrixElementType U08 = new(0x00060001);
        public static readonly MatrixElementType U16 = new(0x00070002);
        public static readonly MatrixElementType U32 = new(0x00080004);
        public static readonly MatrixElementType U64 = new(0x00090008);

        [FieldOffset(0)] internal readonly int Mask;
        [FieldOffset(2)] public readonly ushort IntrinsicNode;

        [FieldOffset(0)] public readonly ushort Size;

        private MatrixElementType(
                int mask
        )
        {
            Unsafe.SkipInit(out this);
            Unsafe.SkipInit(out Size);
            Unsafe.SkipInit(out IntrinsicNode);
            this.Mask = mask;
        }

        [Obsolete("Do not use this constructor. Use pre-defined ones from static fields.", true)]
        public MatrixElementType()
        {
            throw new NotSupportedException("Do not use this constructor. Use pre-defined ones from static fields.");
        }

        public override readonly bool Equals(
                                      object? obj
        )
        {
            return (obj is MatrixElementType type) && (Mask == type.Mask);
        }

        public override readonly int GetHashCode()
        {
            return Mask;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct IntrinsicContext
    {

        public delegate* unmanaged[Cdecl]<char*, int, int> city_hash_simplify;              // city_hash_simplify
        public delegate* unmanaged[Cdecl]<int, ulong> factorial_simd;                       // factorial_simd
        public delegate* unmanaged[Cdecl]<char*, int, int, int> index_pair;                 // index_pair
        public delegate* unmanaged[Cdecl]<Vector4*, Vector4*, void> vec_f32_normalize_3;   // f32_normalize_3
        public MatrixMultiKernelIntrinsicArray mat_mk;                                              // mat[4] in native order
        public MatrixSingleKernelIntrinsicArray mat_sk;                                              // mat[4] in native order

        public IntrinsicContext()
        {
            Unsafe.SkipInit(out this);
        }

    }

    #region mk

    [InlineArray(10)]
    internal unsafe struct MatrixMultiKernelIntrinsic
    {

        private nint handle;              //f32_panel_mul

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Call(
                             MatrixIntrinsicHandle index,
                             MatrixParallelTask* task
        )
        {
            nint ptr = this[(int)index];
            ((delegate* unmanaged[Cdecl]<MatrixParallelTask*, void>)ptr)(task);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly nint Get(
                             MatrixIntrinsicHandle index
        )
        {
            return this[(int)index];
        }

    }

    [InlineArray(MatrixElementType.IntrinsicCount)]
    internal unsafe struct MatrixMultiKernelIntrinsicArray
    {

        private MatrixMultiKernelIntrinsic e0;

    }

        #endregion

    #region sk

    [InlineArray(10)]
    internal unsafe struct MatrixSingleKernelIntrinsic
    {

        private nint handle;              //f32_panel_mul

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Call(
                             MatrixIntrinsicHandle index,
                             MatrixKernel* left,
                             MatrixKernel* right,
                             MatrixKernel* ans,
                             ulong scalar
        )
        {
            nint ptr = this[(int)index];
            ((delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, ulong, void>)ptr)(left,
                                                                                                        right,
                                                                                                        ans,
                                                                                                        scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly nint Get(
                             MatrixIntrinsicHandle index
        )
        {
            return this[(int)index];
        }

    }

    [InlineArray(MatrixElementType.IntrinsicCount)]
    internal unsafe struct MatrixSingleKernelIntrinsicArray
    {

        private MatrixSingleKernelIntrinsic e0;

    }

    #endregion

}
