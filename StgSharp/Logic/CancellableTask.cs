using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    internal class CancellableTask
    {
        private CancellationTokenSource cts;
        private protected Task taskInternal;
        private protected Action<CancellationToken> _startup;

        public CancellationTokenSource Cts
        {
            get => cts;
        }

        public int Id
        {
            get =>taskInternal.Id; 
        }

        private protected CancellableTask() 
        {
            cts = new CancellationTokenSource();
        }

        private protected CancellableTask(Action<CancellationToken> action)
        {
            taskInternal = new Task(InternalAction);
            cts = new CancellationTokenSource();
        }

        public static CancellableTask Run(Action<CancellationToken> startup)
        {
            var ret = new CancellableTask() { _startup = startup };
            ret.taskInternal = Task.Run(ret.InternalAction);
            return ret;
        }

        public bool IsCompleted 
        { 
            get => taskInternal.IsCompleted; 
        }

        public void Cancel()
        {
            Cts.Cancel();
        }

        ~CancellableTask() 
        {
            Cts.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InternalAction()
        {
            //Console.WriteLine($"Task {Task.CurrentId} is running.");
            _startup(Cts.Token);
        }

        public static void WaitAll(IEnumerable<CancellableTask> tasks)
        {
            IEnumerable<Task> convertedTasks = tasks.Select<CancellableTask,Task>
                (ct=>ct.taskInternal);
            Task.WaitAll(convertedTasks.ToArray());
        }

        public static implicit operator Task(CancellableTask task) 
        {
            return task.taskInternal;
        }

    }
}
