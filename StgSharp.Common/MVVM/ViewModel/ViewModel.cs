//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ViewModel.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Controlling.UsrActivity;
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Math;
using StgSharp.MVVM;
using StgSharp.MVVM.ViewModel;
using StgSharp.Threading;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using static StgSharp.Internal.InternalIO;

namespace StgSharp.MVVM.ViewModel
{
    public abstract partial class ViewModelBase
    {

        private FrameBufferSizeHandler _resizeCallback;
        private int _frameCount;
        private int _frameSpeed;
        private ViewPort viewPortDisplay;

        // internal IntPtr monitor;

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
            long frameLength = (FrameSpeed > 0) ? ((1000L * 1000L) / FrameSpeed) : 0;
            frameTimeProvider = new TimeSpanProvider(frameLength, TimeSpanProvider.DefaultProvider);
            FrameSizeCallback = DefaultFrameRewsizeCallback;
        }

        protected ViewModelBase(float frameLength)
        {
            viewPortDisplay = new ViewPort();
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long _frameLength = (FrameSpeed > 0) ? ((long)(1000 * frameLength)) : 0;
            frameTimeProvider = new TimeSpanProvider(_frameLength,
                                                     TimeSpanProvider.DefaultProvider);
            FrameSizeCallback = DefaultFrameRewsizeCallback;
        }

        public ViewModelBase(ViewPort vBinding, int frameSpeed)
        {
            viewPortDisplay = vBinding;
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long frameLength = (frameSpeed > 0) ? ((1000L * 1000L) / frameSpeed) : 0;
            frameTimeProvider = new TimeSpanProvider(frameLength, TimeSpanProvider.DefaultProvider);
            FrameSizeCallback = DefaultFrameRewsizeCallback;
        }

        public ViewModelBase(ViewPort vBinding, float frameLength)
        {
            viewPortDisplay = vBinding;
            allView = new();
            fpsCountProvider = new TimeSpanProvider(1.0, TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long _frameLength = (frameLength > 0) ? ((long)(1000 * frameLength)) : 0;
            frameTimeProvider = new TimeSpanProvider(_frameLength,
                                                     TimeSpanProvider.DefaultProvider);
            FrameSizeCallback = DefaultFrameRewsizeCallback;
        }

        public DataBindingEntry this[string name]
        {
            get
            {
                if (this.TryGetObjectReference(name, out DataBindingEntry entry)) {
                    return entry;
                }
                throw new KeyNotFoundException();
            }
        }

        public bool ShouldClose
        {
            get => InternalIO.glfwWindowShouldClose(viewPortDisplay.ViewPortHandle) != 0;
            set => InternalIO.glfwSetWindowShouldClose(
                viewPortDisplay.ViewPortHandle, value ? 1 : 0);
        }

        public int CurrentFrameCount => frameTimeProvider.CurrentSpan;

        public int FrameSpeed => _frameSpeed;

        /// <summary>
        ///   Height of this form, represent in <see cref="int" />
        /// </summary>
        public int Height
        {
            get => viewPortDisplay.Height;
        }

        /// <summary>
        ///   Width of this form, represent in <see cref="int" />
        /// </summary>
        public int Width
        {
            get => viewPortDisplay.Width;
        }

        public unsafe IntPtr WindowID
        {
            get => viewPortDisplay.ViewPortHandle;
        }

        public string Name
        {
            get => viewPortDisplay.Name;
        }

        /// <summary>
        ///   Width of this form, represent in <see cref="int" />
        /// </summary>
        public Vec2 Size
        {
            get => new Vec2(Width, Height);
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
                _resizeCallback = value;
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(value);
                InternalIO.glfwSetFramebufferSizeCallback(
                    viewPortDisplay.ViewPortHandle, handlerPtr);
            }
        }
        #pragma warning restore CA1044

        public abstract void CustomizeInitialize();

        /// <summary>
        ///   Set the Size limit of current form
        /// </summary>
        /// <param _label="minSize">
        ///   Minimum Size of current form. In form of a <see cref="Vec2" /> of (width,height)
        /// </param>
        /// <param _label="maxSize">
        ///   Maximum Size of current form. In form of a <see cref="Vec2" /> of (width,height)
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSizeLimit(Vec2 minSize, Vec2 maxSize)
        {
            ThreadHelper.MainThreadAssert();
            InternalIO.glfwSetWindowSizeLimits(
                this.viewPortDisplay.ViewPortHandle, (int)minSize.X, (int)minSize.Y, (int)maxSize.X,
                (int)maxSize.Y);
        }

        /// <summary>
        ///   Set the Size limit of current form
        /// </summary>
        /// <param _label="minWidth">
        ///   Minimum width of current form
        /// </param>
        /// <param _label="minHeight">
        ///   Minimum height of current form
        /// </param>
        /// <param _label="maxWidth">
        ///   Maximum width of current form
        /// </param>
        /// <param _label="maxHeight">
        ///   Maximum height of current form
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            ThreadHelper.MainThreadAssert();
            InternalIO.glfwSetWindowSizeLimits(
                this.viewPortDisplay.ViewPortHandle, minWidth, minHeight, maxWidth, maxHeight);
        }

        public abstract bool TryGetObjectReference(string name, out DataBindingEntry entry);

        private void DefaultFrameRewsizeCallback(IntPtr windowHandle, int width, int height)
        {
            ViewPort.GetExistedViewPort(windowHandle, out ViewPort vp);
            vp.RequestFlushSizeInNextFrame(width, height);

            // Console.WriteLine($"{width}   {height}");
        }

    }
}
