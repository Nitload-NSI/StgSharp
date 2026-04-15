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
using StgSharp.Common.HighPerformance.ProcessorAbstraction;
using StgSharp.Internal;
using StgSharp.Mathematics.Numeric.Runtime;
using StgSharp.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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

    public unsafe partial class MatrixParallelThread : IEquatable<MatrixParallelThread>, IDisposable
    {

        private IntrinsicContext* _context;
        private readonly MatrixParallelTask* _currentTask;
        private volatile bool _disposed;
        private readonly ManualResetEventSlim _resetEvent;

        private volatile MatrixThreadState _target;
        private Thread _managedThread;

        internal MatrixParallelThread(
                 LogicalCore binded
        )
        {
            _resetEvent = new ManualResetEventSlim(false);
            _managedThread = new Thread(MatrixParallelWorkLoad);
            _managedThread.Start();
            BindedLogicalCore = binded;
        }

        public LogicalCore BindedLogicalCore { get; init; }

        public ThreadPriority AfterWork { get; set; }

        internal MatrixThreadState State { get; private set; }

        internal SIMDID Simd { get; private set; }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Pause()
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(MatrixParallelThread));
            if (State == MatrixThreadState.Retired) {
                return;
            }
            _resetEvent.Set();
            SpinWait.SpinUntil(() => State == MatrixThreadState.Retired);
            _managedThread.Join();
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposed = true;
                    _resetEvent.Set();
                    _managedThread.Join();
                    _resetEvent.Dispose();
                }
            }
        }

        private unsafe void MatrixParallelWorkLoad()
        {
            State = MatrixThreadState.Born;

            // Bind OS-thread affinity once when the worker starts running
            ThreadHelper.BindToLogicalCore(BindedLogicalCore);
            _ = Thread.Yield();
            if (Simd.Mask != 0)
            {
                SIMDID feature = new(GetSimdId());
                IntrinsicContext* contextPtr = stackalloc IntrinsicContext[1];
                LoadIntrinsicFunction(contextPtr, feature.Mask);
                Simd = feature;
                _context = contextPtr;
            }
            MatrixParallelTask* taskPtr = stackalloc MatrixParallelTask[4];
            State = MatrixThreadState.Ready;
            while (true)
            {
                // Thread self = Thread.CurrentThread;
                _resetEvent.Wait();
                _resetEvent.Reset();
                if (_disposed)
                {
                    break;
                }
                if (_target == MatrixThreadState.Idle)
                {
                    State = MatrixThreadState.Idle;
                    continue;
                }
                State = MatrixThreadState.Idle;
                State = MatrixThreadState.Running;
                _managedThread.Priority = ThreadPriority.Highest;

                // MatrixParallelQueue taskQueue = BindedWrap.TaskQueue;
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

                _managedThread.Priority = AfterWork;
                State = MatrixThreadState.Idle;
            }
            State = MatrixThreadState.Retired;
        }

#pragma warning disable CA5393
        [LibraryImport(Native.LibName, EntryPoint = "sn_get_simd_level_local")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        internal static partial ulong GetSimdId();

        [LibraryImport(Native.LibName, EntryPoint = "load_intrinsic_function")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static unsafe partial void LoadIntrinsicFunction(
                                           IntrinsicContext* context,
                                           ulong id
        );

        public bool Equals(
                    MatrixParallelThread? other
        )
        {
            return (other is not null) && ReferenceEquals(this, other);
        }
#pragma warning restore CA5393
    }
}
