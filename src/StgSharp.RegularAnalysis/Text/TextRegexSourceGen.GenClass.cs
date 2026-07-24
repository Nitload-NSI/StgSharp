// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        private const string current_ptr = "cur_ptr_global";
        private const string offset = "offset";
        private const string orig_text = "text";

        internal static SequenceEmitter<string> GenerateSource(
                                                TextRegexSource source
        )
        {
            SequenceEmitter<string> se = new();
            List<SequenceEmitter<string>> sub_match_list = [];
            SourceGenContext sc = new();

            RegexInfo info = source.Info;
            List<RegexIR> ir = source.IRs;
            ReadOnlySpan<RegexIR> ir_sp = CollectionsMarshal.AsSpan(ir);

            SequenceEmitter<string> match_e = new();
            int index = 0;
            int end = FindRegionEnd(ir_sp, index);
            ReadOnlySpan<RegexIR> region = ir_sp[0..end];

            while (true)
            {
                GenerateRegion(region, sc, match_e, sub_match_list);
                if (end >= ir_sp.Length)
                {
                    break;
                }
                index = end;
                end = FindRegionEnd(ir_sp, end);
                region = ir_sp[index..end];
            }

            _ = se.AppendLine($@"public override MatchResult Match({ROS_char} text)")//
                  .AppendLine(@"{");

            int region_count = info.RegionCount;
            for (int i = 0; i < region_count; i++)
            {
                _ = se.AppendLine($@"int region_{i}_start = -1;")//
                      .AppendLine($@"int region_{i}_end = -1;")
                      .AppendLine($@"bool region_{i}_success;");
            }

            _ = se.AppendLine($@"int {current_ptr} = 0;")
                  .AppendLine($@"int {offset} = 0;")
                  .AppendLine();

            _ = sc.GenVarDefine(se);

            _ = se.Append(match_e);

            foreach (SequenceEmitter<string> _sub in sub_match_list)
            {
                _ = se.AppendLine()                     //
                      .Append(_sub);
            }

            _ = se.Append(@"}");

            return se;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FindRegionEnd(
                           ReadOnlySpan<RegexIR> ir_sp,
                           int current
        )
        {
            int cur_depth = ir_sp[current].Prefix.WorkingRegion;
            int end = ir_sp.Length;
            for (; end > 0; end--)
            {
                if (ir_sp[end].Prefix.WorkingRegion == cur_depth) {
                    return end;
                }
            }
            return 1;
        }

        private static string FormatStringLiteral(
                              string str
        )
        {
            return SyntaxFactory.Literal(str).Text;
        }

        #region sub generator

        private static int TryGenerateFindSeq(
                           ReadOnlySpan<RegexIR> scan_win,
                           SequenceEmitter<string> se,
                           SourceGenContext ng
        )
        {
            if (scan_win[0] is not RegexFindSeqIR seq) {
                return -1;
            }
            string pattern = seq.Pattern;
            int length = pattern.Length;
            string index_caller = seq.IsFirst ? "IndexOf" : "LastIndexOf";

            _ = se.AppendLine()
                  .AppendLine($@"if(({offset} = [{current_ptr}..].{index_caller}({pattern})) != -1)")
                  .AppendLine($@"{{")
                  .AppendLine($@"{current_ptr} += {offset} + {length};")
                  .AppendLine($@"}}");
            return length;
        }

        private static int TryGenerateMatchSeq(
                           ReadOnlySpan<RegexIR> scan_win,
                           SequenceEmitter<string> se,
                           SourceGenContext ng
        )
        {
            if (scan_win[0] is not RegexFindSeqIR seq) {
                return -1;
            }
            string pattern = FormatStringLiteral(seq.Pattern);
            int length = pattern.Length;

            _ = se.AppendLine()
                  .AppendLine($@"if([{current_ptr}..({current_ptr} + {length})] == {pattern})")
                  .AppendLine($@"{{")
                  .AppendLine($@"{current_ptr} += {offset} + {length};")
                  .AppendLine($@"}}");
            return length;
        }

        private static int TryGenerateMatchSet(
                           ReadOnlySpan<RegexIR> scan_win,
                           SequenceEmitter<string> se,
                           SourceGenContext ng
        )
        {
            if (scan_win[0] is not RegexMatchSeqIR seq) {
                return -1;
            }
            string pattern = seq.Pattern;
            RegexCharSet[] set_pattern = FigureCharSetType(pattern, out bool accept);

            string var_char = ng.CurChar;
            _ = se.AppendLine($@"// match charset of {pattern}")
                  .AppendLine($@"char {var_char} = {orig_text}[{current_ptr}];")
                  .AppendLine();

            _ = se.AppendLine("if(");
            RegexCharSet p = set_pattern[0];
            string _case = GenerateCase(p);
            string _case_begin = accept ? string.Empty : "!(";
            _ = se.AppendLine($@"if ({_case_begin}{_case}||");

            for (int i = 1; i < set_pattern.Length - 1; i++)
            {
                p = set_pattern[i];
                _case = GenerateCase(p);
                if (string.IsNullOrEmpty(_case))
                {
                    continue;
                }
                _ = se.AppendLine($@"    {_case} ||");
            }

            p = set_pattern[^1];
            _case = GenerateCase(p);
            string _case_end = accept ? string.Empty : ")";
            _ = se.AppendLine($@"{_case}{_case_end})")
                  .AppendLine("{")
                  .AppendLine($@"{current_ptr} += 1;")
                  .AppendLine("}");

            return 1;


            string GenerateCase(
                   RegexCharSet c
            )
            {
                switch (c.Type)
                {
                    // Handle single character
                    case CharSetType.Single:
                        string set = c.Value;
                        if (set.Length == 1)
                        {
                            return $@"{var_char} == '{c.Value}'";
                        } else if (set.Length < 5)
                        {
                            StringBuilder sb = new();
                            _ = sb.Append('(');
                            foreach (char item in set) {
                                _ = sb.Append(CultureInfo.InvariantCulture, $@"item == '{item}' or ");
                            }
                            _ = sb.Append(')');
                            return sb.ToString();
                        } else
                        {
                            return $@"""{c.Value}"".AsSpan().Contains({var_char})";
                        }

                    // Handle range of characters
                    case CharSetType.Range:
                        return $@"({var_char} >= '{c.Value[0]}' && {var_char} <= '{c.Value[1]}')";

                    // Handle set of characters
                    case CharSetType.Set:
                        if (c.Accept)
                        {
                            return c.Value switch
                            {
                                "s" => $@"char.IsWhiteSpace({var_char})",
                                "w" => $@"char.IsLetterOrDigit({var_char}) || {var_char} == '_'",
                                "d" => $@"char.IsDigit({var_char})",
                                _ => throw new InvalidCastException(),
                            };
                        } else
                        {
                            return c.Value switch
                            {
                                "s" => $@"!char.IsWhiteSpace({var_char})",
                                "w" => $@"!char.IsLetterOrDigit({var_char}) && {var_char} != '_'",
                                "d" => $@"!char.IsDigit({var_char})",
                                _ => throw new InvalidCastException(),
                            };
                        }
                    case CharSetType.None:
                        return string.Empty;
                    default:
                        return string.Empty;
                }
            }
        }

            #endregion

        #region charset config

        internal enum CharSetType
        {

            None,
            Single,
            Range,
            Set

        }

        internal record struct RegexCharSet(
                               string Value,
                               CharSetType Type,
                               bool Accept
        )
        {

            public override readonly string ToString()
            {
                return $"{{value:{Value}, type:{Type}, accept:{Accept}}}";
            }

        }

        private static RegexCharSet[] FigureCharSetType(
                                      ReadOnlySpan<char> source,
                                      out bool accept
        )
        {
            // range or single
            if (source[0] == '[')
            {
                int idx;
                accept = source[1] != '^';
                idx = accept ? 1 : 2;
                ReadOnlySpan<char> seq = source.Slice(idx, source.Length - idx - 1);
                if (seq.IndexOf('-') == -1)
                {
                    return [new RegexCharSet(seq.ToString(), CharSetType.Single, true)];
                } else
                {
                    List<RegexCharSet> list = [];
                    for (; seq.Length > 0; )
                    {
                        int pos = seq.IndexOf('-');
                        if (pos == -1)
                        {
                            // has no range
                            SplitSet(seq, list);
                            break;
                        } else if (pos > 1)
                        {
                            // has other sequence before range
                            SplitSet(seq[0..(pos - 1)], list);
                        }
                        list.Add(new($"{seq[pos - 1]}{seq[pos + 1]}", CharSetType.Range, true));
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

            static void SplitSet(
                        ReadOnlySpan<char> seq,
                        List<RegexCharSet> sets
            )
            {
                if (seq.Length == 0 || sets == null) {
                    return;
                }
                int pos;
                ReadOnlySpan<char> remain = seq;
                while ((pos = remain.IndexOf('\\')) != -1 || remain.Length > 0)
                {
                    if (pos != 0)
                    {
                        ReadOnlySpan<char> singles = remain[0..pos];
                        sets.Add(new RegexCharSet(singles.ToString(), CharSetType.Single, true));
                    }
                    bool accept = char.IsAsciiLetterUpper(remain[pos + 1]);
                    RegexCharSet set_set = char.ToLower(remain[pos + 1], CultureInfo.InvariantCulture)
                        switch
                    {
                        's' => new("s", CharSetType.Set, accept),
                        'w' => new("w", CharSetType.Set, accept),
                        'd' => new("d", CharSetType.Set, accept),
                        _ => throw new NotSupportedException()
                    };
                    sets.Add(set_set);
                    remain = remain[(pos + 2)..];
                }
                if (remain.Length > 0) {
                    sets.Add(new RegexCharSet(remain.ToString(), CharSetType.Single, true));
                }
            }
        }

        #endregion
    }
}
