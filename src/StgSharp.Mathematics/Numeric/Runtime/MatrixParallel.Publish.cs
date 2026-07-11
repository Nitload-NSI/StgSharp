// -----------------------------------------------------------------------------
// file="MatrixParallel.Publish"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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
        )

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
