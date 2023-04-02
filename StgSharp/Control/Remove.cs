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
            StgSharp.ContainerNode<Point> _cache = p._id;
            _cache.Previous.Next = _cache.Next;
            _cache.Next.Previous = _cache.Previous;
            _cache.Next = default;
            _cache.Previous = default;
        }

        

    }
}
