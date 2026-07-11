// -----------------------------------------------------------------------------
// file="vkHandle"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.Vulkan
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct vkHandle
    {

        [FieldOffset(0)] public int SignedHalfValue;
        [FieldOffset(0)] public IntPtr Handle;
        [FieldOffset(0)] public IntPtr UnsignedHandle;
        [FieldOffset(0)] public uint HalfValue;

    }
}
