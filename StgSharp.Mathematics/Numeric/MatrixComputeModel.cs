//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixComputeModel"
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
using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Mathematics.Numeric
{
    internal static class MatrixComputeModel
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_Binary(MatrixParallelTaskPackageNonGeneric* p)
        {
            int eSize = p->ElementSize;

            // base pointer to matrix kernels
            nint l = p->Left, r = p->Right, a = p->Result;
            long

                // set beginning value to counters for primary offsets
                lCount = (long)p->LeftPrimOffset * p->LeftPrimStride,
                rCount = (long)p->RightPrimOffset * p->RightPrimStride,
                aCount = (long)p->ResultPrimOffset * p->ResultPrimStride,
                pCount = p->PrimCount,
                //edge to reset counters
                lLimit = pCount - p->LeftPrimOffset,
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> op =
                    (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)p->ComputeHandle;
            for (long i = 0; i < pCount; i++)
            {
                // update primary counters
                // if reachs edge, reset to zero
                lCount = i < lLimit ? lCount + p->LeftPrimOffset : 0;
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;

                // get kernel column pointers
                nint left = l + (nint)(lCount * eSize);
                nint right = r + (nint)(rCount * eSize);
                nint ans = a + (nint)(aCount * eSize);
                long
                    lc = (long)p->LeftSecOffset * p->LeftSecStride,
                    rc = (long)p->RightSecOffset * p->RightSecStride,
                    ac = (long)p->ResultSecOffset * p->ResultSecStride,
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
                    op(left + (nint)lc, right + (nint)rc, ans + (nint)ac);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_BinaryScalar(MatrixParallelTaskPackageNonGeneric* p)
        {
            nint l = p->Left, r = p->Right, a = p->Result;
            long
                lCount = p->LeftPrimOffset * p->LeftPrimStride,
                rCount = p->RightPrimOffset * p->RightPrimStride,
                aCount = p->ResultPrimOffset * p->ResultPrimStride,
                pCount = p->PrimCount;
            long
                lLimit = pCount - p->LeftPrimOffset,
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<nint, nint, nint, ScalarPacket*, void> op =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint, ScalarPacket*, void>)p->ComputeHandle;
            ScalarPacket* scalar = p->Scalar;
            for (long i = 0; i < pCount; i++)
            {
                lCount = i < lLimit ? lCount + p->LeftPrimOffset : 0;
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                nint
                    left = l + (nint)lCount,
                    right = r + (nint)rCount,
                    ans = a + (nint)aCount;
                long
                    lc = p->LeftSecOffset * p->LeftSecStride,
                    rc = p->RightSecOffset * p->RightSecStride,
                    ac = p->ResultSecOffset * p->ResultSecStride,
                    sCount = p->SecCount,
                    ll = sCount - p->LeftSecOffset,
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    lc = j < ll ? lc + p->LeftSecOffset : 0;
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(left + (nint)lc, right + (nint)rc, ans + (nint)ac, scalar);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_Unary(MatrixParallelTaskPackageNonGeneric* p)
        {
            nint s = p->Right, a = p->Result;
            long rCount = p->RightPrimOffset * p->RightPrimStride,
                 aCount = p->ResultPrimOffset * p->ResultPrimStride,
                 pCount = p->PrimCount;
            long rLimit = pCount - p->RightPrimOffset,
                     aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<nint, nint, void> op =
                    (delegate* unmanaged[Cdecl]<nint, nint, void>)p->ComputeHandle;
            for (long i = 0; i < pCount; i++)
            {
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                nint source = s + (nint)rCount, ans = a + (nint)aCount;
                long rc = p->RightSecOffset * p->RightSecStride,
                         ac = p->ResultSecOffset * p->ResultSecStride,
                         sCount = p->SecCount;
                long rl = sCount - p->RightSecOffset,
                         al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(source + (nint)rc, ans + (nint)ac);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_UnaryScalar(MatrixParallelTaskPackageNonGeneric* p)
        {
            nint s = p->Right, a = p->Result;
            long rCount = p->RightPrimOffset * p->RightPrimStride,
                     aCount = p->ResultPrimOffset * p->ResultPrimStride,
                     pCount = p->PrimCount;
            long rLimit = pCount - p->RightPrimOffset,
                     aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<nint, nint, ScalarPacket*, void> op =
                    (delegate* unmanaged[Cdecl]<nint, nint, ScalarPacket*, void>)p->ComputeHandle;
            ScalarPacket* scalar = p->Scalar;
            for (long i = 0; i < pCount; i++)
            {
                rCount = i < rLimit ? rCount + p->RightPrimOffset : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimOffset : 0;
                nint source = s + (nint)rCount;
                nint ans = a + (nint)aCount;
                long rc = p->RightSecOffset * p->RightSecStride,
                         ac = p->ResultSecOffset * p->ResultSecStride,
                         sCount = p->SecCount;
                long rl = sCount - p->RightSecOffset,
                         al = sCount - p->ResultSecOffset;
                for (int j = 0; j < sCount; j++)
                {
                    rc = j < rl ? rc + p->RightSecOffset : 0;
                    ac = j < al ? ac + p->ResultSecOffset : 0;
                    op(source + (nint)rc, ans + (nint)ac, scalar);
                }
            }
        }

    }
}
