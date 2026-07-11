// -----------------------------------------------------------------------------
// file="M256"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.ProcessorAbstraction
{
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public unsafe struct M256 : IRegisterPresentation
    {

        [FieldOffset(0)] internal fixed byte Buffer[32];
        [FieldOffset(0)] internal Vector256<float> Vec;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T AsRef<T>() where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref Buffer[0]);
        }

        public void BroadCastFrom<T>(
                    T value
        ) where T : unmanaged, INumber<T>
        {
            Vec = Vector256.Create(value).AsSingle();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Member<T>(
                     int index
        ) where T : unmanaged, INumber<T>
        {
            return ref Unsafe.Add(ref AsRef<T>(), index);
        }

    }
}
