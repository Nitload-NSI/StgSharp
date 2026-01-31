//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SlabAllocator.ChunkedConcurrent"
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    internal sealed unsafe class ConcurrentChunkedSlabAllocator<T> : SlabAllocator<T> where T : unmanaged
    {

        private bool _disposed;

        private readonly BufferExpansionLock _lock = new();
        private ConcurrentBufferStack<nuint> _buffers;
        private ConcurrentBufferStack<nuint> _recycle;
        private readonly int _elementSize = Unsafe.SizeOf<T>();

        private nuint _currentBuffer;
        private nuint _currentCapacity, _currentIndex;

        public ConcurrentChunkedSlabAllocator(nuint count)
        {
            _currentBuffer = (nuint)NativeMemory.Alloc(count * (nuint)_elementSize);
            _currentCapacity = count;
            int initCount = count switch
            {
                <= 4 => 4,
                > 64 => 64,
                _ => (int)count
            };
            Span<nuint> span = stackalloc nuint[initCount];
            for (int i = 0; i < initCount; i++) {
                span[i] = _currentBuffer + (nuint)(i * _elementSize);
            }
            _buffers = new(4);
            _recycle = BufferStackBuilder.CreateConcurrent(span);
            _currentCapacity = count;
            _currentIndex = (nuint)initCount;
        }

        public override nuint Allocate()
        {
            if (_recycle.TryPop(out nuint handle)) {
                return handle;
            }
            while (true)
            {
                nuint bufferHandle = Volatile.Read(ref _currentBuffer);
                nuint curIndex = Volatile.Read(ref _currentIndex);
                _lock.EnterMetaDataRead();
                try
                {
                    if (curIndex >= _currentCapacity)
                    {
                        _lock.ExitMetaDataRead();
                        _lock.EnterExpansionProcess();
                        try
                        {
                            nuint cap = Volatile.Read(ref _currentCapacity);
                            nuint newBuffer = (nuint)NativeMemory.Alloc(cap * (nuint)_elementSize);
                            _buffers.Push(bufferHandle);
                            _ = Interlocked.Exchange(ref _currentBuffer, newBuffer);
                            _ = Interlocked.Exchange(ref _currentIndex, 0);
                        }
                        finally
                        {
                            _lock.ExitExpansionProcess();
                        }
                    }
                    curIndex = Volatile.Read(ref _currentIndex);
                    _ = Interlocked.Add(
                        ref Unsafe.As<nuint, ulong>(ref _currentIndex), (ulong)_elementSize);
                    return _currentBuffer + ((nuint)_elementSize * curIndex);
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
            NativeMemory.Free((void*)_currentBuffer);
            while (_buffers.TryPop(out nuint buffer)) {
                NativeMemory.Free((void*)buffer);
            }
            _lock.Dispose();
            _buffers.Dispose();
            _recycle.Dispose();
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void EnterBufferReading() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void ExitBufferReading() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Free(nuint index)
        {
            _recycle.Push(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override SlabAllocationHandle<T> ReadAllocation()
        {
            return new SlabAllocationHandle<T>(this, false);
        }

        ~ConcurrentChunkedSlabAllocator()
        {
            Dispose();
        }

    }
}
