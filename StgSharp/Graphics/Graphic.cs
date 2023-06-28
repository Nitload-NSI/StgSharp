using StgSharp.Control;
using StgSharp.Graphics;
using System;

namespace StgSharp
{
    public static unsafe partial class Graphic
    {
        internal static GraphicIOPack glIO;
        private static bool isGLSet = false;

        /// <summary>
        /// Init an instance of OpenGL program,
        /// This method should be called before other 
        /// graphic methods, after LoadGL method.
        /// </summary>
        public static void InitGL()
        {
            internalIO.InternalInitGL();
        }

        /// <summary>
        /// Load OpenGL API to StgSharp core programm,
        /// please call this method before call any other
        /// graphic APIs in StgSharp.
        /// </summary>
        /// <exception cref="Exception">If run this method twice, will throw exception</exception>
        public static unsafe void LoadGL()
        {
            if (!isGLSet)
            {
                glIO = internalIO.InternalInitGraphicApi();
            }
            else
            {
                throw new Exception("Do not load GL api twice.");
            }
        }

        /// <summary>
        /// Attach GLFW API to GLAD, 
        /// this method should be called after LoadGL 
        /// and before other graphic method.
        /// </summary>
        /// <exception cref="Exception">The accident that program cannot attach GLFW to GLAD</exception>
        public static unsafe void InitGLAD()
        {
            int a = internalIO.InternalInitGLAD();
            if (a!=0)
            {
                throw new Exception("Cannot attach GLFW to GLAD!");
            }
            else
            {
                return;
            }
        }


        internal static byte[] loadShaderCodeStr(string str)
        {
            byte[] cahrStr; 
            try
            {
                cahrStr = System.Text.Encoding.ASCII.GetBytes(str);
                return cahrStr;
            }
            catch (Exception)
            {
                throw;
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
