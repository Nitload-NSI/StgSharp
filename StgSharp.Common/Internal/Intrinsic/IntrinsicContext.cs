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
using StgSharp.HighPerformance;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace StgSharp.Internal.Intrinsic
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MatrixIntrinsic
    {

        public delegate* unmanaged[Cdecl]<void*, void*, void*, long, void> buffer_add;             //f32_buffer_add
        public delegate* unmanaged[Cdecl]<void*, ScalarPacket*, long, void> buffer_fill;                //f32_buffer_fill
        public delegate* unmanaged[Cdecl]<void*, void*, ScalarPacket*, long, void> buffer_scalar_mul;                //f32_buffer_s_m
        public delegate* unmanaged[Cdecl]<void*, void*, void*, long, void> buffer_sub;             //f32_kernel_sub
        public delegate* unmanaged[Cdecl]<void*, void*, void> buffer_transpose;                //f32_kernel_transpose
        public delegate* unmanaged[Cdecl]<MatrixPanel*, MatrixKernel*, int, int, int, int, void> build_panel;              //f32_build_panel
        public delegate* unmanaged[Cdecl]<MatrixPanel*, void> clear_panel;                      //f32_clear_panel
        public delegate* unmanaged[Cdecl]<void*, void*, void*, void> kernel_fma;               //f32_panel_fma
        public delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, M128*, void> kernel_tile_fma;               //f32_panel_fma
        public delegate* unmanaged[Cdecl]<void*, ulong, ulong, ulong, void> pivot;               //f32_panel_mul
        public delegate* unmanaged[Cdecl]<void*, void*, int, int, int, int, void> store_panel; //f32_store_panel

    }

    [InlineArray(4)]
    internal unsafe struct MatrixIntrinsicArray
    {

        private MatrixIntrinsic e0;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct IntrinsicContext
    {

        public delegate* unmanaged[Cdecl]<char*, int, int> city_hash_simplify;              // city_hash_simplify
        public delegate* unmanaged[Cdecl]<int, ulong> factorial_simd;                       // factorial_simd
        public delegate* unmanaged[Cdecl]<char*, int, int, int> index_pair;                 // index_pair
        public delegate* unmanaged[Cdecl]<Vector4*, Vector4*, void> vec_f32_normalize_3;   // f32_normalize_3
        public MatrixIntrinsicArray mat;                                              // mat[4] in native order

        public IntrinsicContext()
        {
            Unsafe.SkipInit(out this);
        }

    }
}