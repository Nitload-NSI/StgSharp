//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="UnmanagedMemoryPool.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance
{
    public unsafe class UnmanagedMemoryPool<T> : IDisposable where T: unmanaged
    {

        private readonly T* _buffer;

        // 4k,1k,256b,64b,16b
        private IndexNode[] _entrance = new IndexNode[5];
        private bool _disposedValue;
        private ConcurrentQueue<IndexNode> _recycle = new();
        private IndexNode _root;
        private readonly int _initialCapacity;

        internal UnmanagedMemoryPool(int initialCapacity, int memoryAlign)
        {
            _initialCapacity = initialCapacity;
            _buffer = (T*)NativeMemory.AlignedAlloc((nuint)initialCapacity, (nuint)memoryAlign);
            _root = CreateNode();
            _root.Buffer = _buffer;
            _root.Size = _initialCapacity;
            _root.Offset = 0;
            _root.Level = 5;
        }

        public UnmanagedMemoryHandle<T> Allocate(uint elementCount = 0)
        {
            if (elementCount == 0) {
                return new UnmanagedMemoryHandle<T>
                {
                    Begin = 0,
                    Buffer = new Span<T>(_buffer, 0)
                };
            }
            uint size = (uint)sizeof(T) * elementCount;
            uint level = GetLevelFromSize(size);
            IndexNode allocator = _entrance[level];
            if (allocator.Size < size) {
                allocator = allocator.Next;
            }
            UnmanagedMemoryHandle<T> handle = new()
            {
                Begin = allocator.Offset,
                Buffer = new Span<T>(_buffer + size, (int)elementCount),
            };
            if (allocator.Size == size)
            {
                UpdateAllocatorInLevel(allocator, 0);
            } else
            {
                UpdateAllocatorInLevel(allocator, size);
            }
            IndexNode n = allocator.Root;
            while (IndexNode.IsNUllOrEmpty(n))
            {
                UpdateAllocatorInLevel(n, size);
                n = n.Root;
            }
            n = allocator.Down;
            while (n != null) {
                UpdateAllocatorInLevel(n, size);
            }

            return handle;
        }

        public static UnmanagedMemoryPool<T> Create(int initialCapacity, int memoryAlign = 16)
        {
            UnmanagedMemoryPool<T> pool = new(initialCapacity, memoryAlign);
            pool._entrance[5] = pool._root;
            IndexNode node = pool._root;
            for (int i = 4; i > 0; i--)
            {
                node = node.Expand();
                pool._entrance[i] = node;
            }
            return pool;
        }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public void Free(UnmanagedMemoryHandle<T> handle)
        {
            if (_buffer + handle.Begin != Unsafe.AsPointer<T>(ref handle.Buffer[0])) {
                throw new InvalidOperationException("Handle does not belong to pool");
            }
            int location = handle.Begin;
            int page = location / 4096;
            uint level = GetLevelFromSize((uint)handle.Buffer.Length);
            IndexNode root = null!;
            for (int i = 4; i >= 0; i--)
            {
                IndexNode node = _entrance[i];
                if (!IndexNode.IsNUllOrEmpty(node))
                {
                    root = node;
                    break;
                }
            }
            while (root is null) {
                _entrance[level] = CreateNode();
            }
            while (root.Level != level) { }


            /*
             * Ignore merge like block A(level) + block B(level-1)
             * such merge requires index in different levels,
             * nearly impossible to implement, for going from 
             * A to B may be a very complex route.
             * Boundary of B usually is not aligned with end of A,
             * otherwise, we can just merge when B and memory nearby 
             * is packed together again.
            */

            //TODO 没写完
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
        private IndexNode CreateNode()
        {
            return _recycle.TryDequeue(out IndexNode? node) ? node : new IndexNode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetLevelFromSize(uint size)
        {
            uint level = 0, temp = size / 8;
            while (temp > 3)
            {
                level++;
                temp >>= 2;
            }
            return level;
        }

        //TODO 节点变基算法存在问题 
        /*
         * 如果上层节点变基后，连带下层节点变基
         * 此时可能使下层节点起点早于上层节点起点，
         * 此时应当进行检查和切割，将下层节点切成两个部分使得第二块的起始位置和上层节点对齐
         */
        private void UpdateAllocatorInLevel(IndexNode allocator, uint offset)
        {
            int level = allocator.Level;
            if (!allocator.Rebase(offset) || offset == 0)
            {
                IndexNode p = allocator.Previous;
                IndexNode n = allocator.Next;
                if (IndexNode.IsNUllOrEmpty(p))
                {
                    _entrance[level] = n;
                    n.Previous = IndexNode.Empty;
                } else if (IndexNode.IsNUllOrEmpty(n))
                {
                    p.Next = IndexNode.Empty;
                } else
                {
                    p.Next = n;
                    n.Previous = p;
                }
                _recycle.Enqueue(allocator);
            }
        }

        ~UnmanagedMemoryPool()
        {
            Dispose(disposing:false);
        }

        internal class IndexNode
        {

            private static readonly int[] _sizeArr = [16,64,256,1024,4096];

            public T* Buffer { get; set; }

            public IndexNode Root { get; set; }

            public IndexNode Next { get; set; } = Empty;

            public IndexNode Previous { get; set; } = Empty;

            public IndexNode Down { get; set; } = Empty;

            public static IndexNode Empty { get; } = new EmptyIndex();

            public int Size { get; set; }

            public int Offset { get; set; }

            public int Level { get; set; } = 0;

            public int BlockSize => LevelToSize(Level);

            public IndexNode Expand()
            {
                if (this.Level < 0) {
                    return Empty;
                }
                IndexNode d = new()
                {
                    Buffer = this.Buffer,
                    Level = this.Level - 1,
                    Root = this
                };
                d.Size = d.BlockSize;
                this.Down = d;
                return d;
            }

            public static bool IsNUllOrEmpty(IndexNode node)
            {
                return node is null or EmptyIndex;
            }

            public static int LevelToSize(int level)
            {
                return level switch
                {
                    < 0 or > 5 => throw new ArgumentOutOfRangeException(
                        nameof(level), "Level must be between 1 and 5."),
                    _ => _sizeArr[level]
                };
            }

            public bool Rebase(uint offset)
            {
                int remain = Size - (int)offset;
                if (remain < LevelToSize(Level)) {
                    return false;
                }
                int sec = LevelToSize(Level),
                    count = remain / sec, size = count * sec;
                Offset += Size - size;
                Size = size;
                return true;
            }

            private class EmptyIndex : IndexNode { }

        }

    }
}
