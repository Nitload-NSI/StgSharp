// -----------------------------------------------------------------------------
// file="ScalarPacket"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    public unsafe struct ScalarPacket
    {

        private fixed ulong data[8];

        public void BroadCast<TScalar, TRegister>(
                    int slotIndex,
                    TScalar value
        ) where TScalar : unmanaged, INumber<TScalar>
            where TRegister : unmanaged, IRegisterPresentation
        {
            if (slotIndex is < 0 or >= 8) {
                throw new IndexOutOfRangeException();
            }
            ref TRegister register = ref Unsafe.As<ulong, TRegister>(ref data[slotIndex]);
            register.BroadCastFrom(value);
        }

        public ref T Data<T>(
                     int index
        ) where T : unmanaged, INumber<T>
        {
            if (index is < 0 or >= 8) {
                throw new IndexOutOfRangeException();
            }

            return ref Unsafe.As<ulong, T>(ref data[index]);
        }

    }
}
