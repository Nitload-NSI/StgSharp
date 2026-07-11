// -----------------------------------------------------------------------------
// file="L4.Evict"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="count">
        ///
        /// </param>
        /// <returns>
        ///   Count of lines not evicted
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int Evict(
                           int count = 1
        )
        {
            count = (count < 0) ? 0 : count;
            int* pPtr = _predictCount, mPtr = _mapCount;


            int remain = count;
            for (int i = 0; i < CacheLineCount; i++)
            {
                if (pPtr[i] > mPtr[i])
                {
                    continue;
                }
                remain -= TryEvictAt(i) ? 1 : 0;
            }
            return remain;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool TryEvictAt(
                            int index
        )
        {
            CacheLineMetadata* head = &_head[index];
            if (Interlocked.CompareExchange(ref head->RefCount, int.MinValue, 0) == 0)
            {
                // EvictLine(i + index);
                _ = _map.TryRemove(head->Origin, out _);
                IL4Predict predict = CurrentPredict;
                predict.WriteBack(head->Origin, head->CustomizeProfile, new ReadOnlySpan<byte>((byte*)(ulong)(head->PositionMask + _baseAddress), (int)(ulong)head->SizeMask));
                _bufferAllocator.Free(index);
                _mapCount[index] = 0;
                _predictCount[index] = int.MaxValue;
                return true;
            }
            return false;
        }

    }
}
