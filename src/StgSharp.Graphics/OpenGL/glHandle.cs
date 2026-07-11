// -----------------------------------------------------------------------------
// file="glHandle"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct GlHandle
    {

        [FieldOffset(0)] internal int SignedValue;
        [FieldOffset(0)] internal uint Value;

        public GlHandle()
        {
            Unsafe.SkipInit(out Value);
            Unsafe.SkipInit(out SignedValue);
            Unsafe.SkipInit(out this);
        }

        public static GlHandle Zero => new GlHandle
        {
            Value = 0
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GlHandle Create(
                               uint value
        )
        {
            return new GlHandle
            {
                Value = value
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GlHandle CreateSigned(
                               int value
        )
        {
            return new GlHandle
            {
                SignedValue = value
            };
        }

    }
}