//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GLonUpdate.cs"
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    internal unsafe partial class GLonUpdate : IglFunc
    {

        private GLcontext* api;
        private FrameOperationBuffer frameOpBuffer;

        internal GLonUpdate(GLcontext* context)
        {
            api = context;
        }

        public FrameOperationBuffer ActivatedOperationBuffer
        {
            get => frameOpBuffer;
            set => frameOpBuffer = value;
        }

        public void GLswapBuffers(Form form)
        {
            if (form.WindowID == IntPtr.Zero)
            {
                return;
            }
            InternalIO.glfwSwapBuffers(form.id);
        }

        void IglFunc.ActiveTexture(uint type)
        {
            frameOpBuffer.ParamEnqueue(type);
            frameOpBuffer.MethodEnqueue(ActiveTexturePtr);
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
        unsafe void IglFunc.AttachShader(glHandle program, glHandle shader)
        {
            frameOpBuffer.ParamEnqueue(program, shader);
            frameOpBuffer.MethodEnqueue(AttachShaderPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.BindBuffer(BufferType bufferType, glHandle handle)
        {
            M128 m = default;
            m.Write<uint>(0, (uint)bufferType);
            m.Write<ulong>(1, (ulong)handle.valuePtr);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(BindBufferPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.BindTexture(uint type, glHandle texture)
        {
            M128 m = default;
            m.Write<uint>(0, type);
            m.Write<ulong>(1, (ulong)texture.valuePtr);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(BindTexturePtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.BindVertexArray(glHandle index)
        {
            frameOpBuffer.ParamEnqueue(index);
            frameOpBuffer.MethodEnqueue(BindVertexArrayPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.Clear(MaskBufferBit mask)
        {
            frameOpBuffer.ParamEnqueue((int)mask);
            frameOpBuffer.MethodEnqueue(ClearPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.ClearColor(float R, float G, float B, float A)
        {
            frameOpBuffer.ParamEnqueue(R, G, B, A);
            frameOpBuffer.MethodEnqueue(ClearColorPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.ClearGraphicError()
        {
            frameOpBuffer.MethodEnqueue(ClearGraphicErrorPtr);
        }

        unsafe void IglFunc.CompileShader(glHandle shaderhandle)
        {
            frameOpBuffer.ParamEnqueue((IntPtr)shaderhandle.valuePtr);
            frameOpBuffer.MethodEnqueue(CompileShaderPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        glHandle IglFunc.CreateProgram()
        {
            glHandle h = glHandle.Alloc(1);
            frameOpBuffer.ParamEnqueue(h);
            frameOpBuffer.MethodEnqueue(CreateProgramPtr);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        glHandleSet IglFunc.CreateProgram(int count)
        {
            M128 m = default;
            glHandleSet h = new glHandleSet(count);
            m.Write<ulong>(0, (ulong)h.handle);
            m.Write<int>(2, count);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(CreateProgramSetPtr);
            return h;
        }

        glHandleSet IglFunc.CreateShaderSet(int count, ShaderType type)
        {
            glHandleSet ret = new glHandleSet(count);
            M128 m = new M128();
            m.Write<int>(0, count);
            m.Write<uint>(1, (uint)type);
            m.Write<ulong>(1, (ulong)ret.handle);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(CreateShaderPtr);
            return ret;
        }

        IntPtr IglFunc.CreateWindowHandle(int width, int height, string tittle, IntPtr monitor, Form share)
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
        glHandle IglFunc.CreatGraphicProgram()
        {
            glHandle h = glHandle.Alloc(1);
            frameOpBuffer.ParamEnqueue(h);
            frameOpBuffer.MethodEnqueue(CreateProgramPtr);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.DeleteBuffer(glHandle handle)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)handle.valuePtr);
            m.Write(2, 1);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(DeleBufferPtr);
        }

        unsafe void IglFunc.DeleteBuffers(glHandleSet handle)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)handle.handle);
            m.Write<uint>(2, (uint)handle.length);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(DeleBufferPtr);
        }

        unsafe void IglFunc.DeleteVertexArrays(glHandleSet arrays)
        {
            M128 m = default; m.Write<ulong>(1, (ulong)arrays.handle);
            m.Write<int>(0, arrays.length);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(DeleteVertexArrayPtr);
        }

        void IglFunc.DrawElements(GepmetryType mode, uint count, TypeCode type, IntPtr ptr)
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
                        $"{$"Parameter error in parameter {type} method DrawElements. "}{$"Only {TypeCode.Byte}, {TypeCode.UInt16}, {TypeCode.UInt32} types are suporrted."}",
                        LogType.Error);
                    return;
            }
            frameOpBuffer.ParamEnqueue((uint)mode, count, a);
            frameOpBuffer.ParamEnqueue(ptr);
            frameOpBuffer.MethodEnqueue(DrawElementsPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.frameworkHandleEnqueue(IntPtr methodHandle, params M128[] param)
        {
            foreach (M128 item in param)
            {
                frameOpBuffer.ParamEnqueue(item);
            }
            frameOpBuffer.ParamEnqueue(methodHandle);
        }

        unsafe glHandleSet IglFunc.GenBuffers(int count)
        {
            glHandleSet h = new glHandleSet(count);
            M128 m = default; m.Write<uint>(2, (uint)count); m.Write<ulong>(0, (ulong)h.handle);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(GenBufferPtr);
            return h;
        }

        unsafe glHandleSet IglFunc.GenTextures(int count)
        {
            glHandleSet h = new glHandleSet(count);
            M128 m = default; m.Write<uint>(2, (uint)count); m.Write<ulong>(0, (ulong)h.handle);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(GenTexturesPtr);
            return h;
        }

        unsafe glHandleSet IglFunc.GenVertexArrays(int count)
        {
            glHandleSet h = new glHandleSet(count);
            M128 m = default; m.Write<uint>(2, (uint)count); m.Write<ulong>(0, (ulong)h.handle);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(GenTexturesPtr);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe glSHandle IglFunc.GetUniformLocation(glHandle program, string ptr)
        {
            glSHandle h = glSHandle.Alloc(0);
            M128 m = default;
            m.Write<ulong>(0, (ulong)program.Handle);
            m.Write<ulong>(1, (ulong)GCHandle.Alloc(Encoding.UTF8.GetBytes(ptr), GCHandleType.Pinned).AddrOfPinnedObject());
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.ParamEnqueue((IntPtr)h.valuePtr);
            frameOpBuffer.MethodEnqueue(GetUniformLocationPtr);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IglFunc.GetWindowShouldClose(Form form)
        {
            return InternalIO.glfwWindowShouldClose(form.id) == 0;
        }

        void IglFunc.LoadShaderSource(glHandle shaderHandle, string codeStream)
        {
            frameOpBuffer.ParamEnqueue(shaderHandle);
            frameOpBuffer.ParamEnqueue(codeStream);
            frameOpBuffer.MethodEnqueue(ShaderSourcePtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IglFunc.MakeContexCurrent(Form form)
        {
            if (form.WindowID == IntPtr.Zero)
            {
                return;
            }
            InternalIO.glfwMakeContextCurrent(form.id);
        }

        void IglFunc.SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage)
        {
            frameOpBuffer.ParamEnqueue((uint)bufferType, (uint)usage);
            frameOpBuffer.ParamEnqueue((IntPtr)sizeof(T));
            frameOpBuffer.ParamEnqueue<T>(new T[] { bufferData });
            frameOpBuffer.MethodEnqueue(BufferDataPtr);
        }

        void IglFunc.SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
        {
            frameOpBuffer.ParamEnqueue((uint)bufferType, (uint)usage);
            frameOpBuffer.ParamEnqueue((IntPtr)(Marshal.SizeOf<T>() * bufferArray.Length));
            frameOpBuffer.ParamEnqueue<T>(bufferArray);
            frameOpBuffer.MethodEnqueue(BufferDataPtr);
        }

        void IglFunc.SetVertexAttribute(uint index, int size, TypeCode t, bool normalized, uint stride, int pointer)
        {
            uint type = InternalIO.GLtype[t];
            frameOpBuffer.ParamEnqueue(index, type, stride);
            frameOpBuffer.ParamEnqueue(size, normalized ? 1 : 0, pointer);
            frameOpBuffer.MethodEnqueue(SetVertexArrayPtr);
        }


        unsafe void IglFunc.TextureImage2d<T>(int target, int level, uint internalformat, uint width, uint height, uint format, uint type, glHandle pixels)
        {
            frameOpBuffer.ParamEnqueue(pixels);
            frameOpBuffer.ParamEnqueue((uint)level, internalformat, width, height);
            frameOpBuffer.ParamEnqueue((uint)target, format, type);
            frameOpBuffer.MethodEnqueue(textureImage2dPtr);
        }

        void IglFunc.TextureParameter(int target, uint pname, int param)
        {
            M128 m = default;
            m.Write<int>(0, target);
            m.Write<uint>(2, pname);
            m.Write<int>(3, param);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(TextureParameterPtr);
        }

        void IglFunc.UseProgram(glHandle program)
        {
            frameOpBuffer.ParamEnqueue((IntPtr)program.valuePtr);
            frameOpBuffer.MethodEnqueue(UseProgramPtr);
        }

        void IglFunc.Viewport(int x, int y, uint width, uint height)
        {
            Vec4d v = new Vec4d(x, y, (int)width, (int)height);
            frameOpBuffer.ParamEnqueue(v);
            frameOpBuffer.MethodEnqueue(ViewPortPtr);
        }

        GLcontext* IglFunc.Context => api;

    }
}