//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="BidirectionalDictionary"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StgSharp.Collections
{
    public sealed class BidirectionalDictionary<TFirst, TSecond> : IBidirectionalDictionary<TFirst, TSecond>
    {

        private readonly Dictionary<TFirst, TSecond> _forward;
        private readonly Dictionary<TSecond, TFirst> _reverse;

        public BidirectionalDictionary()
        {
            _forward = new Dictionary<TFirst, TSecond>();
            _reverse = new Dictionary<TSecond, TFirst>();
        }

        public TFirst this[TSecond key]
        {
            get
            {
                if (_reverse.TryGetValue(key, out TFirst value)) {
                    return value;
                }
                throw new ArgumentException($"Cannot find key {key!.ToString()} in dictionary.");
            }
            set
            {
                if (_reverse.TryGetValue(key, out TFirst origin))
                {
                    _reverse[key] = value;
                    _forward.Remove(origin);
                } else
                {
                    _reverse.Add(key, value);
                }
                _forward.Add(value, key);
            }
        }

        public TSecond this[TFirst key]
        {
            get
            {
                if (_forward.TryGetValue(key, out TSecond value)) {
                    return value;
                }
                throw new ArgumentException($"Cannot find key {key!.ToString()} in dictionary.");
            }
            set
            {
                if (_forward.TryGetValue(key, out TSecond origin))
                {
                    _forward[key] = value;
                    _reverse.Remove(origin);
                } else
                {
                    _forward.Add(key, value);
                }
                _reverse.Add(value, key);
            }
        }

        public bool IsReadOnly => false;

        public ICollection<TFirst> FirstIndex => _forward.Keys;

        public ICollection<TSecond> SecondIndex => _reverse.Keys;

        public int Count => _forward.Count;

        public IReadOnlyDictionary<TFirst, TSecond> Forward => new ReadOnlyDictionary<TFirst, TSecond>(
            _forward);

        public IReadOnlyDictionary<TSecond, TFirst> Reverse => new ReadOnlyDictionary<TSecond, TFirst>(
            _reverse);

        public void Add(KeyValuePair<TFirst, TSecond> item)
        {
            if (_forward.ContainsKey(item.Key) || _reverse.ContainsKey(item.Value)) {
                throw new ArgumentException("Duplicate key or value.");
            }
            _forward.Add(item.Key, item.Value);
            _reverse.Add(item.Value, item.Key);
        }

        public void Add(TFirst key, TSecond value)
        {
            if (_forward.ContainsKey(key) || _reverse.ContainsKey(value)) {
                throw new ArgumentException("Duplicate key or value.");
            }
            _forward.Add(key, value);
            _reverse.Add(value, key);
        }

        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        public bool Contains(KeyValuePair<TFirst, TSecond> item)
        {
            return _forward.Contains(item) && _reverse.TryGetValue(item.Value, out TFirst var) && var!.Equals(item.Key);
        }

        public bool Contains(TSecond key)
        {
            return _reverse.ContainsKey(key);
        }

        public bool Contains(TFirst key)
        {
            return _forward.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            if (array == null) {
                throw new ArgumentNullException(nameof(array));
            }
            if (arrayIndex < 0 || arrayIndex > array.Length) {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }
            if (array.Length - arrayIndex < _forward.Count) {
                throw new ArgumentException("The array is too small to copy the elements.");
            }

            foreach (KeyValuePair<TFirst, TSecond> pair in _forward) {
                array[arrayIndex++] = pair;
            }
        }

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            foreach (KeyValuePair<TFirst, TSecond> pair in _forward) {
                yield return pair;
            }
        }

        public bool Remove(TFirst key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TFirst, TSecond> item)
        {
            if (_forward.Remove(item.Key)) {
                return _reverse.Remove(item.Value);
            }
            return false;
        }

        public bool TryGetValue(TFirst key, out TSecond value)
        {
            return _forward.TryGetValue(key, out value);
        }

        public bool TryGetValue(TSecond key, out TFirst value)
        {
            return _reverse.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}