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

        public static unsafe Matrix<float> Add(Matrix<float> left, Matrix<float> right)
        {
            if (left.ColumnLength != right.ColumnLength || left.RowLength != right.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            Matrix<float> ans = Matrix<float>.FromDefault(left.ColumnLength, left.RowLength);
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            if (count <= 256)
            {
                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                for (int i = 0; i < count; i++) {
                    NativeIntrinsic.Intrinsic.f32_add(leftPtr + i, rightPtr + i, ansPtr + i);
                }
                return ans;
            }
            if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                MatrixKernel<float>* rightPtr = right.Buffer;
                MatrixKernel<float>* ansPtr = ans.Buffer;
                for (int i = 0; i < count; i++) {
                    NativeIntrinsic.Intrinsic.f32_add(leftPtr + i, rightPtr + i, ansPtr + i);
                }
                MatrixParallel.ReturnParallelResources(ref pool);
                return ans;
            }
            MatrixParallelHandle taskGroup = MatrixParallel.PublicTask(left.GetColumnEnumeration(),
                                                                       right.GetColumnEnumeration(),
                                                                       ans.GetColumnEnumeration(),
                                                                       (delegate*<MatrixKernel<float>*, MatrixKernel<float>*, MatrixKernel<float>*, void>)NativeIntrinsic.Intrinsic.f32_add);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            /*
             * run parallel
             */
            taskGroup.Dispose();
            return ans;
        }

        public static unsafe void Fill<T>(Matrix<float> left, T value) where T: unmanaged, INumber<T>
        {
            long count = (long)left.KernelColumnLength * left.KernelRowLength;
            ScalarPacket* scaler = MatrixParallelFactory.CreateScalarPacket();
            scaler->BroadCast<T, M512>(0, value);
            if (count <= 256)
            {
                MatrixKernel<float>* leftPtr = left.Buffer;
                for (int i = 0; i < count; i++) {
                    NativeIntrinsic.Intrinsic.f32_fill(leftPtr + i, scaler);
                }
            }
            if (count <= 1024)
            {
                MatrixParallelThread[] pool = MatrixParallel.LeadParallel(1);

                MatrixKernel<float>* leftPtr = left.Buffer;
                for (int i = 0; i < count; i++) {
                    NativeIntrinsic.Intrinsic.f32_fill(leftPtr + i, scaler);
                }
                MatrixParallel.ReturnParallelResources(ref pool);
            }
            MatrixParallelHandle taskGroup = MatrixParallel.PublicNoAnswerTask(left.GetColumnEnumeration(), scaler,
                (delegate* unmanaged[Cdecl]<MatrixKernel<float>*, ScalarPacket*, void>)NativeIntrinsic.Intrinsic.f32_add);

            MatrixParallel.LaunchParallel(taskGroup, SleepMode.DeepSleep);
            /*
             * run parallel
             */
            taskGroup.Dispose();
        }

    }
}
