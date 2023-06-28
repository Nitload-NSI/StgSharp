using StgSharp.Geometries;

namespace StgSharp.Control
{
    public static class Remove
    {
        public static void Point(Point p)
        {
            TimeLine._currentPool._pointContainer.Remove(p);
        }



    }
}
