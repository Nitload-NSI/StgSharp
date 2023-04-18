using StgSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Control
{
    public static class GameTimeLine
    {
        internal static Pool _currentPool;
        internal static LinkedList<Pool> allPool;
        public static Counter<uint> tickCounter = new Counter<uint>(0,1,1);
        internal static TimelineBlock currentOperation;
        private static bool isPaused;

        internal static void OnRender()
        {
            tickCounter.QuickAdd();
            foreach (Pool p in allPool)
            {
                _currentPool = p;
                if (!isPaused)
                {
                    currentOperation.OnUpdating();
                }
                foreach (EntityPartical b in _currentPool._bulletContainer)
                {

                }
            }
        }
    }
}
