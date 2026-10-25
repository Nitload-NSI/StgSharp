// -----------------------------------------------------------------------------
// file="M128"
// Project: ['.] nFramework
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

namespace Nitload.Common.HighPerformance.ProcessorAbstraction
{
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public unsafe struct M128 : IRegisterPresentation
    {

        [FieldOffset(0)] internal fixed byte Buffer[16];
        [FieldOffset(0)] internal Vector128<byte> Vec;

        public ref T AsRef<T>() where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref Buffer[0]);
        }

        public void BroadCastFrom<T>(
                    T value
        ) where T : unmanaged, INumber<T>
        {
            Vec = Vector128.Create<T>(value).AsByte();
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
