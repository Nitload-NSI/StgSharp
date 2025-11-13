//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="NotEnumerableList"
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
using System.Runtime.CompilerServices;

namespace StgSharp.Collections
{
    public static class LinearBagBuilder
    {

        public static LinearBag<T> FromSpan<T>(ReadOnlySpan<T> values)
        {
            if (values.IsEmpty || values.Length == 0) {
                return new LinearBag<T>(4);
            }
            LinearBag<T> bag = new(values.Length);
            for (int i = 0; i < values.Length; i++) {
                bag.Add(values[i]);
            }
            return bag;
        }

    }

    [CollectionBuilder(typeof(LinearBagBuilder), nameof(LinearBagBuilder.FromSpan))]
    public class LinearBag<T>(int capacity = 4)
    {

        private T[] _items = new T[capacity];

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (uint)index >= (uint)Count ? throw new ArgumentOutOfRangeException(nameof(index)) : _items[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if ((uint)index >= (uint)Count) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                _items[index] = value;
            }
        }

        public int Capacity => _items.Length;

        public int Count { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Count == _items.Length) {
                EnsureCapacity(Count + 1);
            }

            _items[Count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return new Span<T>(_items, 0, Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(_items, 0, Count);
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FastClear()
        {
            // 只重置size，不清理数组内容
            Count = 0;
        }

        public void Remove(T value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i]?.Equals(value) ?? false)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)Count) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            int last = Count - 1;
            if (index != last)
            {
                _items[index] = _items[last]; // swap back from tail
            }

            _items[last] = default!; // clear last slot to avoid ref retention
            Count = last;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
                if (newCapacity < min) {
                    newCapacity = min;
                }

                Array.Resize(ref _items, newCapacity);
            }
        }

    }
}

