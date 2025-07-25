﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="frameworkSyncContext.cs"
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Threading
{
    public class FrameworkSyncContext : SynchronizationContext
    {

        private static readonly FrameworkSyncContext _mainThreadContext = new FrameworkSyncContext();

        private readonly BlockingCollection<SendOrPostCallback> _syncContexts = new BlockingCollection<SendOrPostCallback>(
            );
        private readonly Thread _mainThread = Thread.CurrentThread;

        public FrameworkSyncContext()
        {
            _syncContexts = new BlockingCollection<SendOrPostCallback>();
            _mainThread = Thread.CurrentThread;
        }

        public static FrameworkSyncContext MainThreadContext
        {
            get => _mainThreadContext;
        }

        public override void Post( SendOrPostCallback d, object state )
        {
            _syncContexts.Add( d );
        }

        public override void Send( SendOrPostCallback d, object state )
        {
            if( Thread.CurrentThread == _mainThread )
            {
                d( state );
            } else
            {
                Post( d, state );
            }
        }

    }
}
