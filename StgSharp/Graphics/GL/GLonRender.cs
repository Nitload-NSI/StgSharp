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
    internal unsafe partial class GLonRender : IglFunc
    {

        private FrameTimeoutException refreshSIgnal = new FrameTimeoutException();
        internal GLcontext* api;

        internal GLonRender(GLcontext* context)
        {
            api = context;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.ActiveTexture(uint type)
        {
            api->glActiveTexture(type);
        }

        void IglFunc.AfterAlign(Form form, Action action)
        {
            throw new NotImplementedException();
        }

        void IglFunc.AlignFrame(Form form)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.AttachShader(glHandle program, glHandle shader)
        {
            api->glAttachShader(*program.valuePtr, shader.DirectValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.BindBuffer(BufferType bufferType, glHandle handle)
        {
            api->glBindBuffer((uint)bufferType, handle.DirectValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.BindTexture(uint type, glHandle texture)
        {
            api->glBindTexture(type, texture.DirectValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.BindVertexArray(glHandle handle)
        {
            api->glBindVertexArray(handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.Clear(MaskBufferBit mask)
        {
            api->glClear((uint)mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.ClearColor(float R, float G, float B, float A)
        {
            api->glClearColor(R, G, B, A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.ClearGraphicError()
        {
            while (api->glGetError() != 0) { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.CompileShader(glHandle shaderhandle)
        {
            api->glCompileShader(shaderhandle.DirectValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        glHandle IglFunc.CreateProgram()
        {
            return glHandle.Alloc(api->glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe glHandleSet IglFunc.CreateProgram(int count)
        {
            glHandleSet h = new glHandleSet(count);
            for (int i = 0; i < count; i++)
            {
                h.SetValue(i, api->glCreateProgram());
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        glHandleSet IglFunc.CreateShaderSet(int count, ShaderType type)
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

        unsafe IntPtr IglFunc.CreateWindowHandle(
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
        unsafe glHandle IglFunc.CreatGraphicProgram()
        {
            return glHandle.Alloc(api->glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.DeleteBuffer(glHandle handle)
        {
            api->glDeleteBuffers(1, (uint*)handle.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.DeleteBuffers(glHandleSet handle)
        {
            api->glDeleteBuffers((uint)handle.length, handle.handle);
            handle.Free();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.DeleteVertexArrays(glHandleSet arrays)
        {
            api->glDeleteVertexArrays((uint)arrays.length, arrays.handle);
            arrays.Free();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.DrawElements(GepmetryType mode, uint count, TypeCode type, IntPtr ptr)
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
                        $"{$"Parameter error in parameter {nameof(type)} method DrawElements. "}{$"Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are suporrted."}",
                        LogType.Error);
                    return;
            }
            api->glDrawElements((uint)mode, count, a, (void*)ptr);
        }

        void IglFunc.frameworkHandleEnqueue(System.IntPtr handle, params M128[] param)
        {
            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe glHandleSet IglFunc.GenBuffers(int count)
        {
            glHandleSet h = new glHandleSet(count);
            api->glGenBuffers((uint)count, h.handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe glHandleSet IglFunc.GenTextures(int count)
        {
            glHandleSet h = new glHandleSet(count);
            api->glGenTextures((uint)count, h.handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe glHandleSet IglFunc.GenVertexArrays(int count)
        {
            glHandleSet h = new glHandleSet(count);
            api->glGenVertexArrays((uint)count, h.handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe glSHandle IglFunc.GetUniformLocation(glHandle program, string name)
        {
            byte[] code = Encoding.UTF8.GetBytes(name);
            fixed (byte* ptr = code)
            {
                return glSHandle.Alloc(api->glGetUniformLocation(program.DirectValue, ptr));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe bool IglFunc.GetWindowShouldClose(Form form)
        {
            return InternalIO.glfwWindowShouldClose(form.id) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.LoadShaderSource(glHandle shaderHandle, string codeStream)
        {
            byte[] stream = Encoding.UTF8.GetBytes(codeStream);
            fixed (byte* streamPtr = stream)
            {
                api->glShaderSource(shaderHandle.DirectValue, 1, &streamPtr, (void*)0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.MakeContexCurrent(Form form)
        {
            if (form.WindowID == IntPtr.Zero)
            {
                InternalIO.InternalWriteLog($"From {nameof(form)} is not initted.", LogType.Warning);
                return;
            }
            InternalIO.glfwMakeContextCurrent(form.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage)
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
        unsafe void IglFunc.SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
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
        void IglFunc.SetVertexAttribute(uint index, int size, TypeCode t, bool normalized, uint stride, int pointer)
        {
            uint type = InternalIO.GLtype[t];
            api->glVertexAttribPointer(index, size, type, normalized ? 1 : 0, stride, (void*)pointer);
            api->glEnableVertexAttribArray(index);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.TextureImage2d<T>(int target, int level, uint internalformat, uint width, uint height, uint format, uint type, glHandle pixels)
        {
            api->glTexImage2D(
                (uint)target, level, internalformat, width, height, 0, format, type, pixels.valuePtr
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.TextureParameter(int target, uint pname, int param)
        {
            api->glTexParameteri((uint)target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.UseProgram(glHandle program)
        {
            api->glUseProgram(program.DirectValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.Viewport(int x, int y, uint width, uint height)
        {
            api->glViewport(x, y, width, height);
        }

        GLcontext* IglFunc.Context
=> api;

    }
}