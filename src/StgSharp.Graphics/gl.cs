// -----------------------------------------------------------------------------
// file="gl"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics
{
    internal static partial class GraphicFramework
    {

        internal static uint[] GLtype = new uint[16];

        static GraphicFramework()
        {
            GLtype[(int)TypeCode.Single] = glConst.FLOAT;
            GLtype[(int)TypeCode.Int32] = glConst.INT;
            GLtype[(int)TypeCode.UInt32] = glConst.UNSIGNED_INT;
            GLtype[(int)TypeCode.Int16] = glConst.SHORT;
            GLtype[(int)TypeCode.UInt16] = glConst.UNSIGNED_SHORT;
            GLtype[(int)TypeCode.SByte] = glConst.BYTE;
            GLtype[(int)TypeCode.Byte] = glConst.UNSIGNED_BYTE;
        }

        [LibraryImport(Native.LibName, EntryPoint = "glCheckShaderStat")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        #pragma warning disable CA5392
        internal static partial int glCheckShaderStatus(
                                    ref OpenglContext context,
                                    uint shaderHandle,
                                    int key,
                                    ref IntPtr logPtr
        );
        #pragma warning restore CA5392


        #region ssgc api define

        [LibraryImport(Native.LibName, EntryPoint = "initGL")]
        [UnmanagedCallConv(CallConvs =[typeof(CallConvCdecl)])]
        internal static partial void InternalInitGL(
                                     int majorVersion,
                                     int minorVersion
        );

        [LibraryImport(
                Native.LibName,
                EntryPoint = "loadGlfuncDefault",
                StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs =[typeof(CallConvCdecl)])]
        internal static partial IntPtr InternalLoadGlFuncDefault(
                                       string name
        );

        [LibraryImport(Native.LibName, EntryPoint = "linkShaderProgram")]
        [UnmanagedCallConv(CallConvs =[typeof(CallConvCdecl)])]
        internal static unsafe partial uint InternalLinkShaderProgram(
                                            OpenglContext* context,
                                            uint shaderProgram
        );

        [LibraryImport(Native.LibName, EntryPoint = "readLog")]
        [UnmanagedCallConv(CallConvs =[typeof(CallConvCdecl)])]
        internal static unsafe partial IntPtr InternalReadSSCLog();

        [LibraryImport(
                Native.LibName,
                EntryPoint = "loadImageData",
                StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs =[typeof(CallConvCdecl)])]
        internal static unsafe partial void InternalLoadImage(
                                            string fileName,
                                            ImageInfo* output,
                                            ImageLoader loader
        );

        [LibraryImport(Native.LibName, EntryPoint = "unloadImageData")]
        [UnmanagedCallConv(CallConvs =[typeof(CallConvCdecl)])]
        internal static unsafe partial void InternalUnloadImage(
                                            ImageInfo* output
        );

    #endregion ssgc api define
    }
}
