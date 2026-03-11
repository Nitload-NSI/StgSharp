//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedStack"
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Collections
{
    public unsafe class UnmanagedStack<T>(T* address, int capacity, Action<nuint> freeHandle) : IDisposable
        where T: unmanaged
    {

        private T* _array = address;
        private readonly Action<nuint> _freeHandle = freeHandle;
        private readonly int _capacity = capacity;
        private int _index;

        public void Clear()
        {
            _index = 0;
        }

        public void Dispose()
        {
            if (_array != null)
            {
                _freeHandle((nuint)_array);
                _array = null;
            }
            GC.SuppressFinalize(this);
        }

        public T Pop()
        {
            if (_index == 0) {
                throw new InvalidOperationException("Stack is empty.");
            }
            return _array[--_index];
        }

        public void Push(T value)
        {
            _array[_index] = value;
            _index++;
        }

        public bool TryPop(out T value)
        {
            if (_index == 0)
            {
                value = default;
                return false;
            }
            value = _array[--_index];
            return true;
        }

        public bool TryPush(T value)
        {
            if (_index >= _capacity) {
                return false;
            }
            _array[_index] = value;
            _index++;
            return true;
        }

    }
}
