//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ThreadHelper"
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
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Threading
{
    public static partial class ThreadHelper
    {

        private static readonly int mainThreadId = Environment.CurrentManagedThreadId;

        private static readonly ThreadLocal<bool> isNotMainThread = new(
            () => Environment.CurrentManagedThreadId != mainThreadId);

        static ThreadHelper()
        {
            EmptyThread.Start();
        }

        /// <summary>
        ///   An individual thread doing nothing,  and its <see cref="Thread.IsAlive" /> <see
        ///   langword="property" /> always returns false.
        /// </summary>
        public static Thread EmptyThread { get; } = new(() => { });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindToLogicalCore(
                           LogicalCore core
        )
        {
            SetThreadAffinity(core.GlobalCpuId, core.NumaNodeId);
        }

        [Conditional("DEBUG")]
        internal static void MainThreadAssert()
        {
            if (isNotMainThread.Value) {
                throw new InvalidOperationException(
                    $"Attempt to call sensitive operation which can be only called from main thread from thread  {Environment.CurrentManagedThreadId}.");
            }
        }

#pragma warning disable CA5393

        [LibraryImport(Native.LibName, EntryPoint = "sn_set_thread_affinity")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial void SetThreadAffinity(
                                    uint globalCoreId,
                                    uint numaNodeId
        );

#pragma warning restore CA5393
    }
}
