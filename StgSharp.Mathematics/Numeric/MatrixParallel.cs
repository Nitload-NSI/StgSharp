//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        private static bool _meaningful;

        private static CapacityFixedStack<MatrixParallelThread> _threads;

        private static readonly
#if NET10_0_OR_GREATER
            Lock
#else
            object
#endif
            _lock = new();

        private static ThreadPriority _AfterWork = ThreadPriority.Lowest;

        public static bool SupportParallel { get; } = Environment.ProcessorCount > 6;

        public static bool IsParallelMeaningful => Environment.ProcessorCount > 6 && IsInitialized;

        private static bool IsInitialized { get; set; } = false;

        private static Queue<IntPtr> LeaderTask { get; set; }

        public static void EndParallel()
        {
            Monitor.Exit(_lock);
        }

        public static void LaunchParallel(MatrixParallelHandle handle, SleepMode afterWork)
        {
            _AfterWork = (ThreadPriority)afterWork;
            MatrixParallelThread leader = handle.GetLeader();
            foreach (MatrixParallelWrap w in handle.Wraps) {
                w.SetScheduler();
            }
            leader.MatrixParallelLeader();
            handle.WaitAllWraps();
        }

        internal static void Init()
        {
            if (IsInitialized) {
                return;
            }
            MatrixParallelFactory.Init();
            if (!SupportParallel)
            {
                IsInitialized = true;
                return;
            }
            int count = Environment.ProcessorCount - 4;
            _threads = new(count);
            for (int i = 0; i < count; i++) {
                _threads.Push(new());
            }
            _awaitingLeaderHandle = new AwaitingQueue();
            IsInitialized = true;
        }

    }
}
