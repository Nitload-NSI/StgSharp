//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute.Basic"
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
using StgSharp.HighPerformance;
using StgSharp.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static class MatrixCompute
    {

        public static unsafe Matrix<float> Add(
                                           Matrix<float> left,
                                           Matrix<float> right,
                                           Matrix<float> ans)
        {
            if (left.ColumnLength != right.ColumnLength ||
                left.RowLength != right.RowLength ||
                left.ColumnLength != ans.ColumnLength ||
                left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            if (count <= 256)
            {
                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_add(leftPtr, rightPtr, ansPtr, count);

                return ans;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_add(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return ans;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Result = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixOperationStyle.BUFFER_OP, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_add;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return ans;
        }

        public static unsafe void Fill(Matrix<float> ans, float value)
        {
            long count = (long)ans.KernelColumnLength * ans.KernelRowLength;
            ScalarPacket* scalar = MatrixParallelFactory.CreateScalarPacket();
            scalar->Data<float>(0) = value;
            if (count <= 256)
            {
                NativeIntrinsic.Intrinsic.f32_buffer_fill(ans.Buffer, scalar, count);
                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);
                NativeIntrinsic.Intrinsic.f32_buffer_fill(ans.Buffer, scalar, count);
                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Result = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixOperationStyle.BUFFER_OP, MatrixOperationParam.ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_fill;
            package->Scalar = scalar;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);
            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            MatrixParallelFactory.Release(scalar);
            taskGroup.Dispose();
        }

        public static unsafe Matrix<float> Mul(
                                           Matrix<float> left,
                                           Matrix<float> right,
                                           Matrix<float> ans)
        {
            if (left.RowLength != right.ColumnLength ||
                left.RowLength != ans.RowLength ||
                right.ColumnLength != ans.ColumnLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            throw new NotImplementedException();
        }

        public static unsafe Matrix<float> Mul(Matrix<float> left, float right, Matrix<float> ans)
        {
            if (left.ColumnLength != ans.ColumnLength || left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            ScalarPacket* scalar = MatrixParallelFactory.CreateScalarPacket();
            scalar->Data<float>(0) = right;
            if (count <= 256)
            {
                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                return ans;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return ans;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Right = (nint)left.Buffer;
            package->Result = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->Scalar = scalar;
            package->ComputeMode = new MatrixOpMode(MatrixOperationStyle.BUFFER_OP, MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_scalar_mul;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            MatrixParallelFactory.Release(scalar);
            return ans;
        }

        public static unsafe Matrix<float> Sub(
                                           Matrix<float> left,
                                           Matrix<float> right,
                                           Matrix<float> ans)
        {
            if (left.ColumnLength != right.ColumnLength ||
                left.RowLength != right.RowLength ||
                left.ColumnLength != ans.ColumnLength ||
                left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            if (count <= 256)
            {
                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_sub(leftPtr, rightPtr, ansPtr, count);

                return ans;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_sub(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return ans;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Result = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixOperationStyle.BUFFER_OP, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_sub;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return ans;
        }

    }
}
