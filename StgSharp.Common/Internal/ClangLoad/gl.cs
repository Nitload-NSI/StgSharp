//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="gl.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Internal
{
    internal partial class InternalIO
    {

        [LibraryImport(NativeLibName, EntryPoint = "glCheckShaderStat")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        #pragma warning disable CA5392
        internal static partial int glCheckShaderStatus(
                                    ref OpenglContext context,
                                    uint shaderHandle,
                                    int key,
                                    ref IntPtr logPtr);
        #pragma warning restore CA5392


        #region ssgc api define

        [LibraryImport(NativeLibName, EntryPoint = "initGL")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void InternalInitGL(int majorVersion, int minorVersion);

        [LibraryImport(NativeLibName, EntryPoint = "loadGlfuncDefault", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial IntPtr InternalLoadGlfuncDefault(string name);

        [LibraryImport(NativeLibName, EntryPoint = "linkShaderProgram")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe partial uint InternalLinkShaderProgram(OpenglContext* context, uint shaderProgram);

        [LibraryImport(NativeLibName, EntryPoint = "readLog")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe partial IntPtr InternalReadSSCLog();

        [LibraryImport(NativeLibName, EntryPoint = "loadImageData", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe partial void InternalLoadImage(string fileName, ImageInfo* output, ImageLoader loader);

        [LibraryImport(NativeLibName, EntryPoint = "unloadImageData")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe partial void InternalUnloadImage(ImageInfo* output);

    #endregion ssgc api define
    }
}
