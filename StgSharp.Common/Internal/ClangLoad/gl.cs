//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="gl.cs"
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
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Internal.OpenGL;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Internal
{
    internal partial class InternalIO
    {

        [DllImport(
                SSC_libName,
                EntryPoint = "glCheckShaderStat",
                CallingConvention = CallingConvention.Cdecl )]
        #pragma warning disable CA5392
        internal static extern int glCheckShaderStatus(
                                           ref OpenglContext context,
                                           uint shaderHandle,
                                           int key,
                                           ref IntPtr logPtr );
        #pragma warning restore CA5392


        #region ssgc api define

        [DllImport(
                SSC_libName,
                EntryPoint = "initGL",
                CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InternalInitGL(
                                            int majorVersion,
                                            int minorVersion );

        [DllImport(
                SSC_libName,
                EntryPoint = "loadGlfuncDefault",
                CallingConvention = CallingConvention.Cdecl,
                CharSet = CharSet.Ansi )]
        internal static extern IntPtr InternalLoadGlfuncDefault( string name );

        [DllImport(
                SSC_libName,
                EntryPoint = "linkShaderProgram",
                CallingConvention = CallingConvention.Cdecl )]
        internal static extern unsafe uint InternalLinkShaderProgram(
                                                   OpenglContext* context,
                                                   uint shaderProgram );

        [DllImport(
                SSC_libName,
                EntryPoint = "readLog",
                CallingConvention = CallingConvention.Cdecl )]
        internal static extern unsafe IntPtr InternalReadSSCLog();

        [DllImport(
                SSC_libName,
                EntryPoint = "loadImageData",
                CallingConvention = CallingConvention.Cdecl,
                CharSet = CharSet.Ansi )]
        internal static extern unsafe void InternalLoadImage(
                                                   string fileName,
                                                   ImageInfo* output,
                                                   ImageLoader loader );

        [DllImport(
                SSC_libName,
                EntryPoint = "unloadImageData",
                CallingConvention = CallingConvention.Cdecl,
                CharSet = CharSet.Ansi )]
        internal static extern unsafe void InternalUnloadImage(
                                                   ImageInfo* output );

    #endregion ssgc api define
    }
}
