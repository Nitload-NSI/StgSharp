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

        protected Form()
        {
        }

        /// <summary>
        /// OpenGL package binded to current form context.
        /// A OpenGL context is only binded to current form.
        /// </summary>
        public IglFunc GL => gl;

        /// <summary>
        /// Height of this form, represent in <see cref="int"/>
        /// </summary>
        public int Height
        {
            get => height;
            set => height = value;
        }

        /// <summary>
        /// Representing
        /// </summary>
        public bool IsDoubleRendering => isSingleThread;

        public bool ShouldClose
        {
            get => InternalIO.glfwWindowShouldClose(windowID) != 0;

            set => InternalIO.glfwSetWindowShouldClose(windowID, value ? 1 : 0);
        }

        /// <summary>
        /// Width of this form, represent in <see cref="int"/>
        /// </summary>
        public Vec2d Size
        {
            get => new Vec2d(width, height);
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

        protected FrameBufferSizeHandler FrameBufferSizeCallback
        {
            set
            {
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(value);
                InternalIO.glfwSetFramebufferSizeCallback(windowID, handlerPtr);
            }
        }

        internal unsafe IntPtr id => windowID;

        internal bool IsSingleThread => isSingleThread;



        public abstract void Deinit(params object[] args);

        /// <summary>
        /// Necessary operation to init this form.
        /// </summary>
        /// <param name="args"></param>
        public abstract void Init(params object[] args);

        /// <summary>
        /// Behaviour of current form when in a frame cycle.
        /// </summary>
        /// <param name="args"></param>
        public abstract void Main();

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
                gl = new GLonRender(contextID);
            }
            InternalIO.glfwMakeContextCurrent(windowID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void PollEvents()
        {
            InternalIO.glfwPollEvents();
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
        /// A <see cref="Vec2d"/> (width,height) representing the new size of current form
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSize(Vec2d size)
        {
            this.Size = size;
            InternalIO.glfwSetWindowSize(this.windowID, (int)size.X, (int)size.Y);
        }

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
        /// Minimun size of current form. In form of a <see cref="Vec2d"/> of (width,height)
        /// </param>
        /// <param name="maxSize">
        /// Maximum size of current form. In form of a <see cref="Vec2d"/> of (width,height)
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSizeLimit(Vec2d minSize, Vec2d maxSize)
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
        /// <param name="maxWidth">Maximum width of curent form</param>
        /// <param name="maxHeight">Maximum height of current form</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight)
        { InternalIO.glfwSetWindowSizeLimits(this.windowID, minWidth, minHeight, maxWidth, maxHeight); }

        #region metadata
        internal LinkedList<Control> allControl;
        internal Vector4 backgroundColor;
        internal IglFunc gl;
        internal IntPtr graphicContextID;
        internal int height;
        internal IntPtr monitor;
        internal string name;
        internal Form sharedForm;
        internal int width;
        private IntPtr windowID;

        #endregion metadata

        #region form time

        private short fpsCount;
        internal TimeSpanProvider frameTimeProvider;
        internal TimeSpanProvider fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);

        #endregion


        #region form thread setting
        private bool isSingleThread = true;
        private List<FrameAsyncThread> frameAsyncTaskList = new List<FrameAsyncThread>();
        #endregion form thread setting


        #region form main

        /// <summary>
        /// Init this form and render it on screen, and respond with operation
        /// </summary>
        public unsafe void Show()
        {
            if (gl == null)
            {
                InternalIO.InternalWriteLog($"Attempt to show form {name} before init", LogType.Error);
            }
            else
            {
                frameTimeProvider = new TimeSpanProvider(1 / 60.0, TimeSpanProvider.DefaultProvider);
                if (isSingleThread)
                {
                    while (!ShouldClose)
                    {
                        Main();
                        SwapBuffers();
                        PollEvents();
                        frameTimeProvider.WaitNextSpan();
                        Console.WriteLine(frameTimeProvider.CurrentSpan);
                    }
                }
                else
                {
                    GLonUpdate update = new GLonUpdate((GLcontext*)graphicContextID);   //更换GL库至双线程模式
                    AsyncUpdateDataTask updateT = new AsyncUpdateDataTask(this);             //建立两个异步线程
                    AsyncRenderFrameTask renderT = new AsyncRenderFrameTask(this);

                    frameAsyncTaskList.Add(renderT);
                    frameAsyncTaskList.Add(updateT);
                    gl = update;

                    update.ActivatedOperationBuffer = updateT.opBuffer;
                    Barrier frameBarrier = FrameAsyncThread.CombineBeginningAsync(true, renderT, updateT);
                    Barrier swapBufferBarrier = renderT.TodoCompleteBarrier;
                    Barrier exchangeBufferBarrier = updateT.TodoCompleteBarrier;
                    renderT.AsyncStart();
                    updateT.AsyncStart();

                    while (!ShouldClose)
                    {

                        swapBufferBarrier.SignalAndWait();
                        SwapBuffers();                                             //绘制帧必要操作
                        PollEvents();

                        exchangeBufferBarrier.SignalAndWait();
                        FrameAsyncThread.ExchangeBuffer(updateT, renderT);         //交换两个线程的缓冲区
                        update.ActivatedOperationBuffer = updateT.opBuffer;

                        frameTimeProvider.WaitNextSpan();                          //等待下一帧开始
                        frameBarrier.SignalAndWait();
                        Console.WriteLine(frameTimeProvider.CurrentSpan);
                    }

                    updateT.Terminate();
                    renderT.Terminate();
                }
            }
        }

        public int CurrentFrameCount
=> frameTimeProvider.CurrentSpan;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InternalDeinit(params object[] args) { Deinit(args); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void SwapBuffers()
        {
            InternalIO.glfwSwapBuffers(windowID);
        }

        /// <summary>
        /// Get OpenGL library of this form's context.
        /// </summary>
        /// <returns>A <see cref="IglFunc"/> package contains all opengl APIs</returns>
        protected IglFunc GetGL()
        {
            if (StgSharp.API == GraphicAPI.GL)
            {
                return this.GL;
            }
            else
            {
                InternalIO.InternalWriteLog(
                    "Attempt to get OpenGL API package in other graphic envitronment.",
                    LogType.Error);
                throw new InvalidOperationException("Attempt to get OpenGL API package in other graphic envitronment.");
            }
        }

        /// <summary>
        /// Define how current form render in one frame.
        /// </summary>
        /// <param name="leastThreds">
        /// Please define this befor calling any OpenGL api and other graphic methods.
        /// </param>
        protected unsafe void SetRenderThreads(int leastThreds)
        {
            if (gl != default)
            {
                throw new InvalidOperationException("Cannot define threads after loading gl context.");
            }
            if (leastThreds == 1)
            {
                isSingleThread = true;
            }
            else if (leastThreds >= 2)
            {
                isSingleThread = false;
            }
            else
            {
                throw new ArgumentException("Amount of threads should be bigger than 0");
            }
        }

        unsafe ~Form() { Marshal.FreeHGlobal(graphicContextID); }



        #endregion form main


    }
}