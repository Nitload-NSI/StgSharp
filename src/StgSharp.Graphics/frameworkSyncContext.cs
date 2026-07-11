// -----------------------------------------------------------------------------
// file="frameworkSyncContext"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Threading
{
    public class FrameworkSyncContext : SynchronizationContext
    {

        private readonly BlockingCollection<SendOrPostCallback> _syncContexts = [];
        private readonly Thread _mainThread = Thread.CurrentThread;

        public FrameworkSyncContext()
        {
            _mainThread = Thread.CurrentThread;
        }

        public static FrameworkSyncContext MainThreadContext
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = new FrameworkSyncContext();

        public override void Post(
                             SendOrPostCallback d,
                             object state
        )
        {
            _syncContexts.Add(d);
        }

        public override void Send(
                             SendOrPostCallback d,
                             object state
        )
        {
            if (Thread.CurrentThread == _mainThread)
            {
                d(state);
            } else
            {
                Post(d, state);
            }
        }

    }
}
