//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction"
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
using StgSharp.Graphics;
using StgSharp.Graphics;
using StgSharp.Internal;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Numeric;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        private static readonly Dictionary<IntPtr, OpenGLFunction> contextToGLMap = [];
        private static readonly ThreadLocal<OpenGLFunction> _currentGL = new ThreadLocal<OpenGLFunction>();
        private OpenglContext* _context;

        private Lazy<int> _attachmentColorRange;

        private OpenGLFunction()
        {
            textureUnitCountGetter = glConst.MAX_TEXTURE_IMAGE_UNITS;
            UnusedTextureImageUint = [];
            _attachmentColorRange = new Lazy<int>(
                () => { return GetMaskInteger((uint)glConst.MAX_COLOR_ATTACHMENTS); });
        }

        public int AttachmentColorRange
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _attachmentColorRange.Value;
        }

        public static OpenGLFunction CurrentGL
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _currentGL.Value!;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set => _currentGL.Value = value;
        }

        internal IntPtr ContextHandle => (IntPtr)_context;

        protected ref OpenglContext Context
        {
            get
            {
                #if DEBUG
                ArgumentNullException.ThrowIfNull(_context, nameof(_context));
                #endif
                return ref (*_context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ActiveTextureUnit(
                    uint type
        )
        {
            _context->glActiveTexture(type);
        }

        [Conditional("DEBUG")]
        public void Assert(
                    bool shouldReportWhenNormal
        )
        {
            uint code = _context->glGetError();
            if (code != glConst.NO_ERROR) {
                throw new GlExecutionException(code);
            }
            if (shouldReportWhenNormal) {
                Console.WriteLine("OpenGL runs normally");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBuffer(
                    BufferType bufferType,
                    GlHandle handle
        )
        {
            _context->glBindBuffer((uint)bufferType, handle.Value);
        }

        /// <summary>
        ///   Bind a frame BufferHandle to a frame BufferHandle target Same function as <see ///  
        ///   href="https://docs._gl/gl3/glBindFramebuffer">glBindFramebuffer</see>.
        /// </summary>
        /// <param _label="target">
        ///   The frame BufferHandle target of the binding operation.
        /// </param>
        /// <param _label="handle">
        ///   The handle of the frame BufferHandle object to bind.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFrameBuffer(
                    FrameBufferTarget target,
                    GlHandle handle
        )
        {
            _context->glBindFramebuffer((uint)target, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindRenderBuffer(
                    GlHandle handle
        )
        {
            _context->glBindRenderbuffer(glConst.RENDERBUFFER, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindVertexArray(
                    GlHandle handle
        )
        {
            _context->glBindVertexArray(handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FrameBufferStatus CheckFrameBufferStatus(
                                 FrameBufferTarget target
        )
        {
            return (FrameBufferStatus)_context->glCheckFramebufferStatus((uint)target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(
                    MaskBufferBit mask
        )
        {
            _context->glClear((uint)mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearError()
        {
            uint code = 0;
            for (int i = 0; i < 1024; i++)
            {
                code = _context->glGetError();
                if (code == 0) {
                    return;
                }

                // Console.WriteLine(code);
            }
            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads) {
                Debugger.Break();
            }
            throw new GlErrorOverflowException(1024, code);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ClearGraphicError()
        {
            while (_context->glGetError() != 0) { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CombineFrameBufferRenderBuffer(
                    FrameBufferAttachment attachment,
                    GlHandle rboHandle
        )
        {
            _context->glFramebufferRenderbuffer(glConst.FRAMEBUFFER, (uint)attachment,
                                                glConst.RENDERBUFFER, rboHandle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle CreateGraphicProgram()
        {
            return GlHandle.Create(_context->glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] CreateProgram(
                                 int count
        )
        {
            GlHandle[] h = new GlHandle[count];
            for (int i = 0; i < count; i++) {
                h.SetValue(i, _context->glCreateProgram());
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle[] CreateShaderSet(
                          int count,
                          ShaderType type
        )
        {
            GlHandle[] ret = new GlHandle[count];
            uint t = (uint)type;
            for (int i = 0; i < count; i++)
            {
                uint id = _context->glCreateShader(t);
                if (id == 0)
                {
                    uint error = _context->glGetError();
                    throw new InvalidOperationException($"Failed in creating shader: {error}");
                }
                ret[i] = GlHandle.Create(id);
            }
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteBuffer(
                           GlHandle handle
        )
        {
            _context->glDeleteBuffers(1, (uint*)&handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteBuffers(
                           GlHandle[] handle
        )
        {
            if ((handle == null) || (handle.Length == 0)) {
                throw new ArgumentNullException(nameof(handle));
            }
            fixed (GlHandle* hptr = handle)
            {
                uint* iptr = (uint*)hptr;
                for (int i = 0; i < handle.Length; i++) {
                    _context->glDeleteBuffers(1, iptr + i);
                }
            }
        }

        /// <summary>
        ///   Delete a render BufferHandle object Very simlilar function as <see ///  
        ///   href="https://docs.gl/gl3/glDeleteRenderbuffers">glDeleteRenderbuffers</see>. The only
        ///   deifference is that this mehtod can only delete one renderbuffer at the same time.
        /// </summary>
        /// <param _label="bufferHandle">
        ///   Handle to BufferHandle to delete.
        /// </param>
        public unsafe void DeleteRenderBuffer(
                           GlHandle bufferHandle
        )
        {
            _context->glDeleteRenderbuffers(1, &bufferHandle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteVertexArrays(
                           GlHandle[] arrays
        )
        {
            fixed (GlHandle* hptr = arrays)
            {
                uint* iptr = (uint*)hptr;
                for (int i = 0; i < arrays.Length; i++) {
                    _context->glDeleteVertexArrays(1, iptr + i);
                }
            }
        }

        public void DepthMaskAccess(
                    bool isWrite
        )
        {
            _context->glDepthMask(isWrite ? (byte)1 : (byte)0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable(
                    glOperation operation
        )
        {
            _context->glDisable((uint)operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DrawArrays(
                           GeometryType mode,
                           int first,
                           int count
        )
        {
            _context->glDrawArrays((uint)mode, first, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DrawElements(
                           GeometryType mode,
                           int count,
                           TypeCode type,
                           IntPtr ptr
        )
        {
            uint a;
            a = type switch
            {
                TypeCode.UInt32 => glConst.UNSIGNED_INT,
                TypeCode.UInt16 => glConst.UNSIGNED_SHORT,
                TypeCode.Byte => glConst.UNSIGNED_BYTE,
                _ => throw new ArgumentException(
                    $"{$"{$"Parameter error in parameter {nameof(type)} method DrawElements. "}"}{$"{$"Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are supported."}"}")
            };
            _context->glDrawElements((uint)mode, count, a, (nint*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstanced(
                    GeometryType mode,
                    int count,
                    TypeCode type,
                    IntPtr ptr,
                    int amount
        )
        {
            uint a;
            a = type switch
            {
                TypeCode.UInt32 => glConst.UNSIGNED_INT,
                TypeCode.UInt16 => glConst.UNSIGNED_SHORT,
                TypeCode.Byte => glConst.UNSIGNED_BYTE,
                _ => throw new ArgumentException(
                    $"{$"{$"Parameter error in parameter {nameof(type)} method DrawElements. "}"}{$"{$"Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are supported."}"}")
            };
            _context->glDrawElementsInstanced((uint)mode, count, a, (nint*)ptr, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(
                    glOperation operation
        )
        {
            _context->glEnable((uint)operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FrameBufferTexture2d(
                    FrameBufferTarget target,
                    uint attachment,
                    Texture2DTarget texTarget,
                    GlHandle textureHandle,
                    int level
        )
        {
            _context->glFramebufferTexture2D((uint)target, attachment, (uint)texTarget,
                                             textureHandle.Value, level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenBuffers(
                                 int count
        )
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h)
            {
                for (int i = 0; i < count; i++) {
                    _context->glGenBuffers(count, ((uint*)hptr) + i);
                }
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenRenderBuffer(
                                 int count
        )
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h) {
                _context->glGenRenderbuffers(count, (uint*)hptr);
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenTextures(
                                 int count
        )
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h) {
                _context->glGenTextures(count, (uint*)hptr);
            }
            return h;
        }

        public unsafe GlHandle[] GenVertexArrays(
                                 int count
        )
        {
            GlHandle[] h = new GlHandle[count];
            fixed (GlHandle* hptr = h) {
                _context->glGenVertexArrays(count, (uint*)hptr);
            }
            return h;
        }

        public unsafe int GetMaskInteger(
                          uint statusCode
        )
        {
            int ret = 0;
            _context->glGetIntegerv(statusCode, &ret);
            return ret;
        }

        public string GetShaderStatus(
                      Shader s,
                      int index,
                      int key
        )
        {
            IntPtr sptr = IntPtr.Zero;
            if (GraphicFramework.glCheckShaderStatus(ref Context, s.handle[index].Value, key,
                                                     ref sptr) == 0) {
                return Marshal.PtrToStringAnsi(sptr);
            }
            return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void GetTextureImage<T>(
                           Texture2DTarget target,
                           int level,
                           ImageChannel channel,
                           PixelChannelLayout channelLayout,
                           T[] dataStream
        ) where T : unmanaged,INumber<T>
        {
            fixed (T* tptr = dataStream) {
                _context->glGetTexImage((uint)target, level, (uint)channel, (uint)channelLayout,
                                        (nint*)tptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureLevelProperty(
                    uint textureTypeMask,
                    int level,
                    uint propertyMask,
                    out int propertyValue
        )
        {
            fixed (int* iptr = &propertyValue) {
                _context->glGetTexLevelParameteriv(textureTypeMask, level, propertyMask, iptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle GetUniformLocation(
                               GlHandle program,
                               string name
        )
        {
            byte[] code = Encoding.UTF8.GetBytes(name);
            fixed (byte* ptr = code)
            {
                int ret = _context->glGetUniformLocation(program.Value, ptr);
                #if DEBUG
                if ((ret == glConst.INVALID_OPERATION) || (ret == glConst.INVALID_VALUE))
                {
                    throw new InvalidOperationException();
                }
                if (ret == -1) {
                    throw new UniformEmptyReferenceException(nameof(program), name);
                }
                #endif
                return GlHandle.CreateSigned(ret);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void LoadShaderSource(
                           GlHandle shaderHandle,
                           byte[] codeStream
        )
        {
            fixed (byte* streamPtr = codeStream) {
                _context->glShaderSource(shaderHandle.Value, 1, &streamPtr, (int*)0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void LoadShaderSource(
                           GlHandle shaderHandle,
                           ReadOnlySpan<char> codeStream
        )
        {
            fixed (char* streamPtr = codeStream) {
                _context->glShaderSource(shaderHandle.Value, 1, (byte**)&streamPtr, (int*)0);
            }
        }

        public unsafe void LoadShaderSource(
                           GlHandle shaderHandle,
                           ReadOnlySpan<byte> codeStream
        )
        {
            fixed (byte* streamPtr = codeStream)
            {
                byte* bptr = streamPtr;
                _context->glShaderSource(shaderHandle.Value, 1, &bptr, (int*)0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonMode(
                    FaceMode mode
        )
        {
            _context->glPolygonMode(glConst.FRONT_AND_BACK, (uint)mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadPixels(
                    (int X, int Y) beginPosition,
                    (int width, int height) size,
                    FrameBufferChannel format,
                    PixelChannelLayout dataType,
                    ref byte[] stream
        )
        {
            fixed (byte* bptr = stream) {
                _context->glReadPixels(beginPosition.X, beginPosition.Y, size.width, size.height,
                                       (uint)format, (uint)dataType, (nint*)bptr);
            }
        }

        public void RereadImage(
                    in Image i,
                    (int X, int Y) beginPosition,
                    FrameBufferChannel format,
                    PixelChannelLayout dataType
        )
        {
            byte[] pixels = i.PixelBuffer;
            if (!pixels.CheckArrayFormat(dataType)) {
                throw new GlArrayFormatException(dataType, "i.PixelArray");
            }
            fixed (byte* bptr = pixels) {
                _context->glReadPixels(beginPosition.X, beginPosition.Y, i.Width, i.Height,
                                       (uint)format, (uint)dataType, (nint*)bptr);
            }
            i.PixelUpdateCount++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferData<T>(
                           BufferType bufferType,
                           T bufferData,
                           BufferUsage usage
        ) where T : unmanaged,INumber<T>
        {
            int size = Marshal.SizeOf<T>();

            T* p = &bufferData;

            _context->glBufferData((uint)bufferType, (nuint)size, (nint*)p, (uint)usage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferData<T>(
                           BufferType bufferType,
                           T[] bufferArray,
                           BufferUsage usage
        ) where T : unmanaged,INumber<T>
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null) {
                return;
            }

            fixed (void* bufferPtr = bufferArray) {
                _context->glBufferData((uint)bufferType, (nuint)size, (nint*)bufferPtr,
                                       (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferData<T>(
                           BufferType bufferType,
                           ReadOnlySpan<T> bufferSpan,
                           BufferUsage usage
        ) where T : unmanaged,INumber<T>
        {
            int size = bufferSpan.Length * Marshal.SizeOf<T>();
            if (bufferSpan.IsEmpty) {
                return;
            }

            fixed (void* bufferPtr = bufferSpan) {
                _context->glBufferData((uint)bufferType, (nuint)size, (IntPtr*)bufferPtr,
                                       (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferData<T>(
                           BufferType bufferType,
                           Span<T> bufferArray,
                           BufferUsage usage
        ) where T : unmanaged, INumber<T>
        {
            int size = bufferArray.Length * Marshal.SizeOf<T>();
            if (bufferArray.IsEmpty) {
                return;
            }


            fixed (void* bufferPtr = bufferArray) {
                _context->glBufferData((uint)bufferType, (nuint)size, (IntPtr*)bufferPtr,
                                       (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferVectorData<T>(
                           BufferType bufferType,
                           ReadOnlySpan<T> bufferSpan,
                           BufferUsage usage
        ) where T : unmanaged, IUnmanagedVector<T>
        {
            int size = bufferSpan.Length * Marshal.SizeOf<T>();

            fixed (void* bufferPtr = bufferSpan) {
                _context->glBufferData((uint)bufferType, (nuint)size, (IntPtr*)bufferPtr,
                                       (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetBufferVectorData<T>(
                           BufferType bufferType,
                           T[] bufferArray,
                           BufferUsage usage
        ) where T : unmanaged, IUnmanagedVector<T>
        {
            int size = bufferArray.Length * Marshal.SizeOf(typeof(T));
            if (bufferArray == null) {
                return;
            }
            fixed (void* bufferPtr = bufferArray) {
                _context->glBufferData((uint)bufferType, (nuint)size, (IntPtr*)bufferPtr,
                                       (uint)usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetRenderBufferStorage(
                           RenderBufferInternalFormat internalFormat,
                           (int width, int height) size
        )
        {
            _context->glRenderbufferStorage(glConst.RENDERBUFFER, (uint)internalFormat, size.width,
                                            size.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetRenderBufferStorage(
                           RenderBufferInternalFormat internalFormat,
                           (uint width, uint height) size
        )
        {
            _context->glRenderbufferStorage(glConst.RENDERBUFFER, (uint)internalFormat,
                                            (int)size.width, (int)size.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertexAttribute(
                    uint index,
                    int size,
                    TypeCode t,
                    bool normalized,
                    int stride,
                    int pointer
        )
        {
            uint type = GraphicFramework.GLtype[(int)t];
            _context->glVertexAttribPointer(index, size, type, (byte)(normalized ? 1 : 0), stride,
                                            (nint*)pointer);
            _context->glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureImage2d<T>(
                    Texture2DTarget target,
                    int level,
                    ImageChannel sourceChannel,
                    int width,
                    int height,
                    ImageChannel targetChannel,
                    PixelChannelLayout type,
                    T[] pixels
        ) where T : unmanaged, INumber<T>
        {
            fixed (T* tptr = pixels)
            {
                T* ptr = ((pixels == null) || (pixels.Length == 0)) ? (T*)IntPtr.Zero : tptr;
                _context->glTexImage2D((uint)target, level, (int)sourceChannel, width, height, 0,
                                       (uint)targetChannel, (uint)type, (nint*)tptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureImage2d<T>(
                    Texture2DTarget target,
                    int level,
                    ImageChannel sourceChannel,
                    int width,
                    int height,
                    ImageChannel targetChannel,
                    PixelChannelLayout type,
                    Span<T> pixelSpan
        ) where T : unmanaged, INumber<T>
        {
            fixed (T* tPtr = pixelSpan) {
                _context->glTexImage2D((uint)target, level, (int)sourceChannel, width, height, 0,
                                       (uint)targetChannel, (uint)type, (nint*)tPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameter(
                    int target,
                    uint pname,
                    int param
        )
        {
            _context->glTexParameteri((uint)target, pname, param);
        }

        public static bool TryGetFunctionPackage(
                           IntPtr handle,
                           out OpenGLFunction functionPackage
        )
        {
            return contextToGLMap.TryGetValue(handle, out functionPackage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttributeDivisor(
                    uint layoutIndex,
                    uint divisor
        )
        {
            _context->glVertexAttribDivisor(layoutIndex, divisor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitAccomplishment()
        {
            _context->glFinish();
        }

        internal static OpenGLFunction BuildGlFunctionPackage(
                                       IntPtr handle
        )
        {
            if (contextToGLMap.TryGetValue(handle, out OpenGLFunction pack)) {
                return pack;
            }
            pack = new OpenGLFunction
            {
                _context = (OpenglContext*)handle
            };
            contextToGLMap.Add(handle, pack);
            return pack;
        }

        ~OpenGLFunction()
        {
            _ = contextToGLMap.Remove(ContextHandle);
        }

    }
}

