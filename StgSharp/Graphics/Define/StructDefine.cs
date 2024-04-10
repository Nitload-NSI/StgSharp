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
        private IntPtr destroy;
        private delegate*<byte*> extensionSupported;
        private GLFWbool forward, debug, noerror;

        //PFNGLGETINTEGERVPROC GetIntegerv;
        private IntPtr GetIntegerv;
        private delegate*<delegate*<void>, void> getProcAddress;

        //PFNGLGETSTRINGPROC GetString;
        private IntPtr GetString;

        //PFNGLGETSTRINGIPROC GetStringi;
        private IntPtr GetStringi;
        private int major, minor, revision;

        /*
        void (* makeCurrent) (GLFWwindow*);
        void (* swapBuffers) (GLFWwindow*);
        void (* swapInterval) (int);
        int (* extensionSupported) (const byte*);
        GLFWglproc(*getProcAddress)(const byte*);
        void (* destroy) (GLFWwindow*);
        */

        private delegate*<GLFWwindow*> makeCurrent;
        private int profile;
        private int release;
        private int robustness;
        private int source;
        private delegate*<GLFWwindow*> swapBuffer;
        private delegate*<int> swapInterval;

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

            //OSMesaContext handle;
            private void* handle;
            private int height;

            private int width;

        }

        // This is defined in platform.h
        //GLFW_PLATFORM_CONTEXT_STATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWvidmode
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
    public unsafe struct GLFWgammaramp
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

        /*! The height, in pixels, of this image.
         */
        private int height;
        /*! The pixel data of this image, arranged left-to-right, top-to-bottom.
         */
        private byte* pixels;

        /*! The width, in pixels, of this image.
         */
        private int width;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWgamepadstate
    {

        /*! The states of each [gamepad axis](@ref gamepad_axes), in the range -1.0
         *  to 1.0 inclusive.
         */
        public fixed float axes[6];

        /*! The states of each [gamepad button](@ref gamepad_buttons), `GLFW_PRESS`
         *  or `GLFW_RELEASE`.
         */
        public fixed byte buttons[15];

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWallocator
    {

        //GLFWallocatefun allocate;
        private IntPtr allocate;

        //GLFWdeallocatefun deallocate;
        private IntPtr deallocate;

        //GLFWreallocatefun reallocate;
        private IntPtr reallocate;

        private void* user;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmonitor
    {

        private GLFWvidmode currentMode;
        private GLFWgammaramp currentRamp;

        private int modeCount;

        //GLFWvidmode* modes;
        private IntPtr modes;

        private byte[] name = new byte[128];

        private GLFWgammaramp originalRamp;
        private void* userPointer;

        // Physical dimensions in millimeters.
        private int widthMM, heightMM;

        // The window whose video mode is current on this monitor
        //GLFWwindow* window;
        private IntPtr window;

        // not transformed
        //GLFW_PLATFORM_MONITOR_STATE;

        public GLFWmonitor()
        { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwindow
    {

        private int autoIconify;

        private fixed long callBacks[17];
        private GLFWcursor* cursor = (GLFWcursor*)0;
        private int cursorMode = 0;
        private int decorated;
        private bool doubleBuffer;
        private int floating;
        private int focusOnShow;
        private fixed byte keys[349];
        private int lockKeyMods;
        private int maxWidth = 0, maxHeight = 0;

        private int minWidth = 0, minHeight = 0;
        private IntPtr monitor = IntPtr.Zero;   //public GLFWmonitor* monitor;
        private fixed byte mouseButtons[8];
        private int mousePassthrough;
        private int numer = 0, denom = 0;
        private int rawMouseMotion;

        private int resizable;
        private int stickyKeys;
        private int stickyMouseButtons;
        private void* userPointer = (void*)0;
        private GLFWvidmode videoMode = default;
        internal int shouldClose;

        public GLFWcontext context = default;

        //struct GLFWwindow* next in GLFW
        public GLFWwindow* next;

        // Virtual cursor position when cursor is disabled
        public double virtualCursorPosX = 0, virtualCursorPosY = 0;

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

        private int angleType;

        private GLFWbool hatButtons;
        private int platformID;

        //PFN_vkGetInstanceProcAddr vulkanLoader;
        private IntPtr vulkanLoader;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwndconfig
    {

        private GLFWbool autoIconify;
        private GLFWbool centerCursor;
        private GLFWbool decorated;
        private GLFWbool floating;
        private GLFWbool focused;
        private GLFWbool focusOnShow;
        private int height;
        private GLFWbool maximized;
        private GLFWbool mousePassthrough;
        private GLFWbool resizable;
        private GLFWbool scaleToMonitor;
        private byte* title;
        private GLFWbool visible;
        private int width;

        private int xpos;
        private int ypos;

        internal Ns ns = new Ns();

        internal Win32 win32 = new Win32();

        internal Wl wl = new Wl();

        internal X11 x11 = new X11();

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
        private GLFWbool debug;
        private GLFWbool forward;
        private int major;
        private int minor;
        private GLFWbool noerror;
        private int profile;
        private int release;
        private int robustness;

        //GLFWwindow* share;
        private IntPtr share;
        private int source;

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

        private int accumAlphaBits;
        private int accumBlueBits;
        private int accumGreenBits;
        private int accumRedBits;
        private int alphaBits;
        private int auxBuffers;
        private int blueBits;
        private int depthBits;
        private GLFWbool doublebuffer;
        private int greenBits;
        private uintptr_t handle;

        private int redBits;
        private int samples;
        private GLFWbool sRGB;
        private int stencilBits;
        private GLFWbool stereo;
        private GLFWbool transparent;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GLFWlibrary
    {

        private GLFWallocator allocator;
        private GLFWls contextSlot;

        //GLFWcursor* cursorListHead;
        private IntPtr cursorListHead;

        //GLFWerror* errorListHead;
        private IntPtr errorListHead;
        private GLFWmutex errorLock;

        private GLFWls errorSlot;

        private GLFWbool initialized;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GLconst.GLFW_JOYSTICK_LAST + 1)]
        private GLFWjoystick[] joysticks = new GLFWjoystick[GLconst.GLFW_JOYSTICK_LAST + 1];

        private GLFWbool joysticksInitialized;

        private int mappingCount;

        //GLFWmapping* mappings;
        private IntPtr mappings;

        private int monitorCount;

        //GLFWmonitor** monitors;
        private IntPtr monitors;

        private GLFWplatform platform;

        //GLFWwindow* windowListHead;
        private IntPtr windowListHead;

        internal Callbacks callbacks = new Callbacks();

        internal Egl egl = new Egl();

        internal Hints hints = new Hints();

        internal Osmesa osmesa = new Osmesa();

        public GLFWlibrary()
        {
        }

        internal unsafe struct Hints
        {

            private GLFWctxconfig context;
            private GLFWfbconfig framebuffer;

            private GLFWinitconfig init;
            private int refreshRate;
            private GLFWwndconfig window;

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

            internal bool ANGLE_platform_angle;
            internal bool ANGLE_platform_angle_d3d;
            internal bool ANGLE_platform_angle_metal;
            internal bool ANGLE_platform_angle_opengl;
            internal bool ANGLE_platform_angle_vulkan;
            internal IntPtr BindAPI;
            internal IntPtr CreateContext;
            internal IntPtr CreatePlatformWindowSurfaceEXT;
            internal IntPtr CreateWindowSurface;
            internal IntPtr DestroyContext;
            internal IntPtr DestroySurface;
            internal void* display;
            internal bool EXT_client_extensions;
            internal bool EXT_platform_base;
            internal bool EXT_platform_wayland;
            internal bool EXT_platform_x11;
            internal bool EXT_present_opaque;

            internal IntPtr GetConfigAttrib;
            internal IntPtr GetConfigs;
            internal IntPtr GetDisplay;
            internal IntPtr GetError;

            internal IntPtr GetPlatformDisplayEXT;
            internal IntPtr GetProcAddress;

            internal void* handle;
            internal IntPtr Initialize;
            internal bool KHR_colorSpace;
            internal bool KHR_context_flush_control;

            internal bool KHR_create_context;
            internal bool KHR_create_context_no_error;
            internal bool KHR_get_all_proc_addresses;
            internal int major, minor;
            internal IntPtr MakeCurrent;

            internal int platform;
            internal bool prefix;
            internal IntPtr QueryString;
            internal IntPtr SwapBuffers;
            internal IntPtr SwapInterval;
            internal IntPtr Terminate;

        }

        internal unsafe struct Osmesa
        {

            private IntPtr CreateContextAttribs;

            private IntPtr CreateContextExt;
            private IntPtr DestroyContext;
            private IntPtr GetColorBuffer;
            private IntPtr GetDepthBuffer;
            private IntPtr GetProcAddress;

            private void* handle;
            private IntPtr MakeCurrent;

        }

        internal unsafe struct vk
        {

            private GLFWbool available;
            private GLFWbool EXT_metal_surface;
            private fixed ulong extensions[2];
            private IntPtr GetInstanceProcAddr;
            private void* handle;
            private GLFWbool KHR_surface;
            private GLFWbool KHR_wayland_surface;
            private GLFWbool KHR_win32_surface;
            private GLFWbool KHR_xcb_surface;
            private GLFWbool KHR_xlib_surface;
            private GLFWbool MVK_macos_surface;

            public vk()
            {
            }

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

        private sbyte axisOffset;
        private sbyte axisScale;
        private byte index;

        private byte type;

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

        public GLFWmapping()
        { }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWjoystick
    {

        private GLFWbool allocated;
        private float* axes;
        private int axisCount;
        private int buttonCount;
        private byte* buttons;
        private GLFWbool connected;
        private fixed byte guid[33];
        private int hatCount;
        private byte* hats;

        //GLFWmapping* mapping;
        private IntPtr mapping;
        private fixed byte name[128];
        private void* userPointer;

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

        // init
        //GLFWbool(*init)(void);
        private IntPtr init;

        private int platformID;

        //void (* terminate) (void);
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
        //EGLenum(*getEGLPlatform)(EGLint**);
        //EGLNativeDisplayType(*getEGLNativeDisplay)(void);
        //EGLNativeWindowType(*getEGLNativeWindow)(GLFWwindow*);
        internal IntPtr getEGLPlatform;
        internal IntPtr getFramebufferSize;
        internal IntPtr getGammaRamp;
        internal IntPtr getKeyScancode;
        internal IntPtr getMappingName;
        internal IntPtr getMonitorContentScale;

        internal IntPtr getMonitorPos;
        internal IntPtr getMonitorWorkarea;

        // vulkan
        //void (* getRequiredInstanceExtensions) (byte**);
        //GLFWbool(*getPhysicalDevicePresentationSupport)(VkInstance, VkPhysicalDevice, uint32_t);
        //VkResult(*createWindowSurface)(VkInstance, GLFWwindow*,const VkAllocationCallbacks*, VkSurfaceKHR*);
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