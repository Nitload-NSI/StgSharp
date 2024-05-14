//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glApiDefine.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
/*
 * This file is created to link functions in OpenGL to version in C#
 * The name of delegates and organization to init all these delegates 
 * are refferd from GLAD1 in agreement of MIT license generated from 
 * https://glad.dav1d.de/
 */


using StgSharp.Math;

using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics
{
    #region StgSharpDele

    public unsafe delegate void FrameBufferSizeHandler(IntPtr window, uint width, uint height);

    #endregion

    /// <summary>
    /// Function handler to load an opengl function by serching its name.
    /// </summary>
    /// <param name="name">the name of the Opengl function</param>
    /// <returns>An intptr value representing the pointer to the function</returns>
    public delegate IntPtr glLoader(string name);

    #region glfwAPI

    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWcursor* glfwCreateCursor(GLFWimage* image, int xhot, int yhot);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWcursor* glfwCreateStandardCursor(int shape);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate IntPtr glfwCreateWindow(int width, int height, byte[] title, IntPtr monitor, IntPtr share);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwDefaultWindowHints();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwDestroyCursor(GLFWcursor* cursor);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwDestroyWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwExtensionSupported(byte* extension);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwFocusWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetClipboardString(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate IntPtr glfwGetCurrentContext();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetCursorPos(IntPtr window, double* xpos, double* ypos);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetError(byte** description);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetFramebufferSize(IntPtr window, int* width, int* height);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetGamepadName(int jid);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetGamepadState(int jid, GLFWgamepadstate* state);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate glfwGammaramp* glfwGetGammaRamp(IntPtr monitor);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetInputMode(IntPtr window, int mode);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate float* glfwGetJoystickAxes(int jid, int* count);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetJoystickButtons(int jid, int* count);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetJoystickGUID(int jid);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetJoystickHats(int jid, int* count);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetJoystickName(int jid);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void* glfwGetJoystickUserPointer(int jid);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate KeyStatus glfwGetKey(IntPtr window, int key);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetKeyName(int key, int scancode);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetKeyScancode(int key);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetMonitorContentScale(IntPtr monitor, float* xscale, float* yscale);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetMonitorName(IntPtr monitor);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetMonitorPhysicalSize(IntPtr monitor, int* widthMM, int* heightMM);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetMonitorPos(IntPtr monitor, int* xpos, int* ypos);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate IntPtr* glfwGetMonitors(int* count);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void* glfwGetMonitorUserPointer(IntPtr monitor);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetMonitorWorkarea(IntPtr monitor, int* xpos, int* ypos, int* width, int* height);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetMouseButton(IntPtr window, int button);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate IntPtr glfwGetPrimaryMonitor();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate IntPtr glfwGetProcAddress(string procname);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte** glfwGetRequiredInstanceExtensions(uint* count);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate double glfwGetTime();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate ulong* glfwGetTimerFrequency();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate ulong* glfwGetTimerValue();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetVersion(int* major, int* minor, int* rev);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate byte* glfwGetVersionString();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate glfwVideomode* glfwGetVideoMode(IntPtr monitor);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate glfwVideomode* glfwGetVideoModes(IntPtr monitor, int* count);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetWindowAttrib(IntPtr window, int attrib);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetWindowContentScale(IntPtr window, float* xscale, float* yscale);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetWindowFrameSize(IntPtr window, int* left, int* top, int* right, int* bottom);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate IntPtr glfwGetWindowMonitor(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate float glfwGetWindowOpacity(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetWindowPos(IntPtr window, int* xpos, int* ypos);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwGetWindowSize(IntPtr window, int* width, int* height);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void* glfwGetWindowUserPointer(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwHideWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwIconifyWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwInit();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwInitHint(int hint, int value);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwJoystickIsGamepad(int jid);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwJoystickPresent(int jid);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwMakeContextCurrent(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwMaximizeWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwPollEvents();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwPostEmptyEvent();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwRawMouseMotionSupported();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwRequestWindowAttention(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwRestoreWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWcharfun glfwSetCharCallback(IntPtr window, GLFWcharfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWcharmodsfun glfwSetCharModsCallback(IntPtr window, GLFWcharmodsfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetClipboardString(IntPtr window, byte* str);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetCursor(IntPtr window, GLFWcursor* cursor);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWcursorenterfun glfwSetCursorEnterCallback(IntPtr window, GLFWcursorenterfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetCursorPos(IntPtr window, double xpos, double ypos);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWcursorposfun glfwSetCursorPosCallback(IntPtr window, GLFWcursorposfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWdropfun glfwSetDropCallback(IntPtr window, GLFWdropfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWerrorfun glfwSetErrorCallback(GLFWerrorfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate FramebuffersizeHandler glfwSetFramebufferSizeCallback(IntPtr window, IntPtr callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetGamma(IntPtr monitor, float gamma);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetGammaRamp(IntPtr monitor, glfwGammaramp* ramp);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetInputMode(IntPtr window, int mode, int value);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWjoystickfun glfwSetJoystickCallback(GLFWjoystickfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetJoystickUserPointer(int jid, void* pointer);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWkeyfun glfwSetKeyCallback(IntPtr window, GLFWkeyfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWmonitorfun glfwSetMonitorCallback(GLFWmonitorfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetMonitorUserPointer(IntPtr monitor, void* pointer);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWmousebuttonfun glfwSetMouseButtonCallback(IntPtr window, GLFWmousebuttonfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWscrollfun glfwSetScrollCallback(IntPtr window, GLFWscrollfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetTime(double time);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowAspectRatio(IntPtr window, int numer, int denom);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowAttrib(IntPtr window, int attrib, int value);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowclosefun glfwSetWindowCloseCallback(IntPtr window, GLFWwindowclosefun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback(IntPtr window, GLFWwindowcontentscalefun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowfocusfun glfwSetWindowFocusCallback(IntPtr window, GLFWwindowfocusfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowIcon(IntPtr window, int count, GLFWimage* images);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowiconifyfun glfwSetWindowIconifyCallback(IntPtr window, GLFWwindowiconifyfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowmaximizefun glfwSetWindowMaximizeCallback(IntPtr window, GLFWwindowmaximizefun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowMonitor(IntPtr window, IntPtr monitor, int xpos, int ypos, int width, int height, int refreshRate);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowOpacity(IntPtr window, float opacity);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowPos(IntPtr window, int xpos, int ypos);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowposfun glfwSetWindowPosCallback(IntPtr window, GLFWwindowposfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowrefreshfun glfwSetWindowRefreshCallback(IntPtr window, GLFWwindowrefreshfun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowShouldClose(IntPtr window, int value);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowSize(IntPtr window, int width, int height);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWwindowsizefun glfwSetWindowSizeCallback(IntPtr window, GLFWwindowsizefun callback);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowSizeLimits(IntPtr window, int minwidth, int minheight, int maxwidth, int maxheight);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowTitle(IntPtr window, byte* title);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSetWindowUserPointer(IntPtr window, void* pointer);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwShowWindow(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSwapBuffers(IntPtr window);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwSwapInterval(int interval);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwTerminate();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwUpdateGamepadMappings(byte* str);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwVulkanSupported();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwWaitEvents();
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwWaitEventsTimeout(double timeout);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwWindowHint(int hint, int value);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate void glfwWindowHintString(int hint, byte* value);
    [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwWindowShouldClose(IntPtr window);
    //[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl, CharSet = CharSet.Ansi)] internal unsafe delegate GLFWvkproc glfwGetInstanceProcAddress(VkInstance instance, byte* procname);
    //[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl, CharSet = CharSet.Ansi)] internal unsafe delegate int glfwGetPhysicalDevicePresentationSupport(VkInstance instance, VkPhysicalDevice device, uint* queuefamily);
    //[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl, CharSet = CharSet.Ansi)] internal unsafe delegate VkResult glfwCreateWindowSurface(VkInstance instance, glfwWindow* window, VkAllocationCallbacks* allocator, VkSurfaceKHR* surface);


    #endregion

    #region glDEBUG

    internal unsafe delegate void GLDEBUGPROC(int source, int type, uint id, int severity, int length, sbyte* message, void* userParam);
    internal unsafe delegate void GLDEBUGPROCARB(int source, int type, uint id, int severity, int length, sbyte* message, void* userParam);
    internal unsafe delegate void GLDEBUGPROCKHR(int source, int type, uint id, int severity, int length, sbyte* message, void* userParam);
    internal unsafe delegate void GLDEBUGPROCAMD(uint id, int category, int severity, int length, sbyte* message, void* userParam);

    #endregion


}
