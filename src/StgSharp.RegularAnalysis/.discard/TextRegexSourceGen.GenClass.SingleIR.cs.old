// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass.SingleIR"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
using StgSharp.RegularAnalysis.Abstraction;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {
        /*
         * =======================================================================================
         * L4, one instruction
         * =======================================================================================
         *
         * The fallback of the dispatch chain. Every RegexIRCommand needs a generator here, because
         * this is the only layer guaranteed to be offered every window. L1 and L3 may decline, L4
         * may not.
         *
         * Each generator consumes exactly one entry and describes only what the instruction does.
         * When it runs, which result it reads and whether it leaves the region early are all added
         * by L5, so nothing here should inspect RegexIR.Prefix beyond WorkingRegion.
         *
         * Contract for a generated body, names owned by SourceGenContext:
         *
         *   - assign the outcome to __ok
         *   - advance __cur only on success
         *   - never index __cur past the end of the input
         *   - declare scratch variables inside a block of their own, so a repeated shape does not
         *     collide with itself
         *
         * A body must be generated into a detached emitter and only spliced in once it is known to
         * be claimed. Emitting a prefix for an instruction that turns out to have no generator
         * would open a brace nobody closes.
         *
         * Checklist:
         *
         *   L4.1   MATCH_SEQ                      abc, \., a\*b
         *   L4.2   MATCH_SET, discrete            [abc], [123abc], [.]
         *   L4.3   MATCH_SET, range               [1-9]
         *   L4.4   MATCH_SET, escape class        \w
         *   L4.5   MATCH_SET, any element         .
         *   L4.6   COUNT_SEQ                      a*, a?
         *   L4.7   COUNT_SET                      [abc]*, \w+, .{11,13}, [.]*
         *   L4.10  POP                            ALT tail
         *   L4.11  PACK                           (abc), ordinal group
         *
         * L4.8 and L4.9 were removed. FIND_SEQ and FIND_SET have no single instruction form,
         * because they have to drive a candidate retry loop over the rest of the region. See L1.9
         * in TextRegexSourceGen.GenClass.MultiIR.cs. Only a FIND that happens to be the last
         * instruction of its region degrades into a single search, and that is a branch of L1.9
         * rather than a generator of its own. The numbers are left vacant so existing references
         * do not shift.
         *
         * Semantics worth pinning down before writing these:
         *
         *   - [.] and . are different sets. The former is a literal dot, the latter is any
         *     element, and [.]* must not be folded into the dot star optimisation.
         *   - \w has to keep full .NET semantics. TextRegex.IsCharWord is inherited by the
         *     generated class and already carries them, so the generated code can call it
         *     directly instead of approximating with an ASCII test.
         *   - Repetition in this layer does not backtrack. A lazy repetition therefore consumes
         *     exactly its lower bound and leaves the rest to the following anchor. Backtracking
         *     does exist in this backend, but only inside the FIND retry loop at L1.7 to L1.9,
         *     never here.
         *   - An epsilon sequence and an empty repetition unit both reach these generators.
         *     Decide whether they are vacuous successes or an optimizer bug worth reporting.
         *   - PACK decides nothing, so it should leave __ok untouched. Whether it runs at all is
         *     already expressed by its execution condition.
         */

        #region code

        private static int TryGenerateSingleIR(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            return TryGenerateCountSeq(outer_level, ir_span, sc, code,
                                       local_func) > 0 ||
                   TryGenerateCountSet(outer_level, ir_span, sc, code,
                                       local_func) > 0 ||
                   TryGenerateMatchSet(outer_level, ir_span, sc, code,
                                       local_func) > 0 ||
                   TryGenerateMatchSeq(outer_level, ir_span, sc, code, local_func) > 0 ?
                   1 :
                   -1;
        }

        private static int TryGeneratePack(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            if (ir_span[0] is not RegexPackIR pack) {
                return -1;
            }
            string group_name = pack.GroupName;
            string begin = sc.GetVarGroupBegin(group_name);
            string end = sc.GetVarGroupEnd(group_name);
            _ = code.AppendLine(begin).AppendLine(end);
            return 1;
        }

        private static int TryGenerateCountSet(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            if (ir_span[0] is not RegexCountSetIR count_set) {
                return -1;
            }

            bool is_greedy = count_set.IsGreedy;
            int min = count_set.Min;
            int max = count_set.Max;
            int leave = is_greedy ? max : min;
            string counter = sc.RequireNewCounter();
            string set_source = count_set.CharSet;
            RegexCharSet[] set_arr = FigureCharSetType(set_source, out bool accept);

            _ = code.AppendLine($@"for(; {counter}<{leave}; {counter}++)")
                    .AppendLine("{")
                    .AppendLine(accept ? @$"if(" : @$"if(!(");
            string _case_code;
            RegexCharSet set;
            for (int i = 1; i < set_arr.Length - 1; i++)
            {
                set = set_arr[i];
                _case_code = GenLine(set, sc, "||");
                _ = code.AppendLine(_case_code);
            }
            set = set_arr[^1];
            _case_code = GenLine(set, sc, ")");
            _ = code.AppendLine(_case_code,                     //
                                "{",                            // in case match
                                "}",                            //
                                "else",                         //
                                "{",                            //
                                "break;",                       // in case miss match
                                "}",                            //
                                "}",                            // end of for
                                @$"if({counter}>{min})",        // match here
                                "{",                            // in case count reaches min count request
                                "}",                            //
                                "else",                         //
                                "{",                            // in case count does not reach min count request
                                "}",                            //
                                string.Empty);                  //endof count
            return 1;
        }

        private static int TryGenerateCountSeq(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            if (ir_span[0] is not RegexCountSeqIR count_seq) {
                return -1;
            }
            string seq = count_seq.Pattern;
            string literal = FormatStringLiteral(seq);
            bool is_greedy = count_seq.IsGreedy;
            int min = count_seq.Min;
            int max = count_seq.Max;
            int leave = is_greedy ? max : min;
            string counter = sc.RequireNewCounter();

            // TODO cannot check real count outside the cycle
            _ = code.AppendLine($@"int {counter} = 0;",                                        //
                                @$"for(; {counter}<{leave}; {counter}++)",                          //
                                "{",                                                                //
                                @$"if(!{sc.RemainingSpan}.StartWith({literal}))",                   //
                                "{",                                                                //
                                $@"break;",                                                         //
                                "}",                                                                //
                                "}",                                                                //
                                @$"if({counter}>{min})",                                            //
                                "{",                                                                // in case match
                                sc.MakeRemainOffset(counter),                                       //
                                "}",                                                                //
                                $@"else",                                                           //
                                "{",                                                                // TODO fail here
                                "}");                                                               //
            return 1;
        }

        private static int TryGenerateMatchSeq(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            if (ir_span[0] is not RegexMatchSeqIR match_seq) {
                return -1;
            }
            string seq = match_seq.Pattern;
            string literal = FormatStringLiteral(seq);
            _ = code.AppendLine(@$"if({sc.RemainingSpan}.StartWith({literal}))")
                    .AppendLine("{")
                    .AppendLine($"{sc.StepResult} = true;")
                    .AppendLine(sc.MakeRemainOffset(seq.Length))
                    .AppendLine("}")
                    .AppendLine("else")
                    .AppendLine("{")//TODO fail here
                    .AppendLine("break;")
                    .AppendLine("}");
            return 1;
        }

        private static int TryGenerateMatchSet(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            if (ir_span[0] is not RegexMatchSetIR match_set) {
                return -1;
            }
            string set_source = match_set.Charset;
            RegexCharSet[] set_arr = FigureCharSetType(set_source, out bool accept);

            _ = code.AppendLine(accept ? @$"if(" : @$"if(!(");
            string _case_code;
            RegexCharSet set;
            /*
             * in loop:
             * for( is cas1 1 ||
             * is case 2 ||
             * .....
             * is case last)
             * {
             *     success here
             * }
             * else
             * {
             *     fail case
             * }
             */
            for (int i = 1; i < set_arr.Length - 1; i++)
            {
                set = set_arr[i];
                _case_code = GenLine(set, sc, "||");
                _ = code.AppendLine(_case_code);
            }
            set = set_arr[^1];
            _case_code = GenLine(set, sc, ")");
            _ = code.AppendLine(_case_code)
                    .AppendLine("{")
                    .AppendLine(sc.MakeRemainOffset(1))             // TODO case success
                    .AppendLine("}")
                    .AppendLine("else")
                    .AppendLine("{")
                    .AppendLine("break;")                                         // TODO case fail
                    .AppendLine("}");
            return 1;
        }

        private static string GenLine(
                              RegexCharSet set,
                              SourceGenContext sc,
                              string end_of_line,
                              string symbol_of_offset = ""
        )
        {
            string offset = string.IsNullOrEmpty(symbol_of_offset) ? "0" : $"{symbol_of_offset}";
            switch (set.Type)
            {
                case CharSetType.Single:
                    string cases = set.Value;
                    if (cases.Length > 8)
                    {
                        // more than 8 different chars, using string match
                        string literal = FormatStringLiteral(cases);
                        return $@"{literal}.Contains({sc.RemainingSpan}[{offset}]) {end_of_line}";
                    } else
                    {
                        StringBuilder sb = new();
                        string literal;
                        _ = sb.Append('(');
                        for (int j = 0; j < cases.Length - 1; j++)
                        {
                            literal = FormatCharLiteral(cases[j]);
                            _ = sb.Append(CultureInfo.InvariantCulture, $@"{sc.RemainingSpan}[{offset}] == {literal} ||");
                        }
                        literal = FormatCharLiteral(cases[^1]);
                        _ = sb.Append(CultureInfo.InvariantCulture, $@"{sc.RemainingSpan}[{offset}] == {literal}")
                              .Append(CultureInfo.InvariantCulture, @$") {end_of_line}");
                        return sb.ToString();
                    }
                case CharSetType.Range:
                    string rule = set.Value;
                    char lo = rule[0];
                    char hi = rule[1];
                    return $@"({sc.RemainingSpan}[{offset}] >= '{lo}' && {sc.RemainingSpan}[{offset}] <= '{hi}') {end_of_line}";

                case CharSetType.Set:
                    string accept_mark = set.Accept ? string.Empty : "!";
                    return set.Value switch
                    {
                        "s" => $@"{accept_mark}char.IsWhiteSpace({sc.RemainingSpan}[{offset})] {end_of_line}",
                        "d" => $@"{accept_mark}char.IsDigit({sc.RemainingSpan}[{offset})] {end_of_line}",
                        "w" => $@"{accept_mark}IsCharWord({sc.RemainingSpan}[{offset})] {end_of_line}",
                        _ => string.Empty
                    };
                case CharSetType.None:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
