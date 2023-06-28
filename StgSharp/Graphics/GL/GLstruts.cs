using System;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics
{
    //rename types in style of GLFW

    using GLFWbool = Boolean;

    //strucs defined in glfw3.h

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWcontext
    {
        int client;
        int source;
        int major, minor, revision;
        GLFWbool forward, debug, noerror;
        int profile;
        int robustness;
        int release;

        //PFNGLGETSTRINGIPROC GetStringi;
        IntPtr GetStringi;
        //PFNGLGETINTEGERVPROC GetIntegerv;
        IntPtr GetIntegerv;
        //PFNGLGETSTRINGPROC GetString;
        IntPtr GetString;

        /*
        void (* makeCurrent) (_GLFWwindow*);
        void (* swapBuffers) (_GLFWwindow*);
        void (* swapInterval) (int);
        int (* extensionSupported) (const char*);
        GLFWglproc(*getProcAddress)(const char*);
        void (* destroy) (_GLFWwindow*);
        */

        IntPtr makeCurrent;
        IntPtr swapBuffer;
        IntPtr swapInterval;
        IntPtr extensionSupported;
        IntPtr getProcAddress;
        IntPtr destroy;

        internal struct egl
        {
            /*
        EGLConfig config;
        EGLContext handle;
        EGLSurface surface;
        */
            void* config;
            void* handle;
            void* surface;
            void* client;
        }

        internal struct osmesa
        {
            //OSMesaContext handle;
            void* handle;
            int width;
            int height;
            void* buffer;
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
        int refreshRate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWgammaramp
    {

        /*! An array of value describing the response of the red channel.
         */
        ushort* red;
        /*! An array of value describing the response of the green channel.
         */
        ushort* green;
        /*! An array of value describing the response of the blue channel.
         */
        ushort* blue;
        /*! The number of elements in each array.
         */
        uint size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWimage
    {

        /*! The width, in pixels, of this image.
         */
        int width;
        /*! The height, in pixels, of this image.
         */
        int height;
        /*! The pixel data of this image, arranged left-to-right, top-to-bottom.
         */
        byte* pixels;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWgamepadstate
    {

        /*! The states of each [gamepad button](@ref gamepad_buttons), `GLFW_PRESS`
         *  or `GLFW_RELEASE`.
         */
        byte[] buttons = new byte[15];
        /*! The states of each [gamepad axis](@ref gamepad_axes), in the range -1.0
         *  to 1.0 inclusive.
         */
        float[] axes = new float[6];

        public GLFWgamepadstate()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWallocator
    {
        //GLFWallocatefun allocate;
        IntPtr allocate;
        //GLFWreallocatefun reallocate;
        IntPtr reallocate;
        //GLFWdeallocatefun deallocate;
        IntPtr deallocate;
        void* user;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmonitor
    {

        char[] name = new char[128];
        void* userPointer;

        // Physical dimensions in millimeters.
        int widthMM, heightMM;

        // The window whose video mode is current on this monitor
        //_GLFWwindow* window;
        IntPtr window;

        //GLFWvidmode* modes;
        IntPtr modes;
        int modeCount;
        GLFWvidmode currentMode;

        GLFWgammaramp originalRamp;
        GLFWgammaramp currentRamp;

        // not transformed
        //GLFW_PLATFORM_MONITOR_STATE;

        public GLFWmonitor() { }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwindow
    {
        //struct _GLFWwindow* next in GLFW
        public IntPtr next = IntPtr.Zero;

        public bool resizable = false;
        public bool decorated = false;
        public bool autoIconify = false;
        public bool floating = false;
        public bool focusOnShow = false;
        public bool mousePassthrough = false;
        public bool shouldClose = false;
        public void* userPointer = (void*)0;
        public bool doublebuffer = false;
        public GLFWvidmode videoMode = default;
        public IntPtr monitor = IntPtr.Zero;   //public GLFWmonitor* monitor;
        public GLFWcursor* cursor = (GLFWcursor*)0;

        public int minwidth = 0, minheight = 0;
        public int maxwidth = 0, maxheight = 0;
        public int numer = 0, denom = 0;

        public bool stickyKeys = false;
        public bool stickyMouseButtons = false;
        public bool lockKeyMods = false;
        public int cursorMode = 0;
        public char[] mouseButtons = new char[8];
        public char[] keys = new char[349];
        // Virtual cursor position when cursor is disabled
        public double virtualCursorPosX = 0, virtualCursorPosY = 0;
        public bool rawMouseMotion = false;

        public GLFWcontext context = default;

        public GLFWwindow()
        {

        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GLFWcursor
    {
        //_GLFWcursor* next;
        IntPtr next;

        // This is defined in platform.h
        // not completed in tranformation
        // GLFW_PLATFORM_CURSOR_STATE;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWerror
    {

        //_GLFWerror* next;
        IntPtr next;
        int code;
        char[] description = new char[GLFW._GLFW_MESSAGE_SIZE];

        public GLFWerror()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWinitconfig
    {

        GLFWbool hatButtons;
        int angleType;
        int platformID;
        //PFN_vkGetInstanceProcAddr vulkanLoader;
        IntPtr vulkanLoader;
        struct ns
        {
            GLFWbool menubar;
            GLFWbool chdir;
        }
        struct x11
        {
            GLFWbool xcbVulkanSurface;
        }
        struct wl
        {
            int libdecorMode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWwndconfig
    {

        int xpos;
        int ypos;
        int width;
        int height;
        char* title;
        GLFWbool resizable;
        GLFWbool visible;
        GLFWbool decorated;
        GLFWbool focused;
        GLFWbool autoIconify;
        GLFWbool floating;
        GLFWbool maximized;
        GLFWbool centerCursor;
        GLFWbool focusOnShow;
        GLFWbool mousePassthrough;
        GLFWbool scaleToMonitor;
        internal struct Ns
        {
            GLFWbool retina = false;
            char[] frameName = new char[256];

            public Ns()
            {
            }
        }
        internal Ns ns = new Ns();

        internal struct X11
        {
            char[] className = new char[256];
            char[] instanceName = new char[256];

            public X11()
            {
            }
        }
        internal X11 x11 = new X11();

        internal struct Win32
        {
            GLFWbool keymenu;
        }
        internal Win32 win32 = new Win32();

        internal struct Wl
        {
            char[] appId = new char[256];

            public Wl()
            {
            }
        }
        internal Wl wl = new Wl();

        public GLFWwndconfig()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWctxconfig
    {
        int client;
        int source;
        int major;
        int minor;
        GLFWbool forward;
        GLFWbool debug;
        GLFWbool noerror;
        int profile;
        int robustness;
        int release;
        //_GLFWwindow* share;
        IntPtr share;
        internal struct Nsgl
        {
            GLFWbool offline;
        }
        internal Nsgl nsgl = new Nsgl();

        public GLFWctxconfig()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWfbconfig
    {

        int redBits;
        int greenBits;
        int blueBits;
        int alphaBits;
        int depthBits;
        int stencilBits;
        int accumRedBits;
        int accumGreenBits;
        int accumBlueBits;
        int accumAlphaBits;
        int auxBuffers;
        GLFWbool stereo;
        int samples;
        GLFWbool sRGB;
        GLFWbool doublebuffer;
        GLFWbool transparent;
        uintptr_t handle;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct _GLFWlibrary
    {
        GLFWbool initialized;
        GLFWallocator allocator;

        GLFWplatform platform;

        internal unsafe struct Hints
        {
            GLFWinitconfig init;
            GLFWfbconfig framebuffer;
            GLFWwndconfig window;
            GLFWctxconfig context;
            int refreshRate;
        }
        internal Hints hints = new Hints();

        //_GLFWerror* errorListHead;
        IntPtr errorListHead;
        //_GLFWcursor* cursorListHead;
        IntPtr cursorListHead;
        //_GLFWwindow* windowListHead;
        IntPtr windowListHead;
        //_GLFWmonitor** monitors;
        IntPtr monitors;
        int monitorCount;



        GLFWbool joysticksInitialized;
        _GLFWjoystick[] joysticks = new _GLFWjoystick[GLFW.GLFW_JOYSTICK_LAST + 1];
        //_GLFWmapping* mappings;
        IntPtr mappings;
        int mappingCount;

        _GLFWtls errorSlot;
        _GLFWtls contextSlot;
        _GLFWmutex errorLock;

        internal unsafe struct timer
        {
            ulong offset;
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
            internal bool KHR_gl_colorspace;
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
        internal Egl egl = new Egl();

        internal unsafe struct Osmesa
        {
            void* handle;

            IntPtr CreateContextExt;
            IntPtr CreateContextAttribs;
            IntPtr DestroyContext;
            IntPtr MakeCurrent;
            IntPtr GetColorBuffer;
            IntPtr GetDepthBuffer;
            IntPtr GetProcAddress;
        }
        internal Osmesa osmesa = new Osmesa();

        internal unsafe struct vk
        {
            GLFWbool available;
            void* handle;
            char*[] extensions = new char*[2];
            IntPtr GetInstanceProcAddr;
            GLFWbool KHR_surface;
            GLFWbool KHR_win32_surface;
            GLFWbool MVK_macos_surface;
            GLFWbool EXT_metal_surface;
            GLFWbool KHR_xlib_surface;
            GLFWbool KHR_xcb_surface;
            GLFWbool KHR_wayland_surface;

            public vk()
            {
            }
        }


        internal unsafe struct Callbacks
        {
            IntPtr monitor;
            IntPtr joystick;
        }
        internal Callbacks callbacks = new Callbacks();

        public _GLFWlibrary()
        {
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct _GLFWmapelement
    {
        byte type;
        byte index;
        sbyte axisScale;
        sbyte axisOffset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct _GLFWmapping
    {

        char[] name = new char[128];
        char[] guid = new char[33];
        _GLFWmapelement[] buttons = new _GLFWmapelement[15];
        _GLFWmapelement[] axes = new _GLFWmapelement[6];

        public _GLFWmapping() { }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct _GLFWjoystick
    {

        GLFWbool allocated;
        GLFWbool connected;
        float* axes;
        int axisCount;
        byte* buttons;
        int buttonCount;
        byte* hats;
        int hatCount;
        char[] name = new char[128];
        void* userPointer;
        char[] guid = new char[33];
        //_GLFWmapping* mapping;
        IntPtr mapping;

        // This is defined in platform.h
        // not completed in transforming
        // GLFW_PLATFORM_JOYSTICK_STATE;

        public _GLFWjoystick() { }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct _GLFWtls
    {
        // This is defined in platform.h

        //GLFW_PLATFORM_TLS_STATE
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWplatform
    {

        int platformID;
        // init
        //GLFWbool(*init)(void);
        IntPtr init;
        //void (* terminate) (void);
        IntPtr terminate;

        // input
        /* void (* getCursorPos) (_GLFWwindow*, double*, double*);
    void (* setCursorPos) (_GLFWwindow*, double, double);
    void (* setCursorMode) (_GLFWwindow*, int);
    void (* setRawMouseMotion) (_GLFWwindow*, GLFWbool);
    GLFWbool(*rawMouseMotionSupported)(void);
    GLFWbool(*createCursor)(_GLFWcursor*,const GLFWimage*,int,int);
    GLFWbool(*createStandardCursor)(_GLFWcursor*, int);
    void (* destroyCursor) (_GLFWcursor*);
    void (* setCursor) (_GLFWwindow*, _GLFWcursor*);
    const char* (*getScancodeName)(int);
    int (* getKeyScancode) (int);
    void (* setClipboardString) (const char*);
        const char* (*getClipboardString)(void);
    GLFWbool(*initJoysticks)(void);
    void (* terminateJoysticks) (void);
    GLFWbool(*pollJoystick)(_GLFWjoystick*, int);
    const char* (*getMappingName)(void);
    void (* updateGamepadGUID) (char*);
    // monitor
    void (* freeMonitor) (_GLFWmonitor*);
    void (* getMonitorPos) (_GLFWmonitor*, int*, int*);
    void (* getMonitorContentScale) (_GLFWmonitor*, float*, float*);
    void (* getMonitorWorkarea) (_GLFWmonitor*, int*, int*, int*, int*);
    GLFWvidmode* (* getVideoModes) (_GLFWmonitor*, int*);
    void (* getVideoMode) (_GLFWmonitor*, GLFWvidmode*);
    GLFWbool(*getGammaRamp)(_GLFWmonitor*, GLFWgammaramp*);
    void (* setGammaRamp) (_GLFWmonitor*,const GLFWgammaramp*);
        // window
        GLFWbool(*createWindow)(_GLFWwindow*,const _GLFWwndconfig*,const _GLFWctxconfig*,const _GLFWfbconfig*);
        void (* destroyWindow) (_GLFWwindow*);
    void (* setWindowTitle) (_GLFWwindow*,const char*);
        void (* setWindowIcon) (_GLFWwindow*, int,const GLFWimage*);
        void (* getWindowPos) (_GLFWwindow*, int*, int*);
    void (* setWindowPos) (_GLFWwindow*, int, int);
    void (* getWindowSize) (_GLFWwindow*, int*, int*);
    void (* setWindowSize) (_GLFWwindow*, int, int);
    void (* setWindowSizeLimits) (_GLFWwindow*, int, int, int, int);
    void (* setWindowAspectRatio) (_GLFWwindow*, int, int);
    void (* getFramebufferSize) (_GLFWwindow*, int*, int*);
    void (* getWindowFrameSize) (_GLFWwindow*, int*, int*, int*, int*);
    void (* getWindowContentScale) (_GLFWwindow*, float*, float*);
    void (* iconifyWindow) (_GLFWwindow*);
    void (* restoreWindow) (_GLFWwindow*);
    void (* maximizeWindow) (_GLFWwindow*);
    void (* showWindow) (_GLFWwindow*);
    void (* hideWindow) (_GLFWwindow*);
    void (* requestWindowAttention) (_GLFWwindow*);
    void (* focusWindow) (_GLFWwindow*);
    void (* setWindowMonitor) (_GLFWwindow*, _GLFWmonitor*, int, int, int, int, int);
    GLFWbool(*windowFocused)(_GLFWwindow*);
    GLFWbool(*windowIconified)(_GLFWwindow*);
    GLFWbool(*windowVisible)(_GLFWwindow*);
    GLFWbool(*windowMaximized)(_GLFWwindow*);
    GLFWbool(*windowHovered)(_GLFWwindow*);
    GLFWbool(*framebufferTransparent)(_GLFWwindow*);
    float (* getWindowOpacity) (_GLFWwindow*);
    void (* setWindowResizable) (_GLFWwindow*, GLFWbool);
    void (* setWindowDecorated) (_GLFWwindow*, GLFWbool);
    void (* setWindowFloating) (_GLFWwindow*, GLFWbool);
    void (* setWindowOpacity) (_GLFWwindow*, float);
    void (* setWindowMousePassthrough) (_GLFWwindow*, GLFWbool);
    void (* pollEvents) (void);
    void (* waitEvents) (void);
    void (* waitEventsTimeout) (double);
    void (* postEmptyEvent) (void);
        */
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
        //EGLNativeWindowType(*getEGLNativeWindow)(_GLFWwindow*);
        internal IntPtr getEGLPlatform;
        internal IntPtr getEGLNativeDisplay;
        internal IntPtr getEGLNativeWindow;



        // vulkan
        //void (* getRequiredInstanceExtensions) (char**);
        //GLFWbool(*getPhysicalDevicePresentationSupport)(VkInstance, VkPhysicalDevice, uint32_t);
        //VkResult(*createWindowSurface)(VkInstance, _GLFWwindow*,const VkAllocationCallbacks*, VkSurfaceKHR*);
        internal IntPtr getRequiredInstanceExtensions;
        internal IntPtr VkPhysicalDevice;
        internal IntPtr _GLFWwindow;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct _GLFWmutex
    {
        // This is defined in platform.h
        //GLFW_PLATFORM_MUTEX_STATE
    }

    //public unsafe struct VkAllocationCallbacks { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct VkExtensionProperties
    {
        char[] extensionName = new char[256];
        uint specVersion;

        public VkExtensionProperties()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct _GLFWerror
    {
        IntPtr next;
        int code;
        char[] description = new char[1024];

        public _GLFWerror()
        {
        }
    }


    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct uintptr_t
    {
        [FieldOffset(0)]
        long part1;
        [FieldOffset(8)]
        long part2;
    }




}
