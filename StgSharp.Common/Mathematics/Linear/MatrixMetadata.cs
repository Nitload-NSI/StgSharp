//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixMetadata"
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
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Numeric
{
    public enum MatrixLayout
    {

        DenseRectangle,
        UpperTriangle,
        LowerTriangle,

    }

    public abstract class MatrixKernelBoarder
    {

        private static readonly MatrixKernelBoarder DenseSquareInstance = new DenseSquareKernelBoarder();
        private static readonly MatrixKernelBoarder LowerTriangleInstance = new LowerTriangleKernelBoarder();
        private static readonly MatrixKernelBoarder UpperTriangleInstance = new UpperTriangleKernelBoarder();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixKernelBoarder Create(
                                          MatrixLayout layout
        )
        {
            return layout switch
            {
                MatrixLayout.DenseRectangle => DenseSquareInstance,
                MatrixLayout.UpperTriangle => UpperTriangleInstance,
                MatrixLayout.LowerTriangle => LowerTriangleInstance,
                _ => throw new ArgumentOutOfRangeException(nameof(layout)),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract long GetIndexOffset(
                             int xKernel,
                             int yKernel,
                             in MatrixSize size
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract bool IsInBuffer(
                             int xKernel,
                             int yKernel,
                             in MatrixSize enumerator
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static long GetTriangularOrder(
                              in MatrixSize enumerator
        )
        {
            return Math.Min(enumerator.KernelColumnLength, enumerator.KernelRowLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsWithinKernelDomain(
                              in MatrixSize enumerator,
                              int xKernel,
                              int yKernel
        )
        {
            return xKernel >= 0 &&
                   yKernel >= 0 &&
                   xKernel < enumerator.KernelColumnLength &&
                   yKernel < enumerator.KernelRowLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsWithinTriangularDomain(
                              in MatrixSize enumerator,
                              int xKernel,
                              int yKernel
        )
        {
            long order = GetTriangularOrder(enumerator);
            return xKernel >= 0 && yKernel >= 0 && xKernel < order && yKernel < order;
        }

        private sealed class DenseSquareKernelBoarder : MatrixKernelBoarder
        {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override long GetIndexOffset(
                                 int xKernel,
                                 int yKernel,
                                 in MatrixSize enumerator
            )
            {
                long x = xKernel;
                long y = yKernel;
                long col = enumerator.KernelColumnLength;

                // Console.Write($"Attempt to get offset at ({x},{y}), offset is {x * col + y}  ");
                return x * col + y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override bool IsInBuffer(
                                 int xKernel,
                                 int yKernel,
                                 in MatrixSize enumerator
            )
            {
                return IsWithinKernelDomain(enumerator, xKernel, yKernel);
            }

        }

        private sealed class LowerTriangleKernelBoarder : MatrixKernelBoarder
        {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override long GetIndexOffset(
                                 int xKernel,
                                 int yKernel,
                                 in MatrixSize enumerator
            )
            {
                long order = GetTriangularOrder(enumerator);
                long x = xKernel;
                long y = yKernel;

                long prefixColumns = x * order - (x * (x - 1L)) / 2L;
                return prefixColumns + (y - x);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override bool IsInBuffer(
                                 int xKernel,
                                 int yKernel,
                                 in MatrixSize enumerator
            )
            {
                return IsWithinTriangularDomain(enumerator, xKernel, yKernel) && yKernel >= xKernel;
            }

        }

        private sealed unsafe class UpperTriangleKernelBoarder : MatrixKernelBoarder
        {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override long GetIndexOffset(
                                 int xKernel,
                                 int yKernel,
                                 in MatrixSize enumerator
            )
            {
                long x = xKernel;
                long y = yKernel;

                long prefixColumns = (x * (x + 1L)) / 2L;
                return prefixColumns + y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override bool IsInBuffer(
                                 int xKernel,
                                 int yKernel,
                                 in MatrixSize enumerator
            )
            {
                return IsWithinTriangularDomain(enumerator, xKernel, yKernel) && yKernel <= xKernel;
            }

        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MatrixSize
    {

        [FieldOffset(0)] public readonly int ElementColumnLength;
        [FieldOffset(4)] public readonly int ElementRowLength;

        [FieldOffset(8)] public readonly int KernelColumnLength;
        [FieldOffset(12)] public readonly int KernelRowLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MatrixSize(
               int elementColumnLength,
               int elementRowLength,
               int kernelColumnLength,
               int kernelRowLength
        )
        {
            ElementColumnLength = elementColumnLength;
            ElementRowLength = elementRowLength;
            KernelColumnLength = kernelColumnLength;
            KernelRowLength = kernelRowLength;
        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MatrixKernelEnumerator
    {

        [FieldOffset(0)] public int CurrentColumn;
        [FieldOffset(4)] public int CurrentRow;

        [FieldOffset(8)] public readonly int KernelColumnLength;
        [FieldOffset(12)] public readonly int KernelRowLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MatrixKernelEnumerator(
               int kernelColumnLength,
               int kernelRowLength,
               int currentColumn = 0,
               int currentRow = 0
        )
        {
            CurrentColumn = currentColumn;
            CurrentRow = currentRow;
            KernelColumnLength = kernelColumnLength;
            KernelRowLength = kernelRowLength;
        }

    }
}
