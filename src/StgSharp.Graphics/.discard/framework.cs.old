// -----------------------------------------------------------------------------
// file="framework"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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
