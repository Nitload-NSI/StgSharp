//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4"
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
using StgSharp.Collections;
using System.Data;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public sealed unsafe partial class L4 : IDisposable
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
        private volatile int _prefetching;
        private readonly nuint _baseAddress;
        private readonly SwissTable _map;
        private readonly UnmanagedStack<int> _idMux;

        public L4()
        {
            // init order is fallow by align requirement of each component.
            nuint cacheSize = (nuint)Unsafe.SizeOf<CacheLineData>() * CacheLineCount;   // SIMDID, no less than 16 byte bit
            nuint entrySize = (nuint)Unsafe.SizeOf<CacheLineHead>() * CacheLineCount;   // 16 byte align
            nuint predictionSize = (nuint)Unsafe.SizeOf<CacheLinePrediction>() * PredictionCount;   //16 byte align
            nuint mapSize = (nuint)SwissTable.RequestedNativeMemorySize;     // 16 byte align
            nuint rrpvSize = (nuint)Unsafe.SizeOf<EvictionArray>();           // 16 byte align
            nuint stackSize = (nuint)sizeof(int) * CacheLineCount;                      // int align

            // malloc all memory at once, and free at once in Dispose.
            // This is to avoid fragmentation and improve performance.
            _baseAddress = (nuint)NativeMemory.AlignedAlloc(cacheSize + entrySize + stackSize + mapSize + predictionSize + rrpvSize, 16);
            nuint ptr = _baseAddress;

            // init all components with the buffer, and move the buffer pointer forward.
            nuint _buffer = _baseAddress;
            _data = (CacheLineData*)_buffer;
            ptr += cacheSize;
            _head = (CacheLineHead*)ptr;
            ptr += entrySize;
            _prediction = (CacheLinePrediction*)ptr;
            ptr += predictionSize;
            _map = SwissTable.Create(ptr, _ => { });
            ptr += mapSize;
            _eviction = (EvictionArray*)ptr;
            ptr += rrpvSize;
            _idMux = new UnmanagedStack<int>((int*)ptr, CacheLineCount, _ => { });
            ptr += stackSize;
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
            _idMux.Dispose();
            NativeMemory.AlignedFree((void*)_baseAddress);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CacheLine Map(
                         nuint origin
        )
        {
            if (_aheadCount < PredictionLeastAhead)
            {
                if (Interlocked.CompareExchange(ref _prefetching, 1, 0) == 0)
                {
                    try
                    {
                        if (_aheadCount < PredictionLeastAhead)
                        {
                            // do prefetch in work load now, sync
                            Prefetch();
                        }
                    }
                    finally
                    {
                        _prefetching = 0;
                    }
                }
            }
            while (true)
            {
                if (_map.TryGet(origin, out nuint entryHandle))
                {
                    // cache hit
                    CacheLineHead* entry = (CacheLineHead*)entryHandle;
                    if (Interlocked.Increment(ref entry->_refCount) > 0)
                    {
                        (*_eviction)[entry->_id] = 0;
                        _ = Interlocked.Increment(ref _activeCount);
                        _ = Interlocked.Decrement(ref _aheadCount);
                        return new CacheLine(entry->_address, ref _activeCount, ref entry->_refCount);
                    } else
                    {
                        _ = Interlocked.Decrement(ref entry->_refCount);
                    }
                }
                Thread.Sleep(0);
            }
        }

        private partial void Prefetch();

        private partial bool TestHit(
                             nuint origin
        );

        public ref struct CacheLine : IDisposable
        {

            private ref int _activeCount;
            private ref int _refCount;
            private nuint _handle;

            internal CacheLine(
                     nuint handle,
                     ref int activeCount,
                     ref int cacheRefCount
            )
            {
                _handle = handle;
                _refCount = ref cacheRefCount;
                _activeCount = ref activeCount;
            }

            [Obsolete("You should not create a CacheLineHandle outside a L4", true)]
            public CacheLine() { }

            public nuint Address => _handle;

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
