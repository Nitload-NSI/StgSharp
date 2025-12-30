//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Matrix.Create"
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
using StgSharp.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public unsafe partial class Matrix<T>
    {

        private const int KernelSideLength = 4;

        public static Matrix<T> Create(
                                int columnLength,
                                int rowLength,
                                MatrixLayout layout,
                                HybridLayerSegregatedFitAllocator allocator)
        {
            ArgumentNullException.ThrowIfNull(allocator);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columnLength);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowLength);

            int kernelColumns = DivideRoundUp(columnLength, KernelSideLength);
            int kernelRows = DivideRoundUp(rowLength, KernelSideLength);

            if (layout is MatrixLayout.UpperTriangle or MatrixLayout.LowerTriangle)
            {
                if (kernelColumns != kernelRows) {
                    throw new ArgumentException("Triangular layouts require square matrices.");
                }
            }

            long kernelCount = layout switch
            {
                MatrixLayout.DenseRectangle => (long)kernelColumns * kernelRows,
                MatrixLayout.UpperTriangle => TriangularKernelCount(kernelColumns),
                MatrixLayout.LowerTriangle => TriangularKernelCount(kernelRows),
                _ => throw new ArgumentOutOfRangeException(nameof(layout))
            };

            long requiredBytes = kernelCount * MatrixKernel<T>.Size;
            if (requiredBytes <= 0 || requiredBytes > uint.MaxValue) {
                throw new ArgumentOutOfRangeException(nameof(requiredBytes), "Requested matrix exceeds allocator capacity.");
            }

            HybridLayerSegregatedFitAllocationHandle handle = allocator.Alloc((uint)requiredBytes);
            MatrixStorageHLSF<T> storage = new MatrixStorageHLSF<T>(allocator, handle);

            Matrix<T> matrix = new Matrix<T>(MatrixKernelBoarder.Create(layout), new MatrixSize(columnLength,
                                                                                                rowLength,
                                                                                                kernelColumns,
                                                                                                kernelRows), storage);

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DivideRoundUp(int value, int divisor)
        {
            return (value + (divisor - 1)) / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long TriangularKernelCount(int order)
        {
            long n = order;
            return (n * (n + 1L)) / 2L;
        }

    }
}
