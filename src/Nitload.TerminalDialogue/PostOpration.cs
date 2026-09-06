// -----------------------------------------------------------------------------
// file="PostOpration"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.IO;

namespace StgSharpTerminalDialogue
{
    internal static partial class Program
    {

        private static DialogueView _dv;

        internal static void PostOperation(
                             string instruction,
                             string operand
        )
        {
            switch (instruction)
            {
                case "posterror":
                    _dv.ShowError(operand);
                    using (StreamWriter sw = new StreamWriter(
                        _callerServerPipe)) {
                        sw.WriteLine("ERRORGET");
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
