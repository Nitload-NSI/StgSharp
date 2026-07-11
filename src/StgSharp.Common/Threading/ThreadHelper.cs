// -----------------------------------------------------------------------------
// file="ThreadHelper"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.ProcessorAbstraction;
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
