//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Matrix"
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
using StgSharp.Mathematics.Numeric;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

// using hlsfHandle = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.Mathematics.Numeric
{
    public unsafe partial class Matrix<T> where T : unmanaged, INumber<T>
    {

        private protected static readonly int t_count = 16 / sizeof(T);

        private protected static readonly int t_size = sizeof(T);
        private readonly MatrixKernelBoarder _boarder;
        private readonly MatrixSize _size;
        private readonly MatrixStorage<T> _buffer;

        private protected Matrix(
                          MatrixKernelBoarder boarder,
                          MatrixSize size,
                          MatrixStorage<T> buffer

        )
        {
            _boarder = boarder;
            _size = size;
            _buffer = buffer;
        }

        public T this[
                 int x,
                 int y
        ]
        {
            get
            {
                if (y < 0 || y > _size.ElementColumnLength || x < 0 || x > _size.ElementRowLength) {
                    throw new ArgumentOutOfRangeException($"Index [{x},{y}] is out of matrix index range.");
                }

                int
                    k_c = y / 4,
                    k_r = x / 4,
                    c = y % 4,
                    r = x % 4;
                ref MatrixKernel<T> kernel = ref GetMatrixKernel(k_c, k_r);
                return Unsafe.IsNullRef(ref kernel) ? T.Zero : kernel[c, r];
            }
            set
            {
                if (y < 0 || y > _size.ElementColumnLength || x < 0 || x > _size.ElementRowLength) {
                    throw new ArgumentOutOfRangeException($"Index [{x},{y}] is out of matrix index range.");
                }

                int
                    k_c = y / 4,
                    k_r = x / 4,
                    c = y % 4,
                    r = x % 4;
                ref MatrixKernel<T> kernel = ref GetMatrixKernel(k_c, k_r);
                if (!Unsafe.IsNullRef(ref kernel)) {
                    kernel[c, r] = value;
                }
            }
        }

        /// <summary>
        ///   Matlab style indexer to generate a new matrix
        /// </summary>
        /// <param name="column">
        ///
        /// </param>
        /// <param name="row">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///
        /// </exception>
        public unsafe Matrix<T> this[
                                ReadOnlySpan<MatrixIndex> column,
                                ReadOnlySpan<MatrixIndex> row
        ]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///   Length of the matrix in columns.
        /// </summary>
        public int ColumnLength => _size.ElementColumnLength;

        /// <summary>
        ///   Length of the matrix in rows.
        /// </summary>
        public int RowLength => _size.ElementRowLength;

        public MatrixLayout Layout { get; init; }

        internal MatrixKernel<T>* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (MatrixKernel<T>*)_buffer.BufferPointer;
        }

        protected internal int KernelColumnLength => _size.KernelColumnLength;

        protected internal int KernelRowLength => _size.KernelRowLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref MatrixKernel<T> GetMatrixKernel(
                                          int x,
                                          int y
        )
        {
            MatrixKernelEnumerator enumerator = new(_size.KernelColumnLength, _size.KernelRowLength,
                                                    x, y);
            return ref _boarder.IsInBuffer(x, y, in enumerator) ?
                       ref _buffer[_boarder.GetIndexOffset(x, y, in enumerator)] :
                       ref Unsafe.NullRef<MatrixKernel<T>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe ref T GetElementUnsafe(
                              int x,
                              int y
        )
        {
            // Caller guarantees: indices in range, layout is dense square.
            int k_c = y >> 2;
            int k_r = x >> 2;
            int c = y & 3;
            int r = x & 3;

            MatrixKernelEnumerator enumerator = new(_size.KernelColumnLength, _size.KernelRowLength,
                                                    k_c, k_r);
            ref MatrixKernel<T> kernel = ref _buffer[_boarder.GetIndexOffset(k_c, k_r, in enumerator)];
            return ref kernel[c, r];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe MatrixKernel<T>* GetKernelAddressUnsafe(
                                         int x,
                                         int y
        )
        {
            // Caller guarantees: indices in range, layout is dense square.
            MatrixKernelEnumerator enumerator = new(_size.KernelColumnLength, _size.KernelRowLength,
                                                    x, y);
            return Buffer + _boarder.GetIndexOffset(x, y, in enumerator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe ref MatrixKernel<T> GetKernelUnsafe(
                                            int x,
                                            int y
        )
        {
            // Caller guarantees: indices in range, layout is dense square.
            int k_c = y >> 2;
            int k_r = x >> 2;
            int c = y & 3;
            int r = x & 3;

            MatrixKernelEnumerator enumerator = new(_size.KernelColumnLength, _size.KernelRowLength,
                                                    k_c, k_r);
            return ref _buffer[_boarder.GetIndexOffset(k_c, k_r, in enumerator)];
        }

    }
}
