<<<<<<< HEAD
<<<<<<< HEAD
//-----------------------------------------------------------------------
=======
﻿//-----------------------------------------------------------------------
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
//-----------------------------------------------------------------------
>>>>>>> stgsharp-dev/giga
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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public partial class MatrixParallelThread : IDisposable
    {

        // Affinity binding info for this thread
        private AffinityBinding _affinity;
        private bool _affinityApplied;

        private bool disposedValue;
        private readonly ManualResetEventSlim _resetEvent;
        private readonly Thread _managedThread;

        public MatrixParallelThread()
        {
            // Allocate a unique logical processor for this worker (mutually exclusive)
            _affinity = AffinityAllocator.Allocate();
            _resetEvent = new ManualResetEventSlim(false);
            _managedThread = new Thread(MatrixParallelWorker);
            _managedThread.Start();
        }

        // Global runtime switch: true => UMA/no-affinity; false => NUMA/affinity (auto-detected)
        public static bool UseUmaAffinityPath { get; } = DetectUmaAffinityPath();

        // ~MatrixComputeThread()
        // {
        // Dispose(disposing: false);
        // }

        public ThreadPriority AfterWork
        {
            get => field;
            set => field = value;
        }

        internal ManualResetEventSlim? LeaderEvent { get; set; }

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
                MatrixParallelTaskPackage* p = (MatrixParallelTaskPackage*)handle;
                if ((short)p->ComputeMode.ParameterStyle < 0) {
                    MatrixComputeModel.SpecialBufferCompute(p);
                }
                switch (p->ComputeMode.OperationStyle)
                {
                    case MatrixIndexStyle.BUFFER_INDEX:
                        switch (p->ComputeMode.ParameterStyle)
                        {
                            case MatrixOperationParam.RIGHT_ANS_PARAM:
                                MatrixComputeModel.BufferComputeUnary(p);
                                break;
                            case MatrixOperationParam.LEFT_RIGHT_ANS_PARAM:
                                MatrixComputeModel.BufferComputeBinary(p);
                                break;
                            case MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM:
                                MatrixComputeModel.BufferComputeUnaryScalar(p);
                                break;
                            case MatrixOperationParam.LEFT_RIGHT_ANS_SCALAR_PARAM:
                                MatrixComputeModel.BufferComputeBinaryScalar(p);
                                break;
                            case MatrixOperationParam.ANS_SCALAR_PARAM:
                                MatrixComputeModel.BufferComputeNoOperatorScalar(p);
                                break;
                            default:
                                break;
                        }
                        break;
<<<<<<< HEAD
                    case MatrixOperationStyle.KERNEL_OP:
                        switch (p->ComputeMode.ParameterStyle)
                        {
                            case MatrixOperationParam.RIGHT_ANS_PARAM:
<<<<<<< HEAD
                                MatrixComputeModel.KernelComputeUnary(p);
                                break;
                            case MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM:
                                MatrixComputeModel.KernelComputeUnaryScalar(p);
                                break;
                            case MatrixOperationParam.LEFT_RIGHT_ANS_PARAM:
                                MatrixComputeModel.KernelComputeBinary(p);
                                break;
                            case MatrixOperationParam.LEFT_RIGHT_ANS_SCALAR_PARAM:
                                MatrixComputeModel.KernelComputeBinaryScalar(p);
=======
                                MatrixComputeModel.Compute_Unary(p);
                                break;
                            case MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM:
                                MatrixComputeModel.Compute_UnaryScalar(p);
                                break;
                            case MatrixOperationParam.LEFT_RIGHT_ANS_PARAM:
                                MatrixComputeModel.Compute_Binary(p);
                                break;
                            case MatrixOperationParam.LEFT_RIGHT_ANS_SCALAR_PARAM:
                                MatrixComputeModel.Compute_BinaryScalar(p);
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
                                break;
                            default:
                                break;
                        }
                        break;
=======
>>>>>>> stgsharp-dev/giga
                    default:
                        break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restart()
        {
            if (Role == RoleType.Leader)
            {
                LeaderEvent?.Set();
            } else
            {
                _resetEvent.Set();
            }
        }

        internal void MatrixParallelLeader()
        {
            Thread current = Thread.CurrentThread;
            current.Priority = ThreadPriority.Highest;
            LeaderEvent?.Wait();
            LeaderEvent?.Reset();

            // Console.WriteLine("Leader is computing");
#if WINDOWS
            if (!UseUmaAffinityPath)
            {
                // NUMA/affinity path: bind to this thread's reserved mask, then restore
                nuint previousMask = SetThreadAffinityMask(GetCurrentThread(), _affinity.Mask);
                bool affinityChanged = previousMask != 0; // nonzero => success
                Thread.Sleep(0);
                ExecuteTaskPackage(Tasks);
                if (affinityChanged) {
                    _ = SetThreadAffinityMask(GetCurrentThread(), previousMask);
                }
            } else
            {
                // UMA path: no affinity binding
                ExecuteTaskPackage(Tasks);
            }
#else
            // Non-Windows: no affinity control
            ExecuteTaskPackage(Tasks);
#endif

            BindedWrap.ReportThreadAccomplished();

            // Console.WriteLine("leader's computing is done");
            Role = RoleType.Worker;
            LeaderEvent = null;
            current.Priority = ThreadPriority.Normal;
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

                // Release the reserved logical processor back to the pool
                AffinityAllocator.Release(_affinity.LogicalProcessor);

                disposedValue = true;
            }
        }

        private static bool DetectUmaAffinityPath()
        {
#if WINDOWS
            try
            {
                // Quick heuristic: single CPU looks like UMA
                if (Environment.ProcessorCount <= 1)
                {
                    return true;
                }

                // Query NUMA topology: highest node number 0 => single node => UMA
                uint highest = 0;
                if (GetNumaHighestNodeNumber(out highest)) {
                    return highest == 0;
                }

                // If query fails, prefer UMA (safer: no affinity changes)
                return true;
            }
            catch
            {
                return true;
            }
#else
            // Non-Windows: treat as UMA (no affinity API available)
            return true;
#endif
        }

        private unsafe void MatrixParallelWorker()
        {
            // Bind OS-thread affinity once when the worker starts running
            ApplyAffinityIfNeeded();

            while (true)
            {
                // Thread self = Thread.CurrentThread;
                _managedThread.Priority = ThreadPriority.Highest;
                _resetEvent.Wait();
                _resetEvent.Reset();
                switch (Role)
                {
                    case RoleType.Worker://worker
                        // _ = Interlocked.Decrement(ref RemainThreadCount);
                        // Console.WriteLine("worker start");
                        ExecuteTaskPackage(Tasks);
                        BindedWrap.ReportThreadAccomplished();

                        // Console.WriteLine("worker end");
                        break;
                    case RoleType.Scheduler://scheduler
                        // Console.WriteLine("scheduler start");
                        BindedWrap.PublishTaskToWorkers();
                        BindedWrap.SetAllWorker();

                        // Console.WriteLine("scheduler done");
                        ExecuteTaskPackage(Tasks);
                        BindedWrap.ReportThreadAccomplished();
                        BindedWrap.WaitAllWorker();
                        BindedWrap.ReportWrapAccomplished();
                        Role = RoleType.Worker;
                        break;
                    case RoleType.Leader://leader, error because leader should be at outer thread but not here
                        break;
                    default:
                        break;
                }
                _managedThread.Priority = AfterWork;
            }
        }

        #region core affinity

        /*
         * data to record core affinity
         * preparation for future NUMA affinity setting
         */

        // Represents the binding information for a worker thread
        internal readonly struct AffinityBinding
        {

            public readonly int LogicalProcessor;
            public readonly nuint Mask;

            public AffinityBinding(int logicalProcessor, nuint mask)
            {
                LogicalProcessor = logicalProcessor;
                Mask = mask;
            }

        }

        // Thread-safe allocator that hands out unique logical processors (mutually exclusive)
        internal static class AffinityAllocator
        {

            // Bitset of reserved logical processors (0..63). One bit per CPU.
            private static long s_reservedMask; // treat as unsigned bits

            public static AffinityBinding Allocate()
            {
                int cpuCount = Environment.ProcessorCount;
                if (cpuCount > 64)
                {
                    cpuCount = 64; // SetThreadAffinityMask supports up to 64-bit mask
                }

                while (true)
                {
                    long snapshot = Volatile.Read(ref s_reservedMask);

                    // Find first zero bit
                    int candidate = -1;
                    long bit = 0;
                    for (int i = 0; i < cpuCount; i++)
                    {
                        bit = 1L << i;
                        if ((snapshot & bit) == 0)
                        {
                            candidate = i;
                            break;
                        }
                    }

                    if (candidate < 0)
                    {
                        // No free CPU found: fall back to CPU 0 (shared)
                        return new AffinityBinding(0, (nuint)1);
                    }

                    long desired = snapshot | bit;
                    if (Interlocked.CompareExchange(ref s_reservedMask, desired, snapshot) == snapshot)
                    {
                        // Successfully reserved candidate
                        nuint mask = (nuint)(1UL << candidate);
                        return new AffinityBinding(candidate, mask);
                    }

                    // else: another thread raced us; retry
                }
            }

            public static void Release(int logicalProcessor)
            {
                if ((uint)logicalProcessor >= 64u) {
                    return;
                }

                long bit = 1L << logicalProcessor;
                while (true)
                {
                    long snapshot = Volatile.Read(ref s_reservedMask);
                    if ((snapshot & bit) == 0)
                    {
                        return; // already free
                    }

                    long desired = snapshot & ~bit;
                    if (Interlocked.CompareExchange(ref s_reservedMask, desired, snapshot) == snapshot) {
                        return;
                    }
                }
            }

        }

        private void ApplyAffinityIfNeeded()
        {
            if (_affinityApplied) {
                return;
            }

#if WINDOWS
            try
            {
                // On Windows pin the current native thread to the reserved logical processor
                SetThreadAffinityMask(GetCurrentThread(), _affinity.Mask);
            }
            catch
            {
                // Ignore binding errors; continue without affinity
            }
#elif LINUX

#endif

            _affinityApplied = true;
        }

            #endregion


#if WINDOWS
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
<<<<<<< HEAD
<<<<<<< HEAD
        private static extern nuint SetThreadAffinityMask(
                                    IntPtr hThread,
                                    nuint dwThreadAffinityMask);
=======
        private static extern nuint SetThreadAffinityMask(IntPtr hThread, nuint dwThreadAffinityMask);
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
        private static extern nuint SetThreadAffinityMask(
                                    IntPtr hThread,
                                    nuint dwThreadAffinityMask);
>>>>>>> stgsharp-dev/giga

        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool GetNumaHighestNodeNumber(out uint HighestNodeNumber);

#elif LINUX

#endif

        #region wrap belong

        internal MatrixParallelWrap BindedWrap { get; set; } = null!;

        internal Queue<IntPtr> Tasks { get; } = [];

        public RoleType Role
        {
            get;
            internal set
            {
                // Console.WriteLine($"Thread {_managedThread.ManagedThreadId} set to {value}, from thread {Environment.CurrentManagedThreadId}");
                field = value;
            }
        }

        public enum RoleType : int
        {

            Worker = 0,
            Scheduler = 1,
            Leader = 2,

        }

        #endregion
    }
}
