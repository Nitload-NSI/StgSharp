//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSourceReader.cs"
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
using CommunityToolkit.HighPerformance;

using StgSharp.HighPerformance;

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace StgSharp.Script.Express
{
    public partial class ExpSourceReader : IScriptTokenReader
    {

        private const int
            MultiLineEnd = 0x0029002A;                          //  *)

        private const string TokenMark = "token", EndMark = "end", RestMark = "rest";

        private bool _isReadingMultiLineComment;
        private int _current, _lineNum;
        private IScriptSourceProvider _provider;

        private Regex _nextWordTokenPattern = GetNextWordTokenPattern(),
            _nextStringLiteralPattern = GetStringLiteralPattern(),
            _nextNumberLiteralPattern = GetNumberLiteralPattern(),
            _nextStringLiteralPatternWithSingle = GetStringLiteralPatternWithSingle(),
            _skipBlankPattern = GetSkipBlankPattern(),
            _nextSymbolPattern = GetSymbolPattern();
        private string _lineCache;
        private Token _last;

        public ExpSourceReader(IScriptSourceProvider provider)
        {
            _provider = provider;
            _current = 0;
            _lineCache = string.Empty;
            _isReadingMultiLineComment = false;
        }

        public bool IsEmpty
        {
            get => ((string.IsNullOrEmpty(_lineCache) || _current >= _lineCache.Length)) && _provider.IsEmpty;
        }

        public int LineNumber
        {
            get => _provider.Location;
        }

        public unsafe ReadOnlySpan<char> ReadLine()
        {
            bool lineOver = string.IsNullOrEmpty(_lineCache) || _current >= _lineCache.Length;
            while (lineOver || _isReadingMultiLineComment)
            {
                if (IsEmpty)
                {
                    return ReadOnlySpan<char>.Empty;
                }
                if (lineOver)
                {
                    do
                    {
                        _lineCache = _provider.ReadLine(out _lineNum);
                    } while (string.IsNullOrEmpty(_lineCache));
                    _current = 0;
                }
                if (_isReadingMultiLineComment)
                {
                    int index;
                    fixed (char* cPtr = _lineCache.AsSpan(_current))
                    {
                        index = UnsafeCompute.IndexOfCharPair(
                            cPtr, MultiLineEnd, _lineCache.Length);
                    }
                    if (index == -1)
                    {
                        _lineCache = string.Empty;
                    }
                    else
                    {
                        _isReadingMultiLineComment = false;
                        _current += index + 2;
                    }
                }
                lineOver = string.IsNullOrEmpty(_lineCache) || _current >= _lineCache.Length;
            }
            return _lineCache.AsSpan().Slice(_current);
        }

        public unsafe Token ReadToken()
        {
            int begin, end, rest;
        readLine:
            ReadOnlySpan<char> source;
            if (!TryReadLine(out source))
            {
                return Token.Empty;
            }
            char c = source[0];

            if (char.IsLetter(c))
            {
                Match match = _nextWordTokenPattern.Match(_lineCache, _current);
                if (match.Success)
                {
                    begin = match.Groups[TokenMark].Index;
                    end = match.Groups[EndMark].Index;
                    rest = match.Groups[RestMark].Index;
                    _current = rest;
                    _last = new(
                        ExpressCompile.PoolString(source[..(end - begin)]), _lineNum, begin,
                        TokenFlag.Member);
                    return _last;
                }
                else
                {
                    throw new ExpInvalidCharException(_current);
                }
            }
            else if (char.IsPunctuation(c) || char.IsSymbol(c))
            {
                switch (c)
                {
                    case '\'':

                    case '"':
                        char cNext = source[1];
                        if (cNext == '\"')                                     //empty string
                        {
                            begin = _current;
                            _current += 2;
                            return new(string.Empty, _lineNum, begin, TokenFlag.String);
                        }
                        Match match = _nextStringLiteralPattern.Match(_lineCache, _current);
                        if (match.Success)
                        {
                            begin = match.Groups["token"].Index;
                            end = match.Groups["end"].Index;
                            rest = match.Groups["rest"].Index;
                            _current = rest;
                            _last = new(
                                ExpressCompile.PoolString(source[1..(end - begin)]), _lineNum,
                                begin, TokenFlag.String);
                            return _last;
                        }
                        else
                        {
                            throw new ExpInvalidCharException('\"');
                        }
                    case '(':
                        cNext = source[1];
                        if (cNext == '*')
                        {
                            _isReadingMultiLineComment = true;
                            _current += 2;
                            goto readLine;
                        }
                        else
                        {
                            begin = _current;
                            _current++;
                            _last = new(
                                ExpressCompile.PoolString(source[..1]), _lineNum, begin,
                                TokenFlag.Separator_Left);
                            return _last;
                        }
                    case ')' or '}':
                        begin = _current;
                        _current++;
                        _last = new Token(
                            ExpressCompile.PoolString(source[..1]), _lineNum, begin,
                            TokenFlag.Separator_Right);
                        return _last;
                    case '-':

                        // 1. - number
                        // 2. -- comment
                        // 3. - number type param(UNARY)
                        // 4. param - param(BINARY)
                        cNext = source[1];
                        if (char.IsNumber(cNext))
                        {
                            match = _nextNumberLiteralPattern.Match(_lineCache, _current);
                            if (match.Success)
                            {
                                begin = match.Groups[TokenMark].Index;
                                end = match.Groups[EndMark].Index;
                                rest = match.Groups[RestMark].Index;
                                _current = rest;
                                return new(
                                    ExpressCompile.PoolString(source[..(end - begin)]),
                                    _lineNum, begin, TokenFlag.Number);
                            }
                            else
                            {
                                throw new ExpInvalidCharException(_current);
                            }
                        }
                        else if (cNext == '-')
                        {
                            _lineCache = string.Empty;
                            _current = 0;
                            goto readLine;
                        }
                        else
                        {
                            begin = _current;
                            _current++;
                            if (_last.Flag == TokenFlag.Member || _last.Flag ==
                                TokenFlag.Index_Right || _last.Flag == TokenFlag.Separator_Right)
                            {
                                _last = new(
                                    ExpressCompile.PoolString(source[..1]), _lineNum, begin,
                                    TokenFlag.Symbol_Binary);
                            }
                            else
                            {
                                _last = new(
                                    ExpressCompile.PoolString(source[..1]), _lineNum, begin,
                                    TokenFlag.Symbol_Unary);
                            }
                            return _last;
                        }
                    case '{':
                        begin = _current;
                        _current++;
                        _last = new Token(
                            ExpressCompile.PoolString("{"), _lineNum, begin,
                            TokenFlag.Separator_Left);
                        return _last;
                    case '[':
                        begin = _current;
                        _current++;
                        _last = new Token(
                            ExpressCompile.PoolString("["), _lineNum, begin,
                            TokenFlag.Index_Left);
                        return _last;
                    case ']':
                        begin = _current;
                        _current++;
                        _last = new Token(
                            ExpressCompile.PoolString("]"), _lineNum, begin,
                            TokenFlag.Index_Right);
                        return _last;
                    case ';' or ',':
                        begin = _current;
                        _current++;
                        _last = new Token(
                            ExpressCompile.PoolString(source[..1]), _lineNum, begin,
                            TokenFlag.Separator_Middle);
                        return _last;
                    case ':':
                        match = _nextSymbolPattern.Match(_lineCache, _current);
                        begin = match.Groups[TokenMark].Index;
                        end = match.Groups[EndMark].Index;
                        rest = match.Groups[RestMark].Index;
                        _current = rest;
                        if (end - begin == 1)
                        {
                            _last = new Token(
                                ExpressCompile.Keyword.Colon, _lineNum, begin,
                                TokenFlag.Separator_Middle);
                        }
                        else
                        {
                            _last = new Token(
                                ExpressCompile.PoolString(source[..(end - begin)]), _lineNum,
                                begin, TokenFlag.Symbol_Binary);
                        }
                        return _last;
                    case '+' or '/' or '\\' or '*' or '.':
                        match = _nextSymbolPattern.Match(_lineCache, _current);
                        begin = match.Groups[TokenMark].Index;
                        end = match.Groups[EndMark].Index;
                        rest = match.Groups[RestMark].Index;
                        _current = rest;
                        _last = new Token(
                            ExpressCompile.PoolString(source[..(end - begin)]), _lineNum,
                            begin, TokenFlag.Symbol_Binary);
                        return _last;
                    case '=':
                        begin = _current;
                        _current++;
                        _last = new Token(
                            ExpressCompile.Keyword.Equal, _lineNum, begin,
                            TokenFlag.Symbol_Binary);
                        return _last;
                    default:
                        throw new ExpInvalidCharException(c);
                }
            }
            else if (char.IsNumber(c))
            {
                Match match = _nextNumberLiteralPattern.Match(_lineCache, _current);
                if (match.Success)
                {
                    begin = match.Groups[TokenMark].Index;
                    end = match.Groups[EndMark].Index;
                    rest = match.Groups[RestMark].Index;
                    _current = rest;
                    string str = ExpressCompile.PoolString(source[..(end - begin)]);
                    return new(str, _lineNum, begin, TokenFlag.Number);
                }
                else
                {
                    throw new ExpInvalidCharException(_current);
                }
            }
            else if (char.IsWhiteSpace(c))
            {
                Match match = _skipBlankPattern.Match(_lineCache, _current);
                if (match.Success)
                {
                    rest = match.Groups[RestMark].Index;
                    _current = rest;
                    goto readLine;
                }
                else
                {
                    throw new ExpInvalidCharException(_current);
                }
            }
            else
            {
                throw new ExpInvalidCharException(c);
            }
        }

        public bool TryReadLine(out ReadOnlySpan<char> line)
        {
            line = ReadLine();
            return line != ReadOnlySpan<char>.Empty;
        }

        public bool TryReadToken(out Token t)
        {
            t = ReadToken();
            return !IsEmpty;
        }

        [GeneratedRegex(
                @"(?<token>[a-zA-Z][a-zA-Z0-9_]*)(?<end>\s*?)(?<rest>\S|$)",
                RegexOptions.Singleline)]
        private static partial Regex GetNextWordTokenPattern();

        [GeneratedRegex(
                @"(?<token>-?\s*?[0-9]+(?:\.[0-9]*)?E?(?:[+-]?[0-9]+)?)(?<end>\s*?)(?<rest>\S|$)",
                RegexOptions.Singleline)]
        private static partial Regex GetNumberLiteralPattern();

        [GeneratedRegex(@"(?:\s+?)(?<rest>\S|$)", RegexOptions.Singleline)]
        private static partial Regex GetSkipBlankPattern();

        [GeneratedRegex(
                @"(?<token>"")(?>\\\\|\\""|(?!')[^""])*(?<end>""|$)\s*?(?<rest>\S|$)",
                RegexOptions.Singleline)]
        private static partial Regex GetStringLiteralPattern();

        [GeneratedRegex(
                @"(?<token>')(?>\\\\|\\'|''|(?!')[^'])*(?<end>')\s*?(?<rest>\S|$)",
                RegexOptions.Singleline)]
        private static partial Regex GetStringLiteralPatternWithSingle();

        [GeneratedRegex(
                @"(?<token>[:<>=+\-*\/]+)(?<end>\s?)\s*?(?<rest>\S|$)",
                RegexOptions.Singleline)]
        private static partial Regex GetSymbolPattern();

    }
}
