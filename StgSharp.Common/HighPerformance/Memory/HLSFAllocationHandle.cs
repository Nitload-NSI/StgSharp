//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocationHandle.cs"
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
using StgSharp.Entities;

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Allocator = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe struct HybridLayerSegregatedFitAllocationHandle
    {

        internal readonly byte* BufferPointer;

        internal readonly Allocator.Entry* EntryHandle;
        public readonly uint AllocSize;

        internal HybridLayerSegregatedFitAllocationHandle(Allocator.Entry* handle, uint s)
        {
            EntryHandle = handle;
            BufferPointer = (byte*)handle->Position;
            AllocSize = s;
        }

        public readonly ref byte this[int index]
        {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref BufferPointer[index];
        }

        public readonly byte* Pointer
        {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BufferPointer;
        }

        public readonly Span<byte> BufferHandle
        {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new((byte*) EntryHandle->Position, (int)AllocSize);
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public ref struct Enumerator : IEnumerator<byte>
        {

            private HybridLayerSegregatedFitAllocationHandle _handle;

            private int _index = -1;

            internal Enumerator(HybridLayerSegregatedFitAllocationHandle span)
            {
                _handle = span;
                Span = span.BufferHandle;
            }

            public ref byte RefCurrent => ref _handle[_index];

            public Span<byte> Span { get; init; }

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                _index++;
                return _index < _handle.AllocSize;
            }

            public void Reset()
            {
                _index = 0;
            }

            object IEnumerator.Current => RefCurrent;

            byte IEnumerator<byte>.Current => RefCurrent;

        }

    }
}
