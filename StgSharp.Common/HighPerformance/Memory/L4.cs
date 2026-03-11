//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using StgSharp.Collections;
using StgSharp.Mathematics.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class L4 : IDisposable
    {

        private const int CacheLineCount = 8192;
        private const int PredictionCount = 2048;
        private const int PredictionLeastAhead = 1024;

        private readonly CacheLineData* _data;

        private readonly EvictionArray* _eviction;
        private readonly CacheLineHead* _head;
        private readonly CacheLinePrediction* _prediction;

        private bool _disposed;
        private int _activeCount;

        private int _aheadCount;
        private int _maxId;
        private readonly nuint _baseAddress;
        private readonly SwissTable _map;
        private readonly UnmanagedStack<int> _idMux;

        public L4()
        {
            int cacheSize = Unsafe.SizeOf<CacheLineData>() * CacheLineCount;
            int entrySize = Unsafe.SizeOf<CacheLineHead>() * CacheLineCount;
            int stackSize = sizeof(int) * CacheLineCount;
            int mapSize = SwissTable.RequestedNativeMemorySize;
            int predictionSize = Unsafe.SizeOf<CacheLinePrediction>() * PredictionCount;
            int rrpvSize = Unsafe.SizeOf<EvictionArray>();
            _baseAddress = (nuint)NativeMemory.AlignedAlloc((nuint)(cacheSize + entrySize + stackSize + mapSize + predictionSize + rrpvSize), 16);
            nuint _buffer = _baseAddress;
            _data = (CacheLineData*)_buffer;
            _head = (CacheLineHead*)(_buffer + (nuint)cacheSize);
            _prediction = (CacheLinePrediction*)(_buffer + (nuint)(cacheSize + entrySize));
            _map = SwissTable.Create(_baseAddress + (nuint)(cacheSize + entrySize), _ => { });
            _eviction = (EvictionArray*)(_baseAddress + (nuint)(cacheSize + entrySize + mapSize));
        }

        public IPrefetchPredict PrefetchPredict { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Design",
                "CA1063:Implement IDisposable correctly",
                Justification = """
                    Only owns native memory via NativeMemory.AlignedFree();
                    no managed IDisposable fields.
                    Dispose(bool) pattern is unnecessary.
                    """)]
        public void Dispose()
        {
            if (_disposed) {
                return;
            }
            _disposed = true;
            _map.Dispose();
            NativeMemory.AlignedFree((void*)_baseAddress);
            GC.SuppressFinalize(this);
        }

        public CacheLine Map(nuint origin)
        {
            if (_map.TryGet(origin, out nuint entryHandle))
            {
                // cache hit
                CacheLineHead* entry = (CacheLineHead*)entryHandle;
                return new CacheLine(entry->_address, ref _activeCount, ref entry->_refCount);
            }

            // cache miss
            /*
             * post command
             * then wait for completion
             */
            return /*origin/**/default;   //error now
        }

        private partial void Prefetch();

        private partial bool TestHit(nuint origin);

        public ref struct CacheLine : IDisposable
        {

            private ref int _activeCount;
            private ref int _refCount;
            private nuint _handle;

            internal CacheLine(nuint handle, ref int activeCount, ref int cacheRefCount)
            {
                _handle = handle;
                _refCount = ref cacheRefCount;
                _activeCount = ref activeCount;
                _ = Interlocked.Increment(ref _activeCount);
                _ = Interlocked.Increment(ref _refCount);
            }

            public void Dispose()
            {
                if (_handle == 0) {
                    return;
                }
                _handle = 0;
                _ = Interlocked.Decrement(ref _activeCount);
                _ = Interlocked.Decrement(ref _refCount);
            }

        }

        private struct CacheLineData
        {

            private fixed byte Data[8192];

        }

    }
}
