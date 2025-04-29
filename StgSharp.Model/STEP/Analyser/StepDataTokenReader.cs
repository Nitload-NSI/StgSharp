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
using StgSharp.Model.Step;
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

namespace StgSharp.Model.Step
{
    public partial class StepDataTokenReader : IScriptTokenReader
    {

        private ExpTokenReader _reader;

        private HashSet<string> tokenHash = [
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
            get
            {
                if( _reader.IsEmpty ) {
                    return !_transmitter.IsWriting;
                }
                return false;
            }
        }

        public Token ReadToken()
        {
            return _reader.ReadToken();
        }

        public bool TryReadToken( out Token t )
        {
            return _reader.TryReadToken( out t );
        }

        private partial class TokenPreReader : ExpTokenReaderPreProcessor
        {

            private static Regex NextEnumPattern = GetNextEnumPattern();

            private static Regex _stringPattern = GetStringPattern();

            public override bool Process( string codeLine, int line, ref int current, out Token t )
            {
                ReadOnlySpan<char> chars = codeLine.AsSpan( current );
                char c = chars[ 0 ];
                switch( c )
                {
                    case '.':
                        Match match = NextEnumPattern.Match( codeLine, current );
                        if( match.Success )
                        {
                            int begin = match.Groups[ "token" ].Index;
                            int end = match.Groups[ "end" ].Index;
                            int rest = match.Groups[ "rest" ].Index;
                            current = rest;
                            t = new Token(
                                match.Groups[ "token" ].Value, line, begin, TokenFlag.Enum );
                            return true;
                        }
                        break;
                    case '$':
                        t = new Token( "$", line, current, TokenFlag.Member );
                        current++;
                        return true;
                    case '*':
                        t = new Token( "*", line, current, TokenFlag.Member );
                        current++;
                        return true;
                    case '\'':
                        match = _stringPattern.Match( codeLine, current );
                        if( match.Success )
                        {
                            int begin = match.Groups[ "token" ].Index;
                            int end = match.Groups[ "end" ].Index;
                            int rest = match.Groups[ "rest" ].Index;
                            current = rest;
                            string value = chars.Slice( 1, ( end - begin - 1 ) ).ToString();
                            if( string.IsNullOrWhiteSpace( value ) ) {
                                value = string.Empty;
                            }
                            t = new Token( value, line, begin, TokenFlag.String );
                            return true;
                        }
                        break;
                    default:
                        break;
                }
                t = Token.Empty;
                return false;
            }

            [GeneratedRegex(
                    @"(?<token>\.[A-Za-z_]+\.)(?<end>\s*)(?<rest>\S*)",
                    RegexOptions.Singleline )]
            private static partial Regex GetNextEnumPattern();

            [GeneratedRegex(
                    @"(?<token>')(?>\\\\|''|(?!')[^'])*(?<end>')\s*?(?<rest>\S|$)",
                    RegexOptions.Singleline )]
            private static partial Regex GetStringPattern();

        }

    }
}
