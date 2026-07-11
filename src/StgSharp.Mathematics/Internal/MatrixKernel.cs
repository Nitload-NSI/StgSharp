// -----------------------------------------------------------------------------
// file="MatrixKernel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics
{
    public unsafe struct MatrixKernel
    {

        private byte Head;

    }

    [StructLayout(LayoutKind.Sequential)]
    [InlineArray(16)]
    public unsafe struct MatrixKernel<T> where T : unmanaged, INumber<T>
    {

        public static readonly int Size = 16 * sizeof(T);

        private T c0r0;

        public MatrixKernel()
        {
            Unsafe.SkipInit(out this);
        }

        public MatrixKernel(
               bool clear
        )
        {
            if (!clear) {
                Unsafe.SkipInit(out this);
            }
        }

        public ref T this[
                     int x,
                     int y
        ]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref c0r0, (x * 4) + y);
        }

    }
}
