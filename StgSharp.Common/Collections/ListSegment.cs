//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ListSegment"
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StgSharp.Collections
{
    /// <summary>
    ///   Part of a collection. This contains a direct reference to _origin collection, so all
    ///   change to _origin collection will be reflected in this object.
    /// </summary>
    /// <typeparam name="T">
    ///
    /// </typeparam>
    public struct ListSegment<T> : ICollection<T>
    {

        private int _begin;
        private int _length;
        private List<T> _collection;

        public ListSegment(List<T> collection, int begin, int length)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentOutOfRangeException.ThrowIfLessThan(begin, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(begin + length, collection.Count);
            _collection = collection;
            _begin = begin;
            _length = length;
        }

        public T this[int indexValue]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfLessThan(indexValue, 0);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(indexValue, _length);
                return _collection[indexValue + _begin];
            }
        }

        public T this[Index i]
        {
            get
            {
                int offset = i.IsFromEnd ? _length - i.Value : i.Value;
                ArgumentOutOfRangeException.ThrowIfLessThan(offset, 0);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, _length);
                return _collection[offset + _begin];
            }
        }

        public bool IsReadOnly => true;

        public int Count => _length;

        public bool Contains(T item)
        {
            int index = _collection.IndexOf(item);
            return index >= _begin && index < _length + index;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);
            Span<T> span = CollectionsMarshal.AsSpan<T>(_collection).Slice(_begin, _length);
            Span<T> target = new Span<T>(array, arrayIndex, _length);
            span.CopyTo(target);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _begin; i < _length; i++) {
                yield return this[i];
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
