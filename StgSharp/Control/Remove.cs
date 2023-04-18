using StgSharp.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Control
{
    public static class Remove
    {
        public static void Point(Point p)
        {
            GameTimeLine._currentPool.pointContainer.Remove(p);
        }

        

    }
}
