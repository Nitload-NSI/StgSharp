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
using StgSharp.HighPerformance;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Numeric
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
    internal readonly unsafe struct MatrixParallelTaskPackageNonGeneric
    {

        #region source

        // Group 1: Left/Right matrix kernel base address
        public readonly IntPtr Left;    // 8
        public readonly IntPtr Right;   // 8

        #endregion

        #region ans

        // Group 2: Result kernel base address + reserved
        public readonly IntPtr Result;  // 8
        public readonly int ElementSize;
        public readonly int ReservedPtr0;         // 4 (extension/reserved)

        #endregion

        #region left enum

        public readonly int LeftPrimOffset;         // 4
        public readonly int LeftPrimStride;         // 4 
        public readonly int LeftSecOffset;          // 4
        public readonly int LeftSecStride;          // 4

        #endregion

        #region right enum

        public readonly int RightPrimOffset;        // 4
        public readonly int RightPrimStride;        // 4
        public readonly int RightSecOffset;         // 4
        public readonly int RightSecStride;         // 4

        #endregion

        #region ans enum

        public readonly int ResultPrimOffset;       // 4
        public readonly int ResultPrimStride;       // 4
        public readonly int ResultSecOffset;        // 4
        public readonly int ResultSecStride;        // 4

        #endregion

        #region scalar/compute

        public readonly ScalarPacket* Scalar;  // 8
        public readonly IntPtr ComputeHandle;     // 8

        #endregion

        #region global profile

        public readonly int PrimCount;            // 4
        public readonly int ComputeMode;            // 4 
        public readonly int SecCount;               // 4
        private readonly int ReservedInt0; // 4

        #endregion

        #region count offset

        public readonly int PrimTileOffset;          // 4
        public readonly int SecTileOffset;           // 4
        public readonly int PrimCountInTile;          // 4
        public readonly int SecCountInTile;           // 4

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MatrixParallelTaskPackage<T> where T: unmanaged, INumber<T>
    {

        public MatrixParallelTaskPackage()
        {
            Unsafe.SkipInit(out this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy(MatrixParallelTaskPackage<T>* source, MatrixParallelTaskPackage<T>* target)
        {
            Buffer.MemoryCopy(
                source,
                target,
                sizeof(MatrixParallelTaskPackage<T>),
                sizeof(MatrixParallelTaskPackage<T>));
        }

        #region source

        // Group 1: Left/Right matrix kernel base address
        public MatrixKernel<T>* Left;    // 8
        public MatrixKernel<T>* Right;   // 8

        #endregion

        #region ans

        // Group 2: Result kernel base address + reserved
        public MatrixKernel<T>* Result;  // 8
        public int ElementSize = sizeof(T);
        public int ReservedPtr0;         // 4 (extension/reserved)

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

        #region count offset

        public int PrimTileOffset;          // 4
        public int SecTileOffset;           // 4
        public int PrimColumnCountInTile;          // 4
        public int SecColumnCountInTile;           // 4

    #endregion
    }
}