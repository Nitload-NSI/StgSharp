//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v4_1"
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
        public void ActiveShaderProgram(
                    uint pipeline,
                    GlHandle program
        )
        {
            Context.glActiveShaderProgram(pipeline, program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindProgramPipeline(
                    uint pipeline
        )
        {
            Context.glBindProgramPipeline(pipeline);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearDepthf(
                    float d
        )
        {
            Context.glClearDepthf(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CreateShaderProgramv(
                    ShaderType type,
                    int count,
                    ReadOnlySpan<byte> strings
        )
        {
            fixed (byte* stringsPtr = strings) {
                return Context.glCreateShaderProgramv((uint)type, count, stringsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteProgramPipelines(
                    int n,
                    ReadOnlySpan<uint> pipelines
        )
        {
            fixed (uint* pipelinesPtr = pipelines) {
                Context.glDeleteProgramPipelines(n, pipelinesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthRangeArrayv(
                    uint first,
                    int count,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glDepthRangeArrayv(first, count, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthRangef(
                    float n,
                    float f
        )
        {
            Context.glDepthRangef(n, f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthRangeIndexed(
                    uint index,
                    double n,
                    double f
        )
        {
            Context.glDepthRangeIndexed(index, n, f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenProgramPipelines(
                    int n,
                    uint* pipelines
        )
        {
            Context.glGenProgramPipelines(n, pipelines);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetDoublei_v(
                    uint target,
                    uint index,
                    double* data
        )
        {
            Context.glGetDoublei_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetFloati_v(
                    uint target,
                    uint index,
                    float* data
        )
        {
            Context.glGetFloati_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramBinary(
                    GlHandle program,
                    int bufSize,
                    int* length,
                    uint* binaryFormat,
                    IntPtr* binary
        )
        {
            Context.glGetProgramBinary(program.Value, bufSize, length, binaryFormat, binary);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramPipelineInfoLog(
                    uint pipeline,
                    int bufSize,
                    int* length,
                    byte* infoLog
        )
        {
            Context.glGetProgramPipelineInfoLog(pipeline, bufSize, length, infoLog);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramPipelineiv(
                    uint pipeline,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetProgramPipelineiv(pipeline, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderPrecisionFormat(
                    ShaderType shaderType,
                    uint precisiontype,
                    int* range,
                    int* precision
        )
        {
            Context.glGetShaderPrecisionFormat((uint)shaderType, precisiontype, range, precision);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribLdv(
                    uint index,
                    uint pname,
                    double* @params
        )
        {
            Context.glGetVertexAttribLdv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsProgramPipeline(
                    uint pipeline
        )
        {
            return Context.glIsProgramPipeline(pipeline);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramBinary(
                    GlHandle program,
                    uint binaryFormat,
                    ReadOnlySpan<IntPtr> binary,
                    int length
        )
        {
            fixed (IntPtr* binaryPtr = binary) {
                Context.glProgramBinary(program.Value, binaryFormat, binaryPtr, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramParameteri(
                    GlHandle program,
                    uint pname,
                    int value
        )
        {
            Context.glProgramParameteri(program.Value, pname, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1d(
                    GlHandle program,
                    int location,
                    double v0
        )
        {
            Context.glProgramUniform1d(program.Value, location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1dv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniform1dv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1f(
                    GlHandle program,
                    int location,
                    float v0
        )
        {
            Context.glProgramUniform1f(program.Value, location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1fv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniform1fv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1i(
                    GlHandle program,
                    int location,
                    int v0
        )
        {
            Context.glProgramUniform1i(program.Value, location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1iv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glProgramUniform1iv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1ui(
                    GlHandle program,
                    int location,
                    uint v0
        )
        {
            Context.glProgramUniform1ui(program.Value, location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform1uiv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glProgramUniform1uiv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2d(
                    GlHandle program,
                    int location,
                    double v0,
                    double v1
        )
        {
            Context.glProgramUniform2d(program.Value, location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2dv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniform2dv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2f(
                    GlHandle program,
                    int location,
                    float v0,
                    float v1
        )
        {
            Context.glProgramUniform2f(program.Value, location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2fv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniform2fv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2i(
                    GlHandle program,
                    int location,
                    int v0,
                    int v1
        )
        {
            Context.glProgramUniform2i(program.Value, location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2iv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glProgramUniform2iv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2ui(
                    GlHandle program,
                    int location,
                    uint v0,
                    uint v1
        )
        {
            Context.glProgramUniform2ui(program.Value, location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform2uiv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glProgramUniform2uiv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3d(
                    GlHandle program,
                    int location,
                    double v0,
                    double v1,
                    double v2
        )
        {
            Context.glProgramUniform3d(program.Value, location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3dv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniform3dv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3f(
                    GlHandle program,
                    int location,
                    float v0,
                    float v1,
                    float v2
        )
        {
            Context.glProgramUniform3f(program.Value, location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3fv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniform3fv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3i(
                    GlHandle program,
                    int location,
                    int v0,
                    int v1,
                    int v2
        )
        {
            Context.glProgramUniform3i(program.Value, location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3iv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glProgramUniform3iv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3ui(
                    GlHandle program,
                    int location,
                    uint v0,
                    uint v1,
                    uint v2
        )
        {
            Context.glProgramUniform3ui(program.Value, location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform3uiv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glProgramUniform3uiv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4d(
                    GlHandle program,
                    int location,
                    double v0,
                    double v1,
                    double v2,
                    double v3
        )
        {
            Context.glProgramUniform4d(program.Value, location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4dv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniform4dv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4f(
                    GlHandle program,
                    int location,
                    float v0,
                    float v1,
                    float v2,
                    float v3
        )
        {
            Context.glProgramUniform4f(program.Value, location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4fv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniform4fv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4i(
                    GlHandle program,
                    int location,
                    int v0,
                    int v1,
                    int v2,
                    int v3
        )
        {
            Context.glProgramUniform4i(program.Value, location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4iv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glProgramUniform4iv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4ui(
                    GlHandle program,
                    int location,
                    uint v0,
                    uint v1,
                    uint v2,
                    uint v3
        )
        {
            Context.glProgramUniform4ui(program.Value, location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniform4uiv(
                    GlHandle program,
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glProgramUniform4uiv(program.Value, location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix2dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix2dv(program.Value, location, count, transpose,
                                                  valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix2fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix2fv(program.Value, location, count, transpose,
                                                  valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix2x3dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix2x3dv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix2x3fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix2x3fv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix2x4dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix2x4dv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix2x4fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix2x4fv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix3dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix3dv(program.Value, location, count, transpose,
                                                  valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix3fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix3fv(program.Value, location, count, transpose,
                                                  valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix3x2dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix3x2dv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix3x2fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix3x2fv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix3x4dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix3x4dv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix3x4fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix3x4fv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix4dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix4dv(program.Value, location, count, transpose,
                                                  valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix4fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix4fv(program.Value, location, count, transpose,
                                                  valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix4x2dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix4x2dv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix4x2fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix4x2fv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix4x3dv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<double> value
        )
        {
            fixed (double* valuePtr = value) {
                Context.glProgramUniformMatrix4x3dv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProgramUniformMatrix4x3fv(
                    GlHandle program,
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glProgramUniformMatrix4x3fv(program.Value, location, count, transpose,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseShaderCompiler()
        {
            Context.glReleaseShaderCompiler();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScissorArrayv(
                    uint first,
                    int count,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glScissorArrayv(first, count, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScissorIndexed(
                    uint index,
                    int left,
                    int bottom,
                    int width,
                    int height
        )
        {
            Context.glScissorIndexed(index, left, bottom, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScissorIndexedv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glScissorIndexedv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShaderBinary(
                    int count,
                    ReadOnlySpan<uint> shaders,
                    uint binaryFormat,
                    ReadOnlySpan<IntPtr> binary,
                    int length
        )
        {
            fixed (uint* shadersPtr = shaders)
            {
                fixed (IntPtr* binaryPtr = binary) {
                    Context.glShaderBinary(count, shadersPtr, binaryFormat, binaryPtr, length);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UseProgramStages(
                    uint pipeline,
                    uint stages,
                    GlHandle program
        )
        {
            Context.glUseProgramStages(pipeline, stages, program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ValidateProgramPipeline(
                    uint pipeline
        )
        {
            Context.glValidateProgramPipeline(pipeline);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL1d(
                    uint index,
                    double x
        )
        {
            Context.glVertexAttribL1d(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL1dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttribL1dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL2d(
                    uint index,
                    double x,
                    double y
        )
        {
            Context.glVertexAttribL2d(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL2dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttribL2dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL3d(
                    uint index,
                    double x,
                    double y,
                    double z
        )
        {
            Context.glVertexAttribL3d(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL3dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttribL3dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL4d(
                    uint index,
                    double x,
                    double y,
                    double z,
                    double w
        )
        {
            Context.glVertexAttribL4d(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribL4dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttribL4dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribLPointer(
                    uint index,
                    int size,
                    ShaderType type,
                    int stride,
                    ReadOnlySpan<IntPtr> pointer
        )
        {
            fixed (IntPtr* pointerPtr = pointer) {
                Context.glVertexAttribLPointer(index, size, (uint)type, stride, pointerPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ViewportArrayv(
                    uint first,
                    int count,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                Context.glViewportArrayv(first, count, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ViewportIndexedf(
                    uint index,
                    float x,
                    float y,
                    float w,
                    float h
        )
        {
            Context.glViewportIndexedf(index, x, y, w, h);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ViewportIndexedfv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                Context.glViewportIndexedfv(index, vPtr);
            }
        }

    }
}
