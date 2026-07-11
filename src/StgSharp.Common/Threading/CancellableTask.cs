// -----------------------------------------------------------------------------
// file="CancellableTask"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Threading
{
    public class CancellableTask : IDisposable
    {

        private bool disposedValue;
        private CancellationTokenSource cts;
        private protected Action<CancellationToken> _startup;
        private protected Task _internalTask;

        private protected CancellableTask()
        {
            cts = new CancellationTokenSource();
        }

        private protected CancellableTask(
                          Action<CancellationToken> action
        )
        {
            _internalTask = new Task(InternalAction);
            cts = new CancellationTokenSource();
        }

        public bool IsCompleted => _internalTask.IsCompleted;

        public CancellationTokenSource Cts => cts;

        public int Id => _internalTask.Id;

        public void Cancel()
        {
            Cts.Cancel();
        }

        public void CancelAndWait()
        {
            Cts.Cancel();
            _internalTask.Wait();
        }

        // ~CancellableTask()
        // {
        // Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public static CancellableTask Run(
                                      Action<CancellationToken> startup
        )
        {
            CancellableTask ret = new CancellableTask
            {
                _startup = startup
            };
            ret._internalTask = Task.Run(ret.InternalAction);
            return ret;
        }

        public void Wait()
        {
            _internalTask.Wait();
        }

        public static void WaitAll(
                           IEnumerable<CancellableTask> tasks
        )
        {
            IEnumerable<Task> convertedTasks = tasks.Select<CancellableTask, Task>(
                ct => ct._internalTask);
            Task.WaitAll(convertedTasks.ToArray());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InternalAction()
        {
            _startup(Cts.Token);
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsCompleted)
                    {
                        cts.Dispose();
                        _internalTask.Dispose();
                    } else
                    {
                        throw new InvalidOperationException(
                            "Cannot dispose a cancellable task before completed");
                    }
                }

                disposedValue = true;
            }
        }

        public static implicit operator Task(
                                        CancellableTask task
        )
        {
            return task._internalTask;
        }

        ~CancellableTask()
        {
            Cts.Dispose();
        }

    }
}
