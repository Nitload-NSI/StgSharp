//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepDataSectionTokenReader.cs"
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
using System.Threading.Tasks;

namespace StgSharp.Modeling.Step
{
    public class StepDataSectionTokenReader : IScriptTokenReader
    {

        private ExpSourceReader _reader;
        private Queue<Token> _cache = new Queue<Token>();
        private StepDataSectionTransmitter _transmitter;

        public StepDataSectionTokenReader( StepDataSectionTransmitter transmitter )
        {
            _reader = new ExpSourceReader( transmitter );
            _transmitter = transmitter;
        }

        public Token ReadToken()
        {
            if( _cache.Count == 0 )
            {
                if( _reader.TryReadToken( out Token token0 ) && ( token0.Value != "." ) ) {
                    return token0;
                }
                if( _reader.TryReadToken( out Token token1 ) && ( token1.Value != "T" || token1.Value !=
                                                                  "F" ) )
                {
                    _cache.Enqueue( token1 );
                    return token0;
                }
                if( _reader.TryReadToken( out Token token2 ) && ( token2.Value != "." ) )
                {
                    _cache.Enqueue( token1 );
                    _cache.Enqueue( token2 );
                    return token0;
                }
                switch( token1.Value )
                {
                    case "T":
                        return new Token( ".T.", token0.Line, token1.Column, TokenFlag.Member );
                    case "F":
                        return new Token( ".F.", token0.Line, token1.Column, TokenFlag.Member );
                    default:
                        break;
                }
            }
            return _cache.Dequeue();
        }

        public bool TryReadToken( out Token t )
        {
            if( _cache.Count == 0 )
            {
                if( _reader.TryReadToken( out Token token0 ) )
                {
                    if( token0.Value != "." )
                    {
                        t = token0;
                        return true;
                    }
                } else
                {
                    t = Token.Empty;
                    return false;
                }
                if( _reader.TryReadToken( out Token token1 ) )
                {
                    if( token1.Value != "T" || token1.Value != "F" )
                    {
                        _cache.Enqueue( token1 );
                        t = token0;
                        return true;
                    }
                } else
                {
                    t = Token.Empty;
                    return false;
                }
                if( _reader.TryReadToken( out Token token2 ) )
                {
                    if( token2.Value != "." )
                    {
                        _cache.Enqueue( token1 );
                        _cache.Enqueue( token2 );
                        t = token0;
                        return true;
                    }
                } else
                {
                    t = Token.Empty;
                    return false;
                }
                t = token1.Value switch
                {
                    "T" => new Token( ".T.", token0.Line, token1.Column, TokenFlag.Member ),
                    "F" => new Token( ".F.", token0.Line, token1.Column, TokenFlag.Member ),
                    _ => Token.Empty
                };
            }
            return _reader.TryReadToken( out t );
        }

    }
}
