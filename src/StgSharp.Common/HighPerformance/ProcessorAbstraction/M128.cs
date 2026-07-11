// -----------------------------------------------------------------------------
// file="M128"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using System.Text;

namespace StgSharp.HighPerformance.ProcessorAbstraction
{
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public unsafe struct M128 : IRegisterPresentation
    {

        [FieldOffset(0)] internal fixed byte Buffer[16];
        [FieldOffset(0)] internal Vector128<float> Vec;

        internal M128(
                 Vector4 v
        )
        {
            Unsafe.SkipInit(out this);
            Unsafe.SkipInit(out Vec);
            Vec = v.AsVector128();
        }

        internal M128(
                 Vector128<float> v
        )
        {
            Unsafe.SkipInit(out this);
            Vec = v;
        }

        public ref T AsRef<T>() where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref Buffer[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Vector4 AsVector4()
        {
            return Vec.AsVector4();
        }

        public void BroadCastFrom<T>(
                    T value
        ) where T : unmanaged, INumber<T>
        {
            Vec = Vector128.Create<T>(value).AsSingle();
        }

        public override readonly bool Equals(
                                      [NotNullWhen(true)] object obj
        )
        {
            return obj is M128 other && this == other;
        }

        public override readonly int GetHashCode()
        {
            return Vec.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Member<T>(
                     int index
        ) where T : unmanaged, INumber<T>
        {
            return ref Unsafe.Add(ref AsRef<T>(), index);
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public string ToString(
                      int sample
        )
        {
            string str = string.Empty;
            for (int i = 15; i >= 0; i--) {
                str += $"{Convert.ToString(Buffer[i], sample)} ";
            }
            return str;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
                                    M128 left,
                                    M128 right
        )
        {
            return left.Vec != right.Vec;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
                                    M128 left,
                                    M128 right
        )
        {
            return left.Vec == right.Vec;
        }

    }
}
