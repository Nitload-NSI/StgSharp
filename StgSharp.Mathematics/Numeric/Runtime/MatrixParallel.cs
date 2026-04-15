//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel"
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
using StgSharp.Collections;
using StgSharp.Common.HighPerformance.ProcessorAbstraction;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace StgSharp.Mathematics.Numeric
{
    public static unsafe partial class MatrixParallel
    {

        private const int RESERVED_THREAD_COUNT = 4;

        private static bool _meaningful;

        private static CapacityFixedStack<MatrixParallelThread> _retiredThreads;

        public static bool IsParallelSupported { get; } = Environment.ProcessorCount > 6;

        public static bool IsParallelMeaningful => IsParallelSupported &&
                                                   IsInitialized &&
                                                   _meaningful;

        public static int ContextCount { get; private set; }

        private static bool IsInitialized { get; set; }

        internal static void Deinit()
        {
            foreach (MatrixParallelThread t in _threads)
            {
                t.Pause();
                t.Dispose();
            }
            foreach (MatrixParallelThread t in _retiredThreads) {
                t.Dispose();
            }
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
            SIMDID globalSimd = new SIMDID(0);
            _retiredThreads = new(RESERVED_THREAD_COUNT);

            // init thread on all logical cores
            _numaThreads = Numa.CreateForeachThread<MatrixParallelThread>(
                core => {
                    MatrixParallelThread thread = new(core);
                    SpinWait.SpinUntil(() => thread.State == MatrixThreadState.Ready);

                    SIMDID simd = thread.Simd;
                    ulong mask = simd.Mask;
                    contextMap[mask] = contextMap.TryGetValue(mask, out int contextCount) ?
                                       (contextCount + 1) :
                                       1;
                    globalSimd = (globalSimd.Mask != 0) ? UniteSIMDID(globalSimd, simd) : simd;
                    return thread;
                });

            // make global simd  context
            IntrinsicContext globalContext = new();
            LoadIntrinsicFunction(&globalContext, globalSimd.Mask);
            NumericalModule.GlobalContext = globalContext;
            NumericalModule.GlobalIntrinsicMask = globalSimd;
            ContextCount = contextMap.Count;

            // retire threads
            if (ContextCount == 1)
            {
                MatrixParallelThread[] threadArr = _numaThreads[0];
                int retireCount = Math.Min(RESERVED_THREAD_COUNT, Math.Max(0, threadArr.Length));
                for (int i = 1; i <= retireCount; i++) {
                    RetireThread(threadArr[^i]);
                }
                _numaThreads[0] = threadArr[0..^retireCount];
            } else
            {
                RetireWhenMultipleSIMDID();
            }
            _meaningful = _threads.Length > 1;

            // _awaitingLeaderHandle = new AwaitingQueue();
            IsInitialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void RetireThread(
                            MatrixParallelThread thread
        )
        {
            thread.Pause();
            thread.Dispose();
            _retiredThreads.Push(thread);
        }

        private static void RetireWhenMultipleSIMDID()
        {
            MatrixParallelThread[] allThreads = new MatrixParallelThread[Environment.ProcessorCount];
            int offset = 0;
            foreach (MatrixParallelThread[] threadArr in _numaThreads)
            {
                Array.Copy(threadArr, 0, allThreads, offset, threadArr.Length);
                offset += threadArr.Length;
            }
            Array.Sort(allThreads, static(
                                   left,
                                   right
            ) => CompareSIMDID(left.Simd, right.Simd));
            int retireCount = Math.Min(RESERVED_THREAD_COUNT, Math.Max(0, allThreads.Length));

            // HashSet<MatrixParallelThread> retiredSet = [];
            MatrixParallelThread[] retireArr = new MatrixParallelThread[retireCount];
            for (int i = 0; i < retireCount; i++)
            {
                MatrixParallelThread thread = allThreads[i];
                retireArr[i] = thread;
                RetireThread(thread);
            }

            // rebuild numa active arrays
            for (int numa = 0; numa < _numaThreads.Length; numa++)
            {
                MatrixParallelThread[] source = _numaThreads[numa];
                int keepCount = 0;

                for (int i = 0; i < source.Length; i++)
                {
                    if (!retireArr.Contains(source[i])) {
                        keepCount++;
                    }
                }

                MatrixParallelThread[] active = new MatrixParallelThread[keepCount];
                int index = 0;
                for (int i = 0; i < source.Length; i++)
                {
                    MatrixParallelThread thread = source[i];
                    if (!retireArr.Contains(thread)) {
                        active[index++] = thread;
                    }
                }

                _numaThreads[numa] = active;
            }

            _threads = allThreads[retireCount..];
        }

        #region C invoke

        [LibraryImport(Native.LibName, EntryPoint = "sn_compare_simd")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial int CompareSIMDID(
                                   SIMDID left,
                                   SIMDID right
        );

        [LibraryImport(Native.LibName, EntryPoint = "load_intrinsic_function")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial void LoadIntrinsicFunction(
                                    IntrinsicContext* context,
                                    ulong id
        );

        [LibraryImport(Native.LibName, EntryPoint = "sn_get_unite_simd")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial SIMDID UniteSIMDID(
                                      SIMDID left,
                                      SIMDID right
        );

    #endregion
    }
}
