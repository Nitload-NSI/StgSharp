// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass.MultiIR"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        private static void GenerateRegion(
                            ReadOnlySpan<RegexIR> region_ir,
                            SourceGenContext sc,
                            in SequenceEmitter<string> main_emitter,
                            in List<SequenceEmitter<string>> sub_emitter_list
        )
        {
            int current_region = region_ir[0].Prefix.WorkingRegion;
            ReadOnlySpan<RegexIR> ir_sp = region_ir;
            ReadOnlySpan<RegexIR> scan_win;
            int index = 0;
            do
            {
                int result;
                int count = 0;
                scan_win = ir_sp[index..];
                if ((result = TryGenerateFindSeq(scan_win, sc, in main_emitter,
                                                 in sub_emitter_list)) > 0)
                {
                    count += result;
                } else
                if (TryGenerateMatchSeq(scan_win, main_emitter, sc) > 0)
                {
                    count += 1;
                } else if (TryGenerateMatchSet(scan_win, main_emitter, sc) > 0)
                {
                    count++;
                } else if (TryGenerateFindSeq(scan_win, main_emitter, sc) > 0)
                {
                    count++;
                } else { }
                index += count;
            } while (index < ir_sp.Length);
        }

        private static int TryGenerateFindSeq(
                           ReadOnlySpan<RegexIR> scan_win,
                           SourceGenContext sc,
                           in SequenceEmitter<string> main_emitter,
                           in List<SequenceEmitter<string>> sub_emitter_list
        )
        {
            if (scan_win[0] is not RegexFindSeqIR find) {
                return -1;
            }
            const string cur_ptr = "cur_ptr",
                begin_ptr = "begin_ptr";
            _ = main_emitter.AppendLine()
                            .AppendLine(@$"//Find sequence:{FormatStringLiteral(find.Pattern)} at any pos")
                            .AppendLine(@"do")
                            .AppendLine(@"{")
                            .AppendLine(@$"int {cur_ptr}, {begin_ptr} = {current_ptr};")
                            .AppendLine($@"{ROS_char} rest = ");

            string finder = find.IsFirst ? "FirstIndexOf" : "LastIndexOf";

            if (scan_win[1] is RegexMatchSeqIR seq)
            {
                string seq_pat = FormatStringLiteral(find.Pattern + seq.Pattern);
                _ = main_emitter.AppendLine(@$"//Match sequence:{seq_pat} at current pos")
                                .AppendLine($@"if(  )");
            }

            _ = main_emitter.AppendLine("}").AppendLine(string.Empty);
            return 1;
        }

    }
}
