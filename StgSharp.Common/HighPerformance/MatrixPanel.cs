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
using StgSharp.Internal;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance
{
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
            return PanelSideLength<T>();
        }

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
            return PanelSideFromBits<T>(256) / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountAvx512<T>() where T : unmanaged, INumber<T>
        {
            return PanelSideFromBits<T>(512) / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int KernelSideCountSse<T>() where T : unmanaged, INumber<T>
        {
            return PanelSideFromBits<T>(128) / 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int PanelSideFromBits<T>(int vectorBits) where T : unmanaged, INumber<T>
        {
            int elemBits = sizeof(T) * 8;

            // Q = max(4, vectorBits/64, vectorBits/elemBits)
            int by64 = vectorBits / 64;
            int byElem = vectorBits / elemBits;
            int side = Math.Max(4, Math.Max(by64, byElem));
            return side;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int PanelSideLength<T>() where T : unmanaged, INumber<T>
        {
            return NativeIntrinsic.IntrinsicLevel switch
            {
                IntrinsicLevel.SSE => PanelSideFromBits<T>(128),
                IntrinsicLevel.AVX2 => PanelSideFromBits<T>(256),
                IntrinsicLevel.AVX512 => PanelSideFromBits<T>(512),
                _ => 0
            };
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
