//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute.Float"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixCompute
    {

        public static unsafe void Add(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
        {
            int f32 = (int)MatrixElementType.F32;
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
                NativeIntrinsic.Context.mat[f32].buffer_add(leftPtr, rightPtr, ansPtr, count);

                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f32].buffer_add(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f32].buffer_add;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

        public static unsafe void Fill(Matrix<float> ans, float value)
        {
            int f32 = (int)MatrixElementType.F32;
            long count = (long)ans.KernelColumnLength * ans.KernelRowLength;
            ScalarPacket* scalar = MatrixParallelFactory.CreateScalarPacket();
            scalar->Data<float>(0) = value;
            if (count <= 256)
            {
                NativeIntrinsic.Context.mat[f32].buffer_fill(ans.Buffer, scalar, count);
                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);
                NativeIntrinsic.Context.mat[f32].buffer_fill(ans.Buffer, scalar, count);
                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f32].buffer_fill;
            package->Scalar = scalar;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);
            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            MatrixParallelFactory.Release(scalar);
            taskGroup.Dispose();
        }

        public static unsafe void Mul(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
        {
            int f32 = (int)MatrixElementType.F32;

            // For matrix multiplication (left: MxK) * (right: KxN) => ans: MxN
            // ColumnLength = number of columns (N), RowLength = number of rows (M)
            if (left.ColumnLength != right.RowLength ||
                ans.ColumnLength != right.ColumnLength ||
                ans.RowLength != left.RowLength)
            {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            if (left.Layout != MatrixLayout.DenseRectangle ||
                right.Layout != MatrixLayout.DenseRectangle ||
                ans.Layout != MatrixLayout.DenseRectangle) {
                throw new NotSupportedException("Only dense rectangle matrices are supported.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;

            if (count <= 1024)
            {
                MatrixParallelThread[] pool = null;
                if (count > 256) {
                    pool = MatrixParallel.LeadParallel(1);
                }
                M128* enumeration = stackalloc M128[4];
                for (int i = 0; i < ans.KernelRowLength; i++)
                {
                    for (int j = 0; j < ans.KernelColumnLength; j++)
                    {
                        enumeration->Member<int>(0) = i;
                        enumeration->Member<int>(1) = j;
                        enumeration->Member<int>(2) = ans.KernelRowLength;
                        enumeration->Member<int>(3) = left.KernelColumnLength;

                        NativeIntrinsic.Context
                                           .mat[f32].kernel_tile_fma((MatrixKernel*)left.Buffer,
                                                                     (MatrixKernel*)right.Buffer,
                                                                     (MatrixKernel*)ans.Buffer, enumeration);
                        /*
                        for (int k = 0; k < left.KernelColumnLength; k++) {
                            NativeIntrinsic.Context
                                               .mat[f32].kernel_fma(GetKernelAddressUnsafe(left.Buffer,
                                                                                           left.KernelColumnLength, i,
                                                                                           k),
                                                                GetKernelAddressUnsafe(right.Buffer,
                                                                                       right.KernelColumnLength, k, j),
                                                                GetKernelAddressUnsafe(ans.Buffer,
                                                                                       ans.KernelColumnLength, i, j));
                        }
                        /**/
                    }
                }

                if (count > 256) {
                    MatrixParallel.ReturnParallelResources(ref pool!);
                }
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->AnsPrimOffset = ans.KernelRowLength;
            package->AnsSecOffset = ans.KernelColumnLength;
            package->LeftPrimOffset = left.KernelColumnLength;
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ElementSize = (int)MatrixElementType.F32;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationSpecial.SEQ_FMA);
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

        public static unsafe void Mul(Matrix<float> left, float right, Matrix<float> ans)
        {
            int f32 = (int)MatrixElementType.F32;
            if (left.ColumnLength != ans.ColumnLength || left.RowLength != ans.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            if (left.Layout != MatrixLayout.DenseRectangle || ans.Layout != MatrixLayout.DenseRectangle) {
                throw new NotSupportedException("Only dense rectangle matrices are supported.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            ScalarPacket* scalar = MatrixParallelFactory.CreateScalarPacket();
            scalar->Data<float>(0) = right;
            if (count <= 256)
            {
                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f32].buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f32].buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Right = (nint)left.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->Scalar = scalar;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f32].buffer_scalar_mul;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            MatrixParallelFactory.Release(scalar);
            return;
        }

        public static unsafe void Sub(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
        {
            int f32 = (int)MatrixElementType.F32;
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
                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f32].buffer_sub(leftPtr, rightPtr, ansPtr, count);

                return;
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Context.mat[f32].buffer_sub(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Context.mat[f32].buffer_sub;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

    }
}
