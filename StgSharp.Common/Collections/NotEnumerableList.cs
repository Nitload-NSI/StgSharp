using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Collections
{
    public sealed class NotEnumerableList<T>(int capacity = 4)
    {

        private T[] _items = new T[capacity];
        private int _size = 0;

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (uint)index >= (uint)_size ? throw new ArgumentOutOfRangeException(nameof(index)) : _items[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if ((uint)index >= (uint)_size) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                _items[index] = value;
            }
        }

        public int Count => _size;

        public int Capacity => _items.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (_size == _items.Length) {
                EnsureCapacity(_size + 1);
            }

            _items[_size++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return new Span<T>(_items, 0, _size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FastClear()
        {
            // 只重置size，不清理数组内容
            _size = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
                if (newCapacity < min) {
                    newCapacity = min;
                }

                Array.Resize(ref _items, newCapacity);
            }
        }

    }
}

