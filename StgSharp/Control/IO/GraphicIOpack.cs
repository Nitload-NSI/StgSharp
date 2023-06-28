using System;
using System.Runtime.InteropServices;

namespace StgSharp.Control
{

    //these are gl functions used to create a window
    internal delegate void InternalInitGLDelegate();
    internal delegate IntPtr InternalGLcreateWindowDelegate
        (int width, int height, byte[] tittle, IntPtr monitor, IntPtr share);
    internal delegate void InternalGLmakeContextCurrent(IntPtr window);
    internal delegate void InternalGLswapInterval(int interval);
    internal delegate void InternalGLclearColorDelegate
        (float red, float green, float blue, float alpha);
    internal delegate GLFWproc internalglGetProcAddress(char[] name);
    internal delegate int internalGLADloadGLloader(InternalGLADloadproc load);
    internal delegate int InternalGLwindowShouldClose(IntPtr window);
    internal delegate void InternalGLswapBuffers(IntPtr window);


    //these are gl functions used to set a callback
    internal delegate FrameBufferSizeCallback InternalsetBufferSizeCallback(IntPtr window, FrameBufferSizeCallback callback);


    /// 
    internal delegate void GLFWproc();
    internal unsafe delegate void* InternalGLADloadproc(byte[] name);



    [StructLayout(LayoutKind.Sequential)]
    internal struct GraphicIOPack
    {
        //these are gl functions used to create a window
        internal InternalGLcreateWindowDelegate internalGLcreateWindow;
        internal InternalGLmakeContextCurrent internalGLmakeContextCurrent;
        internal InternalGLswapInterval internalGLswapInterval;
        internal InternalGLclearColorDelegate internalGLclearColor;
        internal InternalGLwindowShouldClose internalGLwindowShouldClose;
        internal InternalGLswapBuffers internalGLswapBuffers;



        //these are gl functions used to set a callback

        internal InternalsetBufferSizeCallback internalGLsetBufferSize;
    }
}
