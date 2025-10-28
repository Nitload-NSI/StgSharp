//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glfw"
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
using System.Text;

namespace StgSharp.Internal
{
    internal static unsafe partial class GraphicFramework
    {

        private static readonly GLFWFunction _glfw = new GLFWFunction();

        public static GLFWFunction glfwContext => _glfw;

        [LibraryImport(Native.LibName, EntryPoint = "load_glfw_functions")]
        internal static unsafe partial void glfwLoadLibrary(GLFWFunction* context);

        internal static unsafe void LoadGlfw()
        {
            fixed (GLFWFunction* func = &_glfw) {
                glfwLoadLibrary(func);
            }
        }

        #region api call

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwInit()
        {
            return _glfw.glfwInit();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwTerminate()
        {
            _glfw.glfwTerminate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwInitHint(int hint, int value)
        {
            _glfw.glfwInitHint(hint, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetVersion(int* major, int* minor, int* rev)
        {
            _glfw.glfwGetVersion(major, minor, rev);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetVersionString()
        {
            return _glfw.glfwGetVersionString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwGetError(byte** description)
        {
            return _glfw.glfwGetError(description);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<int, byte*, void> glfwSetErrorCallback(
                                                                            delegate* unmanaged[Cdecl]<int, byte*, void> callback)
        {
            return _glfw.glfwSetErrorCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr* glfwGetMonitors(int* count)
        {
            return _glfw.glfwGetMonitors(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwGetPrimaryMonitor()
        {
            return _glfw.glfwGetPrimaryMonitor();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetMonitorPos(IntPtr monitor, int* xpos, int* ypos)
        {
            _glfw.glfwGetMonitorPos(monitor, xpos, ypos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetMonitorWorkArea(
                                    IntPtr monitor,
                                    int* xPos,
                                    int* yPos,
                                    int* width,
                                    int* height)
        {
            _glfw.glfwGetMonitorWorkarea(monitor, xPos, yPos, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetMonitorPhysicalSize(IntPtr monitor, int* widthMM, int* heightMM)
        {
            _glfw.glfwGetMonitorPhysicalSize(monitor, widthMM, heightMM);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetMonitorContentScale(IntPtr monitor, float* xscale, float* yscale)
        {
            _glfw.glfwGetMonitorContentScale(monitor, xscale, yscale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetMonitorName(IntPtr monitor)
        {
            return _glfw.glfwGetMonitorName(monitor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetMonitorUserPointer(IntPtr monitor, void* pointer)
        {
            _glfw.glfwSetMonitorUserPointer(monitor, pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void* glfwGetMonitorUserPointer(IntPtr monitor)
        {
            return _glfw.glfwGetMonitorUserPointer(monitor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetMonitorCallback(
                                                                             delegate* unmanaged[Cdecl]<IntPtr, int, void> callback)
        {
            return _glfw.glfwSetMonitorCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe glfwVideomode* glfwGetVideoModes(IntPtr monitor, int* count)
        {
            return (glfwVideomode*)_glfw.glfwGetVideoModes(monitor, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe glfwVideomode* glfwGetVideoMode(IntPtr monitor)
        {
            return (glfwVideomode*)_glfw.glfwGetVideoMode(monitor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetGamma(IntPtr monitor, float gamma)
        {
            _glfw.glfwSetGamma(monitor, gamma);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe glfwGammaramp* glfwGetGammaRamp(IntPtr monitor)
        {
            return (glfwGammaramp*)_glfw.glfwGetGammaRamp(monitor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetGammaRamp(IntPtr monitor, glfwGammaramp* ramp)
        {
            _glfw.glfwSetGammaRamp(monitor, (IntPtr)ramp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwDefaultWindowHints()
        {
            _glfw.glfwDefaultWindowHints();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwWindowHint(int hint, int value)
        {
            _glfw.glfwWindowHint(hint, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwWindowHintString(int hint, byte* value)
        {
            _glfw.glfwWindowHintString(hint, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwCreateWindow(
                                      int width,
                                      int height,
                                      Span<byte> title,
                                      IntPtr monitor,
                                      IntPtr share)
        {
            fixed (byte* titlePtr = title) {
                return _glfw.glfwCreateWindow(width, height, titlePtr, monitor, share);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwDestroyWindow(IntPtr window)
        {
            _glfw.glfwDestroyWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwWindowShouldClose(IntPtr window)
        {
            return _glfw.glfwWindowShouldClose(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowShouldClose(IntPtr window, int value)
        {
            _glfw.glfwSetWindowShouldClose(window, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowTitle(IntPtr window, byte* title)
        {
            _glfw.glfwSetWindowTitle(window, title);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowIcon(IntPtr window, int count, GLFWimage* images)
        {
            _glfw.glfwSetWindowIcon(window, count, (IntPtr)images);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetWindowPos(IntPtr window, int* xpos, int* ypos)
        {
            _glfw.glfwGetWindowPos(window, xpos, ypos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowPos(IntPtr window, int xpos, int ypos)
        {
            _glfw.glfwSetWindowPos(window, xpos, ypos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetWindowSize(IntPtr window, int* width, int* height)
        {
            _glfw.glfwGetWindowSize(window, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowSizeLimits(
                                    IntPtr window,
                                    int minWidth,
                                    int minHeight,
                                    int maxWidth,
                                    int maxHeight)
        {
            _glfw.glfwSetWindowSizeLimits(window, minWidth, minHeight, maxWidth, maxHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowAspectRatio(IntPtr window, int numer, int denom)
        {
            _glfw.glfwSetWindowAspectRatio(window, numer, denom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowSize(IntPtr window, int width, int height)
        {
            _glfw.glfwSetWindowSize(window, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetFramebufferSize(IntPtr window, int* width, int* height)
        {
            _glfw.glfwGetFrameBufferSize(window, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetWindowFrameSize(IntPtr window, int* left, int* top, int* right, int* bottom)
        {
            _glfw.glfwGetWindowFrameSize(window, left, top, right, bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetWindowContentScale(IntPtr window, float* xscale, float* yscale)
        {
            _glfw.glfwGetWindowContentScale(window, xscale, yscale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe float glfwGetWindowOpacity(IntPtr window)
        {
            return _glfw.glfwGetWindowOpacity(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowOpacity(IntPtr window, float opacity)
        {
            _glfw.glfwSetWindowOpacity(window, opacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwIconifyWindow(IntPtr window)
        {
            _glfw.glfwIconifyWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwRestoreWindow(IntPtr window)
        {
            _glfw.glfwRestoreWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwMaximizeWindow(IntPtr window)
        {
            _glfw.glfwMaximizeWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwShowWindow(IntPtr window)
        {
            _glfw.glfwShowWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwHideWindow(IntPtr window)
        {
            _glfw.glfwHideWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwFocusWindow(IntPtr window)
        {
            _glfw.glfwFocusWindow(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwRequestWindowAttention(IntPtr window)
        {
            _glfw.glfwRequestWindowAttention(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwGetWindowMonitor(IntPtr window)
        {
            return _glfw.glfwGetWindowMonitor(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowMonitor(
                                    IntPtr window,
                                    IntPtr monitor,
                                    int xpos,
                                    int ypos,
                                    int width,
                                    int height,
                                    int refreshRate)
        {
            _glfw.glfwSetWindowMonitor(window, monitor, xpos, ypos, width, height, refreshRate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwGetWindowAttrib(IntPtr window, int attrib)
        {
            return _glfw.glfwGetWindowAttrib(window, attrib);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowAttrib(IntPtr window, int attrib, int value)
        {
            _glfw.glfwSetWindowAttrib(window, attrib, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetWindowUserPointer(IntPtr window, void* pointer)
        {
            _glfw.glfwSetWindowUserPointer(window, pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void* glfwGetWindowUserPointer(IntPtr window)
        {
            return _glfw.glfwGetWindowUserPointer(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowPosCallback(
                                                                                  IntPtr window,
                                                                                  delegate* unmanaged[Cdecl]<IntPtr, int, int, void> callback)
        {
            return _glfw.glfwSetWindowPosCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetWindowSizeCallback(
                                                                                  IntPtr window,
                                                                                  delegate* unmanaged[Cdecl]<IntPtr, int, int, void> callback)
        {
            return _glfw.glfwSetWindowSizeCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, void> glfwSetWindowCloseCallback(
                                                                        IntPtr window,
                                                                        delegate* unmanaged[Cdecl]<IntPtr, void> callback)
        {
            return _glfw.glfwSetWindowCloseCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, void> glfwSetWindowRefreshCallback(
                                                                        IntPtr window,
                                                                        delegate* unmanaged[Cdecl]<IntPtr, void> callback)
        {
            return _glfw.glfwSetWindowRefreshCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetWindowFocusCallback(
                                                                             IntPtr window,
                                                                             delegate* unmanaged[Cdecl]<IntPtr, int, void> callback)
        {
            return _glfw.glfwSetWindowFocusCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetWindowIconifyCallback(
                                                                             IntPtr window,
                                                                             delegate* unmanaged[Cdecl]<IntPtr, int, void> callback)
        {
            return _glfw.glfwSetWindowIconifyCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetWindowMaximizeCallback(
                                                                             IntPtr window,
                                                                             delegate* unmanaged[Cdecl]<IntPtr, int, void> callback)
        {
            return _glfw.glfwSetWindowMaximizeCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, int, void> glfwSetFrameBufferSizeCallback(
                                                                                  IntPtr window,
                                                                                  delegate* unmanaged[Cdecl]<IntPtr, int, int, void> callback)
        {
            return _glfw.glfwSetFramebufferSizeCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, float, float, void> glfwSetWindowContentScaleCallback(
                                                                                      IntPtr window,
                                                                                      delegate* unmanaged[Cdecl]<IntPtr, float, float, void> callback)
        {
            return _glfw.glfwSetWindowContentScaleCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwPollEvents()
        {
            _glfw.glfwPollEvents();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwWaitEvents()
        {
            _glfw.glfwWaitEvents();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwWaitEventsTimeout(double timeout)
        {
            _glfw.glfwWaitEventsTimeout(timeout);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwPostEmptyEvent()
        {
            _glfw.glfwPostEmptyEvent();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwGetInputMode(IntPtr window, int mode)
        {
            return _glfw.glfwGetInputMode(window, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetInputMode(IntPtr window, int mode, int value)
        {
            _glfw.glfwSetInputMode(window, mode, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwRawMouseMotionSupported()
        {
            return _glfw.glfwRawMouseMotionSupported();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetKeyName(int key, int scancode)
        {
            return _glfw.glfwGetKeyName(key, scancode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwGetKeyScancode(int key)
        {
            return _glfw.glfwGetKeyScancode(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe KeyStatus glfwGetKey(IntPtr window, int key)
        {
            return _glfw.glfwGetKey(window, key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwGetMouseButton(IntPtr window, int button)
        {
            return _glfw.glfwGetMouseButton(window, button);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwGetCursorPos(IntPtr window, double* xpos, double* ypos)
        {
            _glfw.glfwGetCursorPos(window, xpos, ypos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetCursorPos(IntPtr window, double xpos, double ypos)
        {
            _glfw.glfwSetCursorPos(window, xpos, ypos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwCreateCursor(GLFWimage* image, int xhot, int yhot)
        {
            return _glfw.glfwCreateCursor((IntPtr)image, xhot, yhot);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwCreateStandardCursor(int shape)
        {
            return _glfw.glfwCreateStandardCursor(shape);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwDestroyCursor(IntPtr cursor)
        {
            _glfw.glfwDestroyCursor(cursor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetCursor(IntPtr window, IntPtr cursor)
        {
            _glfw.glfwSetCursor(window, cursor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void> glfwSetKeyCallback(
                                                                                            IntPtr window,
                                                                                            delegate* unmanaged[Cdecl]<IntPtr, int, int, int, int, void> callback)
        {
            return _glfw.glfwSetKeyCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, uint, void> glfwSetCharCallback(
                                                                              IntPtr window,
                                                                              delegate* unmanaged[Cdecl]<IntPtr, uint, void> callback)
        {
            return _glfw.glfwSetCharCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, uint, int, void> glfwSetCharModsCallback(
                                                                                   IntPtr window,
                                                                                   delegate* unmanaged[Cdecl]<IntPtr, uint, int, void> callback)
        {
            return _glfw.glfwSetCharModsCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, int, int, void> glfwSetMouseButtonCallback(
                                                                                       IntPtr window,
                                                                                       delegate* unmanaged[Cdecl]<IntPtr, int, int, int, void> callback)
        {
            return _glfw.glfwSetMouseButtonCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, double, double, void> glfwSetCursorPosCallback(
                                                                                        IntPtr window,
                                                                                        delegate* unmanaged[Cdecl]<IntPtr, double, double, void> callback)
        {
            return _glfw.glfwSetCursorPosCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, void> glfwSetCursorEnterCallback(
                                                                             IntPtr window,
                                                                             delegate* unmanaged[Cdecl]<IntPtr, int, void> callback)
        {
            return _glfw.glfwSetCursorEnterCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, double, double, void> glfwSetScrollCallback(
                                                                                        IntPtr window,
                                                                                        delegate* unmanaged[Cdecl]<IntPtr, double, double, void> callback)
        {
            return _glfw.glfwSetScrollCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<IntPtr, int, byte**, void> glfwSetDropCallback(
                                                                                     IntPtr window,
                                                                                     delegate* unmanaged[Cdecl]<IntPtr, int, byte**, void> callback)
        {
            return _glfw.glfwSetDropCallback(window, callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwJoystickPresent(int jid)
        {
            return _glfw.glfwJoystickPresent(jid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe float* glfwGetJoystickAxes(int jid, int* count)
        {
            return _glfw.glfwGetJoystickAxes(jid, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetJoystickButtons(int jid, int* count)
        {
            return _glfw.glfwGetJoystickButtons(jid, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetJoystickHats(int jid, int* count)
        {
            return _glfw.glfwGetJoystickHats(jid, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetJoystickName(int jid)
        {
            return _glfw.glfwGetJoystickName(jid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetJoystickGUID(int jid)
        {
            return _glfw.glfwGetJoystickGUID(jid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetJoystickUserPointer(int jid, void* pointer)
        {
            _glfw.glfwSetJoystickUserPointer(jid, pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void* glfwGetJoystickUserPointer(int jid)
        {
            return _glfw.glfwGetJoystickUserPointer(jid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwJoystickIsGamepad(int jid)
        {
            return _glfw.glfwJoystickIsGamePad(jid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe delegate* unmanaged[Cdecl]<int, int, void> glfwSetJoystickCallback(
                                                                          delegate* unmanaged[Cdecl]<int, int, void> callback)
        {
            return _glfw.glfwSetJoystickCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwUpdateGamePadMappings(byte* str)
        {
            return _glfw.glfwUpdateGamePadMappings(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetGamePadName(int jid)
        {
            return _glfw.glfwGetGamepadName(jid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwGetGamePadState(int jid, GLFWgamepadstate* state)
        {
            return _glfw.glfwGetGamepadState(jid, (IntPtr)state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetClipboardString(IntPtr window, byte* str)
        {
            _glfw.glfwSetClipboardString(window, str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte* glfwGetClipboardString(IntPtr window)
        {
            return _glfw.glfwGetClipboardString(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe double glfwGetTime()
        {
            return _glfw.glfwGetTime();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSetTime(double time)
        {
            _glfw.glfwSetTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe ulong glfwGetTimerValue()
        {
            return _glfw.glfwGetTimerValue();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe ulong glfwGetTimerFrequency()
        {
            return _glfw.glfwGetTimerFrequency();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwMakeContextCurrent(IntPtr window)
        {
            _glfw.glfwMakeContextCurrent(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwGetCurrentContext()
        {
            return _glfw.glfwGetCurrentContext();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSwapBuffers(IntPtr window)
        {
            _glfw.glfwSwapBuffers(window);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void glfwSwapInterval(int interval)
        {
            _glfw.glfwSwapInterval(interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwExtensionSupported(byte* extension)
        {
            return _glfw.glfwExtensionSupported(extension);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwGetProcAddress(string procName)
        {
            return glfwGetProcAddress(Encoding.UTF8.GetBytes(procName));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwGetProcAddress(ReadOnlySpan<byte> procName)
        {
            return glfwGetProcAddress(procName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe IntPtr glfwGetProcAddress(Span<byte> procName)
        {
            fixed (byte* bPtr = procName) {
                return _glfw.glfwGetProcAddress(bPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int glfwVulkanSupported()
        {
            return _glfw.glfwVulkanSupported();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe byte** glfwGetRequiredInstanceExtensions(uint* count)
        {
            return _glfw.glfwGetRequiredInstanceExtensions(count);
        }

    }
}

#endregion

