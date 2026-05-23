//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelQueue"
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
                    in MatrixParallelTask task
        )
        {
            if (_cursor >= _currentCapacity) {
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

