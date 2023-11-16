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
        void (* makeCurrent) (GLFWwindow*);
        void (* swapBuffers) (GLFWwindow*);
        void (* swapInterval) (int);
        int (* extensionSupported) (const byte*);
        GLFWglproc(*getProcAddress)(const byte*);
        void (* destroy) (GLFWwindow*);
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

        byte[] name = new byte[128];
        void* userPointer;

        // Physical dimensions in millimeters.
        int widthMM, heightMM;

        // The window whose video mode is current on this monitor
        //GLFWwindow* window;
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
        //struct GLFWwindow* next in GLFW
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] mouseButtons = new byte[8];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 349)]
        public byte[] keys = new byte[349];
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
        //GLFWcursor* next;
        IntPtr next;

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
        byte* title;
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] frameName = new byte[256];

            public Ns()
            {
            }
        }
        internal Ns ns = new Ns();

        internal struct X11
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] className = new byte[256];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] instanceName = new byte[256];

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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] appId = new byte[256];

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
        //GLFWwindow* share;
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
    internal unsafe struct GLFWlibrary
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

        //GLFWerror* errorListHead;
        IntPtr errorListHead;
        //GLFWcursor* cursorListHead;
        IntPtr cursorListHead;
        //GLFWwindow* windowListHead;
        IntPtr windowListHead;
        //GLFWmonitor** monitors;
        IntPtr monitors;
        int monitorCount;



        GLFWbool joysticksInitialized;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GLconst.GLFW_JOYSTICK_LAST + 1)]
        GLFWjoystick[] joysticks = new GLFWjoystick[GLconst.GLFW_JOYSTICK_LAST + 1];
        //GLFWmapping* mappings;
        IntPtr mappings;
        int mappingCount;

        GLFWls errorSlot;
        GLFWls contextSlot;
        GLFWmutex errorLock;

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
            internal bool KHR_colorspace;
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            byte*[] extensions = new byte*[2];
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

        public GLFWlibrary()
        {
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapelement
    {
        byte type;
        byte index;
        sbyte axisScale;
        sbyte axisOffset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWmapping
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        byte[] name = new byte[128];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        byte[] guid = new byte[33];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        GLFWmapelement[] buttons = new GLFWmapelement[15];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        GLFWmapelement[] axes = new GLFWmapelement[6];

        public GLFWmapping() { }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GLFWjoystick
    {

        GLFWbool allocated;
        GLFWbool connected;
        float* axes;
        int axisCount;
        byte* buttons;
        int buttonCount;
        byte* hats;
        int hatCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        byte[] name = new byte[128];
        void* userPointer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        byte[] guid = new byte[33];
        //GLFWmapping* mapping;
        IntPtr mapping;

        // This is defined in platform.h
        // not completed in transforming
        // GLFW_PLATFORM_JOYSTICK_STATE;

        public GLFWjoystick() { }
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

        int platformID;
        // init
        //GLFWbool(*init)(void);
        IntPtr init;
        //void (* terminate) (void);
        IntPtr terminate;

        // input
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        byte[] extensionName = new byte[256];
        uint specVersion;

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
        long part1;
        [FieldOffset(8)]
        long part2;
    }
    #endregion

    #region glad struct

    [StructLayout(LayoutKind.Sequential)]
    internal struct GLsync
    {

    }


    #endregion

}
/**/