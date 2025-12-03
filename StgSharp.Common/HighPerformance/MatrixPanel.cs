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
    #pragma warning disable CA1000

    public interface IMatrixPanel<T> : IDisposable where T : unmanaged,INumber<T>
    {

        ref T this[int column, int row] { get; }

        static abstract int MaxElementCount { get; }

        static abstract int ElementSideLength { get; }

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
    public unsafe struct MatrixPanelSSE<T> : IMatrixPanel<T> where T : unmanaged, INumber<T>
    {

        private T element;

        [Obsolete("Compute kernel should not be created from new(), use Create instead", true)]
        public MatrixPanelSSE() { }

        public ref T this[int column, int row]
        {
            get
            {
                int index = row * ElementSideLength + column;
                return ref Unsafe.Add(ref element, index);
            }
        }

        public static int ElementSideLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = int.Max(4, 16 / sizeof(T));

        public static int MaxElementCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = ElementSideLength * ElementSideLength;

        public static unsafe nuint Size { get; } = (nuint)(MaxElementCount * sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ChunkCount() => (ElementSideLength + LanesPerVector() - 1) / LanesPerVector();

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref M128 GetVectorChunk(int column, int chunk)
        {
            int lanes = LanesPerVector();
            int rowStart = chunk * lanes;
            ref T colBase = ref this[column, 0];
            return ref Unsafe.As<T, M128>(ref Unsafe.Add(ref colBase, rowStart));
        }

        // SIMD access helpers (column-major)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LanesPerVector() => 16 / sizeof(T);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVectorChunk(int column, int chunk, in M128 value)
        {
            GetVectorChunk(column, chunk) = value;
        }

    }

    /// <summary>
    ///   Buffer for AVX targeted matrix panel, size of a panel is 4x4 elements, and no smaller than
    ///   16 bytes.
    /// </summary>
    /// <typeparam name="T">
    ///   Type of matrix element.
    /// </typeparam>
    public unsafe struct MatrixPanelAVX<T> : IMatrixPanel<T> where T : unmanaged, INumber<T>
    {

        private T element;

        [Obsolete("Compute kernel should not be created from new(), use Create instead", true)]
        public MatrixPanelAVX() { }

        public ref T this[int column, int row]
        {
            get
            {
                int index = row * ElementSideLength + column;
                return ref Unsafe.Add(ref element, index);
            }
        }

        public static int ElementSideLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = int.Max(8, 32 / sizeof(T));

        public static unsafe int MaxElementCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = ElementSideLength * ElementSideLength;

        public static unsafe nuint Size { get; } = (nuint)(MaxElementCount * sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ChunkCount() => (ElementSideLength + LanesPerVector() - 1) / LanesPerVector();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixPanelAVX<T>* Create()
        {
            MatrixPanelAVX<T>* kernel = (MatrixPanelAVX<T>*)NativeMemory.AlignedAlloc(Size, 32);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref M256 GetVectorChunk(int column, int chunk)
        {
            int lanes = LanesPerVector();
            int rowStart = chunk * lanes;
            ref T colBase = ref this[column, 0];
            return ref Unsafe.As<T, M256>(ref Unsafe.Add(ref colBase, rowStart));
        }

        // SIMD access helpers for AVX (column-major)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LanesPerVector() => 32 / sizeof(T);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVectorChunk(int column, int chunk, in M256 value)
        {
            GetVectorChunk(column, chunk) = value;
        }

    }

    /// <summary>
    ///   Buffer for AVX512 targeted matrix panel, size of a panel is 4x4 elements, and no smaller
    ///   than 16 bytes.
    /// </summary>
    /// <typeparam name="T">
    ///   Type of matrix element.
    /// </typeparam>
    public unsafe struct MatrixComputeKernelAVX512<T> : IMatrixPanel<T>
        where T : unmanaged, INumber<T>
    {

        private T element;

        [Obsolete("Compute kernel should not be created from new(), use Create instead", true)]
        public MatrixComputeKernelAVX512() { }

        public ref T this[int column, int row]
        {
            get
            {
                int index = row * ElementSideLength + column;
                return ref Unsafe.Add(ref element, index);
            }
        }

        public static int ElementSideLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = int.Max(8, 64 / sizeof(T));

        public static unsafe int MaxElementCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = ElementSideLength * ElementSideLength;

        public static unsafe nuint Size { get; } = (nuint)(MaxElementCount * sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ChunkCount() => (ElementSideLength + LanesPerVector() - 1) / LanesPerVector();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixComputeKernelAVX512<T>* Create()
        {
            MatrixComputeKernelAVX512<T>* kernel = (MatrixComputeKernelAVX512<T>*)NativeMemory.AlignedAlloc(Size, 64);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref M512 GetVectorChunk(int column, int chunk)
        {
            int lanes = LanesPerVector();
            int rowStart = chunk * lanes;
            ref T colBase = ref this[column, 0];
            return ref Unsafe.As<T, M512>(ref Unsafe.Add(ref colBase, rowStart));
        }

        // SIMD access helpers for AVX-512 (column-major)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LanesPerVector() => 64 / sizeof(T);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVectorChunk(int column, int chunk, in M512 value)
        {
            GetVectorChunk(column, chunk) = value;
        }
#pragma warning restore CA1000
    }
}