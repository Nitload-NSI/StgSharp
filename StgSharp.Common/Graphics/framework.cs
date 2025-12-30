//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="framework"
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
using System;
using System.Runtime.InteropServices;

#if Windows
// using Windows.Win32;
#endif

namespace StgSharp.Graphics
{
    /// <summary>
    ///   Function handler to load an opengl function by searching its _label.
    /// </summary>
    /// <param _label="name">
    ///   the _label of the Opengl function
    /// </param>
    /// <returns>
    ///   An Intptr value representing the pointer to the function
    /// </returns>
    public delegate IntPtr glLoader(
                           string name
    );

    #region StgSharpDele

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void FrameBufferSizeHandler(
                                IntPtr window,
                                int width,
                                int height
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void FramePositionHandler(
                                IntPtr window,
                                float width,
                                float height
    );

    #endregion


    #region glDEBUG

    internal unsafe delegate void GLDEBUGPROC(
                                  int source,
                                  int type,
                                  uint id,
                                  int severity,
                                  int length,
                                  sbyte* message,
                                  void* userParam
    );
    internal unsafe delegate void GLDEBUGPROCARB(
                                  int source,
                                  int type,
                                  uint id,
                                  int severity,
                                  int length,
                                  sbyte* message,
                                  void* userParam
    );
    internal unsafe delegate void GLDEBUGPROCKHR(
                                  int source,
                                  int type,
                                  uint id,
                                  int severity,
                                  int length,
                                  sbyte* message,
                                  void* userParam
    );
    internal unsafe delegate void GLDEBUGPROCAMD(
                                  uint id,
                                  int category,
                                  int severity,
                                  int length,
                                  sbyte* message,
                                  void* userParam
    );

#endregion

}
