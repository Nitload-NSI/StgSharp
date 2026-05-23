//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Prefetch"
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
            int predictIndex = CurrentPredictIndex;
            Span<CacheLineDescription> span = new(_prediction, PredictionCount);
            PredictResult result = predict.Predict(span, out int actualCount);
            if ((result & PredictResult.Exhausted) != 0) {
                _exhausted = !TryGetNextPredict();
            }
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
            _predictors.RequestSpanOperation();
            Span<IL4Predict> predictorSpan = _predictors.AsSpan();
            try
            {
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
                        index = head->Id;
                        (*_predictCount)[index] = 0; // reset eviction counter

                        #region update predictor belonging

                        int predictId = head->Predictor;
                        IL4Predict belong = predictorSpan[predictId];

                        if (!ReferenceEquals(predict, belong))
                        {
                            throw new L4PredictConflictException(head->Origin, head->Profile, predictId);
                        } else
                        {
                            if ((*_predictCount)[index] > short.MaxValue / 2)
                            {
                                int predictCount = Volatile.Read(ref (*_predictCount)[index]);
                                int mapCount = Volatile.Read(ref (*_mapCount)[index]);
                                int decrement = (int.Min(predictCount, mapCount) + 1) / 2;
                                _ = Interlocked.Add(ref (*_predictCount)[index], -decrement);
                                _ = Interlocked.Add(ref (*_mapCount)[index], -decrement);
                            }
                            (*_predictCount)[index]++;
                        }

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
                    head->Profile = pHandle.MapPolicy;
                    head->RefCount = 0;
                    head->Predictor = (short)predictIndex;

                    (*_predictCount)[index] = 1;
                    (*_mapCount)[index] = 1;

                    // pre fetch them here
                    predict.Prefetch(head->Origin, head->Profile, new Span<byte>((byte*)(ulong)head->Position, (int)(ulong)head->SizeForAllocator));
                    _ = _map.TryAddOrSet(address, (nuint)head, out _);
                }
                _ = Interlocked.Add(ref _aheadCount, actualCount);
            }
            finally
            {
                _predictors.ReleaseSpanOperation();
            }
        }

    }
}
