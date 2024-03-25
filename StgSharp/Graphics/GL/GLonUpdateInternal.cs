//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GLonUpdateInternal.cs"
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
using System.Linq;

namespace StgSharp.Graphics
{
    internal unsafe partial class GLonUpdate : IglFunc
    {

        private IntPtr ActiveTexturePtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalActiveTexture;
        private IntPtr AttachShaderPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalAttachShader;
        private IntPtr BindBufferPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalBindBuffer;
        private IntPtr BindTexturePtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalBindTexture;
        private IntPtr BindVertexArrayPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalBindVertexArray;
        private IntPtr BufferDataPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalSetBufferData;
        private IntPtr ClearColorPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalClearColor;
        private IntPtr ClearGraphicErrorPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalClearGraphicError;
        private IntPtr ClearPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalClear;
        private IntPtr CompileShaderPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalCompileShader;
        private IntPtr CreateProgramPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalCreateProgram;
        private IntPtr CreateProgramSetPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalCreateProgramSet;
        private IntPtr CreateShaderPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalCreateShader;
        private IntPtr DeleBufferPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalDeleteBuffer;
        private IntPtr DeleteVertexArrayPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalDeleteVertexArrays;
        private IntPtr DrawElementsPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalDrawElements;
        private IntPtr GenBufferPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalGenBuffers;
        private IntPtr GenTexturesPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalGenTextures;
        private IntPtr GetUniformLocationPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalGetUniformLocation;
        private IntPtr SetVertexArrayPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalSetVertexAttribute;
        private IntPtr ShaderSourcePtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalShaderSource;
        private IntPtr textureImage2dPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalTextureImage2d;
        private IntPtr TextureParameterPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalTextureParameter;
        private IntPtr UseProgramPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalUseProgram;
        private IntPtr ViewPortPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&InternalViewport;

        private static void InternalActiveTexture(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint t;
            frameBuffer.TryDequeue(out t);
            activatedContext->glActiveTexture(t);
        }

        private static void InternalAttachShader(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint p, s;
            frameBuffer.TryDequeue(out p, out s);
            activatedContext->glAttachShader(p, s);
        }

        private static void InternalBindBuffer(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = new M128();
            frameBuffer.TryDequeue(out m);
            uint usage = m.Read<uint>(0);
            uint handle = *(uint*)m.Read<ulong>(1);
            activatedContext->glBindBuffer(usage, handle);
        }

        private static void InternalBindTexture(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glBindTexture(
                m.Read<uint>(0),
                *(uint*)m.Read<ulong>(1)
                );
        }

        private static void InternalBindVertexArray(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr i;
            frameBuffer.TryDequeue(out i);
            activatedContext->glBindVertexArray(*(uint*)i);
        }

        private static void InternalClear(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint mt;
            frameBuffer.TryDequeue(out mt);
            activatedContext->glClear(mt);
        }

        private static void InternalClearColor(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            float r, g, b, a;
            frameBuffer.TryDequeue(out r, out g, out b, out a);
            activatedContext->glClearColor(r, g, b, a);
        }

        private static void InternalClearGraphicError(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            while (activatedContext->glGetError() != 0) { }
        }

        private static unsafe void InternalCompileShader(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr handle = default;
            frameBuffer.TryDequeue(out handle);
            activatedContext->glCompileShader(*(uint*)handle);
        }

        private static void InternalCreateProgram(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr programHandle = default;
            frameBuffer.TryDequeue(out programHandle);
            *(uint*)programHandle = activatedContext->glCreateProgram();
        }

        private static void InternalCreateProgramSet(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint* programHandle;
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            programHandle = (uint*)m.Read<ulong>(0);
            for (int i = 0; i < m.Read<int>(2); i++)
            {
                *(programHandle + i) = activatedContext->glCreateProgram();
            }
        }

        private static unsafe void InternalCreateShader(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            int count = m.Read<int>(0); uint type = m.Read<uint>(1); uint* handle = (uint*)m.Read<ulong>(1);
            for (int i = 0; i < count; i++)
            {
                *(handle + i) = activatedContext->glCreateShader(type);
            }
        }

        private static void InternalDeleteBuffer(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glDeleteBuffers(m.Read<uint>(2), (uint*)m.Read<ulong>(0));
            for (int i = 0; i < m.Read<uint>(2); i++)
            {
                *(((uint*)m.Read<ulong>(0)) + i) = 0;
            }
        }

        private static void InternalDeleteVertexArrays(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glDeleteVertexArrays((uint)m.Read<int>(0), (uint*)m.Read<ulong>(0));
        }

        private static void InternalDrawElements(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint mode, count, type;
            IntPtr ptr;
            frameBuffer.TryDequeue(out mode, out count, out type);
            frameBuffer.TryDequeue(out ptr);
            activatedContext->glDrawElements(mode, count, type, (void*)ptr);
        }

        private static void InternalGenBuffers(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glGenBuffers(m.Read<uint>(2), (uint*)m.Read<ulong>(0));
        }

        private static void InternalGenTextures(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glGenTextures(m.Read<uint>(2), (uint*)m.Read<ulong>(0));
        }

        private static void InternalGetUniformLocation(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr programHandle = default, nameHandle = default, targetHandle = default;
            frameBuffer.TryDequeue(out programHandle, out nameHandle);
            frameBuffer.TryDequeue(out targetHandle);
            *(int*)targetHandle = activatedContext->glGetUniformLocation(*(uint*)programHandle, (byte*)nameHandle);
        }

        private static void InternalSetBufferData(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint type = 0, use = 0;
            IntPtr size = default;
            glArrayHandle array = default;
            frameBuffer.TryDequeue(out type, out use);
            frameBuffer.TryDequeue(out size);
            frameBuffer.TryDequeue(out array);
            activatedContext->glBufferData(type, size, array.AddressOfArray(), use);
        }

        private static void InternalSetVertexAttribute(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            uint index, type, stride;
            int size, normalized, pointer;
            frameBuffer.TryDequeue(out index, out type, out stride);
            frameBuffer.TryDequeue(out size, out normalized, out pointer);
            activatedContext->glVertexAttribPointer(index, size, type, normalized, stride, (void*)pointer);
            activatedContext->glEnableVertexAttribArray(index);
        }

        private static unsafe void InternalShaderSource(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr handle = default;
            glArrayHandle stream = default;
            frameBuffer.TryDequeue(out handle);
            frameBuffer.TryDequeue(out stream);
            IntPtr buffer = stream.AddressOfArray();
            activatedContext->glShaderSource(*(uint*)handle, 1, (byte**)&buffer, (void*)0);
        }

        private static void InternalTextureImage2d(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr pix = IntPtr.Zero;
            uint target, level, internalFormat, width, height, format, type;
            frameBuffer.TryDequeue(out pix);
            frameBuffer.TryDequeue(out level, out internalFormat, out width, out height);
            frameBuffer.TryDequeue(out target, out format, out type);
            activatedContext->glTexImage2D(target, (int)level, internalFormat, width, height, 0, format, type, (void*)pix);
        }

        private static void InternalTextureParameter(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glTextureParameteri(m.Read<uint>(0), m.Read<uint>(2), m.Read<int>(3));
        }

        private static void InternalUseProgram(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            IntPtr handle = IntPtr.Zero;
            frameBuffer.TryDequeue(out handle);
            activatedContext->glUseProgram(*(uint*)handle);
        }

        private static void InternalViewport(FrameOperationBuffer frameBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameBuffer.TryDequeue(out m);
            activatedContext->glViewport(m.Read<int>(0), m.Read<int>(1), m.Read<uint>(2), m.Read<uint>(3));
        }

    }
}