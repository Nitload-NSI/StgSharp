// -----------------------------------------------------------------------------
// file="Monitor"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public static class GraphicsEnvironment
    {

        private static IntPtr[] _monitorsHandleArray;

        public static unsafe ReadOnlySpan<IntPtr> MonitorsHandleArray
        {
            get
            {
                if (_monitorsHandleArray == null)
                {
                    int count = 0;
                    IntPtr* arrayHandle = GraphicFramework.glfwGetMonitors(&count);
                    _monitorsHandleArray = new IntPtr[count];
                    Marshal.Copy((IntPtr)arrayHandle, _monitorsHandleArray, 0, count);
                    Marshal.FreeHGlobal((IntPtr)arrayHandle);
                }
                return _monitorsHandleArray;
            }
        }

    }
}
