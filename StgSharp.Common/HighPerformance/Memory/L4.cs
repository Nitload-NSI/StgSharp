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
using StgSharp.HighPerformance.Memory;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System.Data;
using System.Runtime.InteropServices;

using HLSF = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public sealed unsafe partial class L4 : IDisposable
    {

        private const int CacheLineCount = 8192;
        private const nuint PageSize = 4096;
        private const int PredictionCount = 2048;
        private const int PredictionLeastAhead = 1024;

        // private static readonly IIntrinsicKernel _intrinsic = IIntrinsicKernel.Instance;
        private readonly CacheLineHead* _head;
        private readonly MapArray* _mapCount;
        private readonly PredictionArray* _predictCount;
        private readonly CacheLineDescription* _prediction;
        private bool _disposed;
        private bool _exhausted;

        private readonly HLSF _bufferAllocator;
        private int _activeCount;

        private int _aheadCount;

        private readonly int _bindedNuma;
        private volatile int _prefetching;
        private readonly nuint _baseAddress;
        private readonly Slab _entryManager;
        private readonly SwissTable _map;

        public L4(int bindedNuma, nuint bufferSize = 8192 * 8192, int align = 16, nuint maxCacheLineSize = 8192)
        {
            _bindedNuma = bindedNuma;

            // init order is fallow by align requirement of each component.
            nuint entrySize = Slab.RequestedAllocation;   // SIMDID, no less than 16 byte bit
            nuint headSize = ((nuint)Unsafe.SizeOf<CacheLineHead>()) * CacheLineCount;   // 16 byte align
            nuint predictionSize = ((nuint)Unsafe.SizeOf<CacheLineDescription>()) * PredictionCount;   //16 byte align
            nuint mapSize = SwissTable.RequestedNativeMemorySize;     // 16 byte align
            nuint predictionCountSize = (nuint)Unsafe.SizeOf<PredictionArray>();           // 16 byte align
            nuint normalizedBufferSize = (bufferSize + (PageSize - 1)) & (~(PageSize - 1));    // align to 4096 byte, page size
            ByteCapacity = normalizedBufferSize;

            // malloc all memory at once, and free at once in Dispose.
            // This is to avoid fragmentation and improve performance.
            _baseAddress = (nuint)Numa.Alloc(bindedNuma, entrySize + headSize + mapSize + predictionSize + (predictionCountSize * 2) + normalizedBufferSize);
            nuint ptr = _baseAddress;

            AllocSize = entrySize;

            // init all components with the buffer, and move the buffer pointer forward.
            nuint _buffer = _baseAddress + normalizedBufferSize;
            _entryManager = new Slab(_buffer);
            _bufferAllocator = new HLSF(entrySize, align, maxCacheLineSize, (void*)_baseAddress, static _ => { },
                                        _entryManager);
            ptr += entrySize;
            _head = (CacheLineHead*)ptr;
            ptr += headSize;
            _prediction = (CacheLineDescription*)ptr;
            ptr += predictionSize;
            _map = SwissTable.Create(ptr, _ => { });
            ptr += mapSize;
            _predictCount = (PredictionArray*)ptr;
            ptr += predictionCountSize;
            _mapCount = (MapArray*)ptr;
            ptr += predictionCountSize;

            M512* mPtr = (M512*)_head;
            for (uint i = 0; i < CacheLineCount; i++) {
                mPtr->BroadCastFrom<ulong>(i);
            }
        }

        public nuint ByteCapacity { get; init; }

        public nuint AllocSize { get; init; }

        // public IL4Predict PrefetchPredict { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ContinueOnce(L4CacheLine line)
        {
            if (line.Address > 0) {
                _ = Interlocked.Decrement(ref _aheadCount);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Design",
                "CA1063:Implement IDisposable correctly",
                Justification = """
                    Only owns native memory via Numa.Free();
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
            _entryManager.Dispose();
            _bufferAllocator.Dispose();


            // init order is fallow by align requirement of each component.
            nuint entrySize = Slab.RequestedAllocation;   // SIMDID, no less than 16 byte bit
            nuint headSize = ((nuint)Unsafe.SizeOf<CacheLineHead>()) * CacheLineCount;   // 16 byte align
            nuint predictionSize = ((nuint)Unsafe.SizeOf<CacheLineDescription>()) * PredictionCount;   //16 byte align
            nuint mapSize = SwissTable.RequestedNativeMemorySize;     // 16 byte align
            nuint predictionCountSize = (nuint)Unsafe.SizeOf<PredictionArray>();           // 16 byte align
            nuint normalizedBufferSize = ByteCapacity;    // align to 4096 byte, page size
            Numa.Free((void*)_baseAddress, _bindedNuma, entrySize + headSize + mapSize + predictionSize + (predictionCountSize * 2) + normalizedBufferSize);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public L4CacheLine Map(ref CacheLineDescription prediction)
        {
            if ((_aheadCount < PredictionLeastAhead) && !_exhausted)
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
                if (_map.TryGet(prediction.GetHashCodeLong(), out nuint entryHandle))
                {
                    // cache hit
                    CacheLineHead* entry = (CacheLineHead*)entryHandle;
                    if (Interlocked.Increment(ref entry->RefCount) > 0)
                    {
                        (*_predictCount)[entry->Id] = 0;
                        _ = Interlocked.Increment(ref _activeCount);
                        _ = Interlocked.Decrement(ref _aheadCount);
                        _ = Interlocked.Decrement(ref (*_mapCount)[entry->Id]);
                        return new L4CacheLine(entry->Address, ref _activeCount, ref entry->RefCount);
                    } else
                    {
                        _ = Interlocked.Decrement(ref entry->RefCount);
                    }
                }
                Thread.Sleep(0);
            }
        }

        private partial void Prefetch();

    }

    public ref struct L4CacheLine : IDisposable
    {

        private ref int _activeCount;
        private ref int _refCount;
        private nuint _handle;

        internal L4CacheLine(nuint handle, ref int activeCount, ref int cacheRefCount)
        {
            _handle = handle;
            _refCount = ref cacheRefCount;
            _activeCount = ref activeCount;
        }

        [Obsolete("You should not create a CacheLineHandle instance outside a L4", true)]
        public L4CacheLine() { }

        public readonly nuint Address => _handle;

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
}
