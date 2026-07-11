// -----------------------------------------------------------------------------
// file="MatrixParallel.Numa"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;
using StgSharp.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static unsafe partial class MatrixParallel
    {

        private static L4[] _numaBuffer;
        private static NumaMeasurement[] _numaMeasurements;

        private static int[] _numaPayload;
        private static MatrixParallelThread[][] _numaSyncHeavy;
        private static MatrixParallelThread[][] _numaWaveFront;
        private static MatrixParallelThread[] _threads;

        private static void UpdateThreadPayload()
        {
            for (int i = 0; i < _numaPayload.Length; i++) { }
        }

        private struct NumaMeasurement
        {

            public uint ActivePredictors;
            public uint ActiveThreadCount;
            public ulong ActiveTasks;

        }

    }
}
