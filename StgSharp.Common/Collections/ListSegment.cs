using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StgSharp.Collections
{
    /// <summary>
    /// Part of a collection. This contains a direct reference to origin collection,
    /// so all change to origin collection will be reflected in this object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ListSegment<T> : ICollection<T>
    {
        private List<T> _collection;
        private int _begin;
        private int _length;


        public ListSegment(List<T> collection, int begin, int length)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentOutOfRangeException.ThrowIfLessThan(begin, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(begin + length, collection.Count);
            _collection = collection;
            _begin = begin;
            _length = length;
        }   

        public int Count => _length;

        public bool IsReadOnly => true;

        public T this[int indexValue]
        {
            get {
                ArgumentOutOfRangeException.ThrowIfLessThan(indexValue, 0);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(indexValue, _length);
                return _collection[indexValue + _begin];
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            var index=  _collection.IndexOf(item);
            return index >= _begin && index < _length + index;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);
            var span = CollectionsMarshal.AsSpan<T>(_collection).Slice(_begin, _length);
            var target = new Span<T>(array, arrayIndex,_length);
            span.CopyTo(target);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _begin; i < _length; i++)
            {
                yield return this[i];
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
