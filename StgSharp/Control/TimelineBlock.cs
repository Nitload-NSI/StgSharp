namespace StgSharp.Controlling
{
    public abstract class TimelineBlock
    {
        public T GetCurrentPool<T>() where T:Pool
        {
            return TimeLine._currentPool as T;
        }

        public abstract bool isComplete
        {
            get;
        }

        /*
        A virtual method api to define what should program do
        in a framecycle, an int value is required to express 
        the result of the cycle, that if there is any error in the cycle
         */
        public abstract int OnUpdating();
        public abstract bool GetShouldComplete();  
    }
}
