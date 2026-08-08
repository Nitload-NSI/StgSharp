// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {
        /*
         * ---------------------------------------------------------------------------------------
         * NGRA text regex source generation backend
         * ---------------------------------------------------------------------------------------
         *
         * The backend turns a fully prefixed IR stream into the body of one Match method. It is
         * organised in layers, ordered by how many IR entries a single generator consumes:
         *
         *   L0  whole IR stream     method skeleton, slot declarations, entry guards
         *   L1  several regions     ALT chain, group tail, complex count, complex find
         *   L2  one region          the block a TRY lowers to
         *   L3  two to eight IR     fixed windows worth collapsing into better code
         *   L4  one IR              the fallback, one generator per RegexIRCommand
         *   L5  zero IR             prefix decoration wrapped around L1 to L4 output
         *
         * File layout:
         *
         *   TextRegexSourceGen.GenClass.cs           this file, L0 and shared helpers
         *   TextRegexSourceGen.GenClass.MultiIR.cs   L1, L2 and L3
         *   TextRegexSourceGen.GenClass.SingleIR.cs  L4
         *   TextRegexSourceGen.GenClass.Prefix.cs    L5
         *   TextRegexSourceGen.GenContext.cs         variable naming and slot allocation
         *
         * Contract every generator honours:
         *
         *   1. A TryGenerateXxx returns how many IR entries it consumed, or -1 when the window
         *      does not match its shape. It returns an entry count, never a character count or a
         *      pattern length.
         *   2. The dispatch chain offers a window to L1, then L2, then L3, then L4. Higher
         *      consumption wins, so a shape spanning a whole region plus a tail is never split by
         *      a smaller generator.
         *   3. L2, L4, L5 and the required entries of L1 together are sufficient for correctness.
         *      The flattening entries of L1 and all of L3 only improve the shape of what those
         *      would already emit, so an unimplemented one must return -1 cleanly rather than
         *      emit a partial result.
         *   4. A prefix consumes no IR. L1 to L4 describe what an instruction does, L5 decides
         *      when it runs and who reads the outcome.
         *   5. Scope follows Prefix.WorkingRegion, not expression nesting.
         *
         * Control flow shape:
         *
         *   A region lowers to do { ... } while(false), and POP lowers to break. The goto plus
         *   label scheme used by the .NET generated regex backend is deliberately not adopted.
         *
         *   A FIND lowers to a candidate retry loop nested inside the region block. That loop is
         *   nearer than the region do block, so a POP inside it needs the __rN_done flag described
         *   in TextRegexSourceGen.GenClass.MultiIR.cs rather than a plain break.
         *
         * Generated variable names are owned by SourceGenContext. No generator invents one.
         *
         * The per shape checklist this backend is rebuilt against lives in
         * future/ngra-regex-sourcegen-goldens.md, and the handwritten target code for each shape
         * lives in test/SingleFile/regex.cs.
         */

        // private const string current_ptr = "cur_ptr_global";                // global match cursor
        // private const string offset = "offset";                             // 
        // private const string orig_text = "text";                            // 

        #region L0 method level

        /// <summary>
        ///   Generates one <c> Match </c> method from a fully prefixed IR stream.
        /// </summary>
        /// <remarks>
        ///   Not implemented yet. Returns an empty emitter so the generator pipeline stays runnable
        ///   while the backend is being rebuilt.
        /// </remarks>
        internal static SequenceEmitter<string> GenerateSource(
                                                TextRegexSource source
        )
        {
            //
            // L0, method skeleton
            //
            // TODO  L0.1  Emit the Match signature from ROS_char and SourceGenContext.InputSpan.
            // TODO  L0.2  Emit region anchor and region result slots, count from RegexInfo.RegionCount.
            // TODO  L0.3  Emit line result slots, only for lines an Observe_Line prefix refers to.
            // TODO  L0.5  Emit the cursor, the step result and any scratch declaration the context collected.
            //

            /*
             * Slot demand is only known once the body exists, so the body has to be generated
             * into a detached emitter first and spliced in after the declaration block.
             *
             * The dispatch chain that walks the IR stream belongs here too. It offers each window
             * to L1, L2, L3 then L4, and adds the returned consumption count to the cursor. An
             * unclaimed window must produce a hard build break rather than silently emit nothing,
             * because code that compiles but matches incorrectly is far harder to diagnose.
             */

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
                // GenerateRegion(region, sc, match_e, sub_match_list);
                _ = TryGenerateAltChain(0, region, sc, match_e, sub_match_list);
                _ = TryGenerateTryBlock(0, region, sc, match_e, sub_match_list);
                _ = TryGenerateNamedCountAny(0, region, sc, match_e, sub_match_list);
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

            int min_predict_length = info.MinPredictLength;
            if (min_predict_length > 0) {
                _ = se.AppendLine($@"if (text.Length < {min_predict_length})")
                      .AppendLine(@"{")
                      .AppendLine(@"return default;")
                      .AppendLine(@"}")
                      .AppendLine();
            }

            int region_count = info.RegionCount;
            for (int i = 0; i < region_count; i++)
            {
                _ = se.AppendLine($@"int region_{i}_start = -1;")//
                      .AppendLine($@"int region_{i}_end = -1;")
                      .AppendLine($@"bool region_{i}_success;");
            }
            /*
            _ = se.AppendLine($@"int {current_ptr} = 0;")
                  .AppendLine($@"int {offset} = 0;")
                  .AppendLine();
            /**/

            // _ = sc.GenVarDefine(se);

            _ = se.Append(match_e);

            foreach (SequenceEmitter<string> _sub in sub_match_list)
            {
                _ = se.AppendLine()                     //
                      .Append(_sub);
            }

            _ = se.Append(@"}");

            return se;
        }

            #endregion

        private enum RegionExitMethod
        {

            Break,
            Continue

        }

        #region shared helpers

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

        private static string FormatCharLiteral(
                              char value
        )
        {
            return SyntaxFactory.Literal(value).Text;
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

        internal record class RegexCharSet(
                              string Value,
                              CharSetType Type,
                              bool Accept
        )
        {

            public override string ToString()
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
            HashSet<char> single_char = [];
            if (source[0] == '[')
            {
                int idx;
                accept = source[1] != '^';
                idx = accept ? 1 : 2;
                List<RegexCharSet> list = [];
                ReadOnlySpan<char> seq = source.Slice(idx, source.Length - idx - 1);
                if (seq.IndexOf('-') == -1)
                {
                    SplitSet(single_char, seq, list);
                    StringBuilder sb = new();
                    foreach (char c in single_char) {
                        _ = sb.Append(c);
                    }
                    list.Add(new(sb.ToString(), CharSetType.Single, true));
                    return [.. list];
                } else
                {
                    for (; seq.Length > 0; )
                    {
                        int pos = seq.IndexOf('-');
                        if (pos == -1)
                        {
                            // has no range
                            SplitSet(single_char, seq, list);
                            break;
                        } else if (pos > 1)
                        {
                            // has other sequence before range
                            SplitSet(single_char, seq[0..(pos - 1)], list);
                        }
                        list.Add(new($"{seq[pos - 1]}{seq[pos + 1]}", CharSetType.Range, true));
                        seq = seq[(pos + 2)..];
                    }
                    StringBuilder sb = new();
                    foreach (char c in single_char) {
                        _ = sb.Append(c);
                    }
                    list.Add(new(sb.ToString(), CharSetType.Single, true));
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
                        HashSet<char> single_char,
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

                    switch (char.ToLower(remain[pos + 1], CultureInfo.InvariantCulture))
                    {
                        case 's':
                            sets.Add(new("s", CharSetType.Set, accept));
                            break;
                        case 'w':
                            sets.Add(new("w", CharSetType.Set, accept));
                            break;
                        case 'd':
                            sets.Add(new("d", CharSetType.Set, accept));
                            break;
                        case '[' or '(' or ')' or ']' or '\\' or '\"':
                            _ = single_char.Add(remain[pos + 1]);
                            break;
                        default:
                            throw new InvalidCastException($"Unknown char or charset {remain[0..1]}");
                    }
                    remain = remain[(pos + 2)..];
                }
                if (remain.Length > 0)
                {
                    foreach (char c in remain) {
                        _ = single_char.Add(c);
                    }
                }
            }
        }

        #endregion
    }
}
