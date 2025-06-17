//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="RandomAccessibleStack.cs"
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Collections
{
    public class RandomAccessibleStack<T> : IEnumerable<T>
    {

        private const int DefaultCapacity = 4;

        private T[] _array;
        private int _size;
        private int _version;

        /// <summary>
        ///   Initializes a new instance of the RandomAccessStack class that is empty and has the
        ///   default initial capacity.
        /// </summary>
        public RandomAccessibleStack()
        {
            _array = new T[DefaultCapacity];
        }

        /// <summary>
        ///   Initializes a new instance of the RandomAccessStack class that is empty and has the
        ///   specified initial capacity.
        /// </summary>
        public RandomAccessibleStack( int capacity )
        {
            if( capacity < 0 ) {
                throw new ArgumentOutOfRangeException(
                    nameof( capacity ), "Capacity must be non-negative." );
            }

            _array = new T[capacity];
        }

        /// <summary>
        ///   Gets or sets the element at the specified index in the stack (0 = bottom, Count-1 =
        ///   top).
        /// </summary>
        public T this[ int index ]
        {
            get
            {
                if( index < 0 || index >= _size ) {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                return _array[ index ];
            }
            /*
            set
            {
                if( index < 0 || index >= _size ) {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }
                _array[ index ] = value;
                _version++;
            }
            /**/
        }

        /// <summary>
        ///   Gets the number of elements contained in the stack.
        /// </summary>
        public int Count => _size;

        /// <summary>
        ///   Removes all objects from the stack.
        /// </summary>
        public void Clear()
        {
            Array.Clear( _array, 0, _size );
            _size = 0;
            _version++;
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the stack from top to bottom. Throws if
        ///   the stack is modified during iteration.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            int expectedVersion = _version;
            for( int i = _size - 1; i >= 0; i-- )
            {
                if( expectedVersion != _version ) {
                    throw new InvalidOperationException(
                        "Collection was modified during enumeration." );
                }

                yield return _array[ i ];
            }
        }

        /// <summary>
        ///   Returns the object at the top of the stack without removing it.
        /// </summary>
        public T Peek()
        {
            if( _size == 0 ) {
                throw new InvalidOperationException( "Stack is empty." );
            }

            return _array[ _size - 1 ];
        }

        /// <summary>
        ///   Removes and returns the object at the top of the stack.
        /// </summary>
        public T Pop()
        {
            if( _size == 0 ) {
                throw new InvalidOperationException( "Stack is empty." );
            }

            T item = _array[ --_size ];
            _array[ _size ] = default!;
            _version++;
            return item;
        }

        /// <summary>
        ///   Adds an item to the top of the stack.
        /// </summary>
        public void Push( T item )
        {
            if( _size == _array.Length ) {
                EnsureCapacity( _size + 1 );
            }

            _array[ _size++ ] = item;
            _version++;
        }

        private void EnsureCapacity( int min )
        {
            int newCapacity = _array.Length == 0 ? DefaultCapacity : _array.Length * 2;
            if( newCapacity < min ) {
                newCapacity = min;
            }

            Array.Resize( ref _array, newCapacity );
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
