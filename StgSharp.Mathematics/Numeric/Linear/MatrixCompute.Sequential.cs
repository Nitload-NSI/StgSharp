//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute.Sequential"
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
using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using unsafe MkProc = delegate* unmanaged[Cdecl]<StgSharp.Mathematics.Numeric.Runtime.MatrixParallelTask*, void>;

namespace StgSharp.Mathematics.Numeric.Linear
{
    public static unsafe partial class MatrixCompute
    {

        public static void Add(
                           Matrix<float> left,
                           Matrix<float> right,
                           Matrix<float> ans
        )
        {
            ArgumentNullException.ThrowIfNull(left, nameof(left));
            ArgumentNullException.ThrowIfNull(right, nameof(right));
            ArgumentNullException.ThrowIfNull(ans, nameof(ans));
            if ((left.ColumnLength != right.ColumnLength) ||
                (left.RowLength != right.RowLength) ||
                (left.ColumnLength != ans.ColumnLength) ||
                (left.RowLength != ans.RowLength)) {
                throw new MatrixDimensionMismatchException<float>(
                    left, right, ans
                    );
            }
            MatrixElementType f32 = MatrixElementType.F32;
            long count = ((long)left.KernelColumnLength) * left.KernelRowLength;

            MatrixKernel<float>* leftPtr = left.Buffer;
            MatrixKernel<float>* rightPtr = right.Buffer;
            MatrixKernel<float>* ansPtr = ans.Buffer;
            MkProc add = (MkProc)NumericalModule.GlobalContext.
                mat_mk[f32.IntrinsicNode].
                Get(MatrixIntrinsicHandle.Add);
            MatrixParallelTask* t = stackalloc MatrixParallelTask[1];

            t->Mat1 = (MatrixKernel*)leftPtr;
            t->Mat2 = (MatrixKernel*)rightPtr;
            t->Mat3 = (MatrixKernel*)ansPtr;
            t->ElementType = f32;
            t->ComputeHandle = MatrixIntrinsicHandle.Add;
            t->Scalar.Data<long>(0) = count;
            SequentialOperation(t, count);
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
            } else if (count <= 1024)
            {
                pool = MatrixParallel.LeadParallel(1);
                op(task);
                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            pool = MatrixParallel.LeadParallel(count / 1024);
            MatrixParallelWrap taskGroup = MatrixParallel.ScheduleTask<BufferSequentialPredictor>(pool, task);
            taskGroup.LaunchParallel(SleepMode.DeepSleep);
            taskGroup.Dispose();
        }

    }
}
