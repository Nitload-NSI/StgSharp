//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Matrix.cs"
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using hlsfAllocator = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;
using hlsfHandle = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.Mathematics
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

        public int ColumnLength { get; init; }

        public int RowLength { get; init; }

        public static MatrixAllocation DefaultAllocation { get; set; } = MatrixAllocation.GC;

        protected int KernelColumnLength { get; init; }

        protected int KernelRowLength { get; init; }

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
        public abstract MatrixSegmentEnumeration GetColumnEnumeration();

        public abstract ref MatrixKernel GetMatrixKernel(int column, int row);

        public abstract MatrixSegmentEnumeration GetRowEnumeration();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract MatrixSegmentEnumeration GetSequentialEnumeration();

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

    public readonly unsafe ref struct MatrixSegmentEnumeration(
                                      MatrixKernel* source,
                                      int stride,
                                      int size,
                                      int secondaryStride,
                                      int secondarySize)
    {

        private readonly MatrixKernel* _source = source;
        private readonly int _secondarySize = secondarySize;
        private readonly int _secondaryStride = secondaryStride;

        private readonly int _size = size;
        private readonly int _stride = stride;

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in this);
        }

        public ref struct Enumerator(in MatrixSegmentEnumeration enumeration)
        {

            private MatrixKernel* _current = enumeration._source - enumeration._stride;
            private readonly MatrixKernel* _end = enumeration._source + enumeration._size;
            private readonly int _secondarySize = enumeration._secondarySize;
            private readonly int _secondaryStride = enumeration._secondaryStride;
            private readonly int _stride = enumeration._stride;

            public readonly Segment Current
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => new(_current, _secondarySize, _secondaryStride);
            }

            public bool MoveNext()
            {
                _current += _stride;
                return _current < _end;
            }

        }

    }

    public readonly unsafe ref struct Segment(MatrixKernel* source, int size, int stride)
    {

        private readonly MatrixKernel* _source = source;
        private readonly int _size = size;
        private readonly int _stride = stride;

        public ref MatrixKernel this[int index]
        {
            get
            {
                if (index < 0 || index >= _size) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return ref _source[index * _stride];
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_source, _size, _stride);
        }

        public ref struct Enumerator(MatrixKernel* source, int count, int stride)
        {

            private readonly MatrixKernel* _end = source + (count * stride);
            private readonly int _stride = stride;

            public MatrixKernel* Current
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get;
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                private set;
            } = source - stride;

            public bool MoveNext()
            {
                Current += _stride;
                return Current < _end;
            }

        }

    }
}
