//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glType.cs"
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
using StgSharp.Geometries;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;


namespace StgSharp.Graphics
{

    public delegate IntPtr GLfuncLoader(string name);

    internal delegate void GLVULKANPROCNV();

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

    #region GLFW_dele

    internal unsafe delegate void GLFWwindowsizefun(glfwWindow* window, int width, int height);
    internal unsafe delegate void GLFWwindowrefreshfun(glfwWindow* window);
    internal unsafe delegate void GLFWwindowposfun(glfwWindow* window, int xpos, int ypos);
    internal unsafe delegate void GLFWwindowmaximizefun(glfwWindow* window, int maximized);
    internal unsafe delegate void GLFWwindowiconifyfun(glfwWindow* window, int iconified);
    internal unsafe delegate void GLFWwindowfocusfun(glfwWindow* window, int focused);
    internal unsafe delegate void GLFWwindowcontentscalefun(glfwWindow* window, float xscale, float yscale);
    internal unsafe delegate void GLFWwindowclosefun(glfwWindow* window);
    internal unsafe delegate void GLFWvkproc();
    internal unsafe delegate void GLFWscrollfun(glfwWindow* window, double xoffset, double yoffset);
    internal unsafe delegate void GLFWmousebuttonfun(glfwWindow* window, int button, int action, int mods);
    internal unsafe delegate void GLFWmonitorfun(glfwMonitor* monitor, int monitorevent);
    internal unsafe delegate void GLFWkeyfun(glfwWindow* window, int key, int scancode, int action, int mods);
    internal unsafe delegate void GLFWjoystickfun(int jid, int keyevent);
    public unsafe delegate void GLFWglproc();
    internal unsafe delegate void FramebuffersizeHandler(IntPtr window, int width, int height); //window is the pointer to a glfwWindow instance
    internal unsafe delegate void GLFWerrorfun(int error_code, sbyte* description);
    internal unsafe delegate void GLFWdropfun(glfwWindow* window, int path_count, sbyte*[] paths);
    internal unsafe delegate void GLFWcursorposfun(glfwWindow* window, double xpos, double ypos);
    internal unsafe delegate void GLFWcursorenterfun(glfwWindow* window, int entered);
    internal unsafe delegate void GLFWcharmodsfun(glfwWindow* window, uint codepoint, int mods);
    internal unsafe delegate void GLFWcharfun(glfwWindow* window, uint codepoint);

    #endregion


}