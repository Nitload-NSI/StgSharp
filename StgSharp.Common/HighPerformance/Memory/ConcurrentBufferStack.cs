//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ConcurrentBufferStack.cs"
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

            BufferStack<T> stack = new BufferStack<T>(span.Length);


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

            ConcurrentBufferStack<T> stack = new ConcurrentBufferStack<T>(span.Length);


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
        private ulong _count;

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
            get => Volatile.Read(ref _count);
        }

        public void Clear()
        {
            Header* header = (Header*)_buffer;
            _lock.EnterExpansionProcess();
            try
            {
                header->Top = 0;
                Volatile.Write(ref _count, 0);
            }
            finally
            {
                _lock.ExitExpansionProcess();
            }
        }

        public void Dispose()
        {
            _lock.Dispose();
            Marshal.FreeHGlobal((nint)_buffer);
        }

        public T Pop()
        {
            Header* header = (Header*)_buffer;
            while (true)
            {
                nuint oldTop = Volatile.Read(ref header->Top);
                if (oldTop <= 0) {
                    throw new InvalidOperationException("Stack underflow");
                }

                if (Interlocked.CompareExchange(ref header->Top, oldTop - 1, oldTop) == oldTop)
                {
                    _lock.EnterBufferRead();
                    try
                    {
                        T* dataStart = (T*)(_buffer + sizeof(Header));
                        _ = Interlocked.Decrement(ref _count);
                        return dataStart[oldTop - 1];
                    }
                    finally
                    {
                        _lock.ExitBufferRead();
                    }
                }
                Thread.Sleep(0); // Yield to avoid busy waiting
            }
        }

        public void Push(T item)
        {
            while (true)
            {
                _lock.EnterMetaDataRead();
                try
                {
                    Header* header = (Header*)_buffer;
                    nuint oldTop = Volatile.Read(ref header->Top);
                    nuint cap = Volatile.Read(ref header->Capacity);
                    if (oldTop >= cap)
                    {
                        _lock.ExitMetaDataRead();
                        _lock.EnterExpansionProcess();
                        try
                        {
                            header = (Header*)_buffer;
                            if (header->Top >= header->Capacity)
                            {
                                nuint newCap = header->Capacity * 2;
                                nuint newSize = (nuint)sizeof(Header) + (newCap * (nuint)sizeof(T));

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

                    if (Interlocked.CompareExchange(ref header->Top, oldTop + 1, oldTop) == oldTop)
                    {
                        _lock.EnterBufferRead();
                        try
                        {
                            T* dataStart = (T*)(_buffer + sizeof(Header));
                            dataStart[oldTop] = item;
                            _ = Interlocked.Increment(ref _count);
                            return;
                        }
                        finally
                        {
                            _lock.ExitBufferRead();
                        }
                    }
                }
                finally
                {
                    if (_lock.IsThreadReadingMetaData) {
                        _lock.ExitMetaDataRead();
                    }
                }
                Thread.Sleep(0); // Yield to avoid busy waiting
            }
        }

        public bool TryPop(out T value)
        {
            Header* header = (Header*)_buffer;
            while (true)
            {
                nuint oldTop = Volatile.Read(ref header->Top);
                if (oldTop <= 0)
                {
                    value = default;
                    return false;
                }

                if (Interlocked.CompareExchange(ref header->Top, oldTop - 1, oldTop) == oldTop)
                {
                    _lock.EnterBufferRead();
                    try
                    {
                        T* dataStart = (T*)(_buffer + sizeof(Header));
                        value = dataStart[oldTop - 1];
                        _ = Interlocked.Decrement(ref _count);
                    }
                    finally
                    {
                        _lock.ExitBufferRead();
                    }
                    return true;
                }
                Thread.Sleep(0); // Yield to avoid busy waiting
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
                    header->Capacity * (nuint)sizeof(T), 
                    (nuint)span.Length * (nuint)sizeof(T));
            }

            header->Top = (nuint)span.Length;
            Volatile.Write(ref _count, (ulong)span.Length);
        }

        private void AllocateBuffer(int capacity)
        {
            nuint size = (nuint)sizeof(Header) + ((nuint)capacity * (nuint)sizeof(T));
            _buffer = (byte*)NativeMemory.Alloc(size);
            Header* header = (Header*)_buffer;
            header->Top = 0;
            header->Capacity = (nuint)capacity;
        }

        private struct Header
        {

            public nuint Capacity;   // max capacity of the stack (number of elements)
            public nuint Top;        // next available index in the stack

        }

    }
}
