// -----------------------------------------------------------------------------
// file="M512"
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
    [StructLayout(LayoutKind.Explicit, Size = 64, Pack = 64)]
    public unsafe struct M512 : IRegisterPresentation
    {

        [FieldOffset(0)] private fixed byte _buffer[64];
        [FieldOffset(0)] private Vector512<byte> Vec;

        public M512()
        {
            Unsafe.SkipInit(out this);
            Unsafe.SkipInit(out Vec);
        }

        public ref T AsRef<T>() where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref _buffer[0]);
        }

        public void BroadCastFrom<T>(
                    T value
        ) where T : unmanaged, INumber<T>
        {
            Vec = Vector512.Create(value).AsByte();
        }

        public ref T Member<T>(
                     int index
        ) where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref _buffer[index]);
        }

    }
}
