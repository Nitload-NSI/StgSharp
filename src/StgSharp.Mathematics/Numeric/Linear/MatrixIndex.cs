// -----------------------------------------------------------------------------
// file="MatrixIndex"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Numeric
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct MatrixIndex
    {

        public readonly int Begin;
        public readonly int End;

        private MatrixIndex(
                int begin,
                int end,
                bool safe = false
        )
        {
            Begin = begin;
            End = end;
        }

        internal MatrixIndex(
                 int begin,
                 int end
        )
        {
            if (begin > end) {
                throw new ArgumentException("Begin index cannot be greater than end index.");
            }

            Begin = begin;
            End = end;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndex(
                                        (int, int) range
        )
        {
            return new(range.Item1, range.Item2, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndex(
                                        int index
        )
        {
            return new(index, -1, false);
        }

    }
}
