//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TimeSpanProvider"
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
using StgSharp.Common.Timing;
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
        private int _emittedSpanCount; // legacy counter

        // internal ended flag (0 = running, 1 = ended)
        private int _endedFlag;
        private readonly TimeSequence _sequence;
        private readonly TimeSourceProviderBase _timeSource;

        public TimeSpanProvider(
               TimeSequence sequence,
               TimeSourceProviderBase provider,
               int maxWaitCount = 1)
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
        public double CurrentSecond
        {
            get => (double)(_sequence.PreviousFrameTick - _sequence.BeginningTick) / _timeSource.Frequency;
        }

        public int CurrentSpan => _emittedSpanCount;

        public static TimeSourceProviderBase DefaultProvider => World.MainTimeProvider;

        public static explicit operator int(TimeSpanProvider provider) => provider?._emittedSpanCount ??
            0;

    }
}