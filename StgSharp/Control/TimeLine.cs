using StgSharp.Entities;
using StgSharp.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace StgSharp.Controlling
{
    public static partial class TimeLine
    {
        public const long version = 10;

        internal static Pool _currentPool = default;
        internal static LinkedList<Pool> allPool = new LinkedList<Pool>();
        internal static LinkedList<Form> allForm;

        /// <summary>
        /// Tick counter count the frames passed since the program start.
        /// </summary>
        public static readonly Counter<uint> tickCounter = new Counter<uint>(0, 1, 1);

        internal static TimelineBlock currentOperation;
        private static bool isPaused = false;

        static unsafe TimeLine()
        {
            allForm = new LinkedList<Form>();
            allPool = new LinkedList<Pool>();
        }

        public static TimelineBlock CurrentOperation
        {
            get { return currentOperation; }
            set 
            {
                if (currentOperation==default||
                    currentOperation.isComplete)
                {
                    currentOperation = value;
                }
            }
        }

        public static void OnRender()
        {
            tickCounter.QuickAdd();
            foreach (Pool p in allPool)
            {
                _currentPool = p;
                if (!currentOperation.isComplete)
                {
                    currentOperation.OnUpdating();
                }
            }
        }


        public static void InitProgram()
        {

        }

        public static unsafe void StartProgram()
        {
        }

        public static void LoadPool(Pool pool)
        {
            allPool.Add(pool);
            Console.WriteLine(allPool.count.Value);
        }

        public static Pool GetCurrentPool()
        {
            return _currentPool;
        }

    }
}
