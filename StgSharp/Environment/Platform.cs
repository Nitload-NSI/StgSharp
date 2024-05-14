//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Platform.cs"
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
// using StgSharp.Envitronment;
// using StgSharp.Envitronment.Win32;
// using StgSharp.Graphics;
// using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if Windows

// using Windows.Win32.UI.WindowsAndMessaging;
// using Windows.Win32;
// using Windows.Win32.Foundation;
// using Windows.Win32.Graphics.OpenGL;
// using Windows.Win32.Graphics.Gdi;
// using Windows.Win32.UI;

#endif

namespace StgSharp
{
    internal enum GraphicAPI : byte
    {
        GL,
        DX,
        VK
    }

    public static partial class StgSharp
    {

        /// <summary>
        /// Version of current StgSharp platform.
        /// Main version represent in hundred's digit,
        /// and sub version represent in ten's digit.
        /// </summary>
        public const long version = 430;

        private static uint markCount = 0;
        private static bool vsyncActivated;
        internal static GraphicAPI API = default;

        public static void Init()
        {
            mainTimeProvider = new StgSharpTime();
            MainTimeProvider.StartProvidingTime();
            InternalIO.glfwInit();
            InternalIO.InternalAppendLog("\n\n\n");
            InternalIO.InternalWriteLog(
                $"Program {Assembly.GetEntryAssembly()!.FullName} Started.",
                LogType.Info);
        }

        public static void Mark(bool needConsole)
        {
            markCount++;
            if (needConsole)
            {
                Console.WriteLine(markCount);
            }
        }

        public static void Terminate()
        {
            MainTimeProvider.Terminate();
            InternalIO.glfwTerminate();
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
            FieldInfo[] fieldInfo = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            log += "|\tFieldName\t|\tFieldType\t|\tFieldValue\t|\n";
            foreach (FieldInfo field in fieldInfo)
            {
                log += $"|{field.Name}\t\t|{field.FieldType}\t\t|{field.GetValue(target)}\t\t|\n";
            }
            log += $"Tracking at:\n{filePath}\t{callerName}\tline {lineNumber}\n";
            log += $"Additional information:\n{message}\n";
            if (isConsole)
            {
                Console.WriteLine(log);
            }
            else
            {
                InternalIO.InternalWriteLog(log, LogType.Info);
            }
        }

        /// <summary>
        /// Write a log message to log file, and mark the level of this message.
        /// </summary>
        /// <param name="message">A string contains information of how current evironment runs.</param>
        /// <param name="logType">The severity of current message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLog(string message, LogType logType)
        {
            InternalIO.InternalWriteLog(message, logType);
        }

        internal static TDele ConvertAPI<TDele>(IntPtr funcPtr)
        {
            if (funcPtr == IntPtr.Zero)
            {
                throw new Exception("Failed to convert api, the function pointer is zero.");
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

        internal static Delegate ConvertAPI(IntPtr funcPtr, Type T)
        {
            if (funcPtr == IntPtr.Zero)
            {
                throw new Exception("Failed to convert api, the function pointer is zero.");
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

    }
}