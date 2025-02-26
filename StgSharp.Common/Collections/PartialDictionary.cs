//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PartialDictionary.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace StgSharp.Collections
{
    public partial class PartialDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _source;
        private readonly HashSet<TKey> _keys;
        private readonly PartialDictionaryValues _values;

        public PartialDictionary(
                       Dictionary<TKey, TValue> source,
                       TKey[] index )
        {
            _source = source;
            _keys = new HashSet<TKey>( index );
            foreach( TKey item in _keys ) {
                if( !_source.ContainsKey( item ) ) {
                    _source.Add( item, default! );
                }
            }
            _values = new PartialDictionaryValues( this );
        }

        public PartialDictionary(
                       Dictionary<TKey, TValue> source,
                       HashSet<TKey> keys )
        {
            _source = source;
            _keys = keys;
            foreach( TKey item in _keys ) {
                if( !_source.ContainsKey( item ) ) {
                    _source.Add( item, default! );
                }
            }
            _values = new PartialDictionaryValues( this );
        }

        public PartialDictionary(
                Dictionary<TKey, TValue> source,
                TKey[] index, IHashMultiplexer<TKey> multiplexer
            )
        {
            _source = source;
            _keys = new HashSet<TKey>(index);
            foreach (TKey item in _keys)
            {
                if (!_source.ContainsKey(item))
                {
                    _source.Add(item, default!);
                }
            }
            _values = new PartialDictionaryValues(this);
        }

        public TValue this[ TKey key ]
        {
            get
            {
                if( _keys.Contains( key ) ) {
                    return _source[ key ];
                }
                throw new KeyNotFoundException();
            }
            set
            {
                if( _keys.Contains( key ) ) {
                    _source[ key ] = value;
                }
                throw new KeyNotFoundException();
            }
        }

        public ICollection<TKey> Keys => _keys;

        public ICollection<TValue> Values => _values;

        public int Count => _keys.Count;

        public bool Contains( KeyValuePair<TKey, TValue> item )
        {
            return _keys.Contains( item.Key ) && _source.Contains( item );
        }

        public bool ContainsKey( TKey key )
        {
            return _keys.Contains( key );
        }

        public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach( TKey item in _keys ) {
                yield return new KeyValuePair<TKey, TValue>(
                    item, _source[ item ] );
            }
        }

        public bool TryGetValue( TKey key, out TValue value )
        {
            if( _keys.Contains( key ) ) {
                value = _source[ key ];
                return true;
            }
            value = default!;
            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(
                                                             KeyValuePair<TKey, TValue> item )
        {
            return;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
                                                             KeyValuePair<TKey, TValue> item )
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

        void IDictionary<TKey, TValue>.Add( TKey key, TValue value )
        {
            return;
        }

        bool IDictionary<TKey, TValue>.Remove( TKey key )
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
