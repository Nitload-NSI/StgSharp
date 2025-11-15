//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelTaskPackage"
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
    /*
     * #define PANEL_OP 1
     * #define BUFFER_OP 2
     * #define KERNEL_OP 3
     * 
     * #define RIGHT_ANS_PARAM 1
     * #define LEFT_RIGHT_ANS_PARAM 2
     * #define RIGHT_ANS_SCALAR_PARAM 3
     * #define LEFT_RIGHT_ANS_SCALAR_PARAM 4
     * #define ANS_SCALAR_PARAM 5
     * 
     * typedef struct matrix_op_mode {
     *         int16_t operation_style; // PANEL_OP, BUFFER_OP, KERNEL_OP
     *     int16_t parameter_style; // RIGHT_ANS_PARAM, LEFT_RIGHT_ANS_PARAM, e.g.
     * }
     * matrix_op_mode;
     */

    public enum MatrixOperationStyle : short
    {

        PANEL_OP = 1,
        BUFFER_OP = 2,
        KERNEL_OP = 3,

    }

    public enum MatrixOperationParam : short
    {

        RIGHT_ANS_PARAM = 1,
        LEFT_RIGHT_ANS_PARAM = 2,
        RIGHT_ANS_SCALAR_PARAM = 3,
        LEFT_RIGHT_ANS_SCALAR_PARAM = 4,
        ANS_SCALAR_PARAM = 5,

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MatrixOpMode
    {

        public MatrixOperationParam ParameterStyle;
        public MatrixOperationStyle OperationStyle;

    }

    [StructLayout(LayoutKind.Explicit, Size = 128)]
    internal unsafe struct MatrixParallelTaskPackageNonGeneric
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy(
                                  MatrixParallelTaskPackageNonGeneric* source,
                                  MatrixParallelTaskPackageNonGeneric* target)
        {
            Buffer.MemoryCopy(source, target, sizeof(MatrixParallelTaskPackageNonGeneric),
                              sizeof(MatrixParallelTaskPackageNonGeneric));
        }

        #region source

        // Group 1: Left/Right matrix kernel base address
        [FieldOffset(0)]public IntPtr Left;    // 8
        [FieldOffset(8)] public IntPtr Right;   // 8

        #endregion

        #region ans

        // Group 2: Result kernel base address + reserved
        [FieldOffset(16)] public IntPtr Result;  // 8
        [FieldOffset(24)]public int ElementSize;
        [FieldOffset(28)] public int ReservedPtr0;         // 4 (extension/reserved)

        #endregion

        #region left enum

        [FieldOffset(32)] public int LeftPrimOffset;         // 4
        [FieldOffset(36)]public int LeftPrimStride;         // 4 
        [FieldOffset(40)]public int LeftSecOffset;          // 4
        [FieldOffset(44)]public int LeftSecStride;          // 4

        #endregion

        #region right enum

        [FieldOffset(32)] public int RightPrimOffset;        // 4
        [FieldOffset(36)]public int RightPrimStride;        // 4
        [FieldOffset(40)]public int RightSecOffset;         // 4
        [FieldOffset(44)]public int RightSecStride;         // 4

        #endregion

        #region ans enum

        [FieldOffset(48)]public int ResultPrimOffset;       // 4
        [FieldOffset(52)]public int ResultPrimStride;       // 4
        [FieldOffset(56)]public int ResultSecOffset;        // 4
        [FieldOffset(60)] public int ResultSecStride;        // 4

        #endregion

        #region scalar/compute

        [FieldOffset(80)]public ScalarPacket* Scalar;  // 8
        [FieldOffset(88)]public IntPtr ComputeHandle;     // 8

        #endregion

        #region global profile

        [FieldOffset(96)]public  int PrimCount;            // 4
        [FieldOffset(100)]public MatrixOpMode ComputeMode;            // 4 
        [FieldOffset(104)]public  int SecCount;               // 4
        [FieldOffset(108)]private int ReservedInt0; // 4

        #endregion

        #region count offset

        [FieldOffset(112)]public int PrimTileOffset;          // 4
        [FieldOffset(116)]public int SecTileOffset;           // 4
        [FieldOffset(120)]public int PrimCountInTile;          // 4
        [FieldOffset(124)]public int SecCountInTile;           // 4

    #endregion
    }

    [StructLayout(LayoutKind.Explicit, Size =128)]
    internal unsafe struct MatrixParallelTaskPackage<T> where T : unmanaged, INumber<T>
    {

        public MatrixParallelTaskPackage()
        {
            Unsafe.SkipInit(out this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy(MatrixParallelTaskPackage<T>* source, MatrixParallelTaskPackage<T>* target)
        {
            Buffer.MemoryCopy(source, target, sizeof(MatrixParallelTaskPackage<T>),
                              sizeof(MatrixParallelTaskPackage<T>));
        }

        #region source

        // Group 1: Left/Right matrix kernel base address
        [FieldOffset(0)]public MatrixKernel<T>* Left;    // 8
        [FieldOffset(8)] public MatrixKernel<T>* Right;   // 8

        #endregion

        #region ans

        // Group 2: Result kernel base address + reserved
        [FieldOffset(16)] public MatrixKernel<T>* Result;  // 8
        [FieldOffset(24)] public int ElementSize = sizeof(T);
        [FieldOffset(28)] public int ReservedPtr0;         // 4 (extension/reserved)

        #endregion

        #region left enum

        [FieldOffset(32)]public int LeftPrimOffset;         // 4
        [FieldOffset(36)]public int LeftPrimStride;         // 4 
        [FieldOffset(40)]public int LeftSecOffset;          // 4
        [FieldOffset(44)] public int LeftSecStride;          // 4

        #endregion

        #region right enum

        [FieldOffset(48)]public int RightPrimOffset;        // 4
        [FieldOffset(52)]public int RightPrimStride;        // 4
        [FieldOffset(56)]public int RightSecOffset;         // 4
        [FieldOffset(60)]public int RightSecStride;         // 4

        #endregion

        #region ans enum

        [FieldOffset(64)]public int ResultPrimOffset;       // 4
        [FieldOffset(68)]public int ResultPrimStride;       // 4
        [FieldOffset(72)]public int ResultSecOffset;        // 4
        [FieldOffset(76)] public int ResultSecStride;        // 4

        #endregion

        #region scalar/compute

        [FieldOffset(80)] public ScalarPacket* Scalar;  // 8
        [FieldOffset(88)]public IntPtr ComputeHandle;     // 8

        #endregion

        #region global profile

        [FieldOffset(96)] public int PrimCount;            // 4
        [FieldOffset(100)]public MatrixOpMode ComputeMode;            // 4 
        [FieldOffset(104)]public int SecCount;               // 4
        [FieldOffset(108)] private readonly int ReservedInt0; // 4

        #endregion

        #region count offset

        [FieldOffset(112)]public int PrimTileOffset;          // 4
        [FieldOffset(116)]public int SecTileOffset;           // 4
        [FieldOffset(120)]public int PrimColumnCountInTile;          // 4
        [FieldOffset(124)]public int SecColumnCountInTile;           // 4

    #endregion
    }
}