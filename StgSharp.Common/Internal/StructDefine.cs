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
    // rename types in style of GLFW

    using GLFWbool = Boolean;

    #region glad struct

    [StructLayout(LayoutKind.Sequential)]
    internal struct GLsync
    {

        public IntPtr Handle;

    }

        #endregion glad struct

    // strucs defined in GLFW3.h

    #region GLFW3 struct

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwContext
    {

        private delegate*<byte*> extensionSupported;
        private delegate*<delegate*<void>, void> getProcAddress;

        /*
        void (* makeCurrent) (glfwWindow*);
        void (* swapBuffers) (glfwWindow*);
        void (* swapInterval) (int);
        int (* extensionSupported) (const byte*);
        delegate*<void>(*getProcAddress)(const byte*);
        void (* destroy) (glfwWindow*);
        */

        private delegate*<glfwWindow*> makeCurrent;
        private delegate*<glfwWindow*> swapBuffer;
        private delegate*<int> swapInterval;
        private GLFWbool forward, debug, noerror;

        private int client;
        private int major, minor, revision;
        private int profile;
        private int release;
        private int robustness;
        private int source;
        private IntPtr destroy;

        // PFNGLGETINTEGERVPROC GetIntegerv;
        private IntPtr GetIntegerv;

        // PFNGLGETSTRINGPROC GetString;
        private IntPtr GetString;

        // PFNGLGETSTRINGIPROC GetStringi;
        private IntPtr GetStringi;

        internal struct egl
        {

            private void* client;

            /*
        EGLConfig config;
        EGLContext handle;
        EGLSurface surface;
        */
            private void* config;
            private void* handle;
            private void* surface;

        }

        internal struct osmesa
        {

            private void* buffer;

            // OSMesaContext handle;
            private void* handle;
            private int height;

            private int width;

        }

        // This is defined in platform.h
        // GLFW_PLATFORM_CONTEXT_STATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwVideomode
    {
        /*! The refresh rate, in Hz, of the video mode.
         */

        private int refreshRate;
        /*! The bit depth of the blue channel of the video mode.
         */
        public int blueBits;
        /*! The bit depth of the green channel of the video mode.
         */
        public int greenBits;
        /*! The height, in screen coordinates, of the video mode.
         */
        public int height;
        /*! The bit depth of the red channel of the video mode.
         */
        public int redBits;

        /*! The width, in screen coordinates, of the video mode.
         */
        public int width;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwGammaramp
    {
        /*! An array of value describing the response of the blue channel.
         */

        private ushort* blue;
        /*! An array of value describing the response of the green channel.
         */
        private ushort* green;

        /*! An array of value describing the response of the red channel.
         */
        private ushort* red;
        /*! The number of elements in each array.
         */
        private uint size;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWimage
    {
        /*! The pixel _data of this image, arranged left-to-right, top-to-bottom.
         */

        private byte* pixels;

        /*! The height, in pixels, of this image.
         */
        private int height;
        /*! The width, in pixels, of this image.
         */
        private int width;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWgamepadstate
    {
        /*! The states of each [gamepad button](@ref gamepad_buttons), `GLFW_PRESS`
         *  or `GLFW_RELEASE`.
         */

        public fixed byte buttons[15];

        /*! The states of each [gamepad axis](@ref gamepad_axes), in the range -1.0
         *  to 1.0 inclusive.
         */
        public fixed float axes[6];

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWallocator
    {

        private void* user;

        // GLFWallocatefun allocate;
        private IntPtr allocate;

        // GLFWdeallocatefun deallocate;
        private IntPtr deallocate;

        // GLFWreallocatefun reallocate;
        private IntPtr reallocate;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct glfwMonitor
    {

        private glfwVideomode* modes;

        private glfwWindow* window;

        private fixed byte name[128];
        private glfwGammaramp currentRamp;

        private glfwGammaramp originalRamp;
        private glfwVideomode currentMode;
        private int modeCount;

        private int width, height;
        private IntPtr userPointer;

        public glfwMonitor() { }

        public unsafe string Name
        {
            get
            {
                fixed (byte* cptr = name) {
                    return Marshal.PtrToStringAnsi((IntPtr)cptr);
                }
            }
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct glfwWindow
    {

        private glfwWindow* next;
        private void* userPointer;
        private bool rawMouseMotion;
        private Callbacks callbacks;

        // Virtual cursor position when cursor is disabled
        private double virtualCursorPosX, virtualCursorPosY;
        private int autoIconify;
        private int cursorMode;
        private int decorated;
        private int doublebuffer;
        private int floating;
        private int focusOnShow;
        private int lockKeyMods;
        private int maxwidth, maxheight;

        private int minwidth, minheight;
        private int mousePassthrough;
        private int numer, denom;

        // Window settings and state
        private int resizable;
        private int shouldClose;

        private int stickyKeys;
        private int stickyMouseButtons;
        private IntPtr cursor; // _GLFWcursor* cursor;
        // glfwVideomode videoMode; // 根据GLFWvidmode的定义转换
        private IntPtr monitor; // _GLFWmonitor* monitor;
        public fixed byte keys[Constant.GLFW_KEY_LAST + 1];
        public fixed byte mouseButtons[Constant.GLFW_MOUSE_BUTTON_LAST + 1];

        // _GLFWcontext viewPortDisplay; // 根据_GLFWcontext的定义转换

        // Callbacks are represented as IntPtr since we don't need the actual function pointers in C#
        internal struct Callbacks
        {

            internal IntPtr character;
            internal IntPtr charmods;
            internal IntPtr close;
            internal IntPtr cursorEnter;
            internal IntPtr cursorPos;
            internal IntPtr drop;
            internal IntPtr fbsize;
            internal IntPtr focus;
            internal IntPtr iconify;
            internal IntPtr key;
            internal IntPtr maximize;
            internal IntPtr mouseButton;
            internal IntPtr pos;
            internal IntPtr refresh;
            internal IntPtr scale;
            internal IntPtr scroll;
            internal IntPtr size;

        }

        // GLFW_PLATFORM_WINDOW_STATE platformWindowState; 
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWwindowCallback { }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWcursor
    {

        // GLFWcursor* next;
        private IntPtr next;

        // This is defined in platform.h
        // not completed in tranformation
        // GLFW_PLATFORM_CURSOR_STATE;
    }

    /*
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWerror
    {
        //GLFWerror* next;
        IntPtr next;
        int code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)glConst.GLFW_MESSAGE_SIZE)]
        byte[] description = new byte[glConst.GLFW_MESSAGE_SIZE];

        public GLFWerror()
        {
        }
    }
    */

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWinitconfig
    {

        private GLFWbool hatButtons;

        private int angleType;
        private int platformID;

        // PFN_vkGetInstanceProcAddr vulkanLoader;
        private IntPtr vulkanLoader;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwndconfig
    {

        private byte* title;

        private GLFWbool autoIconify;
        private GLFWbool centerCursor;
        private GLFWbool decorated;
        private GLFWbool floating;
        private GLFWbool focused;
        private GLFWbool focusOnShow;
        private GLFWbool maximized;
        private GLFWbool mousePassthrough;
        private GLFWbool resizable;
        private GLFWbool scaleToMonitor;
        private GLFWbool visible;
        private int height;
        private int width;

        private int xpos;
        private int ypos;

        internal Ns ns = new Ns();

        internal Win32 win32 = new Win32();

        internal Wl wl = new Wl();

        internal X11 x11 = new X11();

        public GLFWwndconfig() { }

        internal struct Ns
        {

            private GLFWbool retina = false;
            public fixed byte frameName[256];

            public Ns() { }

        }

        internal struct X11
        {

            public fixed byte className[256];
            public fixed byte instanceName[256];

            public X11() { }

        }

        internal struct Win32
        {

            private GLFWbool keymenu;

        }

        internal struct Wl
        {

            public fixed byte appId[256];

            public Wl() { }

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWctxconfig
    {

        private GLFWbool debug;
        private GLFWbool forward;
        private GLFWbool noerror;

        private int client;
        private int major;
        private int minor;
        private int profile;
        private int release;
        private int robustness;
        private int source;

        // glfwWindow* share;
        private IntPtr share;

        internal Nsgl nsgl = new Nsgl();

        public GLFWctxconfig() { }

        internal struct Nsgl
        {

            private GLFWbool offline;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWfbconfig
    {

        private GLFWbool doublebuffer;
        private GLFWbool sRGB;
        private GLFWbool stereo;
        private GLFWbool transparent;

        private int accumAlphaBits;
        private int accumBlueBits;
        private int accumGreenBits;
        private int accumRedBits;
        private int alphaBits;
        private int auxBuffers;
        private int blueBits;
        private int depthBits;
        private int greenBits;

        private int redBits;
        private int samples;
        private int stencilBits;
        private uintptr_t handle;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GLFWlibrary
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.GLFW_JOYSTICK_LAST + 1)]
        private GLFWjoystick[] joysticks = new GLFWjoystick[Constant.GLFW_JOYSTICK_LAST + 1];

        private GLFWallocator allocator;

        private GLFWbool initialized;

        private GLFWbool joysticksInitialized;
        private GLFWls contextSlot;

        private GLFWls errorSlot;
        private GLFWmutex errorLock;

        private GLFWplatform platform;

        private int mappingCount;

        private int monitorCount;

        // GLFWcursor* cursorListHead;
        private IntPtr cursorListHead;

        // GLFWerror* errorListHead;
        private IntPtr errorListHead;

        // GLFWmapping* mappings;
        private IntPtr mappings;

        // GLFWmonitor** monitors;
        private IntPtr monitors;

        // glfwWindow* windowListHead;
        private IntPtr windowListHead;

        internal Callbacks callbacks = new Callbacks();

        internal Egl egl = new Egl();

        internal Hints hints = new Hints();

        internal Osmesa osmesa = new Osmesa();

        public GLFWlibrary() { }

        internal unsafe struct Hints
        {

            private GLFWctxconfig context;
            private GLFWfbconfig framebuffer;

            private GLFWinitconfig init;
            private GLFWwndconfig window;
            private int refreshRate;

        }

        internal unsafe struct timer
        {

            private ulong offset;

            // This is defined in platform.h

            // not transformed function defination
            // GLFW_PLATFORM_LIBRARY_TIMER_STATE
        }

        internal unsafe struct Egl
        {

            internal void* display;

            internal void* handle;

            internal bool ANGLE_platform_angle;
            internal bool ANGLE_platform_angle_d3d;
            internal bool ANGLE_platform_angle_metal;
            internal bool ANGLE_platform_angle_opengl;
            internal bool ANGLE_platform_angle_vulkan;
            internal bool EXT_client_extensions;
            internal bool EXT_platform_base;
            internal bool EXT_platform_wayland;
            internal bool EXT_platform_x11;
            internal bool EXT_present_opaque;
            internal bool KHR_colorSpace;
            internal bool KHR_context_flush_control;

            internal bool KHR_create_context;
            internal bool KHR_create_context_no_error;
            internal bool KHR_get_all_proc_addresses;
            internal bool prefix;
            internal int major, minor;

            internal int platform;
            internal IntPtr BindAPI;
            internal IntPtr CreateContext;
            internal IntPtr CreatePlatformWindowSurfaceEXT;
            internal IntPtr CreateWindowSurface;
            internal IntPtr DestroyContext;
            internal IntPtr DestroySurface;

            internal IntPtr GetConfigAttrib;
            internal IntPtr GetConfigs;
            internal IntPtr GetDisplay;
            internal IntPtr GetError;

            internal IntPtr GetPlatformDisplayEXT;
            internal IntPtr GetProcAddress;
            internal IntPtr Initialize;
            internal IntPtr MakeCurrent;
            internal IntPtr QueryString;
            internal IntPtr SwapBuffers;
            internal IntPtr SwapInterval;
            internal IntPtr Terminate;

        }

        internal unsafe struct Osmesa
        {

            private void* handle;

            private IntPtr CreateContextAttribs;

            private IntPtr CreateContextExt;
            private IntPtr DestroyContext;
            private IntPtr GetColorBuffer;
            private IntPtr GetDepthBuffer;
            private IntPtr GetProcAddress;
            private IntPtr MakeCurrent;

        }

        internal unsafe struct vk
        {

            private void* handle;

            private GLFWbool available;
            private GLFWbool EXT_metal_surface;
            private GLFWbool KHR_surface;
            private GLFWbool KHR_wayland_surface;
            private GLFWbool KHR_win32_surface;
            private GLFWbool KHR_xcb_surface;
            private GLFWbool KHR_xlib_surface;
            private GLFWbool MVK_macos_surface;
            private IntPtr GetInstanceProcAddr;
            private fixed ulong extensions[2];

            public vk() { }

        }

        internal unsafe struct Callbacks
        {

            private IntPtr joystick;

            private IntPtr monitor;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapelement
    {

        private byte index;

        private byte type;

        private sbyte axisOffset;
        private sbyte axisScale;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapping
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        private GLFWmapelement[] axes = new GLFWmapelement[6];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        private GLFWmapelement[] buttons = new GLFWmapelement[15];
        private fixed byte guid[33];

        private fixed byte name[128];

        public GLFWmapping() { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWjoystick
    {

        private float* axes;
        private byte* buttons;
        private byte* hats;
        private void* userPointer;
        private fixed byte guid[33];
        private fixed byte name[128];

        private GLFWbool allocated;
        private GLFWbool connected;
        private int axisCount;
        private int buttonCount;
        private int hatCount;

        // GLFWmapping* mapping;
        private IntPtr mapping;

        // This is defined in platform.h
        // not completed in transforming
        // GLFW_PLATFORM_JOYSTICK_STATE;

        public GLFWjoystick() { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWls
    {

        // This is defined in platform.h

        // GLFW_PLATFORM_TLS_STATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWplatform
    {

        private int platformID;

        // init
        // GLFWbool(*init)(void);
        private IntPtr init;

        // void (* terminate) (void);
        private IntPtr terminate;
        internal IntPtr createCursor;
        internal IntPtr createStandardCursor;

        // window
        internal IntPtr createWindow;
        internal IntPtr destroyCursor;

        internal IntPtr destroyWindow;
        internal IntPtr focusWindow;
        internal IntPtr framebufferTransparent;

        // monitor
        internal IntPtr freeMonitor;
        internal IntPtr getClipboardString;

        internal IntPtr getEGLNativeDisplay;
        internal IntPtr getEGLNativeWindow;

        // EGL
        // EGLenum(*getEGLPlatform)(EGLint**);
        // EGLNativeDisplayType(*getEGLNativeDisplay)(void);
        // EGLNativeWindowType(*getEGLNativeWindow)(glfwWindow*);
        internal IntPtr getEGLPlatform;
        internal IntPtr getFramebufferSize;
        internal IntPtr getGammaRamp;
        internal IntPtr getKeyScancode;
        internal IntPtr getMappingName;
        internal IntPtr getMonitorContentScale;

        internal IntPtr getMonitorPos;
        internal IntPtr getMonitorWorkarea;

        // vulkan
        // void (* getRequiredInstanceExtensions) (byte**);
        // GLFWbool(*getPhysicalDevicePresentationSupport)(VkInstance, VkPhysicalDevice, uint32_t);
        // VkResult(*createWindowSurface)(VkInstance, glfwWindow*,const VkAllocationCallbacks*, VkSurfaceKHR*);
        internal IntPtr getRequiredInstanceExtensions;
        internal IntPtr getScancodeName;
        internal IntPtr getVideoMode;
        internal IntPtr getVideoModes;
        internal IntPtr getWindowContentScale;
        internal IntPtr getWindowFrameSize;
        internal IntPtr getWindowOpacity;
        internal IntPtr getWindowPos;
        internal IntPtr getWindowSize;
        internal IntPtr GLFWwindow;
        internal IntPtr hideWindow;
        internal IntPtr iconifyWindow;
        internal IntPtr initJoysticks;
        internal IntPtr maximizeWindow;
        internal IntPtr pollEvents;
        internal IntPtr pollJoystick;
        internal IntPtr postEmptyEvent;
        internal IntPtr rawMouseMotionSupported;
        internal IntPtr requestWindowAttention;
        internal IntPtr restoreWindow;
        internal IntPtr setClipboardString;
        internal IntPtr setCursor;
        internal IntPtr setCursorMode;

        #region input

        /* void (* getCursorPos) (glfwWindow*, double*, double*);
        void (* setCursorPos) (glfwWindow*, double, double);
        void (* setCursorMode) (glfwWindow*, int);
        void (* setRawMouseMotion) (glfwWindow*, GLFWbool);
        GLFWbool(*rawMouseMotionSupported)(void);
        GLFWbool(*createCursor)(GLFWcursor*,const GLFWimage*,int,int);
        GLFWbool(*createStandardCursor)(GLFWcursor*, int);
        void (* destroyCursor) (GLFWcursor*);
        void (* setCursor) (glfwWindow*, GLFWcursor*);
        const byte* (*getScancodeName)(int);
        int (* getKeyScancode) (int);
        void (* setClipboardString) (const byte*);
            const byte* (*getClipboardString)(void);
        GLFWbool(*initJoysticks)(void);
        void (* terminateJoysticks) (void);
        GLFWbool(*pollJoystick)(GLFWjoystick*, int);
        const byte* (*getMappingName)(void);
        void (* updateGamepadGUID) (byte*);
        // monitor
        void (* freeMonitor) (GLFWmonitor*);
        void (* getMonitorPos) (GLFWmonitor*, int*, int*);
        void (* getMonitorContentScale) (GLFWmonitor*, float*, float*);
        void (* getMonitorWorkarea) (GLFWmonitor*, int*, int*, int*, int*);
        glfwVideomode* (* getVideoModes) (GLFWmonitor*, int*);
        void (* getVideoMode) (GLFWmonitor*, glfwVideomode*);
        GLFWbool(*getGammaRamp)(GLFWmonitor*, glfwGammaramp*);
        void (* setGammaRamp) (GLFWmonitor*,const glfwGammaramp*);
        // window
        GLFWbool(*createWindow)(glfwWindow*,const GLFWwndconfig*,const GLFWctxconfig*,const GLFWfbconfig*);
        void (* destroyWindow) (glfwWindow*);
        void (* setWindowTitle) (glfwWindow*,const byte*);
        void (* setWindowIcon) (glfwWindow*, int,const GLFWimage*);
        void (* getWindowPos) (glfwWindow*, int*, int*);
        void (* setWindowPos) (glfwWindow*, int, int);
        void (* getWindowSize) (glfwWindow*, int*, int*);
        void (* setWindowSize) (glfwWindow*, int, int);
        void (* setWindowSizeLimits) (glfwWindow*, int, int, int, int);
        void (* setWindowAspectRatio) (glfwWindow*, int, int);
        void (* getFramebufferSize) (glfwWindow*, int*, int*);
        void (* getWindowFrameSize) (glfwWindow*, int*, int*, int*, int*);
        void (* getWindowContentScale) (glfwWindow*, float*, float*);
        void (* iconifyWindow) (glfwWindow*);
        void (* restoreWindow) (glfwWindow*);
        void (* maximizeWindow) (glfwWindow*);
        void (* showWindow) (glfwWindow*);
        void (* hideWindow) (glfwWindow*);
        void (* requestWindowAttention) (glfwWindow*);
        void (* focusWindow) (glfwWindow*);
        void (* setWindowMonitor) (glfwWindow*, GLFWmonitor*, int, int, int, int, int);
        GLFWbool(*windowFocused)(glfwWindow*);
        GLFWbool(*windowIconified)(glfwWindow*);
        GLFWbool(*windowVisible)(glfwWindow*);
        GLFWbool(*windowMaximized)(glfwWindow*);
        GLFWbool(*windowHovered)(glfwWindow*);
        GLFWbool(*framebufferTransparent)(glfwWindow*);
        float (* getWindowOpacity) (glfwWindow*);
        void (* setWindowResizable) (glfwWindow*, GLFWbool);
        void (* setWindowDecorated) (glfwWindow*, GLFWbool);
        void (* setWindowFloating) (glfwWindow*, GLFWbool);
        void (* setWindowOpacity) (glfwWindow*, float);
        void (* setWindowMousePassthrough) (glfwWindow*, GLFWbool);
        void (* pollEvents) (void);
        void (* waitEvents) (void);
        void (* waitEventsTimeout) (double);
        void (* postEmptyEvent) (void);
        */

        #endregion

        internal IntPtr setCursorPos;
        internal IntPtr setGammaRamp;
        internal IntPtr setRawMouseMotion;
        internal IntPtr setWindowAspectRatio;
        internal IntPtr setWindowDecorated;
        internal IntPtr setWindowFloating;
        internal IntPtr setWindowIcon;
        internal IntPtr setWindowMonitor;
        internal IntPtr setWindowMousePassthrough;
        internal IntPtr setWindowOpacity;
        internal IntPtr setWindowPos;
        internal IntPtr setWindowResizable;
        internal IntPtr setWindowSize;
        internal IntPtr setWindowSizeLimits;
        internal IntPtr setWindowTitle;
        internal IntPtr showWindow;
        internal IntPtr terminateJoysticks;
        internal IntPtr updateGamepadGUID;

        internal IntPtr VkPhysicalDevice;
        internal IntPtr waitEvents;
        internal IntPtr waitEventsTimeout;
        internal IntPtr windowFocused;
        internal IntPtr windowHovered;
        internal IntPtr windowIconified;
        internal IntPtr windowMaximized;
        internal IntPtr windowVisible;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmutex
    {

        // This is defined in platform.h
        // GLFW_PLATFORM_MUTEX_STATE
    }

    // public unsafe struct VkAllocationCallbacks { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct VkExtensionProperties
    {

        private fixed byte extensionName[256];
        private uint specVersion;

        public VkExtensionProperties() { }

    }

    /*
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GLFWerror
    {
        IntPtr next;
        int code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        byte[] description = new byte[1024];

        public GLFWerror()
        {
        }
    }
    */

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct uintptr_t
    {

        [FieldOffset(0)]
        private long part1;

        [FieldOffset(8)]
        private long part2;

    }

    #endregion GLFW3 struct

}

/**/