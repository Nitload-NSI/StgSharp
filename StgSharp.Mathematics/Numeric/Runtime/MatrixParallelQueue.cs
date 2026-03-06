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
using StgSharp.Mathematics.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    internal unsafe class MatrixParallelQueue
    {

        private static ConcurrentStack<MatrixParallelQueue> _recycle = [];
        private MatrixParallelTask* _package;
        private ulong _cursor;
        private ulong _maxCount;

        private MatrixParallelQueue() { }

        public MatrixParallelTask* Package => _package;

        public static MatrixParallelQueue Rent(
                                          MatrixParallelTask* pack
        )
        {
            if (!_recycle.TryPop(out MatrixParallelQueue? queue)) {
                queue = new MatrixParallelQueue();
            }
            queue._package = pack;
            queue._maxCount = (ulong)pack->CommonX * (ulong)pack->CommonY;
            queue._cursor = 0;
            return queue;
        }

        public static void Return(
                           MatrixParallelQueue queue
        )
        {
            _recycle.Push(queue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (ulong count, ulong offset) Steal(
                                           ulong request
        )
        {
            ulong cursor = Interlocked.Add(ref _cursor, request) - request;
            return (Math.Min(request, Math.Max(0, _maxCount - cursor)), cursor);
        }

    }
}
