// -----------------------------------------------------------------------------
// file="L4.PredictorQueue"
// Project: StgSharp
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
    public partial class L4
    {

        private readonly IL4Predictor[] _ring = new IL4Predictor[256];
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

        internal void RegistPredictor(
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

            WaitForSpare();

            _consume++;
            try
            {
                unchecked
                {
                    byte idx = (byte)_consume;
                    _ring[idx] = predictor;
                }
            }
            finally
            {
                _ = Interlocked.Exchange(ref _queue_sync, 0);
            }
        }

        private void WaitForSpare()
        {
            int spin_count = 0;

            // wait until the queue has space
            while (_enter - _consume >= 256)
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

    }
}
