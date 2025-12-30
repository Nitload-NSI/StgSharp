//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixComputeModel"
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
using System.Runtime.CompilerServices;

namespace StgSharp.Mathematics.Numeric
{
    internal static class MatrixComputeModel
    {

        internal static unsafe void BufferComputeBinary(
                                    MatrixParallelTaskPackage* p
        )
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;

            ((delegate* unmanaged[Cdecl]<void*, void*, void*, long, void>)p->ComputeHandle)((void*)(p->Left + offset),
                                                                                            (void*)(p->Right + offset),
                                                                                            (void*)(p->Answer + offset),
                                                                                            count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void BufferComputeBinaryScalar(
                                    MatrixParallelTaskPackage* p
        )
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;
            ScalarPacket* scalar = p->Scalar;
            ((delegate* unmanaged[Cdecl]<void*, void*, void*, ScalarPacket*, long, void>)p->ComputeHandle)((void*)(p->Left + offset),
                                                                                                           (void*)(p->Right + offset),
                                                                                                           (void*)(p->Answer + offset),
                                                                                                           scalar,
                                                                                                           count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void BufferComputeNoOperatorScalar(
                                    MatrixParallelTaskPackage* p
        )
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;
            ((delegate* unmanaged[Cdecl]<void*, ScalarPacket*, long, void>)p->ComputeHandle)((void*)(p->Answer + offset),
                                                                                              p->Scalar, count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void BufferComputeUnary(
                                    MatrixParallelTaskPackage* p
        )
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;

            ((delegate* unmanaged[Cdecl]<void*, void*, long, void>)p->ComputeHandle)((void*)(p->Right + offset),
                                                                                      (void*)(p->Answer + offset),
                                                                                      count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void BufferComputeUnaryScalar(
                                    MatrixParallelTaskPackage* p
        )
        {
            int size = p->ElementSize * 16;
            long count = Unsafe.As<int, long>(ref p->PrimCount);
            long offset = Unsafe.As<int, long>(ref p->PrimTileOffset) * size;
            ((delegate* unmanaged[Cdecl]<void*, void*, ScalarPacket*, long, void>)p->ComputeHandle)((void*)(p->Right + offset),
                                                                                                     (void*)(p->Answer + offset),
                                                                                                     p->Scalar,
                                                                                                     count);
            MatrixParallelFactory.Release(p);
        }

        internal static unsafe void SpecialBufferCompute(
                                    MatrixParallelTaskPackage* p
        )
        {
            switch ((MatrixOperationSpecial)p->ComputeMode.ParameterStyle)
            {
                case MatrixOperationSpecial.SEQ_FMA:
                    switch ((MatrixElementType)p->ElementSize)
                    {
                        case MatrixElementType.F64:
                            MatrixCompute.ComputeFmaPanelF64(p);
                            return;
                        case MatrixElementType.F32:
                            MatrixCompute.ComputeFmaPanelF32(p);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            MatrixParallelFactory.Release(p);
        }

    }
}
