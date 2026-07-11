// -----------------------------------------------------------------------------
// file="CapacityFixedStack"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.Collections
{
    [CollectionBuilder(typeof(CapacityFixedStackBuilder), nameof(CapacityFixedStackBuilder.Create))]
    public class CapacityFixedStack<T> : IEnumerable<T>
    {

        private readonly T[] _values;

        // _index points to the next free slot (top empty position). Therefore Count == _index.
        private int _index;

        public CapacityFixedStack(
               int size
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(size);
            _values = new T[size];
            _index = 0; // empty stack
        }

        public T? First => Volatile.Read(ref _index) > 0 ? _values[0] : default;

        public bool IsEmpty => Volatile.Read(ref _index) == 0;

        public int Count => Volatile.Read(ref _index);

        public int Capacity => _values.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> AsSpan()
        {
            return _values.AsSpan(0, _index);
        }

        public void Clear()
        {
            if (_index > 0)
            {
                Array.Clear(_values, 0, _index);
                _index = 0;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _index - 1; i >= 0; i--) {
                yield return _values[i];
            }
        }

        public T Peek()
        {
            if (_index == 0) {
                throw new InvalidOperationException("Stack is empty.");
            }
            return _values[_index - 1];
        }

        public T Pop()
        {
            if (_index == 0) {
                throw new InvalidOperationException("Stack is empty.");
            }
            _index--;
            T item = _values[_index];
            _values[_index] = default!; // Clear reference for GC
            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] PopRange(
                   int count
        )
        {
            if (count <= 0 || _index == 0) {
                return Array.Empty<T>();
            }
            int n = Math.Min(count, _index);
            int start = _index - n;
            T[] ret = new T[n];
            Array.Copy(_values, start, ret, 0, n);
            Array.Clear(_values, start, n);
            _index -= n;
            return ret;
        }

        public void Push(
                    T item
        )
        {
            if (_index >= _values.Length) {
                throw new InvalidOperationException("Stack is full.");
            }
            _values[_index] = item;
            _index++;
        }

        public void PushRange(
                    ReadOnlySpan<T> span
        )
        {
            if (span.Length == 0) {
                return;
            }
            if (_index + span.Length > _values.Length) {
                throw new InvalidOperationException("Not enough space in the stack to push the entire span.");
            }
            span.CopyTo(_values.AsSpan(_index, span.Length));
            _index += span.Length;
        }

        public bool TryPop(
                    out T value
        )
        {
            if (_index == 0)
            {
                value = default!;
                return false;
            }
            _index--;
            value = _values[_index];
            _values[_index] = default!;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<T> GetValueLayout()
        {
            return _values.AsSpan();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public static class CapacityFixedStackBuilder
    {

        public static CapacityFixedStack<T> Create<T>(
                                            ReadOnlySpan<T> values
        )
        {
            CapacityFixedStack<T> stack = new(values.Length);
            stack.PushRange(values); // Push in order
            return stack;
        }

    }
}