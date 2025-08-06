//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedMemoryHandle.cs"
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
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe struct UnmanagedMemoryHandle
    {

        public readonly byte* BufferHandle, AllocatorHandle;
        public readonly nuint Size;

        public UnmanagedMemoryHandle(byte* pointer, byte* handle, nuint s)
        {
            BufferHandle = pointer;
            AllocatorHandle = handle;
            Size = s;
        }

        public readonly ref byte this[nuint index]
        {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.AddByteOffset(ref Unsafe.AsRef<byte>(BufferHandle), index);
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public ref struct Enumerator : IEnumerator<byte>
        {

            private nuint _index = nuint.MaxValue;

            private UnmanagedMemoryHandle _handle;

            internal Enumerator(UnmanagedMemoryHandle span)
            {
                _handle = span;
            }

            public ref byte RefCurrent => ref _handle[_index];

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                _index++;
                return _index < _handle.Size;
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
