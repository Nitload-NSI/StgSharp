//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glfw.cs"
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
using StgSharp.Graphics;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Internal
{
    internal partial class InternalIO
    {

        #region api call

        [LibraryImport( SSC_libName, EntryPoint = "glfwInit" )]
        internal static unsafe partial int glfwInit();

        [LibraryImport( SSC_libName, EntryPoint = "glfwTerminate" )]
        internal static unsafe partial void glfwTerminate();

        [LibraryImport( SSC_libName, EntryPoint = "glfwInitHint" )]
        internal static unsafe partial void glfwInitHint( int hint, int value );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetVersion" )]
        internal static unsafe partial void glfwGetVersion(
                                                    int* major,
                                                    int* minor,
                                                    int* rev );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetVersionString" )]
        internal static unsafe partial byte* glfwGetVersionString();

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetError" )]
        internal static unsafe partial int glfwGetError( byte** description );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetErrorCallback" )]
        internal static unsafe partial GLFWerrorfun glfwSetErrorCallback(
                                                            GLFWerrorfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitors" )]
        internal static unsafe partial IntPtr* glfwGetMonitors( int* count );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetPrimaryMonitor" )]
        internal static unsafe partial IntPtr glfwGetPrimaryMonitor();

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitorPos" )]
        internal static unsafe partial void glfwGetMonitorPos(
                                                    IntPtr monitor,
                                                    int* xpos,
                                                    int* ypos );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitorWorkarea" )]
        internal static unsafe partial void glfwGetMonitorWorkarea(
                                                    IntPtr monitor,
                                                    int* xpos,
                                                    int* ypos,
                                                    int* width,
                                                    int* height );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitorPhysicalSize" )]
        internal static unsafe partial void glfwGetMonitorPhysicalSize(
                                                    IntPtr monitor,
                                                    int* widthMM,
                                                    int* heightMM );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitorContentScale" )]
        internal static unsafe partial void glfwGetMonitorContentScale(
                                                    IntPtr monitor,
                                                    float* xscale,
                                                    float* yscale );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitorName" )]
        internal static unsafe partial byte* glfwGetMonitorName(
                                                     IntPtr monitor );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetMonitorUserPointer" )]
        internal static unsafe partial void glfwSetMonitorUserPointer(
                                                    IntPtr monitor,
                                                    void* pointer );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMonitorUserPointer" )]
        internal static unsafe partial void* glfwGetMonitorUserPointer(
                                                     IntPtr monitor );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetMonitorCallback" )]
        internal static unsafe partial GLFWmonitorfun glfwSetMonitorCallback(
                                                              GLFWmonitorfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetVideoModes" )]
        internal static unsafe partial glfwVideomode* glfwGetVideoModes(
                                                              IntPtr monitor,
                                                              int* count );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetVideoMode" )]
        internal static unsafe partial glfwVideomode* glfwGetVideoMode(
                                                              IntPtr monitor );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetGamma" )]
        internal static unsafe partial void glfwSetGamma(
                                                    IntPtr monitor,
                                                    float gamma );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetGammaRamp" )]
        internal static unsafe partial glfwGammaramp* glfwGetGammaRamp(
                                                              IntPtr monitor );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetGammaRamp" )]
        internal static unsafe partial void glfwSetGammaRamp(
                                                    IntPtr monitor,
                                                    glfwGammaramp* ramp );

        [LibraryImport( SSC_libName, EntryPoint = "glfwDefaultWindowHints" )]
        internal static unsafe partial void glfwDefaultWindowHints();

        [LibraryImport( SSC_libName, EntryPoint = "glfwWindowHint" )]
        internal static unsafe partial void glfwWindowHint(
                                                    int hint,
                                                    int value );

        [LibraryImport( SSC_libName, EntryPoint = "glfwWindowHintString" )]
        internal static unsafe partial void glfwWindowHintString(
                                                    int hint,
                                                    byte* value );

        [LibraryImport( SSC_libName, EntryPoint = "glfwCreateWindow" )]
        internal static unsafe partial IntPtr glfwCreateWindow(
                                                      int width,
                                                      int height,
                                                      byte[] title,
                                                      IntPtr monitor,
                                                      IntPtr share );

        [LibraryImport( SSC_libName, EntryPoint = "glfwDestroyWindow" )]
        internal static unsafe partial void glfwDestroyWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwWindowShouldClose" )]
        internal static unsafe partial int glfwWindowShouldClose(
                                                   IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowShouldClose" )]
        internal static unsafe partial void glfwSetWindowShouldClose(
                                                    IntPtr window,
                                                    int value );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowTitle" )]
        internal static unsafe partial void glfwSetWindowTitle(
                                                    IntPtr window,
                                                    byte* title );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowIcon" )]
        internal static unsafe partial void glfwSetWindowIcon(
                                                    IntPtr window,
                                                    int count,
                                                    GLFWimage* images );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowPos" )]
        internal static unsafe partial void glfwGetWindowPos(
                                                    IntPtr window,
                                                    int* xpos,
                                                    int* ypos );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowPos" )]
        internal static unsafe partial void glfwSetWindowPos(
                                                    IntPtr window,
                                                    int xpos,
                                                    int ypos );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowSize" )]
        internal static unsafe partial void glfwGetWindowSize(
                                                    IntPtr window,
                                                    int* width,
                                                    int* height );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowSizeLimits" )]
        internal static unsafe partial void glfwSetWindowSizeLimits(
                                                    IntPtr window,
                                                    int minwidth,
                                                    int minheight,
                                                    int maxwidth,
                                                    int maxheight );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowAspectRatio" )]
        internal static unsafe partial void glfwSetWindowAspectRatio(
                                                    IntPtr window,
                                                    int numer,
                                                    int denom );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowSize" )]
        internal static unsafe partial void glfwSetWindowSize(
                                                    IntPtr window,
                                                    int width,
                                                    int height );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetFramebufferSize" )]
        internal static unsafe partial void glfwGetFramebufferSize(
                                                    IntPtr window,
                                                    int* width,
                                                    int* height );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowFrameSize" )]
        internal static unsafe partial void glfwGetWindowFrameSize(
                                                    IntPtr window,
                                                    int* left,
                                                    int* top,
                                                    int* right,
                                                    int* bottom );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowContentScale" )]
        internal static unsafe partial void glfwGetWindowContentScale(
                                                    IntPtr window,
                                                    float* xscale,
                                                    float* yscale );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowOpacity" )]
        internal static unsafe partial float glfwGetWindowOpacity(
                                                     IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowOpacity" )]
        internal static unsafe partial void glfwSetWindowOpacity(
                                                    IntPtr window,
                                                    float opacity );

        [LibraryImport( SSC_libName, EntryPoint = "glfwIconifyWindow" )]
        internal static unsafe partial void glfwIconifyWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwRestoreWindow" )]
        internal static unsafe partial void glfwRestoreWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwMaximizeWindow" )]
        internal static unsafe partial void glfwMaximizeWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwShowWindow" )]
        internal static unsafe partial void glfwShowWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwHideWindow" )]
        internal static unsafe partial void glfwHideWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwFocusWindow" )]
        internal static unsafe partial void glfwFocusWindow( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwRequestWindowAttention" )]
        internal static unsafe partial void glfwRequestWindowAttention(
                                                    IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowMonitor" )]
        internal static unsafe partial IntPtr glfwGetWindowMonitor(
                                                      IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowMonitor" )]
        internal static unsafe partial void glfwSetWindowMonitor(
                                                    IntPtr window,
                                                    IntPtr monitor,
                                                    int xpos,
                                                    int ypos,
                                                    int width,
                                                    int height,
                                                    int refreshRate );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowAttrib" )]
        internal static unsafe partial int glfwGetWindowAttrib(
                                                   IntPtr window,
                                                   int attrib );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowAttrib" )]
        internal static unsafe partial void glfwSetWindowAttrib(
                                                    IntPtr window,
                                                    int attrib,
                                                    int value );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowUserPointer" )]
        internal static unsafe partial void glfwSetWindowUserPointer(
                                                    IntPtr window,
                                                    void* pointer );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetWindowUserPointer" )]
        internal static unsafe partial void* glfwGetWindowUserPointer(
                                                     IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowPosCallback" )]
        internal static unsafe partial GLFWwindowposfun glfwSetWindowPosCallback(
                                                                IntPtr window,
                                                                IntPtr callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowSizeCallback" )]
        internal static unsafe partial GLFWwindowsizefun glfwSetWindowSizeCallback(
                                                                 IntPtr window,
                                                                 GLFWwindowsizefun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowCloseCallback" )]
        internal static unsafe partial GLFWwindowclosefun glfwSetWindowCloseCallback(
                                                                  IntPtr window,
                                                                  GLFWwindowclosefun callback );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwSetWindowRefreshCallback" )]
        internal static unsafe partial GLFWwindowrefreshfun glfwSetWindowRefreshCallback(
                                                                    IntPtr window,
                                                                    GLFWwindowrefreshfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetWindowFocusCallback" )]
        internal static unsafe partial GLFWwindowfocusfun glfwSetWindowFocusCallback(
                                                                  IntPtr window,
                                                                  GLFWwindowfocusfun callback );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwSetWindowIconifyCallback" )]
        internal static unsafe partial GLFWwindowiconifyfun glfwSetWindowIconifyCallback(
                                                                    IntPtr window,
                                                                    GLFWwindowiconifyfun callback );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwSetWindowMaximizeCallback" )]
        internal static unsafe partial GLFWwindowmaximizefun glfwSetWindowMaximizeCallback(
                                                                     IntPtr window,
                                                                     GLFWwindowmaximizefun callback );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwSetFramebufferSizeCallback" )]
        internal static unsafe partial FrameBufferSizeHandler glfwSetFramebufferSizeCallback(
                                                                      IntPtr window,
                                                                      IntPtr callback );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwSetWindowContentScaleCallback" )]
        internal static unsafe partial GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback(
                                                                         IntPtr window,
                                                                         GLFWwindowcontentscalefun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwPollEvents" )]
        internal static unsafe partial void glfwPollEvents();

        [LibraryImport( SSC_libName, EntryPoint = "glfwWaitEvents" )]
        internal static unsafe partial void glfwWaitEvents();

        [LibraryImport( SSC_libName, EntryPoint = "glfwWaitEventsTimeout" )]
        internal static unsafe partial void glfwWaitEventsTimeout(
                                                    double timeout );

        [LibraryImport( SSC_libName, EntryPoint = "glfwPostEmptyEvent" )]
        internal static unsafe partial void glfwPostEmptyEvent();

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetInputMode" )]
        internal static unsafe partial int glfwGetInputMode(
                                                   IntPtr window,
                                                   int mode );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetInputMode" )]
        internal static unsafe partial void glfwSetInputMode(
                                                    IntPtr window,
                                                    int mode,
                                                    int value );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwRawMouseMotionSupported" )]
        internal static unsafe partial int glfwRawMouseMotionSupported();

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetKeyName" )]
        internal static unsafe partial byte* glfwGetKeyName(
                                                     int key,
                                                     int scancode );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetKeyScancode" )]
        internal static unsafe partial int glfwGetKeyScancode( int key );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetKey" )]
        internal static unsafe partial KeyStatus glfwGetKey(
                                                         IntPtr window,
                                                         int key );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetMouseButton" )]
        internal static unsafe partial int glfwGetMouseButton(
                                                   IntPtr window,
                                                   int button );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetCursorPos" )]
        internal static unsafe partial void glfwGetCursorPos(
                                                    IntPtr window,
                                                    double* xpos,
                                                    double* ypos );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetCursorPos" )]
        internal static unsafe partial void glfwSetCursorPos(
                                                    IntPtr window,
                                                    double xpos,
                                                    double ypos );

        [LibraryImport( SSC_libName, EntryPoint = "glfwCreateCursor" )]
        internal static unsafe partial GLFWcursor* glfwCreateCursor(
                                                           GLFWimage* image,
                                                           int xhot,
                                                           int yhot );

        [LibraryImport( SSC_libName, EntryPoint = "glfwCreateStandardCursor" )]
        internal static unsafe partial GLFWcursor* glfwCreateStandardCursor(
                                                           int shape );

        [LibraryImport( SSC_libName, EntryPoint = "glfwDestroyCursor" )]
        internal static unsafe partial void glfwDestroyCursor(
                                                    GLFWcursor* cursor );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetCursor" )]
        internal static unsafe partial void glfwSetCursor(
                                                    IntPtr window,
                                                    GLFWcursor* cursor );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetKeyCallback" )]
        internal static unsafe partial GLFWkeyfun glfwSetKeyCallback(
                                                          IntPtr window,
                                                          GLFWkeyfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetCharCallback" )]
        internal static unsafe partial GLFWcharfun glfwSetCharCallback(
                                                           IntPtr window,
                                                           GLFWcharfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetCharModsCallback" )]
        internal static unsafe partial GLFWcharmodsfun glfwSetCharModsCallback(
                                                               IntPtr window,
                                                               GLFWcharmodsfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetMouseButtonCallback" )]
        internal static unsafe partial GLFWmousebuttonfun glfwSetMouseButtonCallback(
                                                                  IntPtr window,
                                                                  GLFWmousebuttonfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetCursorPosCallback" )]
        internal static unsafe partial GLFWcursorposfun glfwSetCursorPosCallback(
                                                                IntPtr window,
                                                                GLFWcursorposfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetCursorEnterCallback" )]
        internal static unsafe partial GLFWcursorenterfun glfwSetCursorEnterCallback(
                                                                  IntPtr window,
                                                                  GLFWcursorenterfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetScrollCallback" )]
        internal static unsafe partial GLFWscrollfun glfwSetScrollCallback(
                                                             IntPtr window,
                                                             GLFWscrollfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetDropCallback" )]
        internal static unsafe partial GLFWdropfun glfwSetDropCallback(
                                                           IntPtr window,
                                                           GLFWdropfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwJoystickPresent" )]
        internal static unsafe partial int glfwJoystickPresent( int jid );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetJoystickAxes" )]
        internal static unsafe partial float* glfwGetJoystickAxes(
                                                      int jid,
                                                      int* count );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetJoystickButtons" )]
        internal static unsafe partial byte* glfwGetJoystickButtons(
                                                     int jid,
                                                     int* count );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetJoystickHats" )]
        internal static unsafe partial byte* glfwGetJoystickHats(
                                                     int jid,
                                                     int* count );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetJoystickName" )]
        internal static unsafe partial byte* glfwGetJoystickName( int jid );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetJoystickGUID" )]
        internal static unsafe partial byte* glfwGetJoystickGUID( int jid );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetJoystickUserPointer" )]
        internal static unsafe partial void glfwSetJoystickUserPointer(
                                                    int jid,
                                                    void* pointer );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetJoystickUserPointer" )]
        internal static unsafe partial void* glfwGetJoystickUserPointer(
                                                     int jid );

        [LibraryImport( SSC_libName, EntryPoint = "glfwJoystickIsGamepad" )]
        internal static unsafe partial int glfwJoystickIsGamepad( int jid );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetJoystickCallback" )]
        internal static unsafe partial GLFWjoystickfun glfwSetJoystickCallback(
                                                               GLFWjoystickfun callback );

        [LibraryImport( SSC_libName, EntryPoint = "glfwUpdateGamepadMappings" )]
        internal static unsafe partial int glfwUpdateGamepadMappings(
                                                   byte* str );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetGamepadName" )]
        internal static unsafe partial byte* glfwGetGamepadName( int jid );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetGamepadState" )]
        internal static unsafe partial int glfwGetGamepadState(
                                                   int jid,
                                                   GLFWgamepadstate* state );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetClipboardString" )]
        internal static unsafe partial void glfwSetClipboardString(
                                                    IntPtr window,
                                                    byte* str );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetClipboardString" )]
        internal static unsafe partial byte* glfwGetClipboardString(
                                                     IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetTime" )]
        internal static unsafe partial double glfwGetTime();

        [LibraryImport( SSC_libName, EntryPoint = "glfwSetTime" )]
        internal static unsafe partial void glfwSetTime( double time );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetTimerValue" )]
        internal static unsafe partial ulong* glfwGetTimerValue();

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetTimerFrequency" )]
        internal static unsafe partial ulong* glfwGetTimerFrequency();

        [LibraryImport( SSC_libName, EntryPoint = "glfwMakeContextCurrent" )]
        internal static unsafe partial void glfwMakeContextCurrent(
                                                    IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwGetCurrentContext" )]
        internal static unsafe partial IntPtr glfwGetCurrentContext();

        [LibraryImport( SSC_libName, EntryPoint = "glfwSwapBuffers" )]
        internal static unsafe partial void glfwSwapBuffers( IntPtr window );

        [LibraryImport( SSC_libName, EntryPoint = "glfwSwapInterval" )]
        internal static unsafe partial void glfwSwapInterval( int interval );

        [LibraryImport( SSC_libName, EntryPoint = "glfwExtensionSupported" )]
        internal static unsafe partial int glfwExtensionSupported(
                                                   byte* extension );

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwGetProcAddress",
                StringMarshalling = StringMarshalling.Utf8 )]
        internal static unsafe partial IntPtr glfwGetProcAddress(
                                                      string procname );

        [LibraryImport( SSC_libName, EntryPoint = "glfwVulkanSupported" )]
        internal static unsafe partial int glfwVulkanSupported();

        [LibraryImport(
                SSC_libName,
                EntryPoint = "glfwGetRequiredInstanceExtensions" )]
        internal static unsafe partial byte** glfwGetRequiredInstanceExtensions(
                                                      uint* count );

        //[LibraryImport(InternalIO.SSC_libname, EntryPoint = "glfwGetInstanceProcAddress")] internal unsafe partial static GLFWvkproc glfwGetInstanceProcAddress(VkInstance instance, byte* procname);
        //[LibraryImport(InternalIO.SSC_libname, EntryPoint = "glfwGetPhysicalDevicePresentationSupport")] internal unsafe partial static int glfwGetPhysicalDevicePresentationSupport(VkInstance instance, VkPhysicalDevice device, uint* queuefamily);
        //[LibraryImport(InternalIO.SSC_libname, EntryPoint = "glfwCreateWindowSurface")] internal unsafe partial static VkResult glfwCreateWindowSurface(VkInstance instance, glfwWindow* window, VkAllocationCallbacks* allocator, VkSurfaceKHR* surface);

        #endregion api call
    }
}
