//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ConcurrentCapacityFixedStack"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.Collections
{
    /// <summary>
    ///   Lock-free, fixed-capacity stack implemented on top of an array using atomic index
    ///   operations. - No linked list; only array slots - Uses CAS on a shared top index to
    ///   reserve/release slots - Per-slot state flag to safely publish/consume values under races
    /// </summary>
    public class ConcurrentCapacityFixedStack<T> : IEnumerable<T>
    {

        private readonly int[] _states; // 0 = empty, 1 = full
        private readonly T[] _values;
        private readonly int _capacity;

        // Number of full slots (observable count)
        private int _count;

        // Next push position (0.._capacity). Only modified via CAS.
        private int _top;

        public ConcurrentCapacityFixedStack(int capacity)
        {
            if (capacity <= 0) {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _capacity = capacity;
            _values = new T[capacity];
            _states = new int[capacity];
            _top = 0;
            _count = 0;
        }

        public bool IsEmpty => Count == 0;

        public int Capacity => _capacity;

        public int Count => Volatile.Read(ref _count);

        public void Clear()
        {
            // Pop all elements to return slots to empty state
            while (TryPop(out _)) { }
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] snap = ToArraySnapshot();
            for (int i = 0; i < snap.Length; i++) {
                yield return snap[i];
            }
        }

        public T Peek()
        {
            if (TryPeek(out T? v)) {
                return v;
            }

            throw new InvalidOperationException("Stack is empty.");
        }

        public T Pop()
        {
            if (TryPop(out T value)) {
                return value;
            }

            throw new InvalidOperationException("Stack is empty.");
        }

        public T[] PopRange(int count)
        {
            if (count <= 0) {
                return [];
            }

            T[] result = new T[count];
            int i = 0;
            for (; i < count; i++)
            {
                if (!TryPop(out result[i]))
                {
                    break;
                }
            }
            if (i == count) {
                return result;
            }

            if (i == 0) {
                return [];
            }

            T[] trimmed = new T[i];
            Array.Copy(result, trimmed, i);
            return trimmed;
        }

        public void Push(T item)
        {
            SpinWait sw = default;
            while (true)
            {
                int oldTop = Volatile.Read(ref _top);
                if (oldTop >= _capacity) {
                    throw new InvalidOperationException("Stack is full.");
                }

                if (Interlocked.CompareExchange(ref _top, oldTop + 1, oldTop) != oldTop)
                {
                    sw.SpinOnce();
                    continue; // failed to reserve slot
                }

                // We exclusively own slot oldTop. Publish value then state.
                _values[oldTop] = item;
                Volatile.Write(ref _states[oldTop], 1); // publish
                _ = Interlocked.Increment(ref _count);
                return;
            }
        }

        public void PushRange(ReadOnlySpan<T> span)
        {
            for (int i = 0; i < span.Length; i++) {
                Push(span[i]);
            }
        }

        /// <summary>
        ///   Best-effort snapshot for enumeration; not strongly consistent under concurrent
        ///   mutations.
        /// </summary>
        public T[] ToArraySnapshot()
        {
            int expected = Math.Min(Volatile.Read(ref _count), _capacity);
            if (expected == 0) {
                return Array.Empty<T>();
            }

            T[] snap = new T[expected];
            int copied = 0;
            int idx = Volatile.Read(ref _top) - 1;
            while (idx >= 0 && copied < expected)
            {
                if (Volatile.Read(ref _states[idx]) == 1) {
                    snap[copied++] = _values[idx];
                }
                idx--;
            }

            if (copied == snap.Length) {
                return snap;
            }

            T[] trimmed = new T[copied];
            Array.Copy(snap, trimmed, copied);
            return trimmed;
        }

        public bool TryPeek(out T value)
        {
            value = default!;
            SpinWait sw = default;

            while (true)
            {
                int top = Volatile.Read(ref _top);
                if (top <= 0) {
                    return false;
                }
                int idx = top - 1;

                // If not yet published or racing, retry/spin
                if (Volatile.Read(ref _states[idx]) == 0)
                {
                    if (Volatile.Read(ref _top) != top)
                    {
                        continue; // head moved, retry
                    }
                    sw.SpinOnce();
                    continue;
                }

                T v = _values[idx];

                // Validate consistency
                if (Volatile.Read(ref _top) == top && Volatile.Read(ref _states[idx]) == 1)
                {
                    value = v;
                    return true;
                }
            }
        }

        public bool TryPop(out T value)
        {
            value = default!;
            SpinWait sw = default;

            while (true)
            {
                int oldTop = Volatile.Read(ref _top);
                if (oldTop <= 0)
                {
                    return false; // empty
                }

                int newTop = oldTop - 1; // index to consume
                if (Interlocked.CompareExchange(ref _top, newTop, oldTop) != oldTop)
                {
                    sw.SpinOnce();
                    continue; // failed to reserve element
                }

                // If producer hasn't published yet, roll back our reservation and retry.
                if (Volatile.Read(ref _states[newTop]) == 0)
                {
                    // attempt to restore top; ok if it fails due to concurrent operations
                    _ = Interlocked.CompareExchange(ref _top, newTop + 1, newTop);
                    sw.SpinOnce();
                    continue;
                }

                value = _values[newTop];
                _values[newTop] = default!; // avoid ref retention
                Volatile.Write(ref _states[newTop], 0); // mark empty
                _ = Interlocked.Decrement(ref _count);
                return true;
            }
        }

        public int TryPopRange(Span<T> destination)
        {
            if (destination.IsEmpty) {
                return 0;
            }

            int i = 0;
            for (; i < destination.Length; i++)
            {
                if (!TryPop(out destination[i]))
                {
                    break;
                }
            }
            return i;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
