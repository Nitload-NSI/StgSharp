<<<<<<< HEAD
<<<<<<< HEAD
//-----------------------------------------------------------------------
=======
﻿//-----------------------------------------------------------------------
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
//-----------------------------------------------------------------------
>>>>>>> stgsharp-dev/giga
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
<<<<<<< HEAD
<<<<<<< HEAD
using StgSharp.Internal;
=======
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
using StgSharp.Internal;
>>>>>>> stgsharp-dev/giga
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance
{
<<<<<<< HEAD
<<<<<<< HEAD
    public unsafe struct MatrixPanel
    {

        public const int AlignmentBytes = 64;

        private byte Head;

        // Allocate aligned memory for one panel; caller must free via Destroy.
        public static unsafe MatrixPanel<T>* Create<T>() where T : unmanaged, INumber<T>
        {
            return (MatrixPanel<T>*)NativeMemory.AlignedAlloc(Size<T>(), AlignmentBytes);
        }

        public static unsafe void Destroy<T>(MatrixPanel<T>* panel) where T : unmanaged, INumber<T>
        {
            NativeMemory.AlignedFree(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<T> Elements<T>(ref T head) where T : unmanaged, INumber<T>
        {
            void* p = Unsafe.AsPointer(ref head);
            return new Span<T>(p, MaxElementCount<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ElementSideLength<T>() where T : unmanaged, INumber<T>
        {
            return Math.Max(8, AlignmentBytes / sizeof(T));
        }

        // View an existing pointer; caller guarantees lifetime/alignment.
        public static unsafe MatrixPanel<T>* FromPointer<T>(void* ptr)
            where T : unmanaged, INumber<T>
        {
            return (MatrixPanel<T>*)Align64(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MaxElementCount<T>() where T : unmanaged, INumber<T>
        {
            int side = ElementSideLength<T>();
            return side * side;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint Size<T>() where T : unmanaged, INumber<T>
        {
            return (nuint)(MaxElementCount<T>() * sizeof(T));
        }

        /// <summary>
        ///   Number of 4x4 kernels that fit in a panel for the given ISA and element T.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int KernelCapacity<T>() where T : unmanaged, INumber<T>
        {
            int perSide = KernelSideCount<T>();
            return perSide * perSide;
        }

        /// <summary>
        ///   How many 4x4 kernels fit along a single panel dimension for the given ISA.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int KernelSideCount<T>() where T : unmanaged, INumber<T>
        {
            return NativeIntrinsic.IntrinsicLevel switch
            {
                IntrinsicLevel.SSE => KernelSideCountSse<T>(),
                IntrinsicLevel.AVX2 => KernelSideCountAvx2<T>(),
                IntrinsicLevel.AVX512 => KernelSideCountAvx512<T>(),
                _ => 0
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe nuint Align64(void* p) => ((nuint)p + (AlignmentBytes - 1)) & ~(nuint)(AlignmentBytes - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountAvx2<T>() where T : unmanaged, INumber<T>
        {
            const int minQ = 8;
            int lanes = 32 / sizeof(T);
            int side = Math.Max(lanes, minQ);
            return side / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountAvx512<T>() where T : unmanaged, INumber<T>
        {
            const int minQ = 8;
            int lanes = 64 / sizeof(T);
            int side = Math.Max(lanes, minQ);
            return side / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountSse<T>() where T : unmanaged, INumber<T>
        {
            const int minQ = 4;
            int lanes = 16 / sizeof(T);
            int side = Math.Max(lanes, minQ);
            return side / 4;
        }

    }

    // Single POD-style panel: ZMM-compatible size. Alignment handled by allocator, not by the struct.
    public unsafe struct MatrixPanel<T> where T : unmanaged, INumber<T>
    {

        internal T Head; // first element of the panel buffer 

        public ref T this[int column, int row]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Head, row * MatrixPanel.ElementSideLength<T>() + column);
        }

        public Span<T> Elements
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MatrixPanel.Elements(ref Head);
        }

    }
}

=======
    #pragma warning disable CA1000

    public interface IMatrixPanel<T> : IDisposable where T : unmanaged,INumber<T>
=======
    public unsafe struct MatrixPanel
>>>>>>> stgsharp-dev/giga
    {

        public const int AlignmentBytes = 64;

        private byte Head;

        // Allocate aligned memory for one panel; caller must free via Destroy.
        public static unsafe MatrixPanel<T>* Create<T>() where T : unmanaged, INumber<T>
        {
            return (MatrixPanel<T>*)NativeMemory.AlignedAlloc(Size<T>(), AlignmentBytes);
        }

        public static unsafe void Destroy<T>(MatrixPanel<T>* panel) where T : unmanaged, INumber<T>
        {
            NativeMemory.AlignedFree(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<T> Elements<T>(ref T head) where T : unmanaged, INumber<T>
        {
            void* p = Unsafe.AsPointer(ref head);
            return new Span<T>(p, MaxElementCount<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ElementSideLength<T>() where T : unmanaged, INumber<T>
        {
            return Math.Max(8, AlignmentBytes / sizeof(T));
        }

        // View an existing pointer; caller guarantees lifetime/alignment.
        public static unsafe MatrixPanel<T>* FromPointer<T>(void* ptr)
            where T : unmanaged, INumber<T>
        {
            return (MatrixPanel<T>*)Align64(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MaxElementCount<T>() where T : unmanaged, INumber<T>
        {
            int side = ElementSideLength<T>();
            return side * side;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint Size<T>() where T : unmanaged, INumber<T>
        {
            return (nuint)(MaxElementCount<T>() * sizeof(T));
        }

        /// <summary>
        ///   Number of 4x4 kernels that fit in a panel for the given ISA and element T.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int KernelCapacity<T>() where T : unmanaged, INumber<T>
        {
            int perSide = KernelSideCount<T>();
            return perSide * perSide;
        }

        /// <summary>
        ///   How many 4x4 kernels fit along a single panel dimension for the given ISA.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int KernelSideCount<T>() where T : unmanaged, INumber<T>
        {
            return NativeIntrinsic.IntrinsicLevel switch
            {
                IntrinsicLevel.SSE => KernelSideCountSse<T>(),
                IntrinsicLevel.AVX2 => KernelSideCountAvx2<T>(),
                IntrinsicLevel.AVX512 => KernelSideCountAvx512<T>(),
                _ => 0
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe nuint Align64(void* p) => ((nuint)p + (AlignmentBytes - 1)) & ~(nuint)(AlignmentBytes - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountAvx2<T>() where T : unmanaged, INumber<T>
        {
            const int minQ = 8;
            int lanes = 32 / sizeof(T);
            int side = Math.Max(lanes, minQ);
            return side / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountAvx512<T>() where T : unmanaged, INumber<T>
        {
            const int minQ = 8;
            int lanes = 64 / sizeof(T);
            int side = Math.Max(lanes, minQ);
            return side / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountSse<T>() where T : unmanaged, INumber<T>
        {
            const int minQ = 4;
            int lanes = 16 / sizeof(T);
            int side = Math.Max(lanes, minQ);
            return side / 4;
        }

    }

    // Single POD-style panel: ZMM-compatible size. Alignment handled by allocator, not by the struct.
    public unsafe struct MatrixPanel<T> where T : unmanaged, INumber<T>
    {

        internal T Head; // first element of the panel buffer 

        public ref T this[int column, int row]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Head, row * MatrixPanel.ElementSideLength<T>() + column);
        }

        public Span<T> Elements
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MatrixPanel.Elements(ref Head);
        }

    }
}

<<<<<<< HEAD
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
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
>>>>>>> stgsharp-dev/giga
