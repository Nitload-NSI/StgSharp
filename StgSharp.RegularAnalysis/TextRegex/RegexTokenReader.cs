//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexTokenReader"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.TextRegex
{
    internal class RegexTokenReader : ITokenReader<RegexElementLabel>
    {

        private static string _source;

        private int position = 0;

        public RegexTokenReader() { }

        public RegexTokenReader(
               string source
        )
        {
            _source = source;
        }

        public bool IsEmpty => string.IsNullOrEmpty(_source) || position >= _source.Length;

        public Token<RegexElementLabel> ReadToken()
        {
            int pos = position;
            switch (_source[pos])
            {
                case '\\':
                    if (pos + 1 == _source.Length) {
                        throw new InvalidOperationException("Unclosed char");
                    }
                    position = pos + 2;
                    return new Token<RegexElementLabel>(_source[(pos)..(pos + 2)], 0, pos,
                                                        RegexElementLabel.UNIT);
                case '[':
                    for (int i = pos + 1; i < _source.Length; i++)
                    {
                        if (_source[i] == ']' && _source[i - 1] != '\\')
                        {
                            int length = (i - pos - 1);
                            position = i + 1;
                            return new Token<RegexElementLabel>(_source[(pos - 1)..(i - 1)], 0,
                                                                position,
                                                                RegexElementLabel.UNIT_SET);
                        }
                    }
                    throw new InvalidOperationException("Unclosed character set");
                case '(':

                    // very comlex here
                    return ReadGroupBeginToken();
                case ')':
                    position = pos + 1;
                    return new Token<RegexElementLabel>(_source[(pos)..(pos + 1)], 0, pos,
                                                        RegexElementLabel.GROUP_END);
                case '{':
                    for (int i = pos + 1; i < _source.Length; i++)
                    {
                        if (_source[i] == '}')
                        {
                            position = i + 1;
                            return new Token<RegexElementLabel>(_source[(pos)..(i - 1)], 0, pos,
                                                                RegexElementLabel.COUNT);
                        }
                    }
                    throw new InvalidOperationException("Unclosed count specifier");
                case '*' or '+':
                    int count = _source[pos + 1] == '?' ? 2 : 1;
                    position = pos + count;
                    return new Token<RegexElementLabel>(_source[(pos)..(pos + count)], 0, pos,
                                                        RegexElementLabel.COUNT);
                case '?':
                    position = pos + 1;
                    return new Token<RegexElementLabel>(_source[(pos)..(pos + 1)], 0, pos,
                                                        RegexElementLabel.COUNT);
                case '.':
                    return new Token<RegexElementLabel>(_source[(pos)..(pos + 1)], 0, pos,
                                                        RegexElementLabel.UNIT);
                default:
                    position = pos + 1;
                    return new Token<RegexElementLabel>(_source[(pos)..(pos + 1)], 0, pos,
                                                        RegexElementLabel.UNIT);
            }
        }

        public bool TryReadToken(
                    out Token<RegexElementLabel> t
        )
        {
            if (IsEmpty)
            {
                t = Token<RegexElementLabel>.Empty;
                return false;
            }
            t = ReadToken();
            return Token<RegexElementLabel>.Empty == t;
        }

        private Token<RegexElementLabel> ReadGroupBeginToken()
        {
            if (IsEmpty) {
                throw new InvalidOperationException("Source is null.");
            }

            int start = position;

            if (start >= _source.Length || _source[start] != '(') {
                throw new InvalidOperationException("ReadGroupBeginToken called when current char is not '('.");
            }

            // Case 1: plain capturing group "("
            if (start + 1 >= _source.Length || _source[start + 1] != '?')
            {
                position = start + 1;
                return new Token<RegexElementLabel>("(", 0, start, RegexElementLabel.GROUP_BEGIN);
            }

            // From here we have "(?"
            if (start + 2 >= _source.Length)
            {
                throw new InvalidOperationException("Unfinished group prefix '(?'.");
            }

            char c2 = _source[start + 2];

            // Case 2: non-capturing group "(?:"
            if (c2 == ':')
            {
                position = start + 3;
                return new Token<RegexElementLabel>("(?:", 0, start, RegexElementLabel.GROUP_BEGIN);
            }

            // Assertions (write the first-level cases, but not supported yet)
            if (c2 == '=')
            {
                // "(?="
                position = start + 3;
                throw new NotSupportedException("Positive lookahead '(?=...)' is not supported yet.");
            }
            if (c2 == '!')
            {
                // "(?!"
                position = start + 3;
                throw new NotSupportedException("Negative lookahead '(?!...)' is not supported yet.");
            }

            // Named capturing group or lookbehind starts with "(?<"
            if (c2 == '<')
            {
                if (start + 3 >= _source.Length) {
                    throw new InvalidOperationException("Unfinished group prefix '(?<'.");
                }

                char c3 = _source[start + 3];

                // Lookbehind prefixes: "(?<=" and "(?<!"
                if (c3 == '=')
                {
                    position = start + 4;
                    throw new NotSupportedException("Positive lookbehind '(?<=...)' is not supported yet.");
                }
                if (c3 == '!')
                {
                    position = start + 4;
                    throw new NotSupportedException("Negative lookbehind '(?<!...)' is not supported yet.");
                }

                // Otherwise: "(?<name>"
                int nameStart = start + 3;

                // Read name until '>'
                int i = nameStart;

                // Name must start with [A-Za-z_]
                if (i >= _source.Length)
                {
                    throw new InvalidOperationException("Unfinished named group prefix.");
                }

                char first = _source[i];
                if (!(first == '_' || char.IsLetter(first))) {
                    throw new InvalidOperationException($"Invalid named group name start '{first}' at position {i}.");
                }

                i++;

                while (i < _source.Length)
                {
                    char ch = _source[i];
                    if (ch == '>')
                    {
                        break;
                    }

                    if (!(ch == '_' || char.IsLetterOrDigit(ch))) {
                        throw new InvalidOperationException($"Invalid character '{ch}' in group name at position {i}.");
                    }

                    i++;
                }

                if (i >= _source.Length || _source[i] != '>') {
                    throw new InvalidOperationException("Unclosed named group prefix; missing '>'.");
                }

                // Consume "(?<name>"
                int consumedLen = (i - start) + 1; // include '>'
                string text = _source.Substring(start, consumedLen);
                position = start + consumedLen;

                return new Token<RegexElementLabel>(text, 0, start, RegexElementLabel.GROUP_BEGIN);
            }

            // Optional: comments "(?#...)" or inline options "(?i:...)" etc.
            // First-level cases listed but not supported yet:
            if (c2 == '#')
            {
                position = start + 3;
                throw new NotSupportedException("Comment group '(?#...)' is not supported yet.");
            }

            // Inline options: (?i:...), (?m:...), (?-i:...), (?im), etc.
            // Too many dialect differences; reserve the prefix but mark unsupported.
            if (char.IsLetter(c2) || c2 == '-')
            {
                // We don't know how long the option spec is here; do NOT advance position too much.
                // Keep position at '(' so caller can decide how to handle, or just throw.
                position = start;
                throw new NotSupportedException("Inline option groups like '(?i:...)' are not supported yet.");
            }

            // Unknown "(?X" prefix
            position = start;
            throw new InvalidOperationException($"Unknown or unsupported group prefix starting at position {start}: '{_source.Substring(start, Math.Min(4, _source.Length - start))}'");
        }

    }
}
