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
using StgSharp.HighPerformance;
using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Mathematics.Numeric
{
    internal static class MatrixComputeModel
    {

        internal static unsafe void BufferComputeBinary(MatrixParallelTaskPackage* p)
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;

            ((delegate* unmanaged[Cdecl]<void*, void*, void*, long, void>)p->ComputeHandle)((void*)(p->Left + offset),
                                                                                            (void*)(p->Right + offset),
                                                                                            (void*)(p->Result + offset),
                                                                                            count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void BufferComputeNoOperatorScalar(MatrixParallelTaskPackage* p)
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;
            ((delegate* unmanaged[Cdecl]<void*, ScalarPacket*,long, void>)p->ComputeHandle)((void*)(p->Result + offset),
                                                                                                     p->Scalar, count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void BufferComputeUnaryScalar(MatrixParallelTaskPackage* p)
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;

            ((delegate* unmanaged[Cdecl]<void*, void*, long, void>)p->ComputeHandle)((void*)(p->Right + offset),
                                                                                           (void*)(p->Result + offset),
                                                                                           count);
            MatrixParallelFactory.Release(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_Binary(MatrixParallelTaskPackage* p)
        {
            int eSize = p->ElementSize;

            // base pointer to matrix kernels
            nint l = p->Left, r = p->Right, a = p->Result;
            long

                // set beginning value to counters for primary offsets
                lCount = (long)p->LeftPrimOffset * p->LeftPrimStride,
                rCount = (long)p->RightPrimOffset * p->RightPrimStride,
                aCount = (long)p->ResultPrimOffset * p->ResultPrimStride,
                pCount = int.Min(p->PrimCount, p->PrimColumnCountInTile + p->PrimTileOffset) /*_count of tile may be too much*/,
                //edge to reset counters
                lLimit = pCount - p->LeftPrimOffset,
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> op =
                    (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)p->ComputeHandle;
            for (long i = p->PrimTileOffset % pCount; i < pCount; i++)
            {
                // update primary counters
                // if reaches edge, reset to zero
                lCount = i < lLimit ? lCount + p->LeftPrimStride : 0;
                rCount = i < rLimit ? rCount + p->RightPrimStride : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimStride : 0;

                // get kernel column pointers
                nint left = l + (nint)(lCount * eSize);
                nint right = r + (nint)(rCount * eSize);
                nint ans = a + (nint)(aCount * eSize);
                long
                    lc = (long)p->LeftSecOffset * p->LeftSecStride,
                    rc = (long)p->RightSecOffset * p->RightSecStride,
                    ac = (long)p->ResultSecOffset * p->ResultSecStride,
                    sCount = int.Min(p->SecCount, p->PrimColumnCountInTile * p->SecTileOffset);
                long
                    ll = sCount - p->LeftSecOffset,
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (long j = p->SecTileOffset % sCount; j < sCount; j++)
                {
                    lc = j < ll ? lc + p->LeftSecStride : 0;
                    rc = j < rl ? rc + p->RightSecStride : 0;
                    ac = j < al ? ac + p->ResultSecStride : 0;
                    op(left + (nint)lc, right + (nint)rc, ans + (nint)ac);
                }
            }
            MatrixParallelFactory.Release(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_BinaryScalar(MatrixParallelTaskPackage* p)
        {
            int eSize = p->ElementSize;

            // base pointer to matrix kernels
            nint l = p->Left, r = p->Right, a = p->Result;
            long

                // set beginning value to counters for primary offsets
                lCount = (long)p->LeftPrimOffset * p->LeftPrimStride,
                rCount = (long)p->RightPrimOffset * p->RightPrimStride,
                aCount = (long)p->ResultPrimOffset * p->ResultPrimStride,
                pCount = int.Min(p->PrimCount, p->PrimColumnCountInTile + p->PrimTileOffset) /*_count of tile may be too much*/,
                //edge to reset counters
                lLimit = pCount - p->LeftPrimOffset,
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, ScalarPacket*, void> op =
                    (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, ScalarPacket*, void>)p->ComputeHandle;
            for (long i = p->PrimTileOffset % pCount; i < pCount; i++)
            {
                // update primary counters
                // if reaches edge, reset to zero
                lCount = i < lLimit ? lCount + p->LeftPrimStride : 0;
                rCount = i < rLimit ? rCount + p->RightPrimStride : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimStride : 0;

                // get kernel column pointers
                nint left = l + (nint)(lCount * eSize);
                nint right = r + (nint)(rCount * eSize);
                nint ans = a + (nint)(aCount * eSize);
                long
                    lc = (long)p->LeftSecOffset * p->LeftSecStride,
                    rc = (long)p->RightSecOffset * p->RightSecStride,
                    ac = (long)p->ResultSecOffset * p->ResultSecStride,
                    sCount = int.Min(p->SecCount, p->PrimColumnCountInTile * p->SecTileOffset);
                long
                    ll = sCount - p->LeftSecOffset,
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (long j = p->SecTileOffset % sCount; j < sCount; j++)
                {
                    lc = j < ll ? lc + p->LeftSecStride : 0;
                    rc = j < rl ? rc + p->RightSecStride : 0;
                    ac = j < al ? ac + p->ResultSecStride : 0;
                    op(left + (nint)lc, right + (nint)rc, ans + (nint)ac, p->Scalar);
                }
            }
            MatrixParallelFactory.Release(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_Unary(MatrixParallelTaskPackage* p)
        {
            int eSize = p->ElementSize;

            // base pointer to matrix kernels
            nint r = p->Right, a = p->Result;
            long

                // set beginning value to counters for primary offsets
                rCount = (long)p->RightPrimOffset * p->RightPrimStride,
                aCount = (long)p->ResultPrimOffset * p->ResultPrimStride,
                pCount = int.Min(p->PrimCount, p->PrimColumnCountInTile + p->PrimTileOffset) /*_count of tile may be too much*/,
                //edge to reset counters
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> op =
                    (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)p->ComputeHandle;
            for (long i = p->PrimTileOffset % pCount; i < pCount; i++)
            {
                // update primary counters
                // if reaches edge, reset to zero
                rCount = i < rLimit ? rCount + p->RightPrimStride : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimStride : 0;

                // get kernel column pointers
                nint right = r + (nint)(rCount * eSize);
                nint ans = a + (nint)(aCount * eSize);
                long
                    rc = (long)p->RightSecOffset * p->RightSecStride,
                    ac = (long)p->ResultSecOffset * p->ResultSecStride,
                    sCount = int.Min(p->SecCount, p->PrimColumnCountInTile * p->SecTileOffset);
                long
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (long j = p->SecTileOffset % sCount; j < sCount; j++)
                {
                    rc = j < rl ? rc + p->RightSecStride : 0;
                    ac = j < al ? ac + p->ResultSecStride : 0;
                    op(right + (nint)rc, ans + (nint)ac);
                }
            }
            MatrixParallelFactory.Release(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void Compute_UnaryScalar(MatrixParallelTaskPackage* p)
        {
            int eSize = p->ElementSize;

            // base pointer to matrix kernels
            nint r = p->Right, a = p->Result;
            long

                // set beginning value to counters for primary offsets
                rCount = (long)p->RightPrimOffset * p->RightPrimStride,
                aCount = (long)p->ResultPrimOffset * p->ResultPrimStride,
                pCount = int.Min(p->PrimCount, p->PrimColumnCountInTile + p->PrimTileOffset) /*_count of tile may be too much*/,
                //edge to reset counters
                rLimit = pCount - p->RightPrimOffset,
                aLimit = pCount - p->ResultPrimOffset;
            delegate* unmanaged[Cdecl]<IntPtr, IntPtr, ScalarPacket*, void> op =
                    (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, ScalarPacket*, void>)p->ComputeHandle;
            for (long i = p->PrimTileOffset % pCount; i < pCount; i++)
            {
                // update primary counters
                // if reaches edge, reset to zero
                rCount = i < rLimit ? rCount + p->RightPrimStride : 0;
                aCount = i < rLimit ? aCount + p->ResultPrimStride : 0;

                // get kernel column pointers
                nint right = r + (nint)(rCount * eSize);
                nint ans = a + (nint)(aCount * eSize);
                long
                    rc = (long)p->RightSecOffset * p->RightSecStride,
                    ac = (long)p->ResultSecOffset * p->ResultSecStride,
                    sCount = int.Min(p->SecCount, p->PrimColumnCountInTile * p->SecTileOffset);
                long
                    rl = sCount - p->RightSecOffset,
                    al = sCount - p->ResultSecOffset;
                for (long j = p->SecTileOffset % sCount; j < sCount; j++)
                {
                    rc = j < rl ? rc + p->RightSecStride : 0;
                    ac = j < al ? ac + p->ResultSecStride : 0;
                    op(right + (nint)rc, ans + (nint)ac, p->Scalar);
                }
            }
            MatrixParallelFactory.Release(p);
        }

    }
}