//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RenderStream"
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
using StgSharp.Graphics.OpenGL;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;
using StgSharp.MVVM;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public abstract class RenderStream
    {

        private Camera nativeCamera;

        private protected ViewPort primeArgs;
        internal TimeSpanProvider _timeProvider;

        public (int width, int height) Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (Width, Height);
        }

        public abstract bool IsContextSharable { get; }

        public int Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => primeArgs.Height;
        }

        public int Width
        {
            get => primeArgs.Width;
        }

        public IntPtr Monitor
        {
            get => primeArgs.Monitor;
            internal set => primeArgs.Monitor = value;
        }

        public string Name
        {
            get => primeArgs.Name;
        }

        public ViewPort BindedViewPortContext
        {
            get => primeArgs;
            internal set => primeArgs = value;
        }

        internal IntPtr CanvasHandle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => primeArgs.ViewPortHandle;
        }

        internal IntPtr ContextHandle
        {
            get => primeArgs.GraphicHandle;
            private protected set => primeArgs.GraphicHandle = value;
        }

        protected internal Camera NativeCamera
        {
            get => nativeCamera;
            internal set => nativeCamera = value;
        }

        protected float FramePassed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeProvider.CurrentSpan;
        }

        protected int PassedFrames
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeProvider.CurrentSpan;
        }

        protected float TimePassed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeProvider.CurrentSecond;
        }

        protected TimeSpanProvider TimeProvider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeProvider;
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
        public abstract void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight);

        public static TTarget ShareContextFrom<TSource, TTarget>([NotNull]TSource source) where TSource: RenderStream
            where TTarget: RenderStream, new()
        {
            TTarget target = new TTarget();
            target.Initialize(source.BindedViewPortContext, source._timeProvider);
            return target;
        }

        /**/
        protected internal static void PollEvents()
        {
            GraphicFramework.glfwPollEvents();
        }

        private protected void SwapBuffers()
        {
            GraphicFramework.glfwSwapBuffers(CanvasHandle);
        }

        /**/

        #region native camera operation

        protected abstract void NativeCameraViewRange(
                                Radius fieldOfRange,
                                Vec2 offset,
                                (float frontDepth, float backDepth) viewDepth);

        protected abstract void NativeCameraViewTarget(Vec3 position, Vec3 direction, Vec3 up);

        /// <summary>
        ///   Get <see cref="Uniform{T}" />(TView is <see cref="Matrix44" />) uniform from a shader.
        ///   This uniform will be used as projection matrix of this camera. Additionally, for
        ///   multi- shader rendering, each shader requires a uniform,  so do not ignore return
        ///   value of this method.
        /// </summary>
        /// <param _label="source">
        ///   Shader using this camera
        /// </param>
        /// <param _label="name">
        ///   ContextName of projection camera named in shader
        /// </param>
        /// <returns>
        ///   <see cref="Uniform{T}" />(TView is <see cref="Matrix44" />) representing  projection
        ///   matrix in shader program.
        /// </returns>
        protected abstract Uniform<Matrix44> NativeCameraUniform(ShaderProgram source, string name);

        #endregion

        #region internal operation

        public void Initialize(ViewPort v, TimeSpanProvider timeProvider)
        {
            if (v == null) {
                throw new ArgumentNullException(nameof(v));
            }
            primeArgs = v;
            this._timeProvider = timeProvider;
            nativeCamera = new Camera();
            PlatformSpecifiedInitialize();
            CustomizeInit();
            GraphicFramework.glfwMakeContextCurrent(IntPtr.Zero);
        }

        internal abstract void PlatformSpecifiedInitialize();

        internal abstract void Terminate();

        #endregion

        #region public operation

        protected abstract void CustomizeDeinit();

        protected abstract void CustomizeInit();

        /// <summary>
        ///   Marking the start of current render operations,  and set this renser context as
        ///   current.
        /// </summary>
        public abstract void RenderStart();

        /// <summary>
        ///   Marking the end of current render operations,  and release the binding of this renser
        ///   context to current,  making it available for another thread as current.
        /// </summary>
        public abstract void RenderEnd();

        #endregion

        #region public resource generator

        protected abstract Shader CreateShaderSegment(ShaderType type, int count);

        protected abstract ShaderProgram CreateShaderProgram();

    #endregion
    }
}
