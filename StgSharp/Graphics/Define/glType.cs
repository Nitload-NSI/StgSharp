using StgSharp.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp
{
    internal static unsafe partial class InternalIO
    {
        internal static Dictionary<Type, uint> GLtype =
            new Dictionary<Type, uint>();

    }
}

namespace StgSharp.Graphics
{

    internal delegate void GLVULKANPROCNV();

    #region GLFW_dele

    internal unsafe delegate void GLFWwindowsizefun(GLFWwindow* window, int width, int height);
    internal unsafe delegate void GLFWwindowrefreshfun(GLFWwindow* window);
    internal unsafe delegate void GLFWwindowposfun(GLFWwindow* window, int xpos, int ypos);
    internal unsafe delegate void GLFWwindowmaximizefun(GLFWwindow* window, int maximized);
    internal unsafe delegate void GLFWwindowiconifyfun(GLFWwindow* window, int iconified);
    internal unsafe delegate void GLFWwindowfocusfun(GLFWwindow* window, int focused);
    internal unsafe delegate void GLFWwindowcontentscalefun(GLFWwindow* window, float xscale, float yscale);
    internal unsafe delegate void GLFWwindowclosefun(GLFWwindow* window);
    internal unsafe delegate void GLFWvkproc();
    internal unsafe delegate void GLFWscrollfun(GLFWwindow* window, double xoffset, double yoffset);
    internal unsafe delegate void GLFWmousebuttonfun(GLFWwindow* window, int button, int action, int mods);
    internal unsafe delegate void GLFWmonitorfun(GLFWmonitor* monitor, int monitorevent);
    internal unsafe delegate void GLFWkeyfun(GLFWwindow* window, int key, int scancode, int action, int mods);
    internal unsafe delegate void GLFWjoystickfun(int jid, int keyevent);
    public unsafe delegate void GLFWglproc();
    internal unsafe delegate void FramebuffersizeHandler(IntPtr window, int width, int height); //window is the pointer to a GLFWwindow instance
    internal unsafe delegate void GLFWerrorfun(int error_code, sbyte* description);
    internal unsafe delegate void GLFWdropfun(GLFWwindow* window, int path_count, sbyte*[] paths);
    internal unsafe delegate void GLFWcursorposfun(GLFWwindow* window, double xpos, double ypos);
    internal unsafe delegate void GLFWcursorenterfun(GLFWwindow* window, int entered);
    internal unsafe delegate void GLFWcharmodsfun(GLFWwindow* window, uint codepoint, int mods);
    internal unsafe delegate void GLFWcharfun(GLFWwindow* window, uint codepoint);

    #endregion

    public unsafe struct GLhandleARB
    {

        dynamic value;

        public unsafe GLhandleARB(dynamic value)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (value.GetType().Name != "Uint32")
                {
                    goto errorlog;
                }
                this.value = value;
            }
            else
            {
                if (value.GetType().Name != "IntPtr")
                {
                    goto errorlog;
                }
                this.value = value;
            }

        errorlog:
            {
                InternalIO.InternalWriteLog(
                    $"{value.GetType().Name} does not match definination of GLhandlARB on OS {Environment.OSVersion.Platform} ",
                    LogType.Error);
            }

        }

        public static implicit operator uint(GLhandleARB value)
        {
            return (uint)value;
        }



    }

    public delegate IntPtr GLfuncLoader(string name);

}