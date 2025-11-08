//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ConcurrentBufferStack"
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public static class BufferStackBuilder
    {

        // For BufferStack collection builder
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BufferStack<T> Create<T>(ReadOnlySpan<T> span) where T: unmanaged
        {
            if (span.IsEmpty)
            {
                return new BufferStack<T>(4); // Default small capacity
            }

            BufferStack<T> stack = new(span.Length);


            // Use internal method for efficient bulk initialization
            stack.InitializeFromSpan(span);

            return stack;
        }

        // For ConcurrentBufferStack collection builder
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConcurrentBufferStack<T> CreateConcurrent<T>(ReadOnlySpan<T> span) where T: unmanaged
        {
            if (span.IsEmpty)
            {
                return new ConcurrentBufferStack<T>(4); // Default small capacity
            }

            ConcurrentBufferStack<T> stack = new(span.Length);


            // Use internal method for efficient bulk initialization
            stack.InitializeFromSpan(span);

            return stack;
        }

    }

    [CollectionBuilder(builderType: typeof(BufferStackBuilder), methodName: nameof(BufferStackBuilder.CreateConcurrent))]
    public unsafe class ConcurrentBufferStack<T> : IDisposable where T: unmanaged
    {

        private byte* _buffer;
        private readonly BufferExpansionLock _lock = new();
        private long _count;

        public ConcurrentBufferStack()
        {
            AllocateBuffer(4);
        }

        public ConcurrentBufferStack(int capacity)
        {
            if (capacity <= 0) {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            AllocateBuffer(capacity);
        }

        public ulong Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (ulong)Volatile.Read(ref _count);
        }

        public void Clear()
        {
            _lock.EnterExpansionProcess();
            try
            {
                _lock.EnterBufferCopy(); // exclusive while mutating Top
                try
                {
                    Header* header = (Header*)_buffer;
                    header->Top = 0;
                    Volatile.Write(ref _count, 0);
                }
                finally
                {
                    _lock.ExitBufferCopy();
                }
            }
            finally
            {
                _lock.ExitExpansionProcess();
            }
        }

        public void Dispose()
        {
            _lock.Dispose();
            NativeMemory.Free(_buffer);
        }

        public T Pop()
        {
            while (true)
            {
                _lock.EnterBufferRead(); // prevent buffer being reallocated while reading/modifying Top
                try
                {
                    Header* header = (Header*)_buffer;
                    int oldTop = Volatile.Read(ref header->Top);
                    if (oldTop <= 0) {
                        throw new InvalidOperationException("Stack underflow");
                    }

                    if (Interlocked.CompareExchange(ref header->Top, oldTop - 1, oldTop) == oldTop)
                    {
                        T* dataStart = (T*)(_buffer + sizeof(Header));
                        _ = Interlocked.Decrement(ref _count);
                        return dataStart[oldTop - 1];
                    }
                }
                finally
                {
                    _lock.ExitBufferRead();
                }
                Thread.Sleep(0);
            }
        }

        public void Push(T item)
        {
            while (true)
            {
                // Ensure capacity, expanding if needed
                _lock.EnterMetaDataRead();
                try
                {
                    Header* header = (Header*)_buffer;
                    int topSnapshot = Volatile.Read(ref header->Top);
                    int cap = Volatile.Read(ref header->Capacity);
                    if (topSnapshot >= cap)
                    {
                        // Need expansion
                        _lock.ExitMetaDataRead();
                        _lock.EnterExpansionProcess();
                        try
                        {
                            header = (Header*)_buffer;
                            if (header->Top >= header->Capacity)
                            {
                                int newCap = header->Capacity * 2;
                                nuint newSize = (nuint)sizeof(Header) + ((nuint)newCap * (nuint)sizeof(T));

                                _lock.EnterBufferCopy();
                                try
                                {
                                    _buffer = (byte*)NativeMemory.Realloc(_buffer, newSize);
                                    ((Header*)_buffer)->Capacity = newCap;
                                }
                                finally
                                {
                                    _lock.ExitBufferCopy();
                                }
                            }
                        }
                        finally
                        {
                            _lock.ExitExpansionProcess();
                        }
                        continue;
                    }
                }
                finally
                {
                    if (_lock.IsThreadReadingMetaData) {
                        _lock.ExitMetaDataRead();
                    }
                }

                // Reserve and write under exclusive buffer lock to avoid consumers reading unwritten data
                _lock.EnterBufferCopy();
                try
                {
                    Header* header = (Header*)_buffer;
                    int idx = header->Top; // reserve next slot
                    if (idx >= header->Capacity)
                    {
                        // lost race against expander or other producer; retry
                        continue;
                    }

                    T* dataStart = (T*)(_buffer + sizeof(Header));
                    dataStart[idx] = item; // write first
                    header->Top = idx + 1; // then publish by increasing top
                    _ = Interlocked.Increment(ref _count);
                    return;
                }
                finally
                {
                    _lock.ExitBufferCopy();
                }
            }
        }

        public bool TryPop(out T value)
        {
            while (true)
            {
                _lock.EnterBufferRead();
                try
                {
                    Header* header = (Header*)_buffer;
                    int oldTop = Volatile.Read(ref header->Top);
                    if (oldTop <= 0)
                    {
                        value = default;
                        return false;
                    }

                    if (Interlocked.CompareExchange(ref header->Top, oldTop - 1, oldTop) == oldTop)
                    {
                        T* dataStart = (T*)(_buffer + sizeof(Header));
                        value = dataStart[oldTop - 1];
                        _ = Interlocked.Decrement(ref _count);
                        return true;
                    }
                }
                finally
                {
                    _lock.ExitBufferRead();
                }
                Thread.Sleep(0);
            }
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
                    (nuint)header->Capacity * (nuint)sizeof(T),
                    (nuint)span.Length * (nuint)sizeof(T));
            }

            header->Top = span.Length;
            Volatile.Write(ref _count, span.Length);
        }

        private void AllocateBuffer(int capacity)
        {
            nuint size = (nuint)sizeof(Header) + (nuint)capacity * (nuint)sizeof(T);
            _buffer = (byte*)NativeMemory.Alloc(size);
            Header* header = (Header*)_buffer;
            header->Top = 0;
            header->Capacity = capacity;
        }

        private struct Header
        {

            public int Capacity;   // max capacity of the stack (number of elements)
            public int Top;        // next available index in the stack

        }

    }
}
