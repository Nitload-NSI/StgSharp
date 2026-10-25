// -----------------------------------------------------------------------------
// file="L4.Map"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    public unsafe partial class L4
    {

        private const int _low_predict = 512;

        private int _predict_count;
        private readonly int _predict_slow_path_count;
        private int _swiss_reader_counter;
        private int _swiss_swap_lock;
        private int _swiss_writer_lock;

        public partial Handle Map(
                              AddressSource source
        )
        {
            ulong handle;
            int predictCount = Volatile.Read(ref _predict_count);
            if (predictCount < _low_predict)
            {
                if (Interlocked.CompareExchange(ref _swiss_writer_lock, 1, 0) == 0)
                {
                    try
                    {
                        int effectivePredictionCount = Predict();
                        if (effectivePredictionCount > 0)
                        {
                            _ = Interlocked.Exchange(ref _swiss_swap_lock, 1);
                            while (Volatile.Read(ref _swiss_reader_counter) != 0) { }

                            _addressMap.SwapBuffers();
                            _ = Interlocked.Add(ref _predict_count, effectivePredictionCount);
                            _addressMap.TryRehash();
                        }

                        if (effectivePredictionCount < 2048)
                        {
                            // TODO: Implement prediction-exhausted mode and keep checking predictors for more work.
                        }

                        if (!_addressMap.TryGet(source, out handle)) {
                            throw new InvalidOperationException($"Unexpected map request: [address source: 0x{source.Address:x16}; layout profile: {source.Description:x16}].");
                        }
                    }
                    finally
                    {
                        _ = Interlocked.Exchange(ref _swiss_swap_lock, 0);
                        _ = Interlocked.Exchange(ref _swiss_writer_lock, 0);
                    }
                } else if (!TryGetAddress(source, out handle)) {
                    throw new InvalidOperationException($"Unexpected map request: [address source: 0x{source.Address:x16}; layout profile: {source.Description:x16}].");
                }
            } else if (predictCount < _low_predict + _predict_slow_path_count)
            {
                if (!TryGetAddress(source, out handle)) {
                    throw new InvalidOperationException($"Unexpected map request: [address source: 0x{source.Address:x16}; layout profile: {source.Description:x16}].");
                }
            } else if (!_addressMap.TryGet(source, out handle)) {
                throw new InvalidOperationException($"Unexpected map request: [address source: 0x{source.Address:x16}; layout profile: {source.Description:x16}].");
            }

            CacheLineMetadata* metadata = (CacheLineMetadata*)handle;
            metadata->IncreaseMapCount();

            nuint address = _baseAddress + metadata->Position;
            _ = Interlocked.Decrement(ref _predict_count);

            return new Handle(address, (nuint)handle);
        }

        private bool TryGetAddress(
                     AddressSource source,
                     out ulong handle
        )
        {
            while (true)
            {
                while (Volatile.Read(ref _swiss_swap_lock) != 0) { }

                _ = Interlocked.Increment(ref _swiss_reader_counter);
                if (Volatile.Read(ref _swiss_swap_lock) == 0)
                {
                    break;
                }

                _ = Interlocked.Decrement(ref _swiss_reader_counter);
            }

            try
            {
                return _addressMap.TryGet(source, out handle);
            }
            finally
            {
                _ = Interlocked.Decrement(ref _swiss_reader_counter);
            }
        }

    }
}
