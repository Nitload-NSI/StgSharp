//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="IntrinsicContext"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
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
    internal unsafe struct IntrinsicContext
    {

        public delegate* unmanaged[Cdecl]<char*, int, int> city_hash_simplify;              //city_hash_simplify
        public delegate* unmanaged[Cdecl]<void*, void*, void*, long, void> f32_buffer_add;             //f32_buffer_add
        public delegate* unmanaged[Cdecl]<void*, ScalarPacket*,long, void> f32_buffer_fill;                //f32_buffer_fill
        public delegate* unmanaged[Cdecl]<void*, void*, ScalarPacket*, long, void> f32_buffer_scalar_mul;                //f32_buffer_s_m
        public delegate* unmanaged[Cdecl]<void*, void*, void*, long, void> f32_buffer_sub;             //f32_kernel_sub
        public delegate* unmanaged[Cdecl]<void*, void*, void> f32_buffer_transpose;                //f32_kernel_transpose
        public delegate* unmanaged[Cdecl]<void*, void*, int, int, int, int, void> f32_build_panel;              //f32_build_panel
        public delegate* unmanaged[Cdecl]<Vector4*, Vector4*, void> f32_normalize_3;                //f32_normalize_3
        public delegate* unmanaged[Cdecl]<void*, void*, void*, void> f32_panel_fma;               //f32_panel_fma
        public delegate* unmanaged[Cdecl]<void*, void*, int, int, int, int, void> f32_store_panel; //f32_store_panel
        public delegate* unmanaged[Cdecl]<int, ulong> factorial_simd;               //factorial_simd
        public delegate* unmanaged[Cdecl]<char*, int, int, int> index_pair;             //index_pair
        public delegate* unmanaged[Cdecl]<void*, void*, ScalarPacket*, long, void> variant ;              //variant 

        public IntrinsicContext()
        {
            Unsafe.SkipInit(out this);
        }

    }
}