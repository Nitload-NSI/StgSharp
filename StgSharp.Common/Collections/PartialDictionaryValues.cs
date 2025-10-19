//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="PartialDictionaryValues"
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
using System.Text;

namespace StgSharp.Collections
{
    public partial class PartialDictionary<TKey, TValue>
    {

        internal sealed class PartialDictionaryValues : ICollection<TValue>
        {

            private readonly PartialDictionary<TKey, TValue> _values;

            public PartialDictionaryValues(PartialDictionary<TKey, TValue> value)
            {
                _values = value;
            }

            public bool IsReadOnly => true;

            public int Count => _values.Count;

            public bool Contains(TValue item)
            {
                foreach (KeyValuePair<TKey, TValue> kvp in _values)
                {
                    if (kvp.Value != null && kvp.Value.Equals(item)) {
                        return true;
                    }
                }
                return false;
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                foreach (KeyValuePair<TKey, TValue> kvp in _values)
                {
                    array[arrayIndex] = kvp.Value;
                    arrayIndex++;
                }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                foreach (TKey key in _values._keys) {
                    yield return _values._source[key];
                }
            }

            void ICollection<TValue>.Add(TValue item)
            {
                return;
            }

            void ICollection<TValue>.Clear()
            {
                return;
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

    }
}
