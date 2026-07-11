// -----------------------------------------------------------------------------
// file="ColumnSet2"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Mathematics.Internal
{
    [StructLayout(LayoutKind.Explicit, Size = 12 * sizeof(float), Pack = 16)]
    internal struct ColumnSet2 : IEquatable<ColumnSet2>
    {

        [FieldOffset(0 * sizeof(float))] internal float m00;
        [FieldOffset(4 * sizeof(float))] internal float m01;
        [FieldOffset(1 * sizeof(float))] internal float m10;
        [FieldOffset(5 * sizeof(float))] internal float m11;
        [FieldOffset(2 * sizeof(float))] internal float m20;
        [FieldOffset(6 * sizeof(float))] internal float m21;
        [FieldOffset(3 * sizeof(float))] internal float m30;
        [FieldOffset(7 * sizeof(float))] internal float m31;

        [FieldOffset(0)] internal Vector4 colum0;
        [FieldOffset(4 * sizeof(float))] internal Vector4 colum1;

        internal ColumnSet2(
                 Vector4 c0,
                 Vector4 c1
        )
        {
            Unsafe.SkipInit(out this);
            colum0 = c0;
            colum1 = c1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(
                             object? obj
        )
        {
            return (obj is ColumnSet2 mat) && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(
                    ColumnSet2 other
        )
        {
            return colum0.Equals(other.colum0) && colum1.Equals(other.colum1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
                                    ColumnSet2 left,
                                    ColumnSet2 right
        )
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
                                    ColumnSet2 left,
                                    ColumnSet2 right
        )
        {
            return left.Equals(right);
        }

    }
}
