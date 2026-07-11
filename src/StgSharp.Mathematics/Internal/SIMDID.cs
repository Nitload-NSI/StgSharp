// -----------------------------------------------------------------------------
// file="SIMDID"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Internal
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct SIMDID
    {

        [FieldOffset(0)] public fixed byte MaskByte[16];
        [FieldOffset(0)] public ulong Mask;

        internal SIMDID(
                 ulong data
        )
        {
            Mask = data;
        }

        public SIMDID()
        {
            Unsafe.SkipInit(out this);
        }

        public static bool operator !=(
                                    SIMDID left,
                                    SIMDID right
        )
        {
            return left.Mask != right.Mask;
        }

        public static bool operator ==(
                                    SIMDID left,
                                    SIMDID right
        )
        {
            return left.Mask == right.Mask;
        }

    }
}
