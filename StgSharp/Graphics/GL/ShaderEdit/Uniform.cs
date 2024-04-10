//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Uniform.cs"
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
using StgSharp.Graphic;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public class Uniform
    {
        protected IntPtr binding;
        internal glSHandle id;

        
        public glSHandle Handle
        {
            get => id;
        }
    }

    public sealed class Uniform<T> : Uniform
        where T : struct
    {

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(
            ShaderProgram program, string name, Form binding)
        {
            this.binding = binding.graphicContextID;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal unsafe Uniform(
            ShaderProgram program, string name, IntPtr binding)
        {
            this.binding = binding;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal Uniform()
        {

        }

        internal static Uniform<T> FromHandle(glSHandle handle)
        {
            return new Uniform<T>() { id = handle };
        }

    }

    public sealed class Uniform<T, U>: Uniform
        where T : struct
        where U : struct
    {

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(
            ShaderProgram program, string name, Form binding)
        {
            this.binding = binding.graphicContextID;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal unsafe Uniform(
            ShaderProgram program, string name, IntPtr binding)
        {
            this.binding = binding;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal Uniform()
        {

        }

        internal static Uniform<T,U> FromHandle(glSHandle handle)
        {
            return new Uniform<T, U>() { id = handle };
        }

    }

    public sealed class Uniform<T, U, V>:Uniform
        where T : struct
        where U : struct
        where V : struct
    {

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(
            ShaderProgram program, string name)
        {
            this.binding = (IntPtr)GL.api;
            id = GL.GetUniformLocation(program.handle, name);
        }

        public unsafe Uniform(ShaderProgram program, string name, Form binding)
        {
            this.binding = binding.graphicContextID;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal Uniform()
        {

        }

        internal static Uniform<T,U,V> FromHandle(glSHandle handle)
        {
            return new Uniform<T, U, V>() { id = handle };
        }

    }

    public sealed class Uniform<T, U, V, W>:Uniform
        where T : struct
        where U : struct
        where V : struct
        where W : struct
    {


        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(ShaderProgram program, string name, Form binding)
        {
            this.binding = binding.graphicContextID;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal unsafe Uniform(
            ShaderProgram program, string name)
        {
            this.binding = (IntPtr)GL.api;
            id = GL.GetUniformLocation(program.handle, name);
        }

        internal Uniform()
        {

        }

        internal static Uniform<T, U, V, W> FromHandle(glSHandle handle)
        {
            return new Uniform<T, U, V, W>() { id = handle };
        }

    }

    internal static partial class GL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetValue(Uniform<Matrix44> uniform, Matrix44 mat)
        {
            Matrix44 temp = mat;
            api->glUniformMatrix4fv(uniform.id.Value, 1, false, (float*)&mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetValue(Uniform<float, float, float, float> uniform, vec4d vec)
        {
            vec4d temp = vec;
            api->glUniform1fv(uniform.id.Value , 4, &temp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetValue(Uniform<vec4d> uniform, vec4d vec)
        {
            vec4d temp = vec;
            api->glUniform1fv(uniform.id.Value , 4, &temp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetValue(Uniform<float> uniform, float v0)
        {
            api->glUniform1f(uniform.id.Value , v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetValue(Uniform<int> uniform, int i0)
        {
            api->glUniform1i(uniform.id.Value , i0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetValue(Uniform<float, float> uniform, float v0, float v1)
        {
            api->glUniform2f(uniform.id.Value , v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetValue(Uniform<float, float, float> uniform, float v0, float v1, float v2)
        {
            api->glUniform3f(uniform.id.Value , v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetValue(Uniform<float, float, float, float> uniform, float v0, float v1, float v2, float v3)
        {
            api->glUniform4f(uniform.id.Value , v0, v1, v2, v3);
        }

    }

}