//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TimeProvider"
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
using StgSharp.Collections;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp
{
    public static partial class World
    {

        internal static StgSharpTime mainTimeProvider = new StgSharpTime();

        internal static TimeSourceProviderBase MainTimeProvider => mainTimeProvider;

    }

    public sealed class StgSharpTime : TimeSourceProviderBase
    {

        private static readonly LinearBag<TimeSpanProvider> _subscribers = new();
        private static long accuracy;
        private static readonly Stopwatch mainProvider = new();
        private static Thread timeProvideThread;

        internal StgSharpTime() { }

        public static TimeSourceProviderBase OnlyInstance => World.MainTimeProvider;

        public sealed override void StartProvidingTime()
        {
            if (mainProvider.IsRunning) {
                return;
            }
            mainProvider.Start();

            // speed test
            long totalMS, internalFrequency = Stopwatch.Frequency;
            for (int i = 0; i < 100; i++)
            {
                totalMS = mainProvider.ElapsedTicks / (internalFrequency / (1000L * 1000L));
                for (int t = 0; t < 10; t++)
                {
                    if (totalMS < 1000) {
                        _subscribers.Add(
                            new TimeSpanProvider(100L, TimeSpanProvider.DefaultProvider));
                    }
                }
            }
            mainProvider.Stop();
            _subscribers.Clear();
            totalMS = mainProvider.ElapsedTicks / (internalFrequency / (1000L * 1000L)) / 100L;
            accuracy = totalMS * 2 / 3;
            mainProvider.Reset();
            timeProvideThread = new Thread(new ThreadStart(ProvideTime));
            timeProvideThread.Start();
        }

        public override void StopProvidingTime()
        {
            lock (mainProvider) {
                mainProvider.Stop();
            }
        }

        public void Terminate()
        {
            lock (mainProvider) {
                mainProvider.Stop();
            }
        }

        protected sealed override void AddSubscriberUnsynced(TimeSpanProvider subscriber)
        {
            _subscribers.Add(subscriber);
        }

        protected sealed override void RemoveSubscriberUnsynced(TimeSpanProvider subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        private void ProvideTime()
        {
            long totalMS, internalFrequency = Stopwatch.Frequency;
            lock (mainProvider) {
                mainProvider.Restart();
            }
            while (mainProvider.IsRunning)
            {
                totalMS = mainProvider.ElapsedTicks / (internalFrequency / (1000L * 1000L));
                if (_subscribers.Count == 0)
                {
                    continue;
                }
                lock (SubscribeLock)
                {
                    for (int i = 0; i < _subscribers.Count; i++)
                    {
                        if (!_subscribers[i].CheckTime(totalMS))
                        {
                            _subscribers.RemoveAt(i);
                            DefaultLog.InternalWriteLog(
                                $"Time subscriber {_subscribers[i]} is to be removed.", LogType.Info);
                        }
                    }
                }
            }
        }

    }
}
