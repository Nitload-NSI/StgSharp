// -----------------------------------------------------------------------------
// file="TimeSourceProvider"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Linq;
using System.Threading;

namespace StgSharp.Timing
{
    public class TimeSourceStartupConfiguration(
                 nuint bindedNumaNode = 0,
                 nuint bindedCoreIndex = 0,
                 ThreadPriority threadPriority = ThreadPriority.Highest
    )
    {

        public nuint BindedNumaNode { get; } = bindedNumaNode;

        public nuint BindedCoreIndex { get; } = bindedCoreIndex;

        public ThreadPriority Priority { get; } = threadPriority;

    }

    public abstract class TimeSourceProviderBase
    {

        public abstract long Frequency { get; }

        protected object SubscribeLock { get; } = new();

        public void AddSubscriber(
                    TimeSpanProvider subscriber
        )
        {
            lock (SubscribeLock) {
                AddSubscriberUnsynced(subscriber);
            }
        }

        public abstract long GetCurrentTimeSpanTick();

        public void RemoveSubscriber(
                    TimeSpanProvider subscriber
        )
        {
            lock (SubscribeLock) {
                RemoveSubscriberUnsynced(subscriber);
            }
        }

        public abstract void StartProvidingTime(
                             TimeSourceStartupConfiguration threadConfig
        );

        public abstract void StopProvidingTime();

        protected abstract void AddSubscriberUnsynced(
                                TimeSpanProvider subscriber
        );

        protected abstract void RemoveSubscriberUnsynced(
                                TimeSpanProvider subscriber
        );

    }
}
