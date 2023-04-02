using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    public interface INode<T> where T : INode<T>
    {
        internal ContainerNode<T> ID { get; }

        internal void setID(ContainerNode<T> id);
    }


    public class ContainerNode<T>  where T : INode<T>
    {
        internal readonly T _value;
        internal ContainerNode<T> _previous;
        internal ContainerNode<T> _next;
        internal ContainerNode<T> _side;

        public ContainerNode(T t)
        {
            _value = t;
            t.setID(this);
        }

        public virtual ContainerNode<T> Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        public ContainerNode<T> Next
        {
            get { return _next; }
            set { _next = value; }
        }

        public ContainerNode<T> Side
        {
            get { return _side; }
            set { _side = value; }
        }

        public T Value
        {
            get { return _value; }
        }

        public void Remove()
        {
            _previous.Next = _next;
            _next.Previous = _previous;
            _next = default(ContainerNode<T>);
            _previous = default(ContainerNode<T>);
        }

    }
}
