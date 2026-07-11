// -----------------------------------------------------------------------------
// file="RegexAnalyzer.SourceGen.CharSetEmitter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal enum CharSetType
    {

        None,
        Single,
        Range,
        Set

    }

    internal record struct RegexCharSet(
                           string value,
                           CharSetType t,
                           bool accept
    )
    {

        public bool IsAcceptable { get; set; } = accept;

        public CharSetType Type { get; set; } = t;

        public string Value { get; set; } = value;

        public override readonly string ToString()
        {
            return $"{{value:{value}, type:{Type}, accept:{IsAcceptable}}}";
        }

    }

    internal static partial class RegexAnalyzerBackEnd
    {

        public static RegexCharSet[] FigureCharSetType(
                                     ReadOnlySpan<char> source
        )
        {
            // range or single
            bool accept;
            if (source[0] == '[')
            {
                int idx;
                accept = source[1] != '^';
                idx = accept ? 1 : 2;
                ReadOnlySpan<char> seq = source.Slice(idx, source.Length - idx - 1);
                if (seq.IndexOf('-') == -1)
                {
                    return [new RegexCharSet(seq.ToString(), CharSetType.Single, accept)];
                } else
                {
                    List<RegexCharSet> list = [];
                    for (; seq.Length > 0; )
                    {
                        int pos = seq.IndexOf('-');
                        if (pos == -1)
                        {
                            RegexCharSet single = new(seq.ToString(), CharSetType.Single, accept);
                            list.Add(single);
                            break;
                        } else
                        if (pos > 1) {
                            list.Add(new(seq[0..(pos - 1)].ToString(), CharSetType.Single, accept));
                        }
                        list.Add(new($"{seq[pos - 1]}{seq[pos + 1]}", CharSetType.Range, accept));
                        seq = seq[(pos + 2)..];
                    }
                    return [.. list];
                }
            } else if (source[0] == '\\')
            {
                // set
                accept = char.IsAsciiLetterUpper(source[0]);
                return char.ToLower(source[0], CultureInfo.InvariantCulture)
                switch
                {
                    's' => [new("s", CharSetType.Set, accept)],
                    'w' => [new("w", CharSetType.Set, accept)],
                    'd' => [new("d", CharSetType.Set, accept)],
                    _ => throw new NotSupportedException()
                };
            } else
            {
                // unknown, throw
                throw new InvalidCastException($"Unknown char set: {source}");
            }
        }

    }
}
