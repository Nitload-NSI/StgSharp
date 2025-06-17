//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Dialogue.cs"
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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StgSharp
{
    public static class Dialogue
    {

        private static Process? _dialogueProcess;
        private static AnonymousPipeServerStream _dialogueClientPipe,_dialogueServerPipe;
        private static ConcurrentBag<string> msgCache = new ConcurrentBag<string>();
        private static int _readAndWriteThreadId;
        private static ProcessStartInfo _dialogueProcessInfo;
        private static StreamReader _dr;
        private static StreamWriter _dw;

        public static bool NeedShowWhenStartup { get; set; }

        public static void PostError( Exception ex )
        {
            CreateDialogueProcessIfNotExist();

            _dw.WriteLine( $"posterror {ex.Message}" );

            _dialogueProcess!.Exited -= DialogueProcessExitCallback;
        }

        public static void SetReadAndWriteThread( Thread thread )
        {
            _readAndWriteThreadId = thread.ManagedThreadId;
        }

        internal static void CreateDialogueProcessIfNotExist()
        {
            if( _dialogueProcess != null ) {
                return;
            }
            if( !File.Exists( _dialogueProcessInfo.FileName ) ) { }
            _dialogueProcess = Process.Start( _dialogueProcessInfo );

            _dr = new StreamReader( _dialogueClientPipe );
            _dw = new StreamWriter( _dialogueServerPipe );

            _dialogueProcess!.Exited += DialogueProcessExitCallback;

            _dialogueServerPipe.DisposeLocalCopyOfClientHandle();
            _dialogueClientPipe.DisposeLocalCopyOfClientHandle();
        }

        internal static void LoadDialogue()
        {
            _dialogueServerPipe = new AnonymousPipeServerStream(
                PipeDirection.Out, HandleInheritability.Inheritable );
            _dialogueClientPipe = new AnonymousPipeServerStream(
                PipeDirection.In, HandleInheritability.Inheritable );

            string route = Assembly.GetAssembly( typeof( World ) ).Location;
            string path = Path.GetDirectoryName( route );

            string exeName;

            if( RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
            {
                exeName = ".exe";
            } else if( RuntimeInformation.IsOSPlatform( OSPlatform.Linux ) ||
                       RuntimeInformation.IsOSPlatform( OSPlatform.OSX ) )
            {
                exeName = string.Empty;
            } else
            {
                throw new PlatformNotSupportedException( "Unknown OS platform" );
            }

            string consoleRoute = Path.Combine(
                path, @"StgSharpTerminalDialogue", $@"StgSharpTerminalDialogue{exeName}" );

            _dialogueProcessInfo = new ProcessStartInfo
            {
                FileName = consoleRoute,
                Arguments = $"{_dialogueServerPipe.GetClientHandleAsString()} {_dialogueClientPipe.GetClientHandleAsString()}",
                UseShellExecute = true,
                RedirectStandardOutput = false,
            };
        }

        internal static void PostMessage( string instruction, params string[] operands )
        {
            CreateDialogueProcessIfNotExist();

            _dw.WriteLine( $"{instruction} {operands}" );
        }

        internal static void UnloadDialogue()
        {
            if( _dialogueProcess != null ) {
                _dialogueProcess.Kill();
            }
            _dialogueProcess = null;

            _dialogueServerPipe.Dispose();
            _dialogueClientPipe.Dispose();
        }

        internal static string WaitCertainReturn( string target )
        {
            if( _dialogueProcess == null ) {
                return string.Empty;
            }

            string temp = string .Empty;
            do
            {
                temp = _dr.ReadLine();
                temp = temp ?? string.Empty;
                if( !temp!.StartsWith( target ) )
                {
                    break;
                }
            } while (true);

            int index = temp.IndexOf( ' ' );
            return index != -1 ? temp.Substring( index + 1 ) : string.Empty;
        }

        private static void AssertThread()
        {
            if( _readAndWriteThreadId != Environment.CurrentManagedThreadId ) {
                throw new InvalidOperationException(
                    "Communication with dialogue can only call from certain thread." );
            }
        }

        private static void DialogueProcessExitCallback( object? sender, EventArgs e )
        {
            _dialogueProcess = null;
            _dr.Close();
            _dw.Close();
        }

    }
}
