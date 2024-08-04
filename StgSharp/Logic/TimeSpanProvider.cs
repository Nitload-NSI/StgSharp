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
using StgSharp.Logic;

using System;
using System.Linq;
using System.Threading;

namespace StgSharp
{

    /// <summary>
    /// A microsecond-accuracy timer. It will refresh every a certain time span.
    /// </summary>
    public class TimeSpanProvider
    {

        private Action _frameUpdateMissCallback = () => { };
        private Action _refreshCallback;
        private bool _subscribed;
        private Counter<int> timeSpanCount = new Counter<int>(-1, 1, 1);
        private readonly int _maxSpanCount;
        private long _spanBegin;
        private readonly long _spanLength;
        private SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(0, 1);
        private readonly TimeSourceProviderBase _timeSource;

        

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider"/> and define the length of time span. Length of a single time span is defined in microseconds.
        /// </summary>
        /// <param name="spanMicroSeconds"></param>
        /// <param name="provider"></param>
        public TimeSpanProvider(long spanMicroSeconds, TimeSourceProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            _timeSource = provider;
            _spanLength = spanMicroSeconds;
            _maxSpanCount = int.MaxValue;
            provider.AddSubscriber(this);
            _subscribed = true;
            _refreshCallback = new Action(() => { });
        }

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider"/> and define the length of time span. 
        /// Length of a single time span is defined in seconds.
        /// </summary>
        /// <param name="spanSeconds"></param>
        /// <param name="provider"></param>
        public TimeSpanProvider(double spanSeconds, TimeSourceProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            _spanLength = (long)(spanSeconds * 1000L * 1000L);
            _spanBegin = -_spanLength;
            _maxSpanCount = int.MaxValue;
            provider.AddSubscriber(this);
            _subscribed = true;
            _refreshCallback = new Action(() => { });
        }

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider"/> and define the length of time span. Length of a single time span is defined in microseconds.
        /// </summary>
        /// <param name="spanLength"></param>
        /// <param name="maxSpan"></param>
        /// <param name="provider"></param>
        public TimeSpanProvider(long spanLength, int maxSpan, TimeSourceProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (maxSpan <= 0)
            {
                maxSpan = int.MaxValue;
            }
            _spanLength = spanLength; _spanBegin = -_spanLength;
            provider.AddSubscriber(this);
            _subscribed = true;
        }

        public int CurrentSpan
        {
            get => timeSpanCount.Value;
        }

        public float CurrentSecond
        {
            get => timeSpanCount.Value * SpanLengthLowPrecession;
        }

        /// <summary>
        /// Time source provider provided in <see cref="StgSharp"/> internal framework.
        /// </summary>
        public static TimeSourceProviderBase DefaultProvider => StgSharpTime.OnlyInstance;

        public double SpanLength
        {
            get => (_spanLength * 1.0) / (1000L * 1000L);
        }

        public float SpanLengthLowPrecession
        {
            get => (_spanLength * 1.0f) / (1000 * 1000);
        }

        /// <summary>
        /// The method this <see cref="TimeSpanProvider"/> will call every time a new time span started.
        /// </summary>
        public Action SpanRefreshCallback
        {
            get => _refreshCallback;
            set => _refreshCallback = value;
        }

        public void SubscribeToTimeSource(TimeSourceProviderBase serviceHandler)
        {
            if (serviceHandler == null)
            {
                throw new ArgumentNullException(nameof(serviceHandler));
            }
            if (_subscribed)
            {
                return;
            }
            _subscribed = true;
            serviceHandler.AddSubscriber(this);
        }

        public void WaitNextSpan()
        {
            _refreshSemaphore.Wait();
        }

        internal bool CheckTime(long microseconds)
        {
            //Console.Write("1 ");
            if (microseconds - _spanBegin < _spanLength)
            {
                return true;
            }
            //Console.Write("2 ");
            _spanBegin = microseconds;
            timeSpanCount++;
            //Console.Write("3 ");
            if (_refreshCallback != null)
            {
                _refreshCallback();
            }
            //Console.Write("4 ");
            if (_refreshSemaphore.CurrentCount == 0)
            {
                _refreshSemaphore.Release();
            }
            else
            {
                if (_frameUpdateMissCallback != null)
                {
                    _frameUpdateMissCallback();
                }
            }
            //Console.Write("5 ");
            //Console.WriteLine( $"{timeSpanCount}\n" );
            return timeSpanCount < _maxSpanCount;
        }

        public static implicit operator int(TimeSpanProvider provider)
        {
            if (provider == null)
            {
                return 0;
            }
            return provider.ToInt32();
        }

        public int ToInt32()
        {
            return (int)timeSpanCount.Value;
        }
    }
}