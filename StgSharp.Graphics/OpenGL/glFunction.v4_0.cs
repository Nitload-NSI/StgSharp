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
            _context->glBeginQueryIndexed(target, index, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTransformFeedback(
                    uint target,
                    uint id
        )
        {
            _context->glBindTransformFeedback(target, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquationi(
                    uint buf,
                    uint mode
        )
        {
            _context->glBlendEquationi(buf, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquationSeparatei(
                    uint buf,
                    uint modeRGB,
                    uint modeAlpha
        )
        {
            _context->glBlendEquationSeparatei(buf, modeRGB, modeAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFunci(
                    uint buf,
                    uint src,
                    uint dst
        )
        {
            _context->glBlendFunci(buf, src, dst);
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
            _context->glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteTransformFeedbacks(
                    int n,
                    ReadOnlySpan<uint> ids
        )
        {
            fixed (uint* idsPtr = ids) {
                _context->glDeleteTransformFeedbacks(n, idsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArraysIndirect(
                    uint mode,
                    ReadOnlySpan<IntPtr> indirect
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                _context->glDrawArraysIndirect(mode, indirectPtr);
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
                _context->glDrawElementsIndirect(mode, (uint)type, indirectPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTransformFeedback(
                    uint mode,
                    uint id
        )
        {
            _context->glDrawTransformFeedback(mode, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTransformFeedbackStream(
                    uint mode,
                    uint id,
                    uint stream
        )
        {
            _context->glDrawTransformFeedbackStream(mode, id, stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndQueryIndexed(
                    uint target,
                    uint index
        )
        {
            _context->glEndQueryIndexed(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenTransformFeedbacks(
                    int n,
                    uint* ids
        )
        {
            _context->glGenTransformFeedbacks(n, ids);
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
            _context->glGetActiveSubroutineName(program.Value, (uint)shaderType, index, bufSize,
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
            _context->glGetActiveSubroutineUniformiv(program.Value, (uint)shaderType, index, pName,
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
            _context->glGetActiveSubroutineUniformName(program.Value, (uint)shaderType, index,
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
            _context->glGetProgramStageiv(program.Value, (uint)shaderType, pName, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryIndexediv(
                    uint target,
                    uint index,
                    uint pName,
                    int* @params
        )
        {
            _context->glGetQueryIndexediv(target, index, pName, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetSubroutineIndex(
                    GlHandle program,
                    ShaderType shaderType,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetSubroutineIndex(program.Value, (uint)shaderType, namePtr);
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
                return _context->glGetSubroutineUniformLocation(program.Value, (uint)shaderType, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformdv(
                    GlHandle program,
                    int location,
                    double* @params
        )
        {
            _context->glGetUniformdv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformSubroutineuiv(
                    ShaderType shaderType,
                    int location,
                    uint* @params
        )
        {
            _context->glGetUniformSubroutineuiv((uint)shaderType, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsTransformFeedback(
                    uint id
        )
        {
            return _context->glIsTransformFeedback(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MinSampleShading(
                    float value
        )
        {
            _context->glMinSampleShading(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatchParameterfv(
                    uint pName,
                    ReadOnlySpan<float> values
        )
        {
            fixed (float* valuesPtr = values) {
                _context->glPatchParameterfv(pName, valuesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatchParameteri(
                    uint pName,
                    int value
        )
        {
            _context->glPatchParameteri(pName, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseTransformFeedback()
        {
            _context->glPauseTransformFeedback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResumeTransformFeedback()
        {
            _context->glResumeTransformFeedback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1d(
                    int location,
                    double x
        )
        {
            _context->glUniform1d(location, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                _context->glUniform1dv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2d(
                    int location,
                    double x,
                    double y
        )
        {
            _context->glUniform2d(location, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                _context->glUniform2dv(location, count, valuePtr);
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
            _context->glUniform3d(location, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                _context->glUniform3dv(location, count, valuePtr);
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
            _context->glUniform4d(location, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4dv(
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                _context->glUniform4dv(location, count, valuePtr);
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
                _context->glUniformMatrix2dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix2x3dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix2x4dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix3dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix3x2dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix3x4dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix4dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix4x2dv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix4x3dv(location, count, transpose, valuePtr);
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
                _context->glUniformSubroutinesuiv((uint)shaderType, count, indicesPtr);
            }
        }

    }
}
