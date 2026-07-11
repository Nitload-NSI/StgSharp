// -----------------------------------------------------------------------------
// file="StgSharpTime"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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

        public override long GetCurrentTimeSpanTick()
        {
            return mainProvider.ElapsedTicks;
        }

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

        protected sealed override void AddSubscriberUnsynced(
                                       TimeSpanProvider subscriber
        )
        {
            _subscribers.Add(subscriber);
        }

        protected sealed override void RemoveSubscriberUnsynced(
                                       TimeSpanProvider subscriber
        )
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
                            // Console.WriteLine("removed");
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
