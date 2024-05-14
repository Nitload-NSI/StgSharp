using StgSharp.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{
    public static unsafe partial class StgSharp
    {
        private static SSD defaultIdProvider = new SSD();
        private static List<Area> root = new List<Area>();

        private static byte leastUnusedArea;

        public static byte NewArea()
        {
            byte ret = leastUnusedArea;
            leastUnusedArea++;
            return ret;
        }


    }//------------------ end of class ---------------------
}
