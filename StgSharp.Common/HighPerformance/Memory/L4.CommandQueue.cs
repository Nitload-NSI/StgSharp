//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.CommandQueue"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class L4
    {

        private CommandQueue _commandQueue;

        private struct CommandQueue
        {

            private CommandCache* ReadCache;

            private int _curRead;
            private int _curWrite;

            /// <summary>
            ///   Try get a next command to execute
            /// </summary>
            /// <param name="command">
            ///   pointer to next command
            /// </param>
            /// <returns>
            ///   True if there is a command to execute, otherwise false
            /// </returns>
            public bool TryDequeue(out nint command)
            {
                if (_curRead >= 64)
                {
                    // next cache
                    CommandCache* next = ReadCache->Next;
                }

                // current cache
                command = default;
                return true;
            }

        }

        private struct CommandCache
        {

            public CommandCache* Next;

            public CommandCache* Prev;
            public CommandArray Commands;

        }

        [InlineArray(64)]
        private struct CommandArray
        {

            public Command Command0;

        }

        private struct Command
        {

            public int CommandType;
            public nint Destination;
            public nint Source;

        }

    }
}
