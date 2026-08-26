// -----------------------------------------------------------------------------
// file="glfw.struct"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace StgSharp.Internal
{
    // GLFW owns these objects. The managed layer only passes opaque pointers to them.

    [StructLayout(LayoutKind.Sequential)]
    public struct glfwVideomode { }

    [StructLayout(LayoutKind.Sequential)]
    public struct glfwGammaramp { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GlfwImageNative
    {

        internal int Width;
        internal int Height;
        internal byte* Pixels;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWgamepadstate { }
}
