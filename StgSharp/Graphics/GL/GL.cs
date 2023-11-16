using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public static unsafe partial class GL
    {
        internal unsafe static GLcontext gl;

        /// <summary>
        /// sets the framebuffer resize callback of the specified <see cref="Form"/>,
        ///which is called when the framebuffer of the specified <see cref="Form"/> is resized.
        /// </summary>
        /// <param name="form">The Form instance to be controlled</param>
        /// <param name="handler">Method hadler </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetFrameBufferSizeCallback(Form form, FrameBufferSizeHandler handler)
        {
            InternalIO.glfw.SetFramebufferSizeCallback(form.windowID, handler);
        }


        /// <summary>
        /// Call glClearColor in OpenGL
        /// specifies the red, green, blue, and alpha values used by glClear to clear the color buffers. 
        /// Values specified by glClearColor are clamped to the range [0,1]
        /// </summary>
        /// <param name="R">Specify the red values used when the color buffers are cleared.</param>
        /// <param name="G">Specify the red values used when the color buffers are cleared.</param>
        /// <param name="B">Specify the red values used when the color buffers are cleared.</param>
        /// <param name="A">Specify the red values used when the color buffers are cleared.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ClearColor(float R, float G, float B, float A)
        {
            gl.ClearColor(R, G, B, A);
        }


        /// <summary>
        /// Conpile the provided shader code in string form.
        /// </summary>
        /// <param name="shaderCode">A string represent the shader's code</param>
        /// <param name="shaderType">An <see cref="ShaderType"/> enum represent the type of the shader to
        /// conpile, including vertex shader and fragment shader.</param>
        /// <returns>An uint value as the only mark of conpiled shader</returns>
        public static uint CompileShader(string shaderCode, ShaderType shaderType)
        {
            uint shaderID;
            shaderCode += '\0';
            shaderID = InternalIO.InternalLoadShaderCode(shaderCode, (int)shaderType);
            if (shaderID == 0)
            {
                IntPtr logPtr = InternalIO.InternalReadLog();
                string log;
                try
                {
                    byte[] logByte = new byte[512];
                    Marshal.Copy(logPtr, logByte, 0, 512);
                    log = Encoding.UTF8.GetString(logByte);
                    log = log.Replace("\0", "");

                    bool isUseful = false;
                    foreach (char item in log)
                    {
                        if (!char.IsControl(item))
                        {
                            isUseful = true; break;
                        }
                    }
                    if (!isUseful)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    log = "Failed to compile shader.\nFailed to get GLSL compiler log";
                }

                InternalIO.InternalWriteLog(log, LogType.Error);

            }
            return shaderID;
        }

        /// <summary>
        /// Creates a form and its associated 
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Required width</param>
        /// <param name="tittle">Tittle of the window </param>
        /// <param name="monitor"></param>
        /// <param name="share"></param>
        /// <returns>The instance of created form</returns>
        /// <exception cref="Exception"></exception>
        public static unsafe Form CreateWindow(
            int width, int height, string tittle, IntPtr monitor, Form share)
        {
            byte[] tittleBytes = Encoding.UTF8.GetBytes(tittle);
            IntPtr windowPtr;
            if (share == null)
            {
                windowPtr = InternalIO.glfw.CreateWindow(
                    width, height, tittleBytes, monitor, IntPtr.Zero);
            }
            else
            {
                windowPtr = InternalIO.glfw.CreateWindow(
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


        public static unsafe uint CreatGraphicProgram()
        {
            return gl.CreateProgram();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawElements(GepmetryType mode, int count, Type type, IntPtr ptr)
        {
            uint a;
            switch (type.Name)
            {
                case "UInt32":
                    a = GLconst.UNSIGNED_INT;
                    break;
                case "UInt16":
                    a = GLconst.UNSIGNED_SHORT;
                    break;
                case "Byte":
                    a = GLconst.UNSIGNED_BYTE;
                    break;
                default:
                    InternalIO.InternalWriteLog(
                        $"Parameter error in parameter {nameof(type)} method DrawElements. " +
                        $"Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are suporrted.",
                        LogType.Error);
                    return;
            }
            gl.DrawElements((uint)mode, count, a, (void*)ptr);
        }




        public static unsafe bool GetWindowShouldClose(Form form)
        {
            return InternalIO.glfw.WindowShouldClose(form.windowID) == 0;
        }


        public static unsafe void GLswapBuffers(Form form)
        {
            if (form.windowID == IntPtr.Zero)
            {
                return;
            }
            InternalIO.glfw.SwapBuffers(form.windowID);
        }


        public static unsafe void MakeContexCurrent(Form form)
        {
            if (form.windowID == IntPtr.Zero)
            {
                return;
            }
            InternalIO.glfw.MakeContextCurrent(form.windowID);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage) where T : IMat
        {
            int size = sizeof(T);
            gl.BufferData(
                (uint)bufferType,
                size,
                &bufferData,
                (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ClearGraphicError()
        {
            while (gl.GetError() != 0) { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetGraphicError()
        {
            string error = null;
            uint code = gl.GetError();
            while (code != 0)
            {
                error += $"OpenGL error {code}\n";
            }
            try
            {
                if (error != null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                /*
                InternalIO.InternalWriteLog(
                    error + ex.StackTrace,
                    LogType.Error
                    );
                /**/
            }
            return;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage) where T : struct, IConvertible
        {

            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));

            fixed (void* bufferPtr = &bufferArray[0])
                gl.BufferData(
                    (uint)bufferType,
                    size,
                    bufferPtr,
                    (uint)usage);
        }
    }



}
