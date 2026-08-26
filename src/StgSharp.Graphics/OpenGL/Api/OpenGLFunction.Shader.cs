// -----------------------------------------------------------------------------
// file="OpenGLFunction.Shader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
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
            GlHandle handle = CreateShader((uint)type);
            if (handle.Value == 0)
            {
                uint error = _context->glGetError();
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
            _context->glGetShaderiv(shader.Value, statusName, &status);
            if (status != 0) {
                return string.Empty;
            }

            int logLength;
            _context->glGetShaderiv(shader.Value, glConst.INFO_LOG_LENGTH, &logLength);
            if (logLength <= 1) {
                return string.Empty;
            }

            byte[] log = new byte[logLength];
            fixed (byte* logPtr = log)
            {
                int written;
                _context->glGetShaderInfoLog(shader.Value, log.Length, &written, logPtr);
                return Encoding.UTF8.GetString(log, 0, Math.Clamp(written, 0, log.Length));
            }
        }

        public GlHandle GetUniformLocation(
                        GlHandle program,
                        string name
        )
        {
            ArgumentNullException.ThrowIfNull(name);

            int byteCount = Encoding.UTF8.GetByteCount(name);
            Span<byte> utf8Name = byteCount < 256
                                  ? stackalloc byte[byteCount + 1]
                                  : new byte[byteCount + 1];
            int written = Encoding.UTF8.GetBytes(name.AsSpan(), utf8Name);
            utf8Name[written] = 0;

            int location;
            fixed (byte* namePtr = utf8Name) {
                location = _context->glGetUniformLocation(program.Value, namePtr);
            }

            #if DEBUG
            if (location == -1) {
                throw new UniformEmptyReferenceException(nameof(program), name);
            }
            #endif

            return GlHandle.CreateSigned(location);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uniform<T> GetUniform<T>(
                          GlHandle program,
                          string name
        ) where T : unmanaged
        {
            return new Uniform<T>(GetUniformLocation(program, name));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uniform<T, U> GetUniform<T, U>(
                             GlHandle program,
                             string name
        ) where T : unmanaged where U : struct
        {
            return new Uniform<T, U>(GetUniformLocation(program, name));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uniform<T, U, V> GetUniform<T, U, V>(
                                GlHandle program,
                                string name
        ) where T : unmanaged where U : struct where V : struct
        {
            return new Uniform<T, U, V>(GetUniformLocation(program, name));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uniform<T, U, V, W> GetUniform<T, U, V, W>(
                                   GlHandle program,
                                   string name
        ) where T : unmanaged where U : struct where V : struct where W : struct
        {
            return new Uniform<T, U, V, W>(GetUniformLocation(program, name));
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
                _context->glShaderSource(shader.Value, 1, &sourceAddress, &length);
            }
        }

        public void ShaderSource(
                    GlHandle shader,
                    string source
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            int byteCount = Encoding.UTF8.GetByteCount(source);
            Span<byte> utf8Source = byteCount < 1024
                                    ? stackalloc byte[byteCount]
                                    : new byte[byteCount];
            int written = Encoding.UTF8.GetBytes(source.AsSpan(), utf8Source);
            ShaderSource(shader, utf8Source[..written]);
        }

    }
}
