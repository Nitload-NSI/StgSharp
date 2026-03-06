//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelTask"
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
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Internal
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct MatrixParallelTask
    {

        [FieldOffset(0)]public MatrixKernel* Mat1;    // 8
        [FieldOffset(8)] public MatrixKernel* Mat2;    // 8
        [FieldOffset(16)] public MatrixKernel* Mat3;    // 16
        [FieldOffset(40)] public int Common_k;
        [FieldOffset(32)] public int CommonX;
        [FieldOffset(36)] public int CommonY;
        [FieldOffset(44)] public int i;
        [FieldOffset(48)] public int j;
        [FieldOffset(52)] public int k;
        [FieldOffset(56)] public MatrixElementType ElementType;    // 56
        [FieldOffset(24)] public nint Span;    // 24
        [FieldOffset(64)] public ScalarPacket Scalar;

        public MatrixParallelTask()
        {
            Unsafe.SkipInit(out this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(
                           MatrixParallelTask* source,
                           MatrixParallelTask* target
        )
        {
            Buffer.MemoryCopy(source, target, sizeof(MatrixParallelTask),
                              sizeof(MatrixParallelTask));
        }

    }
}