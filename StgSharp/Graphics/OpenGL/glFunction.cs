﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glFunction.cs"
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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp
{
    internal static unsafe partial class InternalIO
    {

        [DllImport(SSC_libName, EntryPoint = "glCheckShaderStat",
            CallingConvention = CallingConvention.Cdecl)]
#pragma warning disable CA5392
        internal static extern int glCheckShaderStatus(ref OpenglContext context, uint shaderHandle, int key, ref IntPtr logPtr);
#pragma warning restore CA5392


    }
}

namespace StgSharp.Graphics.OpenGL
{
    

    public unsafe partial class GlFunction
    {

        private static Dictionary<IntPtr, GlFunction> contextToGLMap = new Dictionary<IntPtr, GlFunction>();
        private OpenglContext* _context;

        private GlFunction()
        {
        }

        internal static GlFunction CurrentGL { get; set; }

        internal IntPtr ContextHandle => (IntPtr)_context;

        protected ref OpenglContext Context
        {
            get
            {
#if DEBUG
                if ((IntPtr)_context == IntPtr.Zero)
                {
                    throw new NullReferenceException();
                }
#endif
                return ref (*_context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ActiveTexture(uint type)
        {
            Context.glActiveTexture(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachShader(GlHandle program, GlHandle shader)
        {
            Context.glAttachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBuffer(BufferType bufferType, GlHandle handle)
        {
            Context.glBindBuffer((uint)bufferType, handle.Value);
        }

        /// <summary>
        /// Bind a frame buffer to a frame buffer target
        /// Same function as <see href="https://docs.GL/gl3/glBindFramebuffer">glBindFramebuffer</see>.
        /// </summary>
        /// <param name="target"> The frame buffer target of the binding operation.</param>
        /// <param name="handle"> The handle of the frame buffer object to bind.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFrameBuffer(FrameBufferTarget target, GlHandle handle)
        {
            Context.glBindFramebuffer((uint)target, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindRenderBuffer(GlHandle handle)
        {
            Context.glBindRenderbuffer(GLconst.RENDERBUFFER, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTexture(uint type, GlHandle texture)
        {
            Context.glBindTexture(type, texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindVertexArray(GlHandle handle)
        {
            Context.glBindVertexArray(handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FrameBufferStatus CheckFrameBufferStatus(FrameBufferTarget target)
        {
            return (FrameBufferStatus)Context.glCheckFramebufferStatus((uint)target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(MaskBufferBit mask)
        {
            Context.glClear((uint)mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ClearColor(float R, float G, float B, float A)
        {
            Context.glClearColor(R, G, B, A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearError()
        {
            while (Context.glGetError() != 0) { }
        }

        [Conditional("DEBUG")]
        public void Assert(bool shouldReportWhenNormal)
        {
            try
            {
                uint code = Context.glGetError();
                if (code != GLconst.NO_ERROR)
                {
                    throw new GlExecutionException(code);
                }
                if (shouldReportWhenNormal)
                {
                    Console.WriteLine("OpenGL runs normally");
                }
            }
            catch (GlExecutionException ex)
            {
                throw ex;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenerateMipmap(Texture2DTarget target)
        {
            Context.glGenerateMipmap((uint)target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ClearGraphicError()
        {
            while (Context.glGetError() != 0) { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CombineFrameBufferRenderBuffer(FrameBufferAttachment attachment, GlHandle rboHandle)
        {
            Context.glFramebufferRenderbuffer(GLconst.FRAMEBUFFER, (uint)attachment, GLconst.RENDERBUFFER, rboHandle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompileShader(GlHandle shaderhandle)
        {
            Context.glCompileShader(shaderhandle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle CreateGraphicProgram()
        {
            return GlHandle.Alloc(Context.glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle CreateProgram()
        {
            return GlHandle.Alloc(Context.glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] CreateProgram(int count)
        {
            GlHandle[] h = new GlHandle[count];
            for (int i = 0; i < count; i++)
            {
                h.SetValue(i, Context.glCreateProgram());
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle[] CreateShaderSet(int count, ShaderType type)
        {
            GlHandle[] ret = new GlHandle[count];
            uint t = (uint)type;
            for (int i = 0; i < count; i++)
            {
                uint id = Context.glCreateShader(t);
                if (id == 0)
                {
                    uint error = Context.glGetError();
                    throw new InvalidOperationException($"Failed in creating shader: {error}");
                }
                ret[i] = GlHandle.Alloc(id);
            }
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteBuffer(GlHandle handle)
        {
            Context.glDeleteBuffers(1, (uint*)&handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteBuffers(GlHandle[] handle)
        {
            if ((handle == null) || (handle.Length == 0))
            {
                throw new ArgumentNullException(nameof(handle));
            }
            fixed (GlHandle* hptr = handle)
            {
                uint* iptr = (uint*)hptr;
                for (int i = 0; i < handle.Length; i++)
                {
                    Context.glDeleteBuffers(1, iptr + i);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteVertexArrays(GlHandle[] arrays)
        {
            fixed (GlHandle* hptr = arrays)
            {
                uint* iptr = (uint*)hptr;
                for (int i = 0; i < arrays.Length; i++)
                {
                    Context.glDeleteVertexArrays(1, iptr + i);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DrawElements(GeometryType mode, uint count, TypeCode type, IntPtr ptr)
        {
            uint a;
            a = type switch
            {
                TypeCode.UInt32 => GLconst.UNSIGNED_INT,
                TypeCode.UInt16 => GLconst.UNSIGNED_SHORT,
                TypeCode.Byte => GLconst.UNSIGNED_BYTE,
                _ => throw new ArgumentException(
                $"{($"{$"Parameter error in parameter {nameof(type)} method DrawElements. "}")}{($"{$"Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are supported."}")}")
            };
            Context.glDrawElements((uint)mode, count, a, (void*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstanced(
            GeometryType mode, uint count, TypeCode type, IntPtr ptr, uint amount
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
            Context.glDrawElementsInstanced((uint)mode, count, a, (void*)ptr, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(GLOperation operation)
        {
            Context.glEnable((uint)operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FrameBufferTexture2d(FrameBufferTarget target, uint attachment, Texture2DTarget texTarget, GlHandle textureHandle, int level)
        {
            Context.glFramebufferTexture2D(
                (uint)target, attachment, (uint)texTarget, textureHandle.Value, level
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenBuffers(int count)
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h)
            {
                for (int i = 0; i < count; i++)
                {
                    Context.glGenBuffers(count, ((uint*)hptr) + i);
                }
            }
            return h;
        }

        public unsafe GlHandle[] GenFrameBuffers(int count)
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h)
            {
                Context.glGenFramebuffers(1, (uint*)hptr);
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenRenderBuffer(int count)
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h)
            {
                Context.glGenRenderbuffers(count, (uint*)hptr);
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenTextures(int count)
        {

            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h)
            {
                Context.glGenTextures(count, (uint*)hptr);
            }
            return h;
        }


        public unsafe GlHandle[] GenVertexArrays(int count)
        {

            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h)
            {
                Context.glGenVertexArrays(count, (uint*)hptr);
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetError()
        {
            return Context.glGetError();
        }

        public unsafe int GetMaskInteger(uint statusCode)
        {
            int ret = 0;
            Context.glGetIntegerv(statusCode, &ret);
            return ret;
        }

        public string GetShaderStatus(Shader s, int index, int key)
        {
            IntPtr sptr = IntPtr.Zero;
            if (InternalIO.glCheckShaderStatus(ref Context, s.handle[index].Value, key, ref sptr) == 0)
            {
                return Marshal.PtrToStringAnsi(sptr);
            }
            return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void GetTextureImage<T>(Texture2DTarget target, int level, ImageChannel channel, PixelChannelLayout channelLayout, T[] dataStream)
            where T : struct,
#if NET8_0_OR_GREATER
            INumber
#else
            IConvertible
#endif
        {
#pragma warning disable CS8500
            fixed (T* tptr = dataStream)
            {
                Context.glGetTexImage((uint)target, level, (uint)channel, (uint)channelLayout, tptr);
            }
#pragma warning restore CS8500
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle GetUniformLocation(GlHandle program, string name)
        {
            byte[] code = Encoding.UTF8.GetBytes(name);
            fixed (byte* ptr = code)
            {
                int ret = Context.glGetUniformLocation(program.Value, ptr);
#if DEBUG
                if ((ret == GLconst.INVALID_OPERATION) ||
                    (ret == GLconst.INVALID_VALUE))
                {
                    throw new InvalidOperationException();
                }
                if (ret == -1)
                {
                    throw new UniformEmptyReferenceException(nameof(program), name);
                }
#endif
                return GlHandle.SignedAlloc(ret);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void LoadShaderSource(GlHandle shaderHandle, byte[] codeStream)
        {
            fixed (byte* streamPtr = codeStream)
            {
                Context.glShaderSource(shaderHandle.Value, 1, &streamPtr, (void*)0);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadPixels(
            (int X, int Y) beginPosition, (int width, int height) size, FrameBufferChannel format, PixelChannelLayout dataType, ref byte[] stream
            )
        {
            fixed (byte* bptr = stream)
            {
                Context.glReadPixels(
                    beginPosition.X, beginPosition.Y,
                    size.width, size.height, (uint)format, (uint)dataType, bptr);
            }
        }

        public void RereadImage(
            in Image i, (int X, int Y) beginPosition, FrameBufferChannel format, PixelChannelLayout dataType)
        {
            byte[] pixels = i.PixelBuffer;
            if (!pixels.CheckArrayFormat(dataType))
            {
                throw new GlArrayFormatException(dataType, "i.PixelArray");
            }
            fixed (byte* bptr = pixels)
            {
                Context.glReadPixels(
                            beginPosition.X, beginPosition.Y, i.Width, i.Height, (uint)format, (uint)dataType, bptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferData<T>(BufferType bufferType, T bufferData, BufferUsage usage)
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
        public unsafe void SetBufferData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
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
        public unsafe void SetBufferData<T>(BufferType bufferType, ReadOnlySpan<T> bufferArray, BufferUsage usage)
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
        public unsafe void SetBufferData<T>(BufferType bufferType, Span<T> bufferArray, BufferUsage usage)
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
        public unsafe void SetBufferVectorData<T>(BufferType bufferType, ReadOnlySpan<T> bufferArray, BufferUsage usage)
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
        public unsafe void SetBufferVectorData<T>(BufferType bufferType, T[] bufferArray, BufferUsage usage)
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
        public unsafe void SetRenderBufferStorage(RenderBufferInternalFormat internalFormat, (int width, int height) size)
        {
            Context.glRenderbufferStorage(
                GLconst.RENDERBUFFER, (uint)internalFormat, size.width, size.height
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertexAttribute(uint index, int size, TypeCode t, bool normalized, uint stride, int pointer)
        {
            uint type = InternalIO.GLtype[t];
            Context.glVertexAttribPointer(index, size, type, normalized ? 1 : 0, stride, (void*)pointer);
            Context.glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureImage2d<T>(Texture2DTarget target, int level, uint internalformat, uint width, uint height,
            uint format, PixelChannelLayout type, T[] pixels) where T : struct,
#if NET8_0_OR_GREATER
            INumber
#else
            IConvertible
#endif
        {
#pragma warning disable CS8500
            fixed (T* tptr = ((pixels == null) || (pixels.Length == 0)) ? pixels : null)
            {
                Context.glTexImage2D(
                (uint)target, level, internalformat, width, height, 0, format, (uint)type, tptr);
            }
#pragma warning restore CS8500
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureImage2d<T>(Texture2DTarget target, int level, uint internalformat, uint width, uint height,
            uint format, PixelChannelLayout type, Span<T> pixelSpan) where T : struct,
#if NET8_0_OR_GREATER
            INumber 
#else
            IConvertible
#endif
        {
#pragma warning disable CS8500
            fixed (T* tptr = pixelSpan)
            {
                Context.glTexImage2D(
                (uint)target, level, internalformat, width, height, 0, format, (uint)type, tptr);
            }
#pragma warning restore CS8500
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameter(int target, uint pname, int param)
        {
            Context.glTexParameteri((uint)target, pname, param);
        }

        public static bool TryGetFunctionPackage(IntPtr handle, out GlFunction functionPackage)
        {
            return contextToGLMap.TryGetValue(handle, out functionPackage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UseProgram(GlHandle program)
        {
            Context.glUseProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttributeDivisor(uint layoutIndex, uint divisor)
        {
            Context.glVertexAttribDivisor(layoutIndex, divisor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Viewport(int x, int y, uint width, uint height)
        {
            Context.glViewport(x, y, width, height);
        }


        public void WaitAccomplishment()
        {
            Context.glFinish();
        }

        internal static GlFunction BuildGlFunctionPackage(IntPtr handle)
        {
            GlFunction pack;
            if (contextToGLMap.TryGetValue(handle, out pack))
            {
                return pack;
            }
            pack = new GlFunction { _context = (OpenglContext*)handle };
            contextToGLMap.Add(handle, pack);
            return pack;
        }

        ~GlFunction()
        {
            contextToGLMap.Remove(ContextHandle);
        }

    }
}
