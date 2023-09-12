using StgSharp.Controlling;
using StgSharp.Math;
using System;

namespace StgSharp
{
    public static partial class Control
    {
        /// <summary>
        /// Add a pool to timeline controller, and return the instance of the pool.
        /// </summary>
        /// <returns></returns>
        public static Pool InitPool()
        {
            Pool pool = new Pool();
            AddPool(pool);
            return pool;
        }

        public static void AddPool(Pool pool)
        {
            StgSharp.Controlling.TimeLine.allPool.Add(pool);
            StgSharp.Controlling.TimeLine._currentPool = pool;
        }

        public static void LoadTimeline()
        {
            
        }

        internal static void InitVector4map(Pool target)
        {
            Vector4Map map = new Vector4Map();
            TimeLine.GetCurrentPool()._Vector4mapContainer.Add(map);
        }

    }
}
