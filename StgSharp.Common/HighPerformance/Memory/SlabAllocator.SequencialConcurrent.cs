//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SlabAllocator.SequencialConcurrent"
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
using StgSharp.Threading;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public sealed unsafe class ConcurrentSequentialSlabAllocator<T> : SlabAllocator<T> where T: unmanaged
    {

        private T* _buffer;

        private readonly BufferExpansionLock _lock = new();
        private readonly ConcurrentBufferStack<nuint> _stack;
        private readonly int _elementSize;
        private nuint _highestAllocated;
        private ulong _capacity;

        public ConcurrentSequentialSlabAllocator(nuint initialCapacity)
        {
            _elementSize = Unsafe.SizeOf<T>();
            _capacity = initialCapacity;
            _highestAllocated = 0;

            _buffer = (T*)NativeMemory.Alloc((nuint)_capacity * (nuint)_elementSize);
            int initCount = _capacity switch
            {
                <= 4 => 4,
                > 64 => 64,
                _ => (int)_capacity / 4
            };
            Span<nuint> span = stackalloc nuint[initCount];
            for (int i = 0; i < initCount; i++) {
                span[i] = (nuint)i;
            }
            _stack = BufferStackBuilder.CreateConcurrent(span);
            _highestAllocated += (nuint)initCount;
        }

        internal T* BasePointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _buffer;
        }

        public override nuint Allocate()
        {
            if (_stack.TryPop(out nuint index)) {
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
                    _ = Interlocked.Increment(ref Unsafe.As<nuint, ulong>(ref _highestAllocated));
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

        public override void Dispose()
        {
            _stack.Dispose();
            NativeMemory.Free(_buffer);
            _lock.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EnterBufferReading()
        {
            _lock.EnterBufferRead();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void ExitBufferReading()
        {
            _lock.ExitBufferRead();
        }

        public override void Free(nuint index)
        {
            _stack.Push(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override SlabAllocationHandle<T> ReadAllocation()
        {
            EnterBufferReading();
            return new SlabAllocationHandle<T>(this, true);
        }

        ~ConcurrentSequentialSlabAllocator()
        {
            Dispose();
        }

    }
}