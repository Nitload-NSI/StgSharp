// -----------------------------------------------------------------------------
// file="glRenderStream"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;
using StgSharp.Threading;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    public abstract partial class glRender : RenderStream
    {

        private protected static readonly ConcurrentDictionary<glRender, Thread> renderToThread
            = new ConcurrentDictionary<glRender, Thread>();

        private OpenGLFunction _context;

        protected internal OpenGLFunction GL => _context;

        /// <summary>
        ///   Initialize an opengl context and load all opengl functions.
        /// </summary>
        public unsafe void InitializeContext()
        {
            if (ContextHandle == default)
            {
                ContextHandle = Marshal.AllocHGlobal(Marshal.SizeOf<OpenglContext>());
                OpenglContext* contextID = (OpenglContext*)ContextHandle;
                contextID->glGetString = (delegate*<uint,byte*>)IntPtr.Zero;
            }
            if (GL == null) {
                _context = OpenGLFunction.BuildGlFunctionPackage(ContextHandle);
            }
        }

        /// <summary>
        ///   Initialize an OpenGl viewPortDisplay if current form has not been binded to any
        ///   viewPortDisplay.  and set the viewPortDisplay binded to current form as current OpenGL
        ///   viewPortDisplay.
        /// </summary>
        /// <exception cref="Exception">
        ///   Something goes wrong and World fails in creating a new OpenGL viewPortDisplay.
        /// </exception>
        public unsafe void MakeAsCurrentContext()
        {
            GraphicFramework.glfwMakeContextCurrent(CanvasHandle);
            OpenGLFunction.CurrentGL = this.GL;
        }

        /// <inheritdoc />
        public override void RenderEnd()
        {
            if (!renderToThread.TryGetValue(this, out Thread t) || t != Thread.CurrentThread) {
                throw new InvalidOperationException(
                    $"Current render has had been binded to thread {t!.ManagedThreadId}");
            }
            SwapBuffers();
            renderToThread[this] = ThreadHelper.EmptyThread;
            GraphicFramework.glfwMakeContextCurrent(IntPtr.Zero);
            OpenGLFunction.CurrentGL = null;
        }

        /// <inheritdoc />
        public override void RenderStart()
        {
            Thread current = Thread.CurrentThread;
            if (!renderToThread.TryGetValue(this, out Thread t) || t == ThreadHelper.EmptyThread)
            {
                renderToThread[this] = current;
                MakeAsCurrentContext();
                return;
            }
            if (t != current) {
                throw new InvalidOperationException(
                    $"Current render has had been binded to thread {t!.ManagedThreadId}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetSizeLimit(
                             int minWidth,
                             int minHeight,
                             int maxWidth,
                             int maxHeight
        )
        {
            GraphicFramework.glfwSetWindowSizeLimits(this.CanvasHandle, minWidth, minHeight,
                                                     maxWidth, maxHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal sealed override void PlatformSpecifiedInitialize()
        {
            InitializeContext();
            MakeAsCurrentContext();
            glManager.LoadOpenGLApiTo(BindedViewPortContext.GraphicHandle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal sealed override void Terminate()
        {
            CustomizeDeinit();
        }

        protected sealed override Uniform<GraphicsMatrix> NativeCameraUniform(
                                                          ShaderProgram source,
                                                          string name
        )
        {
            NativeCamera.GainAllUniforms(source, name);
            return NativeCamera.convertedUniform;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected sealed override void NativeCameraViewRange(
                                       Radius fieldOfRange,
                                       Vec2 offset,
                                       (float frontDepth, float backDepth) viewDepth
        )
        {
            NativeCamera.SetViewRange(fieldOfRange, this.Size, offset, viewDepth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected sealed override void NativeCameraViewTarget(
                                       Vec3 position,
                                       Vec3 direction,
                                       Vec3 up
        )
        {
            NativeCamera.SetViewDirection(position, direction, up);
        }

    }
}
