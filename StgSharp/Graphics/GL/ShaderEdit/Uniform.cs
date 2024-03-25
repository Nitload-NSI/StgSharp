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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public class Uniform<T> where T : struct
    {

        private Form binding;
        internal glSHandle id;

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(
            ShaderProgram program, string name, Form binding)
        {
            this.binding = binding;
            id = binding.GL.GetUniformLocation(program.handle, name);
        }

    }

    public class Uniform<T, U>
        where T : struct
        where U : struct
    {

        private Form binding;
        internal glSHandle id;

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(
            ShaderProgram program, string name, Form binding)
        {
            this.binding = binding;
            id = binding.GL.GetUniformLocation(program.handle, name);
        }

    }

    public class Uniform<T, U, V>
        where T : struct
        where U : struct
        where V : struct
    {

        private Form binding;
        internal glSHandle id;

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        public unsafe Uniform(ShaderProgram program, string name, Form binding)
        {
            this.binding = binding;
            id = binding.GL.GetUniformLocation(program.handle, name);
        }

    }

    public class Uniform<T, U, V, W>
        where T : struct
        where U : struct
        where V : struct
        where W : struct
    {

        private Form binding;
        internal glSHandle id;

        internal unsafe Uniform(glSHandle id)
        {
            this.id = id;
        }

        internal unsafe Uniform(ShaderProgram program, string name, Form binding)
        {
            this.binding = binding;
            id = binding.GL.GetUniformLocation(program.handle, name);
        }

    }

    internal partial class GLonRender : IglFunc
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetValue(Uniform<float, float, float, float> uniform, Vec4d vec)
        {
            api->glUniform1fv(uniform.id.DirectValue, 4, &vec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetValue(Uniform<Vec4d> uniform, Vec4d vec)
        {
            api->glUniform1fv(uniform.id.DirectValue, 4, &vec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.SetValue(Uniform<float> uniform, float v0)
        {
            api->glUniform1f(uniform.id.DirectValue, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.SetValue(Uniform<int> uniform, int i0)
        {
            api->glUniform1i(uniform.id.DirectValue, i0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.SetValue(Uniform<float, float> uniform, float v0, float v1)
        {
            api->glUniform2f(uniform.id.DirectValue, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.SetValue(Uniform<float, float, float> uniform, float v0, float v1, float v2)
        {
            api->glUniform3f(uniform.id.DirectValue, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe void IglFunc.SetValue(Uniform<float, float, float, float> uniform, float v0, float v1, float v2, float v3)
        {
            api->glUniform4f(uniform.id.DirectValue, v0, v1, v2, v3);
        }

    }

    internal unsafe partial class GLonUpdate
    {

        private IntPtr UniformVal1fPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&UniformVal1f;
        private IntPtr UniformVal1fvPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&UniformVal1fv;
        private IntPtr UniformVal1iPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&UniformVal1i;
        private IntPtr UniformVal2fPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&UniformVal2f;
        private IntPtr UniformVal3fPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&UniformVal3f;
        private IntPtr UniformVal4fPtr = (IntPtr)(delegate*<FrameOperationBuffer, GLcontext*, void>)&UniformVal4f;

        private static void UniformVal1f(FrameOperationBuffer frameOpBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameOpBuffer.TryDequeue(out m);
            activatedContext->glUniform1f(*(int*)m.Read<ulong>(0), m.Read<uint>(2));
        }

        private static unsafe void UniformVal1fv(FrameOperationBuffer frameOpBuffer, GLcontext* activatedContext)
        {
            M128 m = default, v = default;
            frameOpBuffer.TryDequeue(out m);
            frameOpBuffer.TryDequeue(out v);
            activatedContext->glUniform1fv(*(int*)m.Read<ulong>(0), 4, (Vec4d*)&v);
        }

        private static void UniformVal1i(FrameOperationBuffer frameOpBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameOpBuffer.TryDequeue(out m);
            activatedContext->glUniform1i(*(int*)m.Read<ulong>(0), m.Read<int>(2));
        }

        private static void UniformVal2f(FrameOperationBuffer frameOpBuffer, GLcontext* activatedContext)
        {
            M128 m = default;
            frameOpBuffer.TryDequeue(out m);
            activatedContext->glUniform2f(*(int*)m.Read<ulong>(0), m.Read<float>(2), m.Read<float>(3));
        }

        private static void UniformVal3f(FrameOperationBuffer frameOpBuffer, GLcontext* activatedContext)
        {
            IntPtr handle = IntPtr.Zero;
            M128 m = default;
            frameOpBuffer.TryDequeue(out handle);
            frameOpBuffer.TryDequeue(out m);
            activatedContext->glUniform3f(*(int*)handle, m.Read<float>(0), m.Read<float>(1), m.Read<float>(2));
        }

        private static void UniformVal4f(FrameOperationBuffer frameOpBuffer, GLcontext* activatedContext)
        {
            IntPtr handle = IntPtr.Zero;
            M128 m = default;
            frameOpBuffer.TryDequeue(out handle);
            frameOpBuffer.TryDequeue(out m);
            activatedContext->glUniform4f(*(int*)handle, m.Read<float>(0), m.Read<float>(1), m.Read<float>(2), m.Read<float>(3));
        }

        unsafe void IglFunc.SetValue(Uniform<float, float, float> uniform, float f0, float f1, float f2)
        {
            frameOpBuffer.ParamEnqueue((IntPtr)uniform.id.valuePtr);
            frameOpBuffer.ParamEnqueue(f0, f1, f2);
            frameOpBuffer.MethodEnqueue(UniformVal3fPtr);
        }

        unsafe void IglFunc.SetValue(Uniform<float, float> uniform, float f0, float f1)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)uniform.id.valuePtr);
            m.Write<float>(2, f0);
            m.Write<float>(3, f1);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(UniformVal2fPtr);
        }

        unsafe void IglFunc.SetValue(Uniform<float, float, float, float> uniform, float f0, float f1, float f2, float f3)
        {
            frameOpBuffer.ParamEnqueue((IntPtr)uniform.id.valuePtr);
            frameOpBuffer.ParamEnqueue(f0, f1, f2, f3);
            frameOpBuffer.MethodEnqueue(UniformVal4fPtr);
        }

        unsafe void IglFunc.SetValue(Uniform<float, float, float, float> uniform, Vec4d vec)
        {
            frameOpBuffer.ParamEnqueue((IntPtr)uniform.id.valuePtr);
            frameOpBuffer.ParamEnqueue(vec);
            frameOpBuffer.MethodEnqueue(UniformVal1fvPtr);
        }

        unsafe void IglFunc.SetValue(Uniform<float> uniform, float f0)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)uniform.id.valuePtr);
            m.Write<float>(2, f0);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(UniformVal1fPtr);
        }

        unsafe void IglFunc.SetValue(Uniform<int> uniform, int i0)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)uniform.id.valuePtr);
            m.Write<int>(2, i0);
            frameOpBuffer.ParamEnqueue(m);
            frameOpBuffer.MethodEnqueue(UniformVal1fPtr);
        }

        unsafe void IglFunc.SetValue(Uniform<Vec4d> uniform, Vec4d vec)
        {
            frameOpBuffer.ParamEnqueue((IntPtr)uniform.id.valuePtr);
            frameOpBuffer.ParamEnqueue(vec);
            frameOpBuffer.MethodEnqueue(UniformVal1fvPtr);
        }

    }
}