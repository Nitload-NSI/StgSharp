//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpCompile.cs"
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
using CommunityToolkit.HighPerformance.Buffers;

using StgSharp.Collections;

using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace StgSharp.Script.Express
{
    public static partial class ExpCompile
    {

        private static string[] _operatorsWithLetter;
        private static ConcurrentStringHashMultiplexer _multiplexer;
        private static FrozenDictionary<string, ExpElementInstanceBase> _builtinConst;
        private static FrozenDictionary<string, ExpTypeSource> _builtinType;
        private static FrozenDictionary<string, ExpBinaryOperatorNode> _operators;
        private static StringPool _compileStringCache;

        internal static ConcurrentStringHashMultiplexer Multiplexer
        {
            get
            {
                if( _multiplexer == null ) {
                    throw new ExpCompileNotInitializedException();
                }
                return _multiplexer;
            }
        }

        public static void ClearStringCache()
        {
            _compileStringCache.Reset();
        }

        public static void DeinitCompile()
        {
            _compileStringCache.Reset();
            _compileStringCache = null!;

            _multiplexer.Dispose();
            _multiplexer = null!;
        }

        public static void InitCompile()
        {
            _compileStringCache = new StringPool();
            _multiplexer = new ConcurrentStringHashMultiplexer();

            ExpSchema_Nitload.Only.Init();

            PoolKeywords();
            InitPrecedence();

            _operatorsWithLetter = [
                KeyWord.ADD,
                KeyWord.SUB,
                KeyWord.MUL,
                KeyWord.DIV,
                KeyWord.AND,
                KeyWord.OR,
                KeyWord.XOR,
                KeyWord.ANDOR,
                KeyWord.NOT,
                ];

            ExpSchema_Nitload.Only
                    .TryGetType(
                        PoolString( KeyWord.Int ), out ExpTypeSource? _expInt );
            ExpInt = _expInt;

            ExpSchema_Nitload.Only
                    .TryGetType(
                        PoolString( KeyWord.Real ),
                        out ExpTypeSource? _expReal );
            ExpReal = _expReal;
        }

        public static bool LetterIsOperator( string str )
        {
            if( char.IsLetter( str[ 0 ] ) ) {
                return Array.IndexOf( _operatorsWithLetter, str ) != -1;
            }
            return true;
        }

        public static string PoolString( string str )
        {
            return _compileStringCache.GetOrAdd( str );
        }

        public static string PoolString( ReadOnlySpan<char> str )
        {
            return _compileStringCache.GetOrAdd( str );
        }

    }
}
