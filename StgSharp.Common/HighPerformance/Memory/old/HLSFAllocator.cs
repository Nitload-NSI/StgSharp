//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator"
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
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator : IDisposable
    {

        // Bucket system field - directly in main allocator class
        private readonly BucketNode*[] _bucketHeads;
        private Metadata* _metadata;

        // _spareMemory is also the beginning position of allocator
        private Entry* _spareMemory;
        private readonly byte* m_Buffer;

        private readonly Action<nuint> _freeHandle;
        private bool disposedValue;
        private readonly int _align;
        private readonly int _maxAllocCount;
        private readonly int _maxLevel;
        private int unusedMetaData;
        private readonly nuint _maxAllocSize;
        private readonly nuint _size;
        private Ptr64 _basePtr;
        private readonly SlabAllocator<Entry> _entries;

        internal HybridLayerSegregatedFitAllocator(
                 nuint byteSize,
                 int align,
                 nuint maxBlockSize,
                 Func<nuint, int, nuint> allocator,
                 Action<nuint> freeHandle,
                 SlabAllocator<Entry>? entryManager
        )
            : this(byteSize, align, maxBlockSize, (void*)allocator(byteSize, align), freeHandle,
                   entryManager) { }

        internal HybridLayerSegregatedFitAllocator(
                 nuint byteSize,
                 int align,
                 nuint maxBlockSize,
                 void* externalPointer,
                 Action<nuint> freeHandle,
                 SlabAllocator<Entry>? entryManager = null
        )
        {
            if ((align < 16) || ((align & (align - 1)) != 0)) {
                throw new ArgumentException(
                                "Alignment must be a power of two and at least 16 bytes.");
            }
            _entries = entryManager ??
                SlabAllocator.Create<Entry>(64, SlabBufferLayout.Chunked, false);
            m_Buffer = (byte*)externalPointer;
            _basePtr = (Ptr64)(nuint)externalPointer;
            _metadata = (Metadata*)((nuint)externalPointer + byteSize);
            _align = align;
            _size = byteSize;

            _spareMemory = (Entry*)_entries.Allocate();
            _spareMemory->State = EntryState.Empty;
            _spareMemory->PreviousNear = _spareMemory;
            _spareMemory->NextNear = _spareMemory;
            _spareMemory->Position = (nuint)m_Buffer;
            _spareMemory->Size = byteSize;

            _maxLevel = GetLevelFromSize(maxBlockSize) + 1;
            _bucketHeads = new BucketNode*[(_maxLevel * 3) + 1];
            _maxAllocSize = maxBlockSize;
            _freeHandle = freeHandle ?? (static _ => { });
        }

        public HybridLayerSegregatedFitAllocator(
               nuint byteSize
        )
            : this(byteSize, 16, 32U * 1024U * 1024U, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public HybridLayerSegregatedFitAllocator(
               nuint byteSize,
               uint maxBlockSize
        )
            : this(byteSize, 16, maxBlockSize, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public HybridLayerSegregatedFitAllocator(
               nuint byteSize,
               int align
        )
            : this(byteSize, align, 32U * 1024U * 1024U, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public HybridLayerSegregatedFitAllocator(
               nuint byteSize,
               int align,
               nuint maxBlockSize
        )
            : this(byteSize, align, maxBlockSize, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public partial HlsfHandle Alloc(
                                  nuint size
        );

        public partial void Collect();

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing) {
                    _entries.Dispose();
                }
                _freeHandle((nuint)m_Buffer);
                disposedValue = true;
            }
        }

        ~HybridLayerSegregatedFitAllocator()
        {
            Dispose(disposing:false);
        }

    }
}
