using StgSharp.Controlling;
using StgSharp.Graphics;
using System;
using System.Runtime.InteropServices;

namespace StgSharp
{
    public static unsafe partial class Graphic
    {
        private static bool isGLSet = false;
        private static bool isGLADset = false;
        private static bool isGLready = false;



        /// <summary>
        /// Init an instance of OpenGL program,
        /// This method should be called before other 
        /// graphic methods, after LoadGL method.
        /// </summary>
        public static void InitGL(int majorVersion, int minorVersion)
        {
            if (!isGLSet)
            {
                internalIO.InternalInitGL(majorVersion, minorVersion);
                isGLready = isGLSet && isGLADset;
            }
            else
            {
                throw new Exception("Do not init GL twice");
            }
        }



        /// <summary>
        /// Attach GLFW API to GLAD, 
        /// this method should be called after LoadGL 
        /// and before other graphic method.
        /// </summary>
        /// <exception cref="Exception">The accident that program cannot attach GLFW to GLAD</exception>
        public static unsafe void InitGLAD(glLoader loader)
        {

            if (!isGLADset)
            {
                int a = internalIO.InternalInitGLAD();
                if (a == 0)
                {
                    throw new Exception("Cannot attach GLFW to GLAD!");
                }
                else
                {
                    isGLready = isGLSet && isGLADset;

                    LoadGLLoader(loader);

                    return;
                }
            }
            else
            {
                throw new Exception("Do not init GLAD twice.");
            }
        }





        /*
        /// <summary>
        /// Init a form, attach its data to GLFW program, 
        /// and return the pointer of the GLFW package of the form.
        /// </summary>
        /// <param name="form">The C# instance of the form</param>
        /// <returns>Pointer of the GLFW package of this form</returns>
        /// <exception cref="Exception">The issue that form is not successfully initilized
        /// or GLAD program fail to init</exception>
        public static unsafe IntPtr InitWindow(Form form)
        {
            IntPtr a = internalIO.InternalInitWindow(form.info);
            if ((long)a == -1)
            {
                throw new Exception("Failed to init form.");
            }
            else if ((long)a == -2)
            {
                throw new Exception("Failed to load GLAD program.");
            }
            else
            {
                return a;
            }
        }
        */




    }
}
