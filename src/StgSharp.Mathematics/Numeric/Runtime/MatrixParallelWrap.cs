// -----------------------------------------------------------------------------
// file="MatrixParallelWrap"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;
using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public enum SleepMode : int
    {

        QuickWakeup = ThreadPriority.AboveNormal,
        NormalSleep = ThreadPriority.BelowNormal,
        DeepSleep = ThreadPriority.Lowest,

    }

    public class MatrixParallelWrap : IDisposable
    {

        private MatrixParallelThread[] _threads;
        private bool _disposed;
        private readonly CountdownEvent _countdownEvent;

        internal MatrixParallelWrap(
                 MatrixParallelThread[] threadArray
        )
        {
            _threads = threadArray;
            _countdownEvent = new CountdownEvent(threadArray.Length);
        }

        public ReadOnlySpan<MatrixParallelThread> Threads => _disposed ? [] : _threads;

        public ThreadPriority AfterWork { get; private set; }

        internal L4 DataCache { get; set; }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LaunchParallel(
                    SleepMode afterWork
        )
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(MatrixParallelWrap));
            AfterWork = (ThreadPriority)afterWork;
            foreach (MatrixParallelThread thread in _threads)
            {
                thread.AfterWork = AfterWork;

                // thread.BindedWrap = this;
            }
            WaitAllWraps();
        }

        public void WaitAllWraps()
        {
            if (_disposed) {
                return;
            }
            _countdownEvent.Wait();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ReportThreadAccomplished()
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(MatrixParallelWrap));
            _ = _countdownEvent.Signal();
        }

        private void Dispose(
                     bool disposing
        )
        {
            if (_disposed) {
                return;
            }
            if (disposing)
            {
                // MatrixParallel.ReturnParallelResources(ref _threads);
                _countdownEvent.Dispose();
            }
            _disposed = true;
        }

    }
}
