using StgSharp.Geometries;
using StgSharp.Graphics;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal enum LogType
    {
        Error,
        Warning,
        Info,
        InfoError,
        InfoWarning,
    }



    internal static unsafe partial class InternalIO
    {
        internal static IntPtr SSClibPtr = IntPtr.Zero;

        internal const string SS_errorlog =
            "SS_errorlog.log";

        internal const string SSC_libname =
                    "StgSharpC";

        static InternalIO()
        {
            GLtype.Add(typeof(float), GLconst.FLOAT);
            GLtype.Add(typeof(int), GLconst.INT);
            GLtype.Add(typeof(uint), GLconst.UNSIGNED_INT);
            GLtype.Add(typeof(short), GLconst.SHORT);
            GLtype.Add(typeof(ushort), GLconst.UNSIGNED_SHORT);
            GLtype.Add(typeof(sbyte), GLconst.BYTE);
            GLtype.Add(typeof(byte), GLconst.UNSIGNED_BYTE);

        }

        internal static T InternalLoadProc<T>(IntPtr libPtr, string name) where T : Delegate
        {
            try
            {
                IntPtr procPtr = InternalGetWINproc(libPtr, name);
                T ret = Marshal.GetDelegateForFunctionPointer<T>(procPtr);
                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
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

        [DllImport("kernal32.dll", EntryPoint = "FreeLibrary",
            CharSet = CharSet.Ansi)]
        internal extern static IntPtr InternalFreeWINlib(IntPtr libPtr);

        [DllImport("kernel32.dll", EntryPoint = "GetPeocAddress",
            CharSet = CharSet.Ansi)]
        internal extern static IntPtr InternalGetWINproc(IntPtr libPtr, string procName);

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary",
            CharSet = CharSet.Ansi)]
        internal extern static IntPtr InternalLoadWINlib(string name);

        internal static void InternalWriteLog(string beforeTime, string logLine, LogType logType)
        {
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
        }

        internal static void InternalWriteLog(string logLine, LogType logType)
        {
            if (!File.Exists(InternalIO.SS_errorlog))
            {
                FileStream stream = File.Create(InternalIO.SS_errorlog);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(SS_errorlog))
            {
                logStream.WriteLine($"{logType}\t@\t{DateTime.Now.ToString("o")}");

                logStream.WriteLine(logLine);
            }
        }
    }
}
