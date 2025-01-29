//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TimeSpanProvider.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Timing
{
    /// <summary>
    /// A microsecond-accuracy timer. It will refresh every a certain time span.
    /// </summary>
    public class TimeSpanProvider
    {

        private bool _subscribed;

        private ConcurrentQueue<TimeSpanAwaitingToken> _tokenToAdd = new ConcurrentQueue<TimeSpanAwaitingToken>(
            );
        private ConcurrentQueue<TimeSpanAwaitingToken> _tokenToQuit;
        private Counter<int> timeSpanCount = new Counter<int>( -1, 1, 1 );
        private HashSet<TimeSpanAwaitingToken> _awaitingTokens;
        private long _maxSpanCount;
        private long _spanBegin;
        private readonly long _spanLength;
        private readonly TimeSourceProviderBase _timeSource;

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider" /> and define the length of time span. Length
        /// of a single time span is defined in microseconds.
        /// </summary>
        /// <param _name="spanMicroSeconds"></param>
        /// <param _name="provider"></param>
        public TimeSpanProvider(
            long spanMicroSeconds,
            TimeSourceProviderBase provider )
        {
            if( provider == null ) {
                throw new ArgumentNullException( nameof( provider ) );
            }
            _timeSource = provider;
            _spanLength = spanMicroSeconds;
            _maxSpanCount = int.MaxValue;
            provider.AddSubscriber( this );
            _subscribed = true;
            _awaitingTokens = new HashSet<TimeSpanAwaitingToken>();
            _tokenToQuit = new ConcurrentQueue<TimeSpanAwaitingToken>();
        }

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider" /> and define the length of time span.  Length
        /// of a single time span is defined in seconds.
        /// </summary>
        /// <param _name="spanSeconds"></param>
        /// <param _name="provider"></param>
        public TimeSpanProvider(
            double spanSeconds,
            TimeSourceProviderBase provider )
        {
            if( provider == null ) {
                throw new ArgumentNullException( nameof( provider ) );
            }
            _timeSource = provider;
            _spanLength = ( long )( spanSeconds * 1000L * 1000L );
            _spanBegin = -_spanLength;
            _maxSpanCount = long.MaxValue;
            provider.AddSubscriber( this );
            _subscribed = true;
            _awaitingTokens = new HashSet<TimeSpanAwaitingToken>();
            _tokenToQuit = new ConcurrentQueue<TimeSpanAwaitingToken>();
        }

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider" /> and define the length of time span. Length
        /// of a single time span is defined in microseconds.
        /// </summary>
        /// <param _name="spanLength"></param>
        /// <param _name="maxSpan"></param>
        /// <param _name="provider"></param>
        public TimeSpanProvider(
            long spanLength,
            long maxSpan,
            TimeSourceProviderBase provider )
        {
            if( provider == null ) {
                throw new ArgumentNullException( nameof( provider ) );
            }
            if( maxSpan <= 0 ) {
                throw new ArgumentException(
                    "Sapn count cannot be smaller than zero" );
            }
            _timeSource = provider;
            _spanLength = spanLength;
            _spanBegin = -_spanLength;
            provider.AddSubscriber( this );
            _subscribed = true;
            _awaitingTokens = new HashSet<TimeSpanAwaitingToken>();
            _tokenToQuit = new ConcurrentQueue<TimeSpanAwaitingToken>();
        }

        public double SpanLength
        {
            get => ( _spanLength * 1.0 ) / ( 1000L * 1000L );
        }

        public float CurrentSecond
        {
            get => timeSpanCount.Value * SpanLengthLowPrecession;
        }

        public float SpanLengthLowPrecession
        {
            get => ( _spanLength * 1.0f ) / ( 1000 * 1000 );
        }

        public int CurrentSpan
        {
            get => timeSpanCount.Value;
        }

        /// <summary>
        /// Time source provider provided in <see cref="StgSharp" /> internal framework.
        /// </summary>
        public static TimeSourceProviderBase DefaultProvider => StgSharpTime.OnlyInstance;

        public TimeSpanAwaitingToken ParticipantSpanAwaiting( int count )
        {
            TimeSpanAwaitingToken token = new TimeSpanAwaitingToken(
                this, count );
            _tokenToAdd.Enqueue( token );
            return token;
        }

        public void QuitSpanAwaiting( TimeSpanAwaitingToken token )
        {
            _tokenToQuit.Enqueue( token );
        }

        public void StopSpanProviding()
        {
            Interlocked.Exchange( ref _maxSpanCount, 0 );
        }

        public void SubscribeToTimeSource(
            TimeSourceProviderBase serviceHandler )
        {
            if( serviceHandler == null ) {
                throw new ArgumentNullException( nameof( serviceHandler ) );
            }
            if( _subscribed ) {
                return;
            }
            _subscribed = true;
            serviceHandler.AddSubscriber( this );
        }

        public int ToInt32()
        {
            return timeSpanCount.Value;
        }

        internal bool CheckTime( long microseconds )
        {
            if( microseconds - _spanBegin < _spanLength ) {
                return true;
            }
            while( !_tokenToAdd.IsEmpty ) {
                _tokenToAdd.TryDequeue( out TimeSpanAwaitingToken tokenAdd );
                _awaitingTokens.Add( tokenAdd );
            }
            while( !_tokenToQuit.IsEmpty ) {
                _tokenToQuit.TryDequeue( out TimeSpanAwaitingToken tokenQuit );
                _awaitingTokens.Remove( tokenQuit );
                InternalIO.InternalWriteLog(
                    $"Awaiting token {tokenQuit!.TokenID} has been removed.",
                    LogType.Info );
            }

            _spanBegin = microseconds;
            timeSpanCount++;

            foreach( TimeSpanAwaitingToken item in _awaitingTokens ) {
                if( item.AwaitingSemaphoreSlim.CurrentCount == 0 ) {
                    item.AwaitingSemaphoreSlim.Release();
                    item.Refresh();
                } else {
                    item.MissRefresh();
                }
            }

            return timeSpanCount < _maxSpanCount;
        }

        public static explicit operator int(
            [NotNull]TimeSpanProvider provider )
        {
            return provider.ToInt32();
        }

    }
}