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
using StgSharp.HighPerformance.Memory;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System.Data;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public sealed unsafe partial class L4 : IDisposable
    {

        private const int CacheLineCount = 8192;
        private const nuint PageSize = 4096;
        private const int PredictionCount = 2048;
        private const int PredictionLeastAhead = 1024;

        /// <summary>
        ///   Array of metadata for each cache line, also used as metadata for tlsf allocator.
        /// </summary>
        private readonly CacheLineMetadata* _head;
        /// <summary>
        ///   Count of cache lines mapped for each prediction, used for eviction policy.
        /// </summary>
        private readonly MapArray* _mapCount;
        /// <summary>
        ///   Count of times each cache line is predicted, used for eviction policy.
        /// </summary>
        private readonly PredictionArray* _predictCount;
        /// <summary>
        ///   Original memory view for prediction span.
        /// </summary>
        private readonly CacheLineDescription* _prediction;
        private bool _disposed;
        private bool _exhausted;

        private int _aheadCount;

        private readonly int _bindedNuma;
        private volatile int _prefetching;
        private readonly nuint _baseAddress;
        private readonly SwissTable _map;

        private readonly TLSF _bufferAllocator;

        public L4(
               int bindedNuma,
               nuint bufferSize = 8192 * 8192,
               int align = 16,
               nuint maxCacheLineSize = 8192
        )
        {
            _bindedNuma = bindedNuma;

            // init order is fallow by align requirement of each component.
            nuint headSize = ((nuint)Unsafe.SizeOf<CacheLineMetadata>()) * CacheLineCount;   // 16 byte align
            nuint predictionSize = ((nuint)Unsafe.SizeOf<CacheLineDescription>()) * PredictionCount;   //16 byte align
            nuint mapCountSize = SwissTable.RequestedNativeMemorySize;     // 16 byte align
            nuint predictionCountSize = (nuint)Unsafe.SizeOf<PredictionArray>();           // 16 byte align
            nuint normalizedBufferSize = (bufferSize + (PageSize - 1)) & (~(PageSize - 1));    // align to 4096 byte, page size
            ByteCapacity = normalizedBufferSize;

            AllocSize = headSize + mapCountSize + predictionSize + (predictionCountSize * 2) + normalizedBufferSize;

            // malloc all memory at once, and free at once in Dispose.
            // This is to avoid fragmentation and improve performance.
            _baseAddress = (nuint)Numa.Alloc(bindedNuma, AllocSize);
            nuint ptr = _baseAddress;

            // init all components with the buffer, and move the buffer pointer forward.
            nuint _buffer = _baseAddress;
            _bufferAllocator = new TLSF(bufferSize, maxCacheLineSize, (void*)_baseAddress);
            _buffer += normalizedBufferSize;
            _head = (CacheLineMetadata*)ptr;
            ptr += headSize;
            _prediction = (CacheLineDescription*)ptr;
            ptr += predictionSize;
            _map = SwissTable.Create(ptr, _ => { });
            ptr += mapCountSize;
            _predictCount = (PredictionArray*)ptr;
            ptr += predictionCountSize;
            _mapCount = (MapArray*)ptr;
            ptr += predictionCountSize;

            M512* mPtr = (M512*)_head;
            for (uint i = 1; i < CacheLineCount; i++) {
                mPtr->BroadCastFrom<ulong>(0);
            }
        }

        public nuint ByteCapacity { get; init; }

        public nuint AllocSize { get; init; }

        // public IL4Predict PrefetchPredict { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ContinueOnce(
                    Handle line
        )
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
#pragma warning disable CA1816
            GC.SuppressFinalize(_bufferAllocator);
#pragma warning restore CA1816


            // init order is fallow by align requirement of each component.
            Numa.Free((void*)_baseAddress, _bindedNuma, AllocSize);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Handle Map(
                      ref CacheLineDescription prediction
        )
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
                    CacheLineMetadata* entry = (CacheLineMetadata*)entryHandle;
                    if (Interlocked.Increment(ref entry->RefCount) > 0)
                    {
                        (*_predictCount)[entry->Id] = 0;
                        _ = Interlocked.Decrement(ref _aheadCount);
                        _ = Interlocked.Decrement(ref (*_mapCount)[entry->Id]);
                        return new Handle((nuint)entry->Position, ref entry->RefCount);
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
}
