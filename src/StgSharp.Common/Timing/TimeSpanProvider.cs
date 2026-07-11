// -----------------------------------------------------------------------------
// file="TimeSpanProvider"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Timing;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.Timing
{
    /// <summary>
    ///   TimeSpanProvider: uses a bound TimeSequence and TimeSourceProviderBase to publish span
    ///   ticks. Each OnSpanTick advances the sequence; when sequence ends provider unsubscribes.
    /// </summary>
    public partial class TimeSpanProvider
    {

        // private bool _subscribed;
        private readonly int _emittedSpanCount; // legacy counter

        // internal ended flag (0 = running, 1 = ended)
        private int _endedFlag;
        private readonly TimeSequence _sequence;
        private readonly TimeSourceProviderBase _timeSource;

        public TimeSpanProvider(
               TimeSequence sequence,
               TimeSourceProviderBase provider,
               int maxWaitCount = 1
        )
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(sequence);
            _timeSource = provider;
            _sequence = sequence;
            _sequence.StartFrom(provider);
            provider.AddSubscriber(this);

            // _subscribed = true;
            if (!s_unusedID.TryPop(out int id))
            {
                id = Interlocked.Increment(ref s_maxID);
            }
            TokenID = id;
            _size = maxWaitCount > 0 ? maxWaitCount : 1;
            _event = new ManualResetEventSlim(initialState:false, spinCount:0);
            _pendingSpans = 0;
        }

        /// <summary>
        ///   True when the provider has reached its end and will not emit further spans.
        /// </summary>
        public bool IsEnded => Volatile.Read(ref _endedFlag) != 0;

        /// <summary>
        ///   Current emitted time in seconds relative to the sequence beginning.
        /// </summary>
        public double CurrentSecond => (double)(_sequence.PreviousFrameTick - _sequence.BeginningTick) / _timeSource.Frequency;

        public int CurrentSpan => _emittedSpanCount;

        public static TimeSourceProviderBase DefaultProvider => World.MainTimeProvider;

        public static int ToInt32(
                          TimeSpanProvider provider
        )
        {
            return provider?._emittedSpanCount ?? 0;
        }

        public static explicit operator int(
                                        TimeSpanProvider provider
        )
        {
            return provider?._emittedSpanCount ?? 0;
        }

    }
}
