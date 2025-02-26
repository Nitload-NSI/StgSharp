//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="CancellableTask.cs"
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Threading
{
    public class CancellableTask:IDisposable
    {
        private protected Action<CancellationToken> _startup;
        private protected Task _internalTask;
        private CancellationTokenSource cts;
        private bool disposedValue;

        private protected CancellableTask()
        {
            cts = new CancellationTokenSource();
        }

        private protected CancellableTask( Action<CancellationToken> action )
        {
            _internalTask = new Task( InternalAction );
            cts = new CancellationTokenSource();
        }

        public bool IsCompleted
        {
            get => _internalTask.IsCompleted;
        }

        public CancellationTokenSource Cts
        {
            get => cts;
        }

        public int Id
        {
            get => _internalTask.Id;
        }

        public void Cancel()
        {
            Cts.Cancel();
        }

        public void CancelAndWait()
        {
            Cts.Cancel();
            _internalTask.Wait();
        }

        public static CancellableTask Run( Action<CancellationToken> startup )
        {
            CancellableTask ret = new CancellableTask { _startup = startup };
            ret._internalTask = Task.Run( ret.InternalAction );
            return ret;
        }

        public void Wait()
        {
            _internalTask.Wait();
        }

        public static void WaitAll( IEnumerable<CancellableTask> tasks )
        {
            IEnumerable<Task> convertedTasks = tasks.Select<CancellableTask, Task>(
                ct => ct._internalTask );
            Task.WaitAll( convertedTasks.ToArray() );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal void InternalAction()
        {
            //Console.WriteLine($"Task {Task.CurrentId} is running.");
            _startup( Cts.Token );
        }

        public static implicit operator Task( CancellableTask task )
        {
            return task._internalTask;
        }

        ~CancellableTask()
        {
            Cts.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsCompleted)
                    {
                        cts.Dispose();
                        _internalTask.Dispose();
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot dispose a cancellable task before completed");
                    }
                }

                disposedValue = true;
            }
        }

        // ~CancellableTask()
        // {
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
