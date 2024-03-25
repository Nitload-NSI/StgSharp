//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IglFunc.cs"
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
using System.Collections.Concurrent;
using System.ComponentModel.Design;

namespace StgSharp.Graphics
{
    public interface IglFunc
    {

        internal unsafe GLcontext* Context
        {
            get;
        }

        /// <summary>
        /// Same function as OpenGL api glActiveTexture
        /// </summary>
        /// <param name="type">type of texture to activate</param>
        void ActiveTexture(uint type);

        /// <summary>
        /// Operation after alignment by calling <see cref="AlignFrame(Form)"/>.
        /// No calling of this method is acceptable, but it will bring the risk
        /// of that relevant code will not run if it is interrupted by frame overtime.
        /// Please call this method immediately after calling <see cref="AlignFrame(Form)"/>,
        /// too early or sonner may cause <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="form">The form used as frame update benchmark.</param>
        /// <param name="action">Operation after frame update alignment.</param>
        /// <exception cref="InvalidOperationException">The action binded to a wrong alignment.</exception>
        void AfterAlign(Form form, Action action);

        /// <summary>
        /// Align a sets of code to frame update of a <see cref="Form"/>.
        /// Usually, a call of <see cref="AfterAlign(Form,Action)"/> is required.
        /// This method is used as sigle thread method. If more than one
        /// alignment is requested, a <see cref="InvalidOperationException"/> will throw.
        /// </summary>
        /// <param name="form">The form used as frame update benchmark.</param>
        /// <exception cref="InvalidOperationException">More than one alingment attached to one form timer</exception>
        void AlignFrame(Form form);

        /// <summary>
        /// Same fucntion as OpenGL api glAttachShader.
        /// Only the first <see href="shader"/> in handle
        /// will be attached to the first <see href="program"/> in handle.
        /// The rest shader and program will be ignored.
        /// </summary>
        /// <param name="program">Shader program to be attachec</param>
        /// <param name="shader">A shader part to attach</param>
        void AttachShader(glHandle program, glHandle shader);

        //TODO:BindBuffer注释没写完
        /// <summary>
        /// Bind the indexed buffer object to certain type of buffer
        /// </summary>
        /// <param name="bufferType"></param>
        /// <param name="handle"></param>
        void BindBuffer(BufferType bufferType, glHandle handle);

        /// <summary>
        /// Same function as OpenGL api, glBindTexture.
        /// </summary>
        /// <param name="type">Type of texture to bind</param>
        /// <param name="texture">Sign of texture</param>
        void BindTexture(uint type, glHandle texture);

        /// <summary>
        /// Same function as OpenGL api, glBindVertexArray
        /// </summary>
        /// <param name="index"></param>
        void BindVertexArray(glHandle index);

        /// <summary>
        /// Clear buffers to preset values
        /// </summary>
        /// <param name="mask"></param>
        void Clear(MaskBufferBit mask);

        /// <summary>
        /// Call apiClearColor in OpenGL
        /// specifies the red, green, blue, and alpha values used by apiClear to clear the color buffers.
        /// Values specified by apiClearColor are clamped to the range [0,1]
        /// </summary>
        /// <param name="R">Specify the red values used when the color buffers are cleared.</param>
        /// <param name="G">Specify the red values used when the color buffers are cleared.</param>
        /// <param name="B">Specify the red values used when the color buffers are cleared.</param>
        /// <param name="A">Specify the red values used when the color buffers are cleared.</param>
        void ClearColor(float R, float G, float B, float A);

        void ClearGraphicError();

        void CompileShader(glHandle shaderhandle);

        /// <summary>
        /// Same function as OpenGL api, glCreateProgram
        /// </summary>
        /// <returns>A handle representing a single shader program</returns>
        glHandle CreateProgram();

        /// <summary>
        /// Same function as OpenGL api, glCreateProgram.
        /// Used for creating multiple shader program.
        /// </summary>
        /// <returns>A handle representing all shader programs</returns>
        glHandleSet CreateProgram(int count);

        glHandleSet CreateShaderSet(int count, ShaderType type);

        /// <summary>
        /// Creates a form and its associated
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Required width</param>
        /// <param name="tittle">Tittle of the window </param>
        /// <param name="monitor"></param>
        /// <param name="share"></param>
        /// <exception cref="Exception"></exception>
        IntPtr CreateWindowHandle(int width, int height, string tittle, IntPtr monitor, Form share);

        glHandle CreatGraphicProgram();

        /// <summary>
        /// Same function as OpenGL api, glDeleteBuffer. This method can delete one buffer in one call
        /// </summary>
        /// <param name="handle">a handle representing the buffer to be deleted</param>
        void DeleteBuffer(glHandle handle);

        /// <summary>
        /// Same function as OpenGL api, glDeleteBuffer. This method can delete a sets of buffers in one call.
        /// </summary>
        /// <param name="handleSet">Array of buffers to be deleted</param>
        unsafe void DeleteBuffers(glHandleSet handleSet);

        /// <summary>
        /// Same function as OpenGL api, glDeleteVertexArrays
        /// </summary>
        /// <param name="handleSet">Handle of vertex objects.</param>
        unsafe void DeleteVertexArrays(glHandleSet handleSet);

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <param name="ptr"></param>
        void DrawElements(GepmetryType mode, uint count, TypeCode type, IntPtr ptr);

        /// <summary>
        /// Same fucntion as the OpenGL api glGenBuffers
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        unsafe glHandleSet GenBuffers(int count);

        /// <summary>
        /// Same fucnction as OpenGL api glGenTextures.
        /// </summary>
        /// <param name="count">amount of textures to generate</param>
        unsafe glHandleSet GenTextures(int count);

        /// <summary>
        /// Same fucntion as OpenGL api, glGenVertexArray
        /// </summary>
        /// <param name="count">Amount of vao to be generated</param>
        unsafe glHandleSet GenVertexArrays(int count);

        /// <summary>
        /// Same fucntion as OpenGL api, glGetUniformLocation
        /// </summary>
        /// <param name="program">Shader program to find the uniform</param>
        /// <param name="name">Name of the uniform </param>
        /// <returns></returns>
        unsafe glSHandle GetUniformLocation(glHandle program, string name);

        bool GetWindowShouldClose(Form form);

        void LoadShaderSource(glHandle shaderHandle, string codeStream);

        /// <summary>
        /// Make the OpenGL context of a certain <see cref="Form"/> as the activated OpenGL context.
        /// </summary>
        /// <param name="form"></param>
        void MakeContexCurrent(Form form);

        void SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage) where T : struct, IMat;

        void SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage) where T : struct, IConvertible;

        /// <summary>
        /// Set data value to a float <see cref="Uniform{T,U,V,W}"/>,
        /// all types of parameters are float.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T,U,V,W}"/> target</param>
        /// <param name="vec">A <see cref="Vec4d"/> packing four parameters of the uniform </param>
        void SetValue(Uniform<float, float, float, float> uniform, Vec4d vec);

        /// <summary>
        /// Set data value to a float <see cref="Uniform{T}"/>,
        /// all types of parameters are float.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T}"/> target</param>
        /// <param name="v0">The value to be set</param>
        void SetValue(Uniform<float> uniform, float v0);

        /// <summary>
        /// Set data value to a int <see cref="Uniform{T}"/>,
        /// all types of parameters are int.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T}"/> target</param>
        /// <param name="i0">The value to be set</param>
        void SetValue(Uniform<int> uniform, int i0);

        /// <summary>
        /// Set data value to a float <see cref="Uniform{T}"/>,
        /// all types of parameters are <see cref="Vec4d"/>.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T}"/> target</param>
        /// <param name="vec">A <see cref="Vec4d"/> value of the uniform </param>
        void SetValue(Uniform<Vec4d> uniform, Vec4d vec);

        /// <summary>
        /// Set data value to a float <see cref="Uniform{T,U}"/>,
        /// all types of parameters are float.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T,U}"/> target</param>
        /// <param name="v0">The first value to be set</param>
        /// <param name="v1">The second value to be set</param>
        void SetValue(Uniform<float, float> uniform, float v0, float v1);

        /// <summary>
        /// Set data value to a float <see cref="Uniform{T,U,V}"/>,
        /// all types of parameters are float.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T,U,V}"/> target</param>
        /// <param name="v0">The first value to be set</param>
        /// <param name="v1">The second value to be set</param>
        /// <param name="v2">The third value to be set</param>
        void SetValue(Uniform<float, float, float> uniform, float v0, float v1, float v2);

        /// <summary>
        /// Set data value to a float <see cref="Uniform{T,U,V,W}"/>,
        /// all types of parameters are float.
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform{T,U,V,W}"/> target</param>
        /// <param name="v0">The first value to be set</param>
        /// <param name="v1">The second value to be set</param>
        /// <param name="v2">The third value to be set</param>
        /// <param name="v3">The forth value to be set</param>
        void SetValue(Uniform<float, float, float, float> uniform, float v0, float v1, float v2, float v3);

        /// <summary>
        /// Set how a shader parameter read data from a VBO.
        /// </summary>
        /// <param name="index">Serial number of the parameter</param>
        /// <param name="size">Amount of data each elements contains</param>
        /// <param name="t">Type of data read by shader</param>
        /// <param name="normalized"></param>
        /// <param name="stride">Pixels stride of each sets of data</param>
        /// <param name="pointer"></param>
        void SetVertexAttribute(uint index, int size, TypeCode t, bool normalized, uint stride, int pointer);

        /// <summary>
        /// Same function as OpenGL api glTexImage2d
        /// </summary>
        /// <param name="target">texture target to load image in</param>
        /// <param name="level">level of mipmap, 0 if default</param>
        /// <param name="internalformat">OpenGL's internal color channel format</param>
        /// <param name="width">width of image</param>
        /// <param name="height">height of image</param>
        /// <param name="format">color channels of this image</param>
        /// <param name="type">how this image express each pixel</param>
        /// <param name="pixels">data array of pixels</param>
        unsafe void TextureImage2d<T>(int target, int level, uint internalformat, uint width, uint height, uint format, uint type, glHandle pixels) where T : struct;

        /// <summary>
        /// Same fucntion as OpenGL api glTexParameteri
        /// </summary>
        /// <param name="target">texture target to set parameters</param>
        /// <param name="pname">name of parameter</param>
        /// <param name="param">value of parameter</param>
        void TextureParameter(int target, uint pname, int param);

        /// <summary>
        /// Same fucntion as OpenGL api glUseShader.
        /// </summary>
        /// <param name="program">Sign of program to be used</param>
        void UseProgram(glHandle program);

        /// <summary>
        /// Same function as the OpenGL api, glViewPort
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Viewport(int x, int y, uint width, uint height);

        internal void frameworkHandleEnqueue(IntPtr handle, params M128[] param);


    }
}