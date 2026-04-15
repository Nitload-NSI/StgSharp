//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ConcurrentFlexibaleArray"
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
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Common.Collections
{
    public class ConcurrentFlexibleArray<T>(
                 int capacity = 4
    )
    {

        private volatile T[] _array = new T[capacity];
        private readonly ReaderWriterLockSlim _rwLock = new();

        public T this[
                 int index
        ]
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    if ((uint)index >= (uint)_array.Length) {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }

                    return _array[index];
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            set
            {
                if ((uint)index >= (uint)_array.Length)
                {
                    _rwLock.EnterWriteLock();
                    try
                    {
                        if ((uint)index >= (uint)_array.Length)
                        {
                            int newSize = Math.Max(_array.Length * 2, index + 1);
                            T[] newArray = new T[newSize];
                            Array.Copy(_array, newArray, _array.Length);
                            _array = newArray;
                            _array[index] = value;
                        }
                    }
                    finally
                    {
                        _rwLock.ExitWriteLock();
                    }
                } else
                {
                    _rwLock.EnterReadLock();
                    try
                    {
                        _array[index] = value;
                    }
                    finally
                    {
                        _rwLock.ExitReadLock();
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<T> AsSpan()
        {
            return _array.AsSpan();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ReleaseSpanOperation()
        {
            _rwLock.ExitReadLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RequestSpanOperation()
        {
            _rwLock.EnterReadLock();
        }

    }
}
