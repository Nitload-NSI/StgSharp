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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using hlsfAllocator = global::StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

namespace StgSharp.Mathematics.Numeric
{
    public static class Matrix
    {

        public static hlsfAllocator? DefaultAllocator
        {
            get
            {
                ArgumentNullException.ThrowIfNull(field, nameof(DefaultAllocator));
                return null;
            }
            set;
        } = null;

        public static MatrixAllocation DefaultAllocation { get; set; } = MatrixAllocation.GC;

        public static Matrix<T> FromDefault<T>(int columnLength, int rowLength) where T : unmanaged, INumber<T>
        {
            return DefaultAllocation switch
            {
                MatrixAllocation.GC => FromGC<T>(columnLength, rowLength),
                MatrixAllocation.HLSF => FromHlsf<T>(columnLength, rowLength, DefaultAllocator!),
                _ => throw new NotSupportedException("This allocation is not supported."),
            };
        }

        public static Matrix<T> FromGC<T>(int columnLength, int rowLength) where T : unmanaged, INumber<T>
        {
            return new GCMatrix<T>(columnLength, rowLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix<T> FromHlsf<T>(int columnLength, int rowLength, hlsfAllocator allocator)
            where T : unmanaged, INumber<T>
        {
            return new HlsfMatrix<T>(columnLength, rowLength, allocator);
        }

    }
}
