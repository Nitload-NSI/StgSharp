// -----------------------------------------------------------------------------
// file="ConcurrentFlexibleArray"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Collections
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
