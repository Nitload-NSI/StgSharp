using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Logic
{
    internal class LinkedListEnumrator<T> : IEnumerator where T : INode<T>
    {
        private LinkedList<T> _target;
        ContainerNode<T> enumrator;

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
            return enumrator == default(ContainerNode<T>);
        }

        void IEnumerator.Reset()
        {
            enumrator = _target._heading;
        }
    }
}
