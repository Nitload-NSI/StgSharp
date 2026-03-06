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
using StgSharp.Internal;
using StgSharp.Mathematics.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Mathematics.Numeric
{
    internal enum MatrixThreadState : int
    {

        Born = 0,
        Ready = 1,
        Running = 2,
        Idle = 4,
        Retired = 8,

    }

    public unsafe partial class MatrixParallelThread : IDisposable
    {

        private IntPtr* _context;
        private volatile bool _disposed, _isRetireRequested = false;
        private readonly ManualResetEventSlim _resetEvent;
        private readonly Thread _managedThread;

        internal MatrixParallelThread(
                 int poolId
        )
        {
            _resetEvent = new ManualResetEventSlim(false);
            _managedThread = new Thread(MatrixParallelWorkLoad);
            _managedThread.Start();
            PoolId = poolId;
        }

        public int PoolId { get; init; }

        public ThreadPriority AfterWork { get; set; }

        internal ManualResetEventSlim? LeaderEvent { get; set; }

        internal IntrinsicContext* Intrinsic { get; set; }

        internal bool IsSingle { get; set; }

        internal MatrixParallelWrap BindedWrap { get; set; }

        internal MatrixThreadState State { get; set; }

        internal SIMDID Simd { get; set; }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restart()
        {
            _resetEvent.Set();
        }

        [LibraryImport(Native.LibName, EntryPoint = "sn_get_simd_level_local")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial ulong GetSimdId();

        [LibraryImport(Native.LibName, EntryPoint = "sn_set_thread_affinity")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SetThreadAffinity(
                                     int coreId
        );

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _isRetireRequested = true;
                    _resetEvent.Set();
                    _managedThread.Join();
                    _resetEvent.Dispose();
                }
                _disposed = true;
            }
        }

        private unsafe void MatrixParallelWorkLoad()
        {
            State = MatrixThreadState.Born;

            // Bind OS-thread affinity once when the worker starts running
            SetThreadAffinity(PoolId);
            _ = Thread.Yield();
            SIMDID feature = new()
            {
                Mask = GetSimdId()
            };
            _context = (nint*)MatrixParallel.GetContext(feature);
            State = MatrixThreadState.Ready;
            while (true)
            {
                // Thread self = Thread.CurrentThread;
                _resetEvent.Wait();
                _resetEvent.Reset();
                if (_isRetireRequested)
                {
                    break;
                }
                State = MatrixThreadState.Running;
                AfterWork = BindedWrap.AfterWork;
                _managedThread.Priority = ThreadPriority.Highest;
                if (IsSingle)
                {
                    // TODO 将外部线程绑定到当前线程上，并执行任务包
                } else
                {
                    MatrixParallelQueue taskQueue = BindedWrap.TaskQueue;
                    MatrixParallelTask* p = taskQueue.Package;
                    /*
                    if (0 > (int)p ->ComputeHandle) {
                        SpecialBufferCompute(p);
                    }
                    switch (p->ComputeMode)
                    {
                        case MatrixIndexStyle.BUFFER_INDEX:
                            BufferCompute(p);
                            break;
                        default:
                            break;
                    }
                    /**/
                    BindedWrap.ReportThreadAccomplished();
                }
                _managedThread.Priority = AfterWork;
                State = MatrixThreadState.Idle;
            }
            State = MatrixThreadState.Retired;
        }

    }
}
