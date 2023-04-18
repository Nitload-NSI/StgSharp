using StgSharp.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Control
{
    public static class Add
    {
        public static void Pool(Pool pool)
        {
            GameTimeLine.allPool.Add(pool);
        }
    }
}
