//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Publish.cs"
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

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        public static unsafe void PublicTask<T>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation)
            where T: unmanaged, INumber<T>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 1; // Unary

            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation)
            where T: unmanaged, INumber<T>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 3; // Binary

            PublicTaskPrivate<T>(package);
        }

        // Unary with 1..8 scalars (each scalar is independent generic type)
        public static unsafe void PublicTask<T, TScalar0>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2; // UnaryScalar
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        // Binary with 1..8 scalars (each scalar is independent generic type)
        public static unsafe void PublicTask<T, TScalar0>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4; // BinaryScalar
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4, TScalar5>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4,
                                  TScalar5 s5)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
            where TScalar5: unmanaged, INumber<TScalar5>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            sp->Data<TScalar5>(5) = s5;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4, TScalar5, TScalar6>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4,
                                  TScalar5 s5,
                                  TScalar6 s6)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
            where TScalar5: unmanaged, INumber<TScalar5>
            where TScalar6: unmanaged, INumber<TScalar6>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            sp->Data<TScalar5>(5) = s5;
            sp->Data<TScalar6>(6) = s6;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4, TScalar5>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4,
                                  TScalar5 s5)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
            where TScalar5: unmanaged, INumber<TScalar5>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            sp->Data<TScalar5>(5) = s5;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4, TScalar5, TScalar6, TScalar7>(
                                  MatrixSegmentEnumeration<T> source,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4,
                                  TScalar5 s5,
                                  TScalar6 s6,
                                  TScalar7 s7)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
            where TScalar5: unmanaged, INumber<TScalar5>
            where TScalar6: unmanaged, INumber<TScalar6>
            where TScalar7: unmanaged, INumber<TScalar7>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Right = source.Source;
            package->Result = ans.Source;
            package->RightPrimOffset = source.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->RightPrimStride = source.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->RightSecOffset = source.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->RightSecStride = source.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = source.PrimSize;
            package->SecCount = source.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 2;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            sp->Data<TScalar5>(5) = s5;
            sp->Data<TScalar6>(6) = s6;
            sp->Data<TScalar7>(7) = s7;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4, TScalar5, TScalar6>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4,
                                  TScalar5 s5,
                                  TScalar6 s6)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
            where TScalar5: unmanaged, INumber<TScalar5>
            where TScalar6: unmanaged, INumber<TScalar6>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            sp->Data<TScalar5>(5) = s5;
            sp->Data<TScalar6>(6) = s6;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        public static unsafe void PublicTask<T, TScalar0, TScalar1, TScalar2, TScalar3, TScalar4, TScalar5, TScalar6, TScalar7>(
                                  MatrixSegmentEnumeration<T> left,
                                  MatrixSegmentEnumeration<T> right,
                                  MatrixSegmentEnumeration<T> ans,
                                  delegate*<MatrixKernel<T>*, MatrixKernel<T>*, MatrixKernel<T>*, void> Operation,
                                  TScalar0 s0,
                                  TScalar1 s1,
                                  TScalar2 s2,
                                  TScalar3 s3,
                                  TScalar4 s4,
                                  TScalar5 s5,
                                  TScalar6 s6,
                                  TScalar7 s7)
            where T: unmanaged, INumber<T>
            where TScalar0: unmanaged, INumber<TScalar0>
            where TScalar1: unmanaged, INumber<TScalar1>
            where TScalar2: unmanaged, INumber<TScalar2>
            where TScalar3: unmanaged, INumber<TScalar3>
            where TScalar4: unmanaged, INumber<TScalar4>
            where TScalar5: unmanaged, INumber<TScalar5>
            where TScalar6: unmanaged, INumber<TScalar6>
            where TScalar7: unmanaged, INumber<TScalar7>
        {
            MatrixParallelTaskPackage<T>* package = MatrixParallelFactory.CreateBaseTask<T>();
            package->Left = left.Source;
            package->Right = right.Source;
            package->Result = ans.Source;
            package->LeftPrimOffset = left.PrimOffset;
            package->RightPrimOffset = right.PrimOffset;
            package->ResultPrimOffset = ans.PrimOffset;
            package->LeftPrimStride = left.PrimStride;
            package->RightPrimStride = right.PrimStride;
            package->ResultPrimStride = ans.PrimStride;
            package->LeftSecOffset = left.SecondaryOffset;
            package->RightSecOffset = right.SecondaryOffset;
            package->ResultSecOffset = ans.SecondaryOffset;
            package->LeftSecStride = left.SecondaryStride;
            package->RightSecStride = right.SecondaryStride;
            package->ResultSecStride = ans.SecondaryStride;
            package->PrimCount = left.PrimSize;
            package->SecCount = left.SecondarySize;
            package->ComputeHandle = (nint)Operation;
            package->ElementSize = sizeof(T);
            package->ComputeMode = 4;
            ScalarPacket* sp = MatrixParallelFactory.CreateScalarPacket();

            sp->Data<TScalar0>(0) = s0;
            sp->Data<TScalar1>(1) = s1;
            sp->Data<TScalar2>(2) = s2;
            sp->Data<TScalar3>(3) = s3;
            sp->Data<TScalar4>(4) = s4;
            sp->Data<TScalar5>(5) = s5;
            sp->Data<TScalar6>(6) = s6;
            sp->Data<TScalar7>(7) = s7;
            package->Scalar = sp;
            PublicTaskPrivate<T>(package);
        }

        private static unsafe void PublicTaskPrivate<T>(MatrixParallelTaskPackage<T>* package)
            where T: unmanaged, INumber<T>
        {
            int primCount = package->PrimCount;
            int secCount = package->SecCount;
            int tileWidth = (1024 / secCount) + 1;
            int tileCount = (primCount / tileWidth) + 1;

            int tilePerThreadLess = tileCount / Wraps.Length;
            int tileToSlice = tileCount % Wraps.Length;

            // TODO 这个方法依然是独占，需要一个新的缓存类型用来辅助复用空线程束
            for (int i = 0; i < Wraps.Length; i++) { }
        }

    }
}
