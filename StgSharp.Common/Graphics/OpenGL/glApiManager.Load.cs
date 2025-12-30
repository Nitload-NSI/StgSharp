//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glApiManager.Load"
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
using StgSharp.HighPerformance;
using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public static partial class glManager
    {

        internal static unsafe bool LoadGLcore10(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core10)
            {
                DefaultLog.InternalAppendLog("The core version 1.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBlendFunc = LoadGLFunction(load, "glBlendFunc"u8);
            contextPtr->glClear = LoadGLFunction(load, "glClear"u8);
            contextPtr->glClearColor = LoadGLFunction(load, "glClearColor"u8);
            contextPtr->glClearDepth = LoadGLFunction(load, "glClearDepth"u8);
            contextPtr->glClearStencil = LoadGLFunction(load, "glClearStencil"u8);
            contextPtr->glColorMask = LoadGLFunction(load, "glColorMask"u8);
            contextPtr->glCullFace = LoadGLFunction(load, "glCullFace"u8);
            contextPtr->glDepthFunc = LoadGLFunction(load, "glDepthFunc"u8);
            contextPtr->glDepthMask = LoadGLFunction(load, "glDepthMask"u8);
            contextPtr->glDepthRange = LoadGLFunction(load, "glDepthRange"u8);
            contextPtr->glDisable = LoadGLFunction(load, "glDisable"u8);
            contextPtr->glDrawBuffer = LoadGLFunction(load, "glDrawBuffer"u8);
            contextPtr->glEnable = LoadGLFunction(load, "glEnable"u8);
            contextPtr->glFinish = LoadGLFunction(load, "glFinish"u8);
            contextPtr->glFlush = LoadGLFunction(load, "glFlush"u8);
            contextPtr->glFrontFace = LoadGLFunction(load, "glFrontFace"u8);
            contextPtr->glGetBooleanv = LoadGLFunction(load, "glGetBooleanv"u8);
            contextPtr->glGetDoublev = LoadGLFunction(load, "glGetDoublev"u8);
            contextPtr->glGetError = LoadGLFunction(load, "glGetError"u8);
            contextPtr->glGetFloatv = LoadGLFunction(load, "glGetFloatv"u8);
            contextPtr->glGetIntegerv = LoadGLFunction(load, "glGetIntegerv"u8);
            contextPtr->glGetString = LoadGLFunction(load, "glGetString"u8);
            contextPtr->glGetTexImage = LoadGLFunction(load, "glGetTexImage"u8);
            contextPtr->glGetTexLevelParameterfv = LoadGLFunction(load, "glGetTexLevelParameterfv"u8);
            contextPtr->glGetTexLevelParameteriv = LoadGLFunction(load, "glGetTexLevelParameteriv"u8);
            contextPtr->glGetTexParameterfv = LoadGLFunction(load, "glGetTexParameterfv"u8);
            contextPtr->glGetTexParameteriv = LoadGLFunction(load, "glGetTexParameteriv"u8);
            contextPtr->glHint = LoadGLFunction(load, "glHint"u8);
            contextPtr->glIsEnabled = LoadGLFunction(load, "glIsEnabled"u8);
            contextPtr->glLineWidth = LoadGLFunction(load, "glLineWidth"u8);
            contextPtr->glLogicOp = LoadGLFunction(load, "glLogicOp"u8);
            contextPtr->glPixelStoref = LoadGLFunction(load, "glPixelStoref"u8);
            contextPtr->glPixelStorei = LoadGLFunction(load, "glPixelStorei"u8);
            contextPtr->glPointSize = LoadGLFunction(load, "glPointSize"u8);
            contextPtr->glPolygonMode = LoadGLFunction(load, "glPolygonMode"u8);
            contextPtr->glReadBuffer = LoadGLFunction(load, "glReadBuffer"u8);
            contextPtr->glReadPixels = LoadGLFunction(load, "glReadPixels"u8);
            contextPtr->glScissor = LoadGLFunction(load, "glScissor"u8);
            contextPtr->glStencilFunc = LoadGLFunction(load, "glStencilFunc"u8);
            contextPtr->glStencilMask = LoadGLFunction(load, "glStencilMask"u8);
            contextPtr->glStencilOp = LoadGLFunction(load, "glStencilOp"u8);
            contextPtr->glTexImage1D = LoadGLFunction(load, "glTexImage1D"u8);
            contextPtr->glTexImage2D = LoadGLFunction(load, "glTexImage2D"u8);
            contextPtr->glTexParameterf = LoadGLFunction(load, "glTexParameterf"u8);
            contextPtr->glTexParameterfv = LoadGLFunction(load, "glTexParameterfv"u8);
            contextPtr->glTexParameteri = LoadGLFunction(load, "glTexParameteri"u8);
            contextPtr->glTexParameteriv = LoadGLFunction(load, "glTexParameteriv"u8);
            contextPtr->glViewport = LoadGLFunction(load, "glViewport"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 1.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore11(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core11)
            {
                DefaultLog.InternalAppendLog("The core version 1.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindTexture = LoadGLFunction(load, "glBindTexture"u8);
            contextPtr->glCopyTexImage1D = LoadGLFunction(load,
                "glCopyTexImage1D"u8);
            contextPtr->glCopyTexImage2D = LoadGLFunction(load,
                "glCopyTexImage2D"u8);
            contextPtr->glCopyTexSubImage1D = LoadGLFunction(load,
                "glCopyTexSubImage1D"u8);
            contextPtr->glCopyTexSubImage2D = LoadGLFunction(load,
                "glCopyTexSubImage2D"u8);
            contextPtr->glDeleteTextures = LoadGLFunction(load,
                "glDeleteTextures"u8);
            contextPtr->glDrawArrays = LoadGLFunction(load, "glDrawArrays"u8);
            contextPtr->glDrawElements = LoadGLFunction(load,
                "glDrawElements"u8);
            contextPtr->glGenTextures = LoadGLFunction(load, "glGenTextures"u8);

            // contextPtr->glGetPointerv =LoadGLFunction(load,"glGetPointerv"u8);
            contextPtr->glIsTexture = LoadGLFunction(load, "glIsTexture"u8);
            contextPtr->glPolygonOffset = LoadGLFunction(load, "glPolygonOffset"u8);
            contextPtr->glTexSubImage1D = LoadGLFunction(load,
                "glTexSubImage1D"u8);
            contextPtr->glTexSubImage2D = LoadGLFunction(load,
                "glTexSubImage2D"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 1.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore12(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core12)
            {
                DefaultLog.InternalAppendLog("The core version 1.2 is not supported.");
                return false;
            }

            #region load

            contextPtr->glCopyTexSubImage3D = LoadGLFunction(load,
                "glCopyTexSubImage3D"u8);
            contextPtr->glDrawRangeElements = LoadGLFunction(load,
                "glDrawRangeElements"u8);
            contextPtr->glTexImage3D = LoadGLFunction(load,
                "glTexImage3D"u8);
            contextPtr->glTexSubImage3D = LoadGLFunction(load,
                "glTexSubImage3D"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 1.2 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore13(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core13)
            {
                DefaultLog.InternalAppendLog("The core version 1.3 is not supported.");
                return false;
            }

            #region load

            contextPtr->glActiveTexture = LoadGLFunction(load, "glActiveTexture"u8);
            contextPtr->glCompressedTexImage1D = LoadGLFunction(load,
                "glCompressedTexImage1D"u8);
            contextPtr->glCompressedTexImage2D = LoadGLFunction(load,
                "glCompressedTexImage2D"u8);
            contextPtr->glCompressedTexImage3D = LoadGLFunction(load,
                "glCompressedTexImage3D"u8);
            contextPtr->glCompressedTexSubImage1D = LoadGLFunction(load,
                "glCompressedTexSubImage1D"u8);
            contextPtr->glCompressedTexSubImage2D = LoadGLFunction(load,
                "glCompressedTexSubImage2D"u8);
            contextPtr->glCompressedTexSubImage3D = LoadGLFunction(load,
                "glCompressedTexSubImage3D"u8);
            contextPtr->glGetCompressedTexImage = LoadGLFunction(load,
                "glGetCompressedTexImage"u8);
            contextPtr->glSampleCoverage = LoadGLFunction(load,
                "glSampleCoverage"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 1.3 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore14(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core14)
            {
                DefaultLog.InternalAppendLog("The core version 1.4 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBlendColor = LoadGLFunction(load,
                "glBlendColor"u8);
            contextPtr->glBlendEquation = LoadGLFunction(load, "glBlendEquation"u8);
            contextPtr->glBlendFuncSeparate = LoadGLFunction(load,
                "glBlendFuncSeparate"u8);
            contextPtr->glMultiDrawArrays = LoadGLFunction(load,
                "glMultiDrawArrays"u8);
            contextPtr->glMultiDrawElements = LoadGLFunction(load,
                "glMultiDrawElements"u8);
            contextPtr->glPointParameterf = LoadGLFunction(load,
                "glPointParameterf"u8);
            contextPtr->glPointParameterfv = LoadGLFunction(load,
                "glPointParameterfv"u8);
            contextPtr->glPointParameteri = LoadGLFunction(load,
                "glPointParameteri"u8);
            contextPtr->glPointParameteriv = LoadGLFunction(load,
                "glPointParameteriv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 1.4 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore15(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core15)
            {
                DefaultLog.InternalAppendLog("The core version 1.5 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBeginQuery = LoadGLFunction(load, "glBeginQuery"u8);
            contextPtr->glBindBuffer = LoadGLFunction(load, "glBindBuffer"u8);
            contextPtr->glBufferData = LoadGLFunction(load,
                "glBufferData"u8);
            contextPtr->glBufferSubData = LoadGLFunction(load,
                "glBufferSubData"u8);
            contextPtr->glDeleteBuffers = LoadGLFunction(load, "glDeleteBuffers"u8);
            contextPtr->glDeleteQueries = LoadGLFunction(load, "glDeleteQueries"u8);
            contextPtr->glEndQuery = LoadGLFunction(load, "glEndQuery"u8);
            contextPtr->glGenBuffers = LoadGLFunction(load, "glGenBuffers"u8);
            contextPtr->glGenQueries = LoadGLFunction(load, "glGenQueries"u8);
            contextPtr->glGetBufferParameteriv = LoadGLFunction(load,
                "glGetBufferParameteriv"u8);
            contextPtr->glGetBufferPointerv = LoadGLFunction(load,
                "glGetBufferPointerv"u8);
            contextPtr->glGetBufferSubData = LoadGLFunction(load,
                "glGetBufferSubData"u8);
            contextPtr->glGetQueryObjectiv = LoadGLFunction(load,
                "glGetQueryObjectiv"u8);
            contextPtr->glGetQueryObjectuiv = LoadGLFunction(load,
                "glGetQueryObjectuiv"u8);
            contextPtr->glGetQueryiv = LoadGLFunction(load, "glGetQueryiv"u8);
            contextPtr->glIsBuffer = LoadGLFunction(load, "glIsBuffer"u8);
            contextPtr->glIsQuery = LoadGLFunction(load, "glIsQuery"u8);
            contextPtr->glMapBuffer = LoadGLFunction(load, "glMapBuffer"u8);
            contextPtr->glUnmapBuffer = LoadGLFunction(load, "glUnmapBuffer"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 1.5 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore20(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core20)
            {
                DefaultLog.InternalAppendLog("The core version 2.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glAttachShader = LoadGLFunction(load, "glAttachShader"u8);
            contextPtr->glBindAttribLocation = LoadGLFunction(load,
                "glBindAttribLocation"u8);
            contextPtr->glBlendEquationSeparate = LoadGLFunction(load,
                "glBlendEquationSeparate"u8);
            contextPtr->glCompileShader = LoadGLFunction(load, "glCompileShader"u8);
            contextPtr->glCreateProgram = LoadGLFunction(load, "glCreateProgram"u8);
            contextPtr->glCreateShader = LoadGLFunction(load, "glCreateShader"u8);
            contextPtr->glDeleteProgram = LoadGLFunction(load, "glDeleteProgram"u8);
            contextPtr->glDeleteShader = LoadGLFunction(load, "glDeleteShader"u8);
            contextPtr->glDetachShader = LoadGLFunction(load, "glDetachShader"u8);
            contextPtr->glDisableVertexAttribArray = LoadGLFunction(load,
                "glDisableVertexAttribArray"u8);
            contextPtr->glDrawBuffers = LoadGLFunction(load, "glDrawBuffers"u8);
            contextPtr->glEnableVertexAttribArray = LoadGLFunction(load,
                "glEnableVertexAttribArray"u8);
            contextPtr->glGetActiveAttrib = LoadGLFunction(load,
                "glGetActiveAttrib"u8);
            contextPtr->glGetActiveUniform = LoadGLFunction(load,
                "glGetActiveUniform"u8);
            contextPtr->glGetAttachedShaders = LoadGLFunction(load,
                "glGetAttachedShaders"u8);
            contextPtr->glGetAttribLocation = LoadGLFunction(load,
                "glGetAttribLocation"u8);
            contextPtr->glGetProgramInfoLog = LoadGLFunction(load,
                "glGetProgramInfoLog"u8);
            contextPtr->glGetProgramiv = LoadGLFunction(load,
                "glGetProgramiv"u8);
            contextPtr->glGetShaderInfoLog = LoadGLFunction(load,
                "glGetShaderInfoLog"u8);
            contextPtr->glGetShaderSource = LoadGLFunction(load,
                "glGetShaderSource"u8);
            contextPtr->glGetShaderiv = LoadGLFunction(load, "glGetShaderiv"u8);
            contextPtr->glGetUniformLocation = LoadGLFunction(load,
                "glGetUniformLocation"u8);
            contextPtr->glGetUniformfv = LoadGLFunction(load,
                "glGetUniformfv"u8);
            contextPtr->glGetUniformiv = LoadGLFunction(load,
                "glGetUniformiv"u8);
            contextPtr->glGetVertexAttribPointerv = LoadGLFunction(load,
                "glGetVertexAttribPointerv"u8);
            contextPtr->glGetVertexAttribdv = LoadGLFunction(load,
                "glGetVertexAttribdv"u8);
            contextPtr->glGetVertexAttribfv = LoadGLFunction(load,
                "glGetVertexAttribfv"u8);
            contextPtr->glGetVertexAttribiv = LoadGLFunction(load,
                "glGetVertexAttribiv"u8);
            contextPtr->glIsProgram = LoadGLFunction(load, "glIsProgram"u8);
            contextPtr->glIsShader = LoadGLFunction(load, "glIsShader"u8);
            contextPtr->glLinkProgram = LoadGLFunction(load, "glLinkProgram"u8);
            contextPtr->glShaderSource = LoadGLFunction(load,
                "glShaderSource"u8);
            contextPtr->glStencilFuncSeparate = LoadGLFunction(load,
                "glStencilFuncSeparate"u8);
            contextPtr->glStencilMaskSeparate = LoadGLFunction(load,
                "glStencilMaskSeparate"u8);
            contextPtr->glStencilOpSeparate = LoadGLFunction(load,
                "glStencilOpSeparate"u8);
            contextPtr->glUniform1f = LoadGLFunction(load, "glUniform1f"u8);
            contextPtr->glUniform1fv = LoadGLFunction(load, "glUniform1fv"u8);
            contextPtr->glUniform1i = LoadGLFunction(load, "glUniform1i"u8);
            contextPtr->glUniform1iv = LoadGLFunction(load, "glUniform1iv"u8);
            contextPtr->glUniform2f = LoadGLFunction(load, "glUniform2f"u8);
            contextPtr->glUniform2fv = LoadGLFunction(load, "glUniform2fv"u8);
            contextPtr->glUniform2i = LoadGLFunction(load, "glUniform2i"u8);
            contextPtr->glUniform2iv = LoadGLFunction(load, "glUniform2iv"u8);
            contextPtr->glUniform3f = LoadGLFunction(load,
                "glUniform3f"u8);
            contextPtr->glUniform3fv = LoadGLFunction(load, "glUniform3fv"u8);
            contextPtr->glUniform3i = LoadGLFunction(load, "glUniform3i"u8);
            contextPtr->glUniform3iv = LoadGLFunction(load, "glUniform3iv"u8);
            contextPtr->glUniform4f = LoadGLFunction(load,
                "glUniform4f"u8);
            contextPtr->glUniform4fv = LoadGLFunction(load, "glUniform4fv"u8);
            contextPtr->glUniform4i = LoadGLFunction(load,
                "glUniform4i"u8);
            contextPtr->glUniform4iv = LoadGLFunction(load, "glUniform4iv"u8);
            contextPtr->glUniformMatrix2fv = LoadGLFunction(load,
                "glUniformMatrix2fv"u8);
            contextPtr->glUniformMatrix3fv = LoadGLFunction(load,
                "glUniformMatrix3fv"u8);
            contextPtr->glUniformMatrix4fv = LoadGLFunction(load,
                "glUniformMatrix4fv"u8);
            contextPtr->glUseProgram = LoadGLFunction(load, "glUseProgram"u8);
            contextPtr->glValidateProgram = LoadGLFunction(load, "glValidateProgram"u8);
            contextPtr->glVertexAttrib1d = LoadGLFunction(load,
                "glVertexAttrib1d"u8);
            contextPtr->glVertexAttrib1dv = LoadGLFunction(load,
                "glVertexAttrib1dv"u8);
            contextPtr->glVertexAttrib1f = LoadGLFunction(load,
                "glVertexAttrib1f"u8);
            contextPtr->glVertexAttrib1fv = LoadGLFunction(load,
                "glVertexAttrib1fv"u8);
            contextPtr->glVertexAttrib1s = LoadGLFunction(load,
                "glVertexAttrib1s"u8);
            contextPtr->glVertexAttrib1sv = LoadGLFunction(load,
                "glVertexAttrib1sv"u8);
            contextPtr->glVertexAttrib2d = LoadGLFunction(load,
                "glVertexAttrib2d"u8);
            contextPtr->glVertexAttrib2dv = LoadGLFunction(load,
                "glVertexAttrib2dv"u8);
            contextPtr->glVertexAttrib2f = LoadGLFunction(load,
                "glVertexAttrib2f"u8);
            contextPtr->glVertexAttrib2fv = LoadGLFunction(load,
                "glVertexAttrib2fv"u8);
            contextPtr->glVertexAttrib2s = LoadGLFunction(load,
                "glVertexAttrib2s"u8);
            contextPtr->glVertexAttrib2sv = LoadGLFunction(load,
                "glVertexAttrib2sv"u8);
            contextPtr->glVertexAttrib3d = LoadGLFunction(load,
                "glVertexAttrib3d"u8);
            contextPtr->glVertexAttrib3dv = LoadGLFunction(load,
                "glVertexAttrib3dv"u8);
            contextPtr->glVertexAttrib3f = LoadGLFunction(load,
                "glVertexAttrib3f"u8);
            contextPtr->glVertexAttrib3fv = LoadGLFunction(load,
                "glVertexAttrib3fv"u8);
            contextPtr->glVertexAttrib3s = LoadGLFunction(load,
                "glVertexAttrib3s"u8);
            contextPtr->glVertexAttrib3sv = LoadGLFunction(load,
                "glVertexAttrib3sv"u8);
            contextPtr->glVertexAttrib4Nbv = LoadGLFunction(load,
                "glVertexAttrib4Nbv"u8);
            contextPtr->glVertexAttrib4Niv = LoadGLFunction(load,
                "glVertexAttrib4Niv"u8);
            contextPtr->glVertexAttrib4Nsv = LoadGLFunction(load,
                "glVertexAttrib4Nsv"u8);
            contextPtr->glVertexAttrib4Nub = LoadGLFunction(load,
                "glVertexAttrib4Nub"u8);
            contextPtr->glVertexAttrib4Nubv = LoadGLFunction(load,
                "glVertexAttrib4Nubv"u8);
            contextPtr->glVertexAttrib4Nuiv = LoadGLFunction(load,
                "glVertexAttrib4Nuiv"u8);
            contextPtr->glVertexAttrib4Nusv = LoadGLFunction(load,
                "glVertexAttrib4Nusv"u8);
            contextPtr->glVertexAttrib4bv = LoadGLFunction(load,
                "glVertexAttrib4bv"u8);
            contextPtr->glVertexAttrib4d = LoadGLFunction(load,
                "glVertexAttrib4d"u8);
            contextPtr->glVertexAttrib4dv = LoadGLFunction(load,
                "glVertexAttrib4dv"u8);
            contextPtr->glVertexAttrib4f = LoadGLFunction(load,
                "glVertexAttrib4f"u8);
            contextPtr->glVertexAttrib4fv = LoadGLFunction(load,
                "glVertexAttrib4fv"u8);
            contextPtr->glVertexAttrib4iv = LoadGLFunction(load,
                "glVertexAttrib4iv"u8);
            contextPtr->glVertexAttrib4s = LoadGLFunction(load,
                "glVertexAttrib4s"u8);
            contextPtr->glVertexAttrib4sv = LoadGLFunction(load,
                "glVertexAttrib4sv"u8);
            contextPtr->glVertexAttrib4ubv = LoadGLFunction(load,
                "glVertexAttrib4ubv"u8);
            contextPtr->glVertexAttrib4uiv = LoadGLFunction(load,
                "glVertexAttrib4uiv"u8);
            contextPtr->glVertexAttrib4usv = LoadGLFunction(load,
                "glVertexAttrib4usv"u8);
            contextPtr->glVertexAttribPointer = LoadGLFunction(load,
                "glVertexAttribPointer"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 2.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore21(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core21)
            {
                DefaultLog.InternalAppendLog("The core version 2.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glUniformMatrix2x3fv = LoadGLFunction(load,
                "glUniformMatrix2x3fv"u8);
            contextPtr->glUniformMatrix2x4fv = LoadGLFunction(load,
                "glUniformMatrix2x4fv"u8);
            contextPtr->glUniformMatrix3x2fv = LoadGLFunction(load,
                "glUniformMatrix3x2fv"u8);
            contextPtr->glUniformMatrix3x4fv = LoadGLFunction(load,
                "glUniformMatrix3x4fv"u8);
            contextPtr->glUniformMatrix4x2fv = LoadGLFunction(load,
                "glUniformMatrix4x2fv"u8);
            contextPtr->glUniformMatrix4x3fv = LoadGLFunction(load,
                "glUniformMatrix4x3fv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 2.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore30(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core30)
            {
                DefaultLog.InternalAppendLog("The core version 3.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBeginConditionalRender = LoadGLFunction(load,
                "glBeginConditionalRender"u8);
            contextPtr->glBeginTransformFeedback = LoadGLFunction(load,
                "glBeginTransformFeedback"u8);
            contextPtr->glBindBufferBase = LoadGLFunction(load,
                "glBindBufferBase"u8);
            contextPtr->glBindBufferRange = LoadGLFunction(load,
                "glBindBufferRange"u8);
            contextPtr->glBindFragDataLocation = LoadGLFunction(load,
                "glBindFragDataLocation"u8);
            contextPtr->glBindFramebuffer = LoadGLFunction(load,
                "glBindFramebuffer"u8);
            contextPtr->glBindRenderbuffer = LoadGLFunction(load,
                "glBindRenderbuffer"u8);
            contextPtr->glBindVertexArray = LoadGLFunction(load, "glBindVertexArray"u8);
            contextPtr->glBlitFramebuffer = LoadGLFunction(load,
                "glBlitFramebuffer"u8);
            contextPtr->glCheckFramebufferStatus = LoadGLFunction(load,
                "glCheckFramebufferStatus"u8);
            contextPtr->glClampColor = LoadGLFunction(load, "glClampColor"u8);
            contextPtr->glClearBufferfi = LoadGLFunction(load,
                "glClearBufferfi"u8);
            contextPtr->glClearBufferfv = LoadGLFunction(load,
                "glClearBufferfv"u8);
            contextPtr->glClearBufferiv = LoadGLFunction(load,
                "glClearBufferiv"u8);
            contextPtr->glClearBufferuiv = LoadGLFunction(load,
                "glClearBufferuiv"u8);
            contextPtr->glColorMaski = LoadGLFunction(load,
                "glColorMaski"u8);
            contextPtr->glDeleteFramebuffers = LoadGLFunction(load,
                "glDeleteFramebuffers"u8);
            contextPtr->glDeleteRenderbuffers = LoadGLFunction(load,
                "glDeleteRenderbuffers"u8);
            contextPtr->glDeleteVertexArrays = LoadGLFunction(load,
                "glDeleteVertexArrays"u8);
            contextPtr->glDisablei = LoadGLFunction(load, "glDisablei"u8);
            contextPtr->glEnablei = LoadGLFunction(load, "glEnablei"u8);
            contextPtr->glEndConditionalRender = LoadGLFunction(load, "glEndConditionalRender"u8);
            contextPtr->glEndTransformFeedback = LoadGLFunction(load, "glEndTransformFeedback"u8);
            contextPtr->glFlushMappedBufferRange = LoadGLFunction(load,
                "glFlushMappedBufferRange"u8);
            contextPtr->glFramebufferRenderbuffer = LoadGLFunction(load,
                "glFramebufferRenderbuffer"u8);
            contextPtr->glFramebufferTexture1D = LoadGLFunction(load,
                "glFramebufferTexture1D"u8);
            contextPtr->glFramebufferTexture2D = LoadGLFunction(load,
                "glFramebufferTexture2D"u8);
            contextPtr->glFramebufferTexture3D = LoadGLFunction(load,
                "glFramebufferTexture3D"u8);
            contextPtr->glFramebufferTextureLayer = LoadGLFunction(load,
                "glFramebufferTextureLayer"u8);
            contextPtr->glGenFramebuffers = LoadGLFunction(load,
                "glGenFramebuffers"u8);
            contextPtr->glGenRenderbuffers = LoadGLFunction(load,
                "glGenRenderbuffers"u8);
            contextPtr->glGenVertexArrays = LoadGLFunction(load,
                "glGenVertexArrays"u8);
            contextPtr->glGenerateMipmap = LoadGLFunction(load, "glGenerateMipmap"u8);
            contextPtr->glGetBooleani_v = LoadGLFunction(load,
                "glGetBooleani_v"u8);
            contextPtr->glGetFragDataLocation = LoadGLFunction(load,
                "glGetFragDataLocation"u8);
            contextPtr->glGetFramebufferAttachmentParameteriv = LoadGLFunction(load,
                "glGetFramebufferAttachmentParameteriv"u8);
            contextPtr->glGetIntegeri_v = LoadGLFunction(load,
                "glGetIntegeri_v"u8);
            contextPtr->glGetRenderbufferParameteriv = LoadGLFunction(load,
                "glGetRenderbufferParameteriv"u8);
            contextPtr->glGetStringi = LoadGLFunction(load, "glGetStringi"u8);
            contextPtr->glGetTexParameterIiv = LoadGLFunction(load,
                "glGetTexParameterIiv"u8);
            contextPtr->glGetTexParameterIuiv = LoadGLFunction(load,
                "glGetTexParameterIuiv"u8);
            contextPtr->glGetTransformFeedbackVarying = LoadGLFunction(load,
                "glGetTransformFeedbackVarying"u8);
            contextPtr->glGetUniformuiv = LoadGLFunction(load,
                "glGetUniformuiv"u8);
            contextPtr->glGetVertexAttribIiv = LoadGLFunction(load,
                "glGetVertexAttribIiv"u8);
            contextPtr->glGetVertexAttribIuiv = LoadGLFunction(load,
                "glGetVertexAttribIuiv"u8);
            contextPtr->glIsEnabledi = LoadGLFunction(load, "glIsEnabledi"u8);
            contextPtr->glIsFramebuffer = LoadGLFunction(load, "glIsFramebuffer"u8);
            contextPtr->glIsRenderbuffer = LoadGLFunction(load, "glIsRenderbuffer"u8);
            contextPtr->glIsVertexArray = LoadGLFunction(load, "glIsVertexArray"u8);
            contextPtr->glMapBufferRange = LoadGLFunction(load,
                "glMapBufferRange"u8);
            contextPtr->glRenderbufferStorage = LoadGLFunction(load,
                "glRenderbufferStorage"u8);
            contextPtr->glRenderbufferStorageMultisample = LoadGLFunction(load,
                "glRenderbufferStorageMultisample"u8);
            contextPtr->glTexParameterIiv = LoadGLFunction(load,
                "glTexParameterIiv"u8);
            contextPtr->glTexParameterIuiv = LoadGLFunction(load,
                "glTexParameterIuiv"u8);
            contextPtr->glTransformFeedbackVaryings = LoadGLFunction(load,
                "glTransformFeedbackVaryings"u8);
            contextPtr->glUniform1ui = LoadGLFunction(load, "glUniform1ui"u8);
            contextPtr->glUniform1uiv = LoadGLFunction(load, "glUniform1uiv"u8);
            contextPtr->glUniform2ui = LoadGLFunction(load, "glUniform2ui"u8);
            contextPtr->glUniform2uiv = LoadGLFunction(load, "glUniform2uiv"u8);
            contextPtr->glUniform3ui = LoadGLFunction(load,
                "glUniform3ui"u8);
            contextPtr->glUniform3uiv = LoadGLFunction(load, "glUniform3uiv"u8);
            contextPtr->glUniform4ui = LoadGLFunction(load,
                "glUniform4ui"u8);
            contextPtr->glUniform4uiv = LoadGLFunction(load, "glUniform4uiv"u8);
            contextPtr->glVertexAttribI1i = LoadGLFunction(load,
                "glVertexAttribI1i"u8);
            contextPtr->glVertexAttribI1iv = LoadGLFunction(load,
                "glVertexAttribI1iv"u8);
            contextPtr->glVertexAttribI1ui = LoadGLFunction(load,
                "glVertexAttribI1ui"u8);
            contextPtr->glVertexAttribI1uiv = LoadGLFunction(load,
                "glVertexAttribI1uiv"u8);
            contextPtr->glVertexAttribI2i = LoadGLFunction(load,
                "glVertexAttribI2i"u8);
            contextPtr->glVertexAttribI2iv = LoadGLFunction(load,
                "glVertexAttribI2iv"u8);
            contextPtr->glVertexAttribI2ui = LoadGLFunction(load,
                "glVertexAttribI2ui"u8);
            contextPtr->glVertexAttribI2uiv = LoadGLFunction(load,
                "glVertexAttribI2uiv"u8);
            contextPtr->glVertexAttribI3i = LoadGLFunction(load,
                "glVertexAttribI3i"u8);
            contextPtr->glVertexAttribI3iv = LoadGLFunction(load,
                "glVertexAttribI3iv"u8);
            contextPtr->glVertexAttribI3ui = LoadGLFunction(load,
                "glVertexAttribI3ui"u8);
            contextPtr->glVertexAttribI3uiv = LoadGLFunction(load,
                "glVertexAttribI3uiv"u8);
            contextPtr->glVertexAttribI4bv = LoadGLFunction(load,
                "glVertexAttribI4bv"u8);
            contextPtr->glVertexAttribI4i = LoadGLFunction(load,
                "glVertexAttribI4i"u8);
            contextPtr->glVertexAttribI4iv = LoadGLFunction(load,
                "glVertexAttribI4iv"u8);
            contextPtr->glVertexAttribI4sv = LoadGLFunction(load,
                "glVertexAttribI4sv"u8);
            contextPtr->glVertexAttribI4ubv = LoadGLFunction(load,
                "glVertexAttribI4ubv"u8);
            contextPtr->glVertexAttribI4ui = LoadGLFunction(load,
                "glVertexAttribI4ui"u8);
            contextPtr->glVertexAttribI4uiv = LoadGLFunction(load,
                "glVertexAttribI4uiv"u8);
            contextPtr->glVertexAttribI4usv = LoadGLFunction(load,
                "glVertexAttribI4usv"u8);
            contextPtr->glVertexAttribIPointer = LoadGLFunction(load,
                "glVertexAttribIPointer"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 3.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore31(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core31)
            {
                DefaultLog.InternalAppendLog("The core version 3.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindBufferBase = LoadGLFunction(load,
                "glBindBufferBase"u8);
            contextPtr->glBindBufferRange = LoadGLFunction(load,
                "glBindBufferRange"u8);
            contextPtr->glCopyBufferSubData = LoadGLFunction(load,
                "glCopyBufferSubData"u8);
            contextPtr->glDrawArraysInstanced = LoadGLFunction(load,
                "glDrawArraysInstanced"u8);
            contextPtr->glDrawElementsInstanced = LoadGLFunction(load,
                "glDrawElementsInstanced"u8);
            contextPtr->glGetActiveUniformBlockName = LoadGLFunction(load,
                "glGetActiveUniformBlockName"u8);
            contextPtr->glGetActiveUniformBlockiv = LoadGLFunction(load,
                "glGetActiveUniformBlockiv"u8);
            contextPtr->glGetActiveUniformName = LoadGLFunction(load,
                "glGetActiveUniformName"u8);
            contextPtr->glGetActiveUniformsiv = LoadGLFunction(load,
                "glGetActiveUniformsiv"u8);
            contextPtr->glGetIntegeri_v = LoadGLFunction(load,
                "glGetIntegeri_v"u8);
            contextPtr->glGetUniformBlockIndex = LoadGLFunction(load,
                "glGetUniformBlockIndex"u8);
            contextPtr->glGetUniformIndices = LoadGLFunction(load,
                "glGetUniformIndices"u8);
            contextPtr->glPrimitiveRestartIndex = LoadGLFunction(load,
                "glPrimitiveRestartIndex"u8);
            contextPtr->glTexBuffer = LoadGLFunction(load, "glTexBuffer"u8);
            contextPtr->glUniformBlockBinding = LoadGLFunction(load,
                "glUniformBlockBinding"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 3.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore32(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core32)
            {
                DefaultLog.InternalAppendLog("The core version 3.2 is not supported.");
                return false;
            }

            #region load

            contextPtr->glClientWaitSync = LoadGLFunction(load,
                "glClientWaitSync"u8);
            contextPtr->glDeleteSync = LoadGLFunction(load, "glDeleteSync"u8);
            contextPtr->glDrawElementsBaseVertex = LoadGLFunction(load,
                "glDrawElementsBaseVertex"u8);
            contextPtr->glDrawElementsInstancedBaseVertex = LoadGLFunction(load,
                "glDrawElementsInstancedBaseVertex"u8);
            contextPtr->glDrawRangeElementsBaseVertex = LoadGLFunction(load,
                "glDrawRangeElementsBaseVertex"u8);
            contextPtr->glFenceSync = LoadGLFunction(load, "glFenceSync"u8);
            contextPtr->glFramebufferTexture = LoadGLFunction(load,
                "glFramebufferTexture"u8);
            contextPtr->glGetBufferParameteri64v = LoadGLFunction(load,
                "glGetBufferParameteri64v"u8);
            contextPtr->glGetInteger64i_v = LoadGLFunction(load,
                "glGetInteger64i_v"u8);
            contextPtr->glGetInteger64v = LoadGLFunction(load, "glGetInteger64v"u8);
            contextPtr->glGetMultisamplefv = LoadGLFunction(load,
                "glGetMultisamplefv"u8);
            contextPtr->glGetSynciv = LoadGLFunction(load,
                "glGetSynciv"u8);
            contextPtr->glIsSync = LoadGLFunction(load, "glIsSync"u8);
            contextPtr->glMultiDrawElementsBaseVertex = LoadGLFunction(load,
                "glMultiDrawElementsBaseVertex"u8);
            contextPtr->glProvokingVertex = LoadGLFunction(load, "glProvokingVertex"u8);
            contextPtr->glSampleMaski = LoadGLFunction(load, "glSampleMaski"u8);
            contextPtr->glTexImage2DMultisample = LoadGLFunction(load,
                "glTexImage2DMultisample"u8);
            contextPtr->glTexImage3DMultisample = LoadGLFunction(load,
                "glTexImage3DMultisample"u8);
            contextPtr->glWaitSync = LoadGLFunction(load, "glWaitSync"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 3.2 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore33(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core33)
            {
                DefaultLog.InternalAppendLog("The core version 3.3 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindFragDataLocationIndexed = LoadGLFunction(load,
                "glBindFragDataLocationIndexed"u8);
            contextPtr->glBindSampler = LoadGLFunction(load, "glBindSampler"u8);
            contextPtr->glDeleteSamplers = LoadGLFunction(load,
                "glDeleteSamplers"u8);
            contextPtr->glGenSamplers = LoadGLFunction(load, "glGenSamplers"u8);
            contextPtr->glGetFragDataIndex = LoadGLFunction(load,
                "glGetFragDataIndex"u8);
            contextPtr->glGetQueryObjecti64v = LoadGLFunction(load,
                "glGetQueryObjecti64v"u8);
            contextPtr->glGetQueryObjectui64v = LoadGLFunction(load,
                "glGetQueryObjectui64v"u8);
            contextPtr->glGetSamplerParameterIiv = LoadGLFunction(load,
                "glGetSamplerParameterIiv"u8);
            contextPtr->glGetSamplerParameterIuiv = LoadGLFunction(load,
                "glGetSamplerParameterIuiv"u8);
            contextPtr->glGetSamplerParameterfv = LoadGLFunction(load,
                "glGetSamplerParameterfv"u8);
            contextPtr->glGetSamplerParameteriv = LoadGLFunction(load,
                "glGetSamplerParameteriv"u8);
            contextPtr->glIsSampler = LoadGLFunction(load, "glIsSampler"u8);
            contextPtr->glQueryCounter = LoadGLFunction(load, "glQueryCounter"u8);
            contextPtr->glSamplerParameterIiv = LoadGLFunction(load,
                "glSamplerParameterIiv"u8);
            contextPtr->glSamplerParameterIuiv = LoadGLFunction(load,
                "glSamplerParameterIuiv"u8);
            contextPtr->glSamplerParameterf = LoadGLFunction(load,
                "glSamplerParameterf"u8);
            contextPtr->glSamplerParameterfv = LoadGLFunction(load,
                "glSamplerParameterfv"u8);
            contextPtr->glSamplerParameteri = LoadGLFunction(load,
                "glSamplerParameteri"u8);
            contextPtr->glSamplerParameteriv = LoadGLFunction(load,
                "glSamplerParameteriv"u8);
            contextPtr->glVertexAttribDivisor = LoadGLFunction(load,
                "glVertexAttribDivisor"u8);
            contextPtr->glVertexAttribP1ui = LoadGLFunction(load,
                "glVertexAttribP1ui"u8);
            contextPtr->glVertexAttribP1uiv = LoadGLFunction(load,
                "glVertexAttribP1uiv"u8);
            contextPtr->glVertexAttribP2ui = LoadGLFunction(load,
                "glVertexAttribP2ui"u8);
            contextPtr->glVertexAttribP2uiv = LoadGLFunction(load,
                "glVertexAttribP2uiv"u8);
            contextPtr->glVertexAttribP3ui = LoadGLFunction(load,
                "glVertexAttribP3ui"u8);
            contextPtr->glVertexAttribP3uiv = LoadGLFunction(load,
                "glVertexAttribP3uiv"u8);
            contextPtr->glVertexAttribP4ui = LoadGLFunction(load,
                "glVertexAttribP4ui"u8);
            contextPtr->glVertexAttribP4uiv = LoadGLFunction(load,
                "glVertexAttribP4uiv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 3.3 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore40(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core40)
            {
                DefaultLog.InternalAppendLog("The core version 4.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBeginQueryIndexed = LoadGLFunction(load,
                "glBeginQueryIndexed"u8);
            contextPtr->glBindTransformFeedback = LoadGLFunction(load,
                "glBindTransformFeedback"u8);
            contextPtr->glBlendEquationSeparatei = LoadGLFunction(load,
                "glBlendEquationSeparatei"u8);
            contextPtr->glBlendEquationi = LoadGLFunction(load, "glBlendEquationi"u8);
            contextPtr->glBlendFuncSeparatei = LoadGLFunction(load,
                "glBlendFuncSeparatei"u8);
            contextPtr->glBlendFunci = LoadGLFunction(load, "glBlendFunci"u8);
            contextPtr->glDeleteTransformFeedbacks = LoadGLFunction(load,
                "glDeleteTransformFeedbacks"u8);
            contextPtr->glDrawArraysIndirect = LoadGLFunction(load,
                "glDrawArraysIndirect"u8);
            contextPtr->glDrawElementsIndirect = LoadGLFunction(load,
                "glDrawElementsIndirect"u8);
            contextPtr->glDrawTransformFeedback = LoadGLFunction(load,
                "glDrawTransformFeedback"u8);
            contextPtr->glDrawTransformFeedbackStream = LoadGLFunction(load,
                "glDrawTransformFeedbackStream"u8);
            contextPtr->glEndQueryIndexed = LoadGLFunction(load,
                "glEndQueryIndexed"u8);
            contextPtr->glGenTransformFeedbacks = LoadGLFunction(load,
                "glGenTransformFeedbacks"u8);
            contextPtr->glGetActiveSubroutineName = LoadGLFunction(load,
                "glGetActiveSubroutineName"u8);
            contextPtr->glGetActiveSubroutineUniformName = LoadGLFunction(load,
                "glGetActiveSubroutineUniformName"u8);
            contextPtr->glGetActiveSubroutineUniformiv = LoadGLFunction(load,
                "glGetActiveSubroutineUniformiv"u8);
            contextPtr->glGetProgramStageiv = LoadGLFunction(load,
                "glGetProgramStageiv"u8);
            contextPtr->glGetQueryIndexediv = LoadGLFunction(load,
                "glGetQueryIndexediv"u8);
            contextPtr->glGetSubroutineIndex = LoadGLFunction(load,
                "glGetSubroutineIndex"u8);
            contextPtr->glGetSubroutineUniformLocation = LoadGLFunction(load,
                "glGetSubroutineUniformLocation"u8);
            contextPtr->glGetUniformSubroutineuiv = LoadGLFunction(load,
                "glGetUniformSubroutineuiv"u8);
            contextPtr->glGetUniformdv = LoadGLFunction(load,
                "glGetUniformdv"u8);
            contextPtr->glIsTransformFeedback = LoadGLFunction(load,
                "glIsTransformFeedback"u8);
            contextPtr->glMinSampleShading = LoadGLFunction(load, "glMinSampleShading"u8);
            contextPtr->glPatchParameterfv = LoadGLFunction(load,
                "glPatchParameterfv"u8);
            contextPtr->glPatchParameteri = LoadGLFunction(load,
                "glPatchParameteri"u8);
            contextPtr->glPauseTransformFeedback = LoadGLFunction(load,
                "glPauseTransformFeedback"u8);
            contextPtr->glResumeTransformFeedback = LoadGLFunction(load,
                "glResumeTransformFeedback"u8);
            contextPtr->glUniform1d = LoadGLFunction(load, "glUniform1d"u8);
            contextPtr->glUniform1dv = LoadGLFunction(load, "glUniform1dv"u8);
            contextPtr->glUniform2d = LoadGLFunction(load, "glUniform2d"u8);
            contextPtr->glUniform2dv = LoadGLFunction(load, "glUniform2dv"u8);
            contextPtr->glUniform3d = LoadGLFunction(load,
                "glUniform3d"u8);
            contextPtr->glUniform3dv = LoadGLFunction(load, "glUniform3dv"u8);
            contextPtr->glUniform4d = LoadGLFunction(load,
                "glUniform4d"u8);
            contextPtr->glUniform4dv = LoadGLFunction(load, "glUniform4dv"u8);
            contextPtr->glUniformMatrix2dv = LoadGLFunction(load,
                "glUniformMatrix2dv"u8);
            contextPtr->glUniformMatrix2x3dv = LoadGLFunction(load,
                "glUniformMatrix2x3dv"u8);
            contextPtr->glUniformMatrix2x4dv = LoadGLFunction(load,
                "glUniformMatrix2x4dv"u8);
            contextPtr->glUniformMatrix3dv = LoadGLFunction(load,
                "glUniformMatrix3dv"u8);
            contextPtr->glUniformMatrix3x2dv = LoadGLFunction(load,
                "glUniformMatrix3x2dv"u8);
            contextPtr->glUniformMatrix3x4dv = LoadGLFunction(load,
                "glUniformMatrix3x4dv"u8);
            contextPtr->glUniformMatrix4dv = LoadGLFunction(load,
                "glUniformMatrix4dv"u8);
            contextPtr->glUniformMatrix4x2dv = LoadGLFunction(load,
                "glUniformMatrix4x2dv"u8);
            contextPtr->glUniformMatrix4x3dv = LoadGLFunction(load,
                "glUniformMatrix4x3dv"u8);
            contextPtr->glUniformSubroutinesuiv = LoadGLFunction(load,
                "glUniformSubroutinesuiv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore41(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core41)
            {
                DefaultLog.InternalAppendLog("The core version 4.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glActiveShaderProgram = LoadGLFunction(load,
                "glActiveShaderProgram"u8);
            contextPtr->glBindProgramPipeline = LoadGLFunction(load,
                "glBindProgramPipeline"u8);
            contextPtr->glClearDepthf = LoadGLFunction(load, "glClearDepthf"u8);
            contextPtr->glCreateShaderProgramv = LoadGLFunction(load,
                "glCreateShaderProgramv"u8);
            contextPtr->glDeleteProgramPipelines = LoadGLFunction(load,
                "glDeleteProgramPipelines"u8);
            contextPtr->glDepthRangeArrayv = LoadGLFunction(load,
                "glDepthRangeArrayv"u8);
            contextPtr->glDepthRangeIndexed = LoadGLFunction(load,
                "glDepthRangeIndexed"u8);
            contextPtr->glDepthRangef = LoadGLFunction(load, "glDepthRangef"u8);
            contextPtr->glGenProgramPipelines = LoadGLFunction(load,
                "glGenProgramPipelines"u8);
            contextPtr->glGetDoublei_v = LoadGLFunction(load,
                "glGetDoublei_v"u8);
            contextPtr->glGetFloati_v = LoadGLFunction(load,
                "glGetFloati_v"u8);
            contextPtr->glGetProgramBinary = LoadGLFunction(load,
                "glGetProgramBinary"u8);
            contextPtr->glGetProgramPipelineInfoLog = LoadGLFunction(load,
                "glGetProgramPipelineInfoLog"u8);
            contextPtr->glGetProgramPipelineiv = LoadGLFunction(load,
                "glGetProgramPipelineiv"u8);
            contextPtr->glGetShaderPrecisionFormat = LoadGLFunction(load,
                "glGetShaderPrecisionFormat"u8);
            contextPtr->glGetVertexAttribLdv = LoadGLFunction(load,
                "glGetVertexAttribLdv"u8);
            contextPtr->glIsProgramPipeline = LoadGLFunction(load, "glIsProgramPipeline"u8);
            contextPtr->glProgramBinary = LoadGLFunction(load,
                "glProgramBinary"u8);
            contextPtr->glProgramParameteri = LoadGLFunction(load,
                "glProgramParameteri"u8);
            contextPtr->glProgramUniform1d = LoadGLFunction(load,
                "glProgramUniform1d"u8);
            contextPtr->glProgramUniform1dv = LoadGLFunction(load,
                "glProgramUniform1dv"u8);
            contextPtr->glProgramUniform1f = LoadGLFunction(load,
                "glProgramUniform1f"u8);
            contextPtr->glProgramUniform1fv = LoadGLFunction(load,
                "glProgramUniform1fv"u8);
            contextPtr->glProgramUniform1i = LoadGLFunction(load,
                "glProgramUniform1i"u8);
            contextPtr->glProgramUniform1iv = LoadGLFunction(load,
                "glProgramUniform1iv"u8);
            contextPtr->glProgramUniform1ui = LoadGLFunction(load,
                "glProgramUniform1ui"u8);
            contextPtr->glProgramUniform1uiv = LoadGLFunction(load,
                "glProgramUniform1uiv"u8);
            contextPtr->glProgramUniform2d = LoadGLFunction(load,
                "glProgramUniform2d"u8);
            contextPtr->glProgramUniform2dv = LoadGLFunction(load,
                "glProgramUniform2dv"u8);
            contextPtr->glProgramUniform2f = LoadGLFunction(load,
                "glProgramUniform2f"u8);
            contextPtr->glProgramUniform2fv = LoadGLFunction(load,
                "glProgramUniform2fv"u8);
            contextPtr->glProgramUniform2i = LoadGLFunction(load,
                "glProgramUniform2i"u8);
            contextPtr->glProgramUniform2iv = LoadGLFunction(load,
                "glProgramUniform2iv"u8);
            contextPtr->glProgramUniform2ui = LoadGLFunction(load,
                "glProgramUniform2ui"u8);
            contextPtr->glProgramUniform2uiv = LoadGLFunction(load,
                "glProgramUniform2uiv"u8);
            contextPtr->glProgramUniform3d = LoadGLFunction(load,
                "glProgramUniform3d"u8);
            contextPtr->glProgramUniform3dv = LoadGLFunction(load,
                "glProgramUniform3dv"u8);
            contextPtr->glProgramUniform3f = LoadGLFunction(load,
                "glProgramUniform3f"u8);
            contextPtr->glProgramUniform3fv = LoadGLFunction(load,
                "glProgramUniform3fv"u8);
            contextPtr->glProgramUniform3i = LoadGLFunction(load,
                "glProgramUniform3i"u8);
            contextPtr->glProgramUniform3iv = LoadGLFunction(load,
                "glProgramUniform3iv"u8);
            contextPtr->glProgramUniform3ui = LoadGLFunction(load,
                "glProgramUniform3ui"u8);
            contextPtr->glProgramUniform3uiv = LoadGLFunction(load,
                "glProgramUniform3uiv"u8);
            contextPtr->glProgramUniform4d = LoadGLFunction(load,
                "glProgramUniform4d"u8);
            contextPtr->glProgramUniform4dv = LoadGLFunction(load,
                "glProgramUniform4dv"u8);
            contextPtr->glProgramUniform4f = LoadGLFunction(load,
                "glProgramUniform4f"u8);
            contextPtr->glProgramUniform4fv = LoadGLFunction(load,
                "glProgramUniform4fv"u8);
            contextPtr->glProgramUniform4i = LoadGLFunction(load,
                "glProgramUniform4i"u8);
            contextPtr->glProgramUniform4iv = LoadGLFunction(load,
                "glProgramUniform4iv"u8);
            contextPtr->glProgramUniform4ui = LoadGLFunction(load,
                "glProgramUniform4ui"u8);
            contextPtr->glProgramUniform4uiv = LoadGLFunction(load,
                "glProgramUniform4uiv"u8);
            contextPtr->glProgramUniformMatrix2dv = LoadGLFunction(load,
                "glProgramUniformMatrix2dv"u8);
            contextPtr->glProgramUniformMatrix2fv = LoadGLFunction(load,
                "glProgramUniformMatrix2fv"u8);
            contextPtr->glProgramUniformMatrix2x3dv = LoadGLFunction(load,
                "glProgramUniformMatrix2x3dv"u8);
            contextPtr->glProgramUniformMatrix2x3fv = LoadGLFunction(load,
                "glProgramUniformMatrix2x3fv"u8);
            contextPtr->glProgramUniformMatrix2x4dv = LoadGLFunction(load,
                "glProgramUniformMatrix2x4dv"u8);
            contextPtr->glProgramUniformMatrix2x4fv = LoadGLFunction(load,
                "glProgramUniformMatrix2x4fv"u8);
            contextPtr->glProgramUniformMatrix3dv = LoadGLFunction(load,
                "glProgramUniformMatrix3dv"u8);
            contextPtr->glProgramUniformMatrix3fv = LoadGLFunction(load,
                "glProgramUniformMatrix3fv"u8);
            contextPtr->glProgramUniformMatrix3x2dv = LoadGLFunction(load,
                "glProgramUniformMatrix3x2dv"u8);
            contextPtr->glProgramUniformMatrix3x2fv = LoadGLFunction(load,
                "glProgramUniformMatrix3x2fv"u8);
            contextPtr->glProgramUniformMatrix3x4dv = LoadGLFunction(load,
                "glProgramUniformMatrix3x4dv"u8);
            contextPtr->glProgramUniformMatrix3x4fv = LoadGLFunction(load,
                "glProgramUniformMatrix3x4fv"u8);
            contextPtr->glProgramUniformMatrix4dv = LoadGLFunction(load,
                "glProgramUniformMatrix4dv"u8);
            contextPtr->glProgramUniformMatrix4fv = LoadGLFunction(load,
                "glProgramUniformMatrix4fv"u8);
            contextPtr->glProgramUniformMatrix4x2dv = LoadGLFunction(load,
                "glProgramUniformMatrix4x2dv"u8);
            contextPtr->glProgramUniformMatrix4x2fv = LoadGLFunction(load,
                "glProgramUniformMatrix4x2fv"u8);
            contextPtr->glProgramUniformMatrix4x3dv = LoadGLFunction(load,
                "glProgramUniformMatrix4x3dv"u8);
            contextPtr->glProgramUniformMatrix4x3fv = LoadGLFunction(load,
                "glProgramUniformMatrix4x3fv"u8);
            contextPtr->glReleaseShaderCompiler = LoadGLFunction(load,
                "glReleaseShaderCompiler"u8);
            contextPtr->glScissorArrayv = LoadGLFunction(load,
                "glScissorArrayv"u8);
            contextPtr->glScissorIndexed = LoadGLFunction(load,
                "glScissorIndexed"u8);
            contextPtr->glScissorIndexedv = LoadGLFunction(load,
                "glScissorIndexedv"u8);
            contextPtr->glShaderBinary = LoadGLFunction(load,
                "glShaderBinary"u8);
            contextPtr->glUseProgramStages = LoadGLFunction(load,
                "glUseProgramStages"u8);
            contextPtr->glValidateProgramPipeline = LoadGLFunction(load,
                "glValidateProgramPipeline"u8);
            contextPtr->glVertexAttribL1d = LoadGLFunction(load,
                "glVertexAttribL1d"u8);
            contextPtr->glVertexAttribL1dv = LoadGLFunction(load,
                "glVertexAttribL1dv"u8);
            contextPtr->glVertexAttribL2d = LoadGLFunction(load,
                "glVertexAttribL2d"u8);
            contextPtr->glVertexAttribL2dv = LoadGLFunction(load,
                "glVertexAttribL2dv"u8);
            contextPtr->glVertexAttribL3d = LoadGLFunction(load,
                "glVertexAttribL3d"u8);
            contextPtr->glVertexAttribL3dv = LoadGLFunction(load,
                "glVertexAttribL3dv"u8);
            contextPtr->glVertexAttribL4d = LoadGLFunction(load,
                "glVertexAttribL4d"u8);
            contextPtr->glVertexAttribL4dv = LoadGLFunction(load,
                "glVertexAttribL4dv"u8);
            contextPtr->glVertexAttribLPointer = LoadGLFunction(load,
                "glVertexAttribLPointer"u8);
            contextPtr->glViewportArrayv = LoadGLFunction(load,
                "glViewportArrayv"u8);
            contextPtr->glViewportIndexedf = LoadGLFunction(load,
                "glViewportIndexedf"u8);
            contextPtr->glViewportIndexedfv = LoadGLFunction(load,
                "glViewportIndexedfv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore42(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core42)
            {
                DefaultLog.InternalAppendLog("The core version 4.2 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindImageTexture = LoadGLFunction(load,
                "glBindImageTexture"u8);
            contextPtr->glDrawArraysInstancedBaseInstance = LoadGLFunction(load,
                "glDrawArraysInstancedBaseInstance"u8);
            contextPtr->glDrawElementsInstancedBaseInstance = LoadGLFunction(load,
                "glDrawElementsInstancedBaseInstance"u8);
            contextPtr->glDrawElementsInstancedBaseVertexBaseInstance = LoadGLFunction(load,
                "glDrawElementsInstancedBaseVertexBaseInstance"u8);
            contextPtr->glDrawTransformFeedbackInstanced = LoadGLFunction(load,
                "glDrawTransformFeedbackInstanced"u8);
            contextPtr->glDrawTransformFeedbackStreamInstanced = LoadGLFunction(load,
                "glDrawTransformFeedbackStreamInstanced"u8);
            contextPtr->glGetActiveAtomicCounterBufferiv = LoadGLFunction(load,
                "glGetActiveAtomicCounterBufferiv"u8);
            contextPtr->glGetInternalformativ = LoadGLFunction(load,
                "glGetInternalformativ"u8);
            contextPtr->glMemoryBarrier = LoadGLFunction(load, "glMemoryBarrier"u8);
            contextPtr->glTexStorage1D = LoadGLFunction(load,
                "glTexStorage1D"u8);
            contextPtr->glTexStorage2D = LoadGLFunction(load,
                "glTexStorage2D"u8);
            contextPtr->glTexStorage3D = LoadGLFunction(load,
                "glTexStorage3D"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.2 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore43(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core43)
            {
                DefaultLog.InternalAppendLog("The core version 4.3 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindVertexBuffer = LoadGLFunction(load,
                "glBindVertexBuffer"u8);
            contextPtr->glClearBufferData = LoadGLFunction(load,
                "glClearBufferData"u8);
            contextPtr->glClearBufferSubData = LoadGLFunction(load,
                "glClearBufferSubData"u8);
            contextPtr->glCopyImageSubData = LoadGLFunction(load,
                "glCopyImageSubData"u8);
            contextPtr->glDebugMessageCallback = LoadGLFunction(load,
                "glDebugMessageCallback"u8);
            contextPtr->glDebugMessageControl = LoadGLFunction(load,
                "glDebugMessageControl"u8);
            contextPtr->glDebugMessageInsert = LoadGLFunction(load,
                "glDebugMessageInsert"u8);
            contextPtr->glDispatchCompute = LoadGLFunction(load,
                "glDispatchCompute"u8);
            contextPtr->glDispatchComputeIndirect = LoadGLFunction(load,
                "glDispatchComputeIndirect"u8);
            contextPtr->glFramebufferParameteri = LoadGLFunction(load,
                "glFramebufferParameteri"u8);
            contextPtr->glGetDebugMessageLog = LoadGLFunction(load,
                "glGetDebugMessageLog"u8);
            contextPtr->glGetFramebufferParameteriv = LoadGLFunction(load,
                "glGetFramebufferParameteriv"u8);
            contextPtr->glGetInternalformati64v = LoadGLFunction(load,
                "glGetInternalformati64v"u8);
            contextPtr->glGetObjectLabel = LoadGLFunction(load,
                "glGetObjectLabel"u8);
            contextPtr->glGetObjectPtrLabel = LoadGLFunction(load,
                "glGetObjectPtrLabel"u8);

            // contextPtr->glGetPointerv =LoadGLFunction(load,"glGetPointerv"u8);
            contextPtr->glGetProgramInterfaceiv = LoadGLFunction(load,
                "glGetProgramInterfaceiv"u8);
            contextPtr->glGetProgramResourceIndex = LoadGLFunction(load,
                "glGetProgramResourceIndex"u8);
            contextPtr->glGetProgramResourceLocation = LoadGLFunction(load,
                "glGetProgramResourceLocation"u8);
            contextPtr->glGetProgramResourceLocationIndex = LoadGLFunction(load,
                "glGetProgramResourceLocationIndex"u8);
            contextPtr->glGetProgramResourceName = LoadGLFunction(load,
                "glGetProgramResourceName"u8);
            contextPtr->glGetProgramResourceiv = LoadGLFunction(load,
                "glGetProgramResourceiv"u8);
            contextPtr->glInvalidateBufferData = LoadGLFunction(load,
                "glInvalidateBufferData"u8);
            contextPtr->glInvalidateBufferSubData = LoadGLFunction(load,
                "glInvalidateBufferSubData"u8);
            contextPtr->glInvalidateFramebuffer = LoadGLFunction(load,
                "glInvalidateFramebuffer"u8);
            contextPtr->glInvalidateSubFramebuffer = LoadGLFunction(load,
                "glInvalidateSubFramebuffer"u8);
            contextPtr->glInvalidateTexImage = LoadGLFunction(load,
                "glInvalidateTexImage"u8);
            contextPtr->glInvalidateTexSubImage = LoadGLFunction(load,
                "glInvalidateTexSubImage"u8);
            contextPtr->glMultiDrawArraysIndirect = LoadGLFunction(load,
                "glMultiDrawArraysIndirect"u8);
            contextPtr->glMultiDrawElementsIndirect = LoadGLFunction(load,
                "glMultiDrawElementsIndirect"u8);
            contextPtr->glObjectLabel = LoadGLFunction(load,
                "glObjectLabel"u8);
            contextPtr->glObjectPtrLabel = LoadGLFunction(load,
                "glObjectPtrLabel"u8);
            contextPtr->glPopDebugGroup = LoadGLFunction(load, "glPopDebugGroup"u8);
            contextPtr->glPushDebugGroup = LoadGLFunction(load,
                "glPushDebugGroup"u8);
            contextPtr->glShaderStorageBlockBinding = LoadGLFunction(load,
                "glShaderStorageBlockBinding"u8);
            contextPtr->glTexBufferRange = LoadGLFunction(load,
                "glTexBufferRange"u8);
            contextPtr->glTexStorage2DMultisample = LoadGLFunction(load,
                "glTexStorage2DMultisample"u8);
            contextPtr->glTexStorage3DMultisample = LoadGLFunction(load,
                "glTexStorage3DMultisample"u8);
            contextPtr->glTextureView = LoadGLFunction(load,
                "glTextureView"u8);
            contextPtr->glVertexAttribBinding = LoadGLFunction(load,
                "glVertexAttribBinding"u8);
            contextPtr->glVertexAttribFormat = LoadGLFunction(load,
                "glVertexAttribFormat"u8);
            contextPtr->glVertexAttribIFormat = LoadGLFunction(load,
                "glVertexAttribIFormat"u8);
            contextPtr->glClearBufferData = LoadGLFunction(load,
                "glClearBufferData"u8);
            contextPtr->glVertexBindingDivisor = LoadGLFunction(load,
                "glVertexBindingDivisor"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.3 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore44(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core44)
            {
                DefaultLog.InternalAppendLog("The core version 4.4 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindBuffersBase = LoadGLFunction(load,
                "glBindBuffersBase"u8);
            contextPtr->glBindBuffersRange = LoadGLFunction(load,
                "glBindBuffersRange"u8);
            contextPtr->glBindImageTextures = LoadGLFunction(load,
                "glBindImageTextures"u8);
            contextPtr->glBindSamplers = LoadGLFunction(load,
                "glBindSamplers"u8);
            contextPtr->glBindTextures = LoadGLFunction(load,
                "glBindTextures"u8);
            contextPtr->glBindVertexBuffers = LoadGLFunction(load,
                "glBindVertexBuffers"u8);
            contextPtr->glBufferStorage = LoadGLFunction(load,
                "glBufferStorage"u8);
            contextPtr->glClearTexImage = LoadGLFunction(load,
                "glClearTexImage"u8);
            contextPtr->glClearTexSubImage = LoadGLFunction(load,
                "glClearTexSubImage"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.4 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore45(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core45)
            {
                DefaultLog.InternalAppendLog("The core version 4.5 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindTextureUnit = LoadGLFunction(load,
                "glBindTextureUnit"u8);
            contextPtr->glBlitNamedFramebuffer = LoadGLFunction(load,
                "glBlitNamedFramebuffer"u8);
            contextPtr->glCheckNamedFramebufferStatus = LoadGLFunction(load,
                "glCheckNamedFramebufferStatus"u8);
            contextPtr->glClearNamedBufferData = LoadGLFunction(load,
                "glClearNamedBufferData"u8);
            contextPtr->glClearNamedBufferSubData = LoadGLFunction(load,
                "glClearNamedBufferSubData"u8);
            contextPtr->glClearNamedFramebufferfi = LoadGLFunction(load,
                "glClearNamedFramebufferfi"u8);
            contextPtr->glClearNamedFramebufferfv = LoadGLFunction(load,
                "glClearNamedFramebufferfv"u8);
            contextPtr->glClearNamedFramebufferiv = LoadGLFunction(load,
                "glClearNamedFramebufferiv"u8);
            contextPtr->glClearNamedFramebufferuiv = LoadGLFunction(load,
                "glClearNamedFramebufferuiv"u8);
            contextPtr->glClipControl = LoadGLFunction(load, "glClipControl"u8);
            contextPtr->glCompressedTextureSubImage1D = LoadGLFunction(load,
                "glCompressedTextureSubImage1D"u8);
            contextPtr->glCompressedTextureSubImage2D = LoadGLFunction(load,
                "glCompressedTextureSubImage2D"u8);
            contextPtr->glCompressedTextureSubImage3D = LoadGLFunction(load,
                "glCompressedTextureSubImage3D"u8);
            contextPtr->glCopyNamedBufferSubData = LoadGLFunction(load,
                "glCopyNamedBufferSubData"u8);
            contextPtr->glCopyTextureSubImage1D = LoadGLFunction(load,
                "glCopyTextureSubImage1D"u8);
            contextPtr->glCopyTextureSubImage2D = LoadGLFunction(load,
                "glCopyTextureSubImage2D"u8);
            contextPtr->glCopyTextureSubImage3D = LoadGLFunction(load,
                "glCopyTextureSubImage3D"u8);
            contextPtr->glCreateBuffers = LoadGLFunction(load, "glCreateBuffers"u8);
            contextPtr->glCreateFramebuffers = LoadGLFunction(load,
                "glCreateFramebuffers"u8);
            contextPtr->glCreateProgramPipelines = LoadGLFunction(load,
                "glCreateProgramPipelines"u8);
            contextPtr->glCreateQueries = LoadGLFunction(load,
                "glCreateQueries"u8);
            contextPtr->glCreateRenderbuffers = LoadGLFunction(load,
                "glCreateRenderbuffers"u8);
            contextPtr->glCreateSamplers = LoadGLFunction(load,
                "glCreateSamplers"u8);
            contextPtr->glCreateTextures = LoadGLFunction(load,
                "glCreateTextures"u8);
            contextPtr->glCreateTransformFeedbacks = LoadGLFunction(load,
                "glCreateTransformFeedbacks"u8);
            contextPtr->glCreateVertexArrays = LoadGLFunction(load,
                "glCreateVertexArrays"u8);
            contextPtr->glDisableVertexArrayAttrib = LoadGLFunction(load,
                "glDisableVertexArrayAttrib"u8);
            contextPtr->glEnableVertexArrayAttrib = LoadGLFunction(load,
                "glEnableVertexArrayAttrib"u8);
            contextPtr->glFlushMappedNamedBufferRange = LoadGLFunction(load,
                "glFlushMappedNamedBufferRange"u8);
            contextPtr->glGenerateTextureMipmap = LoadGLFunction(load,
                "glGenerateTextureMipmap"u8);
            contextPtr->glGetCompressedTextureImage = LoadGLFunction(load,
                "glGetCompressedTextureImage"u8);
            contextPtr->glGetCompressedTextureSubImage = LoadGLFunction(load,
                "glGetCompressedTextureSubImage"u8);
            contextPtr->glGetGraphicsResetStatus = LoadGLFunction(load,
                "glGetGraphicsResetStatus"u8);
            contextPtr->glGetNamedBufferParameteri64v = LoadGLFunction(load,
                "glGetNamedBufferParameteri64v"u8);
            contextPtr->glGetNamedBufferParameteriv = LoadGLFunction(load,
                "glGetNamedBufferParameteriv"u8);
            contextPtr->glGetNamedBufferPointerv = LoadGLFunction(load,
                "glGetNamedBufferPointerv"u8);
            contextPtr->glGetNamedBufferSubData = LoadGLFunction(load,
                "glGetNamedBufferSubData"u8);
            contextPtr->glGetNamedFramebufferAttachmentParameteriv = LoadGLFunction(load,
                "glGetNamedFramebufferAttachmentParameteriv"u8);
            contextPtr->glGetNamedFramebufferParameteriv = LoadGLFunction(load,
                "glGetNamedFramebufferParameteriv"u8);
            contextPtr->glGetNamedRenderbufferParameteriv = LoadGLFunction(load,
                "glGetNamedRenderbufferParameteriv"u8);
            contextPtr->glGetQueryBufferObjecti64v = LoadGLFunction(load,
                "glGetQueryBufferObjecti64v"u8);
            contextPtr->glGetQueryBufferObjectiv = LoadGLFunction(load,
                "glGetQueryBufferObjectiv"u8);
            contextPtr->glGetQueryBufferObjectui64v = LoadGLFunction(load,
                "glGetQueryBufferObjectui64v"u8);
            contextPtr->glGetQueryBufferObjectuiv = LoadGLFunction(load,
                "glGetQueryBufferObjectuiv"u8);
            contextPtr->glGetTextureImage = LoadGLFunction(load,
                "glGetTextureImage"u8);
            contextPtr->glGetTextureLevelParameterfv = LoadGLFunction(load,
                "glGetTextureLevelParameterfv"u8);
            contextPtr->glGetTextureLevelParameteriv = LoadGLFunction(load,
                "glGetTextureLevelParameteriv"u8);
            contextPtr->glGetTextureParameterIiv = LoadGLFunction(load,
                "glGetTextureParameterIiv"u8);
            contextPtr->glGetTextureParameterIuiv = LoadGLFunction(load,
                "glGetTextureParameterIuiv"u8);
            contextPtr->glGetTextureParameterfv = LoadGLFunction(load,
                "glGetTextureParameterfv"u8);
            contextPtr->glGetTextureParameteriv = LoadGLFunction(load,
                "glGetTextureParameteriv"u8);
            contextPtr->glGetTextureSubImage = LoadGLFunction(load,
                "glGetTextureSubImage"u8);
            contextPtr->glGetTransformFeedbacki64_v = LoadGLFunction(load,
                "glGetTransformFeedbacki64_v"u8);
            contextPtr->glGetTransformFeedbacki_v = LoadGLFunction(load,
                "glGetTransformFeedbacki_v"u8);
            contextPtr->glGetTransformFeedbackiv = LoadGLFunction(load,
                "glGetTransformFeedbackiv"u8);
            contextPtr->glGetVertexArrayIndexed64iv = LoadGLFunction(load,
                "glGetVertexArrayIndexed64iv"u8);
            contextPtr->glGetVertexArrayIndexediv = LoadGLFunction(load,
                "glGetVertexArrayIndexediv"u8);
            contextPtr->glGetVertexArrayiv = LoadGLFunction(load,
                "glGetVertexArrayiv"u8);
            contextPtr->glGetnCompressedTexImage = LoadGLFunction(load,
                "glGetnCompressedTexImage"u8);
            contextPtr->glGetnTexImage = LoadGLFunction(load,
                "glGetnTexImage"u8);
            contextPtr->glGetnUniformdv = LoadGLFunction(load,
                "glGetnUniformdv"u8);
            contextPtr->glGetnUniformfv = LoadGLFunction(load,
                "glGetnUniformfv"u8);
            contextPtr->glGetnUniformiv = LoadGLFunction(load,
                "glGetnUniformiv"u8);
            contextPtr->glGetnUniformuiv = LoadGLFunction(load,
                "glGetnUniformuiv"u8);
            contextPtr->glInvalidateNamedFramebufferData = LoadGLFunction(load,
                "glInvalidateNamedFramebufferData"u8);
            contextPtr->glInvalidateNamedFramebufferSubData = LoadGLFunction(load,
                "glInvalidateNamedFramebufferSubData"u8);
            contextPtr->glMapNamedBuffer = LoadGLFunction(load,
                "glMapNamedBuffer"u8);
            contextPtr->glMapNamedBufferRange = LoadGLFunction(load,
                "glMapNamedBufferRange"u8);
            contextPtr->glMemoryBarrierByRegion = LoadGLFunction(load,
                "glMemoryBarrierByRegion"u8);
            contextPtr->glNamedBufferData = LoadGLFunction(load,
                "glNamedBufferData"u8);
            contextPtr->glNamedBufferStorage = LoadGLFunction(load,
                "glNamedBufferStorage"u8);
            contextPtr->glNamedBufferSubData = LoadGLFunction(load,
                "glNamedBufferSubData"u8);
            contextPtr->glNamedFramebufferDrawBuffer = LoadGLFunction(load,
                "glNamedFramebufferDrawBuffer"u8);
            contextPtr->glNamedFramebufferDrawBuffers = LoadGLFunction(load,
                "glNamedFramebufferDrawBuffers"u8);
            contextPtr->glNamedFramebufferParameteri = LoadGLFunction(load,
                "glNamedFramebufferParameteri"u8);
            contextPtr->glNamedFramebufferReadBuffer = LoadGLFunction(load,
                "glNamedFramebufferReadBuffer"u8);
            contextPtr->glNamedFramebufferRenderbuffer = LoadGLFunction(load,
                "glNamedFramebufferRenderbuffer"u8);
            contextPtr->glNamedFramebufferTexture = LoadGLFunction(load,
                "glNamedFramebufferTexture"u8);
            contextPtr->glNamedFramebufferTextureLayer = LoadGLFunction(load,
                "glNamedFramebufferTextureLayer"u8);
            contextPtr->glNamedRenderbufferStorage = LoadGLFunction(load,
                "glNamedRenderbufferStorage"u8);
            contextPtr->glNamedRenderbufferStorageMultisample = LoadGLFunction(load,
                "glNamedRenderbufferStorageMultisample"u8);
            contextPtr->glReadnPixels = LoadGLFunction(load,
                "glReadnPixels"u8);
            contextPtr->glTextureBarrier = LoadGLFunction(load, "glTextureBarrier"u8);
            contextPtr->glTextureBuffer = LoadGLFunction(load,
                "glTextureBuffer"u8);
            contextPtr->glTextureBufferRange = LoadGLFunction(load,
                "glTextureBufferRange"u8);
            contextPtr->glTextureParameterIiv = LoadGLFunction(load,
                "glTextureParameterIiv"u8);
            contextPtr->glTextureParameterIuiv = LoadGLFunction(load,
                "glTextureParameterIuiv"u8);
            contextPtr->glTextureParameterf = LoadGLFunction(load,
                "glTextureParameterf"u8);
            contextPtr->glTextureParameterfv = LoadGLFunction(load,
                "glTextureParameterfv"u8);
            contextPtr->glTextureParameteri = LoadGLFunction(load,
                "glTextureParameteri"u8);
            contextPtr->glTextureParameteriv = LoadGLFunction(load,
                "glTextureParameteriv"u8);
            contextPtr->glTextureStorage1D = LoadGLFunction(load,
                "glTextureStorage1D"u8);
            contextPtr->glTextureStorage2D = LoadGLFunction(load,
                "glTextureStorage2D"u8);
            contextPtr->glTextureStorage2DMultisample = LoadGLFunction(load,
                "glTextureStorage2DMultisample"u8);
            contextPtr->glTextureStorage3D = LoadGLFunction(load,
                "glTextureStorage3D"u8);
            contextPtr->glTextureStorage3DMultisample = LoadGLFunction(load,
                "glTextureStorage3DMultisample"u8);
            contextPtr->glTextureSubImage1D = LoadGLFunction(load,
                "glTextureSubImage1D"u8);
            contextPtr->glTextureSubImage2D = LoadGLFunction(load,
                "glTextureSubImage2D"u8);
            contextPtr->glTextureSubImage3D = LoadGLFunction(load,
                "glTextureSubImage3D"u8);
            contextPtr->glTransformFeedbackBufferBase = LoadGLFunction(load,
                "glTransformFeedbackBufferBase"u8);
            contextPtr->glTransformFeedbackBufferRange = LoadGLFunction(load,
                "glTransformFeedbackBufferRange"u8);
            contextPtr->glUnmapNamedBuffer = LoadGLFunction(load, "glUnmapNamedBuffer"u8);
            contextPtr->glVertexArrayAttribBinding = LoadGLFunction(load,
                "glVertexArrayAttribBinding"u8);
            contextPtr->glVertexArrayAttribFormat = LoadGLFunction(load,
                "glVertexArrayAttribFormat"u8);
            contextPtr->glVertexArrayAttribIFormat = LoadGLFunction(load,
                "glVertexArrayAttribIFormat"u8);
            contextPtr->glVertexArrayAttribLFormat = LoadGLFunction(load,
                "glVertexArrayAttribLFormat"u8);
            contextPtr->glVertexArrayBindingDivisor = LoadGLFunction(load,
                "glVertexArrayBindingDivisor"u8);
            contextPtr->glVertexArrayElementBuffer = LoadGLFunction(load,
                "glVertexArrayElementBuffer"u8);
            contextPtr->glVertexArrayVertexBuffer = LoadGLFunction(load,
                "glVertexArrayVertexBuffer"u8);
            contextPtr->glVertexArrayVertexBuffers = LoadGLFunction(load,
                "glVertexArrayVertexBuffers"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.5 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore46(
                                    glContextShadow* contextPtr,
                                    delegate*<ReadOnlySpan<byte>, IntPtr> load
        )
        {
            if (!core46)
            {
                DefaultLog.InternalAppendLog("The core version 4.6 is not supported.");
                return false;
            }

            #region load

            contextPtr->glMultiDrawArraysIndirectCount = LoadGLFunction(load,
                "glMultiDrawArraysIndirectCount"u8);
            contextPtr->glMultiDrawElementsIndirectCount = LoadGLFunction(load,
                "glMultiDrawElementsIndirectCount"u8);
            contextPtr->glPolygonOffsetClamp = LoadGLFunction(load,
                "glPolygonOffsetClamp"u8);
            contextPtr->glSpecializeShader = LoadGLFunction(load,
                "glSpecializeShader"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load OpenGL 4.6 APIs");
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe IntPtr LoadGLFunction(
                                     delegate*<ReadOnlySpan<byte>, IntPtr> load,
                                     ReadOnlySpan<byte> name
        )
        {
            IntPtr ptr = load(name);
            if (ptr == IntPtr.Zero) {
                DefaultLog.InternalAppendLog($"Failed to load proc {System.Text.Encoding.UTF8.GetString(name)}.");
            }
            return ptr;
        }

    }
}
