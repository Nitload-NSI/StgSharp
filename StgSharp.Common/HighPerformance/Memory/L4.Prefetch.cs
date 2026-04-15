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

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class L4
    {

        public const int MaxActivateCacheLineCount = 1024;

        private partial void Prefetch()
        {
            if (CurrentPredict is null) {
                return;
            }
            if (_activeCount > MaxActivateCacheLineCount) {
                throw new OverflowException("Too many activated cache line in external reference.");
            }
            IL4Predict predict = CurrentPredict;
            int predictIndex = CurrentPredictIndex;
            Span<CacheLinePrediction> span = new(_prediction, PredictionCount);
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
            AgeAll();
            for (int i = 0; i < actualCount; i++)
            {
                ref CacheLinePrediction handle = ref span[i];
                nuint address = handle.Address;
                if (!TestHit(address))
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
                    CacheLineHead* head;
                    ref CacheLinePrediction pHandle = ref span[i];
                    nuint address = pHandle.Address;
                    int index;
                    if (_map.TryGet(address, out nuint handle))
                    {
                        head = (CacheLineHead*)handle;
                        index = head->_id;
                        (*_eviction)[index] = 0; // reset eviction counter

                        #region update predictor belonging

                        int predictId = head->_predictor;
                        IL4Predict belong = predictorSpan[predictId];
                        // This branch performs resident-line adoption rather than a simple policy
                        // equality check. Returning true means the current predictor can keep using
                        // the existing cache bytes without a write-back plus re-prefetch cycle.
                        if (predict.IsSamePolicy(head->_profile, belong))
                        {
                            head->_predictor =predictIndex;
                        } else
                        {
                            // Compatibility failed, so the old owner must first flush its view of
                            // the line before the new predictor rematerializes the cache bytes.
                            belong.WriteBack(head->_origin, head->_profile, new ReadOnlySpan<byte>((byte*)head->_address, Unsafe.SizeOf<CacheLineData>()));
                            head->_profile = pHandle.MapPolicy;
                            predict.Prefetch(head->_origin, head->_profile, new Span<byte>((byte*)head->_address, Unsafe.SizeOf<CacheLineData>()));
                        }

                        #endregion

                        continue;
                    }

                    int retryCount = 0, remain = missIndex - i;
                    const int maxRetry = 1024;
                    while (!TryGetEmptyLine(out index))
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
                    head = &_head[index];
                    head->_id = index;
                    head->_origin = address;
                    head->_profile = pHandle.MapPolicy;
                    head->_refCount = 0;
                    head->_predictor = predictIndex;
                    head->_address = (nuint)(_data + index);

                    // pre fetch them here
                    predict.Prefetch(head->_origin, head->_profile, new Span<byte>((byte*)head->_address, Unsafe.SizeOf<CacheLineData>()));
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
