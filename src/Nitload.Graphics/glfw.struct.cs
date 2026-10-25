// -----------------------------------------------------------------------------
// file="glfw.struct"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Nitload.Common.Internal
{
    // GLFW owns these objects. The managed layer only passes opaque pointers to them.

    [StructLayout(LayoutKind.Sequential)]
    public struct glfwVideomode { }

    [StructLayout(LayoutKind.Sequential)]
    public struct glfwGammaramp { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GlfwImageNative
    {

        internal byte* Pixels;
        internal int Height;

        internal int Width;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWgamepadstate { }
}
