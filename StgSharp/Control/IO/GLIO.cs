using StgSharp.Controlling;
using StgSharp.Graphics;
using System;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal static partial class internalIO
    {

        [DllImport(SSGC_libname, EntryPoint = "initGL",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern unsafe void InternalInitGL(int majorVersion, int minorVersion);

        [DllImport(SSGC_libname, EntryPoint = "initGLAD",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern unsafe int InternalInitGLAD();


        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowUserPointer",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void* glfwGetWindowUserPointer(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void* glfwGetMonitorUserPointer(GLFWmonitor* monitor);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void* glfwGetJoystickUserPointer(int jid);

        [DllImport(SSGC_libname, EntryPoint = "glfwWindowHintString", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwWindowHintString(int hint, byte* value);

        [DllImport(SSGC_libname, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwWindowHint(int hint, int value);

        [DllImport(SSGC_libname, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwWaitEventsTimeout(double timeout);

        [DllImport(SSGC_libname, EntryPoint = "glfwWaitEvents", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwWaitEvents();

        [DllImport(SSGC_libname, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwTerminate();

        [DllImport(SSGC_libname, EntryPoint = "glfwSwapInterval", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSwapInterval(int interval);

        //window is the pointer to a glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSwapBuffers(IntPtr window);

        [DllImport(SSGC_libname, EntryPoint = "glfwShowWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwShowWindow(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowUserPointer(GLFWwindow* window, void* pointer);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowTitle(GLFWwindow* window, byte* title);

        //window is the pointer of glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowSizeLimits", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowSizeLimits(IntPtr window, int minwidth, int minheight, int maxwidth, int maxheight);

        //window is the pointer of glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowSize(IntPtr window, int width, int height);

        //window is the pointer of glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowShouldClose(IntPtr window, int value);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowPos(GLFWwindow* window, int xpos, int ypos);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowOpacity(GLFWwindow* window, float opacity);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowMonitor(GLFWwindow* window, GLFWmonitor* monitor, int xpos, int ypos, int width, int height, int refreshRate);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowIcon", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowIcon(GLFWwindow* window, int count, GLFWimage* images);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowAttrib(GLFWwindow* window, int attrib, int value);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowAspectRatio", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetWindowAspectRatio(GLFWwindow* window, int numer, int denom);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetTime", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetTime(double time);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetMonitorUserPointer(GLFWmonitor* monitor, void* pointer);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetJoystickUserPointer(int jid, void* pointer);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetInputMode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetInputMode(GLFWwindow* window, int mode, int value);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetGammaRamp(GLFWmonitor* monitor, GLFWgammaramp* ramp);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetGamma", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetGamma(GLFWmonitor* monitor, float gamma);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetCursorPos(GLFWwindow* window, double xpos, double ypos);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetCursor", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetCursor(GLFWwindow* window, GLFWcursor* cursor);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetClipboardString", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwSetClipboardString(GLFWwindow* window, byte* str);

        [DllImport(SSGC_libname, EntryPoint = "glfwRestoreWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwRestoreWindow(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwRequestWindowAttention", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwRequestWindowAttention(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwPostEmptyEvent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwPostEmptyEvent();

        [DllImport(SSGC_libname, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwPollEvents();

        [DllImport(SSGC_libname, EntryPoint = "glfwMaximizeWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwMaximizeWindow(GLFWwindow* window);

        //window is the pointer to a glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwMakeContextCurrent(IntPtr window);

        [DllImport(SSGC_libname, EntryPoint = "glfwInitHint", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwInitHint(int hint, int value);

        [DllImport(SSGC_libname, EntryPoint = "glfwIconifyWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwIconifyWindow(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwHideWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwHideWindow(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetWindowSize(GLFWwindow* window, int* width, int* height);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetWindowPos(GLFWwindow* window, int* xpos, int* ypos);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowFrameSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetWindowFrameSize(GLFWwindow* window, int* left, int* top, int* right, int* bottom);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowContentScale", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetWindowContentScale(GLFWwindow* window, float* xscale, float* yscale);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetVersion", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetVersion(int* major, int* minor, int* rev);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMonitorWorkarea", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetMonitorWorkarea(GLFWmonitor* monitor, int* xpos, int* ypos, int* width, int* height);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMonitorPos", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetMonitorPos(GLFWmonitor* monitor, int* xpos, int* ypos);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMonitorPhysicalSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetMonitorPhysicalSize(GLFWmonitor* monitor, int* widthMM, int* heightMM);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMonitorContentScale", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetMonitorContentScale(GLFWmonitor* monitor, float* xscale, float* yscale);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetFramebufferSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetFramebufferSize(GLFWwindow* window, int* width, int* height);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwGetCursorPos(GLFWwindow* window, double* xpos, double* ypos);

        [DllImport(SSGC_libname, EntryPoint = "glfwFocusWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwFocusWindow(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwDestroyWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwDestroyWindow(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwDestroyCursor", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwDestroyCursor(GLFWcursor* cursor);

        [DllImport(SSGC_libname, EntryPoint = "glfwDefaultWindowHints", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void glfwDefaultWindowHints();

        [DllImport(SSGC_libname, EntryPoint = "glfwGetTimerValue", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe ulong glfwGetTimerValue();

        [DllImport(SSGC_libname, EntryPoint = "glfwGetTimerFrequency", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe ulong glfwGetTimerFrequency();

        //window is the pointer to a glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwWindowShouldClose(IntPtr window);

        [DllImport(SSGC_libname, EntryPoint = "glfwVulkanSupported", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwVulkanSupported();

        [DllImport(SSGC_libname, EntryPoint = "glfwUpdateGamepadMappings", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwUpdateGamepadMappings(byte* str);

        [DllImport(SSGC_libname, EntryPoint = "glfwRawMouseMotionSupported", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwRawMouseMotionSupported();

        [DllImport(SSGC_libname, EntryPoint = "glfwJoystickPresent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwJoystickPresent(int jid);

        [DllImport(SSGC_libname, EntryPoint = "glfwJoystickIsGamepad", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwJoystickIsGamepad(int jid);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwGetWindowAttrib(GLFWwindow* window, int attrib);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMouseButton", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwGetMouseButton(GLFWwindow* window, int button);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetKeyScancode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwGetKeyScancode(int key);


        //window is the pointer to GLFWwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwGetKey", CallingConvention = CallingConvention.Cdecl)]
        internal static extern KeyStatus glfwGetKey(IntPtr window, int key);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetInputMode", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwGetInputMode(GLFWwindow* window, int mode);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetGamepadState", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwGetGamepadState(int jid, GLFWgamepadstate* state);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetError", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwGetError(byte** description);

        [DllImport(SSGC_libname, EntryPoint = "glfwExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe int glfwExtensionSupported(byte* extension);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowsizefun glfwSetWindowSizeCallback(GLFWwindow* window, GLFWwindowsizefun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowRefreshCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowrefreshfun glfwSetWindowRefreshCallback(GLFWwindow* window, GLFWwindowrefreshfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowPosCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowposfun glfwSetWindowPosCallback(GLFWwindow* window, GLFWwindowposfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowMaximizeCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowmaximizefun glfwSetWindowMaximizeCallback(GLFWwindow* window, GLFWwindowmaximizefun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowIconifyCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowiconifyfun glfwSetWindowIconifyCallback(GLFWwindow* window, GLFWwindowiconifyfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowFocusCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowfocusfun glfwSetWindowFocusCallback(GLFWwindow* window, GLFWwindowfocusfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowContentScaleCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback(GLFWwindow* window, GLFWwindowcontentscalefun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindowclosefun glfwSetWindowCloseCallback(GLFWwindow* window, GLFWwindowclosefun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWwindow* glfwGetCurrentContext();

        //monitor and share are pointers to instance of glfwmonitor and glfwwindow
        //the method returns a pointer to glfw window instance
        [DllImport(SSGC_libname, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe IntPtr glfwCreateWindow(
            int width, int height, byte[] title, IntPtr monitor, IntPtr share);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetScrollCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWscrollfun glfwSetScrollCallback(GLFWwindow* window, GLFWscrollfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetMouseButtonCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWmousebuttonfun glfwSetMouseButtonCallback(GLFWwindow* window, GLFWmousebuttonfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetMonitorCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWmonitorfun glfwSetMonitorCallback(GLFWmonitorfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetMonitors",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWmonitor** glfwGetMonitors(int* count);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowMonitor",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWmonitor* glfwGetWindowMonitor(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetPrimaryMonitor",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWmonitor* glfwGetPrimaryMonitor();

        [DllImport(SSGC_libname, EntryPoint = "glfwSetKeyCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWkeyfun glfwSetKeyCallback(GLFWwindow* window, GLFWkeyfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetJoystickCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWjoystickfun glfwSetJoystickCallback(GLFWjoystickfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetProcAddress",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern unsafe IntPtr glfwGetProcAddress(string procName);

        //window is the pointer to a glfwwindow instance
        [DllImport(SSGC_libname, EntryPoint = "glfwSetFramebufferSizeCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWframebuffersizefun glfwSetFramebufferSizeCallback(IntPtr window, GLFWframebuffersizefun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetErrorCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWerrorfun glfwSetErrorCallback(GLFWerrorfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetDropCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWdropfun glfwSetDropCallback(GLFWwindow* window, GLFWdropfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetCursorPosCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWcursorposfun glfwSetCursorPosCallback(GLFWwindow* window, GLFWcursorposfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetCursorEnterCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWcursorenterfun glfwSetCursorEnterCallback(GLFWwindow* window, GLFWcursorenterfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwCreateStandardCursor",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWcursor* glfwCreateStandardCursor(int shape);

        [DllImport(SSGC_libname, EntryPoint = "glfwCreateCursor",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWcursor* glfwCreateCursor(GLFWimage* image, int xhot, int yhot);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetCharModsCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWcharmodsfun glfwSetCharModsCallback(GLFWwindow* window, GLFWcharmodsfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwSetCharCallback",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWcharfun glfwSetCharCallback(GLFWwindow* window, GLFWcharfun callback);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetWindowOpacity",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe float glfwGetWindowOpacity(GLFWwindow* window);

        [DllImport(SSGC_libname, EntryPoint = "glfwGetTime",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe double glfwGetTime();

        [DllImport(SSGC_libname, EntryPoint = "unsigned", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetJoystickHats(int jid, int* count);

        [DllImport(SSGC_libname, EntryPoint = "unsigned", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetJoystickButtons(int jid, int* count);

        [DllImport(SSGC_libname, EntryPoint = "GLFWvidmode*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWvidmode* glfwGetVideoModes(GLFWmonitor* monitor, int* count);

        [DllImport(SSGC_libname, EntryPoint = "GLFWvidmode*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWvidmode* glfwGetVideoMode(GLFWmonitor* monitor);

        [DllImport(SSGC_libname, EntryPoint = "GLFWgammaramp*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe GLFWgammaramp* glfwGetGammaRamp(GLFWmonitor* monitor);

        [DllImport(SSGC_libname, EntryPoint = "float*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe float* glfwGetJoystickAxes(int jid, int* count);

        [DllImport(SSGC_libname, EntryPoint = "byte**", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte** glfwGetRequiredInstanceExtensions(uint* count);

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetVersionString();

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetMonitorName(GLFWmonitor* monitor);

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetKeyName(int key, int scancode);

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetJoystickName(int jid);

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetJoystickGUID(int jid);

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetGamepadName(int jid);

        [DllImport(SSGC_libname, EntryPoint = "byte*", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* glfwGetClipboardString(GLFWwindow* window);



        //
    }
}
