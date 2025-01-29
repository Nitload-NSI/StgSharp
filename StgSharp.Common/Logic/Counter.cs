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

namespace StgSharp
{
    public class Counter<T>
        where T: struct, IConvertible, IFormattable, IComparable
    #if NET8_OR_GREATER
        ,INumber
    #endif
    {
        internal dynamic _value;

        internal readonly T _defualtIncreament;
        internal readonly T _defualtDecreament;
        internal readonly T _defaultValue;

        public Counter( T defualtValue, T defualtIncreament, T defualtDecreament )
        {
            Type tType = typeof( T );
            if (
                tType != typeof(byte) &&
                tType != typeof(short) &&
                tType != typeof(int) &&
                tType != typeof(long) &&
                tType != typeof(sbyte) &&
                tType != typeof(ushort) &&
                tType != typeof(uint) &&
                tType != typeof(ulong) &&
                tType != typeof(float) &&
                tType != typeof(double))
            {
                throw new ArgumentException(
                    "Input is not number value type");
            }

            _value = defualtValue;
            _defaultValue = defualtValue;
            _defualtIncreament = defualtIncreament;
            _defualtDecreament = defualtDecreament;
        }

        public T DecreamentSoan => _defualtDecreament;

        public T IncreamentSpan => _defualtIncreament;

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

        public override string ToString()
        {
            return Value.ToString();
        }

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
            return counter.Value;
        }
    }
}