namespace StgSharp.Logic
{
    public abstract class QuickClearNode<T> where T : QuickClearNode<T>
    {
        internal LinkedList<LinkedListNode<T>> _idList
            = new LinkedList<LinkedListNode<T>>();

        internal void QuickDestruct(LinkedList<T> list)
        {
            foreach (LinkedListNode<T> id in _idList)
            {
                if (id.list == list)
                {
                    RecycleBin.Add(id);
                    break;
                }
            }
        }

        internal static LinkedList<LinkedListNode<T>> RecycleBin
            = new LinkedList<LinkedListNode<T>>();
    }
}
