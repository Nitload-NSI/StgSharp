//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.cs"
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
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator : IDisposable
    {

        private static readonly nuint EmptyHandle = (nuint)(void*)null;

        // Bucket system field - directly in main allocator class
        private ulong[] _bucketHeads;

        private Entry* _spareMemory;
        private readonly byte* m_Buffer;
        private bool disposedValue;
        private readonly int _align;
        private readonly nuint _size;
        private readonly SlabAllocator<Entry> _entries;

        public HybridLayerSegregatedFitAllocator(nuint byteSize)
        {
            if (byteSize < 64) {
                throw new InvalidOperationException();
            }
            _entries = SlabAllocator<Entry>.Create(64, SlabBufferLayout.Chunked, false);
            _size = byteSize;
            m_Buffer = (byte*)NativeMemory.AlignedAlloc(byteSize, 16);
            _align = 16;

            InitializeBucketSystem();

            _spareMemory = (Entry*)_entries.Allocate();
            _spareMemory->State = EntryState.Empty;
            _spareMemory->PreviousNear = EmptyHandle;
            _spareMemory->NextNear = EmptyHandle;
            _spareMemory->Position = (nuint)m_Buffer;
            _spareMemory->Size = (uint)byteSize;
        }

        public HybridLayerSegregatedFitAllocator(nuint byteSize, int align)
        {
            if (align < 16 || (align & (align - 1)) != 0) {
                throw new ArgumentException(
                    "Alignment must be a power of two and at least 8 bytes.");
            }
            _entries = SlabAllocator<Entry>.Create(64, SlabBufferLayout.Chunked);
            m_Buffer = (byte*)NativeMemory.AlignedAlloc(byteSize, (nuint)align);
            _align = align;
            _size = byteSize;

            InitializeBucketSystem();

            _spareMemory = (Entry*)_entries.Allocate();
            _spareMemory->State = EntryState.Empty;
            _spareMemory->PreviousNear = EmptyHandle;
            _spareMemory->NextNear = EmptyHandle;
            _spareMemory->Position = (nuint)m_Buffer;
            _spareMemory->Size = (uint)byteSize;
        }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) {
                    _entries.Dispose();
                }
                NativeMemory.AlignedFree(m_Buffer);
                disposedValue = true;
            }
        }

        ~HybridLayerSegregatedFitAllocator()
        {
            Dispose(disposing:false);
        }

    }
}
