//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SlabAllocator.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance.Memory;
using StgSharp.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe class SlabAllocator<T> : IDisposable where T: unmanaged, ISlabAllocatable
    {

        private T* _buffer;

        private readonly BufferExpansionLock _lock = new();
        private readonly ConcurrentStackBuffer<ulong> _stack;
        private readonly int _elementSize;
        private ulong _capacity;
        private ulong _highestAllocated;

        public SlabAllocator(int initialCapacity)
        {
            _elementSize = Unsafe.SizeOf<T>();
            _capacity = (nuint)initialCapacity;
            _highestAllocated = 0;

            _buffer = (T*)NativeMemory.Alloc((nuint)_capacity * (nuint)_elementSize);
            int initCount = _capacity switch
            {
                <= 4 => 4,
                > 64 => 64,
                _ => (int)_capacity / 4
            };
            Span<ulong> span = stackalloc ulong[initCount];
            for (int i = 0; i < initCount; i++) {
                span[i] = (ulong)i;
            }
            _stack = new ConcurrentStackBuffer<ulong>(span);
            _highestAllocated += (ulong)initCount;
        }

        public ulong Allocate()
        {
            if (_stack.TryPop(out ulong index)) {
                return index;
            }
            while (true)
            {
                index = Volatile.Read(ref _highestAllocated);
                _lock.EnterMetaDataRead();
                try
                {
                    if (index >= _capacity)
                    {
                        _lock.ExitMetaDataRead();
                        _lock.EnterExpansionProcess();
                        try
                        {
                            _capacity = Volatile.Read(ref _capacity) * 2;
                            nuint newSize = (nuint)_capacity * (nuint)_elementSize;
                            _lock.EnterBufferCopy();
                            try
                            {
                                _buffer = (T*)NativeMemory.Realloc(_buffer, newSize);
                            }
                            finally
                            {
                                _lock.ExitBufferCopy();
                            }
                        }
                        finally
                        {
                            _lock.ExitExpansionProcess();
                        }
                    }
                    _ = Interlocked.Increment(ref _highestAllocated);
                    return index;
                }
                finally
                {
                    if (_lock.IsThreadReadingMetaData) {
                        _lock.ExitMetaDataRead();
                    }
                }
            }
        }

        public void Dispose()
        {
            _stack.Dispose();
            NativeMemory.Free(_buffer);
            _lock.Dispose();
        }

        public void EnterBufferReading()
        {
            _lock.EnterBufferRead();
        }

        public void ExitBufferReading()
        {
            _lock.ExitBufferRead();
        }

        public void Free(nuint index)
        {
            _stack.Push(index);
        }

    }
}
