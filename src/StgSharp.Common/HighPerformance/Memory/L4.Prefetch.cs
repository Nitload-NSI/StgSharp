// -----------------------------------------------------------------------------
// file="L4.Prefetch"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.ComponentModel;
using System.Numerics;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class L4
    {

        private partial void Prefetch()
        {
            if (CurrentPredict is null) {
                return;
            }
            IL4Predict predict = CurrentPredict;
            Span<CacheLineDescription> span = new(_prediction, PredictionCount);
            PredictResult result = predict.Predict(span, out int actualCount);
            _exhausted = (result & PredictResult.Exhausted) != 0;
            if (actualCount <= 0)
            {
                _exhausted = true;
                return;
            }
            int missIndex = 0;
            for (int i = 0; i < actualCount; i++)
            {
                ref CacheLineDescription handle = ref span[i];
                nuint address = handle.Address;
                if (!_map.TryGet(address, out _))
                {
                    // miss and record
                    span[missIndex] = handle;
                    missIndex++;
                }
            }

            // now prefetch the misses
            for (int i = 0; i < missIndex; i++)
            {
                CacheLineMetadata* head;
                ref CacheLineDescription pHandle = ref span[i];
                nuint address = pHandle.Address;
                int index;
                if (_map.TryGet(address, out nuint handle))
                {
                    head = (CacheLineMetadata*)handle;
                    index = (int)(head - _head);
                    _predictCount[index] = 0; // reset eviction counter

                    #region update predictor belonging

                    if (_predictCount[index] > short.MaxValue / 2)
                    {
                        int predictCount = Volatile.Read(ref _predictCount[index]);
                        int mapCount = Volatile.Read(ref _mapCount[index]);
                        int decrement = (int.Min(predictCount, mapCount) + 1) / 2;
                        _ = Interlocked.Add(ref _predictCount[index], -decrement);
                        _ = Interlocked.Add(ref _mapCount[index], -decrement);
                    }
                    _predictCount[index]++;

                    #endregion

                    continue;
                }

                int retryCount = 0, remain = missIndex - i;
                const int maxRetry = 1024;
                while (!_bufferAllocator.TryAlloc(pHandle.Size, out index))
                {
                    remain = Evict(remain);
                    retryCount++;
                    if (retryCount > maxRetry) {
                        throw new OverflowException($"Failed to evict cache line after {maxRetry} retries.");
                    }
                    if (retryCount > maxRetry / 2) {
                        Thread.Sleep(1);
                    }
                }
                head = _head + index;
                head->Origin = address;
                head->CustomizeProfile = pHandle.MapPolicy;
                head->RefCount = 0;

                _predictCount[index] = 1;
                _mapCount[index] = 1;

                // pre fetch them here
                predict.Prefetch(head->Origin, head->CustomizeProfile, new Span<byte>((byte*)(head->PositionMask + _baseAddress), (int)head->SizeMask));
                _ = _map.TryAddOrSet(address, (nuint)head, out _);
            }
            _ = Interlocked.Add(ref _aheadCount, actualCount);
        }

    }
}
