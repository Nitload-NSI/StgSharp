// -----------------------------------------------------------------------------
// file="CapacityFixedBag"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace StgSharp.Collections
{
    public sealed class CapacityFixedBag<T> : IEnumerable<T>
    {

        private readonly T[] _buffer;

        public CapacityFixedBag(
               int capacity
        )
        {
            _buffer = new T[capacity];
        }

        public int Count { get; set; }

        public int Capacity => _buffer.Length;

        public void Add(
                    T item
        )
        {
            if (Count >= _buffer.Length) {
                throw new InvalidOperationException("Bag capacity exceeded.");
            }
            _buffer[Count] = item;
            Count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++) {
                yield return _buffer[i];
            }
        }

        public void Remove(
                    T item
        )
        {
            int index = Array.IndexOf(_buffer, item, 0, Count);
            if (index >= 0)
            {
                _buffer[index] = _buffer[Count - 1];
                _buffer[Count - 1] = default!;
                Count--;
            }
        }

        public void RemoveAt(
                    int index
        )
        {
            if (index >= 0)
            {
                _buffer[index] = _buffer[Count - 1];
                _buffer[Count - 1] = default!;
                Count--;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
