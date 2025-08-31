//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance.Memory;

using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    internal static class MatrixParallel
    {

        private static Thread[] _threads;
        private static Channel<IntPtr> _taskChannel = Channel.CreateUnbounded<IntPtr>();
        private static int _paraState;

        public static void Init()
        {
            int count = Environment.ProcessorCount - 4;
            count = count < 0 ? 1 : count;
            _threads = new Thread[count];
            Interlocked.Exchange(ref _paraState, 1);
            for (int i = 0; i < count; i++)
            {
                _threads[i] = new(MatrixParallelCycle)
                {
                    IsBackground = true
                };
                _threads[i].Start();
            }
        }

        public static void PublishTask() { }

        public static void Terminate()
        {
            _ = Interlocked.Exchange(ref _paraState, 0);
        }

        private static unsafe void MatrixParallelCycle()
        {
            Thread t_cur = Thread.CurrentThread;
            while (_taskChannel.Reader.TryRead(out IntPtr ptr))
            {
                MatrixParallelTask* taskHandle = (MatrixParallelTask*)ptr;

                // Execute the task directly
                taskHandle->Execute();
            }
        }

    }
}
