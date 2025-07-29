//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedMemoryPool.cs"
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
using System.Collections.Concurrent;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Transactions;

namespace StgSharp.HighPerformance
{
    public unsafe partial class UnmanagedMemoryPool
    {

        private readonly byte* _buffer;

        // 4k,1k,256b,64b,16b
        private nuint[] _entrance = new nuint[5];
        private bool _disposedValue;
        private readonly nuint _initialCapacity;

        internal UnmanagedMemoryPool(nuint initialCapacity, int memoryAlign)
        {
            _initialCapacity = initialCapacity;
            _buffer = (byte*)NativeMemory.AlignedAlloc(initialCapacity, (nuint)memoryAlign);
        }

        public UnmanagedMemoryHandle Allocate(uint size = 0)
        {
            if (size == 0) {
                return new UnmanagedMemoryHandle
                {
                    Postion = 0,
                    Buffer = new Span<byte>(_buffer, 0)
                };
            }
            if (size > 0x8000_0000) {
                throw new ArgumentOutOfRangeException(
                    "Size of buffer cannot be greater than 2147483648");
            }
            int level = GetLevelFromSize(size);
            IndexNode allocator = GetNode(_entrance[level]);
            nuint previous = allocator.Previous[level];
            if (allocator.Size < size)
            {
                previous = allocator.Position;
                allocator = GetNode(allocator.Next[level]);
            }
            bool isOverflow = false;
            checked
            {
                try
                {
                    isOverflow = (uint)(allocator.Position - previous + size) > 0xC0000000;
                }
                catch (OverflowException)
                {
                    isOverflow = true;
                }
            }
            if (isOverflow)
            {
                // offset is too large
            }

            UnmanagedMemoryHandle handle = new()
            {
                Postion = allocator.Position,
                Buffer = new Span<byte>(_buffer + allocator.Position, (int)size),
            };
            Rebase(allocator, size);
            return handle;
        }

        public static UnmanagedMemoryPool Create(nuint initialCapacity, int memoryAlign = 16)
        {
            UnmanagedMemoryPool pool = new(initialCapacity, memoryAlign);
            IndexNode node = pool.GetNode(0);
            for (int i = 4; i > 0; i--)
            {
                node.Previous[i] = 0;
                node.Next[i] = IndexNode.LevelToSize(i);
            }
            node.Size = (uint)initialCapacity;
            pool._megaFirst = new MegaBlockHead
            {
                Position = 0,
                Size = initialCapacity,
                Usage = MegaBlockUsage.EmptyDag
            };
            _ = pool._megaEmpty.AddLast(pool._megaFirst);
            return pool;
        }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public void Free(UnmanagedMemoryHandle handle)
        {
            if (_buffer + handle.Postion != Unsafe.AsPointer(ref handle.Buffer[0])) {
                throw new InvalidOperationException("Handle does not belong to pool");
            }
            nuint location = handle.Postion;
            IndexNode root = GetNode(_entrance[4]);
            while (root.Position > location) { }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                NativeMemory.AlignedFree(_buffer);
                _entrance = null!;
                _disposedValue = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLevelFromSize(uint size)
        {
            int level = 0;
            uint temp = size / 8;
            while (temp > 3)
            {
                level++;
                temp >>= 2;
            }
            return level;
        }

        ~UnmanagedMemoryPool()
        {
            Dispose(disposing:false);
        }

    }
}
