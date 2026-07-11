// -----------------------------------------------------------------------------
// file="RegexAnalyzer.SourceGen"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    public partial class RegexAnalyzer
    {

        internal static SequenceEmitter<string> GenerateSource(
                                                TextRegexSource source
        )
        {
            SequenceEmitter<string> se = new();
            List<SequenceEmitter<string>> sub_match_list = [];

            RegexInfo info = source.Info;
            List<RegexIR> ir = source.IRs;
            ReadOnlySpan<RegexIR> ir_sp = CollectionsMarshal.AsSpan(ir);
            RegexIR[] window = new RegexIR[8];
            Span<RegexIR> scan_win = window;

            _ = se.AppendLine(@"public override MatchResult Match(ReadOnlySpan<char> text)")//
                  .AppendLine(@"{");

            int region_count = info.RegionCount;
            for (int i = 0; i < region_count; i++)
            {
                _ = se.AppendLine($@"int region_{i}_start = -1;")//
                      .AppendLine($@"int region_{i}_end = -1;")
                      .AppendLine($@"bool region_{i}_success;");
            }

            int index = 0;
            do
            {
                int count = int.Min(8, ir_sp.Length - index);
                ir_sp.Slice(index, count).CopyTo(scan_win);
            } while (index < ir.Count);

            foreach (SequenceEmitter<string> _sub in sub_match_list)
            {
                _ = se.AppendLine()                     //
                      .Append(_sub);
            }

            _ = se.Append(@"}");

            return se;
            /**/
            int TryGenerateMatch(
                Span<RegexIR> scan_win,
                SequenceEmitter<string> sb
            )
            {
                if (scan_win[0] is not RegexMatchSeqIR seq) {
                    return -1;
                }
                _ = sb.Append(string.Empty);
                return 1;
            }
            /**/
        }

    }
}
