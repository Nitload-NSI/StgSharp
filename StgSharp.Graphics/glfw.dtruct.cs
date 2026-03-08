//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StructDefine"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
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