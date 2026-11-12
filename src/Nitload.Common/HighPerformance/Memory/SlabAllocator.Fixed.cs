// -----------------------------------------------------------------------------
// file="SlabAllocator.Fixed"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.Collections;
using Nitload.Common.HighPerformance.Memory;
using Nitload.Common.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    internal unsafe class FixedSlabAllocator<T> : SlabAllocator<T> where T : unmanaged
    {

        private readonly Action<UnmanagedMemoryProperty> _freeMemory;

        private bool _disposed;
        private CapacityFixedStack<nuint> _recycle;
        private readonly int _elementSize = Unsafe.SizeOf<T>();

        private nuint _Capacity, _currentIndex;
        private readonly UnmanagedMemoryProperty _memory;

        private FixedSlabAllocator(
                UnmanagedMemoryProperty memory,
                nuint count,
                Action<UnmanagedMemoryProperty> freeHandle
        )
        {
            _memory = memory;
            _freeMemory = freeHandle;

            _Capacity = count;
            _recycle = new((int)_Capacity);
            _recycle.FillRange(init, memory.Address);
            _currentIndex = _Capacity;

            static void init(
                        Span<nuint> arr,
                        nuint buffer
            )
            {
                for (int i = 0; i < arr.Length; i++) {
                    arr[i] = buffer + ((nuint)i * (nuint)Unsafe.SizeOf<T>());
                }
            }
        }

        internal FixedSlabAllocator(
                 nuint buffer,
                 nuint count,
                 Action<nuint> freeHandle
        )
            : this(new UnmanagedMemoryProperty(buffer, checked(count * (nuint)Unsafe.SizeOf<T>()),
                                               1,
                                               -1), count, memory => freeHandle(memory.Address)) { }

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
            return _memory.Address + (((nuint)_elementSize) * curIndex);
        }

        public override void Dispose()
        {
            if (_disposed) {
                return;
            }
            _disposed = true;
            _freeMemory(_memory);
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

        internal static FixedSlabAllocator<T> FromMemory(
                                              UnmanagedMemoryProperty memory,
                                              nuint count,
                                              Action<UnmanagedMemoryProperty> freeHandle
        )
        {
            return new FixedSlabAllocator<T>(memory, count, freeHandle);
        }

        ~FixedSlabAllocator()
        {
            Dispose();
        }

    }
}
