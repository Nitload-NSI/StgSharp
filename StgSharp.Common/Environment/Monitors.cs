//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Monitors"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
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
using System.Runtime.InteropServices;

namespace StgSharp
{
    public static partial class World
    {

        public static unsafe IntPtr[] MonitorsHandleArray
        {
            get
            {
                if (MonitorsArray == null)
                {
                    int count = 0;
                    IntPtr* arrayHandle = GraphicFramework.glfwGetMonitors(&count);
                    MonitorsArray = new IntPtr[count];
                    Marshal.Copy((IntPtr)arrayHandle, MonitorsArray, 0, count);
                    Marshal.FreeHGlobal((IntPtr)arrayHandle);
                }
                return MonitorsArray;
            }
        }

        public static nint[] MonitorsArray { get; set; } = [];

        public static IntPtr DefaultMonitor => MonitorsHandleArray[0];

    }
}
