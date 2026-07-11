// -----------------------------------------------------------------------------
// file="glfw.dtruct"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;

using System;
using System.Runtime.InteropServices;

/**/

namespace StgSharp.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwContext { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwVideomode { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwGammaramp { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWimage { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWgamepadstate { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWallocator { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwMonitor { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct glfwWindow { }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWwindowCallback { }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWcursor { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWinitconfig { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwndconfig { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWctxconfig { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWfbconfig { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GLFWlibrary { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapelement { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapping { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWjoystick { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWls { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWplatform { }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmutex { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct VkExtensionProperties { }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct uintptr_t { }
}

/**/