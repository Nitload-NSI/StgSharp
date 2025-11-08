//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelWrap"
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
using StgSharp.HighPerformance;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public unsafe class MatrixParallelWrap
    {

        private readonly MatrixParallelThread[] _threads;
        private int _begin;

        internal MatrixParallelWrap(MatrixParallelThread[] threads, int begin, int count)
        {
            _threads = threads;
            _begin = begin;
            Count = threads.Length;
            _threads[_begin].Role = 1;
        }

        public MatrixParallelWrap()
        {
            _threads = new MatrixParallelThread[5];
        }

        public static SleepMode AfterWork
        {
            get;
            set => Interlocked.Exchange(ref Unsafe.As<SleepMode, int>(ref field), (int)value);
        }

        internal int Magic { get; set; }

        internal ManualResetEventSlim SchedulerReset { get; } = new(false);

        private int Count { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAllWorker()
        {
            foreach (MatrixParallelThread t in _threads) {
                t.Restart();
            }
        }

        internal void PublishTaskToWorkers(Queue<IntPtr> tasks)
        {
            while (tasks.TryDequeue(out nint handle))
            {
                MatrixParallelTaskPackageNonGeneric* task = (MatrixParallelTaskPackageNonGeneric*)handle;
                PublishTaskToWorkers(task);
            }
        }

        internal void PublishTaskToWorkers(MatrixParallelTaskPackageNonGeneric* taskPackage)
        {
            int primCount = taskPackage->PrimCount;
            int secCount = taskPackage->SecCount;

            int primAverage = primCount / Count;
            int secAverage = secCount / Count;
            bool shouldSliceSec = secAverage > 512;
            for (int i = _begin; i < _begin + Count; i++)
            {
                Queue<IntPtr> tasks = _threads[i].Tasks;
                MatrixParallelTaskPackageNonGeneric* subTask = MatrixParallelFactory.FromExistPackage(taskPackage);
                subTask->PrimTileOffset = i * primAverage;
            }
        }

        public class Handle(int inWrapIndex, MatrixParallelWrap wrap)
        {

            public int InWrapIndex { get; } = inWrapIndex;

            /// <summary>
            ///   Role of the thread in the wrap. 1 is Scheduler, 0 are Workers. 2 is leader, beyond
            ///   wrap's control.
            /// </summary>
            public int Role { get; internal set; }

            public Queue<IntPtr> TaskCache { get; } = [];

            internal ManualResetEventSlim ResetEvent { get; } = new(false);

            internal MatrixParallelWrap Wrap { get; } = wrap;

        }

        #region wrap task

        internal MatrixParallelTaskPackageNonGeneric* WrapTask { get; set; }

        internal int WrapTaskSliceCount { get; set; }

        internal int WrapTaskCapacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _threads.Length;
        }

        #endregion
    }

    public enum SleepMode : int
    {

        QuickWakeup = ThreadPriority.AboveNormal,
        NormalSleep = ThreadPriority.BelowNormal,
        DeepSleep = ThreadPriority.Lowest,

    }
}
