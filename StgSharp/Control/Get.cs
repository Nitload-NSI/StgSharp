using StgSharp.Controlling;
using StgSharp.Graphics;
using StgSharp.Logic;
using StgSharp.Math;

namespace StgSharp
{
    public static unsafe partial class Control
    {
        public static LinkedList<T> GetAllActivatedPool<T>() where T : Pool
        {
            /**/
            LinkedList<T> pool = new LinkedList<T>();
            foreach (Pool item in TimeLine.allPool)
            {
                pool.Add((T)item);
            }
            return pool;
            /**/
        }

        internal static Vector4Map GetAvailabeVector4map()
        {
            Pool p = TimeLine.GetCurrentPool();
            Vector4Map lastMap = p._Vector4mapContainer.Tail;

            if (lastMap.isFull)
            {
                lastMap = new Vector4Map();
                p._Vector4mapContainer.Add(lastMap);
            }

            return lastMap;
        }

        internal static long GetAvailableVector4ptr()
        {
            Pool p = TimeLine.GetCurrentPool();
            Vector4Map lastMap = p._Vector4mapContainer.Tail;

            if (lastMap.isFull)
            {
                lastMap = new Vector4Map();
                p._Vector4mapContainer.Add(lastMap);
            }
            long ptr = lastMap.RegistManagedVector4Ptr();

            return ptr;
        }

        /// <summary>
        /// Get the status of a certain key on the keyboard.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyStatus GetKeyStatus(Form form, Key key)
        {
            return internalIO.glfwGetKey(form.windowID, (int)key);
        }

    }
}
