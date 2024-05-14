//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glRenderStream.cs"
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
using StgSharp.Logic;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public abstract partial class glRenderStream : RenderStream
    {

        protected glRenderStream(RenderStreamConstructArgs args) : base(args)
        {
        }

        protected IntPtr glContextHandle
        {
            get { return ContextHandle; }
            set { ContextHandle = value; }
        }

        public static T AsMainStreamOf<T>(Form f, (int x, int y, int z) unitCubeSize) where T : glRenderStream
        {
            RenderStreamConstructArgs args = new RenderStreamConstructArgs();
            args.UnitCubeSize = unitCubeSize;
            args.Name = f.name;
            args.Height = f.Height;
            args.Width = f.Width;
            T ret = Activator.CreateInstance(typeof(T), args) as T;
            ret!.InternalInit();
            f.WindowID = ret.CanvasHandle;
            return ret;
        }

        /// <summary>
        /// Init an OpenGl context if current form has not been binded to any context. and set the context binded to
        /// current form as current OpenGL context.
        /// </summary>
        /// <exception cref="Exception">
        /// Something goes wrong and StgSharp fails in creating a new OpenGL context.
        /// </exception>
        public unsafe void MakeAsCurrentContext()
        {
            if (ContextHandle == default)
            {
                ContextHandle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GLcontext)));
                GLcontext* contextID = (GLcontext*)ContextHandle;
                contextID->glGetString = (delegate*<uint, IntPtr>)IntPtr.Zero;
            }
            if (GL.CurrentContextHandle != ContextHandle)
            {
                GL.CurrentContextHandle = ContextHandle;
                //Console.WriteLine($"context set as {(ulong)GL.CurrentContextHandle}");
                InternalIO.glfwMakeContextCurrent(CanvasHandle);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            InternalIO.glfwSetWindowSizeLimits(this.CanvasHandle, minWidth, minHeight, maxWidth, maxHeight);
        }

        internal override sealed unsafe void CreateCanvas()
        {
            byte[] nameCode = Encoding.UTF8.GetBytes(name);
            CanvasHandle = InternalIO.glfwCreateWindow(
                Width, Height, nameCode, Monitor, IntPtr.Zero
                );
            NativeCamera = new Camera();
        }

        internal override sealed void InternalDraw(
            in Dictionary<string, BlueprintPipeline> input,
            in Dictionary<string, BlueprintPipeline> output)
        {
            MakeAsCurrentContext();
            ProcessInput();
            Main(in input, in output);
        }
        internal override sealed void InternalInit()
        {
            CreateCanvas();
            MakeAsCurrentContext();
            this.LoadGLapi();
            Init();
        }

        internal override sealed void InternalTerminate()
        {
            Deinit();
        }

        protected override sealed Uniform<Matrix44> NativeCameraUniform(ShaderProgram source, string name)
        {
            NativeCamera.GainAllUniforms(source, name);
            return NativeCamera.convertedUniform;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override sealed void NativeCameraViewRange(Radius fieldOfRange, vec2d offset, (float frontDepth, float backDepth) viewDepth)
        {
            NativeCamera.SetViewRange(fieldOfRange, this.Size, offset, viewDepth);
        }

        protected override sealed void NativeCameraViewTarget(vec3d position, vec3d direction, vec3d up)
        {
            NativeCamera.SetViewDirection(position, direction, up);
        }

        protected static void PollEvents()
        {
            InternalIO.glfwPollEvents();
        }

        protected void SwapBuffers()
        {
            InternalIO.glfwSwapBuffers(CanvasHandle);
        }

        #region Key Control

        protected KeyStatus CheckKey(KeyboardKey key)
        {
            return InternalIO.glfwGetKey(this.CanvasHandle, (int)key);
        }

        protected KeyStatus CheckKey(Mouse key)
        {
            return InternalIO.glfwGetKey(this.CanvasHandle, (int)key);
        }

        protected KeyStatus CheckKey(Joystick key)
        {
            return InternalIO.glfwGetKey(this.CanvasHandle, (int)key);
        }

        #endregion

    }
}
