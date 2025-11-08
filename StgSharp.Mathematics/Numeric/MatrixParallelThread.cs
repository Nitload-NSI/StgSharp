//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelThread"
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
    public class MatrixParallelThread : IDisposable
    {

        private bool disposedValue;
        private ManualResetEventSlim _resetEvent;
        private Thread _managedThread;

        public MatrixParallelThread()
        {
            _managedThread = new Thread(MatrixParallelWorker);
            _resetEvent = new ManualResetEventSlim(false);
        }

        // ~MatrixComputeThread()
        // {
        // Dispose(disposing: false);
        // }

        public ThreadPriority AfterWork
        {
            get => field;
            set => field = value;
        }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
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

        public void Restart()
        {
            _resetEvent.Set();
        }

        public void ReturnToPool()
        {
            // MatrixParallel.(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _resetEvent.Dispose();
                    _managedThread.Join();
                }

                disposedValue = true;
            }
        }

        private unsafe void MatrixParallelWorker()
        {
            // Thread self = Thread.CurrentThread;
            _resetEvent.Wait();
            _resetEvent.Reset();
            _managedThread.Priority = ThreadPriority.Highest;
            switch (Role)
            {
                case 0://worker
                    // _ = Interlocked.Decrement(ref RemainThreadCount);
                    ExecuteTaskPackage(Tasks);
                    break;
                case 1://scheduler
                    BindedWrap.PublishTaskToWorkers(Tasks);
                    BindedWrap.SetAllWorker();
                    ExecuteTaskPackage(Tasks);

                    break;
                case 2://leader, error because leader should be at outer thread but not here
                    break;
                default:
                    break;
            }
            _managedThread.Priority = AfterWork;
        }

        #region wrap belong

        internal MatrixParallelWrap BindedWrap { get; set; } = null!;

        internal Queue<IntPtr> Tasks { get; } = [];

        public int Role
        {
            get;
            internal set
            {
                if ((Role & (~3u)) == 0)
                {
                    field = value;
                } else
                {
#if DEBUG
                    throw new ArgumentOutOfRangeException(nameof(Role));
#endif
                }
            }
        }

        #endregion
    }
}
