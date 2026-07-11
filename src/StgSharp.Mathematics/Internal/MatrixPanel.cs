// -----------------------------------------------------------------------------
// file="MatrixPanel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Internal;
using StgSharp.Mathematics.Numeric;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Numeric
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

        public static unsafe void Destroy<T>(
                                  MatrixPanel<T>* panel
        ) where T : unmanaged, INumber<T>
        {
            NativeMemory.AlignedFree(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<T> Elements<T>(
                                     ref T head
        ) where T : unmanaged, INumber<T>
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
            SIMDID id = NumericalModule.GlobalIntrinsicMask;
            switch (id.MaskByte[0] & 0b_00001111)
            {
                case 1:
                    return id.MaskByte[1] switch
                    {
                        1 => KernelSideCountSse<T>(),
                        2 => KernelSideCountAvx2<T>(),
                        3 => KernelSideCountAvx512<T>(),
                        _ => throw new NotSupportedException("Unsupported SIMD level."),
                    };
                default:
                    throw new NotSupportedException("Current hardware platform is not supported");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe nuint Align64(
                                    void* p
        )
        {
            return ((nuint)p + (AlignmentBytes - 1)) & ~(nuint)(AlignmentBytes - 1);
        }

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
        private static int PanelSideFromBits<T>(
                           int vectorBits
        ) where T : unmanaged, INumber<T>
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
            SIMDID id = NumericalModule.GlobalIntrinsicMask;
            switch (id.MaskByte[0] & 0b_00001111)
            {
                case 1:
                    return id.MaskByte[1] switch
                    {
                        1 => PanelSideFromBits<T>(128),
                        2 => PanelSideFromBits<T>(256),
                        3 => PanelSideFromBits<T>(512),
                        _ => throw new NotSupportedException("Unsupported SIMD level."),
                    };
                default:
                    throw new NotSupportedException("Current hardware platform is not supported");
            }
        }

    }

    // Single POD-style panel: ZMM-compatible size. Alignment handled by allocator, not by the struct.
    public unsafe struct MatrixPanel<T> where T : unmanaged, INumber<T>
    {

        internal T Head; // first element of the panel buffer 

        public ref T this[
                     int column,
                     int row
        ]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Head, row * MatrixPanel.ElementSideLength<T>() + column);
        }

        public Span<T> Elements => MatrixPanel.Elements(ref Head);

    }
}
