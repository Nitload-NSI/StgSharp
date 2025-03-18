//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Program.cs"
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
using System.Globalization;
using System.IO.Pipes;

using Terminal.Gui;

namespace StgSharpTerminalDialogue
{
    internal static partial class Program
    {

        private static AnonymousPipeClientStream _callerClientPipe, _callerServerPipe;

        private static void Main( string[] args )
        {
            Application.Init();

            Console.CancelKeyPress += ( sender, e ) => {
                e.Cancel = true;
                Application.RequestStop();
            };

            Task renderTask = Task.Factory
                    .StartNew(
                        () => {
                            _dv = DialogueView.Single;
                            Application.Run( view: _dv );
                        } );

            /**/
            if( args.Length != 2 ) {
                Console.ReadLine();
                Application.RequestStop();
            }
            /**/

            _callerClientPipe = new AnonymousPipeClientStream(
                PipeDirection.In, args[ 0 ] );
            _callerServerPipe = new AnonymousPipeClientStream(
                PipeDirection.Out, args[ 1 ] );


            using( StreamReader clientReader = new StreamReader(
                _callerClientPipe ) ) {
                using( StreamWriter clientSender = new StreamWriter(
                    _callerServerPipe ) ) {
                    clientSender.AutoFlush = true;
                    string temp = string.Empty;
                    while( !temp!.Contains( "exit " ) && !renderTask.IsCompleted ) {
                        temp = clientReader.ReadLine()!;
                        temp = temp ?? string.Empty;
                        if( string.IsNullOrEmpty( temp ) ) {
                            continue;
                        }

                        int index = temp.IndexOf( ' ' );
                        if( index == -1 ) {
                            continue;
                        }

                        string command = temp.Substring( 0, index );
                        string operand = temp.Substring( index + 1 );

                        PostOperation( command, operand );
                    }
                }
            }
        }

    }
}
