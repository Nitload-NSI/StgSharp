//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="InternalIO"
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
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using System.Text;
using System.Threading;

namespace StgSharp.Internal
{
    internal static unsafe partial class InternalIO
    {

        private const long MaxLogFileSize = 10 * 1024 * 1024; // 10 MB

        internal const string NativeLibName =
            "StgSharp.Native";

        internal const string SS_errorLog =
            "SS_errorlog.log";

        private static LogType _currentLogLevel = LogType.Info;

        internal static Dictionary<TypeCode, uint> GLtype = [];

        internal static readonly GCHandle ssc_logHandle =
            GCHandle.Alloc(new byte[512], GCHandleType.Pinned);

        internal static IntPtr _nativeLibPtr = IntPtr.Zero;

        internal static SemaphoreSlim logSyncSemaphore = new(1, 1);

        static InternalIO()
        {
            GLtype.Add(TypeCode.Single, glConst.FLOAT);
            GLtype.Add(TypeCode.Int32, glConst.INT);
            GLtype.Add(TypeCode.UInt32, glConst.UNSIGNED_INT);
            GLtype.Add(TypeCode.Int16, glConst.SHORT);
            GLtype.Add(TypeCode.UInt16, glConst.UNSIGNED_SHORT);
            GLtype.Add(TypeCode.SByte, glConst.BYTE);
            GLtype.Add(TypeCode.Byte, glConst.UNSIGNED_BYTE);
        }

        internal static void InternalAppendLog(string log)
        {
            if (!File.Exists(SS_errorLog))
            {
                FileStream stream = File.Create(SS_errorLog);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(SS_errorLog)) {
                logStream.WriteLine(log);
            }
        }

        internal static void InternalWriteLog(string logLine, LogType logType)
        {
            if (logType < _currentLogLevel) {
                return;
            }

            logSyncSemaphore.Wait();
            try
            {
                RotateLogFile();

                if (!File.Exists(SS_errorLog))
                {
                    FileStream stream = File.Create(SS_errorLog);
                    stream.Close();
                }

                using (StreamWriter logStream = File.AppendText(SS_errorLog)) {
                    logStream.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logType}] {logLine}");
                }
            }
            finally
            {
                logSyncSemaphore.Release();
            }
        }

        internal static void InternalWriteLog(string beforeTime, string logLine, LogType logType)
        {
            logSyncSemaphore.Wait();
            if (!File.Exists(SS_errorLog))
            {
                FileStream stream = File.Create(SS_errorLog);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(SS_errorLog))
            {
                logStream.WriteLine(beforeTime);
                logStream.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logType}] {logLine}");
            }
            _ = logSyncSemaphore.Release(1);
        }

        internal static void SetLogLevel(LogType logLevel)
        {
            _currentLogLevel = logLevel;
        }

        private static void RotateLogFile()
        {
            if (File.Exists(SS_errorLog) && new FileInfo(SS_errorLog).Length > MaxLogFileSize)
            {
                string backupFile = SS_errorLog + ".backup";
                if (File.Exists(backupFile)) {
                    File.Delete(backupFile);
                }
                File.Move(SS_errorLog, backupFile);
            }
        }

    }
}
