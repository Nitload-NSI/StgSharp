// -----------------------------------------------------------------------------
// file="M64"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace StgSharp.HighPerformance.ProcessorAbstraction
{
    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public unsafe struct M64 : IRegisterPresentation
    {

        [FieldOffset(0)] private fixed byte buffer[8];
        [FieldOffset(0)] private ulong value;
        [FieldOffset(0)] private Vector64<float> vec;

        public M64()
        {
            Unsafe.SkipInit(out this);
            Unsafe.SkipInit(out vec);
            Unsafe.SkipInit(out value);
        }

        public ref T AsRef<T>() where T : unmanaged,INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref buffer[0]);
        }

        public void BroadCastFrom<T>(
                    T value
        ) where T : unmanaged, INumber<T>
        {
            vec = Vector64.Create(value).AsSingle();
        }

        public override bool Equals(
                             object obj
        )
        {
            return obj is M64 m64 && this == m64;
        }

        public override readonly int GetHashCode()
        {
            return value.GetHashCode();
        }

        public ulong GetLowerBits(
                     int bitCount
        )
        {
            return value & ((1UL << bitCount) - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Member<T>(
                     int index
        ) where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref buffer[index * sizeof(T)]);
        }

        public void SetLowerBits(
                    int bitCount,
                    ulong bits
        )
        {
            value = (value & (~(1UL << bitCount))) | (bits & (1UL << bitCount));
        }

        public static bool operator !=(
                                    M64 left,
                                    M64 right
        )
        {
            return !(left == right);
        }

        public static M64 operator <<(
                                   M64 value,
                                   int shift
        )
        {
            M64 result = new M64();
            result.value = value.value << shift;
            return result;
        }

        public static bool operator ==(
                                    M64 left,
                                    M64 right
        )
        {
            return left.value == right.value;
        }
        public static M64 operator >>(
                                   M64 value,
                                   int shift
        )
        {
            return new()
            {
                value = value.value << shift
            };
        }

    }
}
