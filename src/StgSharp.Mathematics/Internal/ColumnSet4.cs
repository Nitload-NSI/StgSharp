// -----------------------------------------------------------------------------
// file="ColumnSet4"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp;
using StgSharp.Mathematics.Internal;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Internal
{
    [StructLayout(LayoutKind.Explicit, Size = 16 * sizeof(float))]
    internal struct ColumnSet4 : IEquatable<ColumnSet4>
    {

        [FieldOffset(0)]  internal Vector4 colum0;
        [FieldOffset(16)] internal Vector4 colum1;
        [FieldOffset(32)] internal Vector4 colum2;
        [FieldOffset(48)] internal Vector4 colum3;

        [FieldOffset(0)] public float m00;
        [FieldOffset(16)] public float m01;
        [FieldOffset(32)] public float m02;
        [FieldOffset(48)] public float m03;
        [FieldOffset(4)] public float m10;
        [FieldOffset(20)] public float m11;
        [FieldOffset(36)] public float m12;
        [FieldOffset(52)] public float m13;
        [FieldOffset(8)] public float m20;
        [FieldOffset(24)] public float m21;
        [FieldOffset(40)] public float m22;
        [FieldOffset(56)] public float m23;
        [FieldOffset(12)] public float m30;
        [FieldOffset(28)] public float m31;
        [FieldOffset(44)] public float m32;
        [FieldOffset(60)] public float m33;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ColumnSet4(
                 Vector4 colum0,
                 Vector4 colum1,
                 Vector4 colum2,
                 Vector4 colum3
        )
        {
            Unsafe.SkipInit(out this);
            this.colum0 = colum0;
            this.colum1 = colum1;
            this.colum2 = colum2;
            this.colum3 = colum3;
        }

        public override bool Equals(
                             object? obj
        )
        {
            return (obj is ColumnSet4 mat) && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(
                    ColumnSet4 other
        )
        {
            return colum0.Equals(other.colum0) &&
                   colum1.Equals(other.colum1) &&
                   colum2.Equals(other.colum2) &&
                   colum3.Equals(other.colum3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
                                    ColumnSet4 left,
                                    ColumnSet4 right
        )
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
                                    ColumnSet4 left,
                                    ColumnSet4 right
        )
        {
            return left.Equals(right);
        }

    }
}
