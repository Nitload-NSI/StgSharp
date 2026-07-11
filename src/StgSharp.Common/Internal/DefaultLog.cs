// -----------------------------------------------------------------------------
// file="DefaultLog"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Internal
{
    internal static unsafe partial class DefaultLog
    {

        private const long MaxLogFileSize = 10 * 1024 * 1024; // 10 MB

        private static LogType _currentLogLevel = LogType.Info;

        internal static readonly GCHandle ssc_logHandle =
            GCHandle.Alloc(new byte[512], GCHandleType.Pinned);

        internal static SemaphoreSlim logSyncSemaphore = new(1, 1);

        internal static void InternalAppendLog(
                             string log
        )
        {
            if (!File.Exists(Native.LogPath))
            {
                FileStream stream = File.Create(Native.LogPath);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(Native.LogPath)) {
                logStream.WriteLine(log);
            }
        }

        internal static void InternalWriteLog(
                             string logLine,
                             LogType logType
        )
        {
            if (logType < _currentLogLevel) {
                return;
            }

            logSyncSemaphore.Wait();
            try
            {
                RotateLogFile();

                if (!File.Exists(Native.LogPath))
                {
                    FileStream stream = File.Create(Native.LogPath);
                    stream.Close();
                }

                using (StreamWriter logStream = File.AppendText(Native.LogPath)) {
                    logStream.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logType}] {logLine}");
                }
            }
            finally
            {
                _ = logSyncSemaphore.Release();
            }
        }

        internal static void InternalWriteLog(
                             string beforeTime,
                             string logLine,
                             LogType logType
        )
        {
            logSyncSemaphore.Wait();
            if (!File.Exists(Native.LogPath))
            {
                FileStream stream = File.Create(Native.LogPath);
                stream.Close();
            }

            using (StreamWriter logStream = File.AppendText(Native.LogPath))
            {
                logStream.WriteLine(beforeTime);
                logStream.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logType}] {logLine}");
            }
            _ = logSyncSemaphore.Release(1);
        }

        internal static void SetLogLevel(
                             LogType logLevel
        )
        {
            _currentLogLevel = logLevel;
        }

        private static void RotateLogFile()
        {
            if (File.Exists(Native.LogPath) && new FileInfo(Native.LogPath).Length > MaxLogFileSize)
            {
                string backupFile = Native.LogPath + ".backup";
                if (File.Exists(backupFile)) {
                    File.Delete(backupFile);
                }
                File.Move(Native.LogPath, backupFile);
            }
        }

    }
}
