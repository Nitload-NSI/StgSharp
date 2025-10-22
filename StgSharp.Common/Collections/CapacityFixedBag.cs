//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CapacityFixedBag"
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace StgSharp.Collections
{
    public sealed class CapacityFixedBag<T> : IEnumerable<T>
    {

        private readonly T[] _buffer;

        public CapacityFixedBag(int capacity)
        {
            _buffer = new T[capacity];
        }

        public int Count { get; set; }

        public int Capacity => _buffer.Length;

        public void Add(T item)
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

        public void Remove(T item)
        {
            int index = Array.IndexOf(_buffer, item, 0, Count);
            if (index >= 0)
            {
                _buffer[index] = _buffer[Count - 1];
                _buffer[Count - 1] = default!;
                Count--;
            }
        }

        public void RemoveAt(int index)
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
