//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glfwFunction"
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
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GLFWFunction
    {

        public delegate* unmanaged[Cdecl]<IntPtr, int, int, IntPtr> glfwCreateCursor;
        public delegate* unmanaged[Cdecl]<int, IntPtr> glfwCreateStandardCursor;
        public delegate* unmanaged[Cdecl]<int, int, byte*, IntPtr, IntPtr, IntPtr> glfwCreateWindow;
        public delegate* unmanaged[Cdecl]<void> glfwDefaultWindowHints;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwDestroyCursor;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwDestroyWindow;
        public delegate* unmanaged[Cdecl]<byte*, int> glfwExtensionSupported;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwFocusWindow;
        public delegate* unmanaged[Cdecl]<IntPtr, byte*> glfwGetClipboardString;
        public delegate* unmanaged[Cdecl]<IntPtr> glfwGetCurrentContext;
        public delegate* unmanaged[Cdecl]<IntPtr, double*, double*, void> glfwGetCursorPos;
        public delegate* unmanaged[Cdecl]<byte**, int> glfwGetError;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, void> glfwGetFrameBufferSize;
        public delegate* unmanaged[Cdecl]<int, byte*> glfwGetGamepadName;
        public delegate* unmanaged[Cdecl]<int, IntPtr, int> glfwGetGamepadState;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetGammaRamp;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetInputMode;
        public delegate* unmanaged[Cdecl]<int, int*, float*> glfwGetJoystickAxes;
        public delegate* unmanaged[Cdecl]<int, int*, byte*> glfwGetJoystickButtons;
        public delegate* unmanaged[Cdecl]<int, byte*> glfwGetJoystickGUID;
        public delegate* unmanaged[Cdecl]<int, int*, byte*> glfwGetJoystickHats;
        public delegate* unmanaged[Cdecl]<int, byte*> glfwGetJoystickName;
        public delegate* unmanaged[Cdecl]<int, void*> glfwGetJoystickUserPointer;
        public delegate* unmanaged[Cdecl]<IntPtr, int, KeyStatus> glfwGetKey;
        public delegate* unmanaged[Cdecl]<int, int, byte*> glfwGetKeyName;
        public delegate* unmanaged[Cdecl]<int, int> glfwGetKeyScancode;
        public delegate* unmanaged[Cdecl]<IntPtr, float*, float*, void> glfwGetMonitorContentScale;
        public delegate* unmanaged[Cdecl]<IntPtr, byte*> glfwGetMonitorName;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, void> glfwGetMonitorPhysicalSize;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, void> glfwGetMonitorPos;
        public delegate* unmanaged[Cdecl]<int*, IntPtr*> glfwGetMonitors;
        public delegate* unmanaged[Cdecl]<IntPtr, void*> glfwGetMonitorUserPointer;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, int*, int*, void> glfwGetMonitorWorkarea;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetMouseButton;
        public delegate* unmanaged[Cdecl]<IntPtr> glfwGetPrimaryMonitor;
        public delegate* unmanaged[Cdecl]<byte*, IntPtr> glfwGetProcAddress;
        public delegate* unmanaged[Cdecl]<uint*, byte**> glfwGetRequiredInstanceExtensions;
        public delegate* unmanaged[Cdecl]<double> glfwGetTime;
        public delegate* unmanaged[Cdecl]<ulong> glfwGetTimerFrequency;
        public delegate* unmanaged[Cdecl]<ulong> glfwGetTimerValue;
        public delegate* unmanaged[Cdecl]<int*, int*, int*, void> glfwGetVersion;
        public delegate* unmanaged[Cdecl]<byte*> glfwGetVersionString;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetVideoMode;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, IntPtr*> glfwGetVideoModes;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int> glfwGetWindowAttrib;
        public delegate* unmanaged[Cdecl]<IntPtr, float*, float*, void> glfwGetWindowContentScale;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, int*, int*, void> glfwGetWindowFrameSize;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> glfwGetWindowMonitor;
        public delegate* unmanaged[Cdecl]<IntPtr, float> glfwGetWindowOpacity;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, void> glfwGetWindowPos;
        public delegate* unmanaged[Cdecl]<IntPtr, int*, int*, void> glfwGetWindowSize;
        public delegate* unmanaged[Cdecl]<IntPtr, void*> glfwGetWindowUserPointer;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwHideWindow;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwIconifyWindow;
        public delegate* unmanaged[Cdecl]<int> glfwInit;
        public delegate* unmanaged[Cdecl]<int, int, void> glfwInitHint;
        public delegate* unmanaged[Cdecl]<int, int> glfwJoystickIsGamePad;
        public delegate* unmanaged[Cdecl]<int, int> glfwJoystickPresent;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwMakeContextCurrent;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwMaximizeWindow;
        public delegate* unmanaged[Cdecl]<void> glfwPollEvents;
        public delegate* unmanaged[Cdecl]<void> glfwPostEmptyEvent;
        public delegate* unmanaged[Cdecl]<int> glfwRawMouseMotionSupported;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwRequestWindowAttention;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwRestoreWindow;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, uint, void>, delegate* unmanaged[Cdecl]<IntPtr, uint, void>> glfwSetCharCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, uint, int, void>, delegate* unmanaged[Cdecl]<IntPtr, uint, int, void>> glfwSetCharModsCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, byte*, void> glfwSetClipboardString;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> glfwSetCursor;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, void>> glfwSetCursorEnterCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, double, double, void> glfwSetCursorPos;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, double, double, void>, delegate* unmanaged[Cdecl]<IntPtr, double, double, void>> glfwSetCursorPosCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, byte**, void>, delegate* unmanaged[Cdecl]<IntPtr, int, byte**, void>> glfwSetDropCallback;
        public delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<int, byte*, void>, delegate* unmanaged[Cdecl]<int, byte*, void>> glfwSetErrorCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, int, void>> glfwSetFramebufferSizeCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, float, void> glfwSetGamma;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> glfwSetGammaRamp;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetInputMode;
        public delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<int, int, void>, delegate* unmanaged[Cdecl]<int, int, void>> glfwSetJoystickCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, void*, void> glfwSetJoystickUserPointer;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void>> glfwSetKeyCallback;
        public delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<IntPtr, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, void>> glfwSetMonitorCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, void*, void> glfwSetMonitorUserPointer;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, int, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, int, int, void>> glfwSetMouseButtonCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, double, double, void>, delegate* unmanaged[Cdecl]<IntPtr, double, double, void>> glfwSetScrollCallback;
        public delegate* unmanaged[Cdecl]<double, void> glfwSetTime;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowAspectRatio;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowAttrib;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, void>, delegate* unmanaged[Cdecl]<IntPtr, void>> glfwSetWindowCloseCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, float, float, void>, delegate* unmanaged[Cdecl]<IntPtr, float, float, void>> glfwSetWindowContentScaleCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, void>> glfwSetWindowFocusCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, int, IntPtr, void> glfwSetWindowIcon;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, void>> glfwSetWindowIconifyCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, void>> glfwSetWindowMaximizeCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, int, int, int, int, void> glfwSetWindowMonitor;
        public delegate* unmanaged[Cdecl]<IntPtr, float, void> glfwSetWindowOpacity;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowPos;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, int, void>> glfwSetWindowPosCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, void>, delegate* unmanaged[Cdecl]<IntPtr, void>> glfwSetWindowRefreshCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetWindowShouldClose;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowSize;
        public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int, int, void>, delegate* unmanaged[Cdecl]<IntPtr, int, int, void>> glfwSetWindowSizeCallback;
        public delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void> glfwSetWindowSizeLimits;
        public delegate* unmanaged[Cdecl]<IntPtr, byte*, void> glfwSetWindowTitle;
        public delegate* unmanaged[Cdecl]<IntPtr, void*, void> glfwSetWindowUserPointer;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwShowWindow;
        public delegate* unmanaged[Cdecl]<IntPtr, void> glfwSwapBuffers;
        public delegate* unmanaged[Cdecl]<int, void> glfwSwapInterval;
        public delegate* unmanaged[Cdecl]<void> glfwTerminate;
        public delegate* unmanaged[Cdecl]<byte*, int> glfwUpdateGamePadMappings;
        public delegate* unmanaged[Cdecl]<int> glfwVulkanSupported;
        public delegate* unmanaged[Cdecl]<void> glfwWaitEvents;
        public delegate* unmanaged[Cdecl]<double, void> glfwWaitEventsTimeout;
        public delegate* unmanaged[Cdecl]<int, int, void> glfwWindowHint;
        public delegate* unmanaged[Cdecl]<int, byte*, void> glfwWindowHintString;
        public delegate* unmanaged[Cdecl]<IntPtr, int> glfwWindowShouldClose;

        public GLFWFunction()
        {
            Unsafe.SkipInit(out this);
        }

    }
}