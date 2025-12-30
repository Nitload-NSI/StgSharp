//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v4_0"
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
using StgSharp.Graphics.OpenGL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginQueryIndexed(
                    uint target,
                    uint index,
                    uint id
        )
        {
            Context.glBeginQueryIndexed(target, index, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTransformFeedback(
                    uint target,
                    uint id
        )
        {
            Context.glBindTransformFeedback(target, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquationi(
                    uint buf,
                    uint mode
        )
        {
            Context.glBlendEquationi(buf, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquationSeparatei(
                    uint buf,
                    uint modeRGB,
                    uint modeAlpha
        )
        {
            Context.glBlendEquationSeparatei(buf, modeRGB, modeAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFunci(
                    uint buf,
                    uint src,
                    uint dst
        )
        {
            Context.glBlendFunci(buf, src, dst);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFuncSeparatei(
                    uint buf,
                    uint srcRGB,
                    uint dstRGB,
                    uint srcAlpha,
                    uint dstAlpha
        )
        {
            Context.glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteTransformFeedbacks(
                    int n,
                    ReadOnlySpan<uint> ids
        )
        {
            fixed (uint* idsPtr = ids) {
                Context.glDeleteTransformFeedbacks(n, idsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArraysIndirect(
                    uint mode,
                    ReadOnlySpan<IntPtr> indirect
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                Context.glDrawArraysIndirect(mode, indirectPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsIndirect(
                    uint mode,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indirect
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                Context.glDrawElementsIndirect(mode, (uint)type, indirectPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTransformFeedback(
                    uint mode,
                    uint id
        )
        {
            Context.glDrawTransformFeedback(mode, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTransformFeedbackStream(
                    uint mode,
                    uint id,
                    uint stream
        )
        {
            Context.glDrawTransformFeedbackStream(mode, id, stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndQueryIndexed(
                    uint target,
                    uint index
        )
        {
            Context.glEndQueryIndexed(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenTransformFeedbacks(
                    int n,
                    uint* ids
        )
        {
            Context.glGenTransformFeedbacks(n, ids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveSubroutineName(
                    GlHandle program,
                    ShaderType shaderType,
                    uint index,
                    int bufSize,
                    int* length,
                    byte* name
        )
        {
            Context.glGetActiveSubroutineName(program.Value, (uint)shaderType, index, bufSize,
                                              length, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveSubroutineUniformiv(
                    GlHandle program,
                    ShaderType shaderType,
                    uint index,
                    uint pName,
                    int* values
        )
        {
            Context.glGetActiveSubroutineUniformiv(program.Value, (uint)shaderType, index, pName,
                                                   values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveSubroutineUniformName(
                    GlHandle program,
                    ShaderType shaderType,
                    uint index,
                    int bufSize,
                    int* length,
                    byte* name
        )
        {
            Context.glGetActiveSubroutineUniformName(program.Value, (uint)shaderType, index,
                                                     bufSize, length, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramStageiv(
                    GlHandle program,
                    ShaderType shaderType,
                    uint pName,
                    int* values
        )
        {
            Context.glGetProgramStageiv(program.Value, (uint)shaderType, pName, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryIndexediv(
                    uint target,
                    uint index,
                    uint pName,
                    int* @params
        )
        {
            Context.glGetQueryIndexediv(target, index, pName, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetSubroutineIndex(
                    GlHandle program,
                    ShaderType shaderType,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return Context.glGetSubroutineIndex(program.Value, (uint)shaderType, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetSubroutineUniformLocation(
                   GlHandle program,
                   ShaderType shaderType,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return Context.glGetSubroutineUniformLocation(program.Value, (uint)shaderType, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformdv(
                    GlHandle program,
                    int location,
                    double* @params
        )
        {
            Context.glGetUniformdv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformSubroutineuiv(
                    ShaderType shaderType,
                    int location,
                    uint* @params
        )
        {
            Context.glGetUniformSubroutineuiv((uint)shaderType, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsTransformFeedback(
                    uint id
        )
        {
            return Context.glIsTransformFeedback(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MinSampleShading(
                    float value
        )
        {
            Context.glMinSampleShading(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatchParameterfv(
                    uint pName,
                    ReadOnlySpan<float> values
        )
        {
            fixed (float* valuesPtr = values) {
                Context.glPatchParameterfv(pName, valuesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatchParameteri(
                    uint pName,
                    int value
        )
        {
            Context.glPatchParameteri(pName, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseTransformFeedback()
        {
            Context.glPauseTransformFeedback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResumeTransformFeedback()
        {
            Context.glResumeTransformFeedback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1d(
                    int location,
                    double x
        )
        {
            Context.glUniform1d(location, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniform1dv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2d(
                    int location,
                    double x,
                    double y
        )
        {
            Context.glUniform2d(location, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniform2dv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3d(
                    int location,
                    double x,
                    double y,
                    double z
        )
        {
            Context.glUniform3d(location, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniform3dv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4d(
                    int location,
                    double x,
                    double y,
                    double z,
                    double w
        )
        {
            Context.glUniform4d(location, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniform4dv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix2dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix2dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix2x3dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix2x3dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix2x4dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix2x4dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix3dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix3dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix3x2dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix3x2dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix3x4dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix3x4dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix4dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix4dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix4x2dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix4x2dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix4x3dv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glUniformMatrix4x3dv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformSubroutinesuiv(
                    ShaderType shaderType,
                    int count,
                    ReadOnlySpan<uint> indices
        )
        {
            fixed (uint* indicesPtr = indices) {
                Context.glUniformSubroutinesuiv((uint)shaderType, count, indicesPtr);
            }
        }

    }
}
