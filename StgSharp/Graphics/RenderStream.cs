//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="RenderStream.cs"
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
using StgSharp.Graphics.OpenGL;
using StgSharp.Logic;
using StgSharp.Math;
using StgSharp.MVVM;
using StgSharp.MVVM.View;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public abstract class RenderStream 
    {
        private protected (int x, int y, int z) _coordStretch;
        private Camera nativeCamera;
        private protected ViewPort primeArgs;
        internal TimeSpanProvider _timeProvider;
        
        public (int width, int height) Size
        {
            get => (Width, Height);
            internal set
            {
                primeArgs.Height = value.width;
                primeArgs.Width = value.height;
                InternalIO.glfwSetWindowSize(this.CanvasHandle, Width, Height);
            }
        }

        public int Height
        {
            get => primeArgs.Height;
            internal set { primeArgs.Height = value; }
        }

        public int Width
        {
            get => primeArgs.Width;
            internal set { primeArgs.Width = value; }
        }

        public IntPtr Monitor
        {
            get => primeArgs.Monitor;
            internal set => primeArgs.Monitor = value;
        }

        public abstract bool IsContextSharable
        {
            get;
        }

        public string Name
        {
            get => primeArgs.Name;
        }

        public vec2d GlobalCoordSize => ((Width * 1.0f) / _coordStretch.x, (Height * 1.0f) / _coordStretch.y);

        public ViewPort Port
        {
            get => primeArgs;
            internal set => primeArgs = value;
        }
        internal IntPtr CanvasHandle { get => primeArgs.ViewPortID; }


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

        protected int PassedFrames
        {
            get => _timeProvider.CurrentSpan;
        }

        protected float TimePassed
        {
            get => _timeProvider.CurrentSecond;
        }

        protected float FramePassed
        {
            get => _timeProvider.CurrentSpan;
        }

        protected TimeSpanProvider TimeProvider
        {
            get => _timeProvider;
        }

        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minWidth">Minimum width of current form</param>
        /// <param name="minHeight">Minimum height of current form</param>
        /// <param name="maxWidth">Maximum width of current form</param>
        /// <param name="maxHeight">Maximum height of current form</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight);

        public static TTarget ShareContextFrom<TSource, TTarget>(TSource source)
            where TSource : RenderStream
            where TTarget : RenderStream, new()
        {
            TTarget target = new TTarget();
            target.Initialize(source.Port,source._coordStretch,source._timeProvider);
            return target;
        }

        internal protected static void PollEvents()
        {
            InternalIO.glfwPollEvents();
        }

        internal protected void SwapBuffers()
        {
            InternalIO.glfwSwapBuffers(CanvasHandle);
        }

        #region native camera operation
        protected abstract void NativeCameraViewRange(Radius fieldOfRange, vec2d offset, (float frontDepth, float backDepth) viewDepth);
        protected abstract void NativeCameraViewTarget(vec3d position, vec3d direction, vec3d up);

        /// <summary>
        /// Get <see cref="Uniform{T}"/>(TView is <see cref="Matrix44"/>) uniform from a shader.
        /// This uniform will be used as projection matrix of this camera.
        /// Additionally, for multi-shader rendering, each shader requires a uniform, 
        /// so do not ignore return value of this method.
        /// </summary>
        /// <param name="source">Shader using this camera</param>
        /// <param name="name">ContextName of projection camera named in shader</param>
        /// <returns>
        /// <see cref="Uniform{T}"/>(TView is <see cref="Matrix44"/>) representing 
        /// projection matrix in shader program.
        /// </returns>
        protected abstract Uniform<Matrix44> NativeCameraUniform(ShaderProgram source, string name);
        #endregion

        #region internal operation
        internal abstract void Draw();

        public void Initialize(ViewPort v, (int, int, int) unitCubeSize, TimeSpanProvider timeProvider)
        {
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v));
            }
            primeArgs = v;
            this._timeProvider = timeProvider;
            _coordStretch = unitCubeSize;
            nativeCamera = new Camera();
            PlatformSpecifiedInitialize();
            CustomizeInit();
        }

        internal abstract void PlatformSpecifiedInitialize();

        internal abstract void Terminate();
        #endregion

        #region public operation
        protected abstract void CustomizeDeinit();
        protected abstract void CustomizeInit();
        protected abstract void Main();

        #endregion

        #region public resource generator

        protected abstract Shader CreateShaderSegment(ShaderType type, int count);
        protected abstract ShaderProgram CreateShaderProgram();

        #endregion

    }

}
