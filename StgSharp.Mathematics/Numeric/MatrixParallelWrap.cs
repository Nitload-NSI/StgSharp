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
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public unsafe class MatrixParallelWrap
    {

        private int _begin;

        private int _remainThreadCount;
        private readonly MatrixParallelHandle _handle;

        internal MatrixParallelWrap(MatrixParallelHandle handle, int begin, int count)
        {
            WrapTaskCapacity = count;
            _handle = handle;
            _begin = begin;
            for (int i = 0; i < WrapTaskCapacity; i++)
            {
                Threads[i].Role = MatrixParallelThread.RoleType.Worker;
                Threads[i].BindedWrap = this;
            }
            Threads[0].Role = MatrixParallelThread.RoleType.Scheduler;
        }

        public static SleepMode AfterWork
        {
            get;
            set => Interlocked.Exchange(ref Unsafe.As<SleepMode, int>(ref field), (int)value);
        }

        internal int Magic { get; set; }

        private ReadOnlySpan<MatrixParallelThread> Threads
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _handle.Threads[_begin..(_begin + WrapTaskCapacity)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReportThreadAccomplished()
        {
            Interlocked.Decrement(ref _remainThreadCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReportWrapAccomplished()
        {
            _handle.ReportWrapAccomplished();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAllWorker()
        {
            for (int i = 1; i < WrapTaskCapacity; i++) {
                Threads[i].Restart();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitAllWorker()
        {
            SpinWait.SpinUntil(() => Volatile.Read(ref _remainThreadCount) == 0);
        }

        internal void PublishTaskToWorkers()
        {
            Queue<nint> tasks = WrapTask;
            while (tasks.TryDequeue(out nint handle))
            {
                MatrixParallelTaskPackage* task = (MatrixParallelTaskPackage*)handle;
                if ((int)task->ComputeMode.OperationStyle > 0)
                {
                    PublishBufferTaskToWorkers(task);
                } else
                {
                    throw new NotImplementedException();
                }
            }
        }

        internal void SetScheduler()
        {
            MatrixParallelThread thread = Threads[0];
            thread.Restart();
            _ = Interlocked.Exchange(ref _remainThreadCount, WrapTaskCapacity);
        }

        private void PublishBufferTaskToWorkers(MatrixParallelTaskPackage* package)
        {
            ReadOnlySpan<MatrixParallelThread> pool = Threads;
            if (pool.Length == 1)
            {
                pool[0].Tasks.Enqueue((nint)package);
                return;
            }
            long count = Unsafe.As<int, long>(ref package->PrimCount);
            long actual = count / pool.Length;
            long extra = count % pool.Length;

            long offset = 0;
            MatrixParallelTaskPackage* current = package;
            /*
            string msg = $"""
                Total: count={count}, length={Threads.Length}{Environment.NewLine}
                """;
            /**/
            foreach (MatrixParallelThread thread in pool)
            {
                long wrapTaskLength = actual;
                long ex = extra > 0 ? 1 : 0;
                long length = wrapTaskLength + ex;
                extra -= ex;
                ref long offsetRef = ref Unsafe.As<int, long>(ref current->PrimTileOffset);
                offsetRef += offset;
                ref long lengthRef = ref Unsafe.As<int, long>(ref current->PrimCount);
                lengthRef = length;
                thread.Tasks.Enqueue((nint)current);
                /*
                msg += $"""
                    Slice to thread: total offset={offsetRef} local offset={offset}, length={lengthRef}{Environment.NewLine}
                    """;
                /**/
                current = MatrixParallelFactory.FromExistPackage(current);
                offset = length;
            }

            // Console.WriteLine(msg);
        }

        #region wrap task

        internal unsafe Queue<nint> WrapTask { get; set; } = [];

        internal int WrapTaskCapacity { get ; init; }

        #endregion
    }

    public enum SleepMode : int
    {

        QuickWakeup = ThreadPriority.AboveNormal,
        NormalSleep = ThreadPriority.BelowNormal,
        DeepSleep = ThreadPriority.Lowest,

    }
}
