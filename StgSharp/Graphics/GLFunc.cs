using StgSharp.Control;
using StgSharp.Graphics;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static unsafe Form GLcreateWindow(int width, int height, byte[] tittle, IntPtr monitor, Form share)
        {
            IntPtr windowPtr;
            if (share == null)
            {
                windowPtr = glIO.internalGLcreateWindow(
                    width, height, tittle, monitor, (IntPtr)0);
            }
            else
            {
                windowPtr = glIO.internalGLcreateWindow(
                    width, height, tittle, monitor, (IntPtr)share.windowPtr);
            }


            if ((int)windowPtr == -1 || (int)windowPtr == -2)
            {
                throw new Exception("Cannot create window instance!");
            }
            else
            {
                Form form = new Form();
                form.Size = new vec2d(width, height);
                form.windowPtr = windowPtr;
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
            Form form = new Form();
            form.Size = new vec2d(width, height);

            byte[] tittleCharArray = System.Text.Encoding.UTF8.GetBytes(tittle);
            IntPtr windowPtr = glIO.internalGLcreateWindow(width, height, tittleCharArray, monitor, share);


            form.info.name = tittleCharArray;
            form.windowPtr = windowPtr;
            Console.WriteLine(windowPtr);

            if ((int)windowPtr == -1 || (int)windowPtr == -2)
            {
                throw new Exception("Cannot create window instance!");
            }
            return form;
        }


        public static unsafe void GLmakeContexCurrent(Form form)
        {
            glIO.internalGLmakeContextCurrent((IntPtr)form.windowPtr);
        }


        public static unsafe bool GLwindowShouldClose(Form form)
        {
            int a = glIO.internalGLwindowShouldClose((IntPtr)form.windowPtr);
            return a != 0;
        }


        public static unsafe void GLswapBuffers(Form form)
        {
            glIO.internalGLswapBuffers((IntPtr)form.windowPtr);
        }

    }
}
