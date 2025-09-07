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
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading;

namespace StgSharp.Mathematics
{
    public unsafe struct ScalarPacket
    {

        private fixed ulong data[8];

        public ref T Data<T>(int index) where T: unmanaged,INumber<T>
        {
            if (index is < 0 or >= 8) {
                throw new IndexOutOfRangeException();
            }

            return ref Unsafe.As<ulong, T>(ref data[index]);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MatrixParallelTaskPackage
    {

        #region source

        // Group 1: Left/Right matrix kernel base address
        public MatrixKernel* Left;    // 8
        public MatrixKernel* Right;   // 8

        #endregion

        #region ans

        // Group 2: Result kernel base address + reserved
        public MatrixKernel* Result;  // 8
        public void* ReservedPtr0;         // 8 (extension/reserved)

        #endregion

        #region left enum

        public int LeftPrimOffset;         // 4
        public int LeftPrimStride;         // 4 
        public int LeftSecOffset;          // 4
        public int LeftSecStride;          // 4

        #endregion

        #region right enum

        public int RightPrimOffset;        // 4
        public int RightPrimStride;        // 4
        public int RightSecOffset;         // 4
        public int RightSecStride;         // 4

        #endregion

        #region ans enum

        public int ResultPrimOffset;       // 4
        public int ResultPrimStride;       // 4
        public int ResultSecOffset;        // 4
        public int ResultSecStride;        // 4

        #endregion

        #region scalar/compute

        public ScalarPacket* Scalar;  // 8
        public IntPtr ComputeHandle;     // 8

        #endregion

        #region global profile

        public int PrimCount;            // 4
        public int ComputeMode;            // 4 
        public int SecCount;               // 4
        private readonly int ReservedInt0; // 4

        #endregion

        #region reserve

        public long ReservedLong0;         // 8
        public long ReservedLong1;         // 8

        #endregion
    }
}
