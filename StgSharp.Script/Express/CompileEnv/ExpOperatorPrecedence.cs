//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpOperatorPrecedence.cs"
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
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public static partial class ExpressCompile
    {

        public static bool TryGetOperatorPrecedence(
                                   string operatorName,
                                   out int precedence )
        {
            return _operatorPrecedence.TryGetValue(
                operatorName, out precedence );
        }

        public static bool TryGetSuperTypeOperatorPrecedence(
                                   string operatorName,
                                   out int precedence )
        {
            return _superTypeOperatorPrecedent.TryGetValue(
                operatorName, out precedence );
        }

        private static void InitPrecedence()
        {
            KeyValuePair<string, int>[] source = [
                new KeyValuePair<string, int>( PoolString( "[" ), 1 ),
                new KeyValuePair<string, int>( PoolString( "." ), 1 ),
                new KeyValuePair<string, int>( PoolString( "\\" ), 1 ),
                new KeyValuePair<string, int>( PoolString( "unary+" ), 2 ),
                new KeyValuePair<string, int>( PoolString( "unary-" ), 2 ),
                new KeyValuePair<string, int>( PoolString( "NOT" ), 2 ),
                new KeyValuePair<string, int>( PoolString( "**" ), 3 ),
                new KeyValuePair<string, int>( PoolString( "*" ), 4 ),
                new KeyValuePair<string, int>( PoolString( "/" ), 4 ),
                new KeyValuePair<string, int>( PoolString( "AND" ), 4 ),
                new KeyValuePair<string, int>( PoolString( "MOD" ), 4 ),
                new KeyValuePair<string, int>( PoolString( "DIV" ), 4 ),
                new KeyValuePair<string, int>( PoolString( "||" ), 4 ),
                new KeyValuePair<string, int>( PoolString( "+" ), 5 ),
                new KeyValuePair<string, int>( PoolString( "-" ), 5 ),
                new KeyValuePair<string, int>( PoolString( "OR" ), 5 ),
                new KeyValuePair<string, int>( PoolString( "XOR" ), 5 ),
                new KeyValuePair<string, int>( PoolString( "=" ), 6 ),
                new KeyValuePair<string, int>( PoolString( "<>" ), 6 ),
                new KeyValuePair<string, int>( PoolString( "<=" ), 6 ),
                new KeyValuePair<string, int>( PoolString( ">=" ), 6 ),
                new KeyValuePair<string, int>( PoolString( ">" ), 6 ),
                new KeyValuePair<string, int>( PoolString( "<" ), 6 ),
                new KeyValuePair<string, int>( PoolString( ":=:" ), 6 ),
                new KeyValuePair<string, int>( PoolString( ":<>:" ), 6 ),
                new KeyValuePair<string, int>( PoolString( "IN" ), 6 ),
                new KeyValuePair<string, int>( PoolString( "LIKE" ), 6 ),
                ];
            _operatorPrecedence = new Dictionary<string, int>(
                source, _multiplexer ).ToFrozenDictionary();
            source = [
                new KeyValuePair<string, int>( "ONEOF", 1 ),
                new KeyValuePair<string, int>( "AND", 2 ),
                new KeyValuePair<string, int>( "ANDOR", 3 ),
                ];
            _superTypeOperatorPrecedent = new Dictionary<string, int>(
                source, _multiplexer ).ToFrozenDictionary();
        }

        #pragma warning disable CS8618 
        private static FrozenDictionary<string, int> _superTypeOperatorPrecedent;
        private static FrozenDictionary<string, int> _operatorPrecedence;
        #pragma warning restore CS8618 
    }
}
