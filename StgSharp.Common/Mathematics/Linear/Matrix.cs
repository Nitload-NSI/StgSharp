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
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using hlsfAllocator = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

// using hlsfHandle = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.Mathematics.Numeric
{
    public abstract unsafe partial class Matrix<T> where T: unmanaged, INumber<T>
    {

        private protected readonly int t_count = 16 / sizeof(T);

        private protected readonly int t_size = sizeof(T);

        private protected Matrix() { }

        public abstract ref T this[int column, int row]
        {
            get;
        }

        public abstract unsafe Matrix<T> this[ReadOnlySpan<MatrixIndex> column, ReadOnlySpan<MatrixIndex> row]
        {
            get;
            set;
        }

        public static hlsfAllocator? DefaultAllocator
        {
            get
            {
                ArgumentNullException.ThrowIfNull(field, nameof(DefaultAllocator));
                return null;
            }
            set;
        } = null;

        /// <summary>
        ///   Length of the matrix in columns.
        /// </summary>
        public int ColumnLength { get; init; }

        /// <summary>
        ///   Length of the matrix in rows.
        /// </summary>
        public int RowLength { get; init; }

        public static MatrixAllocation DefaultAllocation { get; set; } = MatrixAllocation.GC;

        internal abstract MatrixKernel<T>* Buffer { get; }

        protected internal int KernelColumnLength { get; init; }

        protected internal int KernelRowLength { get; init; }

        public static Matrix<T> FromDefault(int columnLength, int rowLength)
        {
            return DefaultAllocation switch
            {
                MatrixAllocation.GC => FromGC(columnLength, rowLength),
                MatrixAllocation.HLSF => FromHlsf(columnLength, rowLength, DefaultAllocator!),
                _ => throw new NotSupportedException("This allocation is not supported."),
            };
        }

        public static Matrix<T> FromGC(int columnLength, int rowLength)
        {
            return new GCMatrix<T>(columnLength, rowLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix<T> FromHlsf(int columnLength, int rowLength, hlsfAllocator allocator)
        {
            return new HlsfMatrix<T>(columnLength, rowLength, allocator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract MatrixSegmentEnumeration<T> GetColumnEnumeration();

        public abstract ref MatrixKernel<T> GetMatrixKernel(int column, int row);

        public abstract MatrixSegmentEnumeration<T> GetRowEnumeration();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract MatrixSegmentEnumeration<T> GetSequentialEnumeration();

        [Obsolete("This method does not contain boarder check, so be careful on size or use indexer instead.", false)]
        public abstract ref T UnsafeIndex(int column, int row);

        private protected void AssertIndex(int column, int row)
        {
            if (column < 0 || column >= ColumnLength) {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            if (row < 0 || row >= RowLength) {
                throw new ArgumentOutOfRangeException(nameof(row));
            }
        }

    }

    public readonly unsafe ref struct MatrixSegmentEnumeration<T>(
                                      MatrixKernel<T>* source,
                                      int primStride,
                                      int primSize,
                                      int secondaryStride,
                                      int secondarySize,
                                      int primOffset = 0,
                                      int secondaryOffset = 0)
        where T: unmanaged, INumber<T>
    {

        private readonly MatrixKernel<T>* _source = source;
        internal readonly int PrimOffset = primOffset;

        internal readonly int PrimSize = primSize;
        internal readonly int PrimStride = primStride;
        internal readonly int SecondaryOffset = secondaryOffset;
        internal readonly int SecondarySize = secondarySize;
        internal readonly int SecondaryStride = secondaryStride;

        internal readonly MatrixKernel<T>* Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _source;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in this);
        }

        public ref struct Enumerator(in MatrixSegmentEnumeration<T> enumeration)
        {

            private readonly MatrixKernel<T>* _source = enumeration._source;
            private int _indexCount = -1;
            private readonly int _stride = enumeration.PrimStride;
            private readonly long _bound = (long)enumeration.PrimSize * enumeration.PrimStride;
            private long _index = (long)enumeration.PrimOffset * enumeration.PrimStride;

            private readonly MatrixSegmentEnumeration<T> _enumeration;

            public readonly MatrixSegment<T> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => new(
                    _source + _index,
                    _enumeration.SecondarySize,
                    _enumeration.SecondaryStride,
                    _enumeration.PrimOffset);
            }

            public bool MoveNext()
            {
                _indexCount++;
                _index += _stride;
                if (_index >= _bound) {
                    _index = 0;
                }
                return _indexCount >= _enumeration.PrimSize;
            }

        }

    }

    public readonly unsafe ref struct MatrixSegment<T>(MatrixKernel<T>* source, int size, int stride, int offset)
        where T: unmanaged, INumber<T>
    {

        private readonly MatrixKernel<T>* _source = source;
        private readonly int _offset = offset;
        private readonly int _secondarySize = size;
        private readonly int _size = size;
        private readonly int _stride = stride;

        public ref MatrixKernel<T> this[int index]
        {
            get
            {
                if (index < 0 || index >= _size) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return ref _source[index * _stride];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator()
        {
            return new Enumerator(in this);
        }

        public ref struct Enumerator(in MatrixSegment<T> segment)
        {

            private readonly int _end = segment._size;

            private int _indexCount = -1;
            private readonly long _bound = (long)segment._size * segment._stride;
            private long _index = (long)segment._offset * segment._stride;
            private readonly MatrixSegment<T> _segment = segment;

            public readonly MatrixKernel<T>* Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _segment._source + _index;
            }

            public bool MoveNext()
            {
                _index += _segment._stride;
                _indexCount++;
                if (_index >= _bound) {
                    _index = 0;
                }

                return _indexCount < _end;
            }

        }

    }
}
