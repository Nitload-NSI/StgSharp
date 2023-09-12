using StgSharp.Controlling;
using StgSharp.Graphics;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp
{
    public static unsafe partial class Graphic
    {

        /// <summary>
        /// Creates a form and its associated context.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Required width</param>
        /// <param name="tittle">Tittle of the window </param>
        /// <param name="monitor"></param>
        /// <param name="share"></param>
        /// <returns>The instance of created form</returns>
        /// <exception cref="Exception"></exception>
        public static unsafe Form GLcreateWindow(
            int width, int height, string tittle, IntPtr monitor, Form share)
        {
            byte[] tittleBytes = Encoding.UTF8.GetBytes(tittle);
            IntPtr windowPtr;
            if (share == null)
            {
                windowPtr = internalIO.glfwCreateWindow(
                    800, 600, tittleBytes, monitor, IntPtr.Zero);
            }
            else
            {
                windowPtr = internalIO.glfwCreateWindow(
                    width, height, tittleBytes, monitor, share.windowID);
            }


            if ((long)windowPtr == -1 || (long)windowPtr == -2)
            {
                throw new Exception("Cannot create window instance!");
            }
            else
            {
                Form form = new Form();
                form.Size = new Vec2d(width, height);
                form.windowID = windowPtr;
                return form;
            }
        }

        /// <summary>
        /// Creates a window and its associated context.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Required width</param>
        /// <param name="tittle">Tittle of the window </param>
        /// <param name="monitor"></param>
        /// <param name="share"></param>
        /// <returns>The handle of created window</returns>
        /// <exception cref="Exception"></exception>
        public static unsafe Form GLcreateWindow(int width, int height, string tittle, IntPtr monitor, IntPtr share)
        {
            byte[] tittleCharArray = System.Text.Encoding.UTF8.GetBytes(tittle);
            IntPtr windowPtr = internalIO.glfwCreateWindow
                (width, height, tittleCharArray, monitor, share);

            if ((int)windowPtr == -1 || (int)windowPtr == -2)
            {
                throw new Exception("Cannot create the form!");
            }
            else
            {
                Form form = new Form();
                form.Size = new Vec2d(width, height);
                form.windowID = windowPtr;
                return form;
            }
        }


        public static unsafe void GLmakeContexCurrent(Form form)
        {
            internalIO.glfwMakeContextCurrent(form.windowID);
        }


        public static unsafe bool GLwindowShouldClose(Form form)
        {
            int a = internalIO.glfwWindowShouldClose(form.windowID);
            return a != 0;
        }


        public static unsafe void GLswapBuffers(Form form)
        {
            internalIO.glfwSwapBuffers(form.windowID);
        }



        public static IntPtr GetProcAddress(string name)
        {
            IntPtr ret = internalIO.glfwGetProcAddress(name);
            if (ret == IntPtr.Zero)
            {
#if DEBUG
                Console.WriteLine(name);
#endif
                return internalIO.glfwGetProcAddress(name);
            }
            else
            {
                return ret;
            }
        }

    }
}
