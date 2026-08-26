// -----------------------------------------------------------------------------
// file="SlabAllocator.Chunked"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Threading;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    internal sealed unsafe class ChunkedSlabAllocator<T> : SlabAllocator<T> where T : unmanaged
    {

        private bool _disposed;

        private BufferStack<nuint> _buffers;
        private BufferStack<nuint> _recycle;
        private readonly int _elementSize = Unsafe.SizeOf<T>();

        private nuint _currentBuffer;
        private nuint _currentCapacity, _currentIndex;

        public ChunkedSlabAllocator(
               nuint count
        )
        {
            _currentBuffer = (nuint)NativeMemory.Alloc(count * ((nuint)_elementSize));

            _currentCapacity = count;
            int initCount = count switch
            {
                <= 4 => 4,
                > 64 => 64,
                _ => (int)count
            };
            _buffers = new(4);
            _buffers.Push(_currentBuffer);

            // Fill through a callback rather than building a scratch span and
            // passing it to BufferStackBuilder.Create: no stackalloc, no copy,
            // and no reliance on Span-to-ReadOnlySpan inference (CS0411 before
            // C# 14). Same shape as FixedSlabAllocator.
            int elementSize = _elementSize;
            _recycle = new(initCount);
            _recycle.FillRange(init, _currentBuffer);
            _currentCapacity = count;
            _currentIndex = (nuint)initCount;

            void init(
                 Span<nuint> arr,
                 nuint buffer
            )
            {
                for (int i = 0; i < arr.Length; i++) {
                    arr[i] = buffer + ((nuint)(i * elementSize));
                }
            }
        }

        public override nuint Allocate()
        {
            if (_recycle.TryPop(out nuint handle)) {
                return handle;
            }

            // nuint bufferHandle = _currentBuffer;
            nuint curIndex = _currentIndex;
            if (curIndex >= _currentCapacity)
            {
                nuint cap = _currentCapacity;
                nuint newBuffer = (nuint)NativeMemory.Alloc(cap * ((nuint)_elementSize));
                _buffers.Push(newBuffer);
                _currentBuffer = newBuffer;
                _currentIndex = 0;
            }
            curIndex = _currentIndex;
            _currentIndex += 1;
            return _currentBuffer + (((nuint)_elementSize) * curIndex);
        }

        public override void Dispose()
        {
            if (_disposed) {
                return;
            }
            _disposed = true;

            // NativeMemory.Free((void*)_currentBuffer);
            while (_buffers.TryPop(out nuint buffer))
            {
                NativeMemory.Free((void*)buffer);
            }
            _buffers.Dispose();
            _recycle.Dispose();
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EnterBufferReading() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void ExitBufferReading() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Free(
                             nuint index
        )
        {
            _recycle.Push(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override SlabAllocationHandle<T> ReadAllocation()
        {
            return new SlabAllocationHandle<T>(this, false);
        }

        ~ChunkedSlabAllocator()
        {
            Dispose();
        }

    }
}
