// -----------------------------------------------------------------------------
// file="Matrix.Create"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Tlsf = global::StgSharp.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace StgSharp.Mathematics.Numeric
{
    public unsafe partial class Matrix<T>
    {

        private const int KernelSideLength = 4;

        public static Matrix<T> Create(
                                int columnLength,
                                int rowLength,
                                MatrixLayout layout,
                                TwoLayerSegregatedFitAllocator allocator
        )
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
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(requiredBytes, nameof(requiredBytes));

            Tlsf.Handle handle = allocator.Alloc((uint)requiredBytes);
            MatrixStorageHLSF<T> storage = new MatrixStorageHLSF<T>(allocator, handle);

            Matrix<T> matrix = new Matrix<T>(MatrixKernelBoarder.Create(layout), new MatrixSize(columnLength,
                                                                                                rowLength,
                                                                                                kernelColumns,
                                                                                                kernelRows), storage);

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DivideRoundUp(
                           int value,
                           int divisor
        )
        {
            return (value + (divisor - 1)) / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long TriangularKernelCount(
                            int order
        )
        {
            long n = order;
            return (n * (n + 1L)) / 2L;
        }

    }
}
