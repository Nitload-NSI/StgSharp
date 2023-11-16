using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{

    public static unsafe partial class SSE
    {
        public static int Vector4Size = 16;

        internal static IntPtr defaultVector4Ptr = 
            InternalIO.set_Vector4ptr_default_internal();


        internal static IntPtr defaultM256Ptr;


        public enum Registor : byte
        {
            XMM0 = 0,
            XMM1 = 1,
            XMM2 = 2,
            XMM3 = 3,
            XMM4 = 4,
            XMM5 = 5,
            XMM6 = 6,
            XMM7 = 7
        }

        static SSE()
        {
        }

    }
}
