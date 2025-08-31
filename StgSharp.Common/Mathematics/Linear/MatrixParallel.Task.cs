//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Task.cs"
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
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics
{
    /// <summary>
    ///   Matrix parallel computation task structure with scalar data packet support Uses type-
    ///   erased design for thread pool compatibility
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public unsafe struct MatrixParallelTask
    {

        public const int SizeofConstantParameter = sizeof(ulong);
        public const int SizeOfTask = 48;

        [FieldOffset(0)] public MatrixKernel* left;
        [FieldOffset(8)] public MatrixKernel* result;
        [FieldOffset(16)] public MatrixKernel* right;
        [FieldOffset(24)] public int scalerCount;
        [FieldOffset(32)] public IntPtr executionHandle;

        /// <summary>
        ///   Pointer to the scalar data packet containing up to 8 parameters Each parameter is
        ///   stored as ulong (8 bytes) for type erasure
        /// </summary>
        [FieldOffset(40)] public nint scalerDataPacket;

        /// <summary>
        ///   Execute the task using type-erased function pointer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Execute()
        {
            if (scalerCount == 0)
            {
                ((delegate*<MatrixKernel*, MatrixKernel*, MatrixKernel*, void>)executionHandle)(left, right, result);
            } else
            {
                ((delegate*<MatrixKernel*, MatrixKernel*, MatrixKernel*, void*, void>)executionHandle)(left, right, result, (void*)scalerDataPacket);
            }
        }

    }

    /// <summary>
    ///   Scalar data packet for storing up to 8 parameters as type-erased ulong values Used to
    ///   separate scalar data allocation from task allocation
    /// </summary>
    public unsafe struct ScalarDataPacket
    {

        public const int MaxScalarCount = 8;
        public const int PacketSize = 64;

        private fixed ulong Value[8];

        public ScalarDataPacket()
        {
            Unsafe.SkipInit(out this);
        }

        public ref T GetScalerRef<T>(int index) where T: unmanaged, INumber<T>
        {
            if (index is < 0 or >= MaxScalarCount) {
                throw new IndexOutOfRangeException("ScalarDataPacket index out of range");
            }
            return ref Unsafe.As<ulong, T>(ref Value[index]);
        }

    }
}
