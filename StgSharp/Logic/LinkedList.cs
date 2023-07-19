using System;
using System.Collections;

namespace StgSharp
{
    public class LinkedList<T> : IEnumerable
    {
        internal readonly LinkedListNode<T> _hook;
        private LinkedListNode<T> _cache;
        private bool isEmpty;
        public Counter<uint> count = new Counter<uint>(0, 1, 1);

        public LinkedList()
        {
            _hook = new LinkedListNode<T>();
            isEmpty = true;
        }

        public T First
        {
            get { return _hook._next._value; }
            /*
             * This setter is ued to reset the First node,
             * be careful when use this setter to avoid 
             * unexpected risk of this List lose all nodes.
            */
            private set { _hook._next = default; }
        }

        public T Tail
        {
            get { return _hook._previous._value; }
            /*
             * This setter is ued to reset the Last node,
             * be careful when use this setter to avoid 
             * unexpected risk of this List lose all nodes.
            */
            private set { _hook._previous = default; }
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
                _hook._next = node;
                _hook._previous = node;
                node.Next = _hook;
                node.Previous = _hook;
                isEmpty = false;
            }
            else
            {
                LinkedListNode<T> node = new LinkedListNode<T>(t);
                node.Next = _hook;
                node.Previous = _hook.Previous;
                _hook._previous._next = node;
                _hook._previous = node;

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

        public void RemoveFirst()
        {

            if (count._value == 1)
            {
                First = default;
                isEmpty = true;
            }
            else
            {
                _hook._next.Remove();
            }
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
                _hook.Previous = _hook.Previous.Previous;
                _hook.Previous.Next = _hook;
            }
            count--;
        }

        internal LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> enumerator = this._hook;
            do
            {
                enumerator = enumerator._next;
                Console.WriteLine(enumerator.Value.ToString());
            } while (!enumerator.Value.Equals(value)
            || enumerator == _hook);

            if (enumerator == _hook)
            {
                throw new Exception();
            }

            return enumerator;
        }

        public void Clear()
        {
            _hook._previous = null;
            _hook._next = null;
            count.Reset();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinkedListEnumrator<T>(this);
        }


        /**/
        internal struct LinkedListEnumrator<T> : IEnumerator
        {
            private LinkedList<T> _target;
            LinkedListNode<T> enumerator;
            private uint index=0;
            private T _value;

            public LinkedListEnumrator(LinkedList<T> target)
            {
                _target = target;
                enumerator = target._hook;
            }

            object IEnumerator.Current
            {
                get 
                {
                    if (index == 0 || enumerator == _target._hook)
                    {
                        throw new InvalidOperationException("Out of index range");
                    }
                    else
                    {
                        _value = enumerator._value;
                        return _value;
                    }
                }
            }

            public T Current
            {
                get
                {
                    return _value;
                }
            }

            bool IEnumerator.MoveNext()
            {
                enumerator = enumerator._next;
                if (enumerator == _target._hook)
                {
                    return false;
                }
                index++;
                return true;
            }

            void IEnumerator.Reset()
            {
                enumerator = _target._hook;
            }
        }
        /**/
    }
}
