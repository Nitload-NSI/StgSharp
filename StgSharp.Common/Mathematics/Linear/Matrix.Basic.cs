//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Matrix.Basic.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    public static class Matrix
    {

        public static Matrix<float> Add(Matrix<float> left, Matrix<float> right)
        {
            if (left.ColumnLength != right.ColumnLength || left.RowLength != right.RowLength) {
                throw new IndexOutOfRangeException("Matrix dimensions must agree.");
            }
            MatrixParallel.LeadParallel();
            try
            {
                Matrix<float> result = GCMatrix<float>.FromDefault(left.ColumnLength, left.RowLength);
                unsafe
                {
                    MatrixParallelTaskPackage<float>* handle = MatrixParallelFactory.CreateBaseTask<float>();
                    handle->Left = left.Buffer;
                    handle->Right = right.Buffer;
                    handle->Result = result.Buffer;
                    handle->ComputeHandle = (IntPtr)InternalIO.Intrinsic.f32_add;
                    handle->LeftPrimOffset = 0;
                    handle->RightPrimOffset = 0;
                    handle->LeftPrimStride = left.KernelColumnLength;
                    handle->LeftSecOffset = 1;
                    handle->LeftPrimOffset = 0;
                    handle->RightPrimOffset = 0;
                    handle->RightPrimStride = right.KernelColumnLength;
                    handle->RightSecOffset = 1;
                    handle->RightPrimOffset = 0;
                    handle->ResultPrimOffset = 0;
                    handle->ResultPrimStride = result.KernelColumnLength;
                    handle->ResultSecOffset = 1;
                    handle->ResultPrimOffset = 0;
                    handle->PrimCount = left.KernelRowLength;
                    handle->SecCount = left.KernelColumnLength;
                }

                MatrixParallel.LaunchParallel(SleepMode.DeepSleep);
                return result;
            }
            finally
            {
                MatrixParallel.EndParallel();
            }
        }

    }
}
