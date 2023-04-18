using StgSharp.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    public class LinkedList<T>:INode<LinkedList<T>>,IEnumerable where T: INode<T>
    {
        internal readonly ContainerNode<T> _heading;
        internal ContainerNode<T> _first;
        internal ContainerNode<T> _tail;
        private ContainerNode<T> _cache;

        public LinkedList()
        {
            _heading = new ContainerNode<T>(default(T));
        }

        public T First
        {
            get { return _first._value; }
            set 
            {
                _first = new ContainerNode<T>(value);
                _heading._next = _first;
            }
        }

        public T Tail
        {
            get { return _tail._value; }
            set { _tail = new ContainerNode<T>(value); }
        }

        public void Add(T t)
        {
            if (_first != default(ContainerNode<T>))
            {
                First = t;
            }
            else
            {
                _cache = new ContainerNode<T>(t);
                _tail._next = _cache;
                _cache._previous = _tail;
                _tail = _cache;
            }
        }

        public void Remove(T node)
        {
            foreach (INode<T> item in this)
            {
                if (item == node)
                {
                    node.SelfDestrcut();
                    break;
                }
            }
        }

        public void RemoveTail()
        {
            if (_tail==default(ContainerNode<T>))
            {

            }
            else
            {
                _cache = _tail;
                _tail = _cache._previous;
                _cache._previous = default(ContainerNode<T>);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinkedListEnumrator<T>(this);
        }
    }
}
