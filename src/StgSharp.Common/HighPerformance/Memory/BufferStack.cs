// -----------------------------------------------------------------------------
// file="BufferStack"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    [CollectionBuilder(typeof(BufferStackBuilder), nameof(BufferStackBuilder.Create))]
    public unsafe class BufferStack<T> : IDisposable where T : unmanaged
    {

        private byte* _buffer;
        private ulong _count;

        public BufferStack(
               int capacity
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

            AllocateBuffer(capacity);
        }

        public int Count => (int)_count;

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
        public void Push(
                    T item
        )
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
        public bool TryPop(
                    out T value
        )
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
        internal unsafe void InitializeFromSpan(
                             ReadOnlySpan<T> span
        )
        {
            if (span.IsEmpty) {
                return;
            }

            Header* header = (Header*)_buffer;
            T* dataStart = (T*)(_buffer + sizeof(Header));

            fixed (T* sourcePtr = span) {
                Buffer.MemoryCopy(sourcePtr, dataStart, header->Capacity * (nuint)sizeof(T),
                                  (nuint)span.Length * (nuint)sizeof(T));
            }

            header->Top = (nuint)span.Length;
            _count = (ulong)span.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AllocateBuffer(
                     int capacity
        )
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
            nuint newSize = (nuint)sizeof(Header) + (newCap * (nuint)sizeof(T));
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