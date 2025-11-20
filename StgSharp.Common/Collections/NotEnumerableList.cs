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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Collections
{
    public static class LinearBagBuilder
    {

        /// <summary>
        ///   Build a <see cref="LinearBag{T}" /> from a read-only span. Capacity will exactly fit
        ///   the input.
        /// </summary>
        public static LinearBag<T> FromSpan<T>(ReadOnlySpan<T> values)
        {
            if (values.IsEmpty) {
                return new LinearBag<T>(4);
            }
            LinearBag<T> bag = new(values.Length);
            for (int i = 0; i < values.Length; i++) {
                bag.Add(values[i]);
            }
            return bag;
        }

    }

    /// <summary>
    ///   A very lightweight contiguous storage structure: unordered, not enumerable by design.
    ///   Provides O(1) Add and O(1) RemoveAt (by swapping with the tail). Intended for high-
    ///   performance scenarios where element ordering does not matter. NOT thread-safe.
    /// </summary>
    /// <typeparam name="T">
    ///   Element type.
    /// </typeparam>
    [CollectionBuilder(typeof(LinearBagBuilder), nameof(LinearBagBuilder.FromSpan))]
    public class LinearBag<T>(int capacity = 4)
    {

        // Invariant: valid elements occupy indexes [0, Count-1]. Count is the number of live elements.
        private T[] _items = new T[Math.Max(0, capacity)];

        /// <summary>
        ///   Access element by index (0..Count-1). Throws if out of range.
        /// </summary>
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

        /// <summary>
        ///   Total allocated slots.
        /// </summary>
        public int Capacity => _items.Length;

        /// <summary>
        ///   Number of currently stored elements (valid index range 0..Count-1).
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///   Adds an item (unordered). Expands internal array if needed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Count == _items.Length) {
                EnsureCapacity(Count + 1);
            }

            _items[Count] = item;
            Count++;
        }

        /// <summary>
        ///   Returns a span over the live portion of the bag (0..Count-1).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan() => new Span<T>(_items, 0, Count);

        /// <summary>
        ///   Clears all elements and zeroes out references to avoid retention. Count becomes 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(_items, 0, Count);
            Count = 0;
        }

        /// <summary>
        ///   Fast clear: only resets Count (leaves underlying array unchanged). References are NOT
        ///   released. Use only when you are certain old references can remain.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FastClear() => Count = 0;

        /// <summary>
        ///   Removes the first occurrence of value (unordered). Does nothing if not found.
        /// </summary>
        public void Remove(T value)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < Count; i++)
            {
                if (comparer.Equals(_items[i], value))
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        ///   Removes element at index by swapping the last element into its slot (unless already
        ///   last). Preserves O(1) removal at cost of ordering.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)Count) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            int last = Count - 1;
            if (index != last) {
                _items[index] = _items[last];
            }

            _items[last] = default!; // release reference / reset value
            Count = last;
        }

        /// <summary>
        ///   Ensures capacity is at least <paramref name="min" />. Doubles size when growing.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int min)
        {
            if (_items.Length >= min) {
                return;
            }

            int newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
            if (newCapacity < min) {
                newCapacity = min;
            }

            Array.Resize(ref _items, newCapacity);
        }

    }
}

