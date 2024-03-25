//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="InternalIO.cs"
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
using StgSharp.Geometries;
using StgSharp.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

#if Windows

#endif

namespace StgSharp
{
    public enum LogType
    {
        Error,
        Warning,
        Info,
        InfoError,
        InfoWarning,
    }

    internal static unsafe partial class InternalIO
    {

        internal const string SS_errorlog =
            "SS_errorlog.log";

        internal const string SSC_libname =
                    "StgSharpC";

        internal static Dictionary<TypeCode, uint> GLtype =
            new Dictionary<TypeCode, uint>();

        internal static SemaphoreSlim logSyncSemaphore = new SemaphoreSlim(1, 1);

        internal static readonly GCHandle ssc_logHandle =
            GCHandle.Alloc(new byte[512], GCHandleType.Pinned);

        internal static IntPtr SSClibPtr = IntPtr.Zero;

        static InternalIO()
        {
            GLtype.Add(TypeCode.Single, GLconst.FLOAT);
            GLtype.Add(TypeCode.Int32, GLconst.INT);
            GLtype.Add(TypeCode.UInt32, GLconst.UNSIGNED_INT);
            GLtype.Add(TypeCode.Int16, GLconst.SHORT);
            GLtype.Add(TypeCode.UInt16, GLconst.UNSIGNED_SHORT);
            GLtype.Add(TypeCode.SByte, GLconst.BYTE);
            GLtype.Add(TypeCode.Byte, GLconst.UNSIGNED_BYTE);
        }

        internal static void InternalAppendLog(string log)
        {
            if (!File.Exists(SS_errorlog))
            {
                FileStream stream = File.Create(SS_errorlog);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(SS_errorlog))
            {
                logStream.WriteLine(log);
            }
        }

        [Obsolete("Not completed", true)]
        internal static IntPtr InternalLoadProc<T>(IntPtr lib, string name) where T : Delegate
        {
            try
            {
                IntPtr procPtr =
#if Windows
                    IntPtr.Zero;
#elif Linux

#endif
                return procPtr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void InternalWriteLog(string logLine, LogType logType)
        {
            logSyncSemaphore.Wait();
            if (!File.Exists(SS_errorlog))
            {
                FileStream stream = File.Create(SS_errorlog);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(SS_errorlog))
            {
                logStream.WriteLine($"{logType}\t@\t{DateTime.Now.ToString("o")}");
                logStream.WriteLine(logLine);
            }
            logSyncSemaphore.Release(1);
        }

        internal static void InternalWriteLog(string beforeTime, string logLine, LogType logType)
        {
            logSyncSemaphore.Wait();
            if (!File.Exists(InternalIO.SS_errorlog))
            {
                FileStream stream = File.Create(InternalIO.SS_errorlog);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(SS_errorlog))
            {
                logStream.WriteLine(beforeTime);
                logStream.WriteLine($"{logType}\t@\t{DateTime.Now.ToString("o")}");
                logStream.WriteLine(logLine);
            }
            logSyncSemaphore.Release(1);
        }

    }
}