//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ViewModel.cs"
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
using StgSharp.Controlling.UsrActivity;
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Math;
using StgSharp.MVVM.View;
using StgSharp.MVVM.ViewModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StgSharp.MVVM.ViewModel
{
    public abstract partial class ViewModelBase
    {

        private int _frameCount;
        private int _frameSpeed;
        private ViewPort viewPortDisplay;

        //internal IntPtr monitor;

        internal TimeSpanProvider fpsCountProvider;
        internal TimeSpanProvider frameTimeProvider;
        internal Vector4 backgroundColor;

        protected ViewModelBase(int frameSpeed)
        {
            viewPortDisplay = new ViewPort();
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback = CountFps;
            long frameLength = (FrameSpeed > 0) ? ((1000L * 1000L) / FrameSpeed) : 0;
            frameTimeProvider = new TimeSpanProvider(
                frameLength, TimeSpanProvider.DefaultProvider
                );
            frameTimeProvider.SpanRefreshCallback =
                () =>
                {
                };
        }

        protected ViewModelBase(float frameLength)
        {
            viewPortDisplay = new ViewPort();
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback = CountFps;
            long _frameLength = (FrameSpeed > 0) ? ((long)(1000 * frameLength)) : 0;
            frameTimeProvider = new TimeSpanProvider(
                _frameLength, TimeSpanProvider.DefaultProvider
                );
            frameTimeProvider.SpanRefreshCallback =
                () =>
                {
                };
        }

        public ViewModelBase(ViewPort vbinding, int framespeed)
        {
            viewPortDisplay = vbinding;
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback = CountFps;
            long frameLength = (framespeed > 0) ? ((1000L * 1000L) / framespeed) : 0;
            frameTimeProvider = new TimeSpanProvider(
                frameLength, TimeSpanProvider.DefaultProvider
                );
            frameTimeProvider.SpanRefreshCallback =
                () =>
                {
                };
            viewPortDisplay.FrameSizeCallback = (handle, width, height)
                =>
            {
                InternalIO.glfwMakeContextCurrent(viewPortDisplay.ViewPortID);
                GlFunction.CurrentGL.Viewport(0, 0, width, height);
                PrivateShow();
            };
            viewPortDisplay.FramePostionCallback = (handle, xpos, ypos)
                =>
            {
                PrivateShow();
            };
        }

        public ViewModelBase(ViewPort vbinding, float frameLength)
        {
            viewPortDisplay = vbinding;
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            fpsCountProvider.SpanRefreshCallback = CountFps;
            long _frameLength = (frameLength > 0) ? ((long)(1000 * frameLength)) : 0;
            frameTimeProvider = new TimeSpanProvider(
                _frameLength, TimeSpanProvider.DefaultProvider
                );
            frameTimeProvider.SpanRefreshCallback =
                () =>
                {
                };

            viewPortDisplay.FrameSizeCallback = (handle, width, height)
                =>
            {
                InternalIO.glfwMakeContextCurrent(viewPortDisplay.ViewPortID);
                GlFunction.CurrentGL.Viewport(0, 0, width, height);
                PrivateShow();
            };
            viewPortDisplay.FramePostionCallback = (handle, xpos, ypos)
                =>
            {
                PrivateShow();
            };
        }

        public DataBindingEntry this[string name]
        {
            get
            {
                if (this.TryGetObjectReference(name, out DataBindingEntry entry))
                {
                    return entry;
                }
                throw new KeyNotFoundException();
            }
        }

        public bool ShouldClose
        {
            get => InternalIO.glfwWindowShouldClose(viewPortDisplay.ViewPortID) != 0;
            set => InternalIO.glfwSetWindowShouldClose(viewPortDisplay.ViewPortID, value ? 1 : 0);
        }

        public int CurrentFrameCount => frameTimeProvider.CurrentSpan;

        public int FrameSpeed => _frameSpeed;

        /// <summary>
        /// Height of this form, represent in <see cref="int"/>
        /// </summary>
        public int Height
        {
            get => viewPortDisplay.Height;
            set { viewPortDisplay.Height = value; }
        }

        /// <summary>
        /// Width of this form, represent in <see cref="int"/>
        /// </summary>
        public int Width
        {
            get => viewPortDisplay.Width;
            set
            {
                viewPortDisplay.Width = value;
                if (WindowID != IntPtr.Zero)
                {
                    InternalIO.glfwSetWindowSize(viewPortDisplay.ViewPortID, Width, Height);
                }
            }
        }

        public unsafe IntPtr WindowID
        {
            get => viewPortDisplay.ViewPortID;
        }

        public string Name
        {
            get => viewPortDisplay.Name;
        }

        /// <summary>
        /// Width of this form, represent in <see cref="int"/>
        /// </summary>
        public vec2d Size
        {
            get => new vec2d(Width, Height);
            set
            {
                Width = (int)value.X;
                Height = (int)value.Y;
                if (WindowID != IntPtr.Zero)
                {
                    InternalIO.glfwSetWindowSize(viewPortDisplay.ViewPortID, Width, Height);
                }
            }
        }

        public ViewPort ViewPortBinding
        {
            get => viewPortDisplay;
        }

#pragma warning disable CA1044
        protected FrameBufferSizeHandler FrameSizeCallback
        {
            set
            {
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(value);
                InternalIO.glfwSetFramebufferSizeCallback(viewPortDisplay.ViewPortID, handlerPtr);
            }
        }
#pragma warning restore CA1044


        public abstract void CustomizeInitialize();

        public Action MethodLookup(string methodName)
        {
            MethodInfo method = GetType().GetMethod(methodName);
            return (method != null) ? ((Action)Delegate.CreateDelegate(typeof(Action), this, method)) : null;
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
            InternalIO.glfwSetWindowSize(this.viewPortDisplay.ViewPortID, (int)size.X, (int)size.Y);
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
                this.viewPortDisplay.ViewPortID,
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
            InternalIO.glfwSetWindowSizeLimits(this.viewPortDisplay.ViewPortID, minWidth, minHeight, maxWidth, maxHeight);
        }

        public abstract bool TryGetObjectReference(string name, out DataBindingEntry entry);

        private void CountFps()
        {
            /**/
            Console.Clear();
            Console.WriteLine(FrameSpeed);
            /**/
            Interlocked.Exchange(ref _frameSpeed, _frameCount);
            Interlocked.Exchange(ref _frameCount, 0);
        }

    }

}
