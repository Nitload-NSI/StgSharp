//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.TaskPublic.cs"
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
using StgSharp.HighPerformance;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    public static partial class MatrixParallel
    {

        public static unsafe void PublicTask<T>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation)
            where T: unmanaged, INumber<T>
        {
            int columnCount = left.PrimSize;
            int rowCount = right.PrimSize;

            MatrixParallelTaskPackage<T> package = new()
            {
                Left = left.Source,
                Right = right.Source,
                Result = ans.Source,
                LeftPrimOffset = left.PrimOffset,
                RightPrimOffset = right.PrimOffset,
                ResultPrimOffset = ans.PrimOffset,
                LeftPrimStride = left.PrimStride,
                RightPrimStride = right.PrimStride,
                ResultPrimStride = ans.PrimStride,
                LeftSecOffset = left.SecondaryOffset,
                RightSecOffset = right.SecondaryOffset,
                ResultSecOffset = ans.SecondaryOffset,
                LeftSecStride = left.SecondaryStride,
                RightSecStride = right.SecondaryStride,
                ResultSecStride = ans.SecondaryStride,
                PrimCount = left.PrimSize,
                SecCount = left.SecondarySize,
                ComputeHandle = (nint)Operation
            };
        }

    }
}
