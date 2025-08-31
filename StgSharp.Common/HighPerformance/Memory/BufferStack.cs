//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="BufferStack.cs"
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    [CollectionBuilder(typeof(BufferStackBuilder), nameof(BufferStackBuilder.Create))]
    public unsafe class BufferStack<T> : IDisposable where T: unmanaged
    {

        private byte* _buffer;
        private ulong _count;

        public BufferStack(int capacity)
        {
            if (capacity <= 0) {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            AllocateBuffer(capacity);
        }

        public int Count
        {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)_count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            ((Header*)_buffer)->Top = 0;
            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Marshal.FreeHGlobal((nint)_buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Pop()
        {
            Header* header = (Header*)_buffer;
            if (header->Top == 0) {
                throw new InvalidOperationException("Stack underflow");
            }

            header->Top--;
            _count--;
            T* dataStart = (T*)(_buffer + sizeof(Header));
            return dataStart[header->Top];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T item)
        {
            Header* header = (Header*)_buffer;
            if (header->Top >= header->Capacity)
            {
                Expand();
                header = (Header*)_buffer;
            }

            T* dataStart = (T*)(_buffer + sizeof(Header));
            dataStart[header->Top] = item;
            header->Top++;
            _count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out T value)
        {
            Header* header = (Header*)_buffer;
            if (header->Top == 0)
            {
                value = default;
                return false;
            }
            header->Top--;
            _count--;
            T* dataStart = (T*)(_buffer + sizeof(Header));
            value = dataStart[header->Top];
            return true;
        }

        // Internal method for efficient bulk initialization
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void InitializeFromSpan(ReadOnlySpan<T> span)
        {
            if (span.IsEmpty) {
                return;
            }

            Header* header = (Header*)_buffer;
            T* dataStart = (T*)(_buffer + sizeof(Header));

            fixed (T* sourcePtr = span) {
                Buffer.MemoryCopy(sourcePtr, dataStart,
                    header->Capacity * (nuint)sizeof(T),
                    (nuint)span.Length * (nuint)sizeof(T));
            }

            header->Top = (nuint)span.Length;
            _count = (ulong)span.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AllocateBuffer(int capacity)
        {
            nuint size = (nuint)sizeof(Header) + ((nuint)capacity * (nuint)sizeof(T));
            _buffer = (byte*)NativeMemory.Alloc(size);
            Header* header = (Header*)_buffer;
            header->Top = 0;
            header->Capacity = (nuint)capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Expand()
        {
            Header* header = (Header*)_buffer;
            nuint newCap = header->Capacity * 2;
            nuint newSize = (nuint)sizeof(Header) + newCap * (nuint)sizeof(T);
            _buffer = (byte*)NativeMemory.Realloc(_buffer, newSize);
            header = (Header*)_buffer;
            header->Capacity = newCap;
        }

        private struct Header
        {

            public nuint Capacity;
            public nuint Top;

        }

    }
}