// -----------------------------------------------------------------------------
// file="L4"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.ProcessorAbstraction;

namespace Nitload.Common.HighPerformance.Memory
{
    public sealed unsafe partial class L4 : IDisposable
    {

        private const int MaxActiveCacheLineCount = 8192;

        private const nuint PageSize = 4096;
        private const int PredictorRingCapacity = 256;

        private readonly IL4Predictor[] _ring;

        private bool _disposed;
        private readonly int _bindedNuma;
        private readonly nuint _baseAddress;
        private SwissTable _addressMap;
        private TLSF _bufferAllocator;

        public L4(
               int bindedNuma,
               uint bufferSize = 8192 * 8192,
               int align = 16,
               uint maxCacheLineSize = 8192
        )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bufferSize, CacheLineMetadata.MaxBufferSize);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxCacheLineSize, CacheLineMetadata.MaxBufferSize);
            _ring = new IL4Predictor[PredictorRingCapacity];
            _bindedNuma = bindedNuma;
            _predict_slow_path_count = GetPredictSlowPathCount(bindedNuma);
            nuint normalizedBufferSize = (bufferSize + (PageSize - 1)) & (~(PageSize - 1));
            ByteCapacity = normalizedBufferSize;
            nuint swissBufferOffset = (normalizedBufferSize + TLSF.RequestedAllocation + 15u) & ~15u;
            AllocSize = swissBufferOffset + (nuint)BufferSize;
            _baseAddress = (nuint)Numa.Alloc(bindedNuma, AllocSize);
            _bufferAllocator = new TLSF(bufferSize, maxCacheLineSize, (void*)_baseAddress);
            _addressMap = SwissTable.Create((byte*)(_baseAddress + swissBufferOffset));
        }

        public nuint ByteCapacity { get; init; }

        public nuint AllocSize { get; init; }

        public void Dispose()
        {
            if (_disposed) {
                return;
            }
            _disposed = true;
            _addressMap.Dispose();
            Numa.Free((void*)_baseAddress, _bindedNuma, AllocSize);
            GC.SuppressFinalize(this);
        }

        public partial Handle Map(
                              AddressSource source
        );

        internal void Collect()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            _bufferAllocator.Collect();
        }

        internal void Free(
                      int handle
        )
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            _bufferAllocator.Free(handle);
        }

        internal partial int Predict();

        internal bool TryAllocate(
                      uint size,
                      out int handle
        )
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return _bufferAllocator.TryAlloc(size, out handle);
        }

        private static int GetPredictSlowPathCount(
                           int bindedNuma
        )
        {
            foreach (NumaNode node in Numa.AllNumaNode)
            {
                if (node.NumaNodeId == (uint)bindedNuma) {
                    return checked(node.LogicalCoreCount * 2);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(bindedNuma), "No topology information exists for the bound NUMA node.");
        }

    }
}
