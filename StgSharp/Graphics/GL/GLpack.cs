//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GLpack.cs"
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
using StgSharp.Graphics;
using StgSharp.Logic;
using StgSharp.Math;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp
{
    internal static unsafe partial class InternalIO
    {
        [DllImport(SSC_libName, EntryPoint = "glCheckShaderStat",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern int glCheckShaderStatus(ref GLcontext context, uint shaderHandle,int key, ref IntPtr logPtr);
    }
}
namespace StgSharp.Graphics
{
    internal static unsafe partial class GL
    {

        private static GLcontext* context;

        public static IntPtr CurrentContextHandle
        {
            get { return (IntPtr)context; }
            set 
            {
                context = (GLcontext*)value;
            }
        }

        private static ref GLcontext Context
        {
            get 
            {
#if DEBUG
                if ((IntPtr)context==IntPtr.Zero)
                {
                    throw new NullReferenceException();
                }
#endif
                return ref *context;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ActiveTexture(uint type)
        {
            Context.glActiveTexture(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AttachShader(glHandle program, glHandle shader)
        {
            Context.glAttachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindBuffer(BufferType bufferType, glHandle handle)
        {
            Context.glBindBuffer((uint)bufferType, handle.Value);
        }

        public static string GetShaderStatus(Shader s, int index, int key)
        {
            IntPtr sptr = IntPtr.Zero;
            if (InternalIO.glCheckShaderStatus(ref Context, s.handle[index].Value, key,ref sptr) == 0)
            {
                return Marshal.PtrToStringAnsi(sptr);
            }
            return "";
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
            Context.glBindFramebuffer((uint)target, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindTexture(uint type, glHandle texture)
        {
            Context.glBindTexture(type, texture.Value);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindVertexArray(glHandle handle)
        {
            Context.glBindVertexArray(handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VertexAttributeDivisor(uint layoutIndex, uint divisor)
        {
            Context.glVertexAttribDivisor(layoutIndex, divisor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(MaskBufferBit mask)
        {
            Context.glClear((uint)mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ClearColor(float R, float G, float B, float A)
        {
            Context.glClearColor(R, G, B, A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ClearGraphicError()
        {
            while (Context.glGetError() != 0) { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CompileShader(glHandle shaderhandle)
        {
            Context.glCompileShader(shaderhandle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandle CreateGraphicProgram()
        {
            return glHandle.Alloc(Context.glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static glHandle CreateProgram()
        {
            return glHandle.Alloc(Context.glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet CreateProgram(int count)
        {
            glHandleSet h = new glHandleSet(count);
            for (int i = 0; i < count; i++)
            {
                h.SetValue(i, Context.glCreateProgram());
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
                uint id = Context.glCreateShader(t);
                if (id == 0)
                {
                    uint error = Context.glGetError();
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
        public static unsafe void DeleteBuffer(glHandle handle)
        {
            Context.glDeleteBuffers(1, (uint*)handle.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DeleteBuffers(glHandleSet handle)
        {
            Context.glDeleteBuffers(handle.length, (uint*)handle.Handle);
            handle.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DeleteVertexArrays(glHandleSet arrays)
        {
            Context.glDeleteVertexArrays(arrays.length, (uint*)arrays.Handle);
            arrays.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawElements(GeometryType mode, uint count, TypeCode type, IntPtr ptr)
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
            Context.glDrawElements((uint)mode, count, a, (void*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawElementsInstanced(
            GeometryType mode , uint count, TypeCode type, IntPtr ptr, uint amount
            )
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
            Context.glDrawElementsInstanced((uint)mode, count, a, (void*)ptr,amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(GLOperation operation)
        {
            Context.glEnable((uint)operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet GenBuffers(int count)
        {
            glHandleSet h = new glHandleSet(count);
            Context.glGenBuffers(count, (uint*)h.Handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet GenTextures(int count)
        {
            glHandleSet h = new glHandleSet(count);
            Context.glGenTextures(count, (uint*)h.Handle);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glHandleSet GenVertexArrays(int count)
        {
            glHandleSet h = new glHandleSet(count);
            Context.glGenVertexArrays(count, (uint*)h.Handle);
            return h;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe glSHandle GetUniformLocation(glHandle program, string name)
        {
            byte[] code = Encoding.UTF8.GetBytes(name);
            fixed (byte* ptr = code)
            {
                int ret = Context.glGetUniformLocation(program.Value, ptr);
#if DEBUG
                if (ret == (int)(GLconst.INVALID_OPERATION) ||
                    ret == (int)(GLconst.INVALID_VALUE))
                {
                    throw new InvalidOperationException();
                }
                if (ret == -1 )
                {
                    throw new UniformEmptyReferenceException(nameof(program), name);
                }
#endif
                return glSHandle.Alloc(ret);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void LoadShaderSource(glHandle shaderHandle, string codeStream)
        {
            byte[] stream = Encoding.UTF8.GetBytes(codeStream);
            fixed (byte* streamPtr = stream)
            {
                Context.glShaderSource(shaderHandle.Value, 1, &streamPtr, (void*)0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MakeContextCurrent(Form form)
        {
            if (form.WindowID == IntPtr.Zero)
            {
                InternalIO.InternalWriteLog($"From {nameof(form)} is not inited.", LogType.Warning);
                return;
            }
            InternalIO.glfwMakeContextCurrent(form.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage)
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
#pragma warning disable CS8500
            T* p = &bufferData;
#pragma warning restore CS8500
            Context.glBufferData(
                (uint)bufferType,
                (IntPtr)size,
                (IntPtr)p,
                (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
            where T : struct,
#if NET8_0_OR_GREATER
            INumber
#else
            IConvertible
#endif
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null)
            {
                return;
            }
#pragma warning disable CS8500
            fixed (void* bufferPtr = bufferArray)
#pragma warning restore CS8500
            {
                Context.glBufferData(
                                (uint)bufferType,
                                (IntPtr)size,
                                (IntPtr)bufferPtr,
                                (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferData<T>(BufferType bufferType, Span<T> bufferArray, BufferUsage usage)
            where T : struct,
#if NET8_0_OR_GREATER
            INumber
#else
            IConvertible
#endif
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null)
            {
                return;
            }

#pragma warning disable CS8500
            fixed (void* bufferPtr = &bufferArray.GetPinnableReference())
#pragma warning restore CS8500
            {
                Context.glBufferData(
                                (uint)bufferType,
                                (IntPtr)size,
                                (IntPtr)bufferPtr,
                                (uint)usage);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferVectorData<T>(BufferType bufferType, Span<T> bufferArray, BufferUsage usage)
            where T : struct, IVector
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null)
            {
                return;
            }
#pragma warning disable CS8500
            fixed (void* bufferPtr = &bufferArray.GetPinnableReference())
#pragma warning restore CS8500
            {
                Context.glBufferData(
                                (uint)bufferType,
                                (IntPtr)size,
                                (IntPtr)bufferPtr,
                                (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetBufferVectorData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
            where T : struct, IVector
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null)
            {
                return;
            }
#pragma warning disable CS8500
            fixed (void* bufferPtr = bufferArray)
#pragma warning restore CS8500
            {
                Context.glBufferData(
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
            Context.glVertexAttribPointer(index, size, type, normalized ? 1 : 0, stride, (void*)pointer);
            Context.glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextureImage2d<T>(int target, int level, uint internalformat, uint width, uint height, uint format, uint type, glHandle pixels)
        {
            Context.glTexImage2D(
                (uint)target, level, internalformat, width, height, 0, format, type, pixels.valuePtr
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextureParameter(int target, uint pname, int param)
        {
            Context.glTexParameteri((uint)target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UseProgram(glHandle program)
        {
            Context.glUseProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Viewport(int x, int y, uint width, uint height)
        {
            Context.glViewport(x, y, width, height);
        }

    }
}

