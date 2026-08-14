// -----------------------------------------------------------------------------
// file="RegexAstNode.AdditionalPayload"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StgSharp.RegularAnalysis.Text
{
    public abstract record RegexAstPayload(
                           string Source,
                           int Row,
                           int Column,
                           RegexElementLabel Flag
    ) : ISyntaxPayload<RegexElementLabel>;

    public sealed record RegexEmptyPayload() : RegexAstPayload(string.Empty, 0, 0,
                                                               RegexElementLabel.NONE);

    /// <summary>
    ///   A deterministic literal sequence. Escapes represented as literals are consumed here.
    /// </summary>
    public sealed record RegexLiteralPayload(
                         string Text,
                         string Source,
                         int Row,
                         int Column,
                         RegexElementLabel Flag
    ) : RegexAstPayload(Source, Row, Column, Flag);

    public enum RegexCharSetType
    {

        None,
        Single,
        Range,
        Set,
        Any

    }

    public sealed record RegexCharSet(
                         string Value,
                         RegexCharSetType Type,
                         bool Accept
    );

    /// <summary>
    ///   A character-set atom expanded into the rules formerly produced by FigureCharSetType.
    /// </summary>
    public sealed record RegexCharSetPayload(
                         IReadOnlyList<RegexCharSet> Set,
                         bool Accept,
                         string Source,
                         int Row,
                         int Column
    ) : RegexAstPayload(Source, Row, Column, RegexElementLabel.UNIT_SET)
    {

        public bool IsAny => Set.Count == 1 && Set[0].Type == RegexCharSetType.Any;

    }

    public sealed record RegexCountPayload(
                         int Min,
                         int Max,
                         bool IsGreedy,
                         string Source,
                         int Row,
                         int Column
    ) : RegexAstPayload(Source, Row, Column, RegexElementLabel.COUNT)
    {

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

    }

    public sealed record RegexGroupPayload(
                         string Name,
                         bool IsCapturing,
                         string Source,
                         int Row,
                         int Column
    ) : RegexAstPayload(Source, Row, Column, RegexElementLabel.GROUP_BEGIN);

    public sealed record RegexCommonPayload(
                         string Source,
                         int Row,
                         int Column,
                         RegexElementLabel Flag
    ) : RegexAstPayload(Source, Row, Column, Flag)
    {

        public static RegexAstPayload FromToken(
                                      Token<RegexElementLabel> token
        )
        {
            return token.Flag switch
            {
                RegexElementLabel.UNIT or RegexElementLabel.UNIT_SPAN => new RegexLiteralPayload(DecodeLiteral(token.Value),
                                                                                                 token.Value,
                                                                                                 token.Line,
                                                                                                 token.Column,
                                                                                                 token.Flag),
                RegexElementLabel.UNIT_SET => ParseCharSet(token),
                RegexElementLabel.COUNT => ParseCount(token),
                RegexElementLabel.GROUP_BEGIN => ParseGroup(token),
                _ => new RegexCommonPayload(token.Value, token.Line, token.Column, token.Flag)
            };
        }

        private static void AddEscape(
                            char escaped,
                            List<RegexCharSet> rules
        )
        {
            bool ruleAccept = char.IsAsciiLetterLower(escaped);
            char kind = char.ToLower(escaped, CultureInfo.InvariantCulture);
            if (!IsNamedSet(escaped)) {
                throw new InvalidCastException($"Unknown character set escape: \\{escaped}");
            }
            rules.Add(new(kind.ToString(), RegexCharSetType.Set, ruleAccept));
        }

        private static char DecodeEscapedChar(
                            char value
        )
        {
            return value switch
            {
                'n' or 'r' => throw new NotSupportedException(
                    "Multi-line regular expressions are not supported yet."),
                't' => '\t',
                '0' => '\0',
                _ => value
            };
        }

        private static string DecodeLiteral(
                              string source
        )
        {
            if (source.Length != 2 || source[0] != '\\') {
                return source;
            }
            return source[1] switch
            {
                'n' => "\n",
                'r' => "\r",
                't' => "\t",
                '0' => "\0",
                _ => source[1].ToString()
            };
        }

        private static bool IsNamedSet(
                            char value
        )
        {
            return
            char.ToLower(value, CultureInfo.InvariantCulture) is 's' or 'w' or 'd';
        }

        private static void ParseBracketContent(
                            ReadOnlySpan<char> content,
                            List<RegexCharSet> rules
        )
        {
            HashSet<char> singles = [];
            int index = 0;
            while (index < content.Length)
            {
                if (content[index] is '\r' or '\n') {
                    throw new NotSupportedException(
                        "Multi-line regular expressions are not supported yet.");
                }
                if (content[index] == '\\')
                {
                    if (++index >= content.Length) {
                        throw new InvalidCastException("Unclosed escape in character set.");
                    }
                    char escaped = content[index++];
                    if (IsNamedSet(escaped))
                    {
                        AddEscape(escaped, rules);
                    } else
                    {
                        _ = singles.Add(DecodeEscapedChar(escaped));
                    }
                    continue;
                }

                char first = content[index++];
                if (index < content.Length - 0 &&
                    content[index] == '-' &&
                    index + 1 < content.Length)
                {
                    index++;
                    char last;
                    if (content[index] == '\\')
                    {
                        if (++index >= content.Length) {
                            throw new InvalidCastException("Unclosed range in character set.");
                        }
                        last = DecodeEscapedChar(content[index++]);
                    } else
                    {
                        last = content[index++];
                    }
                    if (last < first) {
                        throw new InvalidCastException($"Invalid character range: {first}-{last}");
                    }
                    rules.Add(new(content[first..last].ToString(), RegexCharSetType.Range, true));
                } else
                {
                    _ = singles.Add(first);
                }
            }

            if (singles.Count > 0)
            {
                StringBuilder text = new(singles.Count);
                foreach (char value in singles) {
                    _ = text.Append(value);
                }
                rules.Add(new(text.ToString(), RegexCharSetType.Single, true));
            }
        }

        private static RegexCharSetPayload ParseCharSet(
                                           Token<RegexElementLabel> token
        )
        {
            string source = token.Value;
            List<RegexCharSet> rules = [];
            bool accept = true;

            if (source == ".")
            {
                rules.Add(new(string.Empty, RegexCharSetType.Any, true));
            } else if (source.Length >= 2 && source[0] == '[' && source[^1] == ']')
            {
                int start = source.Length > 2 && source[1] == '^' ? 2 : 1;
                accept = start == 1;
                ParseBracketContent(source.AsSpan(start, source.Length - start - 1), rules);
            } else if (source.Length == 2 && source[0] == '\\')
            {
                AddEscape(source[1], rules);
            } else
            {
                throw new InvalidCastException($"Unknown char set: {source}");
            }

            return new RegexCharSetPayload(rules.ToArray(), accept, source, token.Line,
                                           token.Column);
        }

        private static RegexCountPayload ParseCount(
                                         Token<RegexElementLabel> token
        )
        {
            string source = token.Value;
            bool greedy = source.Length == 1 || source[^1] != '?';
            ReadOnlySpan<char> count = greedy ?
                                       source.AsSpan() :
                                       source.AsSpan(0, source.Length - 1);
            return count[0] switch
            {
                '*' => new RegexCountPayload(0, -1, greedy, source, token.Line, token.Column),
                '+' => new RegexCountPayload(1, -1, greedy, source, token.Line, token.Column),
                '?' => new RegexCountPayload(0, 1, greedy, source, token.Line, token.Column),
                '{' => ParseRange(count, greedy, token),
                _ => throw new InvalidOperationException($"Invalid count format: {source}")
            };
        }

        private static RegexGroupPayload ParseGroup(
                                         Token<RegexElementLabel> token
        )
        {
            string source = token.Value;
            bool named = source.StartsWith("(?<", StringComparison.Ordinal);
            string name = named ? source[3..^1] : string.Empty;
            return new RegexGroupPayload(name, source != "(?:", source, token.Line, token.Column);
        }

        private static RegexCountPayload ParseRange(
                                         ReadOnlySpan<char> count,
                                         bool greedy,
                                         Token<RegexElementLabel> token
        )
        {
            if (count.Length < 3 || count[^1] != '}') {
                throw new InvalidOperationException($"Invalid count format: {token.Value}");
            }
            ReadOnlySpan<char> body = count[1..^1];
            int comma = body.IndexOf(',');
            if (comma < 0)
            {
                if (!int.TryParse(body, out int exact)) {
                    throw new InvalidOperationException($"Invalid count format: {token.Value}");
                }
                return new RegexCountPayload(exact, exact, greedy, token.Value, token.Line,
                                             token.Column);
            }
            if (!int.TryParse(body[..comma], out int min)) {
                throw new InvalidOperationException($"Invalid count format: {token.Value}");
            }
            ReadOnlySpan<char> maxText = body[(comma + 1)..];
            int max = maxText.IsEmpty ?
                      -1 :
                      int.TryParse(maxText, out int parsed) ?
                      parsed :
                      throw new InvalidOperationException($"Invalid count format: {token.Value}");
            if (max >= 0 && max < min) {
                throw new InvalidOperationException($"Invalid count range: {token.Value}");
            }
            return new RegexCountPayload(min, max, greedy, token.Value, token.Line, token.Column);
        }

    }
}
