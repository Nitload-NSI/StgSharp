//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixPanel"
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

namespace StgSharp.HighPerformance
{
    public interface IMatrixPanel<T> : IDisposable where T: unmanaged,INumber<T>
    {

        static abstract int MaxElementCount { get; }

        static abstract nuint Size { get; }

        void Dispose();

    }

    /// <summary>
    ///   Buffer for SSE targeted matrix panel, size of a panel is 4x4 elements, and no smaller than
    ///   16 bytes.
    /// </summary>
    /// <typeparam name="T">
    ///   Type of matrix element.
    /// </typeparam>
    [InlineArray(16)]
    public unsafe struct MatrixPanelSSE<T> : IMatrixPanel<T> where T: unmanaged, INumber<T>
    {

        private M128 vector;

        [Obsolete("Compute kernel should not be created from new(), use Create instead", true)]
        public MatrixPanelSSE() { }

        public static int MaxElementCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int size = sizeof(T);
                return 16 * 16 / size;
            }
        }

        public static unsafe nuint Size { get; } = 16 * 16;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixPanelSSE<T>* Create()
        {
            MatrixPanelSSE<T>* kernel = (MatrixPanelSSE<T>*)NativeMemory.AlignedAlloc(Size, 16);
            return kernel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(MatrixPanelSSE<T>* kernel)
        {
            NativeMemory.AlignedFree(kernel);
        }

        public void Dispose()
        {
            fixed (MatrixPanelSSE<T>* thisPtr = &this) {
                NativeMemory.AlignedFree(thisPtr);
            }
        }

    }

    /// <summary>
    ///   Buffer for AVX targeted matrix panel, size of a panel is 4x4 elements, and no smaller than
    ///   16 bytes.
    /// </summary>
    /// <typeparam name="T">
    ///   Type of matrix element.
    /// </typeparam>
    [InlineArray(16)]
    public unsafe struct MatrixPanelAVX<T> : IMatrixPanel<T> where T: unmanaged, INumber<T>
    {

        private M256 _buffer;

        [Obsolete("Compute kernel should not be created from new(), use Create instead", true)]
        public MatrixPanelAVX() { }

        public static unsafe int MaxElementCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int size = sizeof(T);
                return size < 4 ? 128 / size : 16;
            }
        }

        public static unsafe nuint Size { get; } = 32 * 16;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixPanelAVX<T>* Create()
        {
            MatrixPanelAVX<T>* kernel = (MatrixPanelAVX<T>*)NativeMemory.AlignedAlloc(Size, 16);
            return kernel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(MatrixPanelAVX<T>* kernel)
        {
            NativeMemory.AlignedFree(kernel);
        }

        public void Dispose()
        {
            fixed (MatrixPanelAVX<T>* thisPtr = &this) {
                NativeMemory.AlignedFree(thisPtr);
            }
        }

    }

    /// <summary>
    ///   Buffer for AVX512 targeted matrix panel, size of a panel is 4x4 elements, and no smaller
    ///   than 16 bytes.
    /// </summary>
    /// <typeparam name="T">
    ///   Type of matrix element.
    /// </typeparam>
    [InlineArray(16)]
    public unsafe struct MatrixComputeKernelAVX512<T> : IMatrixPanel<T> where T: unmanaged, INumber<T>
    {

        private M512 _buffer;

        [Obsolete("Compute kernel should not be created from new(), use Create instead", true)]
        public MatrixComputeKernelAVX512() { }

        public static unsafe int MaxElementCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int size = sizeof(T);
                return size < 4 ? 128 / size : 16;
            }
        }

        public static unsafe nuint Size { get; } = 32 * 16;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixComputeKernelAVX512<T>* Create()
        {
            MatrixComputeKernelAVX512<T>* kernel = (MatrixComputeKernelAVX512<T>*)NativeMemory.AlignedAlloc(Size, 16);
            return kernel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(MatrixComputeKernelAVX512<T>* kernel)
        {
            NativeMemory.AlignedFree(kernel);
        }

        public void Dispose()
        {
            fixed (MatrixComputeKernelAVX512<T>* thisPtr = &this) {
                NativeMemory.AlignedFree(thisPtr);
            }
        }

    }
}