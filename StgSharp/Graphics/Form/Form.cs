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
using System.Diagnostics;
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

        private IntPtr windowID;

        private short fpsCount;
        internal Form sharedForm;

        internal int height;
        internal int width;
        internal IntPtr monitor;

        internal LinkedList<Control> allControl;
        internal string name;
        internal TimeSpanProvider fpsCountProvider;
        internal TimeSpanProvider frameTimeProvider;
        internal Vector4 backgroundColor;

        protected Form((int width, int height) size, string name)
        {
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback =
                () =>
                {
                    Interlocked.Exchange(ref _frameSpeed, _frameCount);
                    Interlocked.Exchange(ref _frameCount, 0);
                    Console.Clear();
                    Console.WriteLine(_frameSpeed);
                };
            frameTimeProvider = new TimeSpanProvider(
                (1000L * 1000L) / 60, TimeSpanProvider.DefaultProvider
                );
            frameTimeProvider.SpanRefreshCallback =
                () =>
                {
                };
            height = size.height;
            width = size.width;
            this.name = name;
        }

        public Form()
        {
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback =
                () =>
                {
                    Interlocked.Exchange(ref _frameSpeed, _frameCount);
                    Interlocked.Exchange(ref _frameCount, 0);
                    Console.Clear();
                    Console.WriteLine(_frameSpeed);
                };
            frameTimeProvider = new TimeSpanProvider(
                (1000L * 1000L) / 60, TimeSpanProvider.DefaultProvider
                );
            frameTimeProvider.SpanRefreshCallback =
                () =>
                {
                };
            height = 800;
            width = 600;
            name = new StackFrame(1, false).
                GetMethod()!.DeclaringType!.Name;
        }

        public int CurrentFrameCount => frameTimeProvider.CurrentSpan;
        public int FrameSpeed => _frameSpeed;

        /// <summary>
        /// Height of this form, represent in <see cref="int"/>
        /// </summary>
        public int Height
        {
            get => height;
            set
            {
                height = value;
                if (WindowID != IntPtr.Zero)
                {
                    InternalIO.glfwSetWindowSize(windowID, Width, Height);
                }
            }
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
                if (WindowID != IntPtr.Zero)
                {
                    InternalIO.glfwSetWindowSize(windowID, Width, Height);
                }
            }
        }

        /// <summary>
        /// Width of this form, represent in <see cref="int"/>
        /// </summary>
        public int Width
        {
            get => width;
            set
            {
                width = value;
                if (WindowID != IntPtr.Zero)
                {
                    InternalIO.glfwSetWindowSize(windowID, Width, Height);
                }
            }
        }


        public unsafe IntPtr WindowID
        {
            get => windowID;
            internal set => windowID = value;
        }

        internal unsafe IntPtr id => windowID;

#pragma warning disable CA1044
        protected FrameBufferSizeHandler FrameSizeCallback
        {
            set
            {
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(value);
                InternalIO.glfwSetFramebufferSizeCallback(windowID, handlerPtr);
            }
        }
#pragma warning restore CA1044

        #region Key Control
        
        protected KeyStatus CheckKey(KeyboardKey key)
        {
            return InternalIO.glfwGetKey(this.WindowID,(int)key);
        }

        protected KeyStatus CheckKey(Mouse key)
        {
            return InternalIO.glfwGetKey(this.WindowID, (int)key);
        }

        protected KeyStatus CheckKey(Joystick key)
        {
            return InternalIO.glfwGetKey(this.WindowID, (int)key);
        }

        #endregion

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


        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minSize">
        /// Minimum size of current form. In form of a <see cref="vec2d"/> of (width,height)
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
        {
            //TODO 重做form的尺寸极限控制
            InternalIO.glfwSetWindowSizeLimits(this.windowID, minWidth, minHeight, maxWidth, maxHeight);
        }

    }


}