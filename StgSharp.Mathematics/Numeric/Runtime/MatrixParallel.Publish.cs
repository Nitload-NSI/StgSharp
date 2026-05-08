//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Publish"
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
using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        private const int MIN_TASK_PER_THREAD = 256;
        private const int REC_TASK_PER_THREAD = 1024;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        internal static unsafe MatrixParallelWrap ScheduleTask<TTaskQueue>(
                                                  MatrixParallelTask* package
        ) where TTaskQueue : IMatrixParallelQueue<TTaskQueue>
        {
            /*
            int primCount = package->PrimCount;
            int secCount = package->SecCount;
            long count = (long)primCount * secCount;
            /**/

            // Console.WriteLine($"amount of kernels is {count}");
            long payload = 0;
            int payloadIndex = 0;
            ulong additionalPayload = 0;
            for (int i = 0; i < _numaMeasurements.Length; i++)
            {
                MatrixParallelThread[] numa = _numaWaveFront[i];
                if ((numa is null) || (numa.Length == 0))
                {
                    continue;
                }
                ulong total = (ulong)_numaWaveFront[i].Length;
                NumaMeasurement measure = _numaMeasurements[i];
                ulong numaPayload = measure.ActiveTasks;

                // uint numaActive = measure.ActiveThreadCount;
                long curPayload = ((long)total) - (((long)(numaPayload + additionalPayload + 63)) / 64);
                if (curPayload < payload)
                {
                    payload = curPayload;
                    payloadIndex = i;
                }
            }
            L4 cache = _numaBuffer[payloadIndex];
            MatrixParallelThread[] selectedNuma = _numaWaveFront[payloadIndex];
            throw new NotImplementedException();
        }

    }
}
