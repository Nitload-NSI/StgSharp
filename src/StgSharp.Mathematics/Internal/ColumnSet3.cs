// -----------------------------------------------------------------------------
// file="ColumnSet3"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Internal;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Internal
{
    [StructLayout(LayoutKind.Explicit, Size = 12 * sizeof(float), Pack = 16)]
    internal struct ColumnSet3 : IEquatable<ColumnSet3>
    {

        [FieldOffset(0 * sizeof(float))] internal float m00;
        [FieldOffset(4 * sizeof(float))] internal float m01;
        [FieldOffset(8 * sizeof(float))] internal float m02;
        [FieldOffset(1 * sizeof(float))] internal float m10;
        [FieldOffset(5 * sizeof(float))] internal float m11;
        [FieldOffset(9 * sizeof(float))] internal float m12;
        [FieldOffset(2 * sizeof(float))] internal float m20;
        [FieldOffset(6 * sizeof(float))] internal float m21;
        [FieldOffset(10 * sizeof(float))] internal float m22;
        [FieldOffset(3 * sizeof(float))] internal float m30;
        [FieldOffset(7 * sizeof(float))] internal float m31;
        [FieldOffset(11 * sizeof(float))] internal float m32;

        [FieldOffset(0)] internal Vector4 colum0;
        [FieldOffset(4 * sizeof(float))] internal Vector4 colum1;
        [FieldOffset(8 * sizeof(float))] internal Vector4 colum2;

        public override bool Equals(
                             object? obj
        )
        {
            return (obj is ColumnSet3 mat) && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(
                    ColumnSet3 other
        )
        {
            return colum0.Equals(other.colum0) &&
                   colum1.Equals(other.colum1) &&
                   colum2.Equals(other.colum2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
                                    ColumnSet3 left,
                                    ColumnSet3 right
        )
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
                                    ColumnSet3 left,
                                    ColumnSet3 right
        )
        {
            return left.Equals(right);
        }

    }
}
