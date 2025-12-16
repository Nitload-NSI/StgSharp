<<<<<<< HEAD
//-----------------------------------------------------------------------
=======
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
//-----------------------------------------------------------------------
=======
﻿//-----------------------------------------------------------------------
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
//-----------------------------------------------------------------------
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
>>>>>>> stgsharp-dev/giga
// -----------------------------------------------------------------------
// file="MatrixCompute"
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
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
=======
using StgSharp.Collections;
using StgSharp.HighPerformance;
using StgSharp.Internal;
using System;
using System.Collections.Generic;
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
>>>>>>> stgsharp-dev/giga
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
<<<<<<< HEAD
    public static partial class MatrixCompute { }
=======
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
    public static partial class MatrixCompute
    {

        public static unsafe void Add(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
=======
    public static class MatrixCompute
    {

        public static unsafe Matrix<float> Add(
                                           Matrix<float> left,
                                           Matrix<float> right,
                                           Matrix<float> ans)
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
    public static partial class MatrixCompute
    {

        public static unsafe void Add(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
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

<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
                return;
=======
                return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
                return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_add(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
                return;
=======
                return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
                return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
=======
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
            package->ComputeMode = new MatrixOpMode(MatrixOperationStyle.BUFFER_OP, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
========
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_add;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
            return;
=======
            return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
            return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
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
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = ans.KernelColumnLength;
            package->SecCount = ans.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_fill;
            package->Scalar = scalar;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);
            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            MatrixParallelFactory.Release(scalar);
            taskGroup.Dispose();
        }

<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
        public static unsafe void Mul(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
        {
            // For matrix multiplication (left: MxK) * (right: KxN) => ans: MxN
            // ColumnLength = number of columns (N), RowLength = number of rows (M)
            if (left.ColumnLength != right.RowLength ||
                ans.ColumnLength != right.ColumnLength ||
                ans.RowLength != left.RowLength)
            {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            if (count <= 1024)
            {
                MatrixParallelThread[] pool = null;
                if (count > 256) {
                    pool = MatrixParallel.LeadParallel(1);
                }
                MatrixPanel<float>* leftPanel = MatrixPanel.Create<float>();
                MatrixPanel<float>* rightPanel = MatrixPanel.Create<float>();
                MatrixPanel<float>* ansPanel = MatrixPanel.Create<float>();
                int size = MatrixPanel.KernelSideCount<float>();
                for (int i = 0; i < left.RowLength; i += size)
                {
                    for (int j = 0; j < right.ColumnLength; j += size)
                    {
                        NativeIntrinsic.Intrinsic.f32_clear_panel((MatrixPanel*)ansPanel);
                        for (int k = 0; k < left.ColumnLength; k += size)
                        {
                            NativeIntrinsic.Intrinsic
                                           .f32_build_panel((MatrixKernel*)left.Buffer,
                                                            (MatrixPanel*)leftPanel,
                                                            left.ColumnLength, left.RowLength, i,
                                                            k);
                            NativeIntrinsic.Intrinsic
                                           .f32_build_panel((MatrixKernel*)right.Buffer,
                                                            (MatrixPanel*)rightPanel,
                                                            right.ColumnLength, right.RowLength, k,
                                                            j);
                            NativeIntrinsic.Intrinsic
                                           .f32_panel_fma((MatrixPanel*)leftPanel,
                                                            (MatrixPanel*)rightPanel,
                                                            (MatrixPanel*)ansPanel);
                        }
                        NativeIntrinsic.Intrinsic
                                       .f32_store_panel((MatrixKernel*)ans.Buffer,
                                                        (MatrixPanel*)ansPanel, ans.ColumnLength,
                                                        ans.RowLength, i, j);
                    }
                }
                MatrixPanel.Destroy(leftPanel);
                MatrixPanel.Destroy(rightPanel);
                MatrixPanel.Destroy(ansPanel);
                if (count > 256) {
                    MatrixParallel.ReturnParallelResources(ref pool!);
                }
                return;
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Right = (nint)left.Buffer;
            package->Result = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixOperationStyle.BUFFER_OP, MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_scalar_mul;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

        public static unsafe void Mul(Matrix<float> left, float right, Matrix<float> ans)
=======
        public static unsafe Matrix<float> Mul(
                                           Matrix<float> left,
                                           Matrix<float> right,
                                           Matrix<float> ans)
========
        public static unsafe void Mul(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
        {
            // For matrix multiplication (left: MxK) * (right: KxN) => ans: MxN
            // ColumnLength = number of columns (N), RowLength = number of rows (M)
            if (left.ColumnLength != right.RowLength ||
                ans.ColumnLength != right.ColumnLength ||
                ans.RowLength != left.RowLength)
            {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            int size = MatrixPanel.KernelSideCount<float>();
            if (count <= 1024)
            {
                MatrixParallelThread[] pool = null;
                if (count > 256) {
                    pool = MatrixParallel.LeadParallel(1);
                }
                MatrixPanel<float>* leftPanel = MatrixPanel.Create<float>();
                MatrixPanel<float>* rightPanel = MatrixPanel.Create<float>();
                MatrixPanel<float>* ansPanel = MatrixPanel.Create<float>();
                for (int i = 0; i < ans.KernelRowLength; i += size)
                {
                    for (int j = 0; j < ans.KernelColumnLength; j += size)
                    {
                        NativeIntrinsic.Intrinsic.f32_clear_panel((MatrixPanel*)ansPanel);

                        // Console.WriteLine($"{i}{j}");
                        for (int k = 0; k < left.KernelColumnLength; k += size)
                        {
                            NativeIntrinsic.Intrinsic
                                           .f32_build_panel((MatrixPanel*)leftPanel,
                                                            (MatrixKernel*)left.Buffer,
                                                            left.KernelColumnLength,
                                                            left.KernelRowLength, i, k);
                            NativeIntrinsic.Intrinsic
                                           .f32_build_panel((MatrixPanel*)rightPanel,
                                                            (MatrixKernel*)right.Buffer,
                                                            right.KernelColumnLength,
                                                            right.KernelRowLength, k, j);
                            NativeIntrinsic.Intrinsic
                                           .f32_panel_fma((MatrixPanel*)leftPanel,
                                                            (MatrixPanel*)rightPanel,
                                                            (MatrixPanel*)ansPanel);
                        }
                        NativeIntrinsic.Intrinsic
                                       .f32_store_panel((MatrixPanel*)ansPanel,
                                                        (MatrixKernel*)ans.Buffer,
                                                        ans.KernelColumnLength, ans.KernelRowLength,
                                                        i, j);
                    }
                }
                MatrixPanel.Destroy(leftPanel);
                MatrixPanel.Destroy(rightPanel);
                MatrixPanel.Destroy(ansPanel);
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
            package->PrimCount = ans.KernelColumnLength / size;
            package->SecCount = ans.KernelRowLength / size;
            package->ElementSize = size;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationSpecial.SEQ_FMA);
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            return;
        }

<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
        public static unsafe Matrix<float> Mul(Matrix<float> left, float right, Matrix<float> ans)
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
        public static unsafe void Mul(Matrix<float> left, float right, Matrix<float> ans)
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
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

<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
                return;
=======
                return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
                return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_scalar_mul(leftPtr, ansPtr, scalar, count);

                MatrixParallel.ReturnParallelResources(ref pool);
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
                return;
=======
                return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
                return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Right = (nint)left.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->Scalar = scalar;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.RIGHT_ANS_SCALAR_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_scalar_mul;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
            MatrixParallelFactory.Release(scalar);
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
            return;
        }

        public static unsafe void Sub(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
=======
            return ans;
        }

        public static unsafe Matrix<float> Sub(
                                           Matrix<float> left,
                                           Matrix<float> right,
                                           Matrix<float> ans)
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
            return;
        }

        public static unsafe void Sub(Matrix<float> left, Matrix<float> right, Matrix<float> ans)
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
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

<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
                return;
=======
                return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
                return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            } else if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                NativeIntrinsic.Intrinsic.f32_buffer_sub(leftPtr, rightPtr, ansPtr, count);

                MatrixParallel.ReturnParallelResources(ref pool);
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
                return;
=======
                return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
                return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
            }
            MatrixParallelTaskPackage* package = MatrixParallelFactory.CreateBaseTask<float>();
            package->Left = (nint)left.Buffer;
            package->Right = (nint)right.Buffer;
            package->Answer = (nint)ans.Buffer;
            package->ElementSize = sizeof(float);
            package->PrimCount = left.KernelColumnLength;
            package->SecCount = left.KernelRowLength;
            package->ComputeMode = new MatrixOpMode(MatrixIndexStyle.BUFFER_INDEX, MatrixOperationParam.LEFT_RIGHT_ANS_PARAM);
            package->ComputeHandle = (IntPtr)NativeIntrinsic.Intrinsic.f32_buffer_sub;
            MatrixParallelHandle taskGroup = MatrixParallel.ScheduleTask(package);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            taskGroup.Dispose();
<<<<<<<< HEAD:StgSharp.Mathematics/Numeric/MatrixCompute.Basic.cs
<<<<<<< HEAD
            return;
=======
            return ans;
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
========
            return;
>>>>>>>> stgsharp-dev/giga:StgSharp.Mathematics/Numeric/MatrixCompute.cs
        }

    }
>>>>>>> stgsharp-dev/giga
}
