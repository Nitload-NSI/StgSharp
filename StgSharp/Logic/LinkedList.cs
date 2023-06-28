using System;
using System.Collections;

namespace StgSharp
{
    public class LinkedList<T> : IEnumerable
    {
        internal readonly LinkedListNode<T> _heading;
        private LinkedListNode<T> _cache;
        private bool isEmpty;
        public Counter<uint> count = new Counter<uint>(0, 1, 1);

        public LinkedList()
        {
            _heading = new LinkedListNode<T>();
            isEmpty = true;
        }

        public T First
        {
            get { return _heading._next._value; }
            /*
             * This setter is ued to reset the First node,
             * be careful when use this setter to avoid 
             * unexpected risk of this List lose all nodes.
            */
            private set { _heading._next = default; }
        }

        public T Tail
        {
            get { return _heading._previous._value; }
            /*
             * This setter is ued to reset the Last node,
             * be careful when use this setter to avoid 
             * unexpected risk of this List lose all nodes.
            */
            private set { _heading._previous = default; }
        }

        /// <summary>
        /// Add an item to the tail of linked list.
        /// </summary>
        /// <param name="t">The item ready to add to the list</param>
        public void Add(T t)
        {
            //checkout if the linkedlist is null
            if (isEmpty)
            {
                LinkedListNode<T> node = new LinkedListNode<T>(t);
                _heading._next = node;
                _heading._previous = node;
                node.Next = _heading;
                node.Previous = _heading;
                isEmpty = false;
            }
            else
            {
                LinkedListNode<T> node = new LinkedListNode<T>(t);
                node.Next = _heading;
                node.Previous = _heading.Previous;
                _heading._previous._next = node;
                _heading._previous = node;

            }
            count++;
        }

        public void Remove(T value)
        {
            LinkedListNode<T> node = Find(value);
            node._next._previous = node._previous;
            node._previous._next = node._next;
            node._previous = null;
            node._next = null;
            count--;
        }

        public void RemoveTail()
        {
            //check out if the list has only one member
            if (count._value == 1)
            {
                First = default;
                isEmpty = true;
            }
            else
            {
                _heading.Previous.Next = _heading;
            }
            count--;
        }

        internal LinkedListNode<T> Find(T value)
        {
            foreach (LinkedListNode<T> node in this)
            {
                if (node._value.Equals(value))
                {
                    return node;
                }
            }
            throw new Exception("Value not found in this LinkedList");
        }

        public void Clear()
        {
            _heading._previous = null;
            _heading._next = null;
            count.Reset();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinkedListEnumrator<T>(this);
        }

        internal struct LinkedListEnumrator<T> : IEnumerator
        {
            private LinkedList<T> _target;
            LinkedListNode<T> enumrator;

            public LinkedListEnumrator(LinkedList<T> target)
            {
                _target = target;
                enumrator = target._heading;
            }

            object IEnumerator.Current
            {
                get { return enumrator._value; }
            }

            bool IEnumerator.MoveNext()
            {
                enumrator = enumrator._next;
                return enumrator == default(LinkedListNode<T>);
            }

            void IEnumerator.Reset()
            {
                enumrator = _target._heading;
            }
        }

    }
}
