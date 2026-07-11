// -----------------------------------------------------------------------------
// file="MatrixCompute.Single"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using unsafe MkProc = delegate* unmanaged[Cdecl]<StgSharp.Mathematics.Numeric.Runtime.MatrixParallelTask*, void>;

namespace StgSharp.Mathematics.Numeric
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
            /*
            t->Mat1 = (MatrixKernel*)leftPtr;
            t->Mat2 = (MatrixKernel*)rightPtr;
            t->Mat3 = (MatrixKernel*)ansPtr;
            t->ElementType = f32;
            t->ComputeHandle = MatrixIntrinsicHandle.Add;
            t->Scalar.Data<long>(0) = count;
            /**/
            SequentialOperation(t, count);
        }

    }
}
