using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StgSharp.Logic
{
    internal class SizeFixedList<T> : IList<T>
    {
        private int capacity, _index;
        private T[] values;

        public SizeFixedList(int size)
        {
            values = new T[size];
        }

        public T this[int index] 
        {
            get => index<_index ? values[index] : throw new ArgumentOutOfRangeException(nameof(index));
            set 
            {
                if (true)
                {
                    values[index] = value ;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        public int Count => _index + 1;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (_index < capacity - 1)
            {
                _index++;
                this[_index] = item;
            }
        }

        public void Clear()
        {
            values = new T[capacity];
            _index = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i <= _index; i++)
            {
                if (item.Equals(values[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i <= _index; i++)
            {
                yield return values[i];
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i <= _index; i++)
            {
                if (values[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {

            for (int i = 0; i <= _index; i++)
            {
                if (values[i].Equals(item))
                {
                    values[i] = values[_index];
                    _index--;
                    return true;
                }
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index <= _index)
            {
                values[index] = values[_index];
                _index--;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref values, newSize);
        }

        public void RemoveAll(Predicate<T> predicate)
        {
            for (int i = _index; i > 0; i--)
            {
                if (predicate(values[i]))
                {
                    values[i] = values[_index];
                    _index--;
                }
            }
        }
    }//------------------------------------- End of Class ------------------------------------------
}
