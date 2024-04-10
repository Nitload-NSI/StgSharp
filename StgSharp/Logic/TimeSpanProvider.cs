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
using StgSharpDebug.Logic;

using System;
using System.Linq;
using System.Threading;

namespace StgSharp
{
    public delegate void TimeSpanRefeshCallback();


    /// <summary>
    /// A microsecond-accuracy timer. It will refresh every a certain time span.
    /// </summary>
    public class TimeSpanProvider
    {

        private Action<SemaphoreFullException> _frameUpdateMissCallback = null;

        private readonly int _maxSpanCount;
        private TimeSpanRefeshCallback _refeshCallback;
        private SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(0, 1);
        private long _spanBegin;
        private readonly long _spanLength;
        private bool _subscribed;
        private Counter<int> timeSpanCount = new Counter<int>(-1, 1, 1);

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider"/> and define the length of time span. Length of a single time span is defined in microseconds.
        /// </summary>
        /// <param name="spanLength"></param>
        /// <parma name="provider"></parma>
        public TimeSpanProvider(long spanLength, TimeSourceProviderBase provider)
        {
            _spanLength = spanLength;
            _maxSpanCount = int.MaxValue;
            provider.AddSubscriber(this);
            _subscribed = true;
            _refeshCallback = () => { };
        }

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider"/> and define the length of time span. 
        /// Length of a single time span is defined in seconds.
        /// </summary>
        /// <param name="spanLength"></param>
        /// <parma name="provider"></parma>
        public TimeSpanProvider(double spanLength, TimeSourceProviderBase provider)
        {
            _spanLength = (long)(spanLength * 1000L * 1000L);
            _spanBegin = -_spanLength;
            _maxSpanCount = int.MaxValue;
            provider.AddSubscriber(this);
            _subscribed = true;
            _refeshCallback = () => { };
        }

        /// <summary>
        /// Create a new <see cref="TimeSpanProvider"/> and define the length of time span. Length of a single time span is defined in microseconds.
        /// </summary>
        /// <param name="spanLength"></param>
        /// <parma name="provider"></parma>
        public TimeSpanProvider(long spanLength, int maxSpan, TimeSourceProviderBase provider)
        {
            if (maxSpan <= 0)
            {
                maxSpan = int.MaxValue;
            }
            _spanLength = spanLength; _spanBegin = -_spanLength;
            provider.AddSubscriber(this);
            _subscribed = true;
        }

        public int CurrentSpan
=> timeSpanCount.Value;
        /// <summary>
        /// Time source provider provided in <see cref="StgSharp"/> internal framework.
        /// </summary>
        public static TimeSourceProviderBase DefaultProvider => StgSharpTime.OnlyInstance;

        public Action<SemaphoreFullException> FrameUpdateMissCallback
        {
            get => _frameUpdateMissCallback;
            set => _frameUpdateMissCallback = value;
        }

        /// <summary>
        /// The method this <see cref="TimeSpanProvider"/> will call every time a new time span started.
        /// </summary>
        public TimeSpanRefeshCallback SpanRefreshCallback
        {
            get => _refeshCallback;
            set => _refeshCallback = value;
        }

        public void SubcsribeToTimeSource(TimeSourceProviderBase serviceHandler)
        {
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
            if (microseconds - _spanBegin > _spanLength)
            {
                _spanBegin = microseconds;
                timeSpanCount++;
                try
                {
                    _refeshCallback();
                    _refreshSemaphore.Release();
                }
                catch (SemaphoreFullException ex)
                {
                    if (_frameUpdateMissCallback != null)
                    {
                        _frameUpdateMissCallback(ex);
                    }
                }
                if (timeSpanCount > _maxSpanCount)
                {
                    return false;
                }
            }
            return true;
        }

        public static implicit operator int(TimeSpanProvider provider)
        {
            return provider.timeSpanCount.Value;
        }

    }
}