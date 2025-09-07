//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Execute.cs"
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
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    internal static partial class MatrixParallel
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_Binary(MatrixParallelTaskPackage* p)
        {
            MatrixKernel* l = p->Left, r = p->Right, a = p->Result;
            long
                lCount = p->LeftPrimOffset * p->LeftPrimStride,
                rCount = p->RightPrimOffset * p->RightPrimStride,
                aCount = p->ResultPrimOffset * p->ResultPrimStride,
                pCount = p->PrimCount;
            long
                lLimit = pCount - p->LeftPrimOffset,
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, void> op =
                    (delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, void>)p->ComputeHandle;
            for (long i = 0; i < pCount; i++)
            {
                lCount = i < lLimit ? lCount + p->LeftPrimOffset : 0;
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                MatrixKernel* left = l + lCount;
                MatrixKernel* right = r + rCount;
                MatrixKernel* ans = a + aCount;
                long
                    lc = p->LeftSecOffset * p->LeftSecStride,
                    rc = p->RightSecOffset * p->RightSecStride,
                    ac = p->ResultSecOffset * p->ResultSecStride,
                    sCount = p->SecCount;
                long
                    ll = sCount - p->LeftSecOffset,
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    lc = j < ll ? lc + p->LeftSecOffset : 0;
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(left + lc, right + rc, ans + ac);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_BinaryScalar(MatrixParallelTaskPackage* p)
        {
            MatrixKernel* l = p->Left, r = p->Right, a = p->Result;
            long
                lCount = p->LeftPrimOffset * p->LeftPrimStride,
                rCount = p->RightPrimOffset * p->RightPrimStride,
                aCount = p->ResultPrimOffset * p->ResultPrimStride,
                pCount = p->PrimCount;
            long
                lLimit = pCount - p->LeftPrimOffset,
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, ScalarPacket*, void> op =
                    (delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, MatrixKernel*, ScalarPacket*, void>)p->ComputeHandle;
            ScalarPacket* scalar = p->Scalar;
            for (long i = 0; i < pCount; i++)
            {
                lCount = i < lLimit ? lCount + p->LeftPrimOffset : 0;
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                MatrixKernel* left = l + lCount;
                MatrixKernel* right = r + rCount;
                MatrixKernel* ans = a + aCount;
                long
                    lc = p->LeftSecOffset * p->LeftSecStride,
                    rc = p->RightSecOffset * p->RightSecStride,
                    ac = p->ResultSecOffset * p->ResultSecStride,
                    sCount = p->SecCount;
                long
                    ll = sCount - p->LeftSecOffset,
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    lc = j < ll ? lc + p->LeftSecOffset : 0;
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(left + lc, right + rc, ans + ac, scalar);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_Unary(MatrixParallelTaskPackage* p)
        {
            MatrixKernel* s = p->Right, a = p->Result;
            long rCount = p->RightPrimOffset * p->RightPrimStride,
                     aCount = p->ResultPrimOffset * p->ResultPrimStride,
                     pCount = p->PrimCount;
            long rLimit = pCount - p->RightPrimOffset,
                     aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, void> op =
                    (delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, void>)p->ComputeHandle;
            for (long i = 0; i < pCount; i++)
            {
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                MatrixKernel* source = s + rCount;
                MatrixKernel* ans = a + aCount;
                long rc = p->RightSecOffset * p->RightSecStride,
                         ac = p->ResultSecOffset * p->ResultSecStride,
                         sCount = p->SecCount;
                long rl = sCount - p->RightSecOffset,
                         al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(source + rc, ans + ac);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_UnaryScalar(MatrixParallelTaskPackage* p)
        {
            MatrixKernel* s = p->Right, a = p->Result;
            long rCount = p->RightPrimOffset * p->RightPrimStride,
                     aCount = p->ResultPrimOffset * p->ResultPrimStride,
                     pCount = p->PrimCount;
            long rLimit = pCount - p->RightPrimOffset,
                     aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, ScalarPacket*, void> op =
                    (delegate* unmanaged[Cdecl]<MatrixKernel*, MatrixKernel*, ScalarPacket*, void>)p->ComputeHandle;
            ScalarPacket* scalar = p->Scalar;
            for (long i = 0; i < pCount; i++)
            {
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                MatrixKernel* source = s + rCount;
                MatrixKernel* ans = a + aCount;
                long rc = p->RightSecOffset * p->RightSecStride,
                         ac = p->ResultSecOffset * p->ResultSecStride,
                         sCount = p->SecCount;
                long rl = sCount - p->RightSecOffset,
                         al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(source + rc, ans + ac, scalar);
                }
            }
        }

    }
}
