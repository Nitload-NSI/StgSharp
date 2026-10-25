// -----------------------------------------------------------------------------
// file="L4.Predict"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    public unsafe partial class L4
    {

        private int _active_line_count;

        private int _last_evict_pos;

        internal partial int Predict()
        {
            if (!TryGetCurrentPredictor(out IL4Predictor? p)) {
                return 0;
            }
            int total_count = 0, count;
            Span<L4.AddressSource> as_sp = stackalloc L4.AddressSource[4];
            SwissEnumerator e = _addressMap.GetWriteBufferEnumerator();
            Predict:
            while ((count = p.PredictMission(in as_sp)) > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (_addressMap.TryGetWrite(as_sp[i], out ulong map_handle_adr))
                    {
                        L4.CacheLineMetadata* map_handle = (L4.CacheLineMetadata*)map_handle_adr;
                        map_handle->IncreasePredictCount();
                        map_handle->CheckCount();
                    } else
                    {
                        if (_active_line_count >= MaxActiveCacheLineCount)
                        {
                            int retry = 0;
                            const int max_retry = 3;
                            for (; retry < max_retry && EvictOnce(ref e); retry++) {
                                e = _addressMap.GetWriteBufferEnumerator(_last_evict_pos);
                            }
                            if (retry >= max_retry)
                            {
                                // stop predicting
                                _last_evict_pos = e.CurrentOffset;
                                return total_count;
                            }
                            _active_line_count--;
                        }

                        uint size = as_sp[i].Size;
                        int handle_idx = _bufferAllocator.Alloc(size);
                        L4.CacheLineMetadata* handle = _bufferAllocator.MetadataAt(handle_idx);
                        handle->IncreasePredictCount();
                        p.Prefetch(as_sp[i], _baseAddress + handle->Position, handle->SizeMask);
                        if (!_addressMap.TryAddOrSet(as_sp[i], (ulong)handle)) {
                            throw new InvalidOperationException();
                        }
                        _active_line_count++;
                    }
                }
                total_count += count;
                if (total_count >= 2048)
                {
                    _last_evict_pos = e.CurrentOffset;
                    return total_count;
                }
            }
            if (MoveNextPredictor())
            {
                _ = TryGetCurrentPredictor(out p);
                goto Predict;
            }
            _last_evict_pos = e.CurrentOffset;
            return total_count;
        }

        private bool EvictOnce(
                     ref SwissEnumerator e
        )
        {
            while (e.MoveNext())
            {
                (AddressSource source, ulong cache_head_ptr) = e.Current;
                L4.CacheLineMetadata* handle = (L4.CacheLineMetadata*)cache_head_ptr;
                if (Volatile.Read(ref handle->ConsumeCount) == Volatile.Read(ref handle->PredictCount))
                {
                    // evict
                    int offset = e.CurrentOffset;
                    _addressMap.RemoveAtOffset(offset);
                    _bufferAllocator.Free(handle);
                    return true;
                }
            }
            return false;
        }

        #region Queue

        private volatile int _consume;
        private volatile int _enter;
        private volatile int _predict;

        private int _queue_sync;

        internal void DropPredictor(
                      IL4Predictor predictor
        )
        {
            if (predictor is null) {
                return;
            }
            SpinWait sp = new();
            while (Interlocked.CompareExchange(ref _queue_sync, 1, 0) != 0) {
                sp.SpinOnce();
            }

            try
            {
                unchecked
                {
                    for (int i = _consume; i < _predict; i++)
                    {
                        byte idx = (byte)i;
                        if (ReferenceEquals(predictor, _ring[idx]))
                        {
                            int end = _predict;
                            for (int move = i; move < end - 1; move++) {
                                _ring[(byte)move] = _ring[(byte)(move + 1)];
                            }
                            _ring[(byte)(end - 1)] = default!;
                            _predict = end - 1;
                            return;
                        }
                    }
                }
            }
            finally
            {
                _ = Interlocked.Exchange(ref _queue_sync, 0);
            }
        }

        internal bool MoveNextPredictor()
        {
            int cur = Interlocked.Increment(ref _predict);
            return cur < _enter;
        }

        internal void RegistPredictor(
                      IL4Predictor predictor
        )
        {
            if (predictor is null) {
                return;
            }
            SpinWait sp = new();

            wait_for_spare:
            WaitForSpare();

            while (Interlocked.CompareExchange(ref _queue_sync, 1, 0) != 0) {
                sp.SpinOnce();
            }
            if (_enter - _consume >= PredictorRingCapacity)
            {
                _ = Interlocked.Exchange(ref _queue_sync, 0);
                goto wait_for_spare;
            }

            try
            {
                unchecked
                {
                    byte idx = (byte)_enter;
                    _enter++;
                    _ring[idx] = predictor;
                }
            }
            finally
            {
                _ = Interlocked.Exchange(ref _queue_sync, 0);
            }
        }

        internal bool TryGetCurrentPredictor(
                      out IL4Predictor predictor
        )
        {
            int cur = _predict;
            if (cur >= _enter)
            {
                predictor = default!;
                return false;
            }
            unchecked
            {
                predictor = _ring[(byte)cur];
                return true;
            }
        }

        private void WaitForSpare()
        {
            int spin_count = 0;

            while (_enter - _consume >= PredictorRingCapacity)
            {
                switch (spin_count)
                {
                    case < 1024:
                        spin_count++;
                        break;
                    case < 65536:
                        spin_count++;
                        Thread.SpinWait(1);
                        break;
                    default:
                        Thread.Sleep(1);
                        break;
                }
            }
        }

        #endregion
    }
}
