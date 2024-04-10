//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GLonRender.cs"
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    internal unsafe static partial class GL
    {
        internal static GLcontext* api;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ActiveTexture(uint type)
        {
            api->glActiveTexture(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AttachShader(glHandle program, glHandle shader)
        {
            api->glAttachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindBuffer(BufferType bufferType, glHandle handle)
        {
            api->glBindBuffer((uint)bufferType, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindTexture(uint type, glHandle texture)
        {
            api->glBindTexture(type, texture.Value);
        }

        /// <summary>
        /// Bind a frame buffer to a frame buffer target
        /// Same function as <see href="https://docs.gl/gl3/glBindFramebuffer">glBindFramebuffer</see>.
        /// </summary>
        /// <param name="target"> The frame buffer target of the binding operation.</param>
        /// <param name="handle"> The handle of the frame buffer object to bind.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindFrameBuffer(FrameBufferTarget target, glHandle handle)
        {
            api->glBindFramebuffer((uint)target,handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindVertexArray(glHandle handle)
        {
            api->glBindVertexArray(handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(MaskBufferBit mask)
        {
            api->glClear((uint)mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void ClearColor(float R, float G, float B, float A)
        {
            api->glClearColor(R, G, B, A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void ClearGraphicError()
        {
            while (api->glGetError() != 0) { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CompileShader(glHandle shaderhandle)
        {
            api->glCompileShader(shaderhandle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static glHandle CreateProgram()
        {
            return glHandle.Alloc(api->glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet CreateProgram(int count)
        {
            glHandleSet h = new glHandleSet(count);
            for (int i = 0; i < count; i++)
            {
                h.SetValue(i, api->glCreateProgram());
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static glHandleSet CreateShaderSet(int count, ShaderType type)
        {
            glHandleSet ret = new glHandleSet(count);
            uint t = (uint)type;
            for (int i = 0; i < count; i++)
            {
                uint id = api->glCreateShader(t);
                if (id == 0)
                {
                    uint error = api->glGetError();
                    InternalIO.InternalWriteLog($"Failed in creating shader: {error}", LogType.Error);
                    throw new Exception();
                }
                ret.SetValue(i, id);
            }
            return ret;
        }

        public static unsafe IntPtr CreateWindowHandle(
            int width, int height, string tittle, IntPtr monitor, Form share)
        {
            byte[] tittleBytes = Encoding.UTF8.GetBytes(tittle);
            IntPtr windowPtr;
            if (share == null)
            {
                windowPtr = InternalIO.glfwCreateWindow(
                    width, height, tittleBytes, monitor, IntPtr.Zero);
            }
            else
            {
                windowPtr = InternalIO.glfwCreateWindow(
                    width, height, tittleBytes, monitor, share.id);
            }

            if (((long)windowPtr == -1) || ((long)windowPtr == -2))
            {
                throw new Exception("Cannot create window instance!");
            }
            else
            {
                return windowPtr;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandle CreateGraphicProgram()
        {
            return glHandle.Alloc(api->glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void DeleteBuffer(glHandle handle)
        {
            api->glDeleteBuffers(1, (uint*)handle.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void DeleteBuffers(glHandleSet handle)
        {
            api->glDeleteBuffers(handle.length, (uint*)handle.Handle);
            handle.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void DeleteVertexArrays(glHandleSet arrays)
        {
            api->glDeleteVertexArrays(arrays.length, (uint*)arrays.Handle);
            arrays.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void DrawElements(GepmetryType mode, uint count, TypeCode type, IntPtr ptr)
        {
            uint a;
            switch (type)
            {
                case TypeCode.UInt32:
                    a = GLconst.UNSIGNED_INT;
                    break;

                case TypeCode.UInt16:
                    a = GLconst.UNSIGNED_SHORT;
                    break;

                case TypeCode.Byte:
                    a = GLconst.UNSIGNED_BYTE;
                    break;

                default:
                    InternalIO.InternalWriteLog(
                        $"{$"Parameter error in parameter {nameof(type)} method DrawElements. "}{$"Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are supported."}",
                        LogType.Error);
                    return;
            }
            api->glDrawElements((uint)mode, count, a, (void*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet GenBuffers(int count)
        {
            glHandleSet h = new glHandleSet(count);
            api->glGenBuffers(count, (uint*)h.Handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet GenTextures(int count)
        {
            glHandleSet h = new glHandleSet(count);
            api->glGenTextures(count, (uint*)h.Handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet GenVertexArrays(int count)
        {
            glHandleSet h = new glHandleSet(count);
            api->glGenVertexArrays(count, (uint*)h.Handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glSHandle GetUniformLocation(glHandle program, string name)
        {
            byte[] code = Encoding.UTF8.GetBytes(name);
            fixed (byte* ptr = code)
            {
                return glSHandle.Alloc(api->glGetUniformLocation(program.Value, ptr));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe public static void LoadShaderSource(glHandle shaderHandle, string codeStream)
        {
            byte[] stream = Encoding.UTF8.GetBytes(codeStream);
            fixed (byte* streamPtr = stream)
            {
                api->glShaderSource(shaderHandle.Value, 1, &streamPtr, (void*)0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MakeContextCurrent(Form form)
        {
            if (form.WindowID == IntPtr.Zero)
            {
                InternalIO.InternalWriteLog($"From {nameof(form)} is not inited.", LogType.Warning);
                return;
            }
            InternalIO.glfwMakeContextCurrent(form.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage)
        {
            int size = Marshal.SizeOf<T>();
            T* p = &bufferData;
            api->glBufferData(
                (uint)bufferType,
                (IntPtr)size,
                (IntPtr)p,
                (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null)
            {
                return;
            }
            fixed (void* bufferPtr = bufferArray)
            {
                api->glBufferData(
                                (uint)bufferType,
                                (IntPtr)size,
                                (IntPtr)bufferPtr,
                                (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetVertexAttribute(uint index, int size, TypeCode t, bool normalized, uint stride, int pointer)
        {
            uint type = InternalIO.GLtype[t];
            api->glVertexAttribPointer(index, size, type, normalized ? 1 : 0, stride, (void*)pointer);
            api->glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextureImage2d<T>(int target, int level, uint internalformat, uint width, uint height, uint format, uint type, glHandle pixels)
        {
            api->glTexImage2D(
                (uint)target, level, internalformat, width, height, 0, format, type, pixels.valuePtr
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextureParameter(int target, uint pname, int param)
        {
            api->glTexParameteri((uint)target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UseProgram(glHandle program)
        {
            api->glUseProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Viewport(int x, int y, uint width, uint height)
        {
            api->glViewport(x, y, width, height);
        }
    }
}

