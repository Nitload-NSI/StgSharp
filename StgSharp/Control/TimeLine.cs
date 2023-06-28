using StgSharp.Entities;
using StgSharp.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace StgSharp.Control
{
    public static class TimeLine
    {
        [AllowNull]
        internal static Pool _currentPool = default;
        internal static LinkedList<Pool> allPool;
        internal static LinkedList<Form> allForm;

        /// <summary>
        /// Tick counter count the frames passed since the program start.
        /// </summary>
        public static readonly Counter<uint> tickCounter = new Counter<uint>(0, 1, 1);

        [AllowNull]
        internal static TimelineBlock currentOperation;
        private static bool isPaused = false;

        static TimeLine()
        {
            allForm = new LinkedList<Form>();
            allPool = new LinkedList<Pool>();
#if DEBUG
            Console.WriteLine("Timeline init OK!");
#endif
            internalIO.InternalInitGL();
        }

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


        public static void InitProgram()
        {

        }

        public static unsafe void StartProgram()
        {
        }

    }
}
