//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelWrap.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
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

        internal MatrixParallelWrap(MatrixParallelThread[] threadArray)
        {
            _threads = threadArray;
            _countdownEvent = new CountdownEvent(threadArray.Length);
        }

        public ReadOnlySpan<MatrixParallelThread> Threads => _disposed ? [] : _threads;

        public ThreadPriority AfterWork { get; private set; }

        internal L4 DataCache { get; set; }

        internal PredictPolicy Policy { get; set; }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LaunchParallel(SleepMode afterWork)
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(MatrixParallelWrap));
            AfterWork = (ThreadPriority)afterWork;
            foreach (MatrixParallelThread thread in _threads)
            {
                thread.AfterWork = AfterWork;
                thread.BindedWrap = this;
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

        private void Dispose(bool disposing)
        {
            if (_disposed) {
                return;
            }
            if (disposing)
            {
                MatrixParallel.ReturnParallelResources(ref _threads);
                _countdownEvent.Dispose();
            }
            _disposed = true;
        }

    }
}
