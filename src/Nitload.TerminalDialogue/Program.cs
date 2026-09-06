// -----------------------------------------------------------------------------
// file="Program"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Gui;

namespace StgSharpTerminalDialogue
{
    internal static partial class Program
    {

        private static AnonymousPipeClientStream _callerClientPipe, _callerServerPipe;

        private static void Main(
                            string[] args
        )
        {
            Application.Init();

            Console.CancelKeyPress += (
                                      sender,
                                      e
            ) => {
                e.Cancel = true;
                Application.RequestStop();
            };

            Task renderTask = Task.Factory
                                  .StartNew((
                                  ) => {
                                      _dv = DialogueView.Single;
                                      Application.Run(view:_dv);
                                  }, CancellationToken.None, TaskCreationOptions.None,
                                            TaskScheduler.Default);

            /**/
            if (args.Length != 2)
            {
                Console.ReadLine();
                Application.RequestStop();
            }
            /**/

            _callerClientPipe = new AnonymousPipeClientStream(
                PipeDirection.In, args[0]);
            _callerServerPipe = new AnonymousPipeClientStream(
                PipeDirection.Out, args[1]);


            using (StreamReader clientReader = new StreamReader(
                _callerClientPipe))
            {
                using (StreamWriter clientSender = new StreamWriter(
                    _callerServerPipe))
                {
                    clientSender.AutoFlush = true;
                    string temp = string.Empty;
                    while (!temp!.Contains("exit ") && !renderTask.IsCompleted)
                    {
                        temp = clientReader.ReadLine()!;
                        temp = temp ?? string.Empty;
                        if (string.IsNullOrEmpty(temp))
                        {
                            continue;
                        }

                        int index = temp.IndexOf(' ');
                        if (index == -1)
                        {
                            continue;
                        }

                        string command = temp[..index];
                        string operand = temp[(index + 1)..];

                        PostOperation(command, operand);
                    }
                }
            }
        }

    }
}
