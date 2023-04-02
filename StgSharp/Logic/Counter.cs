using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    public class Counter<T>where T:struct, IConvertible, IFormattable, IComparable
    {
        internal readonly dynamic _defualtValue;
        internal readonly dynamic _defualtAddValue;
        internal readonly dynamic _defualtSubValue;

        internal dynamic _value;

        public dynamic Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Counter(T defualtValue,T defualtAddValue,T defualtSubValue)
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
            _defualtValue = defualtValue;
            _defualtAddValue = defualtAddValue;
            _defualtSubValue = defualtSubValue;
        }

        
        public void Add(T value)
        {
            _value += value;
        }

        public void QuickAdd()
        {
            _value += _defualtAddValue;
        }

        public void Sub(T value)
        {
            _value -= value;
        }

        public void QuickSub()
        {
            _value -= _defualtSubValue;
        }

        public void Reset()
        {
            _value = _defualtValue;
        }
    }
}
