//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Matrix.HLSF.cs"
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
using System.Text;
using System.Threading.Tasks;

using hlsfAllocator = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;
using hlsfHandle = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.Mathematics
{
    internal unsafe partial class HlsfMatrix<T> : Matrix<T> where T: unmanaged, INumber<T>
    {

        private readonly MatrixKernel<T>* k_buffer;
        private readonly hlsfAllocator allocator;
        private readonly hlsfHandle handle;

        internal HlsfMatrix(int column, int row, hlsfAllocator allocator)
        {
            KernelColumnLength = (column + 3) / 4;
            KernelRowLength = (row + 3) / t_count;
            this.handle = allocator.Alloc((uint)(KernelColumnLength * KernelRowLength * sizeof(MatrixKernel<T>)));
            k_buffer = (MatrixKernel<T>*)handle.Pointer;
        }

        public override ref T this[int column, int row]
        {
            get
            {
                AssertIndex(column, row);
                int kernelColumn = column / 4, c = column % 4;
                int kernelRow = row / t_count, r = row % t_count;
                ref MatrixKernel<T> kernel = ref k_buffer[(kernelColumn * kernelColumn) + kernelRow];
                return ref kernel.UnsafeIndex(c, r);
            }
        }

        public override Matrix<T> this[ReadOnlySpan<MatrixIndex> column, ReadOnlySpan<MatrixIndex> row]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        internal override unsafe MatrixKernel<T>* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => k_buffer;
        }

        public override unsafe ref MatrixKernel<T> GetMatrixKernel(int column, int row)
        {
            AssertIndex(column, row);
            int kernelColumn = column / 4, c = column % 4;
            int kernelRow = row / t_count, r = row % t_count;
            return ref k_buffer[(kernelColumn * kernelColumn) + kernelRow];
        }

        public override ref T UnsafeIndex(int column, int row)
        {
            int kernelColumn = column / 4, c = column % 4;
            int kernelRow = row / t_count, r = row % t_count;
            ref MatrixKernel<T> kernel = ref k_buffer[(kernelColumn * kernelColumn) + kernelRow];
            return ref kernel.UnsafeIndex(c, r);
        }

        ~HlsfMatrix()
        {
            allocator.Free(handle);
        }

        #region enumeration

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe MatrixSegmentEnumeration<T> GetColumnEnumeration()
        {
            return new MatrixSegmentEnumeration<T>(k_buffer, KernelColumnLength, KernelColumnLength * KernelRowLength, 1, KernelColumnLength);
        }

        public override unsafe MatrixSegmentEnumeration<T> GetRowEnumeration()
        {
            return new MatrixSegmentEnumeration<T>(k_buffer, 1, KernelColumnLength, KernelColumnLength, KernelRowLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe MatrixSegmentEnumeration<T> GetSequentialEnumeration()
        {
            return new MatrixSegmentEnumeration<T>(k_buffer, KernelRowLength * KernelColumnLength, KernelColumnLength * KernelRowLength, 1, KernelRowLength * KernelColumnLength);
        }

        #endregion
    }
}
