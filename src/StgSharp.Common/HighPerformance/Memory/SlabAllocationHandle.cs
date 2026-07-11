// -----------------------------------------------------------------------------
// file="SlabAllocationHandle"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe struct SlabAllocationHandle<T> : IDisposable where T : unmanaged
    {

        private readonly SlabAllocator<T> _allocator;

        internal SlabAllocationHandle(
                 SlabAllocator<T> allocator,
                 bool locked
        )
        {
            _allocator = allocator;
            Locked = locked;
        }

        public bool Locked { get; set; }

        public void Dispose()
        {
            if (Locked)
            {
                _allocator.ExitBufferReading();
                Locked = false;
            }
        }

    }
}
