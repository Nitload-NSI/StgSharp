//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepDataTokenReader.cs"
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
using StgSharp.Modeling.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Modeling.Step
{
    public class StepDataTokenReader : IScriptTokenReader
    {

        private ExpTokenReader _reader;

        private HashSet<string> token1Hash = [
            "T","F","UNSPECIFIED"
            ];
        private Queue<Token> _cache = new Queue<Token>();
        private StepDataSectionTransmitter _transmitter;

        public StepDataTokenReader( StepDataSectionTransmitter transmitter )
        {
            _reader = new ExpTokenReader( transmitter );
            _reader.TokenReaderPreProcessor = new TokenPreReader();
            _transmitter = transmitter;
        }

        public bool IsEmpty
        {
            get => _reader.IsEmpty && _transmitter.IsWriting;
        }

        public Token ReadToken()
        {
            return _reader.ReadToken();
        }

        public bool TryReadToken( out Token t )
        {
            return _reader.TryReadToken( out t );
        }

        private class TokenPreReader : ExpTokenReaderPreProcessor
        {

            private static Regex NextEnumPattern = GetNextEnumPattern();

            public override bool Process( string codeLine, int line, ref int current, out Token t )
            {
                ReadOnlySpan<char> chars = codeLine.AsSpan( current );
                char c = chars[ 0 ];
                if( c != '.' )
                {
                    t = Token.Empty;
                    return false;
                }
                Match match = NextEnumPattern.Match( codeLine, current );
                if( match.Success )
                {
                    int begin = match.Groups[ "begin" ].Index;
                    int end = match.Groups[ "end" ].Length;
                    int rest = match.Groups[ "rets" ].Length;
                    current = rest;
                    t = new Token( match.Groups[ "begin" ].Value, line, begin, TokenFlag.Enum );
                    return true;
                } else
                {
                    t = Token.Empty;
                    return false;
                }
            }

            [GeneratedRegex(
                    @"(?<begin>\.[A-Za-z_]+\.)(?<end>\s*)(?<rest>\S*)",
                    RegexOptions.Singleline )]
            private static extern Regex GetNextEnumPattern();

        }

    }
}
