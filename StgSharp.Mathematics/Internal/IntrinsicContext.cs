//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="IntrinsicContext"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
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
        Pivot = 8,

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

        private MatrixElementType(int mask)
        {
            Unsafe.SkipInit(out this);
            Unsafe.SkipInit(out Size);
            Unsafe.SkipInit(out IntrinsicNode);
            this.Mask = mask;
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
        public readonly void Call(MatrixIntrinsicHandle index, MatrixParallelTask* task)
        {
            nint ptr = this[(int)index];
            ((delegate* unmanaged[Cdecl]<MatrixParallelTask*, void>)ptr)(task);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly nint Get(MatrixIntrinsicHandle index)
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
                             ulong scalar)
        {
            nint ptr = this[(int)index];
            ((delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, ulong, void>)ptr)(left, right,
                                                                                                        ans, scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly nint Get(MatrixIntrinsicHandle index)
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
