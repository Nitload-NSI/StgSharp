// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass.MultiIR"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Threading;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {
        /*
         * =======================================================================================
         * L1, several regions plus a tail
         * =======================================================================================
         *
         * Highest consumption in the dispatch chain, so these are offered the window first. Each
         * shape here was already recognised once during prefix emission, so a generator should
         * mirror the matching RegexAnalyzer.PrefixEmit recogniser rather than re-derive the shape.
         *
         *   L1.2  ALT chain with a shared suffix
         *         window   L1.1 followed by linear instructions in the parent region
         *         sample   (ab|[cd]|.)x
         *         nature   flattening, may return -1
         *
         *   L1.3  group tail
         *         window   region, POP FAIL with Prev_Fail, PACK SUCCESS with Prev_Success
         *         mirrors  TryEmitTryGroupTail
         *         shape    region block, capture written on the success branch
         *         sample   (?<name>abc)
         *         nature   flattening, may return -1
         *
         *   L1.4  group tail wrapping an ALT chain
         *         sample   (?<a>abcd|a(bc)c)
         *         nature   flattening, may return -1
         *
         *   L1.5  COUNT_COMPLEX plus body
         *         window   RegexCountComplexIR, then CommandCount entries
         *         mirrors  TryEmitComplexIR
         *         shape    loop wrapping the generated body
         *         sample   (ab){2,3}
         *         nature   REQUIRED, no fallback exists
         *
         *   L1.6  COUNT_COMPLEX whose body contains an ALT chain, plus a linear suffix
         *         sample   (123(4|5)67){3,4}114514
         *         nature   REQUIRED, no fallback exists
         *
         *   L1.7  FIND_COMPLEX with CommandCount >= 0
         *         window   RegexFindComplexIR, then CommandCount entries
         *         shape    candidate retry loop wrapping the body
         *         sample   .*(ab|cd)
         *         nature   REQUIRED, no fallback exists
         *
         *   L1.8  FIND_COMPLEX with CommandCount == -1
         *         window   RegexFindComplexIR, then the rest of the region
         *         shape    candidate retry loop wrapping the rest of the region
         *         sample   (?<name>.*)234
         *         nature   REQUIRED, no fallback exists
         *
         *   L1.9  FIND_SEQ or FIND_SET
         *         window   the instruction, then the rest of the region
         *         shape    candidate retry loop wrapping the rest of the region
         *         sample   .*abc
         *         nature   REQUIRED, no fallback exists
         *
         * L1.5 to L1.9 are required because none of their bodies is a region of its own. L2
         * cannot pick them up, so there is no degradation path if these are missing.
         *
         * CommandCount must be read from the prefixed IR. Prefix emission rebuilds these counts,
         * so the value carried by the pre-prefix IR no longer applies.
         *
         * ---------------------------------------------------------------------------------------
         * Backtracking semantics of the FIND family
         * ---------------------------------------------------------------------------------------
         *
         * A FIND does not yield one position, it yields a priority ordered sequence of candidates.
         * FURTHEST starts at the last one and steps backwards on failure, NEAREST starts at the
         * first and steps forwards. The body is the verifier for a candidate: when it fails, the
         * next candidate has to be tried, and only an exhausted candidate list is a real failure.
         * A single LastIndexOf is wrong, because .*abcX has to fall back to the second to last abc
         * when the last one is not followed by an X.
         *
         * CommandCount == -1 means the body is the rest of the region. TryReplaceDotStar emits
         * FindComplex(-1) for a bare .*, then TyrMergeFindRestTwice and TyrMergeFindRestTry try to
         * collapse it into a definite length. Whatever survives stays at -1. FIND_SEQ and FIND_SET
         * carry no CommandCount at all and behave like -1. Only a FIND that happens to be the last
         * instruction of its region degrades into a single search, consuming one entry.
         *
         * Target shape:
         *
         *     int __bound = text.Length;
         *     __ok = false;
         *     while (true)
         *     {
         *         int __at = text[__cur..__bound].LastIndexOf("abc");
         *         if (__at < 0) { __ok = false; break; }   // candidates exhausted, only failure
         *         int __save = __cur;
         *         __cur = __cur + __at + 3;
         *
         *         // body, or the rest of the region, expands here
         *
         *         if (__ok) { break; }                     // verified
         *         __cur = __save;
         *         __bound = __save + __at;                 // step to the previous candidate
         *     }
         *
         * POP conflicts with the retry loop. A POP lowers to break, and the retry while is nearer
         * than the region do block, so the break would leave the wrong scope. The fix without goto
         * is a region completion flag:
         *
         *     do                              // region N
         *     {
         *         bool __rN_done = false;
         *         while (!__rN_done)          // find retry
         *         {
         *             // candidate search
         *             do                      // isolation layer, POP breaks out here
         *             {
         *                 // body, POP sets __rN_ok, sets __rN_done, breaks
         *             } while (false);
         *             if (__rN_done || __ok) { break; }
         *             // step to the next candidate
         *         }
         *     } while (false);
         *
         * Two further hazards:
         *   - nested FIND produces nested retry loops and the cost multiplies, which is the
         *     catastrophic backtracking shape, worth a guard or at least a marker
         *   - __ok is overwritten across retry iterations, so an Observe_Line prefix pointing into
         *     a retry body reads the value of the last iteration, a case the current prefix model
         *     does not cover
         *
         * Open questions to settle while implementing:
         *   - how the cursor is restored between ALT cases
         *   - which successful outcome a PACK uses as its capture span
         *   - whether a region body ever deserves its own generated method instead of inlining
         *
         * =======================================================================================
         * L2, one region
         * =======================================================================================
         *
         * Consumes CommandCount + 1, the whole block a TRY lowers to. Together with L4 and L5 this
         * is the correctness core: once these three exist, every expression can be generated, just
         * with deeper nesting than L1 and L3 would produce.
         *
         *   L2.1  RegexTryIR plus its body            sample  (abc)
         *   L2.2  the same, body containing a nested region   sample  (a(bc))
         *   L2.3  a single instruction region folded by prefix emission   sample  (a)
         *
         * Target shape, names owned by SourceGenContext:
         *
         *     __rN_begin = __cur;
         *     __rN_ok = false;
         *     do
         *     {
         *         // body, a POP breaks out here
         *         __rN_ok = __ok;
         *     } while (false);
         *     if (!__rN_ok) { __cur = __rN_begin; }
         *     __ok = __rN_ok;
         *
         * Points the implementation has to settle:
         *   - the failure path restores the cursor to the region entry
         *   - break only leaves the nearest enclosing loop, and a FIND retry while counts as one,
         *     so a region containing a FIND needs the __rN_done flag described above instead of a
         *     plain break
         *   - a nested region must not rely on break to exit an outer one
         *   - the whole match body is itself region zero, which is what makes a top level POP
         *     legal
         *
         * =======================================================================================
         * L3, fixed multi instruction windows
         * =======================================================================================
         *
         * Consumes two to eight entries. Pure code quality work, never required for correctness.
         * Every one of these must return -1 cleanly when the window does not match, so the entry
         * falls through to L4.
         *
         *   L3.1  FIND_COMPLEX FURTHEST COUNT:-1, PACK, MATCH_SEQ   3   (?<name>.*)234
         *   L3.2  the same with a single element anchor             3   (?<name>.*)a
         *   L3.3  FIND_SEQ, MATCH_SEQ, merged into one search       2   .*abc
         *   L3.4  MATCH_SEQ, COUNT_SEQ, the expanded a+ shape       2   a{3,5}, a+
         *   L3.5  a run of MATCH_SEQ and MATCH_SET, merged into one condition   2 to 8   a[abc]d
         *   L3.6  trailing COUNT_SET over a dot, fast success       1 to 2   abc.*
         *
         * Two hazards to check for each of these:
         *   - merging must not break the line indices an Observe_Line prefix refers to
         *   - a merged window still has to leave __ok describing the whole window
         */

        #region code

        private static int TryGenerateTryBlock(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            if (ir_span[0]is not RegexTryIR _try) {
                return -1;
            }
            ReadOnlySpan<RegexIR> block = ir_span[1..(1 + _try.CommandCount)];

            // TODO generate here
            return _try.CommandCount;
        }

        private static int TryGenerateNamedCountAny(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            /*
             * (?<name>.*)234
             * [010] 0003, DEF , DEF , NONE, 0000, FIND_COMPLEX FURTHEST COUNT: -1
             * [011] 0003, Sccs, DEF , LINE, 0010, PACK SUCCESS GROUP: "name"
             * [012] 0000, DEF , DEF , NONE, 0000, MATCH_SEQ "234"
             */
            if (ir_span[0] is not RegexCountSetIR { IsDotStar: true } ||
                ir_span[1] is not RegexPackIR pack)
            {
                return -1;
            }
            string group_name = pack.GroupName;

            int i = 0;
            for (; i < ir_span.Length; i++) { }

            return -1;
        }

        private static int TryGenerateAltChain(
                           int outer_level,
                           ReadOnlySpan<RegexIR> ir_span,
                           SourceGenContext sc,
                           in SequenceEmitter<string> code,
                           in List<SequenceEmitter<string>> local_func
        )
        {
            /*
             * L1.1  ALT chain
             *       window   region, then further regions carrying Prev_Fail
             *       mirrors  TryEmitAltChain
             *       shape    if / else if / else
             *       sample   a|b, ab|cd|ef, a|bc|def
             *       nature   flattening, may return -1
             */

            /*
             * [004] 0001, DEF , DEF , NONE, 0000, MATCH_SEQ "abc"
             * [005] 0002, DEF , Sccs, NONE, 0000, MATCH_SEQ "d"    <- an alt chain begins here
             * [006] 0002, Fail, Sccs, LINE, 0005, MATCH_SEQ "c"    <- an alt chain ends here
             * [007] 0001, Fail, DEF , LINE, 0006, POP FAIL
             */

            const RegexIRPrefixMask simple_alt = RegexIRPrefixMask.Prev_Fail | RegexIRPrefixMask.Pop_Success;

            int cur_level = ir_span[0].Prefix.WorkingRegion;
            if (cur_level == outer_level) {
                return -1;
            }
            int i = 1;
            RegexIR ir;
            for (; i < ir_span.Length; i++)
            {
                ir = ir_span[i];
                if ((ir.Prefix.Mask & simple_alt) == simple_alt)
                {
                    if (TryGenerateSingleIR(outer_level, [ir], sc, code, local_func) > 0)
                    {
                        // TODO add additional code for the alt chain
                    }

                    // simple match
                } else if (ir is RegexTryIR _try && (ir.Prefix.Mask & simple_alt) == simple_alt)
                {
                    // match a try region
                    ReadOnlySpan<RegexIR> body_span = ir_span.Slice(i + 1, i + _try.CommandCount);
                    i += _try.CommandCount;
                }
            }
            return i;
        }

        #endregion
    }
}
