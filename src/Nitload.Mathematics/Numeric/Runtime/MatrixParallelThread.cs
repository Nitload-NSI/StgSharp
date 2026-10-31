// -----------------------------------------------------------------------------
// file="MatrixParallelThread"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using Nitload.Common.HighPerformance.ProcessorAbstraction;
using Nitload.Common.Internal;
using Nitload.Mathematics.Numeric.Runtime;
using Nitload.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Nitload.Mathematics.Numeric
{
    internal enum MatrixThreadState : int
    {

        Born = 0,
        Ready = 1,
        Running = 2,
        Idle = 4,
        Retired = 8,

    }

    public sealed unsafe partial class MatrixParallelThread : L4AwareThread
    {

        private IntrinsicContext* _context;

        private readonly MatrixParallelTask* _currentTask;
        private readonly ManualResetEventSlim _resetEvent;

        // private volatile MatrixThreadState _target;

        public ThreadPriority AfterWork { get; set; }

        internal SIMDID Simd { get; private set; }

        public bool Equals(
                    MatrixParallelThread? other
        )
        {
            return (other is not null) && ReferenceEquals(this, other);
        }

        protected override void CustomizeDispose()
        {
            throw new NotImplementedException();
        }

        protected override void CustomizeInitialization()
        {
            throw new NotImplementedException();
        }

        protected override void ThreadMain()
        {
            if (Simd.Mask != 0)
            {
                SIMDID feature = new(GetSimdId());
                IntrinsicContext* contextPtr = stackalloc IntrinsicContext[1];
                LoadIntrinsicFunction(contextPtr, feature.Mask);
                Simd = feature;
                _context = contextPtr;
            }
            MatrixParallelTask* taskPtr = stackalloc MatrixParallelTask[4];
            State = L4AwareThreadState.Ready;
            while (true)
            {
                // Thread self = Thread.CurrentThread;
                _resetEvent.Wait();
                _resetEvent.Reset();
                if (Disposed)
                {
                    break;
                }
                /*
                if (_target == MatrixThreadState.Idle)
                {
                    State = MatrixThreadState.Idle;
                    continue;
                }
                /**/
                State = L4AwareThreadState.Idle;
                State = L4AwareThreadState.Running;
                ManagedThread.Priority = ThreadPriority.Highest;


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

                ManagedThread.Priority = AfterWork;
                State = L4AwareThreadState.Idle;
            }
            State = L4AwareThreadState.Retired;
        }

#pragma warning disable CA5393
        [LibraryImport(Native.LibName, EntryPoint = "nfn_get_simd_level_local")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        internal static partial ulong GetSimdId();

        [LibraryImport(Native.LibName, EntryPoint = "nfn_load_intrinsic_function")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static unsafe partial void LoadIntrinsicFunction(
                                           IntrinsicContext* context,
                                           ulong id
        );

#pragma warning restore CA5393
    }
}
