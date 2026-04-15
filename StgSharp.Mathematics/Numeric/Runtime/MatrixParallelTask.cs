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

namespace StgSharp.Mathematics.Numeric.Runtime
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MatrixParallelSource
    {

        public MatrixParallelTask Task;
        public ulong Count;
        public ulong Cursor;

    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct MatrixParallelTask
    {

        [FieldOffset(0)] public MatrixKernel* Mat1;
        [FieldOffset(8)] public MatrixKernel* Mat2;
        [FieldOffset(16)] public MatrixKernel* Mat3;
        /// <summary>
        ///   Being offset of first kernel in a L4 cache line.
        /// </summary>
        [FieldOffset(24)] public int CacheIndex;
        /// <summary>
        ///   Count of kernels to be calculated in a L4 cache line.
        /// </summary>
        [FieldOffset(28)] public int CacheLength;
        [FieldOffset(32)] public int CommonK;
        [FieldOffset(36)] public int CommonX;
        [FieldOffset(40)] public int CommonY;
        [FieldOffset(44)] public int SpanLength;
        [FieldOffset(48)] public MatrixElementType ElementType;
        [FieldOffset(52)] public MatrixIntrinsicHandle ComputeHandle;
        [FieldOffset(56)] public nint Span;
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
