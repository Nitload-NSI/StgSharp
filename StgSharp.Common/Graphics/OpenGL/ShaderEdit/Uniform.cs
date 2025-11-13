//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Uniform"
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
using StgSharp.Mathematics.Graphic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public class Uniform
    {

        internal GlHandle id;

        public GlHandle Handle => id;

    }

    public sealed class Uniform<T> : Uniform where T: unmanaged
    {

        internal Uniform() { }

        internal unsafe Uniform(GlHandle id)
        {
            this.id = id;
        }

        internal static Uniform<T> FromHandle(GlHandle handle)
        {
            return new Uniform<T>
            {
                id = handle
            };
        }

    }

    public sealed class Uniform<T, U> : Uniform where T: unmanaged where U: struct
    {

        internal Uniform() { }

        internal unsafe Uniform(GlHandle id)
        {
            this.id = id;
        }

        internal static Uniform<T, U> FromHandle(GlHandle handle)
        {
            return new Uniform<T, U>
            {
                id = handle
            };
        }

    }

    public sealed class Uniform<T, U, V> : Uniform where T: unmanaged where U: struct where V: struct
    {

        internal Uniform() { }

        internal unsafe Uniform(GlHandle id)
        {
            this.id = id;
        }

        internal static Uniform<T, U, V> FromHandle(GlHandle handle)
        {
            return new Uniform<T, U, V>
            {
                id = handle
            };
        }

    }

    public sealed class Uniform<T, U, V, W> : Uniform where T: unmanaged where U: struct where V: struct where W: struct
    {

        internal Uniform() { }

        internal unsafe Uniform(GlHandle id)
        {
            this.id = id;
        }

        internal static Uniform<T, U, V, W> FromHandle(GlHandle handle)
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
        public void SetUniformValue([NotNull] Uniform<Matrix44> uniform, Matrix44 mat)
        {
            Matrix44 temp = mat;
            Context.glUniformMatrix4fv(uniform.id.SignedValue, 1, false, (float*)&temp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue([NotNull]Uniform<float, float, float, float> uniform, Vec4 vec)
        {
            Vec4 temp = vec;
            Context.glUniform1fv(uniform.id.SignedValue, 4, &temp.reg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue([NotNull]Uniform<Vec4> uniform, Vec4 vec)
        {
            Vec4 temp = vec;
            Context.glUniform1fv(uniform.id.SignedValue, 4, &temp.reg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue([NotNull]Uniform<float> uniform, float v0)
        {
            Context.glUniform1f(uniform.id.SignedValue, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue([NotNull]Uniform<int> uniform, int i0)
        {
            Context.glUniform1i(uniform.id.SignedValue, i0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue([NotNull] Uniform<float, float> uniform, float v0, float v1)
        {
            Context.glUniform2f(uniform.id.SignedValue, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue([NotNull] Uniform<float, float, float> uniform, float v0, float v1, float v2)
        {
            Context.glUniform3f(uniform.id.SignedValue, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    [NotNull] Uniform<float, float, float, float> uniform,
                    float v0,
                    float v1,
                    float v2,
                    float v3)
        {
            Context.glUniform4f(uniform.id.SignedValue, v0, v1, v2, v3);
        }

    }
}