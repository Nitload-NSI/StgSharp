//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute"
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
using StgSharp.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public partial class MatrixCompute
    {

        internal static unsafe void ComputeFmaPanelF32(
                                    MatrixParallelTaskPackage* p
        )
        {
            int f32 = (int)MatrixElementType.F32;
            MatrixKernel* leftBuffer = (MatrixKernel*)p->Left;
            MatrixKernel* rightBuffer = (MatrixKernel*)p->Right;
            MatrixKernel* ansBuffer = (MatrixKernel*)p->Answer;
            ref long offsetRef = ref Unsafe.As<int, long>(ref p->PrimTileOffset);
            ref long lengthRef = ref Unsafe.As<int, long>(ref p->PrimCount);
            int ansWidth = p->AnsPrimOffset;
            int ansHeight = p->AnsSecOffset;
            int commonK = p->LeftPrimOffset;

            M128* enumeration = stackalloc M128[1];
            enumeration->Member<int>(2) = commonK;
            enumeration->Member<int>(3) = ansHeight;

            // in this occasion size must be 1
            for (long c = offsetRef; c < offsetRef + lengthRef; c++)
            {
                int i = (int)(c / ansHeight);
                int j = (int)(c % ansHeight);
                enumeration->Member<int>(0) = i;
                enumeration->Member<int>(1) = j;
                NativeIntrinsic.Context.mat[f32].kernel_tile_fma(leftBuffer, rightBuffer, ansBuffer,
                                                                 enumeration);
            }
        }

        internal static unsafe void ComputeFmaPanelF64(
                                    MatrixParallelTaskPackage* p
        )
        {
            int f64 = (int)MatrixElementType.F64;

            // int size = 1;
            MatrixKernel* leftBuffer = (MatrixKernel*)p->Left;
            MatrixKernel* rightBuffer = (MatrixKernel*)p->Right;
            MatrixKernel* ansBuffer = (MatrixKernel*)p->Answer;
            ref long offsetRef = ref Unsafe.As<int, long>(ref p->PrimTileOffset);
            ref long lengthRef = ref Unsafe.As<int, long>(ref p->PrimCount);
            int ansWidth = p->AnsPrimOffset;
            int ansHeight = p->AnsSecOffset;
            int commonK = p->LeftPrimOffset;

            M128* enumeration = stackalloc M128[1];
            enumeration->Member<int>(2) = commonK;
            enumeration->Member<int>(3) = ansHeight;

            // in this occasion size must be 1
            for (long c = offsetRef; c < offsetRef + lengthRef; c++)
            {
                int i = (int)(c / ansHeight);
                int j = (int)(c % ansHeight);
                enumeration->Member<int>(0) = i;
                enumeration->Member<int>(1) = j;
                NativeIntrinsic.Context.mat[f64].kernel_tile_fma(leftBuffer, rightBuffer, ansBuffer,
                                                                 enumeration);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe MatrixKernel* GetKernelAddressUnsafe<T>(
                                             MatrixKernel<T>* buffer,
                                             int colLength,
                                             int x,
                                             int y
        ) where T : unmanaged, INumber<T>
        {
            return (MatrixKernel*)(buffer + (colLength * x + y));
        }

    }
}
