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
    internal unsafe interface IMatrixParallelQueue<TSelf> where TSelf : IMatrixParallelQueue<TSelf>
    {

        bool InitNumaPredictor(
             out IL4Predict predictor
        );

        bool RequestTaskResource(
             MatrixParallelTask* task,
             out L4CacheLine left,
             out L4CacheLine right,
             out L4CacheLine ans
        );

        void ReturnTaskResource(
             MatrixParallelTask* task,
             in L4CacheLine left,
             in L4CacheLine right,
             in L4CacheLine ans
        );

    }

    internal interface IBufferQueueLeftRightAns : IMatrixParallelQueue<IBufferQueueLeftRightAns> { }

    internal sealed unsafe class BufferQueueLeftRightAns_NoNuma : IMatrixParallelQueue<IBufferQueueLeftRightAns>
    {

        private MatrixParallelTask* _baseTask;
        private readonly nuint _byteSize;
        private readonly nuint _count;
        private volatile nuint _cursor;

        private volatile nuint _mat1;
        private volatile nuint _mat2;
        private volatile nuint _mat3;

        private BufferQueueLeftRightAns_NoNuma() { }

        internal BufferQueueLeftRightAns_NoNuma(
                 MatrixParallelTask* baseTask
        )
        {
            _baseTask = baseTask;
            _count = ((nuint)_baseTask->CommonX) * ((nuint)_baseTask->CommonY);
            _byteSize = ((nuint)_baseTask->Mat1) + (((nuint)_baseTask->CommonX) * ((nuint)_baseTask->CommonY) * _baseTask->ElementType.Size);


            _mat1 = (nuint)_baseTask->Mat1;
            _mat2 = (nuint)_baseTask->Mat2;
            _mat3 = (nuint)_baseTask->Mat3;
        }

        public bool InitNumaPredictor(
                    out IL4Predict predictor
        )
        {
            predictor = null;
            return false;
        }

        public unsafe bool RequestTaskResource(
                           MatrixParallelTask* task,
                           out L4CacheLine left,
                           out L4CacheLine right,
                           out L4CacheLine ans
        )
        {
            left = new L4CacheLine(0, ref Unsafe.AsRef<int>(null), ref Unsafe.AsRef<int>(null));
            right = new L4CacheLine(0, ref Unsafe.AsRef<int>(null), ref Unsafe.AsRef<int>(null));
            ans = new L4CacheLine(0, ref Unsafe.AsRef<int>(null), ref Unsafe.AsRef<int>(null));
            return true;
        }

        public unsafe void ReturnTaskResource(
                           MatrixParallelTask* task,
                           in L4CacheLine left,
                           in L4CacheLine right,
                           in L4CacheLine ans
        )
        {
            return;
        }

    }
}

