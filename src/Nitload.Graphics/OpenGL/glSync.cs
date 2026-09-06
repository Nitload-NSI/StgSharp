// -----------------------------------------------------------------------------
// file="glSync"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Nitload.Graphics.OpenGL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct glSync
    {

        public IntPtr Handle;

    }
}
