// -----------------------------------------------------------------------------
// file="OpenGLFunction.Shader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nitload.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle CreateGraphicProgram()
        {
            return CreateProgram();
        }

        public GlHandle[] CreateProgram(
                          int count
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            GlHandle[] handles = new GlHandle[count];
            for (int i = 0; i < handles.Length; i++) {
                handles[i] = CreateProgram();
            }
            return handles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle CreateShader(
                        ShaderType type
        )
        {
            GlHandle handle = GlHandle.Create(__context->glCreateShader((uint)type));
            if (handle.Value == 0)
            {
                uint error = __context->glGetError();
                throw new InvalidOperationException($"Failed in creating shader: {error}");
            }
            return handle;
        }

        public GlHandle[] CreateShaderSet(
                          int count,
                          ShaderType type
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            GlHandle[] handles = new GlHandle[count];
            for (int i = 0; i < handles.Length; i++) {
                handles[i] = CreateShader(type);
            }
            return handles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetShaderStatus(
                      GlHandle shader,
                      ShaderStatus status
        )
        {
            return GetShaderStatus(shader, (uint)status);
        }

        public string GetShaderStatus(
                      GlHandle shader,
                      uint statusName
        )
        {
            int status;
            __context->glGetShaderiv(shader.Value, statusName, &status);
            if (status != 0) {
                return string.Empty;
            }

            int logLength;
            __context->glGetShaderiv(shader.Value, glConst.INFO_LOG_LENGTH, &logLength);
            if (logLength <= 1) {
                return string.Empty;
            }

            byte[] log = new byte[logLength];
            fixed (byte* logPtr = log)
            {
                int written;
                __context->glGetShaderInfoLog(shader.Value, log.Length, &written, logPtr);
                return Encoding.UTF8.GetString(log, 0, Math.Clamp(written, 0, log.Length));
            }
        }

        public void ShaderSource(
                    GlHandle shader,
                    ReadOnlySpan<byte> source
        )
        {
            int length = source.Length;
            fixed (byte* sourcePtr = source)
            {
                byte emptySource = 0;
                byte* sourceAddress = sourcePtr == null ? &emptySource : sourcePtr;
                __context->glShaderSource(shader.Value, 1, &sourceAddress, &length);
            }
        }

        public void ShaderSource(
                    GlHandle shader,
                    ReadOnlySpan<char> source
        )
        {
            int byteCount = Encoding.UTF8.GetByteCount(source);
            Span<byte> utf8Source = byteCount < 1024 ?
                                    stackalloc byte[byteCount] :
                                    new byte[byteCount];
            int written = Encoding.UTF8.GetBytes(source, utf8Source);
            ShaderSource(shader, utf8Source[..written]);
        }

        public void ShaderSource(
                    GlHandle shader,
                    string source
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            int byteCount = Encoding.UTF8.GetByteCount(source);
            Span<byte> utf8Source = byteCount < 1024 ?
                                    stackalloc byte[byteCount] :
                                    new byte[byteCount];
            int written = Encoding.UTF8.GetBytes(source.AsSpan(), utf8Source);
            ShaderSource(shader, utf8Source[..written]);
        }

    }
}
