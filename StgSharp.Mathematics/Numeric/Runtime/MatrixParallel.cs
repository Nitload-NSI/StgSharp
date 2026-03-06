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
using StgSharp.Internal;
using StgSharp.Mathematics.Internal;
using StgSharp.Mathematics.Internal;
using StgSharp.Mathematics.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        private static bool _meaningful;

        private static CapacityFixedStack<MatrixParallelThread> _threads;

        private static ConcurrentDictionary<SIMDID, nint> contextMap;

        private static SlabAllocator<IntrinsicContext> contextAllocator;

        public static bool IsParallelSupported { get; } = Environment.ProcessorCount > 6;

        public static bool IsParallelMeaningful => Environment.ProcessorCount > 6 && IsInitialized;

        private static bool IsInitialized { get; set; } = false;

        internal static unsafe IntrinsicContext* GetContext(
                                                 SIMDID simd
        )
        {
            if (!IsParallelSupported) {
                return null;
            }
            if (contextMap.TryGetValue(simd, out nint handle))
            {
                return (IntrinsicContext*)handle;
            } else
            {
                IntrinsicContext* context = (IntrinsicContext*)contextAllocator.Allocate();

                return context;
            }
        }

        internal static void Init()
        {
            if (IsInitialized) {
                return;
            }
            if (!IsParallelSupported)
            {
                IsInitialized = true;
                return;
            }
            MatrixParallelFactory.Init();
            contextAllocator = SlabAllocator<IntrinsicContext>.Create(4, SlabBufferLayout.Chunked, false);
            contextMap = [];
            int count = Environment.ProcessorCount;
            _threads = new(count);
            for (int i = 0; i < count; i++)
            {
                MatrixParallelThread thread = new MatrixParallelThread(i);
                _threads.Push(thread);
                SpinWait.SpinUntil(() => thread.State == MatrixThreadState.Ready);
            }

            // TODO 构建全局的保底上下文
            foreach (MatrixParallelThread thread in _threads) { }
            if (contextMap.Keys.Count == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    MatrixParallelThread thread = _threads.Pop();
                    thread.Dispose();
                }
            } else
            {
                // TODO 排序并丢弃四个能力最弱线程
            }
            _awaitingLeaderHandle = new AwaitingQueue();
            IsInitialized = true;
        }

        [LibraryImport(Native.LibName, EntryPoint = "sn_compare_simd")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static unsafe partial ulong CompareSIMDID(
                                            IntrinsicContext* context,
                                            ulong id
        );

        [LibraryImport(Native.LibName, EntryPoint = "load_intrinsic_function")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static unsafe partial void LoadIntrinsicFunction(
                                           IntrinsicContext* context,
                                           ulong id
        );

    }
}
