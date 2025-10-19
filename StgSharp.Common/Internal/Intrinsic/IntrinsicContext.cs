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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Internal.Intrinsic
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct IntrinsicContext
    {

        public delegate* unmanaged[Cdecl]<char*, int, int> city_hash_simplify;
        public delegate* unmanaged[Cdecl]<void*, void*, void*, void> f32_add;
        public delegate* unmanaged[Cdecl]<void*, void*, void*, void> f32_fma;
        public delegate* unmanaged[Cdecl]<void*, float, void*, void> f32_scalar_mul;
        public delegate* unmanaged[Cdecl]<void*, void*, void*, void> f32_sub;
        public delegate* unmanaged[Cdecl]<void*, void*, void> f32_transpose;
        public delegate* unmanaged[Cdecl]<int, ulong> factorial_simd;
        public delegate* unmanaged[Cdecl]<char*, int, int, int> index_pair;
        public delegate* unmanaged[Cdecl]<Vector4*, Vector4*, void> normalize_3;

        public IntrinsicContext()
        {
            Unsafe.SkipInit(out this);
        }

    }
}
