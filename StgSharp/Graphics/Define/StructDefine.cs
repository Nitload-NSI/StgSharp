//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StructDefine.cs"
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
using System;
using System.Runtime.InteropServices;

/**/

namespace StgSharp.Graphics
{
    //rename types in style of GLFW

    using GLFWbool = Boolean;

    //strucs defined in GLFW3.h

    #region GLFW3 struct

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWcontext
    {

        private int client;
        private int source;
        private int major, minor, revision;
        private GLFWbool forward, debug, noerror;
        private int profile;
        private int robustness;
        private int release;

        //PFNGLGETSTRINGIPROC GetStringi;
        private IntPtr GetStringi;

        //PFNGLGETINTEGERVPROC GetIntegerv;
        private IntPtr GetIntegerv;

        //PFNGLGETSTRINGPROC GetString;
        private IntPtr GetString;

        /*
        void (* makeCurrent) (GLFWwindow*);
        void (* swapBuffers) (GLFWwindow*);
        void (* swapInterval) (int);
        int (* extensionSupported) (const byte*);
        GLFWglproc(*getProcAddress)(const byte*);
        void (* destroy) (GLFWwindow*);
        */

        private delegate*<GLFWwindow*> makeCurrent;
        private delegate*<GLFWwindow*> swapBuffer;
        private delegate*<int> swapInterval;
        private delegate*<byte*> extensionSupported;
        private delegate*<delegate*<void>, void> getProcAddress;
        private IntPtr destroy;

        internal struct egl
        {

            /*
        EGLConfig config;
        EGLContext handle;
        EGLSurface surface;
        */
            private void* config;
            private void* handle;
            private void* surface;
            private void* client;

        }

        internal struct osmesa
        {

            //OSMesaContext handle;
            private void* handle;

            private int width;
            private int height;
            private void* buffer;

        }

        // This is defined in platform.h
        //GLFW_PLATFORM_CONTEXT_STATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWvidmode
    {

        /*! The width, in screen coordinates, of the video mode.
         */
        public int width;
        /*! The height, in screen coordinates, of the video mode.
         */
        public int height;
        /*! The bit depth of the red channel of the video mode.
         */
        public int redBits;
        /*! The bit depth of the green channel of the video mode.
         */
        public int greenBits;
        /*! The bit depth of the blue channel of the video mode.
         */
        public int blueBits;
        /*! The refresh rate, in Hz, of the video mode.
         */
        private int refreshRate;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWgammaramp
    {

        /*! An array of value describing the response of the red channel.
         */
        private ushort* red;
        /*! An array of value describing the response of the green channel.
         */
        private ushort* green;
        /*! An array of value describing the response of the blue channel.
         */
        private ushort* blue;
        /*! The number of elements in each array.
         */
        private uint size;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWimage
    {

        /*! The width, in pixels, of this image.
         */
        private int width;
        /*! The height, in pixels, of this image.
         */
        private int height;
        /*! The pixel data of this image, arranged left-to-right, top-to-bottom.
         */
        private byte* pixels;

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

        //GLFWallocatefun allocate;
        private IntPtr allocate;

        //GLFWreallocatefun reallocate;
        private IntPtr reallocate;

        //GLFWdeallocatefun deallocate;
        private IntPtr deallocate;

        private void* user;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmonitor
    {

        private byte[] name = new byte[128];
        private void* userPointer;

        // Physical dimensions in millimeters.
        private int widthMM, heightMM;

        // The window whose video mode is current on this monitor
        //GLFWwindow* window;
        private IntPtr window;

        //GLFWvidmode* modes;
        private IntPtr modes;

        private int modeCount;
        private GLFWvidmode currentMode;

        private GLFWgammaramp originalRamp;
        private GLFWgammaramp currentRamp;

        // not transformed
        //GLFW_PLATFORM_MONITOR_STATE;

        public GLFWmonitor()
        { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwindow
    {

        //struct GLFWwindow* next in GLFW
        public GLFWwindow* next;

        private int resizable;
        private int decorated;
        private int autoIconify;
        private int floating;
        private int focusOnShow;
        private int mousePassthrough;
        internal int shouldClose;
        private void* userPointer = (void*)0;
        private bool doubleBuffer;
        private GLFWvidmode videoMode = default;
        private IntPtr monitor = IntPtr.Zero;   //public GLFWmonitor* monitor;
        private GLFWcursor* cursor = (GLFWcursor*)0;

        private int minWidth = 0, minHeight = 0;
        private int maxWidth = 0, maxHeight = 0;
        private int numer = 0, denom = 0;
        private int stickyKeys;
        private int stickyMouseButtons;
        private int lockKeyMods;
        private int cursorMode = 0;
        private fixed byte mouseButtons[8];
        private fixed byte keys[349];

        // Virtual cursor position when cursor is disabled
        public double virtualCursorPosX = 0, virtualCursorPosY = 0;
        private int rawMouseMotion;

        public GLFWcontext context = default;

        private fixed long callBacks[17];

        public GLFWwindow()
        {
        }

        public bool AutoIconify
        {
            get => autoIconify != 0;
            set => autoIconify = value ? 1 : 0;
        }
        public bool Decorated
        {
            get => decorated != 0;
            set => decorated = value ? 1 : 0;
        }
        public bool Floating
        {
            get => floating != 0;
            set => floating = value ? 1 : 0;
        }
        public bool FocusOnShow
        {
            get => focusOnShow != 0;
            set => focusOnShow = value ? 1 : 0;
        }
        public bool LockKeyMods
        {
            get => lockKeyMods != 0;
            set => lockKeyMods = value ? 1 : 0;
        }
        public bool MousePassThrough
        {
            get => mousePassthrough != 0;
            set => mousePassthrough = value ? 1 : 0;
        }
        public bool RawMouseMotion { get => rawMouseMotion != 0; set => rawMouseMotion = value ? 1 : 0; }
        public bool Resizable
        {
            get => resizable != 0;
            set => resizable = value ? 1 : 0;
        }
        public bool ShouldClose
        {
            get => shouldClose != 0;
            set => shouldClose = value ? 1 : 0;
        }

        public bool StickyKey
        {
            get => stickyKeys != 0;
            set => stickyKeys = value ? 1 : 0;
        }
        public bool StickyMouseButtons
        {
            get => stickyMouseButtons != 0;
            set => stickyMouseButtons = value ? 1 : 0;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWwindowCallback
    {

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWcursor
    {

        //GLFWcursor* next;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)GLconst.GLFW_MESSAGE_SIZE)]
        byte[] description = new byte[GLconst.GLFW_MESSAGE_SIZE];

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

        //PFN_vkGetInstanceProcAddr vulkanLoader;
        private IntPtr vulkanLoader;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwndconfig
    {

        private int xpos;
        private int ypos;
        private int width;
        private int height;
        private byte* title;
        private GLFWbool resizable;
        private GLFWbool visible;
        private GLFWbool decorated;
        private GLFWbool focused;
        private GLFWbool autoIconify;
        private GLFWbool floating;
        private GLFWbool maximized;
        private GLFWbool centerCursor;
        private GLFWbool focusOnShow;
        private GLFWbool mousePassthrough;
        private GLFWbool scaleToMonitor;

        internal Ns ns = new Ns();

        internal X11 x11 = new X11();

        internal Win32 win32 = new Win32();

        internal Wl wl = new Wl();

        public GLFWwndconfig()
        {
        }

        internal struct Ns
        {

            private GLFWbool retina = false;
            public fixed byte frameName[256];

            public Ns()
            {
            }

        }

        internal struct X11
        {

            public fixed byte className[256];
            public fixed byte instanceName[256];

            public X11()
            {
            }

        }

        internal struct Win32
        {

            private GLFWbool keymenu;

        }

        internal struct Wl
        {

            public fixed byte appId[256];

            public Wl()
            {
            }

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWctxconfig
    {

        private int client;
        private int source;
        private int major;
        private int minor;
        private GLFWbool forward;
        private GLFWbool debug;
        private GLFWbool noerror;
        private int profile;
        private int robustness;
        private int release;

        //GLFWwindow* share;
        private IntPtr share;

        internal Nsgl nsgl = new Nsgl();

        public GLFWctxconfig()
        {
        }

        internal struct Nsgl
        {

            private GLFWbool offline;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWfbconfig
    {

        private int redBits;
        private int greenBits;
        private int blueBits;
        private int alphaBits;
        private int depthBits;
        private int stencilBits;
        private int accumRedBits;
        private int accumGreenBits;
        private int accumBlueBits;
        private int accumAlphaBits;
        private int auxBuffers;
        private GLFWbool stereo;
        private int samples;
        private GLFWbool sRGB;
        private GLFWbool doublebuffer;
        private GLFWbool transparent;
        private uintptr_t handle;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GLFWlibrary
    {

        private GLFWbool initialized;
        private GLFWallocator allocator;

        private GLFWplatform platform;

        internal Hints hints = new Hints();

        //GLFWerror* errorListHead;
        private IntPtr errorListHead;

        //GLFWcursor* cursorListHead;
        private IntPtr cursorListHead;

        //GLFWwindow* windowListHead;
        private IntPtr windowListHead;

        //GLFWmonitor** monitors;
        private IntPtr monitors;

        private int monitorCount;

        private GLFWbool joysticksInitialized;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GLconst.GLFW_JOYSTICK_LAST + 1)]
        private GLFWjoystick[] joysticks = new GLFWjoystick[GLconst.GLFW_JOYSTICK_LAST + 1];

        //GLFWmapping* mappings;
        private IntPtr mappings;

        private int mappingCount;

        private GLFWls errorSlot;
        private GLFWls contextSlot;
        private GLFWmutex errorLock;

        internal Egl egl = new Egl();

        internal Osmesa osmesa = new Osmesa();

        internal Callbacks callbacks = new Callbacks();

        public GLFWlibrary()
        {
        }

        internal unsafe struct Hints
        {

            private GLFWinitconfig init;
            private GLFWfbconfig framebuffer;
            private GLFWwndconfig window;
            private GLFWctxconfig context;
            private int refreshRate;

        }

        internal unsafe struct timer
        {

            private ulong offset;
            // This is defined in platform.h

            //not transformed function defination
            //GLFW_PLATFORM_LIBRARY_TIMER_STATE
        }

        internal unsafe struct Egl
        {

            internal int platform;
            internal void* display;
            internal int major, minor;
            internal bool prefix;

            internal bool KHR_create_context;
            internal bool KHR_create_context_no_error;
            internal bool KHR_colorSpace;
            internal bool KHR_get_all_proc_addresses;
            internal bool KHR_context_flush_control;
            internal bool EXT_client_extensions;
            internal bool EXT_platform_base;
            internal bool EXT_platform_x11;
            internal bool EXT_platform_wayland;
            internal bool EXT_present_opaque;
            internal bool ANGLE_platform_angle;
            internal bool ANGLE_platform_angle_opengl;
            internal bool ANGLE_platform_angle_d3d;
            internal bool ANGLE_platform_angle_vulkan;
            internal bool ANGLE_platform_angle_metal;

            internal void* handle;

            internal IntPtr GetConfigAttrib;
            internal IntPtr GetConfigs;
            internal IntPtr GetDisplay;
            internal IntPtr GetError;
            internal IntPtr Initialize;
            internal IntPtr Terminate;
            internal IntPtr BindAPI;
            internal IntPtr CreateContext;
            internal IntPtr DestroySurface;
            internal IntPtr DestroyContext;
            internal IntPtr CreateWindowSurface;
            internal IntPtr MakeCurrent;
            internal IntPtr SwapBuffers;
            internal IntPtr SwapInterval;
            internal IntPtr QueryString;
            internal IntPtr GetProcAddress;

            internal IntPtr GetPlatformDisplayEXT;
            internal IntPtr CreatePlatformWindowSurfaceEXT;

        }

        internal unsafe struct Osmesa
        {

            private void* handle;

            private IntPtr CreateContextExt;
            private IntPtr CreateContextAttribs;
            private IntPtr DestroyContext;
            private IntPtr MakeCurrent;
            private IntPtr GetColorBuffer;
            private IntPtr GetDepthBuffer;
            private IntPtr GetProcAddress;

        }

        internal unsafe struct vk
        {

            private GLFWbool available;
            private void* handle;
            private fixed ulong extensions[2];
            private IntPtr GetInstanceProcAddr;
            private GLFWbool KHR_surface;
            private GLFWbool KHR_win32_surface;
            private GLFWbool MVK_macos_surface;
            private GLFWbool EXT_metal_surface;
            private GLFWbool KHR_xlib_surface;
            private GLFWbool KHR_xcb_surface;
            private GLFWbool KHR_wayland_surface;

            public vk()
            {
            }

        }

        internal unsafe struct Callbacks
        {

            private IntPtr monitor;
            private IntPtr joystick;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapelement
    {

        private byte type;
        private byte index;
        private sbyte axisScale;
        private sbyte axisOffset;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapping
    {

        private fixed byte name[128];
        private fixed byte guid[33];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        private GLFWmapelement[] buttons = new GLFWmapelement[15];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        private GLFWmapelement[] axes = new GLFWmapelement[6];

        public GLFWmapping()
        { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWjoystick
    {

        private GLFWbool allocated;
        private GLFWbool connected;
        private float* axes;
        private int axisCount;
        private byte* buttons;
        private int buttonCount;
        private byte* hats;
        private int hatCount;
        private fixed byte name[128];
        private void* userPointer;
        private fixed byte guid[33];

        //GLFWmapping* mapping;
        private IntPtr mapping;

        // This is defined in platform.h
        // not completed in transforming
        // GLFW_PLATFORM_JOYSTICK_STATE;

        public GLFWjoystick()
        { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWls
    {
        // This is defined in platform.h

        //GLFW_PLATFORM_TLS_STATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWplatform
    {

        private int platformID;

        // init
        //GLFWbool(*init)(void);
        private IntPtr init;

        //void (* terminate) (void);
        private IntPtr terminate;

        #region input
        /* void (* getCursorPos) (GLFWwindow*, double*, double*);
        void (* setCursorPos) (GLFWwindow*, double, double);
        void (* setCursorMode) (GLFWwindow*, int);
        void (* setRawMouseMotion) (GLFWwindow*, GLFWbool);
        GLFWbool(*rawMouseMotionSupported)(void);
        GLFWbool(*createCursor)(GLFWcursor*,const GLFWimage*,int,int);
        GLFWbool(*createStandardCursor)(GLFWcursor*, int);
        void (* destroyCursor) (GLFWcursor*);
        void (* setCursor) (GLFWwindow*, GLFWcursor*);
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
        GLFWvidmode* (* getVideoModes) (GLFWmonitor*, int*);
        void (* getVideoMode) (GLFWmonitor*, GLFWvidmode*);
        GLFWbool(*getGammaRamp)(GLFWmonitor*, GLFWgammaramp*);
        void (* setGammaRamp) (GLFWmonitor*,const GLFWgammaramp*);
        // window
        GLFWbool(*createWindow)(GLFWwindow*,const GLFWwndconfig*,const GLFWctxconfig*,const GLFWfbconfig*);
        void (* destroyWindow) (GLFWwindow*);
        void (* setWindowTitle) (GLFWwindow*,const byte*);
        void (* setWindowIcon) (GLFWwindow*, int,const GLFWimage*);
        void (* getWindowPos) (GLFWwindow*, int*, int*);
        void (* setWindowPos) (GLFWwindow*, int, int);
        void (* getWindowSize) (GLFWwindow*, int*, int*);
        void (* setWindowSize) (GLFWwindow*, int, int);
        void (* setWindowSizeLimits) (GLFWwindow*, int, int, int, int);
        void (* setWindowAspectRatio) (GLFWwindow*, int, int);
        void (* getFramebufferSize) (GLFWwindow*, int*, int*);
        void (* getWindowFrameSize) (GLFWwindow*, int*, int*, int*, int*);
        void (* getWindowContentScale) (GLFWwindow*, float*, float*);
        void (* iconifyWindow) (GLFWwindow*);
        void (* restoreWindow) (GLFWwindow*);
        void (* maximizeWindow) (GLFWwindow*);
        void (* showWindow) (GLFWwindow*);
        void (* hideWindow) (GLFWwindow*);
        void (* requestWindowAttention) (GLFWwindow*);
        void (* focusWindow) (GLFWwindow*);
        void (* setWindowMonitor) (GLFWwindow*, GLFWmonitor*, int, int, int, int, int);
        GLFWbool(*windowFocused)(GLFWwindow*);
        GLFWbool(*windowIconified)(GLFWwindow*);
        GLFWbool(*windowVisible)(GLFWwindow*);
        GLFWbool(*windowMaximized)(GLFWwindow*);
        GLFWbool(*windowHovered)(GLFWwindow*);
        GLFWbool(*framebufferTransparent)(GLFWwindow*);
        float (* getWindowOpacity) (GLFWwindow*);
        void (* setWindowResizable) (GLFWwindow*, GLFWbool);
        void (* setWindowDecorated) (GLFWwindow*, GLFWbool);
        void (* setWindowFloating) (GLFWwindow*, GLFWbool);
        void (* setWindowOpacity) (GLFWwindow*, float);
        void (* setWindowMousePassthrough) (GLFWwindow*, GLFWbool);
        void (* pollEvents) (void);
        void (* waitEvents) (void);
        void (* waitEventsTimeout) (double);
        void (* postEmptyEvent) (void);
        */
        #endregion

        internal IntPtr setCursorPos;
        internal IntPtr setCursorMode;
        internal IntPtr setRawMouseMotion;
        internal IntPtr rawMouseMotionSupported;
        internal IntPtr createCursor;
        internal IntPtr createStandardCursor;
        internal IntPtr destroyCursor;
        internal IntPtr setCursor;
        internal IntPtr getScancodeName;
        internal IntPtr getKeyScancode;
        internal IntPtr setClipboardString;
        internal IntPtr getClipboardString;
        internal IntPtr initJoysticks;
        internal IntPtr terminateJoysticks;
        internal IntPtr pollJoystick;
        internal IntPtr getMappingName;
        internal IntPtr updateGamepadGUID;

        // monitor
        internal IntPtr freeMonitor;

        internal IntPtr getMonitorPos;
        internal IntPtr getMonitorContentScale;
        internal IntPtr getMonitorWorkarea;
        internal IntPtr getVideoModes;
        internal IntPtr getVideoMode;
        internal IntPtr getGammaRamp;
        internal IntPtr setGammaRamp;

        // window
        internal IntPtr createWindow;

        internal IntPtr destroyWindow;
        internal IntPtr setWindowTitle;
        internal IntPtr setWindowIcon;
        internal IntPtr getWindowPos;
        internal IntPtr setWindowPos;
        internal IntPtr getWindowSize;
        internal IntPtr setWindowSize;
        internal IntPtr setWindowSizeLimits;
        internal IntPtr setWindowAspectRatio;
        internal IntPtr getFramebufferSize;
        internal IntPtr getWindowFrameSize;
        internal IntPtr getWindowContentScale;
        internal IntPtr iconifyWindow;
        internal IntPtr restoreWindow;
        internal IntPtr maximizeWindow;
        internal IntPtr showWindow;
        internal IntPtr hideWindow;
        internal IntPtr requestWindowAttention;
        internal IntPtr focusWindow;
        internal IntPtr setWindowMonitor;
        internal IntPtr windowFocused;
        internal IntPtr windowIconified;
        internal IntPtr windowVisible;
        internal IntPtr windowMaximized;
        internal IntPtr windowHovered;
        internal IntPtr framebufferTransparent;
        internal IntPtr getWindowOpacity;
        internal IntPtr setWindowResizable;
        internal IntPtr setWindowDecorated;
        internal IntPtr setWindowFloating;
        internal IntPtr setWindowOpacity;
        internal IntPtr setWindowMousePassthrough;
        internal IntPtr pollEvents;
        internal IntPtr waitEvents;
        internal IntPtr waitEventsTimeout;
        internal IntPtr postEmptyEvent;

        // EGL
        //EGLenum(*getEGLPlatform)(EGLint**);
        //EGLNativeDisplayType(*getEGLNativeDisplay)(void);
        //EGLNativeWindowType(*getEGLNativeWindow)(GLFWwindow*);
        internal IntPtr getEGLPlatform;

        internal IntPtr getEGLNativeDisplay;
        internal IntPtr getEGLNativeWindow;

        // vulkan
        //void (* getRequiredInstanceExtensions) (byte**);
        //GLFWbool(*getPhysicalDevicePresentationSupport)(VkInstance, VkPhysicalDevice, uint32_t);
        //VkResult(*createWindowSurface)(VkInstance, GLFWwindow*,const VkAllocationCallbacks*, VkSurfaceKHR*);
        internal IntPtr getRequiredInstanceExtensions;

        internal IntPtr VkPhysicalDevice;
        internal IntPtr GLFWwindow;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmutex
    {
        // This is defined in platform.h
        //GLFW_PLATFORM_MUTEX_STATE
    }

    //public unsafe struct VkAllocationCallbacks { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct VkExtensionProperties
    {

        private fixed byte extensionName[256];
        private uint specVersion;

        public VkExtensionProperties()
        {
        }

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

    #region glad struct

    [StructLayout(LayoutKind.Sequential)]
    internal struct GLsync
    {
    }

    #endregion glad struct
}

/**/