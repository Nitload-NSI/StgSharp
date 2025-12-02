//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SlabAllocator.Chunked"
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    internal sealed unsafe class ChunkedSlabAllocator<T> : SlabAllocator<T> where T : unmanaged
    {

        private BufferStack<nuint> _buffers;
        private BufferStack<nuint> _recycle;
        private readonly int _elementSize = Unsafe.SizeOf<T>();

        private nuint _currentBuffer;
        private nuint _currentCapacity, _currentIndex;

        public ChunkedSlabAllocator(nuint count)
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
            _buffers.Push(_currentBuffer);
            _recycle = BufferStackBuilder.Create(span);
            _currentCapacity = count;
            _currentIndex = (nuint)initCount;
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
                nuint newBuffer = (nuint)NativeMemory.Alloc(cap * (nuint)_elementSize);
                _buffers.Push(newBuffer);
                _currentBuffer = newBuffer;
                _currentIndex = 0;
            }
            curIndex = _currentIndex;
            _currentIndex += 1;
            return _currentBuffer + ((nuint)_elementSize * curIndex);
        }

        public override void Dispose()
        {
            // NativeMemory.Free((void*)_currentBuffer);
            while (_buffers.TryPop(out nuint buffer))
            {
                NativeMemory.Free((void*)buffer);
            }
            _buffers.Dispose();
            _recycle.Dispose();
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

        ~ChunkedSlabAllocator()
        {
            Dispose();
        }

    }
}
