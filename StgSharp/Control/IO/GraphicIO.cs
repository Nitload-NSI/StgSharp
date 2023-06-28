using StgSharp.Control;
using StgSharp.Graphics;
using System;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal static partial class internalIO
    {
        [DllImport("StgSharpGraphic.dll",EntryPoint = "loadGLApi", 
            CallingConvention = CallingConvention.Cdecl,CharSet =CharSet.Ansi)]
        internal static extern GraphicIOPack InternalInitGraphicApi();

        [DllImport("StgSharpGraphic.dll",EntryPoint = "initGL", 
            CallingConvention = CallingConvention.Cdecl,CharSet = CharSet.Ansi)]
        internal static extern unsafe void InternalInitGL();

        [DllImport("StgSharpGraphic.dll",EntryPoint = "initGLAD",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern unsafe int InternalInitGLAD();


        //
    }
}
