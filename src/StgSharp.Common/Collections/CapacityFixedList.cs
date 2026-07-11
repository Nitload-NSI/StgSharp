// -----------------------------------------------------------------------------
// file="CapacityFixedList"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Collections
{
    public static class CapacityFixedListBuilder
    {

        internal static CapacityFixedList<T> Create<T>(
                                             ReadOnlySpan<T> values
        )
        {
            return new CapacityFixedList<T>(values);
        }

    }

    [CollectionBuilder(typeof(CapacityFixedListBuilder), nameof(CapacityFixedListBuilder.Create))]
    public class CapacityFixedList<T> : IList<T>
    {

        private T[] _values;
        private readonly int _capacity;
        private int _index;

        public CapacityFixedList(
               int size
        )
        {
            _values = new T[size];
            _capacity = size;
        }

        public CapacityFixedList(
               ReadOnlySpan<T> values
        )
        {
            _values = values.ToArray();
        }

        public T this[
                 int index
        ]
        {
            get => index < _index ?
                   _values[index] :
                   throw new ArgumentOutOfRangeException(nameof(index));
            set => _values[index] =
                   index < _index ? value : throw new ArgumentOutOfRangeException(nameof(index));
        }

        public bool IsReadOnly => false;

        public int Count => _index + 1;

        public void Add(
                    T item
        )
        {
            if (_index < _capacity - 1)
            {
                _index++;
                this[_index] = item;
            }
        }

        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                Array.Fill(_values, default);
            }
            _index = 0;
        }

        public bool Contains(
                    T item
        )
        {
            if (item is null) {
                return false;
            }
            for (int i = 0; i <= _index; i++)
            {
                if (item.Equals(_values[i])) {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(
                    T[] array,
                    int arrayIndex
        )
        {
            _values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i <= _index; i++) {
                yield return _values[i];
            }
        }

        public int IndexOf(
                   T item
        )
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

        public bool Remove(
                    T item
        )
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

        public void RemoveAll(
                    Predicate<T> predicate
        )
        {
            if (predicate is null) {
                return;
            }
            for (int i = _index; i > 0; i--)
            {
                if (predicate(_values[i]))
                {
                    _values[i] = _values[_index];
                    _index--;
                }
            }
        }

        public void RemoveAt(
                    int index
        )
        {
            if (index <= _index)
            {
                Array.Copy(_values, index + 1, _values, index, _index - index);
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    _values[_index] = default!;
                }
                _index--;
            }
        }

        public void Resize(
                    int newSize
        )
        {
            Array.Resize(ref _values, newSize);
        }

        internal Span<T> ToSpan()
        {
            return _values;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

        void IList<T>.Insert(
                      int index,
                      T item
        )
        {
            throw new NotSupportedException();
        }

    }//------------------------------------- End of Class ------------------------------------------
}
