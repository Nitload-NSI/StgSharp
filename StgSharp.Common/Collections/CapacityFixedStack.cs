//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CapacityFixedStack"
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
using System.Runtime.CompilerServices;

namespace StgSharp.Collections
{
    [CollectionBuilder(typeof(CapacityFixedStackBuilder), nameof(CapacityFixedStackBuilder.Create))]
    public class CapacityFixedStack<T> : IEnumerable<T>
    {

        private readonly T[] _values;
        private int _index;

        public CapacityFixedStack(int size)
        {
            _values = new T[size];
            _index = -1;
        }

        public bool IsEmpty => _index == -1;

        public int Count => _index + 1;

        public int Capacity => _values.Length;

        public ReadOnlySpan<T> AsSpan()
        {
            return _values.AsSpan(0, _index + 1);
        }

        public void Clear()
        {
            Array.Clear(_values, 0, _values.Length);
            _index = -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _index; i >= 0; i--) {
                yield return _values[i];
            }
        }

        public T Peek()
        {
            return _index >= 0 ? _values[_index] : throw new InvalidOperationException("Stack is empty.");
        }

        public T Pop()
        {
            if (_index >= 0)
            {
                T item = _values[_index];
                _index--;
                return item;
            } else
            {
                throw new InvalidOperationException("Stack is empty.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> PopRange(int count)
        {
            int i = int.Min(count, _index) - 1;
            return _values[0..i];
        }

        public void Push(T item)
        {
            _values[++_index] = _index < _values.Length - 1 ?
                                item :
                                throw new InvalidOperationException("Stack is full.");
        }

        public void PushRange(ReadOnlySpan<T> span)
        {
            if (_index + span.Length >= _values.Length) {
                throw new InvalidOperationException("Not enough space in the stack to push the entire span.");
            }

            Span<T> sp = _values.AsSpan(_index + 1, span.Length);
            span.CopyTo(sp);
            _index += span.Length;
        }

        public  bool TryPop(out T value)
        {
            if (_index >= 0)
            {
                T item = _values[_index];
                _index--;
                value = item;
                return true;
            } else
            {
                value = default!;
                return false;
            }
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

        public static CapacityFixedStack<T> Create<T>(ReadOnlySpan<T> values)
        {
            CapacityFixedStack<T> stack = new(values.Length);
            Span<T> sp = stack.GetValueLayout();
            values.CopyTo(sp);
            return stack;
        }

    }
}