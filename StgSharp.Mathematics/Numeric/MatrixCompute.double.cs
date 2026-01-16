//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute.double"
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
using StgSharp.HighPerformance;
using StgSharp.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public partial class MatrixCompute
    {

        public static unsafe void Add(
                                  Matrix<double> left,
                                  Matrix<double> right,
                                  Matrix<double> ans
        )
        {
            int f64 = (int)MatrixElementType.F64;
            if (left.ColumnLength != right.ColumnLength ||
                left.RowLength != right.RowLength ||
                left.ColumnLength != ans.ColumnLength ||
                left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            if (count <= 256)
            {
                MatrixKernel<double>* leftPtr = left.Buffer;
                MatrixKernel<double>* rightPtr = right.Buffer;
                MatrixKernel<double>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f64].buffer_add(leftPtr, rightPtr, ansPtr, count);

                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<double>* leftPtr = left.Buffer;
                MatrixKernel<double>* rightPtr = right.Buffer;
                MatrixKernel<double>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f64].buffer_add(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<double>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(double);
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f64].buffer_add;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

        public static unsafe void Fill(
                                  Matrix<double> ans,
                                  double value
        )
        {
            int f64 = (int)MatrixElementType.F64;
            long count = (long)ans.KernelColumnLength * ans.KernelRowLength;
            ScalarPacket* scalar = MatrixParallelFactory.CreateScalarPacket();
            scalar->Data<double>(0) = value;
            if (count <= 256)
            {
                NativeIntrinsic.Context.mat[f64].buffer_fill(ans.Buffer, scalar, count);
                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);
                NativeIntrinsic.Context.mat[f64].buffer_fill(ans.Buffer, scalar, count);
                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<double>();
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(double);
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f64].buffer_fill;
            package->Scalar = scalar;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);
            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            MatrixParallelFactory.Release(scalar);
            taskGroup.Dispose();
        }

        public static unsafe void Mul(
                                  Matrix<double> left,
                                  Matrix<double> right,
                                  Matrix<double> ans
        )
        {
            int f64 = (int)MatrixElementType.F64;

            // For matrix multiplication (left: MxK) * (right: KxN) => ans: MxN
            // ColumnLength = number of columns (N), RowLength = number of rows (M)
            if (left.ColumnLength != right.RowLength ||
                ans.ColumnLength != right.ColumnLength ||
                ans.RowLength != left.RowLength)
            {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;

            // int size = MatrixPanel.KernelSideCount<double>();
            if (count <= 1024)
            {
                MatrixParallelThread[] pool = null;
                if (count > 256) {
                    pool = MatrixParallel.LeadParallel(1);
                }
                M128* enumeration = stackalloc M128[1];
                enumeration->Member<int>(2) = left.KernelColumnLength;
                enumeration->Member<int>(3) = ans.KernelColumnLength;
                for (int i = 0; i < ans.KernelRowLength; i++)
                {
                    for (int j = 0; j < ans.KernelColumnLength; j++)
                    {
                        enumeration->Member<int>(0) = i;
                        enumeration->Member<int>(1) = j;

                        NativeIntrinsic.Context
                                           .mat[f64].kernel_tile_fma((MatrixKernel*)left.Buffer,
                                                                     (MatrixKernel*)right.Buffer,
                                                                     (MatrixKernel*)ans.Buffer,
                                                                     enumeration);

                        // Console.WriteLine($"{i},{j},{ans[4*i,4*j]:0.0E00}\t");
                    }

                    // Console.WriteLine();
                }
                if (count > 256) {
                    MatrixParallel.ReturnParallelResources(ref pool!);
                }
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<double>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->AnsPrimOffset = ans.KernelRowLength;
            package->AnsSecOffset = ans.KernelColumnLength;
            package->LeftPrimOffset = left.KernelColumnLength;
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ElementSize = (int)MatrixElementType.F64;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationSpecial.SEQ_FMA);
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

        public static unsafe void Mul(
                                  Matrix<double> left,
                                  double right,
                                  Matrix<double> ans
        )
        {
            int f64 = (int)MatrixElementType.F64;
            if (left.ColumnLength != ans.ColumnLength || left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            if (left.Layout != MatrixLayout.DenseRectangle ||
                ans.Layout != MatrixLayout.DenseRectangle) {
                throw new NotSupportedException("Only dense rectangle matrices are supported.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            ScalarPacket* scalar = MatrixParallelFactory.CreateScalarPacket();
            scalar->Data<double>(0) = right;
            if (count <= 256)
            {
                MatrixKernel<double>* leftPtr = left.Buffer;
                MatrixKernel<double>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f64].buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<double>* leftPtr = left.Buffer;
                MatrixKernel<double>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f64].buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<double>();
            package->Right = (nint)left.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(double);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->Scalar = scalar;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f64].buffer_scalar_mul;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            MatrixParallelFactory.Release(scalar);
            return;
        }

        public static unsafe void Sub(
                                  Matrix<double> left,
                                  Matrix<double> right,
                                  Matrix<double> ans
        )
        {
            int f64 = (int)MatrixElementType.F64;
            if (left.ColumnLength != right.ColumnLength ||
                left.RowLength != right.RowLength ||
                left.ColumnLength != ans.ColumnLength ||
                left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            if (left.Layout != MatrixLayout.DenseRectangle ||
                right.Layout != MatrixLayout.DenseRectangle ||
                ans.Layout != MatrixLayout.DenseRectangle) {
                throw new NotSupportedException("Only dense rectangle matrices are supported.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            if (count <= 256)
            {
                MatrixKernel<double>* leftPtr = left.Buffer;
                MatrixKernel<double>* rightPtr = right.Buffer;
                MatrixKernel<double>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f64].buffer_sub(leftPtr, rightPtr, ansPtr, count);

                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<double>* leftPtr = left.Buffer;
                MatrixKernel<double>* rightPtr = right.Buffer;
                MatrixKernel<double>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f64].buffer_sub(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<double>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(double);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f64].buffer_sub;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

    }
}
