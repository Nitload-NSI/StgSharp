// -----------------------------------------------------------------------------
// file="Platform"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal enum GraphicAPI : byte
    {

        GL,
        VK

    }

    public static partial class World
    {

        /// <summary>
        ///   Version of current World platform. Main version represent in hundred's digit, and sub
        ///   version represent in ten's digit.
        /// </summary>
        public const long version = 430;

        private static bool vsyncActivated;

        private static uint markCount = 0;
        internal static GraphicAPI API = default;

        public static ModuleToInitializeCollection Initialize()
        {
            return new ModuleToInitializeCollection(new Initializer());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(
                           Exception ex
        )
        {
            DefaultLog.InternalWriteLog(ex.Message, LogType.Error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogInfo(
                           string log
        )
        {
            DefaultLog.InternalWriteLog(log, LogType.Info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(
                           string log
        )
        {
            DefaultLog.InternalWriteLog(log, LogType.Warning);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(
                           Exception notVerySeriesException
        )
        {
            DefaultLog.InternalWriteLog(notVerySeriesException.Message, LogType.Warning);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Mark(
                           bool needConsole
        )
        {
            markCount++;
            if (needConsole) {
                Console.WriteLine(markCount);
            }
        }

        public static void Terminate(
                           int exitCode
        )
        {
            MainTimeProvider.StopProvidingTime();

            // GraphicFramework.glfwTerminate();
            Environment.Exit(exitCode);
        }

        public static void Track(
                           object target,
                           string message,
                           bool isConsole,
                           [CallerFilePath] string filePath = "",
                           [CallerMemberName] string callerName = "",
                           [CallerLineNumber] int lineNumber = 0
        )
        {
            Type t = target.GetType();
            string log = $"Tracked object: {nameof(target)}\t\tType:{t.Name}\n";
            FieldInfo[] fieldInfo = t.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            log += "|\tFieldName\t|\tFieldType\t|\tFieldValue\t|\n";
            foreach (FieldInfo field in fieldInfo) {
                log += $"|{field.Name}\t\t|{field.FieldType}\t\t|{field.GetValue(target)}\t\t|\n";
            }
            log += $"Tracking at:\n{filePath}\t{callerName}\tline {lineNumber}\n";
            log += $"Additional information:\n{message}\n";
            if (isConsole)
            {
                Console.WriteLine(log);
            } else
            {
                DefaultLog.InternalWriteLog(log, LogType.Info);
            }
        }

        /// <summary>
        ///   Write a log message to log file, and mark the _level of this message.
        /// </summary>
        /// <param _label="message">
        ///   A string contains information of how current evironment runs.
        /// </param>
        /// <param _label="logType">
        ///   The severity of current message.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLog(
                           string message,
                           LogType logType
        )
        {
            DefaultLog.InternalWriteLog(message, logType);
        }

        internal static TDele ConvertAPI<TDele>(
                              IntPtr funcPtr
        )
        {
            if (funcPtr == IntPtr.Zero) {
                throw new InvalidCastException(
                    "Failed to convert api, the function pointer is zero.");
            }
            try
            {
                return Marshal.GetDelegateForFunctionPointer<TDele>(funcPtr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static Delegate ConvertAPI(
                                 IntPtr funcPtr,
                                 Type T
        )
        {
            if (funcPtr == IntPtr.Zero) {
                throw new InvalidCastException(
                    "Failed to convert api, the function pointer is zero.");
            }
            try
            {
                return Marshal.GetDelegateForFunctionPointer(funcPtr, T);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static unsafe void InternalInitialize()
        {
            // Initialize time provider and start providing time
            MainTimeProvider.StartProvidingTime();
            DefaultLog.InternalAppendLog("\n\n\n");
            DefaultLog.InternalWriteLog(
                $"Program {Assembly.GetEntryAssembly()!.FullName} on {Environment.MachineName} Started.", LogType.Info);
            MainThreadID = System.Environment.CurrentManagedThreadId;

            /**/
            Dialogue.NeedShowWhenStartup = false;
            Dialogue.LoadDialogue();
            if (Dialogue.NeedShowWhenStartup) {
                Dialogue.CreateDialogueProcessIfNotExist();
            }

            Numa.Initialize();
            /**/
        }

        internal class Initializer : IStaticModule
        {

            public string ModuleName => "StgSharpCore";

            public void InitializeModule(
                        IModuleInitializeProfile profile
            )
            {
                World.InternalInitialize();
            }

            public void UninitializeModule()
            {
                return;
            }

        }

    }
}
