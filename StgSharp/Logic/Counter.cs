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
using System.Xml.Serialization;

namespace StgSharp
{

    public class Counter<T> where T : struct, IConvertible, IFormattable, IComparable
    {

        internal readonly T _defualtAddValue;
        internal readonly T _defualtSubValue;
        internal readonly T _defualtValue;

        internal dynamic _value;

        public Counter(T defualtValue, T defualtAddValue, T defualtSubValue)
        {
            var type = Type.GetTypeCode(defualtValue.GetType());
            switch (type)
            {
                case TypeCode.Byte:
                    break;

                case TypeCode.Int16:
                    break;

                case TypeCode.Int32:
                    break;

                case TypeCode.Int64:
                    break;

                case TypeCode.Double:
                    break;

                case TypeCode.Single:
                    break;

                case TypeCode.SByte:
                    break;

                case TypeCode.UInt16:
                    break;

                case TypeCode.UInt32:
                    break;

                case TypeCode.UInt64:
                    break;

                default:
                    throw new ArgumentException("Input is not number value type");
                    break;
            }
            _value = defualtValue;
            _defualtValue = defualtValue;
            _defualtAddValue = defualtAddValue;
            _defualtSubValue = defualtSubValue;
        }

        public T Decreament => _defualtSubValue;

        public T Increament => _defualtAddValue;

        public T Span => _value - _defualtValue;

        public T Value
        {
            get => _value;
            set => _value = value;
        }

        public void Add(T value)
        {
            _value += value;
        }

        public void QuickAdd()
        {
            _value += _defualtAddValue;
        }

        public void QuickSub()
        {
            _value -= _defualtSubValue;
        }

        public void Reset()
        {
            _value = _defualtValue;
        }

        public void Sub(T value)
        {
            _value -= value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static Counter<T> operator --(Counter<T> counter)
        {
            counter.QuickSub();
            return counter;
        }

        public static Counter<T> operator ++(Counter<T> counter)
        {
            counter.QuickAdd();
            return counter;
        }

        public static implicit operator T(Counter<T> counter)
        {
            return counter.Value;
        }

    }
}