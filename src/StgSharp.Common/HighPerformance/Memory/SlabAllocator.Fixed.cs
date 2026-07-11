// -----------------------------------------------------------------------------
// file="SlabAllocator.Fixed"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;
using StgSharp.HighPerformance.Memory;
using StgSharp.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    internal unsafe class FixedSlabAllocator<T> : SlabAllocator<T> where T : unmanaged
    {

        private Action<nuint> _freeHandle;

        private bool _disposed;
        private CapacityFixedStack<nuint> _recycle;
        private readonly int _elementSize = Unsafe.SizeOf<T>();

        private nuint _buffer;

        private nuint _Capacity, _currentIndex;

        internal FixedSlabAllocator(
                 nuint buffer,
                 nuint count,
                 Action<nuint> freeHandle
        )
        {
            _freeHandle = freeHandle;
            _buffer = buffer;

            _Capacity = count;
            int initCount = count switch
            {
                <= 4 => 4,
                > 64 => 64,
                _ => (int)count
            };
            Span<nuint> span = stackalloc nuint[initCount];
            for (int i = 0; i < initCount; i++) {
                span[i] = _buffer + ((nuint)(i * _elementSize));
            }
            _recycle = CapacityFixedStackBuilder.Create(span);
            _Capacity = count;
            _currentIndex = (nuint)initCount;
        }

        public override nuint Allocate()
        {
            if (_recycle.TryPop(out nuint handle)) {
                return handle;
            }

            // nuint bufferHandle = _currentBuffer;
            nuint curIndex = _currentIndex;
            if (curIndex >= _Capacity) {
                return 0;
            }
            curIndex = _currentIndex;
            _currentIndex += 1;
            return _buffer + (((nuint)_elementSize) * curIndex);
        }

        public override void Dispose()
        {
            if (_disposed) {
                return;
            }
            _disposed = true;
            _freeHandle(_buffer);
            _recycle = null!;
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

        ~FixedSlabAllocator()
        {
            Dispose();
        }

    }
}
