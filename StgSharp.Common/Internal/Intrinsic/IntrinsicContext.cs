//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IntrinsicContext.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp;
using StgSharp.HighPerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Internal.Intrinsic
{
    [StructLayout( LayoutKind.Sequential )]
    internal unsafe struct IntrinsicContext
    {

        public delegate* unmanaged[Cdecl]<ColumnSet2*, ColumnSet2*, ColumnSet2*, void> add_mat_2;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, ColumnSet3*, ColumnSet3*, void> add_mat_3;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, ColumnSet4*, ColumnSet4*, void> add_mat_4;
        public delegate* unmanaged[Cdecl]<char*, int, int> city_hash_simplify;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, ColumnSet3*, float> det_mat_3;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, ColumnSet4*, float> det_mat_4;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, Vector4*, Vector4*, void> dot_31;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, Vector4*, Vector4*, void> dot_32;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, Vector4*, Vector4*, void> dot_41;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, Vector4*, Vector4*, void> dot_42;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, Vector4*, Vector4*, void> dot_43;
        public delegate* unmanaged[Cdecl]<int,ulong> factorial_simd;
        public delegate* unmanaged[Cdecl]<char*, int, int, int> index_pair;
        public delegate* unmanaged[Cdecl]<Vector4*, Vector4*, void> normalize_v3;
        public delegate* unmanaged[Cdecl]<ColumnSet2*, ColumnSet2*, ColumnSet2*, void> sub_mat_2;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, ColumnSet3*, ColumnSet3*, void> sub_mat_3;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, ColumnSet4*, ColumnSet4*, void> sub_mat_4;
        public delegate* unmanaged[Cdecl]<ColumnSet2*, ColumnSet3*, void> transpose23;
        public delegate* unmanaged[Cdecl]<ColumnSet2*, ColumnSet4*, void> transpose24;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, ColumnSet2*, void> transpose32;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, ColumnSet3*, void> transpose33;
        public delegate* unmanaged[Cdecl]<ColumnSet3*, ColumnSet4*, void> transpose34;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, ColumnSet2*, void> transpose42;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, ColumnSet3*, void> transpose43;
        public delegate* unmanaged[Cdecl]<ColumnSet4*, ColumnSet4*, void> transpose44;

    }
}
