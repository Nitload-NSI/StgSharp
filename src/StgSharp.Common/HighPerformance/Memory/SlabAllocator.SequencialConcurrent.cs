// -----------------------------------------------------------------------------
// file="SlabAllocator.SequencialConcurrent"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Threading;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public sealed unsafe class ConcurrentSequentialSlabAllocator<T> : SlabAllocator<T>
        where T : unmanaged
    {

        private T* _buffer;

        private bool _disposed;

        private readonly BufferExpansionLock _lock = new();
        private readonly ConcurrentBufferStack<nuint> _stack;
        private readonly int _elementSize;
        private nuint _highestAllocated;
        private ulong _capacity;

        public ConcurrentSequentialSlabAllocator(
               nuint initialCapacity
        )
        {
            _elementSize = Unsafe.SizeOf<T>();
            _capacity = initialCapacity;
            _highestAllocated = 0;

            _buffer = (T*)NativeMemory.Alloc(((nuint)_capacity) * ((nuint)_elementSize));
            int initCount = _capacity switch
            {
                <= 4 => 4,
                > 64 => 64,
                _ => ((int)_capacity) / 4
            };
            Span<nuint> span = stackalloc nuint[initCount];
            for (int i = 0; i < initCount; i++) {
                span[i] = (nuint)i;
            }
            _stack = BufferStackBuilder.CreateConcurrent(span);
            _highestAllocated += (nuint)initCount;
        }

        internal T* BasePointer => _buffer;

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
                            nuint newSize = ((nuint)_capacity) * ((nuint)_elementSize);
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
            if (_disposed) {
                return;
            }
            _disposed = true;
            _stack.Dispose();
            NativeMemory.Free(_buffer);
            _lock.Dispose();
            GC.SuppressFinalize(this);
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

        public override void Free(
                             nuint index
        )
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