//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Form.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StgSharp.Graphics
{
    /// <summary>
    ///
    /// </summary>
    public abstract unsafe partial class Form
    {

        private int _frameCount;
        private int _frameSpeed;

        private short fpsCount;

        private IntPtr windowID;

        internal LinkedList<Control> allControl;
        internal Vector4 backgroundColor;
        internal TimeSpanProvider fpsCountProvider;
        internal TimeSpanProvider frameTimeProvider;
        internal IntPtr graphicContextID;
        internal int height;
        internal IntPtr monitor;
        internal string name;
        internal Form sharedForm;
        internal int width;

        protected Form()
        {
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback = new TimeSpanRefeshCallback(
                () =>
                {
                    Interlocked.Exchange(ref _frameSpeed, _frameCount);
                    Interlocked.Exchange(ref _frameCount, 0);
                    //Console.Clear();
                    //Console.WriteLine(_frameSpeed);
                });
        }

        public int CurrentFrameCount => frameTimeProvider.CurrentSpan;
        public int FrameSpeed => _frameSpeed;

        /// <summary>
        /// Height of this form, represent in <see cref="int"/>
        /// </summary>
        public int Height
        {
            get => height;
            set => height = value;
        }

        public bool ShouldClose
        {
            get => InternalIO.glfwWindowShouldClose(windowID) != 0;
            set => InternalIO.glfwSetWindowShouldClose(windowID, value ? 1 : 0);
        }

        /// <summary>
        /// Width of this form, represent in <see cref="int"/>
        /// </summary>
        public vec2d Size
        {
            get => new vec2d(width, height);
            set
            {
                width = (int)value.X;
                height = (int)value.Y;
            }
        }

        /// <summary>
        /// Width of this form, represent in <see cref="int"/>
        /// </summary>
        public int Width
        {
            get => width;
            set => width = value;
        }


        public unsafe IntPtr WindowID => windowID;

        internal unsafe IntPtr id => windowID;

        protected FrameBufferSizeHandler FrameBufferSizeCallback
        {
            set
            {
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(value);
                InternalIO.glfwSetFramebufferSizeCallback(windowID, handlerPtr);
            }
        }

        protected abstract void DeInit();

        /// <summary>
        /// Necessary operation to init graphic elements this form.
        /// </summary>
        /// <param name="gl">OpenGL library binded to current form.</param>
        protected abstract void Init();

        /// <summary>
        /// Behaviour of current form when in a frame cycle.
        /// </summary>
        protected abstract void Main();

        /// <summary>
        /// Init an OpenGl context if current form has not been binded to any context. and set the context binded to
        /// current form as current OpenGL context.
        /// </summary>
        /// <exception cref="Exception">
        /// Something goes wrong and StgSharp fails in creating a new OpenGL context.
        /// </exception>
        public unsafe void MakeAsCurrentContext()
        {
            if (windowID == default)
            {
                IntPtr windowPtr;
                byte[] tittleBytes = Encoding.UTF8.GetBytes(name);
                if (sharedForm == null)
                {
                    windowPtr = InternalIO.glfwCreateWindow(width, height, tittleBytes, monitor, IntPtr.Zero);
                }
                else
                {
                    windowPtr = InternalIO.glfwCreateWindow(width, height, tittleBytes, monitor, sharedForm.windowID);
                }
                if (((long)windowPtr == -1) || ((long)windowPtr == -2))
                {
                    throw new Exception("Cannot create window instance!");
                }
                else
                {
                    windowID = windowPtr;
                }

                graphicContextID = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GLcontext)));
                GLcontext* contextID = (GLcontext*)graphicContextID;
                *contextID = new GLcontext();
                contextID->glGetString = (delegate*<uint, IntPtr>)IntPtr.Zero;
            }
            GL.api = (GLcontext*)graphicContextID;
            InternalIO.glfwMakeContextCurrent(windowID);
        }


        /// <summary>
        /// Set all necessary metadata to current form instance
        /// </summary>
        public void SetMetadata(int width, int height, string name, IntPtr monitor, Form sharedForm)
        {
            this.width = width;
            this.height = height;
            this.name = name;
            this.sharedForm = sharedForm;
            this.monitor = monitor;
        }

        /// <summary>
        /// Resize the current form
        /// </summary>
        /// <param name="size">
        /// A <see cref="vec2d"/> (width,height) representing the new size of current form
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSize(vec2d size)
        {
            this.Size = size;
            InternalIO.glfwSetWindowSize(this.windowID, (int)size.X, (int)size.Y);
        }

        protected abstract void ProcessInput();

        /// <summary>
        /// Resize the current form
        /// </summary>
        /// <param name="width">New width of the window</param>
        /// <param name="height">New height of the window</param>
        public void SetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            InternalIO.glfwSetWindowSize(this.windowID, width, height);
        }

        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minSize">
        /// Minimun size of current form. In form of a <see cref="vec2d"/> of (width,height)
        /// </param>
        /// <param name="maxSize">
        /// Maximum size of current form. In form of a <see cref="vec2d"/> of (width,height)
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSizeLimit(vec2d minSize, vec2d maxSize)
        {
            InternalIO.glfwSetWindowSizeLimits(
                this.windowID,
                (int)minSize.X,
                (int)minSize.Y,
                (int)maxSize.X,
                (int)maxSize.Y);
        }

        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minWidth">Minimum width of current form</param>
        /// <param name="minHeight">Minimum height of current form</param>
        /// <param name="maxWidth">Maximum width of current form</param>
        /// <param name="maxHeight">Maximum height of current form</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight)
        { InternalIO.glfwSetWindowSizeLimits(this.windowID, minWidth, minHeight, maxWidth, maxHeight); }

        #region form main


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InternalDeInit() { DeInit(); }


        unsafe ~Form() { Marshal.FreeHGlobal(graphicContextID); }

        #endregion form main


    }


}