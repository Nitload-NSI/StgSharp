// -----------------------------------------------------------------------------
// file="MatrixParallelQueue"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.VisualBasic.FileIO;
using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    internal unsafe class MatrixParallelQueue : IDisposable
    {

        private MatrixParallelTask[] _currentPack;
        private MatrixParallelTask[] _predictBuffer;
        private bool disposedValue;

        private int _currentCapacity;
        private int _cursor;
        private int _predictCapacity;

        private ManualResetEventSlim _exchangeLock = new();

        public MatrixParallelQueue()
        {
            _currentPack = new MatrixParallelTask[512];
            _predictBuffer = new MatrixParallelTask[512];
        }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public bool TryGetTask(
                    out MatrixParallelTask task
        )
        {
            if (_cursor >= _currentCapacity)
            {
                task = default;
                return false;
            }
            task = _currentPack[_cursor++];
            return true;
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing) {
                    _exchangeLock.Dispose();
                }

                disposedValue = true;
            }
        }

    }
}

