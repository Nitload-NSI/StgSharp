using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Data
{

    public partial class Area
    {
        private static byte leastUnusedDisc;

        public byte NewDisc(byte raidWith)
        {
            byte ret = leastUnusedDisc;
            leastUnusedDisc++;
            return ret;
        }

        public void ClearDiscIDBuffer()
        {
            leastUnusedDisc = 0;
        }

        private int leastUnusedID;

        public int NewID()
        {
            int ret = leastUnusedID;
            if (ret == int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Reaches max IDs that one ssd file can provide.");
            }
            leastUnusedID++;
            return ret;
        }

        public void ClearIDBuffer()
        {
            leastUnusedID = 1;
        }

    }

}
