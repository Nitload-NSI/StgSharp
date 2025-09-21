//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance;
using StgSharp.HighPerformance.Memory;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    /*
    * Thread wrapping to accelerate matrix operations by dividing threads into groups (wraps).
    * For M wraps, each containing an average of N threads, we satisfy the condition M * N = T - 4,
    * where T is the total thread count of the CPU. Typically, for modern consumer CPUs, T is around 16.
    * For simplicity, let M * N = 16.
    *
    * The total time complexity for publishing tasks is O(M + N):
    * - O(M) accounts for distributing tasks across M wraps.
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

        private static Thread[] _threads;
        private static MatrixParallelWrap[] _wraps;
        private static readonly (int a, int b)[] LookupTable = [ (1, 1), (0, 2), (3, 0), (2, 1), (1, 2), (0, 3), (3, 1), (2, 2), (1, 3), (0, 4), (3, 2), (2, 3), (1, 4), (0, 5), ];
        private static volatile int RemainThreadCount = 0;

        private static readonly object _lock = new();
        private static ThreadPriority _AfterWork = ThreadPriority.Lowest;

        public static bool SupportParallel { get; } = Environment.ProcessorCount > 6;

        public static bool IsParallelMeaningful { get; } = Environment.ProcessorCount > 6 && IsInitialized;

        public static MatrixParallelWrap.Handle LeaderHandle { get; private/**/ set; }

        private static bool IsInitialized { get; set; } = false;

        private static Queue<IntPtr> LeaderTask { get; set; }

        public static void EndParallel()
        {
            Monitor.Exit(_lock);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int a, int b) FastDecompose(int c)
        {
            if (c < 7) {
                return (0, 0);
            }
            c -= 7;
            int c_prime = c / 14;
            int d = c % 14;

            (int a_prime, int b_prime) = LookupTable[d];

            return (a_prime + c_prime, b_prime + c_prime);
        }

        public static void Init()
        {
            if (IsInitialized) {
                return;
            }
            MatrixParallelFactory.Init();
            int count = Environment.ProcessorCount - 4;
            count = count == 5 ? 4 : count;
            _threads = new Thread[count];
            if (count < 6)
            {
                _wraps = [ new MatrixParallelWrap(0, count, true) ];
            } else if (count == 6)
            {
                _wraps = [ new MatrixParallelWrap(0, 3, true), new MatrixParallelWrap(3, 3, false) ];
            } else
            {
                (int _3wrap, int _4wrap) = FastDecompose(count);
                _wraps = new MatrixParallelWrap[_3wrap + _4wrap];
                int id = 0, idx = 0;
                for (; idx < _3wrap; idx++, id += 3) {
                    _wraps[idx] = new MatrixParallelWrap(idx, 3, id == 0);
                }
                for (; idx < _3wrap + _4wrap; idx++, id += 4) {
                    _wraps[idx] = new MatrixParallelWrap(id, 4, id == 0);
                }
            }
            LeaderHandle = new MatrixParallelWrap.Handle(0, _wraps[0]);
            LeaderTask = LeaderHandle.Wrap.GetWorkerTaskQueue(0);
            IsInitialized = true;
        }

        public static void LaunchParallel(SleepMode afterWork)
        {
            RemainThreadCount = _threads.Length;
            _AfterWork = (ThreadPriority)afterWork;
            SetAllWrap();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LeadParallel()
        {
            Monitor.Enter(_lock);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MatrixParallelLeaderWork()
        {
            MatrixParallelWrap.ExecuteTaskPackage(LeaderTask);
            _ = Interlocked.Decrement(ref RemainThreadCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetThreadCount()
        {
            RemainThreadCount = _threads.Length;
        }

        public static void SetAllWrap()
        {
            ResetThreadCount();
            foreach (MatrixParallelWrap item in _wraps) {
                item.SchedulerReset.Set();
            }
        }

        public unsafe class MatrixParallelWrap
        {

            private readonly Queue<IntPtr>[] _cache;
            private readonly Thread[] _threads;
            private readonly ManualResetEventSlim[] _workerReset;

            public MatrixParallelWrap(int id, int wrapSize, bool isFirst)
            {
                ID = id;
                int begin = isFirst ? 1 : 0, end = wrapSize - 1;
                _cache = new Queue<IntPtr>[wrapSize];
                _workerReset = new ManualResetEventSlim[end];
                _threads = new Thread[wrapSize];
                for (int i = 0; i < end; i++) {
                    _workerReset[i] = new ManualResetEventSlim(false);
                }
                for (int i = 0; i < _cache.Length; i++) {
                    _cache[i] = new Queue<IntPtr>(8);
                }
                for (int i = begin; i < end; i++)
                {
                    _threads[i] = new Thread(MatrixParallelWorker)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.BelowNormal,
                        Name = $"MPT#{ID}_{i}"
                    };
                    _threads[i].Start(new Handle(i, this));
                }
                _threads[end] = new Thread(MatrixParallelScheduler)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal,
                    Name = $"MPT#{ID}_{end}*"
                };
                _threads[end].Start(new Handle(end, this));
            }

            public int ID { get; init; }

            internal ManualResetEventSlim SchedulerReset { get; } = new(false);

            public Span<IntPtr> EnqueuePackage(Span<IntPtr> packageSpan)
            {
                int idx = 0;
                for (; idx < _cache.Length; idx++) {
                    _cache[idx].Enqueue(packageSpan[idx]);
                }
                return packageSpan[idx..];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ManualResetEventSlim GetWorkerResetEvent(int id)
            {
                return _workerReset[id];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Queue<IntPtr> GetWorkerTaskQueue(int idInWrap)
            {
                return _cache[idInWrap];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SetAllWorker()
            {
                foreach (ManualResetEventSlim item in _workerReset) {
                    item.Set();
                }
            }

            public record class Handle(int InWrapIndex, MatrixParallelWrap Wrap);

            #region ThreadOperation

            private static unsafe void MatrixParallelWorker([AllowNull] object obj)
            {
                Handle handle = (obj as Handle)!;
                MatrixParallelWrap wrap = handle.Wrap;
                int inWrapIndex = handle.InWrapIndex;
                Thread self = Thread.CurrentThread;
                ManualResetEventSlim reset = wrap.GetWorkerResetEvent(inWrapIndex);
                do
                {
                    reset.Wait();
                    reset.Reset();
                    self.Priority = ThreadPriority.Highest;
                    ExecuteTaskPackage(wrap.GetWorkerTaskQueue(inWrapIndex));
                    _ = Interlocked.Decrement(ref RemainThreadCount);
                    self.Priority = _AfterWork;
                } while (true);
            }

            private static unsafe void MatrixParallelScheduler([AllowNull] object obj)
            {
                Handle handle = (obj as Handle)!;
                MatrixParallelWrap wrap = handle.Wrap;
                int inWrapIndex = handle.InWrapIndex;
                Queue<IntPtr> taskQueue = wrap.GetWorkerTaskQueue(inWrapIndex);
                ManualResetEventSlim resetEvent = wrap.SchedulerReset;
                Thread self = Thread.CurrentThread;
                do
                {
                    resetEvent.Wait();
                    resetEvent.Reset();
                    self.Priority = ThreadPriority.Highest;

                    // TODO task publication to worker in wrap
                    wrap.SetAllWorker();
                    ExecuteTaskPackage(taskQueue);
                    _ = Interlocked.Decrement(ref RemainThreadCount);
                    self.Priority = _AfterWork;
                } while (true);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void ExecuteTaskPackage(Queue<IntPtr> taskQueue)
            {
                while (taskQueue.TryDequeue(out nint handle))
                {
                    MatrixParallelTaskPackageNonGeneric* p = (MatrixParallelTaskPackageNonGeneric*)handle;
                    switch (p->ComputeMode)
                    {
                        case 1:
                            Compute_Unary(p);
                            break;
                        case 2:
                            Compute_UnaryScalar(p);
                            break;
                        case 3:
                            Compute_Binary(p);
                            break;
                        case 4:
                            Compute_BinaryScalar(p);
                            break;
                        default:
                            break;
                    }
                }
            }

            #endregion
        }

    }

    public enum SleepMode
    {

        QuickWakeup = ThreadPriority.AboveNormal,
        NormalSleep = ThreadPriority.BelowNormal,
        DeepSleep = ThreadPriority.Lowest,

    }
}