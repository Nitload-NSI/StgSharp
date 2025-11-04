//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel"
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
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.Mathematics.Numeric
{
    /*
    * Thread wrapping to accelerate matrix operations by dividing threads into groups (pool).
    * For M pool, each containing an average of N threads, we satisfy the condition M * N = T - 4,
    * where T is the total thread count of the CPU. Typically, for modern consumer CPUs, T is around 16.
    * For simplicity, let M * N = 16.
    *
    * The total time complexity for publishing tasks is O(M + N):
    * - O(M) accounts for distributing tasks across M pool.
    * - O(N) accounts for threads within each wrap fetching tasks from the wrap queue.
    *
    * Using the inequality Sqrt(M * N) <= (M + N) / 2 (derived from the arithmetic-geometric mean inequality),
    * the optimal configuration occurs when M = N. For M * N = 16, this gives M = N = 4.
    * 
    * To ensure compatibility across a wide range of CPU architectures, 
    * we allow each wrap to contain either 3 or 4 threads, providing flexibility while maintaining efficiency.
    */

    public static partial class MatrixParallel
    {

        private static readonly (int a, int b)[] LookupTable = [
            (1, 1), // 7
            (0, 2), // 8  (no non-zero solution)
            (3, 0), // 9  (no non-zero solution)
            (2, 1), // 10
            (1, 2), // 11
            (0, 3), // 12 (no non-zero solution)
            (3, 1), // 13
            (2, 2), // 14
            (1, 3), // 15
            (4, 1), // 16
            (3, 2), // 17
            (2, 3), // 18
            (1, 4), // 19
            (4, 2), // 20
        ];
        private static bool _meaningful;

        private static CapacityFixedStack<MatrixParallelThread> _threads;

        private static volatile int RemainThreadCount = 0;
        private static readonly object _lock = new();
        private static ThreadPriority _AfterWork = ThreadPriority.Lowest;

        public static bool SupportParallel { get; } = Environment.ProcessorCount > 6;

        public static bool IsParallelMeaningful => Environment.ProcessorCount > 6 && IsInitialized;

        private static bool IsInitialized { get; set; } = false;

        private static Queue<IntPtr> LeaderTask { get; set; }

        public static void EndParallel()
        {
            Monitor.Exit(_lock);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int count_3, int count_4) FastDecompose(int c)
        {
            if (c < 7) {
                return (0, 0);
            }

            int t = c - 7;
            int k = t / 14;
            int d = t % 14;

            (int a0, int b0) = LookupTable[d];

            return (a0 + (2 * k), b0 + (2 * k));
        }

        public static void Init()
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
            IsInitialized = true;
        }

        public static void LaunchParallel(IEnumerable<MatrixParallelWrap> wraps, SleepMode afterWork)
        {
            RemainThreadCount = _threads.Count;
            _AfterWork = (ThreadPriority)afterWork;
            ResetThreadCount();
            foreach (MatrixParallelWrap w in wraps) {
                w.SchedulerReset.Set();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetThreadCount()
        {
            RemainThreadCount = _threads.Count;
        }

        #region wrap field

        public static CapacityFixedStack<MatrixParallelThread> LeadParallel()
        {
#if NET9_0_OR_GREATER
#else
            Monitor.Enter(_lock);
#endif

            return _threads;
        }

        public static void ReleaseParallelLeadership(ref CapacityFixedStack<MatrixParallelThread> pool)
        {
#if NET9_0_OR_GREATER
#else
            Monitor.Exit(_lock);
#endif
            pool = null!;
        }

        #endregion
    }
}