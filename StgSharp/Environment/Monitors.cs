using StgSharp.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{
    public static partial class StgSharp
    {
        private unsafe static IntPtr[] monitorsArray;

        public static unsafe IntPtr[] MonitorsHandleArray
        {
            get 
            {
                if (monitorsArray == null)
                {
                    int count = 0;
                    IntPtr* arrayhandle = InternalIO.glfwGetMonitors(&count);
                    monitorsArray = new IntPtr[count];
                    Marshal.Copy((IntPtr)arrayhandle,monitorsArray,0,count);
                    Marshal.FreeHGlobal((IntPtr)arrayhandle);
                }
                return monitorsArray;
            }
        }

        public static IntPtr DefaultMonitor
        {
            get 
            {
                return MonitorsHandleArray[0];
            }
        }
    }


}
