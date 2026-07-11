// -----------------------------------------------------------------------------
// file="MatrixCompute"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.Memory;
using StgSharp.Internal;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using unsafe MkProc = delegate* unmanaged[Cdecl]<StgSharp.Mathematics.Numeric.Runtime.MatrixParallelTask*, void>;

namespace StgSharp.Mathematics.Numeric
{
    public static unsafe partial class MatrixCompute
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe MatrixKernel* GetKernelAddressUnsafe<T>(
                                             MatrixKernel<T>* buffer,
                                             int colLength,
                                             int x,
                                             int y
        ) where T : unmanaged, INumber<T>
        {
            return (MatrixKernel*)(buffer + ((colLength * x) + y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SequentialOperation(
                            MatrixParallelTask* task,
                            long count
        )
        {
            MatrixElementType elementType = task->ElementType;
            MatrixParallelThread[] pool;
            MkProc op = (MkProc)NumericalModule.GlobalContext.
                mat_mk[elementType.IntrinsicNode].
                Get(task->ComputeHandle);
            if ((count <= 256) || !MatrixParallel.IsParallelMeaningful)
            {
                op(task);
                return;
            }
            MatrixParallelWrap taskWrap = /**/ null/*MatrixParallel.ScheduleTask<BufferQueueLeftRightAns_NoNuma>(task)/**/;
            taskWrap.LaunchParallel(SleepMode.DeepSleep);
            taskWrap.Dispose();
        }

    }
}
