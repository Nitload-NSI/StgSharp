//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixKernel.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public unsafe struct MatrixKernel
    {

        [FieldOffset(0)] internal fixed byte Buffer[64];
        [FieldOffset(0)] internal fixed double FP64Buffer[8];
        [FieldOffset(0)] internal fixed float FP32Buffer[16];
        [FieldOffset(0)] public M128 XMM0;
        [FieldOffset(16)] public M128 XMM1;
        [FieldOffset(32)] public M128 XMM2;
        [FieldOffset(48)] public M128 XMM3;
        [FieldOffset(0)] public M256 YMM0;
        [FieldOffset(32)] public M256 YMM1;
        [FieldOffset(64)] public M512 ZMM0;

        public MatrixKernel()
        {
            Unsafe.SkipInit(out this);
        }

        public MatrixKernel(bool clear)
        {
            if (!clear) {
                Unsafe.SkipInit(out this);
            }
        }

        public static MatrixKernel Fp32Unit
        {
            get
            {
                MatrixKernel kernel = new(true);
                kernel.FP32Buffer[0] = 1f;
                kernel.FP32Buffer[3] = 1f;
                kernel.FP32Buffer[8] = 1f;
                kernel.FP32Buffer[15] = 1f;
                return kernel;
            }
        }

        public ref T UnsafeIndex<T>(int column, int row) where T: unmanaged, INumber<T>
        {
            ref M128 col = ref Unsafe.As<byte, M128>(ref Buffer[0]);
            col = ref Unsafe.Add(ref col, column);
            return ref col.Member<T>(row);
        }

    }
}
