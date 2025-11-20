//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ViewModel"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Common.Timing;
using StgSharp.Controlling.UsrActivity;
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;
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
            allView = [];
            fpsCountProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, 1.0 / frameSpeed), TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long frameLength = (FrameSpeed > 0) ? (1000L * 1000L / FrameSpeed) : 0;
            frameTimeProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, 1.0 / frameSpeed), TimeSpanProvider.DefaultProvider);
            SetFrameSizeCallback(DefaultFrameResizeCallback);
        }

        protected ViewModelBase(double frameLength)
        {
            viewPortDisplay = new ViewPort();
            allView = [];
            fpsCountProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, frameLength), TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long _frameLength = (FrameSpeed > 0) ? ((long)(1000 * frameLength)) : 0;
            frameTimeProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, frameLength), TimeSpanProvider.DefaultProvider);
            SetFrameSizeCallback(DefaultFrameResizeCallback);
        }

        public ViewModelBase(ViewPort vBinding, int frameSpeed)
        {
            viewPortDisplay = vBinding;
            allView = [];
            fpsCountProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, 1.0), TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long frameLength = (frameSpeed > 0) ? (1000L * 1000L / frameSpeed) : 0;
            frameTimeProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, 1.0 / frameSpeed), TimeSpanProvider.DefaultProvider);
            SetFrameSizeCallback(DefaultFrameResizeCallback);
        }

        public ViewModelBase(ViewPort vBinding, double frameLength)
        {
            viewPortDisplay = vBinding;
            allView = [];
            fpsCountProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, 1.0), TimeSpanProvider.DefaultProvider);
            _frameSpeed = 0;
            _frameCount = 0;
            long _frameLength = (FrameSpeed > 0) ? ((long)(1000 * frameLength)) : 0;
            frameTimeProvider = new TimeSpanProvider(TimeSequence.CreateLinear(double.MaxValue, frameLength), TimeSpanProvider.DefaultProvider);
            SetFrameSizeCallback(DefaultFrameResizeCallback);
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
            get => GraphicFramework.glfwWindowShouldClose(viewPortDisplay.ViewPortHandle) != 0;
            set => GraphicFramework.glfwSetWindowShouldClose(
                viewPortDisplay.ViewPortHandle, value ? 1 : 0);
        }

        public int CurrentFrameCount => frameTimeProvider.CurrentSpan;

        public int FrameSpeed => _frameSpeed;

        /// <summary>
        ///   Height of this form, represent in <see cref="int" />
        /// </summary>
        public int Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => viewPortDisplay.Height;
        }

        /// <summary>
        ///   Width of this form, represent in <see cref="int" />
        /// </summary>
        public int Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => viewPortDisplay.Width;
        }

        public unsafe IntPtr WindowID
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => viewPortDisplay.ViewPortHandle;
        }

        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => viewPortDisplay.Name;
        }

        /// <summary>
        ///   Width of this form, represent in <see cref="int" />
        /// </summary>
        public Vec2 Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Vec2(Width, Height);
        }

        public ViewPort ViewPortBinding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => viewPortDisplay;
        }

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
            GraphicFramework.glfwSetWindowSizeLimits(this.viewPortDisplay.ViewPortHandle, (int)minSize.X,
                                                     (int)minSize.Y, (int)maxSize.X, (int)maxSize.Y);
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
            GraphicFramework.glfwSetWindowSizeLimits(this.viewPortDisplay.ViewPortHandle, minWidth, minHeight, maxWidth,
                                                     maxHeight);
        }

        public abstract bool TryGetObjectReference(string name, out DataBindingEntry entry);

        protected unsafe void SetFrameSizeCallback(FrameBufferSizeHandler callback)
        {
            _resizeCallback = callback;
            IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(callback);
            _ = GraphicFramework.glfwSetFrameBufferSizeCallback(
                    viewPortDisplay.ViewPortHandle, (delegate* unmanaged[Cdecl]<IntPtr, int, int, void>)handlerPtr);
        }

        private void DefaultFrameResizeCallback(IntPtr windowHandle, int width, int height)
        {
            ViewPort.GetExistedViewPort(windowHandle, out ViewPort vp);
            vp.RequestFlushSizeInNextFrame(width, height);

            // Console.WriteLine($"{width}   {height}");
        }

    }
}
