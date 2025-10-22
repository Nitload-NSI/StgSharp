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

        private readonly Handle[] _resetHandle;
        private readonly Thread[] _threads;

        public MatrixParallelWrap(int id, int wrapSize)
        {
            ID = id;
            _threads = new Thread[wrapSize];
            _resetHandle = new Handle[wrapSize];
            for (int i = 0; i < wrapSize; i++)
            {
                _threads[i] = new Thread(MatrixParallelWorker)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal,
                    Name = $"MPT#{ID}_{i}"
                };
                Handle handle = new Handle(i, this);
                _resetHandle[i] = handle;
                _threads[i].Start(handle);
            }
        }

        public bool IsAllAvailable
        {
            get
            {
                foreach (Handle item in _resetHandle)
                {
                    if (item.Role != 0) {
                        return false;
                    }
                }
                return true;
            }
        }

        public int ID { get; init; }

        public static SleepMode AfterWork
        {
            get;
            set => Interlocked.Exchange(ref Unsafe.As<SleepMode, int>(ref field), (int)value);
        }

        internal int Magic { get; set; }

        internal ManualResetEventSlim SchedulerReset { get; } = new(false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAllWorker()
        {
            foreach (Handle item in _resetHandle) {
                item.ResetEvent.Set();
            }
        }

        internal bool TryGetLeaderHandle(out Handle? handle)
        {
            foreach (Handle item in _resetHandle)
            {
                if (item.Role != 2)
                {
                    handle = item;
                    item.Role = 2;
                    return true;
                }
            }
            handle = null;
            return false;
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


        #region ThreadOperation

        private static unsafe void MatrixParallelWorker([AllowNull] object obj)
        {
            Handle handle = (obj as Handle)!;
            MatrixParallelWrap wrap = handle.Wrap;
            int inWrapIndex = handle.InWrapIndex;
            Thread self = Thread.CurrentThread;
            ManualResetEventSlim reset = handle.ResetEvent;
            do
            {
                reset.Wait();
                reset.Reset();
                Queue<IntPtr> taskQueue = handle.TaskCache;
                self.Priority = ThreadPriority.Highest;
                switch (handle.Role)
                {
                    case 0://worker
                        // _ = Interlocked.Decrement(ref RemainThreadCount);
                        ExecuteTaskPackage(handle.TaskCache);
                        break;
                    case 1://scheduler
                        // TODO task publication to worker in wrap
                        wrap.SetAllWorker();
                        ExecuteTaskPackage(taskQueue);

                        break;
                    case 2://leader, error because leader should be at outer thread but not here
                        break;
                    default:
                        break;
                }
                self.Priority = (ThreadPriority)AfterWork;
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
                        MatrixComputeModel.Compute_Unary(p);
                        break;
                    case 2:
                        MatrixComputeModel.Compute_UnaryScalar(p);
                        break;
                    case 3:
                        MatrixComputeModel.Compute_Binary(p);
                        break;
                    case 4:
                        MatrixComputeModel.Compute_BinaryScalar(p);
                        break;
                    default:
                        break;
                }
            }
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
