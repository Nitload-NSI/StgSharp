// -----------------------------------------------------------------------------
// file="UnmanagedStack"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Collections
{
    public unsafe class UnmanagedStack<T>(
                        T* address,
                        int capacity,
                        Action<nuint> freeHandle
    ) : IDisposable where T : unmanaged
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

        public void Push(
                    T value
        )
        {
            _array[_index] = value;
            _index++;
        }

        public bool TryPop(
                    out T value
        )
        {
            if (_index == 0)
            {
                value = default;
                return false;
            }
            value = _array[--_index];
            return true;
        }

        public bool TryPush(
                    T value
        )
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
