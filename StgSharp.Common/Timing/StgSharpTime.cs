//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StgSharpTime"
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
using System.Diagnostics;
using System.Threading;

namespace StgSharp
{
    public static partial class World
    {

        public static TimeSourceProviderBase MainTimeProvider => StgSharpTime.OnlyInstance;

    }

    public sealed class StgSharpTime : TimeSourceProviderBase
    {

        private static readonly LinearBag<TimeSpanProvider> _subscribers = new();
        private static readonly Stopwatch mainProvider = new();
        private static Thread timeProvideThread;

        private StgSharpTime() { }

        // Use Stopwatch frequency as base tick frequency (ticks per second).
        public override long Frequency => Stopwatch.Frequency;

        public static StgSharpTime OnlyInstance { get; } = new StgSharpTime();

        public override long GetCurrentTimeSpanTick() => mainProvider.ElapsedTicks;

        public sealed override void StartProvidingTime()
        {
            if (mainProvider.IsRunning) {
                return;
            }

            mainProvider.Start();
            timeProvideThread = new Thread(ProvideTime)
            {
                IsBackground = true,
                Name = "StgSharpTime"
            };
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
            long internalFrequency = Stopwatch.Frequency; // ticks per second
            lock (mainProvider) {
                mainProvider.Restart();
            }

            while (mainProvider.IsRunning)
            {
                long elapsedTicks;
                lock (mainProvider) {
                    elapsedTicks = mainProvider.ElapsedTicks;
                }
                if (_subscribers.Count == 0)
                {
                    Thread.SpinWait(50);
                    continue;
                }
                lock (SubscribeLock)
                {
                    for (int i = 0; i < _subscribers.Count; i++)
                    {
                        if (!_subscribers[i].OnSpanTick(elapsedTicks))
                        {
                            _subscribers.RemoveAt(i);
                            i--; // adjust index after removal
                        }
                    }
                }

                // Optional: brief spin to avoid tight loop if frame lengths are large.
                Thread.SpinWait(20);
            }
        }

    }
}
