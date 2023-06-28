using StgSharp.Control;

namespace StgSharp
{
    public static partial class Instruction
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
            TimeLine.allPool.Add(pool);
            TimeLine._currentPool = pool;
        }
    }
}
