//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CapacityFixedList"
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
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Commom.Collections
{
    public static class CapacityFixedListBuilder
    {

        internal static CapacityFixedList<T> Create<T>(ReadOnlySpan<T> values)
        {
            return new CapacityFixedList<T>(values);
        }

    }

    [CollectionBuilder(typeof(CapacityFixedListBuilder), nameof(CapacityFixedListBuilder.Create))]
    public class CapacityFixedList<T> : IList<T>
    {

        private T[] _values;
        private int capacity, _index;

        public CapacityFixedList(int size)
        {
            _values = new T[size];
        }

        public CapacityFixedList(ReadOnlySpan<T> values)
        {
            _values = values.ToArray();
        }

        public T this[int index]
        {
            get => index < _index ? _values[index] : throw new ArgumentOutOfRangeException(nameof(index));
            set
            {
                if (index < _index)
                {
                    _values[index] = value;
                } else
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        public bool IsReadOnly => false;

        public int Count => _index + 1;

        public void Add(T item)
        {
            if (_index < capacity - 1)
            {
                _index++;
                this[_index] = item;
            }
        }

        public void Clear()
        {
            _values = new T[capacity];
            _index = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i <= _index; i++)
            {
                if (item.Equals(_values[i])) {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i <= _index; i++) {
                yield return _values[i];
            }
        }

        public int IndexOf(T item)
        {
            T v;
            for (int i = 0; i <= _index; i++)
            {
                if ((v = _values[i]) != null && v.Equals(item)) {
                    return i;
                }
            }
            return -1;
        }

        public bool Remove(T item)
        {
            T v;
            for (int i = 0; i <= _index; i++)
            {
                if ((v = _values[i]) != null && v.Equals(item))
                {
                    _values[i] = _values[_index];
                    _index--;
                    return true;
                }
            }
            return false;
        }

        public void RemoveAll(Predicate<T> predicate)
        {
            for (int i = _index; i > 0; i--)
            {
                if (predicate(_values[i]))
                {
                    _values[i] = _values[_index];
                    _index--;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (index <= _index)
            {
                _values[index] = _values[_index];
                _index--;
            }
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref _values, newSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

    }//------------------------------------- End of Class ------------------------------------------
}
