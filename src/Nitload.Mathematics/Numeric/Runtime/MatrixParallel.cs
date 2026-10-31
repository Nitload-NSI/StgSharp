// -----------------------------------------------------------------------------
// file="MatrixParallel"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.Collections;
using Nitload.Common.HighPerformance.Memory;
using Nitload.Common.HighPerformance.ProcessorAbstraction;
using Nitload.Mathematics.Numeric.Runtime;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nitload.Mathematics.Numeric
{
    public static unsafe partial class MatrixParallel
    {

        private const int RESERVED_THREAD_COUNT = 4;

        private static bool _meaningful;

        // private static CapacityFixedStack<MatrixParallelThread> _retiredThreads;

        private static L4Manager<MatrixParallelThread> _workregion;

        public static bool IsParallelSupported { get; } = Environment.ProcessorCount > 6;

        public static bool IsParallelMeaningful => IsParallelSupported &&
                                                   IsInitialized &&
                                                   _meaningful;

        public static int ContextCount { get; private set; }

        private static bool IsInitialized { get; set; }

        internal static void Deinit()
        {
            _workregion.ForeachThread(thread => {
                thread.RequestStop();
                thread.Dispose();
            });
            _workregion.Dispose();

            ContextCount = 0;
            IsInitialized = false;
        }

        internal static void Init()
        {
            Dictionary<ulong, int> contextMap = [];

            if (IsInitialized) {
                return;
            }
            if (!IsParallelSupported)
            {
                IsInitialized = true;
                return;
            }
            contextMap = [];
            int count = Environment.ProcessorCount;
            SIMDID globalSimd = new(0);

            // _retiredThreads = new(RESERVED_THREAD_COUNT);

            // init thread on all logical cores
            /*
            _numaWaveFront = Numa.CreateForeachThread<MatrixParallelThread>(
               core => {
                   MatrixParallelThread thread = new(core);
                   SpinWait.SpinUntil(() => thread.State == L4AwareThreadState.Ready);

                   SIMDID simd = thread.Simd;
                   ulong mask = simd.Mask;
                   contextMap[mask] = contextMap.TryGetValue(mask, out int contextCount) ?
                                      (contextCount + 1) :
                                      1;
                   globalSimd = (globalSimd.Mask != 0) ? UniteSIMDID(globalSimd, simd) : simd;
                   return thread;
               });
            /**/
#pragma warning disable CA2000
            L4Manager<MatrixParallelThread> manager = L4Manager.Create<MatrixParallelThread>(
                () => new(), 8192 * 8192, L4ManagerCapacityLayout.PerNuma
             );
#pragma warning restore CA2000
            manager.ForeachThread(thread => {
                SIMDID simd = thread.Simd;
                ulong mask = simd.Mask;
                contextMap[mask] = contextMap.TryGetValue(mask, out int contextCount) ?
                                   (contextCount + 1) :
                                   1;
                globalSimd = (globalSimd.Mask != 0) ? UniteSIMDID(globalSimd, simd) : simd;
            });

            // make global simd  context
            IntrinsicContext globalContext = new();
            LoadIntrinsicFunction(&globalContext, globalSimd.Mask);
            NumericalModule.GlobalContext = globalContext;
            NumericalModule.GlobalIntrinsicMask = globalSimd;
            ContextCount = contextMap.Count;

            manager.RetireByOrder(SimdComparer.Instance, L4ManagerRetirePolicy.Auto);

            // retire threads
            /*
            if (ContextCount == 1)
            {
                MatrixParallelThread[] threadArr = _numaWaveFront[0];
                int retireCount = Math.Min(RESERVED_THREAD_COUNT, Math.Max(0, threadArr.Length));
                for (int i = 1; i <= retireCount; i++) {
                    RetireThread(threadArr[^i]);
                }
                _numaWaveFront[0] = threadArr[0..^retireCount];
            } else
            {
                RetireWhenMultipleSIMDID(_numaWaveFront);
            }
            /**/
            _meaningful = _workregion.ActiveThreadCount > 1;

            // _awaitingLeaderHandle = new AwaitingQueue();
            IsInitialized = true;
        }

        /*
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void RetireThread(
                            MatrixParallelThread thread
        )
        {
            thread.Pause();
            thread.Dispose();
            _retiredThreads.Push(thread);
        }
        /**/

        #region C invoke

        [LibraryImport(Native.LibName, EntryPoint = "nfn_compare_simd")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial int CompareSIMDID(
                                   SIMDID left,
                                   SIMDID right
        );

        [LibraryImport(Native.LibName, EntryPoint = "nfn_load_intrinsic_function")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial void LoadIntrinsicFunction(
                                    IntrinsicContext* context,
                                    ulong id
        );

        [LibraryImport(Native.LibName, EntryPoint = "nfn_get_unite_simd")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial SIMDID UniteSIMDID(
                                      SIMDID left,
                                      SIMDID right
        );

    #endregion
    }
}
