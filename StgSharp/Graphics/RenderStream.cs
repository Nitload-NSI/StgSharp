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
using StgSharp.Logic;
using StgSharp.Math;

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
    public abstract class RenderStream: IConvertableToBlueprintNode
    {

        private IntPtr canvasHandle;
        private IntPtr contextHandle;

        private int height;
        private IntPtr monitor;
        private IntPtr sharedCanvas;
        private int width;
        protected (int x, int y, int z) _coordStretch;
        protected string name;
        private Camera nativeCamera;

        public RenderStream(RenderStreamConstructArgs args)
        {
            height = args.Height;
            monitor = args.Monitor;
            width = args.Width;
            name = args.Name;
            monitor = args.Monitor;
            _coordStretch = args.UnitCubeSize;
        }

        public IntPtr CanvasHandle
        {
            get => canvasHandle;
            internal set => canvasHandle = value;
        }

        public IntPtr ContextHandle
        {
            get => contextHandle;
            internal set => contextHandle = value;
        }

        internal protected Camera NativeCamera
        {
            get => nativeCamera;
            internal set => nativeCamera = value; 
        }

        public FrameBufferSizeHandler FrameSizeCallback
        {
            set
            {
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(value);
                InternalIO.glfwSetFramebufferSizeCallback(CanvasHandle, handlerPtr);
            }
        }

        public vec2d GlobalCoordSize => ((Width * 1.0f) / _coordStretch.x, (Height * 1.0f) / _coordStretch.y);
          
        public int Height
        {
            get => height;
            internal set
            {
                height = value;
                InternalIO.glfwSetWindowSize(this.canvasHandle, Width, Height);
            }
        }

        public IntPtr Monitor
        {
            get => monitor;
            internal set => monitor = value;
        }

        public string Name
        {
            get => name;
            internal set => name = value;
        }

        public IntPtr SharedCanvas
        {
            get => sharedCanvas;
            internal set => sharedCanvas = value;
        }

        public vec2d Size
        {
            get => new vec2d(width, height);
            internal set
            {
                height = (int)value.Y;
                width = (int)value.X;
                InternalIO.glfwSetWindowSize(this.canvasHandle, Width, Height);
            }
        }

        public int Width
        {
            get => width;
            internal set
            {
                width = value;
                InternalIO.glfwSetWindowSize(this.canvasHandle, Width, Height);
            }
        }

        protected abstract string[] InputPortsName { get; }
        protected abstract string[] OutputPortsName { get; }

        string[] IConvertableToBlueprintNode.InputInterfacesName => InputPortsName;

        string[] IConvertableToBlueprintNode.OutputInterfacesName => OutputPortsName;

        BlueprintNodeAction IConvertableToBlueprintNode.MainExecution => InternalDraw;

        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minWidth">Minimum width of current form</param>
        /// <param name="minHeight">Minimum height of current form</param>
        /// <param name="maxWidth">Maximum width of current form</param>
        /// <param name="maxHeight">Maximum height of current form</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight);

        internal abstract void CreateCanvas();

        #region native camera operation
        protected abstract void NativeCameraViewRange(Radius FieldOfRange,vec2d offset, (float frontDepth, float backDepth ) viewDepth );
        protected abstract void NativeCameraViewTarget(vec3d position, vec3d direction, vec3d up);
        /// <summary>
        /// Get <see cref="Uniform{T}"/>(T is <see cref="Matrix44"/>) uniform from a shader.
        /// This uniform will be used as projection matrix of this camera.
        /// Additionally, for multi-shader rendering, each shader requires a uniform, 
        /// so do not ignore return value of this method.
        /// </summary>
        /// <param name="source">Shader using this camera</param>
        /// <param name="name">Name of projection camera named in shader</param>
        /// <returns>
        /// <see cref="Uniform{T}"/>(T is <see cref="Matrix44"/>) representing 
        /// projection matrix in shader program.
        /// </returns>
        protected abstract Uniform<Matrix44> NativeCameraUniform(ShaderProgram source, string name);
        #endregion

        #region internal operation
        internal abstract void InternalDraw(
            in Dictionary<string, BlueprintPipeline> input,
            in Dictionary<string, BlueprintPipeline> output);
        internal abstract void InternalInit();
        internal abstract void InternalTerminate();
        #endregion

        #region public operation
        protected abstract void Deinit();
        protected abstract void Init();
        protected abstract void Main(
            in Dictionary<string, BlueprintPipeline> input,
            in Dictionary<string, BlueprintPipeline> output);
        protected abstract void ProcessInput();
        
        #endregion

        #region public resource generator

        protected abstract Shader CreateShaderSegment(ShaderType type, int count);
        protected abstract ShaderProgram CreateShaderProgram();

        

        #endregion

    }

    public class RenderStreamConstructArgs
    {

        public int Height { get; set; }
        public IntPtr Monitor { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public (int x, int y, int z) UnitCubeSize { get; set; }
    }
}
