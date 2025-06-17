//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpLogic.cs"
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Script.Express
{
    public enum ExpLogicValue
    {

        True,
        False,
        Unknown

    }

    public struct ExpLogic
    {

        public static readonly ExpLogic False = new ExpLogic( ExpLogicValue.False );

        public static readonly ExpLogic True = new ExpLogic( ExpLogicValue.True );
        public static readonly ExpLogic Unknown = new ExpLogic( ExpLogicValue.Unknown );

        private readonly ExpLogicValue _value;

        public ExpLogic( ExpLogicValue value )
        {
            _value = value;
        }

        public bool IsTrue => _value == ExpLogicValue.True;

        public bool IsFalse => _value == ExpLogicValue.False;

        public bool IsUnknown => _value == ExpLogicValue.Unknown;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public bool ToBoolean()
        {
            return _value == ExpLogicValue.True;
        }

        public override string ToString() => _value.ToString();

        public static bool TryParse( ReadOnlySpan<char> value, out ExpLogic result )
        {
            if( Enum.TryParse<ExpLogicValue>( value, out ExpLogicValue e ) )
            {
                result = new ExpLogic( e );
                return true;
            }
            result = default;
            return false;
        }

        // NOT !
        public static ExpLogic operator !( ExpLogic a )
        {
            return a.IsTrue ? False : a.IsFalse ? True : Unknown;
        }

        // AND &
        public static ExpLogic operator &( ExpLogic a, ExpLogic b )
        {
            if( a.IsFalse || b.IsFalse ) {
                return False;
            }

            if( a.IsUnknown || b.IsUnknown ) {
                return Unknown;
            }

            return True;
        }

        // XOR ^
        public static ExpLogic operator ^( ExpLogic a, ExpLogic b )
        {
            if( a.IsUnknown || b.IsUnknown ) {
                return Unknown;
            }

            return ( a.IsTrue ^ b.IsTrue ) ? True : False;
        }

        // OR |
        public static ExpLogic operator |( ExpLogic a, ExpLogic b )
        {
            if( a.IsTrue || b.IsTrue ) {
                return True;
            }

            if( a.IsUnknown || b.IsUnknown ) {
                return Unknown;
            }

            return False;
        }

    }
}
