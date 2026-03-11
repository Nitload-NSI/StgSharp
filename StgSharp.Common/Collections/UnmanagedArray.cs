//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedArray"
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Common.Collections
{
    /// <summary>
    ///   Zero-overhead view over a contiguous region of unmanaged memory. Does not own the memory —
    ///   caller is responsible for lifetime management.
    /// </summary>
    /// <typeparam name="T">
    ///   Element type. Must be unmanaged.
    /// </typeparam>
    public readonly unsafe struct UnmanagedArray<T> where T : unmanaged
    {

        private static readonly nint ElementSize = sizeof(T);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnmanagedArray(
               T* pointer,
               int length
        )
        {
            Address = pointer;
            Length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnmanagedArray(
               nuint address,
               int length
        )
        {
            Address = (T*)address;
            Length = length;
        }

        /// <summary>
        ///   Indexed access. No bounds check in Release builds.
        /// </summary>
        public ref T this[
                     int index
        ]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Debug.Assert((uint)index < (uint)Length,
                    "UnmanagedArray index out of range");
                return ref Address[index];
            }
        }

        /// <summary>
        ///   Raw pointer to the first element.
        /// </summary>
        public T* Address
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            init;
        }

        /// <summary>
        ///   Element count.
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            init;
        }

        /// <summary>
        ///   Total byte size of the array (for allocation calculations).
        /// </summary>
        public int ByteSize => Length * sizeof(T);

        /// <summary>
        ///   Wraps the array as a <see cref="Span{T}" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return new Span<T>(Address, Length);
        }

        /// <summary>
        ///   Wraps a sub-range as a <see cref="Span{T}" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan(
                       int start,
                       int length
        )
        {
            Debug.Assert((uint)start + (uint)length <= (uint)Length,
                "UnmanagedArray.AsSpan range out of bounds");
            return new Span<T>(Address + start, length);
        }

        /// <summary>
        ///   Zero-fill all elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Unsafe.InitBlock(Address, 0, (uint)(Length * sizeof(T)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(
                   T* ptr
        )
        {
            int offset = (int)(((nint)ptr - (nint)Address) / ElementSize);
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(ptr));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, Length, nameof(ptr));
            return offset;
        }

        /// <summary>
        ///   Returns a pointer to element at <paramref name="index" />. Useful for feeding SIMD
        ///   load instructions (e.g. Sse2.LoadVector128).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T* PtrAt(
                  int index
        )
        {
            Debug.Assert((uint)index < (uint)Length,
                "UnmanagedArray.PtrAt index out of range");
            return Address + index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int UncheckedIndexOf(
                   T* ptr
        )
        {
            return (int)(((nint)ptr - (nint)Address) / ElementSize);
        }

    }
}
