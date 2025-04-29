//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Counter.cs"
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
using System.Numerics;

namespace StgSharp
{
    public class Counter<T> where T: struct, INumber<T>
    {

        internal dynamic _value;
        internal readonly T _defaultValue;
        internal readonly T _defualtDecreament;

        internal readonly T _defualtIncreament;

        public Counter( T defaultValue, T defaultIncrement, T defaultDecrement )
        {
            _value = defaultValue;
            _defaultValue = defaultValue;
            _defualtIncreament = defaultIncrement;
            _defualtDecreament = defaultDecrement;
        }

        public T DecrementSpan => _defualtDecreament;

        public T IncrementSpan => _defualtIncreament;

        public T CurrentSpan => _value - _defaultValue;

        public T Value
        {
            get => _value;
            set => _value = value;
        }

        public void Add( T value )
        {
            _value += value;
        }

        public void QuickAdd()
        {
            _value += _defualtIncreament;
        }

        public void QuickSub()
        {
            _value -= _defualtDecreament;
        }

        public void Reset()
        {
            _value = _defaultValue;
        }

        public void Sub( T value )
        {
            _value -= value;
        }

        public override string ToString() => Value.ToString();

        public static Counter<T> operator --( Counter<T> counter )
        {
            counter.QuickSub();
            return counter;
        }

        public static Counter<T> operator ++( Counter<T> counter )
        {
            counter.QuickAdd();
            return counter;
        }

        public static implicit operator T( Counter<T> counter )
        {
            return counter switch
            {
                null => T.Zero,
                _ => counter.Value
            };
        }

    }
}