using System.Diagnostics.CodeAnalysis;

namespace StgSharp
{
    internal class LinkedListNode<T>
    {
        internal readonly T _value;

        [AllowNull]
        internal LinkedListNode<T> _previous;
        [AllowNull]
        internal LinkedListNode<T> _next;
        internal LinkedList<T> list;

        internal LinkedListNode()
        {
        }


        public LinkedListNode(T t)
        {
            _value = t;
            _previous = new LinkedListNode<T>();
            _next = new LinkedListNode<T>();
        }

        public virtual LinkedListNode<T> Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        public LinkedListNode<T> Next
        {
            get { return _next; }
            set { _next = value; }
        }

        public T Value
        {
            get { return _value; }
        }

        public void Remove()
        {
            _previous.Next = _next;
            _next.Previous = _previous;
            _next = default(LinkedListNode<T>);
            _previous = default(LinkedListNode<T>);
        }

    }
}
