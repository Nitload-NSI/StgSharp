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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Timing
{
    /// <summary>
    ///   A microsecond-accuracy timer. It will refresh every a certain time span.
    /// </summary>
    public class TimeSpanProvider
    {

        private bool _subscribed;

        private readonly ConcurrentQueue<TimeSpanAwaitingToken> _tokenToAdd = new();
        private readonly ConcurrentQueue<TimeSpanAwaitingToken> _tokenToQuit;
        private Counter<int> timeSpanCount = new(-1, 1, 1);
        private readonly HashSet<TimeSpanAwaitingToken> _awaitingTokens;
        private long _maxSpanCount;
        private long _spanBegin;
        private readonly long _spanLength;
        private readonly TimeSourceProviderBase _timeSource;

        /// <summary>
        ///   Create a new <see cref="TimeSpanProvider" /> and define the length of time span.
        ///   Length of a single time span is defined in microseconds.
        /// </summary>
        /// <param _label="spanMicroSeconds">
        ///
        /// </param>
        /// <param _label="provider">
        ///
        /// </param>
        public TimeSpanProvider(long spanMicroSeconds, TimeSourceProviderBase provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _timeSource = provider;
            _spanLength = spanMicroSeconds;
            _maxSpanCount = int.MaxValue;
            provider.AddSubscriber(this);
            _subscribed = true;
            _awaitingTokens = [];
            _tokenToQuit = new ConcurrentQueue<TimeSpanAwaitingToken>();
        }

        /// <summary>
        ///   Create a new <see cref="TimeSpanProvider" /> and define the length of time span. 
        ///   Length of a single time span is defined in seconds.
        /// </summary>
        /// <param _label="spanSeconds">
        ///
        /// </param>
        /// <param _label="provider">
        ///
        /// </param>
        public TimeSpanProvider(double spanSeconds, TimeSourceProviderBase provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _timeSource = provider;
            _spanLength = (long)(spanSeconds * 1000L * 1000L);
            _spanBegin = -_spanLength;
            _maxSpanCount = long.MaxValue;
            provider.AddSubscriber(this);
            _subscribed = true;
            _awaitingTokens = [];
            _tokenToQuit = new ConcurrentQueue<TimeSpanAwaitingToken>();
        }

        /// <summary>
        ///   Create a new <see cref="TimeSpanProvider" /> and define the length of time span.
        ///   Length of a single time span is defined in microseconds.
        /// </summary>
        /// <param _label="spanLength">
        ///
        /// </param>
        /// <param _label="maxSpan">
        ///
        /// </param>
        /// <param _label="provider">
        ///
        /// </param>
        public TimeSpanProvider(long spanLength, long maxSpan, TimeSourceProviderBase provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            if (maxSpan <= 0) {
                throw new ArgumentException("Sapn count cannot be smaller than zero");
            }
            _timeSource = provider;
            _spanLength = spanLength;
            _spanBegin = -_spanLength;
            provider.AddSubscriber(this);
            _subscribed = true;
            _awaitingTokens = new HashSet<TimeSpanAwaitingToken>();
            _tokenToQuit = new ConcurrentQueue<TimeSpanAwaitingToken>();
        }

        public double SpanLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _spanLength * 1.0 / (1000L * 1000L);
        }

        public float CurrentSecond
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => timeSpanCount.Value * SpanLengthLowPrecession;
        }

        public float SpanLengthLowPrecession
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _spanLength * 1.0f / (1000 * 1000);
        }

        public int CurrentSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => timeSpanCount.Value;
        }

        /// <summary>
        ///   Time source provider provided in <see cref="World" /> internal framework.
        /// </summary>
        public static TimeSourceProviderBase DefaultProvider => StgSharpTime.OnlyInstance;

        public TimeSpanAwaitingToken ParticipantSpanAwaiting(int count)
        {
            TimeSpanAwaitingToken token = new(this, count);
            _tokenToAdd.Enqueue(token);
            return token;
        }

        public void QuitSpanAwaiting(TimeSpanAwaitingToken token)
        {
            _tokenToQuit.Enqueue(token);
        }

        public void StopSpanProviding()
        {
            _ = Interlocked.Exchange(ref _maxSpanCount, 0);
        }

        public void SubscribeToTimeSource(TimeSourceProviderBase serviceHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceHandler);
            if (_subscribed) {
                return;
            }
            _subscribed = true;
            serviceHandler.AddSubscriber(this);
        }

        public int ToInt32()
        {
            return timeSpanCount.Value;
        }

        internal bool CheckTime(long microseconds)
        {
            if (microseconds - _spanBegin < _spanLength) {
                return true;
            }
            foreach (TimeSpanAwaitingToken tokenAdd in _tokenToAdd) {
                _ = _awaitingTokens.Add(tokenAdd);
            }
            _tokenToAdd.Clear();
            foreach (TimeSpanAwaitingToken tokenQuit in _tokenToQuit)
            {
                _ = _awaitingTokens.Remove(tokenQuit);
                DefaultLog.InternalWriteLog(
                    $"Awaiting token {tokenQuit!.TokenID} has been removed.", LogType.Info);
            }
            _tokenToQuit.Clear();

            _spanBegin = microseconds;
            timeSpanCount++;

            foreach (TimeSpanAwaitingToken item in _awaitingTokens)
            {
                if (item.AwaitingSemaphoreSlim.CurrentCount == 0)
                {
                    _ = item.AwaitingSemaphoreSlim.Release();
                    item.Refresh();
                } else
                {
                    item.MissRefresh();
                }
            }

            return timeSpanCount < _maxSpanCount;
        }

        public static explicit operator int([NotNull]TimeSpanProvider provider)
        {
            return provider.ToInt32();
        }

    }
}