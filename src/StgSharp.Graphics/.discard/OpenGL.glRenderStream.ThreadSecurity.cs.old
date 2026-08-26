// -----------------------------------------------------------------------------
// file="glRenderStream.ThreadSecurity"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    public partial class glRender
    {

        private static ConcurrentDictionary<Thread, glRender> threadLocalIndex
            = new();
        private readonly SynchronizationContext mainThreadContext = SynchronizationContext.Current;

    }
}
