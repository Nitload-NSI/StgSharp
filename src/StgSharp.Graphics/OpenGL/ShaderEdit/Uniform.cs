// -----------------------------------------------------------------------------
// file="Uniform"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Mathematics.Graphics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public class Uniform
    {

        internal GlHandle id;

        public GlHandle Handle => id;

    }

    public sealed class Uniform<T> : Uniform where T : unmanaged
    {

        internal Uniform() { }

        internal unsafe Uniform(
                        GlHandle id
        )
        {
            this.id = id;
        }

        internal static Uniform<T> FromHandle(
                                   GlHandle handle
        )
        {
            return new Uniform<T>
            {
                id = handle
            };
        }

    }

    public sealed class Uniform<T, U> : Uniform where T : unmanaged where U : struct
    {

        internal Uniform() { }

        internal unsafe Uniform(
                        GlHandle id
        )
        {
            this.id = id;
        }

        internal static Uniform<T, U> FromHandle(
                                      GlHandle handle
        )
        {
            return new Uniform<T, U>
            {
                id = handle
            };
        }

    }

    public sealed class Uniform<T, U, V> : Uniform where T : unmanaged where U : struct
        where V : struct
    {

        internal Uniform() { }

        internal unsafe Uniform(
                        GlHandle id
        )
        {
            this.id = id;
        }

        internal static Uniform<T, U, V> FromHandle(
                                         GlHandle handle
        )
        {
            return new Uniform<T, U, V>
            {
                id = handle
            };
        }

    }

    public sealed class Uniform<T, U, V, W> : Uniform where T : unmanaged where U : struct
        where V : struct
        where W : struct
    {

        internal Uniform() { }

        internal unsafe Uniform(
                        GlHandle id
        )
        {
            this.id = id;
        }

        internal static Uniform<T, U, V, W> FromHandle(
                                            GlHandle handle
        )
        {
            return new Uniform<T, U, V, W>
            {
                id = handle
            };
        }

    }

    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull] Uniform<GraphicsMatrix> uniform,
                    GraphicsMatrix mat
        )
        {
            Context.glUniformMatrix4fv(uniform.id.SignedValue, 1, (byte)0, (float*)&mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull]Uniform<float, float, float, float> uniform,
                    Vec4 vec
        )
        {
            Context.glUniform1fv(uniform.id.SignedValue, 4, (float*)&vec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull]Uniform<Vec4> uniform,
                    Vec4 vec
        )
        {
            Context.glUniform1fv(uniform.id.SignedValue, 4, (float*)&vec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull]Uniform<float> uniform,
                    float v0
        )
        {
            Context.glUniform1f(uniform.id.SignedValue, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull]Uniform<int> uniform,
                    int i0
        )
        {
            Context.glUniform1i(uniform.id.SignedValue, i0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull] Uniform<float, float> uniform,
                    float v0,
                    float v1
        )
        {
            Context.glUniform2f(uniform.id.SignedValue, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull] Uniform<float, float, float> uniform,
                    float v0,
                    float v1,
                    float v2
        )
        {
            Context.glUniform3f(uniform.id.SignedValue, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull] Uniform<float, float, float, float> uniform,
                    float v0,
                    float v1,
                    float v2,
                    float v3
        )
        {
            Context.glUniform4f(uniform.id.SignedValue, v0, v1, v2, v3);
        }

    }
}