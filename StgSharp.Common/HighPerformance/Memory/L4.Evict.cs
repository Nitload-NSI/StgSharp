//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Evict"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void AgeAll()
        {
            _intrinsic.AgeAll(_eviction);
        }

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
            int remain = count;
            Vector128<byte> three = Vector128.Create((byte)0x03);
            byte* p = (byte*)_eviction;
            _predictors.RequestSpanOperation();
            Span<IL4Predict> pSpan = _predictors.AsSpan();
            try
            {
                for (int i = 0; (i < CacheLineCount) && (remain > 0); i += 16)
                {
                    int evictMask = _intrinsic.ScanEvict(p + i, three);
                    while ((evictMask != 0) && (remain > 0))
                    {
                        int bitPos = BitOperations.TrailingZeroCount(evictMask);
                        int index = bitPos + i;
                        CacheLineHead* head = &_head[index];
                        if (Interlocked.CompareExchange(ref head->_refCount, int.MinValue, 0) == 0)
                        {
                            // EvictLine(i + index);
                            _ = _map.TryRemove(head->_origin, out _);
                            int id = (int)head->_predictor;
                            IL4Predict predict = pSpan[id] ??
                                throw new L4PredictUnregisteredException(id);
                            predict.WriteBack(head->_origin, head->_profile, new ReadOnlySpan<byte>((byte*)head->_address, Unsafe.SizeOf<CacheLineData>()));
                            RecycleLine(index);
                            remain--;
                        }
                        evictMask &= evictMask - 1;
                    }
                }
                return remain;
            }
            finally
            {
                _predictors.ReleaseSpanOperation();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RecycleLine(
                     int id
        )
        {
            _idMux.Push(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetEmptyLine(
                     out int index
        )
        {
            if (_idMux.TryPop(out index)) {
                return true;
            }
            index = _maxId;
            return Interlocked.Increment(ref _maxId) <= CacheLineCount;
        }

    }
}
