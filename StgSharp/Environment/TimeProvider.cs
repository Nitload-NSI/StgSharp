//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TimeProvider.cs"
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp
{
    public partial class StgSharp
    {


        internal static StgSharpTime mainTimeProvider;

        internal static StgSharpTime MainTimeProvider => mainTimeProvider;

    }

    public sealed class StgSharpTime : TimeSourceProviderBase
    {

        private static long accuracy;
        private static Stopwatch mainProvider = new Stopwatch();
        private static List<TimeSpanProvider> subscriberList = new List<TimeSpanProvider>();
        private static Thread timeProvideThread;

        internal StgSharpTime()
        {
        }

        public static StgSharpTime OnlyInstance => StgSharp.MainTimeProvider;

        public override sealed void StartProvidingTime()
        {
            if (mainProvider.IsRunning)
            {
                return;
            }
            mainProvider.Start();
            //speed test
            long totalMS, internalFrequncy = Stopwatch.Frequency;
            for (int i = 0; i < 100; i++)
            {
                totalMS = mainProvider.ElapsedTicks / (internalFrequncy / (1000L * 1000L));
                for (int t = 0; t < 10; t++)
                {
                    if (totalMS < 1000)
                    {
                        subscriberList.Add(new TimeSpanProvider(100L, TimeSpanProvider.DefaultProvider));
                    }
                }
            }
            mainProvider.Stop();
            subscriberList.Clear();
            totalMS = mainProvider.ElapsedTicks / (internalFrequncy / (1000L * 1000L)) / 100L;
            accuracy = (totalMS * 2) / 3;
            mainProvider.Reset();
            timeProvideThread = new Thread(new ThreadStart(ProvideTime));
            timeProvideThread.Start();
        }

        public void Terminate()
        {
            lock (mainProvider)
            {
                mainProvider.Stop();
            }
        }


        protected override sealed void AddSubscriberUnsynced(TimeSpanProvider subscriber)
        {
            subscriberList.Add(subscriber);
        }

        protected override sealed void RemoveSubscriberUnsynced(TimeSpanProvider subscriber)
        {
            subscriberList.Remove(subscriber);
        }

        private void ProvideTime()
        {
            long totalMS, internalFrequncy = Stopwatch.Frequency;
            lock (mainProvider)
            {
                mainProvider.Restart();
            }
            while (mainProvider.IsRunning)
            {
                totalMS = mainProvider.ElapsedTicks / (internalFrequncy / (1000L * 1000L));
                if (subscriberList.Count == 0)
                {
                    continue;
                }
                CheckSubscriberSemaphore.Wait();
                List<TimeSpanProvider> toRemoveList = new List<TimeSpanProvider>();
                foreach (TimeSpanProvider subscriber in subscriberList)
                {
                    if (!subscriber.CheckTime(totalMS))
                    {
                        toRemoveList.Add(subscriber);
                    };
                }
                if (toRemoveList.Count != 0)
                {
                    foreach (TimeSpanProvider subscriber in toRemoveList)
                    {
                        subscriberList.Remove(subscriber);
                    }
                }
                CheckSubscriberSemaphore.Release();
            }
        }

    }
}