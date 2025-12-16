//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute.Additional"
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
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
=======
using StgSharp.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
>>>>>>> stgsharp-dev/giga
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public partial class MatrixCompute
    {

<<<<<<< HEAD
        private static unsafe void ComputeFmaPanelF32(
                                   MatrixKernel* left,
                                   MatrixKernel* right,
                                   MatrixKernel* ans,
                                   ScalarPacket* scalar)
        {
            const int eSize = 32;
            int colum = scalar->Data<int>(0);
            int row = scalar->Data<int>(1);
            int k = 

=======
        internal static unsafe void ComputeFmaPanelF32(MatrixParallelTaskPackage* p)
        {
            int size = p->ElementSize;
            MatrixKernel* leftBuffer = (MatrixKernel*)p->Left;
            MatrixKernel* rightBuffer = (MatrixKernel*)p->Right;
            MatrixKernel* ansBuffer = (MatrixKernel*)p->Answer;
            MatrixPanel<float>* leftPanel = MatrixPanel.Create<float>();
            MatrixPanel<float>* rightPanel = MatrixPanel.Create<float>();
            MatrixPanel<float>* ansPanel = MatrixPanel.Create<float>();
            ref long offsetRef = ref Unsafe.As<int, long>(ref p->PrimTileOffset);
            ref long lengthRef = ref Unsafe.As<int, long>(ref p->PrimCount);
            int ansWidth = p-> AnsPrimOffset;
            int ansHeight = p->AnsSecOffset;
            int commonK = p->LeftPrimOffset;
            int blocksPerRow = ansWidth / size;

            for (long c = offsetRef; c < offsetRef + lengthRef; c++)
            {
                int i = (int)(c / blocksPerRow) * size;
                int j = (int)(c % blocksPerRow) * size;
                NativeIntrinsic.Intrinsic.f32_clear_panel((MatrixPanel*)ansPanel);
                for (int k = 0; k < commonK; k += size)
                {
                    NativeIntrinsic.Intrinsic
                                   .f32_build_panel((MatrixPanel*)leftPanel, leftBuffer, commonK,
                                                    ansHeight, k, i);
                    NativeIntrinsic.Intrinsic
                                   .f32_build_panel((MatrixPanel*)rightPanel, rightBuffer, ansWidth,
                                                    commonK, j, k);
                    NativeIntrinsic.Intrinsic
                                   .f32_panel_fma((MatrixPanel*)leftPanel, (MatrixPanel*)rightPanel, (MatrixPanel*)ansPanel);
                }
                NativeIntrinsic.Intrinsic
                               .f32_store_panel((MatrixPanel*)ansPanel, ansBuffer, ansWidth,
                                                ansHeight, j, i);
            }
            MatrixPanel.Destroy(leftPanel);
            MatrixPanel.Destroy(rightPanel);
            MatrixPanel.Destroy(ansPanel);
>>>>>>> stgsharp-dev/giga
        }

    }
}
