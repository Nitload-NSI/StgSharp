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

namespace StgSharp.Graphics.OpenGL
{
    public static partial class glManager
    {

        internal static unsafe bool LoadGLcore10(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core10)
            {
                DefaultLog.InternalAppendLog("The core version 1.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBlendFunc = (delegate*<uint, uint, void>)load("glBlendFunc"u8);
            contextPtr->glClear = (delegate*<uint, void>)load("glClear"u8);
            contextPtr->glClearColor = (delegate*<float, float, float, float, void>)load("glClearColor"u8);
            contextPtr->glClearDepth = (delegate*<double, void>)load("glClearDepth"u8);
            contextPtr->glClearStencil = (delegate*<int, void>)load("glClearStencil"u8);
            contextPtr->glColorMask = (delegate*<bool, bool, bool, bool, void>)load("glColorMask"u8);
            contextPtr->glCullFace = (delegate*<uint, void>)load("glCullFace"u8);
            contextPtr->glDepthFunc = (delegate*<uint, void>)load("glDepthFunc"u8);
            contextPtr->glDepthMask = (delegate*<bool, void>)load("glDepthMask"u8);
            contextPtr->glDepthRange = (delegate*<double, double, void>)load("glDepthRange"u8);
            contextPtr->glDisable = (delegate*<uint, void>)load("glDisable"u8);
            contextPtr->glDrawBuffer = (delegate*<uint, void>)load("glDrawBuffer"u8);
            contextPtr->glEnable = (delegate*<uint, void>)load("glEnable"u8);
            contextPtr->glFinish = (delegate*<void>)load("glFinish"u8);
            contextPtr->glFlush = (delegate*<void>)load("glFlush"u8);
            contextPtr->glFrontFace = (delegate*<uint, void>)load("glFrontFace"u8);
            contextPtr->glGetBooleanv = (delegate*<uint, bool*, void>)load("glGetBooleanv"u8);
            contextPtr->glGetDoublev = (delegate*<uint, double*, void>)load("glGetDoublev"u8);
            contextPtr->glGetError = (delegate*<uint>)load("glGetError"u8);
            contextPtr->glGetFloatv = (delegate*<uint, float*, void>)load("glGetFloatv"u8);
            contextPtr->glGetIntegerv = (delegate*<uint, int*, void>)load("glGetIntegerv"u8);
            contextPtr->glGetString = (delegate*<uint, IntPtr>)load("glGetString"u8);
            contextPtr->glGetTexImage = (delegate*<uint, int, uint, uint, void*, void>)load("glGetTexImage"u8);
            contextPtr->glGetTexLevelParameterfv = (delegate*<uint, int, uint, float*, void>)load("glGetTexLevelParameterfv"u8);
            contextPtr->glGetTexLevelParameteriv = (delegate*<uint, int, uint, int*, void>)load("glGetTexLevelParameteriv"u8);
            contextPtr->glGetTexParameterfv = (delegate*<uint, uint, float*, void>)load("glGetTexParameterfv"u8);
            contextPtr->glGetTexParameteriv = (delegate*<uint, uint, int*, void>)load("glGetTexParameteriv"u8);
            contextPtr->glHint = (delegate*<uint, uint, void>)load("glHint"u8);
            contextPtr->glIsEnabled = (delegate*<uint, bool>)load("glIsEnabled"u8);
            contextPtr->glLineWidth = (delegate*<float, void>)load("glLineWidth"u8);
            contextPtr->glLogicOp = (delegate*<uint, void>)load("glLogicOp"u8);
            contextPtr->glPixelStoref = (delegate*<uint, float, void>)load("glPixelStoref"u8);
            contextPtr->glPixelStorei = (delegate*<uint, int, void>)load("glPixelStorei"u8);
            contextPtr->glPointSize = (delegate*<float, void>)load("glPointSize"u8);
            contextPtr->glPolygonMode = (delegate*<uint, uint, void>)load("glPolygonMode"u8);
            contextPtr->glReadBuffer = (delegate*<uint, void>)load("glReadBuffer"u8);
            contextPtr->glReadPixels = (delegate*<int, int, int, int, uint, uint, void*, void>)load("glReadPixels"u8);
            contextPtr->glScissor = (delegate*<int, int, uint, uint, void>)load("glScissor"u8);
            contextPtr->glStencilFunc = (delegate*<uint, int, uint, void>)load("glStencilFunc"u8);
            contextPtr->glStencilMask = (delegate*<uint, void>)load("glStencilMask"u8);
            contextPtr->glStencilOp = (delegate*<uint, uint, uint, void>)load("glStencilOp"u8);
            contextPtr->glTexImage1D = (delegate*<uint, int, int, uint, int, uint, uint, void*, void>)load("glTexImage1D"u8);
            contextPtr->glTexImage2D = (delegate*<uint, int, uint, uint, uint, int, uint, uint, void*, void>)load("glTexImage2D"u8);
            contextPtr->glTexParameterf = (delegate*<uint, uint, float, void>)load("glTexParameterf"u8);
            contextPtr->glTexParameterfv = (delegate*<uint, uint, float*, void>)load("glTexParameterfv"u8);
            contextPtr->glTexParameteri = (delegate*<uint, uint, int, void>)load("glTexParameteri"u8);
            contextPtr->glTexParameteriv = (delegate*<uint, uint, int*, void>)load("glTexParameteriv"u8);
            contextPtr->glViewport = (delegate*<int, int, uint, uint, void>)load("glViewport"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 1.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore11(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core11)
            {
                DefaultLog.InternalAppendLog("The core version 1.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindTexture = (delegate*<uint, uint, void>)load("glBindTexture"u8);
            contextPtr->glCopyTexImage1D = (delegate*<uint, int, uint, int, int, uint, int, void>)load(
                "glCopyTexImage1D"u8);
            contextPtr->glCopyTexImage2D = (delegate*<uint, int, uint, int, int, uint, uint, int, void>)load(
                "glCopyTexImage2D"u8);
            contextPtr->glCopyTexSubImage1D = (delegate*<uint, int, int, int, int, uint, void>)load(
                "glCopyTexSubImage1D"u8);
            contextPtr->glCopyTexSubImage2D = (delegate*<uint, int, int, int, int, int, uint, uint, void>)load(
                "glCopyTexSubImage2D"u8);
            contextPtr->glDeleteTextures = (delegate*<uint, uint*, void>)load(
                "glDeleteTextures"u8);
            contextPtr->glDrawArrays = (delegate*<uint, int, uint, void>)load("glDrawArrays"u8);
            contextPtr->glDrawElements = (delegate*<uint, uint, uint, void*, void>)load(
                "glDrawElements"u8);
            contextPtr->glGenTextures = (delegate*<int, uint*, void>)load("glGenTextures"u8);
            contextPtr->glGetPointerv = (delegate*<uint, void**, void>)load("glGetPointerv"u8);
            contextPtr->glIsTexture = (delegate*<uint, bool>)load("glIsTexture"u8);
            contextPtr->glPolygonOffset = (delegate*<float, float, void>)load("glPolygonOffset"u8);
            contextPtr->glTexSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load(
                "glTexSubImage1D"u8);
            contextPtr->glTexSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load(
                "glTexSubImage2D"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 1.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore12(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core12)
            {
                DefaultLog.InternalAppendLog("The core version 1.2 is not supported.");
                return false;
            }

            #region load

            contextPtr->glCopyTexSubImage3D = (delegate*<uint, int, int, int, int, int, int, uint, uint, void>)load(
                "glCopyTexSubImage3D"u8);
            contextPtr->glDrawRangeElements = (delegate*<uint, uint, uint, uint, uint, void*, void>)load(
                "glDrawRangeElements"u8);
            contextPtr->glTexImage3D = (delegate*<uint, int, int, uint, uint, uint, int, uint, uint, void*, void>)load(
                "glTexImage3D"u8);
            contextPtr->glTexSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load(
                "glTexSubImage3D"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 1.2 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore13(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core13)
            {
                DefaultLog.InternalAppendLog("The core version 1.3 is not supported.");
                return false;
            }

            #region load

            contextPtr->glActiveTexture = (delegate*<uint, void>)load("glActiveTexture"u8);
            contextPtr->glCompressedTexImage1D = (delegate*<uint, int, uint, uint, int, uint, void*, void>)load(
                "glCompressedTexImage1D"u8);
            contextPtr->glCompressedTexImage2D = (delegate*<uint, int, uint, uint, uint, int, uint, void*, void>)load(
                "glCompressedTexImage2D"u8);
            contextPtr->glCompressedTexImage3D = (delegate*<uint, int, uint, uint, uint, uint, int, uint, void*, void>)load(
                "glCompressedTexImage3D"u8);
            contextPtr->glCompressedTexSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load(
                "glCompressedTexSubImage1D"u8);
            contextPtr->glCompressedTexSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load(
                "glCompressedTexSubImage2D"u8);
            contextPtr->glCompressedTexSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load(
                "glCompressedTexSubImage3D"u8);
            contextPtr->glGetCompressedTexImage = (delegate*<uint, int, void*, void>)load(
                "glGetCompressedTexImage"u8);
            contextPtr->glSampleCoverage = (delegate*<float, bool, void>)load(
                "glSampleCoverage"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 1.3 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore14(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core14)
            {
                DefaultLog.InternalAppendLog("The core version 1.4 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBlendColor = (delegate*<float, float, float, float, void>)load(
                "glBlendColor"u8);
            contextPtr->glBlendEquation = (delegate*<uint, void>)load("glBlendEquation"u8);
            contextPtr->glBlendFuncSeparate = (delegate*<uint, uint, uint, uint, void>)load(
                "glBlendFuncSeparate"u8);
            contextPtr->glMultiDrawArrays = (delegate*<uint, int*, uint*, uint, void>)load(
                "glMultiDrawArrays"u8);
            contextPtr->glMultiDrawElements = (delegate*<uint, uint*, uint, void*, void**, uint, void>)load(
                "glMultiDrawElements"u8);
            contextPtr->glPointParameterf = (delegate*<uint, float, void>)load(
                "glPointParameterf"u8);
            contextPtr->glPointParameterfv = (delegate*<uint, float*, void>)load(
                "glPointParameterfv"u8);
            contextPtr->glPointParameteri = (delegate*<uint, int, void>)load(
                "glPointParameteri"u8);
            contextPtr->glPointParameteriv = (delegate*<uint, int*, void>)load(
                "glPointParameteriv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 1.4 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore15(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core15)
            {
                DefaultLog.InternalAppendLog("The core version 1.5 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBeginQuery = (delegate*<uint, uint, void>)load("glBeginQuery"u8);
            contextPtr->glBindBuffer = (delegate*<uint, uint, void>)load("glBindBuffer"u8);
            contextPtr->glBufferData = (delegate*<uint, IntPtr, IntPtr, uint, void>)load(
                "glBufferData"u8);
            contextPtr->glBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load(
                "glBufferSubData"u8);
            contextPtr->glDeleteBuffers = (delegate*<int, uint*, void>)load("glDeleteBuffers"u8);
            contextPtr->glDeleteQueries = (delegate*<uint, uint*, void>)load("glDeleteQueries"u8);
            contextPtr->glEndQuery = (delegate*<uint, void>)load("glEndQuery"u8);
            contextPtr->glGenBuffers = (delegate*<int, uint*, void>)load("glGenBuffers"u8);
            contextPtr->glGenQueries = (delegate*<uint, uint*, void>)load("glGenQueries"u8);
            contextPtr->glGetBufferParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetBufferParameteriv"u8);
            contextPtr->glGetBufferPointerv = (delegate*<uint, uint, void**, void>)load(
                "glGetBufferPointerv"u8);
            contextPtr->glGetBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load(
                "glGetBufferSubData"u8);
            contextPtr->glGetQueryObjectiv = (delegate*<uint, uint, int*, void>)load(
                "glGetQueryObjectiv"u8);
            contextPtr->glGetQueryObjectuiv = (delegate*<uint, uint, uint*, void>)load(
                "glGetQueryObjectuiv"u8);
            contextPtr->glGetQueryiv = (delegate*<uint, uint, int*, void>)load("glGetQueryiv"u8);
            contextPtr->glIsBuffer = (delegate*<uint, bool>)load("glIsBuffer"u8);
            contextPtr->glIsQuery = (delegate*<uint, bool>)load("glIsQuery"u8);
            contextPtr->glMapBuffer = (delegate*<uint, uint, void*>)load("glMapBuffer"u8);
            contextPtr->glUnmapBuffer = (delegate*<uint, bool>)load("glUnmapBuffer"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 1.5 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore20(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core20)
            {
                DefaultLog.InternalAppendLog("The core version 2.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glAttachShader = (delegate*<uint, uint, void>)load("glAttachShader"u8);
            contextPtr->glBindAttribLocation = (delegate*<uint, uint, byte*, void>)load(
                "glBindAttribLocation"u8);
            contextPtr->glBlendEquationSeparate = (delegate*<uint, uint, void>)load(
                "glBlendEquationSeparate"u8);
            contextPtr->glCompileShader = (delegate*<uint, void>)load("glCompileShader"u8);
            contextPtr->glCreateProgram = (delegate*<uint>)load("glCreateProgram"u8);
            contextPtr->glCreateShader = (delegate*<uint, uint>)load("glCreateShader"u8);
            contextPtr->glDeleteProgram = (delegate*<uint, void>)load("glDeleteProgram"u8);
            contextPtr->glDeleteShader = (delegate*<uint, void>)load("glDeleteShader"u8);
            contextPtr->glDetachShader = (delegate*<uint, uint, void>)load("glDetachShader"u8);
            contextPtr->glDisableVertexAttribArray = (delegate*<uint, void>)load(
                "glDisableVertexAttribArray"u8);
            contextPtr->glDrawBuffers = (delegate*<uint, uint*, void>)load("glDrawBuffers"u8);
            contextPtr->glEnableVertexAttribArray = (delegate*<uint, void>)load(
                "glEnableVertexAttribArray"u8);
            contextPtr->glGetActiveAttrib = (delegate*<uint, uint, uint, uint*, int*, uint*, byte*, void>)load(
                "glGetActiveAttrib"u8);
            contextPtr->glGetActiveUniform = (delegate*<uint, uint, uint, uint*, int*, uint*, byte*, void>)load(
                "glGetActiveUniform"u8);
            contextPtr->glGetAttachedShaders = (delegate*<uint, uint, uint*, uint*, void>)load(
                "glGetAttachedShaders"u8);
            contextPtr->glGetAttribLocation = (delegate*<uint, byte*, int>)load(
                "glGetAttribLocation"u8);
            contextPtr->glGetProgramInfoLog = (delegate*<uint, uint, uint*, byte*, void>)load(
                "glGetProgramInfoLog"u8);
            contextPtr->glGetProgramiv = (delegate*<uint, uint, int*, void>)load(
                "glGetProgramiv"u8);
            contextPtr->glGetShaderInfoLog = (delegate*<uint, uint, uint*, byte*, void>)load(
                "glGetShaderInfoLog"u8);
            contextPtr->glGetShaderSource = (delegate*<uint, uint, uint*, byte*, void>)load(
                "glGetShaderSource"u8);
            contextPtr->glGetShaderiv = (delegate*<uint, uint, int*, void>)load("glGetShaderiv"u8);
            contextPtr->glGetUniformLocation = (delegate*<uint, byte*, int>)load(
                "glGetUniformLocation"u8);
            contextPtr->glGetUniformfv = (delegate*<uint, int, float*, void>)load(
                "glGetUniformfv"u8);
            contextPtr->glGetUniformiv = (delegate*<uint, int, int*, void>)load(
                "glGetUniformiv"u8);
            contextPtr->glGetVertexAttribPointerv = (delegate*<uint, uint, void**, void>)load(
                "glGetVertexAttribPointerv"u8);
            contextPtr->glGetVertexAttribdv = (delegate*<uint, uint, double*, void>)load(
                "glGetVertexAttribdv"u8);
            contextPtr->glGetVertexAttribfv = (delegate*<uint, uint, float*, void>)load(
                "glGetVertexAttribfv"u8);
            contextPtr->glGetVertexAttribiv = (delegate*<uint, uint, int*, void>)load(
                "glGetVertexAttribiv"u8);
            contextPtr->glIsProgram = (delegate*<uint, bool>)load("glIsProgram"u8);
            contextPtr->glIsShader = (delegate*<uint, bool>)load("glIsShader"u8);
            contextPtr->glLinkProgram = (delegate*<uint, void>)load("glLinkProgram"u8);
            contextPtr->glShaderSource = (delegate* unmanaged[Stdcall]<uint, uint, byte**, void*, void>)load(
                "glShaderSource"u8);
            contextPtr->glStencilFuncSeparate = (delegate*<uint, uint, int, uint, void>)load(
                "glStencilFuncSeparate"u8);
            contextPtr->glStencilMaskSeparate = (delegate*<uint, uint, void>)load(
                "glStencilMaskSeparate"u8);
            contextPtr->glStencilOpSeparate = (delegate*<uint, uint, uint, uint, void>)load(
                "glStencilOpSeparate"u8);
            contextPtr->glUniform1f = (delegate*<int, float, void>)load("glUniform1f"u8);
            contextPtr->glUniform1fv = (delegate*<int, uint, M128*, void>)load("glUniform1fv"u8);
            contextPtr->glUniform1i = (delegate*<int, int, void>)load("glUniform1i"u8);
            contextPtr->glUniform1iv = (delegate*<int, uint, int*, void>)load("glUniform1iv"u8);
            contextPtr->glUniform2f = (delegate*<int, float, float, void>)load("glUniform2f"u8);
            contextPtr->glUniform2fv = (delegate*<int, uint, float*, void>)load("glUniform2fv"u8);
            contextPtr->glUniform2i = (delegate*<int, int, int, void>)load("glUniform2i"u8);
            contextPtr->glUniform2iv = (delegate*<int, uint, int*, void>)load("glUniform2iv"u8);
            contextPtr->glUniform3f = (delegate*<int, float, float, float, void>)load(
                "glUniform3f"u8);
            contextPtr->glUniform3fv = (delegate*<int, uint, float*, void>)load("glUniform3fv"u8);
            contextPtr->glUniform3i = (delegate*<int, int, int, int, void>)load("glUniform3i"u8);
            contextPtr->glUniform3iv = (delegate*<int, uint, int*, void>)load("glUniform3iv"u8);
            contextPtr->glUniform4f = (delegate*<int, float, float, float, float, void>)load(
                "glUniform4f"u8);
            contextPtr->glUniform4fv = (delegate*<int, uint, float*, void>)load("glUniform4fv"u8);
            contextPtr->glUniform4i = (delegate*<int, int, int, int, int, void>)load(
                "glUniform4i"u8);
            contextPtr->glUniform4iv = (delegate*<int, uint, int*, void>)load("glUniform4iv"u8);
            contextPtr->glUniformMatrix2fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix2fv"u8);
            contextPtr->glUniformMatrix3fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix3fv"u8);
            contextPtr->glUniformMatrix4fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix4fv"u8);
            contextPtr->glUseProgram = (delegate*<uint, void>)load("glUseProgram"u8);
            contextPtr->glValidateProgram = (delegate*<uint, void>)load("glValidateProgram"u8);
            contextPtr->glVertexAttrib1d = (delegate*<uint, double, void>)load(
                "glVertexAttrib1d"u8);
            contextPtr->glVertexAttrib1dv = (delegate*<uint, double*, void>)load(
                "glVertexAttrib1dv"u8);
            contextPtr->glVertexAttrib1f = (delegate*<uint, float, void>)load(
                "glVertexAttrib1f"u8);
            contextPtr->glVertexAttrib1fv = (delegate*<uint, float*, void>)load(
                "glVertexAttrib1fv"u8);
            contextPtr->glVertexAttrib1s = (delegate*<uint, short, void>)load(
                "glVertexAttrib1s"u8);
            contextPtr->glVertexAttrib1sv = (delegate*<uint, short*, void>)load(
                "glVertexAttrib1sv"u8);
            contextPtr->glVertexAttrib2d = (delegate*<uint, double, double, void>)load(
                "glVertexAttrib2d"u8);
            contextPtr->glVertexAttrib2dv = (delegate*<uint, double*, void>)load(
                "glVertexAttrib2dv"u8);
            contextPtr->glVertexAttrib2f = (delegate*<uint, float, float, void>)load(
                "glVertexAttrib2f"u8);
            contextPtr->glVertexAttrib2fv = (delegate*<uint, float*, void>)load(
                "glVertexAttrib2fv"u8);
            contextPtr->glVertexAttrib2s = (delegate*<uint, short, short, void>)load(
                "glVertexAttrib2s"u8);
            contextPtr->glVertexAttrib2sv = (delegate*<uint, short*, void>)load(
                "glVertexAttrib2sv"u8);
            contextPtr->glVertexAttrib3d = (delegate*<uint, double, double, double, void>)load(
                "glVertexAttrib3d"u8);
            contextPtr->glVertexAttrib3dv = (delegate*<uint, double*, void>)load(
                "glVertexAttrib3dv"u8);
            contextPtr->glVertexAttrib3f = (delegate*<uint, float, float, float, void>)load(
                "glVertexAttrib3f"u8);
            contextPtr->glVertexAttrib3fv = (delegate*<uint, float*, void>)load(
                "glVertexAttrib3fv"u8);
            contextPtr->glVertexAttrib3s = (delegate*<uint, short, short, short, void>)load(
                "glVertexAttrib3s"u8);
            contextPtr->glVertexAttrib3sv = (delegate*<uint, short*, void>)load(
                "glVertexAttrib3sv"u8);
            contextPtr->glVertexAttrib4Nbv = (delegate*<uint, sbyte*, void>)load(
                "glVertexAttrib4Nbv"u8);
            contextPtr->glVertexAttrib4Niv = (delegate*<uint, int*, void>)load(
                "glVertexAttrib4Niv"u8);
            contextPtr->glVertexAttrib4Nsv = (delegate*<uint, short*, void>)load(
                "glVertexAttrib4Nsv"u8);
            contextPtr->glVertexAttrib4Nub = (delegate*<uint, byte, byte, byte, byte, void>)load(
                "glVertexAttrib4Nub"u8);
            contextPtr->glVertexAttrib4Nubv = (delegate*<uint, byte*, void>)load(
                "glVertexAttrib4Nubv"u8);
            contextPtr->glVertexAttrib4Nuiv = (delegate*<uint, uint*, void>)load(
                "glVertexAttrib4Nuiv"u8);
            contextPtr->glVertexAttrib4Nusv = (delegate*<uint, ushort*, void>)load(
                "glVertexAttrib4Nusv"u8);
            contextPtr->glVertexAttrib4bv = (delegate*<uint, sbyte*, void>)load(
                "glVertexAttrib4bv"u8);
            contextPtr->glVertexAttrib4d = (delegate*<uint, double, double, double, double, void>)load(
                "glVertexAttrib4d"u8);
            contextPtr->glVertexAttrib4dv = (delegate*<uint, double*, void>)load(
                "glVertexAttrib4dv"u8);
            contextPtr->glVertexAttrib4f = (delegate*<uint, float, float, float, float, void>)load(
                "glVertexAttrib4f"u8);
            contextPtr->glVertexAttrib4fv = (delegate*<uint, float*, void>)load(
                "glVertexAttrib4fv"u8);
            contextPtr->glVertexAttrib4iv = (delegate*<uint, int*, void>)load(
                "glVertexAttrib4iv"u8);
            contextPtr->glVertexAttrib4s = (delegate*<uint, short, short, short, short, void>)load(
                "glVertexAttrib4s"u8);
            contextPtr->glVertexAttrib4sv = (delegate*<uint, short*, void>)load(
                "glVertexAttrib4sv"u8);
            contextPtr->glVertexAttrib4ubv = (delegate*<uint, byte*, void>)load(
                "glVertexAttrib4ubv"u8);
            contextPtr->glVertexAttrib4uiv = (delegate*<uint, uint*, void>)load(
                "glVertexAttrib4uiv"u8);
            contextPtr->glVertexAttrib4usv = (delegate*<uint, ushort*, void>)load(
                "glVertexAttrib4usv"u8);
            contextPtr->glVertexAttribPointer = (delegate*<uint, int, uint, int, uint, void*, void>)load(
                "glVertexAttribPointer"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 2.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore21(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core21)
            {
                DefaultLog.InternalAppendLog("The core version 2.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glUniformMatrix2x3fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix2x3fv"u8);
            contextPtr->glUniformMatrix2x4fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix2x4fv"u8);
            contextPtr->glUniformMatrix3x2fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix3x2fv"u8);
            contextPtr->glUniformMatrix3x4fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix3x4fv"u8);
            contextPtr->glUniformMatrix4x2fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix4x2fv"u8);
            contextPtr->glUniformMatrix4x3fv = (delegate*<int, uint, bool, float*, void>)load(
                "glUniformMatrix4x3fv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 2.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore30(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core30)
            {
                DefaultLog.InternalAppendLog("The core version 3.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBeginConditionalRender = (delegate*<uint, uint, void>)load(
                "glBeginConditionalRender"u8);
            contextPtr->glBeginTransformFeedback = (delegate*<uint, void>)load(
                "glBeginTransformFeedback"u8);
            contextPtr->glBindBufferBase = (delegate*<uint, uint, uint, void>)load(
                "glBindBufferBase"u8);
            contextPtr->glBindBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load(
                "glBindBufferRange"u8);
            contextPtr->glBindFragDataLocation = (delegate*<uint, uint, byte*, void>)load(
                "glBindFragDataLocation"u8);
            contextPtr->glBindFramebuffer = (delegate*<uint, uint, void>)load(
                "glBindFramebuffer"u8);
            contextPtr->glBindRenderbuffer = (delegate*<uint, uint, void>)load(
                "glBindRenderbuffer"u8);
            contextPtr->glBindVertexArray = (delegate*<uint, void>)load("glBindVertexArray"u8);
            contextPtr->glBlitFramebuffer = (delegate*<int, int, int, int, int, int, int, int, uint, uint, void>)load(
                "glBlitFramebuffer"u8);
            contextPtr->glCheckFramebufferStatus = (delegate*<uint, uint>)load(
                "glCheckFramebufferStatus"u8);
            contextPtr->glClampColor = (delegate*<uint, uint, void>)load("glClampColor"u8);
            contextPtr->glClearBufferfi = (delegate*<uint, int, float, int, void>)load(
                "glClearBufferfi"u8);
            contextPtr->glClearBufferfv = (delegate*<uint, int, float*, void>)load(
                "glClearBufferfv"u8);
            contextPtr->glClearBufferiv = (delegate*<uint, int, int*, void>)load(
                "glClearBufferiv"u8);
            contextPtr->glClearBufferuiv = (delegate*<uint, int, uint*, void>)load(
                "glClearBufferuiv"u8);
            contextPtr->glColorMaski = (delegate*<uint, bool, bool, bool, bool, void>)load(
                "glColorMaski"u8);
            contextPtr->glDeleteFramebuffers = (delegate*<uint, uint*, void>)load(
                "glDeleteFramebuffers"u8);
            contextPtr->glDeleteRenderbuffers = (delegate*<uint, uint*, void>)load(
                "glDeleteRenderbuffers"u8);
            contextPtr->glDeleteVertexArrays = (delegate*<int, uint*, void>)load(
                "glDeleteVertexArrays"u8);
            contextPtr->glDisablei = (delegate*<uint, uint, void>)load("glDisablei"u8);
            contextPtr->glEnablei = (delegate*<uint, uint, void>)load("glEnablei"u8);
            contextPtr->glEndConditionalRender = (delegate*<void>)load("glEndConditionalRender"u8);
            contextPtr->glEndTransformFeedback = (delegate*<void>)load("glEndTransformFeedback"u8);
            contextPtr->glFlushMappedBufferRange = (delegate*<uint, IntPtr, UIntPtr, void>)load(
                "glFlushMappedBufferRange"u8);
            contextPtr->glFramebufferRenderbuffer = (delegate*<uint, uint, uint, uint, void>)load(
                "glFramebufferRenderbuffer"u8);
            contextPtr->glFramebufferTexture1D = (delegate*<uint, uint, uint, uint, int, void>)load(
                "glFramebufferTexture1D"u8);
            contextPtr->glFramebufferTexture2D = (delegate*<uint, uint, uint, uint, int, void>)load(
                "glFramebufferTexture2D"u8);
            contextPtr->glFramebufferTexture3D = (delegate*<uint, uint, uint, uint, int, int, void>)load(
                "glFramebufferTexture3D"u8);
            contextPtr->glFramebufferTextureLayer = (delegate*<uint, uint, uint, int, int, void>)load(
                "glFramebufferTextureLayer"u8);
            contextPtr->glGenFramebuffers = (delegate*<uint, uint*, void>)load(
                "glGenFramebuffers"u8);
            contextPtr->glGenRenderbuffers = (delegate*<int, uint*, void>)load(
                "glGenRenderbuffers"u8);
            contextPtr->glGenVertexArrays = (delegate*<int, uint*, void>)load(
                "glGenVertexArrays"u8);
            contextPtr->glGenerateMipmap = (delegate*<uint, void>)load("glGenerateMipmap"u8);
            contextPtr->glGetBooleani_v = (delegate*<uint, uint, bool*, void>)load(
                "glGetBooleani_v"u8);
            contextPtr->glGetFragDataLocation = (delegate*<uint, byte*, int>)load(
                "glGetFragDataLocation"u8);
            contextPtr->glGetFramebufferAttachmentParameteriv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetFramebufferAttachmentParameteriv"u8);
            contextPtr->glGetIntegeri_v = (delegate*<uint, uint, int*, void>)load(
                "glGetIntegeri_v"u8);
            contextPtr->glGetRenderbufferParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetRenderbufferParameteriv"u8);
            contextPtr->glGetStringi = (delegate*<uint, uint, byte*>)load("glGetStringi"u8);
            contextPtr->glGetTexParameterIiv = (delegate*<uint, uint, int*, void>)load(
                "glGetTexParameterIiv"u8);
            contextPtr->glGetTexParameterIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glGetTexParameterIuiv"u8);
            contextPtr->glGetTransformFeedbackVarying = (delegate*<uint, uint, uint, uint*, uint*, uint*, byte*, void>)load(
                "glGetTransformFeedbackVarying"u8);
            contextPtr->glGetUniformuiv = (delegate*<uint, int, uint*, void>)load(
                "glGetUniformuiv"u8);
            contextPtr->glGetVertexAttribIiv = (delegate*<uint, uint, int*, void>)load(
                "glGetVertexAttribIiv"u8);
            contextPtr->glGetVertexAttribIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glGetVertexAttribIuiv"u8);
            contextPtr->glIsEnabledi = (delegate*<uint, uint, bool>)load("glIsEnabledi"u8);
            contextPtr->glIsFramebuffer = (delegate*<uint, bool>)load("glIsFramebuffer"u8);
            contextPtr->glIsRenderbuffer = (delegate*<uint, bool>)load("glIsRenderbuffer"u8);
            contextPtr->glIsVertexArray = (delegate*<uint, bool>)load("glIsVertexArray"u8);
            contextPtr->glMapBufferRange = (delegate*<uint, IntPtr, UIntPtr, uint, void*>)load(
                "glMapBufferRange"u8);
            contextPtr->glRenderbufferStorage = (delegate*<uint, uint, int, int, void>)load(
                "glRenderbufferStorage"u8);
            contextPtr->glRenderbufferStorageMultisample = (delegate*<uint, uint, uint, uint, uint, void>)load(
                "glRenderbufferStorageMultisample"u8);
            contextPtr->glTexParameterIiv = (delegate*<uint, uint, int*, void>)load(
                "glTexParameterIiv"u8);
            contextPtr->glTexParameterIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glTexParameterIuiv"u8);
            contextPtr->glTransformFeedbackVaryings = (delegate*<uint, uint, byte*, byte*, uint, void>)load(
                "glTransformFeedbackVaryings"u8);
            contextPtr->glUniform1ui = (delegate*<int, uint, void>)load("glUniform1ui"u8);
            contextPtr->glUniform1uiv = (delegate*<int, uint, uint*, void>)load("glUniform1uiv"u8);
            contextPtr->glUniform2ui = (delegate*<int, uint, uint, void>)load("glUniform2ui"u8);
            contextPtr->glUniform2uiv = (delegate*<int, uint, uint*, void>)load("glUniform2uiv"u8);
            contextPtr->glUniform3ui = (delegate*<int, uint, uint, uint, void>)load(
                "glUniform3ui"u8);
            contextPtr->glUniform3uiv = (delegate*<int, uint, uint*, void>)load("glUniform3uiv"u8);
            contextPtr->glUniform4ui = (delegate*<int, uint, uint, uint, uint, void>)load(
                "glUniform4ui"u8);
            contextPtr->glUniform4uiv = (delegate*<int, uint, uint*, void>)load("glUniform4uiv"u8);
            contextPtr->glVertexAttribI1i = (delegate*<uint, int, void>)load(
                "glVertexAttribI1i"u8);
            contextPtr->glVertexAttribI1iv = (delegate*<uint, int*, void>)load(
                "glVertexAttribI1iv"u8);
            contextPtr->glVertexAttribI1ui = (delegate*<uint, uint, void>)load(
                "glVertexAttribI1ui"u8);
            contextPtr->glVertexAttribI1uiv = (delegate*<uint, uint*, void>)load(
                "glVertexAttribI1uiv"u8);
            contextPtr->glVertexAttribI2i = (delegate*<uint, int, int, void>)load(
                "glVertexAttribI2i"u8);
            contextPtr->glVertexAttribI2iv = (delegate*<uint, int*, void>)load(
                "glVertexAttribI2iv"u8);
            contextPtr->glVertexAttribI2ui = (delegate*<uint, uint, uint, void>)load(
                "glVertexAttribI2ui"u8);
            contextPtr->glVertexAttribI2uiv = (delegate*<uint, uint*, void>)load(
                "glVertexAttribI2uiv"u8);
            contextPtr->glVertexAttribI3i = (delegate*<uint, int, int, int, void>)load(
                "glVertexAttribI3i"u8);
            contextPtr->glVertexAttribI3iv = (delegate*<uint, int*, void>)load(
                "glVertexAttribI3iv"u8);
            contextPtr->glVertexAttribI3ui = (delegate*<uint, uint, uint, uint, void>)load(
                "glVertexAttribI3ui"u8);
            contextPtr->glVertexAttribI3uiv = (delegate*<uint, uint*, void>)load(
                "glVertexAttribI3uiv"u8);
            contextPtr->glVertexAttribI4bv = (delegate*<uint, sbyte*, void>)load(
                "glVertexAttribI4bv"u8);
            contextPtr->glVertexAttribI4i = (delegate*<uint, int, int, int, int, void>)load(
                "glVertexAttribI4i"u8);
            contextPtr->glVertexAttribI4iv = (delegate*<uint, int*, void>)load(
                "glVertexAttribI4iv"u8);
            contextPtr->glVertexAttribI4sv = (delegate*<uint, short*, void>)load(
                "glVertexAttribI4sv"u8);
            contextPtr->glVertexAttribI4ubv = (delegate*<uint, byte*, void>)load(
                "glVertexAttribI4ubv"u8);
            contextPtr->glVertexAttribI4ui = (delegate*<uint, uint, uint, uint, uint, void>)load(
                "glVertexAttribI4ui"u8);
            contextPtr->glVertexAttribI4uiv = (delegate*<uint, uint*, void>)load(
                "glVertexAttribI4uiv"u8);
            contextPtr->glVertexAttribI4usv = (delegate*<uint, ushort*, void>)load(
                "glVertexAttribI4usv"u8);
            contextPtr->glVertexAttribIPointer = (delegate*<uint, int, uint, uint, void*, void>)load(
                "glVertexAttribIPointer"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 3.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore31(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core31)
            {
                DefaultLog.InternalAppendLog("The core version 3.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindBufferBase = (delegate*<uint, uint, uint, void>)load(
                "glBindBufferBase"u8);
            contextPtr->glBindBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load(
                "glBindBufferRange"u8);
            contextPtr->glCopyBufferSubData = (delegate*<uint, uint, IntPtr, IntPtr, UIntPtr, void>)load(
                "glCopyBufferSubData"u8);
            contextPtr->glDrawArraysInstanced = (delegate*<uint, int, uint, uint, void>)load(
                "glDrawArraysInstanced"u8);
            contextPtr->glDrawElementsInstanced = (delegate*<uint, uint, uint, void*, uint, void>)load(
                "glDrawElementsInstanced"u8);
            contextPtr->glGetActiveUniformBlockName = (delegate*<uint, uint, uint, uint*, byte*, void>)load(
                "glGetActiveUniformBlockName"u8);
            contextPtr->glGetActiveUniformBlockiv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetActiveUniformBlockiv"u8);
            contextPtr->glGetActiveUniformName = (delegate*<uint, uint, uint, uint*, byte*, void>)load(
                "glGetActiveUniformName"u8);
            contextPtr->glGetActiveUniformsiv = (delegate*<uint, uint, uint*, uint, int*, void>)load(
                "glGetActiveUniformsiv"u8);
            contextPtr->glGetIntegeri_v = (delegate*<uint, uint, int*, void>)load(
                "glGetIntegeri_v"u8);
            contextPtr->glGetUniformBlockIndex = (delegate*<uint, byte*, uint>)load(
                "glGetUniformBlockIndex"u8);
            contextPtr->glGetUniformIndices = (delegate*<uint, uint, byte*, char**, uint*, void>)load(
                "glGetUniformIndices"u8);
            contextPtr->glPrimitiveRestartIndex = (delegate*<uint, void>)load(
                "glPrimitiveRestartIndex"u8);
            contextPtr->glTexBuffer = (delegate*<uint, uint, uint, void>)load("glTexBuffer"u8);
            contextPtr->glUniformBlockBinding = (delegate*<uint, uint, uint, void>)load(
                "glUniformBlockBinding"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 3.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore32(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core32)
            {
                DefaultLog.InternalAppendLog("The core version 3.2 is not supported.");
                return false;
            }

            #region load

            contextPtr->glClientWaitSync = (delegate*<GLsync, uint, ulong, uint>)load(
                "glClientWaitSync"u8);
            contextPtr->glDeleteSync = (delegate*<GLsync, void>)load("glDeleteSync"u8);
            contextPtr->glDrawElementsBaseVertex = (delegate*<uint, uint, uint, void*, int, void>)load(
                "glDrawElementsBaseVertex"u8);
            contextPtr->glDrawElementsInstancedBaseVertex = (delegate*<uint, uint, uint, void*, uint, int, void>)load(
                "glDrawElementsInstancedBaseVertex"u8);
            contextPtr->glDrawRangeElementsBaseVertex = (delegate*<uint, uint, uint, uint, uint, void*, int, void>)load(
                "glDrawRangeElementsBaseVertex"u8);
            contextPtr->glFenceSync = (delegate*<uint, uint, GLsync>)load("glFenceSync"u8);
            contextPtr->glFramebufferTexture = (delegate*<uint, uint, uint, int, void>)load(
                "glFramebufferTexture"u8);
            contextPtr->glGetBufferParameteri64v = (delegate*<uint, uint, long*, void>)load(
                "glGetBufferParameteri64v"u8);
            contextPtr->glGetInteger64i_v = (delegate*<uint, uint, long*, void>)load(
                "glGetInteger64i_v"u8);
            contextPtr->glGetInteger64v = (delegate*<uint, long*, void>)load("glGetInteger64v"u8);
            contextPtr->glGetMultisamplefv = (delegate*<uint, uint, float*, void>)load(
                "glGetMultisamplefv"u8);
            contextPtr->glGetSynciv = (delegate*<GLsync, uint, uint, uint*, int*, void>)load(
                "glGetSynciv"u8);
            contextPtr->glIsSync = (delegate*<GLsync, bool>)load("glIsSync"u8);
            contextPtr->glMultiDrawElementsBaseVertex = (delegate*<uint, uint*, uint, void*, void**, uint, int*, void>)load(
                "glMultiDrawElementsBaseVertex"u8);
            contextPtr->glProvokingVertex = (delegate*<uint, void>)load("glProvokingVertex"u8);
            contextPtr->glSampleMaski = (delegate*<uint, uint, void>)load("glSampleMaski"u8);
            contextPtr->glTexImage2DMultisample = (delegate*<uint, uint, uint, uint, uint, bool, void>)load(
                "glTexImage2DMultisample"u8);
            contextPtr->glTexImage3DMultisample = (delegate*<uint, uint, uint, uint, uint, uint, bool, void>)load(
                "glTexImage3DMultisample"u8);
            contextPtr->glWaitSync = (delegate*<GLsync, uint, ulong, void>)load("glWaitSync"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 3.2 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore33(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core33)
            {
                DefaultLog.InternalAppendLog("The core version 3.3 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindFragDataLocationIndexed = (delegate*<uint, uint, uint, byte*, void>)load(
                "glBindFragDataLocationIndexed"u8);
            contextPtr->glBindSampler = (delegate*<uint, uint, void>)load("glBindSampler"u8);
            contextPtr->glDeleteSamplers = (delegate*<uint, uint*, void>)load(
                "glDeleteSamplers"u8);
            contextPtr->glGenSamplers = (delegate*<uint, uint*, void>)load("glGenSamplers"u8);
            contextPtr->glGetFragDataIndex = (delegate*<uint, byte*, int>)load(
                "glGetFragDataIndex"u8);
            contextPtr->glGetQueryObjecti64v = (delegate*<uint, uint, long*, void>)load(
                "glGetQueryObjecti64v"u8);
            contextPtr->glGetQueryObjectui64v = (delegate*<uint, uint, ulong*, void>)load(
                "glGetQueryObjectui64v"u8);
            contextPtr->glGetSamplerParameterIiv = (delegate*<uint, uint, int*, void>)load(
                "glGetSamplerParameterIiv"u8);
            contextPtr->glGetSamplerParameterIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glGetSamplerParameterIuiv"u8);
            contextPtr->glGetSamplerParameterfv = (delegate*<uint, uint, float*, void>)load(
                "glGetSamplerParameterfv"u8);
            contextPtr->glGetSamplerParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetSamplerParameteriv"u8);
            contextPtr->glIsSampler = (delegate*<uint, bool>)load("glIsSampler"u8);
            contextPtr->glQueryCounter = (delegate*<uint, uint, void>)load("glQueryCounter"u8);
            contextPtr->glSamplerParameterIiv = (delegate*<uint, uint, int*, void>)load(
                "glSamplerParameterIiv"u8);
            contextPtr->glSamplerParameterIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glSamplerParameterIuiv"u8);
            contextPtr->glSamplerParameterf = (delegate*<uint, uint, float, void>)load(
                "glSamplerParameterf"u8);
            contextPtr->glSamplerParameterfv = (delegate*<uint, uint, float*, void>)load(
                "glSamplerParameterfv"u8);
            contextPtr->glSamplerParameteri = (delegate*<uint, uint, int, void>)load(
                "glSamplerParameteri"u8);
            contextPtr->glSamplerParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glSamplerParameteriv"u8);
            contextPtr->glVertexAttribDivisor = (delegate*<uint, uint, void>)load(
                "glVertexAttribDivisor"u8);
            contextPtr->glVertexAttribP1ui = (delegate*<uint, uint, bool, uint, void>)load(
                "glVertexAttribP1ui"u8);
            contextPtr->glVertexAttribP1uiv = (delegate*<uint, uint, bool, uint*, void>)load(
                "glVertexAttribP1uiv"u8);
            contextPtr->glVertexAttribP2ui = (delegate*<uint, uint, bool, uint, void>)load(
                "glVertexAttribP2ui"u8);
            contextPtr->glVertexAttribP2uiv = (delegate*<uint, uint, bool, uint*, void>)load(
                "glVertexAttribP2uiv"u8);
            contextPtr->glVertexAttribP3ui = (delegate*<uint, uint, bool, uint, void>)load(
                "glVertexAttribP3ui"u8);
            contextPtr->glVertexAttribP3uiv = (delegate*<uint, uint, bool, uint*, void>)load(
                "glVertexAttribP3uiv"u8);
            contextPtr->glVertexAttribP4ui = (delegate*<uint, uint, bool, uint, void>)load(
                "glVertexAttribP4ui"u8);
            contextPtr->glVertexAttribP4uiv = (delegate*<uint, uint, bool, uint*, void>)load(
                "glVertexAttribP4uiv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 3.3 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore40(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core40)
            {
                DefaultLog.InternalAppendLog("The core version 4.0 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBeginQueryIndexed = (delegate*<uint, uint, uint, void>)load(
                "glBeginQueryIndexed"u8);
            contextPtr->glBindTransformFeedback = (delegate*<uint, uint, void>)load(
                "glBindTransformFeedback"u8);
            contextPtr->glBlendEquationSeparatei = (delegate*<uint, uint, uint, void>)load(
                "glBlendEquationSeparatei"u8);
            contextPtr->glBlendEquationi = (delegate*<uint, uint, void>)load("glBlendEquationi"u8);
            contextPtr->glBlendFuncSeparatei = (delegate*<uint, uint, uint, uint, uint, void>)load(
                "glBlendFuncSeparatei"u8);
            contextPtr->glBlendFunci = (delegate*<uint, uint, uint, void>)load("glBlendFunci"u8);
            contextPtr->glDeleteTransformFeedbacks = (delegate*<uint, uint*, void>)load(
                "glDeleteTransformFeedbacks"u8);
            contextPtr->glDrawArraysIndirect = (delegate*<uint, void*, void>)load(
                "glDrawArraysIndirect"u8);
            contextPtr->glDrawElementsIndirect = (delegate*<uint, uint, void*, void>)load(
                "glDrawElementsIndirect"u8);
            contextPtr->glDrawTransformFeedback = (delegate*<uint, uint, void>)load(
                "glDrawTransformFeedback"u8);
            contextPtr->glDrawTransformFeedbackStream = (delegate*<uint, uint, uint, void>)load(
                "glDrawTransformFeedbackStream"u8);
            contextPtr->glEndQueryIndexed = (delegate*<uint, uint, void>)load(
                "glEndQueryIndexed"u8);
            contextPtr->glGenTransformFeedbacks = (delegate*<uint, uint*, void>)load(
                "glGenTransformFeedbacks"u8);
            contextPtr->glGetActiveSubroutineName = (delegate*<uint, uint, uint, uint, uint*, byte*, void>)load(
                "glGetActiveSubroutineName"u8);
            contextPtr->glGetActiveSubroutineUniformName = (delegate*<uint, uint, uint, uint, uint*, byte*, void>)load(
                "glGetActiveSubroutineUniformName"u8);
            contextPtr->glGetActiveSubroutineUniformiv = (delegate*<uint, uint, uint, uint, int*, void>)load(
                "glGetActiveSubroutineUniformiv"u8);
            contextPtr->glGetProgramStageiv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetProgramStageiv"u8);
            contextPtr->glGetQueryIndexediv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetQueryIndexediv"u8);
            contextPtr->glGetSubroutineIndex = (delegate*<uint, uint, byte*, uint>)load(
                "glGetSubroutineIndex"u8);
            contextPtr->glGetSubroutineUniformLocation = (delegate*<uint, uint, byte*, int>)load(
                "glGetSubroutineUniformLocation"u8);
            contextPtr->glGetUniformSubroutineuiv = (delegate*<uint, int, uint*, void>)load(
                "glGetUniformSubroutineuiv"u8);
            contextPtr->glGetUniformdv = (delegate*<uint, int, double*, void>)load(
                "glGetUniformdv"u8);
            contextPtr->glIsTransformFeedback = (delegate*<uint, bool>)load(
                "glIsTransformFeedback"u8);
            contextPtr->glMinSampleShading = (delegate*<float, void>)load("glMinSampleShading"u8);
            contextPtr->glPatchParameterfv = (delegate*<uint, float*, void>)load(
                "glPatchParameterfv"u8);
            contextPtr->glPatchParameteri = (delegate*<uint, int, void>)load(
                "glPatchParameteri"u8);
            contextPtr->glPauseTransformFeedback = (delegate*<void>)load(
                "glPauseTransformFeedback"u8);
            contextPtr->glResumeTransformFeedback = (delegate*<void>)load(
                "glResumeTransformFeedback"u8);
            contextPtr->glUniform1d = (delegate*<int, double, void>)load("glUniform1d"u8);
            contextPtr->glUniform1dv = (delegate*<int, uint, double*, void>)load("glUniform1dv"u8);
            contextPtr->glUniform2d = (delegate*<int, double, double, void>)load("glUniform2d"u8);
            contextPtr->glUniform2dv = (delegate*<int, uint, double*, void>)load("glUniform2dv"u8);
            contextPtr->glUniform3d = (delegate*<int, double, double, double, void>)load(
                "glUniform3d"u8);
            contextPtr->glUniform3dv = (delegate*<int, uint, double*, void>)load("glUniform3dv"u8);
            contextPtr->glUniform4d = (delegate*<int, double, double, double, double, void>)load(
                "glUniform4d"u8);
            contextPtr->glUniform4dv = (delegate*<int, uint, double*, void>)load("glUniform4dv"u8);
            contextPtr->glUniformMatrix2dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix2dv"u8);
            contextPtr->glUniformMatrix2x3dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix2x3dv"u8);
            contextPtr->glUniformMatrix2x4dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix2x4dv"u8);
            contextPtr->glUniformMatrix3dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix3dv"u8);
            contextPtr->glUniformMatrix3x2dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix3x2dv"u8);
            contextPtr->glUniformMatrix3x4dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix3x4dv"u8);
            contextPtr->glUniformMatrix4dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix4dv"u8);
            contextPtr->glUniformMatrix4x2dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix4x2dv"u8);
            contextPtr->glUniformMatrix4x3dv = (delegate*<int, uint, bool, double*, void>)load(
                "glUniformMatrix4x3dv"u8);
            contextPtr->glUniformSubroutinesuiv = (delegate*<uint, uint, uint*, void>)load(
                "glUniformSubroutinesuiv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.0 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore41(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core41)
            {
                DefaultLog.InternalAppendLog("The core version 4.1 is not supported.");
                return false;
            }

            #region load

            contextPtr->glActiveShaderProgram = (delegate*<uint, uint, void>)load(
                "glActiveShaderProgram"u8);
            contextPtr->glBindProgramPipeline = (delegate*<uint, void>)load(
                "glBindProgramPipeline"u8);
            contextPtr->glClearDepthf = (delegate*<float, void>)load("glClearDepthf"u8);
            contextPtr->glCreateShaderProgramv = (delegate*<uint, uint, byte*, uint>)load(
                "glCreateShaderProgramv"u8);
            contextPtr->glDeleteProgramPipelines = (delegate*<uint, uint*, void>)load(
                "glDeleteProgramPipelines"u8);
            contextPtr->glDepthRangeArrayv = (delegate*<uint, uint, double*, void>)load(
                "glDepthRangeArrayv"u8);
            contextPtr->glDepthRangeIndexed = (delegate*<uint, double, double, void>)load(
                "glDepthRangeIndexed"u8);
            contextPtr->glDepthRangef = (delegate*<float, float, void>)load("glDepthRangef"u8);
            contextPtr->glGenProgramPipelines = (delegate*<uint, uint*, void>)load(
                "glGenProgramPipelines"u8);
            contextPtr->glGetDoublei_v = (delegate*<uint, uint, double*, void>)load(
                "glGetDoublei_v"u8);
            contextPtr->glGetFloati_v = (delegate*<uint, uint, float*, void>)load(
                "glGetFloati_v"u8);
            contextPtr->glGetProgramBinary = (delegate*<uint, uint, uint*, uint*, void*, void>)load(
                "glGetProgramBinary"u8);
            contextPtr->glGetProgramPipelineInfoLog = (delegate*<uint, uint, uint*, byte*, void>)load(
                "glGetProgramPipelineInfoLog"u8);
            contextPtr->glGetProgramPipelineiv = (delegate*<uint, uint, int*, void>)load(
                "glGetProgramPipelineiv"u8);
            contextPtr->glGetShaderPrecisionFormat = (delegate*<uint, uint, int*, int*, void>)load(
                "glGetShaderPrecisionFormat"u8);
            contextPtr->glGetVertexAttribLdv = (delegate*<uint, uint, double*, void>)load(
                "glGetVertexAttribLdv"u8);
            contextPtr->glIsProgramPipeline = (delegate*<uint, bool>)load("glIsProgramPipeline"u8);
            contextPtr->glProgramBinary = (delegate*<uint, uint, void*, uint, void>)load(
                "glProgramBinary"u8);
            contextPtr->glProgramParameteri = (delegate*<uint, uint, int, void>)load(
                "glProgramParameteri"u8);
            contextPtr->glProgramUniform1d = (delegate*<uint, int, double, void>)load(
                "glProgramUniform1d"u8);
            contextPtr->glProgramUniform1dv = (delegate*<uint, int, uint, double*, void>)load(
                "glProgramUniform1dv"u8);
            contextPtr->glProgramUniform1f = (delegate*<uint, int, float, void>)load(
                "glProgramUniform1f"u8);
            contextPtr->glProgramUniform1fv = (delegate*<uint, int, uint, float*, void>)load(
                "glProgramUniform1fv"u8);
            contextPtr->glProgramUniform1i = (delegate*<uint, int, int, void>)load(
                "glProgramUniform1i"u8);
            contextPtr->glProgramUniform1iv = (delegate*<uint, int, uint, int*, void>)load(
                "glProgramUniform1iv"u8);
            contextPtr->glProgramUniform1ui = (delegate*<uint, int, uint, void>)load(
                "glProgramUniform1ui"u8);
            contextPtr->glProgramUniform1uiv = (delegate*<uint, int, uint, uint*, void>)load(
                "glProgramUniform1uiv"u8);
            contextPtr->glProgramUniform2d = (delegate*<uint, int, double, double, void>)load(
                "glProgramUniform2d"u8);
            contextPtr->glProgramUniform2dv = (delegate*<uint, int, uint, double*, void>)load(
                "glProgramUniform2dv"u8);
            contextPtr->glProgramUniform2f = (delegate*<uint, int, float, float, void>)load(
                "glProgramUniform2f"u8);
            contextPtr->glProgramUniform2fv = (delegate*<uint, int, uint, float*, void>)load(
                "glProgramUniform2fv"u8);
            contextPtr->glProgramUniform2i = (delegate*<uint, int, int, int, void>)load(
                "glProgramUniform2i"u8);
            contextPtr->glProgramUniform2iv = (delegate*<uint, int, uint, int*, void>)load(
                "glProgramUniform2iv"u8);
            contextPtr->glProgramUniform2ui = (delegate*<uint, int, uint, uint, void>)load(
                "glProgramUniform2ui"u8);
            contextPtr->glProgramUniform2uiv = (delegate*<uint, int, uint, uint*, void>)load(
                "glProgramUniform2uiv"u8);
            contextPtr->glProgramUniform3d = (delegate*<uint, int, double, double, double, void>)load(
                "glProgramUniform3d"u8);
            contextPtr->glProgramUniform3dv = (delegate*<uint, int, uint, double*, void>)load(
                "glProgramUniform3dv"u8);
            contextPtr->glProgramUniform3f = (delegate*<uint, int, float, float, float, void>)load(
                "glProgramUniform3f"u8);
            contextPtr->glProgramUniform3fv = (delegate*<uint, int, uint, float*, void>)load(
                "glProgramUniform3fv"u8);
            contextPtr->glProgramUniform3i = (delegate*<uint, int, int, int, int, void>)load(
                "glProgramUniform3i"u8);
            contextPtr->glProgramUniform3iv = (delegate*<uint, int, uint, int*, void>)load(
                "glProgramUniform3iv"u8);
            contextPtr->glProgramUniform3ui = (delegate*<uint, int, uint, uint, uint, void>)load(
                "glProgramUniform3ui"u8);
            contextPtr->glProgramUniform3uiv = (delegate*<uint, int, uint, uint*, void>)load(
                "glProgramUniform3uiv"u8);
            contextPtr->glProgramUniform4d = (delegate*<uint, int, double, double, double, double, void>)load(
                "glProgramUniform4d"u8);
            contextPtr->glProgramUniform4dv = (delegate*<uint, int, uint, double*, void>)load(
                "glProgramUniform4dv"u8);
            contextPtr->glProgramUniform4f = (delegate*<uint, int, float, float, float, float, void>)load(
                "glProgramUniform4f"u8);
            contextPtr->glProgramUniform4fv = (delegate*<uint, int, uint, float*, void>)load(
                "glProgramUniform4fv"u8);
            contextPtr->glProgramUniform4i = (delegate*<uint, int, int, int, int, int, void>)load(
                "glProgramUniform4i"u8);
            contextPtr->glProgramUniform4iv = (delegate*<uint, int, uint, int*, void>)load(
                "glProgramUniform4iv"u8);
            contextPtr->glProgramUniform4ui = (delegate*<uint, int, uint, uint, uint, uint, void>)load(
                "glProgramUniform4ui"u8);
            contextPtr->glProgramUniform4uiv = (delegate*<uint, int, uint, uint*, void>)load(
                "glProgramUniform4uiv"u8);
            contextPtr->glProgramUniformMatrix2dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix2dv"u8);
            contextPtr->glProgramUniformMatrix2fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix2fv"u8);
            contextPtr->glProgramUniformMatrix2x3dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix2x3dv"u8);
            contextPtr->glProgramUniformMatrix2x3fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix2x3fv"u8);
            contextPtr->glProgramUniformMatrix2x4dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix2x4dv"u8);
            contextPtr->glProgramUniformMatrix2x4fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix2x4fv"u8);
            contextPtr->glProgramUniformMatrix3dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix3dv"u8);
            contextPtr->glProgramUniformMatrix3fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix3fv"u8);
            contextPtr->glProgramUniformMatrix3x2dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix3x2dv"u8);
            contextPtr->glProgramUniformMatrix3x2fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix3x2fv"u8);
            contextPtr->glProgramUniformMatrix3x4dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix3x4dv"u8);
            contextPtr->glProgramUniformMatrix3x4fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix3x4fv"u8);
            contextPtr->glProgramUniformMatrix4dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix4dv"u8);
            contextPtr->glProgramUniformMatrix4fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix4fv"u8);
            contextPtr->glProgramUniformMatrix4x2dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix4x2dv"u8);
            contextPtr->glProgramUniformMatrix4x2fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix4x2fv"u8);
            contextPtr->glProgramUniformMatrix4x3dv = (delegate*<uint, int, uint, bool, double*, void>)load(
                "glProgramUniformMatrix4x3dv"u8);
            contextPtr->glProgramUniformMatrix4x3fv = (delegate*<uint, int, uint, bool, float*, void>)load(
                "glProgramUniformMatrix4x3fv"u8);
            contextPtr->glReleaseShaderCompiler = (delegate*<void>)load(
                "glReleaseShaderCompiler"u8);
            contextPtr->glScissorArrayv = (delegate*<uint, uint, int*, void>)load(
                "glScissorArrayv"u8);
            contextPtr->glScissorIndexed = (delegate*<uint, int, int, uint, uint, void>)load(
                "glScissorIndexed"u8);
            contextPtr->glScissorIndexedv = (delegate*<uint, int*, void>)load(
                "glScissorIndexedv"u8);
            contextPtr->glShaderBinary = (delegate*<uint, uint*, uint, void*, uint, void>)load(
                "glShaderBinary"u8);
            contextPtr->glUseProgramStages = (delegate*<uint, uint, uint, void>)load(
                "glUseProgramStages"u8);
            contextPtr->glValidateProgramPipeline = (delegate*<uint, void>)load(
                "glValidateProgramPipeline"u8);
            contextPtr->glVertexAttribL1d = (delegate*<uint, double, void>)load(
                "glVertexAttribL1d"u8);
            contextPtr->glVertexAttribL1dv = (delegate*<uint, double*, void>)load(
                "glVertexAttribL1dv"u8);
            contextPtr->glVertexAttribL2d = (delegate*<uint, double, double, void>)load(
                "glVertexAttribL2d"u8);
            contextPtr->glVertexAttribL2dv = (delegate*<uint, double*, void>)load(
                "glVertexAttribL2dv"u8);
            contextPtr->glVertexAttribL3d = (delegate*<uint, double, double, double, void>)load(
                "glVertexAttribL3d"u8);
            contextPtr->glVertexAttribL3dv = (delegate*<uint, double*, void>)load(
                "glVertexAttribL3dv"u8);
            contextPtr->glVertexAttribL4d = (delegate*<uint, double, double, double, double, void>)load(
                "glVertexAttribL4d"u8);
            contextPtr->glVertexAttribL4dv = (delegate*<uint, double*, void>)load(
                "glVertexAttribL4dv"u8);
            contextPtr->glVertexAttribLPointer = (delegate*<uint, int, uint, uint, void*, void>)load(
                "glVertexAttribLPointer"u8);
            contextPtr->glViewportArrayv = (delegate*<uint, uint, float*, void>)load(
                "glViewportArrayv"u8);
            contextPtr->glViewportIndexedf = (delegate*<uint, float, float, float, float, void>)load(
                "glViewportIndexedf"u8);
            contextPtr->glViewportIndexedfv = (delegate*<uint, float*, void>)load(
                "glViewportIndexedfv"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.1 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore42(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core42)
            {
                DefaultLog.InternalAppendLog("The core version 4.2 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindImageTexture = (delegate*<uint, uint, int, bool, int, uint, uint, void>)load(
                "glBindImageTexture"u8);
            contextPtr->glDrawArraysInstancedBaseInstance = (delegate*<uint, int, uint, uint, uint, void>)load(
                "glDrawArraysInstancedBaseInstance"u8);
            contextPtr->glDrawElementsInstancedBaseInstance = (delegate*<uint, uint, uint, void*, uint, uint, void>)load(
                "glDrawElementsInstancedBaseInstance"u8);
            contextPtr->glDrawElementsInstancedBaseVertexBaseInstance = (delegate*<uint, uint, uint, void*, uint, int, uint, void>)load(
                "glDrawElementsInstancedBaseVertexBaseInstance"u8);
            contextPtr->glDrawTransformFeedbackInstanced = (delegate*<uint, uint, uint, void>)load(
                "glDrawTransformFeedbackInstanced"u8);
            contextPtr->glDrawTransformFeedbackStreamInstanced = (delegate*<uint, uint, uint, uint, void>)load(
                "glDrawTransformFeedbackStreamInstanced"u8);
            contextPtr->glGetActiveAtomicCounterBufferiv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetActiveAtomicCounterBufferiv"u8);
            contextPtr->glGetInternalformativ = (delegate*<uint, uint, uint, uint, int*, void>)load(
                "glGetInternalformativ"u8);
            contextPtr->glMemoryBarrier = (delegate*<uint, void>)load("glMemoryBarrier"u8);
            contextPtr->glTexStorage1D = (delegate*<uint, uint, uint, uint, void>)load(
                "glTexStorage1D"u8);
            contextPtr->glTexStorage2D = (delegate*<uint, uint, uint, uint, uint, void>)load(
                "glTexStorage2D"u8);
            contextPtr->glTexStorage3D = (delegate*<uint, uint, uint, uint, uint, uint, void>)load(
                "glTexStorage3D"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.2 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore43(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core43)
            {
                DefaultLog.InternalAppendLog("The core version 4.3 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindVertexBuffer = (delegate*<uint, uint, IntPtr, uint, void>)load(
                "glBindVertexBuffer"u8);
            contextPtr->glClearBufferData = (delegate*<uint, uint, uint, uint, void*, void>)load(
                "glClearBufferData"u8);
            contextPtr->glClearBufferSubData = (delegate*<uint, uint, IntPtr, UIntPtr, uint, uint, void*, void>)load(
                "glClearBufferSubData"u8);
            contextPtr->glCopyImageSubData = (delegate*<uint, uint, int, int, int, int, uint, uint, int, int, int, int, uint, uint, uint, void>)load(
                "glCopyImageSubData"u8);
            contextPtr->glDebugMessageCallback = (delegate*<GLDEBUGPROC, void*, void>)load(
                "glDebugMessageCallback"u8);
            contextPtr->glDebugMessageControl = (delegate*<uint, uint, uint, uint, uint*, bool, void>)load(
                "glDebugMessageControl"u8);
            contextPtr->glDebugMessageInsert = (delegate*<uint, uint, uint, uint, uint, byte*, void>)load(
                "glDebugMessageInsert"u8);
            contextPtr->glDispatchCompute = (delegate*<uint, uint, uint, void>)load(
                "glDispatchCompute"u8);
            contextPtr->glDispatchComputeIndirect = (delegate*<IntPtr, void>)load(
                "glDispatchComputeIndirect"u8);
            contextPtr->glFramebufferParameteri = (delegate*<uint, uint, int, void>)load(
                "glFramebufferParameteri"u8);
            contextPtr->glGetDebugMessageLog = (delegate*<uint, uint, uint*, uint*, uint*, uint*, uint*, byte*, uint>)load(
                "glGetDebugMessageLog"u8);
            contextPtr->glGetFramebufferParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetFramebufferParameteriv"u8);
            contextPtr->glGetInternalformati64v = (delegate*<uint, uint, uint, uint, long*, void>)load(
                "glGetInternalformati64v"u8);
            contextPtr->glGetObjectLabel = (delegate*<uint, uint, uint, uint*, byte*, void>)load(
                "glGetObjectLabel"u8);
            contextPtr->glGetObjectPtrLabel = (delegate*<void*, uint, uint*, byte*, void>)load(
                "glGetObjectPtrLabel"u8);
            contextPtr->glGetPointerv = (delegate*<uint, void**, void>)load("glGetPointerv"u8);
            contextPtr->glGetProgramInterfaceiv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetProgramInterfaceiv"u8);
            contextPtr->glGetProgramResourceIndex = (delegate*<uint, uint, byte*, uint>)load(
                "glGetProgramResourceIndex"u8);
            contextPtr->glGetProgramResourceLocation = (delegate*<uint, uint, byte*, int>)load(
                "glGetProgramResourceLocation"u8);
            contextPtr->glGetProgramResourceLocationIndex = (delegate*<uint, uint, byte*, int>)load(
                "glGetProgramResourceLocationIndex"u8);
            contextPtr->glGetProgramResourceName = (delegate*<uint, uint, uint, uint, uint*, byte*, void>)load(
                "glGetProgramResourceName"u8);
            contextPtr->glGetProgramResourceiv = (delegate*<uint, uint, uint, uint, uint*, uint, uint*, int*, void>)load(
                "glGetProgramResourceiv"u8);
            contextPtr->glInvalidateBufferData = (delegate*<uint, void>)load(
                "glInvalidateBufferData"u8);
            contextPtr->glInvalidateBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void>)load(
                "glInvalidateBufferSubData"u8);
            contextPtr->glInvalidateFramebuffer = (delegate*<uint, uint, uint*, void>)load(
                "glInvalidateFramebuffer"u8);
            contextPtr->glInvalidateSubFramebuffer = (delegate*<uint, uint, uint*, int, int, uint, uint, void>)load(
                "glInvalidateSubFramebuffer"u8);
            contextPtr->glInvalidateTexImage = (delegate*<uint, int, void>)load(
                "glInvalidateTexImage"u8);
            contextPtr->glInvalidateTexSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, void>)load(
                "glInvalidateTexSubImage"u8);
            contextPtr->glMultiDrawArraysIndirect = (delegate*<uint, void*, uint, uint, void>)load(
                "glMultiDrawArraysIndirect"u8);
            contextPtr->glMultiDrawElementsIndirect = (delegate*<uint, uint, void*, uint, uint, void>)load(
                "glMultiDrawElementsIndirect"u8);
            contextPtr->glObjectLabel = (delegate*<uint, uint, uint, byte*, void>)load(
                "glObjectLabel"u8);
            contextPtr->glObjectPtrLabel = (delegate*<void*, uint, byte*, void>)load(
                "glObjectPtrLabel"u8);
            contextPtr->glPopDebugGroup = (delegate*<void>)load("glPopDebugGroup"u8);
            contextPtr->glPushDebugGroup = (delegate*<uint, uint, uint, byte*, void>)load(
                "glPushDebugGroup"u8);
            contextPtr->glShaderStorageBlockBinding = (delegate*<uint, uint, uint, void>)load(
                "glShaderStorageBlockBinding"u8);
            contextPtr->glTexBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load(
                "glTexBufferRange"u8);
            contextPtr->glTexStorage2DMultisample = (delegate*<uint, uint, uint, uint, uint, bool, void>)load(
                "glTexStorage2DMultisample"u8);
            contextPtr->glTexStorage3DMultisample = (delegate*<uint, uint, uint, uint, uint, uint, bool, void>)load(
                "glTexStorage3DMultisample"u8);
            contextPtr->glTextureView = (delegate*<uint, uint, uint, uint, uint, uint, uint, uint, void>)load(
                "glTextureView"u8);
            contextPtr->glVertexAttribBinding = (delegate*<uint, uint, void>)load(
                "glVertexAttribBinding"u8);
            contextPtr->glVertexAttribFormat = (delegate*<uint, int, uint, bool, uint, void>)load(
                "glVertexAttribFormat"u8);
            contextPtr->glVertexAttribIFormat = (delegate*<uint, int, uint, uint, void>)load(
                "glVertexAttribIFormat"u8);
            contextPtr->glClearBufferData = (delegate*<uint, uint, uint, uint, void*, void>)load(
                "glClearBufferData"u8);
            contextPtr->glVertexBindingDivisor = (delegate*<uint, uint, void>)load(
                "glVertexBindingDivisor"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.3 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore44(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core44)
            {
                DefaultLog.InternalAppendLog("The core version 4.4 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindBuffersBase = (delegate*<uint, uint, uint, uint*, void>)load(
                "glBindBuffersBase"u8);
            contextPtr->glBindBuffersRange = (delegate*<uint, uint, uint, uint*, IntPtr*, UIntPtr*, void>)load(
                "glBindBuffersRange"u8);
            contextPtr->glBindImageTextures = (delegate*<uint, uint, uint*, void>)load(
                "glBindImageTextures"u8);
            contextPtr->glBindSamplers = (delegate*<uint, uint, uint*, void>)load(
                "glBindSamplers"u8);
            contextPtr->glBindTextures = (delegate*<uint, uint, uint*, void>)load(
                "glBindTextures"u8);
            contextPtr->glBindVertexBuffers = (delegate*<uint, uint, uint*, IntPtr*, uint*, void>)load(
                "glBindVertexBuffers"u8);
            contextPtr->glBufferStorage = (delegate*<uint, UIntPtr, void*, uint, void>)load(
                "glBufferStorage"u8);
            contextPtr->glClearTexImage = (delegate*<uint, int, uint, uint, void*, void>)load(
                "glClearTexImage"u8);
            contextPtr->glClearTexSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load(
                "glClearTexSubImage"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.4 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore45(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core45)
            {
                DefaultLog.InternalAppendLog("The core version 4.5 is not supported.");
                return false;
            }

            #region load

            contextPtr->glBindTextureUnit = (delegate*<uint, uint, void>)load(
                "glBindTextureUnit"u8);
            contextPtr->glBlitNamedFramebuffer = (delegate*<uint, uint, int, int, int, int, int, int, int, int, uint, uint, void>)load(
                "glBlitNamedFramebuffer"u8);
            contextPtr->glCheckNamedFramebufferStatus = (delegate*<uint, uint, uint>)load(
                "glCheckNamedFramebufferStatus"u8);
            contextPtr->glClearNamedBufferData = (delegate*<uint, uint, uint, uint, void*, void>)load(
                "glClearNamedBufferData"u8);
            contextPtr->glClearNamedBufferSubData = (delegate*<uint, uint, IntPtr, UIntPtr, uint, uint, void*, void>)load(
                "glClearNamedBufferSubData"u8);
            contextPtr->glClearNamedFramebufferfi = (delegate*<uint, uint, int, float, int, void>)load(
                "glClearNamedFramebufferfi"u8);
            contextPtr->glClearNamedFramebufferfv = (delegate*<uint, uint, int, float*, void>)load(
                "glClearNamedFramebufferfv"u8);
            contextPtr->glClearNamedFramebufferiv = (delegate*<uint, uint, int, int*, void>)load(
                "glClearNamedFramebufferiv"u8);
            contextPtr->glClearNamedFramebufferuiv = (delegate*<uint, uint, int, uint*, void>)load(
                "glClearNamedFramebufferuiv"u8);
            contextPtr->glClipControl = (delegate*<uint, uint, void>)load("glClipControl"u8);
            contextPtr->glCompressedTextureSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load(
                "glCompressedTextureSubImage1D"u8);
            contextPtr->glCompressedTextureSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load(
                "glCompressedTextureSubImage2D"u8);
            contextPtr->glCompressedTextureSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load(
                "glCompressedTextureSubImage3D"u8);
            contextPtr->glCopyNamedBufferSubData = (delegate*<uint, uint, IntPtr, IntPtr, UIntPtr, void>)load(
                "glCopyNamedBufferSubData"u8);
            contextPtr->glCopyTextureSubImage1D = (delegate*<uint, int, int, int, int, uint, void>)load(
                "glCopyTextureSubImage1D"u8);
            contextPtr->glCopyTextureSubImage2D = (delegate*<uint, int, int, int, int, int, uint, uint, void>)load(
                "glCopyTextureSubImage2D"u8);
            contextPtr->glCopyTextureSubImage3D = (delegate*<uint, int, int, int, int, int, int, uint, uint, void>)load(
                "glCopyTextureSubImage3D"u8);
            contextPtr->glCreateBuffers = (delegate*<uint, uint*, void>)load("glCreateBuffers"u8);
            contextPtr->glCreateFramebuffers = (delegate*<uint, uint*, void>)load(
                "glCreateFramebuffers"u8);
            contextPtr->glCreateProgramPipelines = (delegate*<uint, uint*, void>)load(
                "glCreateProgramPipelines"u8);
            contextPtr->glCreateQueries = (delegate*<uint, uint, uint*, void>)load(
                "glCreateQueries"u8);
            contextPtr->glCreateRenderbuffers = (delegate*<uint, uint*, void>)load(
                "glCreateRenderbuffers"u8);
            contextPtr->glCreateSamplers = (delegate*<uint, uint*, void>)load(
                "glCreateSamplers"u8);
            contextPtr->glCreateTextures = (delegate*<uint, uint, uint*, void>)load(
                "glCreateTextures"u8);
            contextPtr->glCreateTransformFeedbacks = (delegate*<uint, uint*, void>)load(
                "glCreateTransformFeedbacks"u8);
            contextPtr->glCreateVertexArrays = (delegate*<uint, uint*, void>)load(
                "glCreateVertexArrays"u8);
            contextPtr->glDisableVertexArrayAttrib = (delegate*<uint, uint, void>)load(
                "glDisableVertexArrayAttrib"u8);
            contextPtr->glEnableVertexArrayAttrib = (delegate*<uint, uint, void>)load(
                "glEnableVertexArrayAttrib"u8);
            contextPtr->glFlushMappedNamedBufferRange = (delegate*<uint, IntPtr, UIntPtr, void>)load(
                "glFlushMappedNamedBufferRange"u8);
            contextPtr->glGenerateTextureMipmap = (delegate*<uint, void>)load(
                "glGenerateTextureMipmap"u8);
            contextPtr->glGetCompressedTextureImage = (delegate*<uint, int, uint, void*, void>)load(
                "glGetCompressedTextureImage"u8);
            contextPtr->glGetCompressedTextureSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, void*, void>)load(
                "glGetCompressedTextureSubImage"u8);
            contextPtr->glGetGraphicsResetStatus = (delegate*<uint>)load(
                "glGetGraphicsResetStatus"u8);
            contextPtr->glGetNamedBufferParameteri64v = (delegate*<uint, uint, long*, void>)load(
                "glGetNamedBufferParameteri64v"u8);
            contextPtr->glGetNamedBufferParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetNamedBufferParameteriv"u8);
            contextPtr->glGetNamedBufferPointerv = (delegate*<uint, uint, void**, void>)load(
                "glGetNamedBufferPointerv"u8);
            contextPtr->glGetNamedBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load(
                "glGetNamedBufferSubData"u8);
            contextPtr->glGetNamedFramebufferAttachmentParameteriv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetNamedFramebufferAttachmentParameteriv"u8);
            contextPtr->glGetNamedFramebufferParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetNamedFramebufferParameteriv"u8);
            contextPtr->glGetNamedRenderbufferParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetNamedRenderbufferParameteriv"u8);
            contextPtr->glGetQueryBufferObjecti64v = (delegate*<uint, uint, uint, IntPtr, void>)load(
                "glGetQueryBufferObjecti64v"u8);
            contextPtr->glGetQueryBufferObjectiv = (delegate*<uint, uint, uint, IntPtr, void>)load(
                "glGetQueryBufferObjectiv"u8);
            contextPtr->glGetQueryBufferObjectui64v = (delegate*<uint, uint, uint, IntPtr, void>)load(
                "glGetQueryBufferObjectui64v"u8);
            contextPtr->glGetQueryBufferObjectuiv = (delegate*<uint, uint, uint, IntPtr, void>)load(
                "glGetQueryBufferObjectuiv"u8);
            contextPtr->glGetTextureImage = (delegate*<uint, int, uint, uint, uint, void*, void>)load(
                "glGetTextureImage"u8);
            contextPtr->glGetTextureLevelParameterfv = (delegate*<uint, int, uint, float*, void>)load(
                "glGetTextureLevelParameterfv"u8);
            contextPtr->glGetTextureLevelParameteriv = (delegate*<uint, int, uint, int*, void>)load(
                "glGetTextureLevelParameteriv"u8);
            contextPtr->glGetTextureParameterIiv = (delegate*<uint, uint, int*, void>)load(
                "glGetTextureParameterIiv"u8);
            contextPtr->glGetTextureParameterIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glGetTextureParameterIuiv"u8);
            contextPtr->glGetTextureParameterfv = (delegate*<uint, uint, float*, void>)load(
                "glGetTextureParameterfv"u8);
            contextPtr->glGetTextureParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glGetTextureParameteriv"u8);
            contextPtr->glGetTextureSubImage = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, uint, void*, void>)load(
                "glGetTextureSubImage"u8);
            contextPtr->glGetTransformFeedbacki64_v = (delegate*<uint, uint, uint, long*, void>)load(
                "glGetTransformFeedbacki64_v"u8);
            contextPtr->glGetTransformFeedbacki_v = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetTransformFeedbacki_v"u8);
            contextPtr->glGetTransformFeedbackiv = (delegate*<uint, uint, int*, void>)load(
                "glGetTransformFeedbackiv"u8);
            contextPtr->glGetVertexArrayIndexed64iv = (delegate*<uint, uint, uint, long*, void>)load(
                "glGetVertexArrayIndexed64iv"u8);
            contextPtr->glGetVertexArrayIndexediv = (delegate*<uint, uint, uint, int*, void>)load(
                "glGetVertexArrayIndexediv"u8);
            contextPtr->glGetVertexArrayiv = (delegate*<uint, uint, int*, void>)load(
                "glGetVertexArrayiv"u8);
            contextPtr->glGetnCompressedTexImage = (delegate*<uint, int, uint, void*, void>)load(
                "glGetnCompressedTexImage"u8);
            contextPtr->glGetnTexImage = (delegate*<uint, int, uint, uint, uint, void*, void>)load(
                "glGetnTexImage"u8);
            contextPtr->glGetnUniformdv = (delegate*<uint, int, uint, double*, void>)load(
                "glGetnUniformdv"u8);
            contextPtr->glGetnUniformfv = (delegate*<uint, int, uint, float*, void>)load(
                "glGetnUniformfv"u8);
            contextPtr->glGetnUniformiv = (delegate*<uint, int, uint, int*, void>)load(
                "glGetnUniformiv"u8);
            contextPtr->glGetnUniformuiv = (delegate*<uint, int, uint, uint*, void>)load(
                "glGetnUniformuiv"u8);
            contextPtr->glInvalidateNamedFramebufferData = (delegate*<uint, uint, uint*, void>)load(
                "glInvalidateNamedFramebufferData"u8);
            contextPtr->glInvalidateNamedFramebufferSubData = (delegate*<uint, uint, uint*, int, int, uint, uint, void>)load(
                "glInvalidateNamedFramebufferSubData"u8);
            contextPtr->glMapNamedBuffer = (delegate*<uint, uint, void*>)load(
                "glMapNamedBuffer"u8);
            contextPtr->glMapNamedBufferRange = (delegate*<uint, IntPtr, UIntPtr, uint, void*>)load(
                "glMapNamedBufferRange"u8);
            contextPtr->glMemoryBarrierByRegion = (delegate*<uint, void>)load(
                "glMemoryBarrierByRegion"u8);
            contextPtr->glNamedBufferData = (delegate*<uint, UIntPtr, void*, uint, void>)load(
                "glNamedBufferData"u8);
            contextPtr->glNamedBufferStorage = (delegate*<uint, UIntPtr, void*, uint, void>)load(
                "glNamedBufferStorage"u8);
            contextPtr->glNamedBufferSubData = (delegate*<uint, IntPtr, UIntPtr, void*, void>)load(
                "glNamedBufferSubData"u8);
            contextPtr->glNamedFramebufferDrawBuffer = (delegate*<uint, uint, void>)load(
                "glNamedFramebufferDrawBuffer"u8);
            contextPtr->glNamedFramebufferDrawBuffers = (delegate*<uint, uint, uint*, void>)load(
                "glNamedFramebufferDrawBuffers"u8);
            contextPtr->glNamedFramebufferParameteri = (delegate*<uint, uint, int, void>)load(
                "glNamedFramebufferParameteri"u8);
            contextPtr->glNamedFramebufferReadBuffer = (delegate*<uint, uint, void>)load(
                "glNamedFramebufferReadBuffer"u8);
            contextPtr->glNamedFramebufferRenderbuffer = (delegate*<uint, uint, uint, uint, void>)load(
                "glNamedFramebufferRenderbuffer"u8);
            contextPtr->glNamedFramebufferTexture = (delegate*<uint, uint, uint, int, void>)load(
                "glNamedFramebufferTexture"u8);
            contextPtr->glNamedFramebufferTextureLayer = (delegate*<uint, uint, uint, int, int, void>)load(
                "glNamedFramebufferTextureLayer"u8);
            contextPtr->glNamedRenderbufferStorage = (delegate*<uint, uint, uint, uint, void>)load(
                "glNamedRenderbufferStorage"u8);
            contextPtr->glNamedRenderbufferStorageMultisample = (delegate*<uint, uint, uint, uint, uint, void>)load(
                "glNamedRenderbufferStorageMultisample"u8);
            contextPtr->glReadnPixels = (delegate*<int, int, int, int, uint, uint, uint, void*, void>)load(
                "glReadnPixels"u8);
            contextPtr->glTextureBarrier = (delegate*<void>)load("glTextureBarrier"u8);
            contextPtr->glTextureBuffer = (delegate*<uint, uint, uint, void>)load(
                "glTextureBuffer"u8);
            contextPtr->glTextureBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load(
                "glTextureBufferRange"u8);
            contextPtr->glTextureParameterIiv = (delegate*<uint, uint, int*, void>)load(
                "glTextureParameterIiv"u8);
            contextPtr->glTextureParameterIuiv = (delegate*<uint, uint, uint*, void>)load(
                "glTextureParameterIuiv"u8);
            contextPtr->glTextureParameterf = (delegate*<uint, uint, float, void>)load(
                "glTextureParameterf"u8);
            contextPtr->glTextureParameterfv = (delegate*<uint, uint, float*, void>)load(
                "glTextureParameterfv"u8);
            contextPtr->glTextureParameteri = (delegate*<uint, uint, int, void>)load(
                "glTextureParameteri"u8);
            contextPtr->glTextureParameteriv = (delegate*<uint, uint, int*, void>)load(
                "glTextureParameteriv"u8);
            contextPtr->glTextureStorage1D = (delegate*<uint, uint, uint, uint, void>)load(
                "glTextureStorage1D"u8);
            contextPtr->glTextureStorage2D = (delegate*<uint, uint, uint, uint, uint, void>)load(
                "glTextureStorage2D"u8);
            contextPtr->glTextureStorage2DMultisample = (delegate*<uint, uint, uint, uint, uint, bool, void>)load(
                "glTextureStorage2DMultisample"u8);
            contextPtr->glTextureStorage3D = (delegate*<uint, uint, uint, uint, uint, uint, void>)load(
                "glTextureStorage3D"u8);
            contextPtr->glTextureStorage3DMultisample = (delegate*<uint, uint, uint, uint, uint, uint, bool, void>)load(
                "glTextureStorage3DMultisample"u8);
            contextPtr->glTextureSubImage1D = (delegate*<uint, int, int, uint, uint, uint, void*, void>)load(
                "glTextureSubImage1D"u8);
            contextPtr->glTextureSubImage2D = (delegate*<uint, int, int, int, uint, uint, uint, uint, void*, void>)load(
                "glTextureSubImage2D"u8);
            contextPtr->glTextureSubImage3D = (delegate*<uint, int, int, int, int, uint, uint, uint, uint, uint, void*, void>)load(
                "glTextureSubImage3D"u8);
            contextPtr->glTransformFeedbackBufferBase = (delegate*<uint, uint, uint, void>)load(
                "glTransformFeedbackBufferBase"u8);
            contextPtr->glTransformFeedbackBufferRange = (delegate*<uint, uint, uint, IntPtr, UIntPtr, void>)load(
                "glTransformFeedbackBufferRange"u8);
            contextPtr->glUnmapNamedBuffer = (delegate*<uint, bool>)load("glUnmapNamedBuffer"u8);
            contextPtr->glVertexArrayAttribBinding = (delegate*<uint, uint, uint, void>)load(
                "glVertexArrayAttribBinding"u8);
            contextPtr->glVertexArrayAttribFormat = (delegate*<uint, uint, int, uint, bool, uint, void>)load(
                "glVertexArrayAttribFormat"u8);
            contextPtr->glVertexArrayAttribIFormat = (delegate*<uint, uint, int, uint, uint, void>)load(
                "glVertexArrayAttribIFormat"u8);
            contextPtr->glVertexArrayAttribLFormat = (delegate*<uint, uint, int, uint, uint, void>)load(
                "glVertexArrayAttribLFormat"u8);
            contextPtr->glVertexArrayBindingDivisor = (delegate*<uint, uint, uint, void>)load(
                "glVertexArrayBindingDivisor"u8);
            contextPtr->glVertexArrayElementBuffer = (delegate*<uint, uint, void>)load(
                "glVertexArrayElementBuffer"u8);
            contextPtr->glVertexArrayVertexBuffer = (delegate*<uint, uint, uint, IntPtr, uint, void>)load(
                "glVertexArrayVertexBuffer"u8);
            contextPtr->glVertexArrayVertexBuffers = (delegate*<uint, uint, uint, uint*, IntPtr*, uint*, void>)load(
                "glVertexArrayVertexBuffers"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.5 APIs");
            return true;
        }

        internal static unsafe bool LoadGLcore46(OpenglContext* contextPtr, delegate*<ReadOnlySpan<byte>, IntPtr> load)
        {
            if (!core46)
            {
                DefaultLog.InternalAppendLog("The core version 4.6 is not supported.");
                return false;
            }

            #region load

            contextPtr->glMultiDrawArraysIndirectCount = (delegate*<uint, void*, IntPtr, uint, uint, void>)load(
                "glMultiDrawArraysIndirectCount"u8);
            contextPtr->glMultiDrawElementsIndirectCount = (delegate*<uint, uint, void*, IntPtr, uint, uint, void>)load(
                "glMultiDrawElementsIndirectCount"u8);
            contextPtr->glPolygonOffsetClamp = (delegate*<float, float, float, void>)load(
                "glPolygonOffsetClamp"u8);
            contextPtr->glSpecializeShader = (delegate*<uint, byte*, uint, uint*, uint*, void>)load(
                "glSpecializeShader"u8);

            #endregion load

            DefaultLog.InternalAppendLog("Successfully load all OpenGL 4.6 APIs");
            return true;
        }

    }
}
