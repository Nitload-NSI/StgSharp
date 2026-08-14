// -----------------------------------------------------------------------------
// file="RegexAnalyzer.IROptimize"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    public partial class RegexAnalyzer
    {

        internal static void ScanAndOptimizeIR(
                             List<RegexIR> baseIR,
                             out List<RegexIR> optIR
        )
        {
            int opt_count;
            List<RegexIR> origin_ir = baseIR;
            List<RegexIR> opt_ir = new(origin_ir.Count);
            RegexIR[] opt_window = new RegexIR[8];
            Span<RegexIR> opt_win_sp = opt_window;
            int result;

            // phase 0: common opt
            phase_zero:
            opt_count = 0;
            int index = 0;
            Span<RegexIR> origin_span = CollectionsMarshal.AsSpan(origin_ir);

            do
            {
                int process_count = 0;
                int last = int.Min(origin_span.Length, index + 8);
                opt_win_sp.Clear();
                origin_span[index..last].CopyTo(opt_win_sp);

                if ((result = TryExpandPlus(opt_win_sp, opt_ir)) >= 0)
                {
                    process_count++;
                    if (result > 0)
                    {
                        RegexCountComplexIR cc = Unsafe.As<RegexCountComplexIR>(opt_win_sp[0]);
                        Span<RegexIR> sp = origin_span.Slice(index + 1, result);
                        TryExpandComplexPlus(cc.IsGreedy, sp, opt_ir);
                        process_count += result;
                    }
                    opt_count++;
                } else if (TryOptDotCountCount(opt_win_sp, opt_ir) >= 0)
                {
                    process_count += 2;
                    opt_count++;
                } else if (TryFlattenCount(opt_win_sp, opt_ir) >= 0)
                {
                    process_count++;
                    opt_count++;
                } else if ((result = TryExpandOnce(opt_win_sp, opt_ir)) >= 0)
                {
                    process_count++;
                    if (result > 0)
                    {
                        RegexCountComplexIR cc = Unsafe.As<RegexCountComplexIR>(opt_win_sp[0]);
                        Span<RegexIR> sp = origin_span.Slice(index + 1, result);
                        TryExpandComplex(sp, opt_ir, true);
                        process_count += result;
                    }
                    opt_count++;
                } else if (opt_win_sp[0] is IRegexComplex { IsFullyOptimized: true } skip)
                {
                    int count = skip.CommandCount;
                    process_count += count + 1;
                    Span<RegexIR> opt_block = origin_span.Slice(index, count + 1);
                    opt_ir.AddRange(opt_block);
                } else if (opt_win_sp[0] is RegexTryIR { IsFullyOptimized: false } try_ir)
                {
                    process_count++;
                    int cmd_cnt = try_ir.CommandCount;
                    Span<RegexIR> try_sp = origin_span.Slice(index + 1, cmd_cnt);
                    ScanAndOptimizeIR(try_sp, out List<RegexIR> opt_try);
                    RegexTryIR new_try = new(opt_try.Count)
                    {
                        IsFullyOptimized = true
                    };
                    opt_ir.Add(new_try);
                    opt_ir.AddRange(opt_try);
                    process_count += cmd_cnt;
                    opt_count++;
                } else
                {
                    opt_ir.Add(opt_win_sp[0]);
                    process_count++;
                }

                index += process_count;
            } while (index < origin_ir.Count);
            if (opt_count > 0)
            {
                // need to swap the origin_ir and opt_ir, and clear the opt_ir for next round
                if (ReferenceEquals(origin_ir, baseIR))
                {
                    origin_ir = new(opt_ir.Count);
                }
                (origin_ir, opt_ir) = (opt_ir, origin_ir);
                opt_ir.Clear();
                goto phase_zero;
            }

            // phase 1: dot star replace
            (origin_ir, opt_ir) = (opt_ir, origin_ir);
            opt_ir.Clear();
            index = 0;
            opt_count = 0;
            origin_span = CollectionsMarshal.AsSpan(origin_ir);
            do
            {
                int process_count = 0;
                int last = int.Min(origin_span.Length, index + 8);
                opt_win_sp.Clear();
                origin_span[index..last].CopyTo(opt_win_sp);

                if (opt_win_sp[0] is IRegexComplex { IsFullyOptimized: true } skip)
                {
                    int count = skip.CommandCount;
                    process_count += count + 1;
                    Span<RegexIR> opt_block = origin_span.Slice(index, count + 1);
                    opt_ir.AddRange(opt_block);
                } else
                if (TryReplaceDotStar(opt_win_sp, opt_ir) >= 0)
                {
                    opt_count++;
                    process_count++;
                } else
                {
                    opt_ir.Add(opt_win_sp[0]);
                    process_count++;
                }

                index += process_count;
            } while (index < origin_ir.Count);
            if (opt_count == 0)
            {
                optIR = opt_ir;
                return;
            }

            // phase 2: find-1 merge and try-find-1 rotate
            do
            {
                (origin_ir, opt_ir) = (opt_ir, origin_ir);
                opt_ir.Clear();
                opt_count = 0;
                index = 0;
                origin_span = CollectionsMarshal.AsSpan(origin_ir);
                do
                {
                    int process_count = 0;
                    int last = int.Min(origin_span.Length, index + 8);
                    opt_win_sp.Clear();
                    origin_span[index..last].CopyTo(opt_win_sp);

                    if (opt_win_sp[0] is IRegexComplex { IsFullyOptimized: true, CommandCount: > 0 } skip)
                    {
                        int count = skip.CommandCount;
                        process_count += count + 1;
                        Span<RegexIR> opt_block = origin_span.Slice(index, count + 1);
                        opt_ir.AddRange(opt_block);
                    } else if (TyrMergeFindRestTwice(opt_win_sp, opt_ir) == 0)
                    {
                        process_count += 2;
                        opt_count++;
                    } else if ((result = TyrMergeFindRestTry(opt_win_sp, opt_ir)) >= 0)
                    {
                        Span<RegexIR> opt_block = origin_span.Slice(index + 2, result);
                        opt_ir.AddRange(opt_block);
                        process_count += 2;
                        process_count += result;
                        opt_count++;
                    } else
                    {
                        opt_ir.Add(opt_win_sp[0]);
                        process_count++;
                    }

                    index += process_count;
                } while (index < origin_ir.Count);
            } while (opt_count > 0);
            optIR = opt_ir;
        }

        private static void ScanAndOptimizeIR(
                            Span<RegexIR> baseIR,
                            out List<RegexIR> opt_ir
        )
        {
            List<RegexIR> originList = new(baseIR.Length);
            CollectionsMarshal.SetCount(originList, baseIR.Length);
            baseIR.CopyTo(CollectionsMarshal.AsSpan(originList));
            ScanAndOptimizeIR(originList, out opt_ir);
        }

        private static int TryFlattenCount(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            const int flatten_size = 32;
            RegexIR ir = window[0];
            if (ir is not RegexCountSeqIR seq) {
                return -1;
            }
            int min = seq.Min;
            int max = seq.Max;
            if (min == 0) {
                return -1;
            }
            string pattern = seq.Pattern;
            int pattern_size = pattern.Length;
            if (pattern_size > flatten_size) {
                opt.Add(ir);
            }
            int min_expand = flatten_size / pattern_size;
            min_expand = int.Min(min_expand, min);
            string expand = string.Concat(Enumerable.Repeat(pattern, min_expand));
            int new_min = min / min_expand;
            int new_rest = min % min_expand;
            if (new_min == 1)
            {
                opt.Add(new RegexMatchSeqIR(expand));
            } else
            {
                opt.Add(new RegexCountSeqIR(new_min, new_min, seq.IsGreedy, expand));
            }
            if (new_rest > 0)
            {
                string expand_tail = string.Concat(Enumerable.Repeat(pattern, new_rest));
                opt.Add(new RegexMatchSeqIR(expand_tail));
            }
            int rest_count = max - min;
            rest_count = int.Max(rest_count, -1);
            opt.Add(new RegexCountSeqIR(0, rest_count, seq.IsGreedy, pattern));
            return 0;
        }

        #region dot count count

        private static int TryOptDotCountCount(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            /* .* <any>*
             * convert to
             * .*
             */
            RegexIR cur_ir = window[0];
            if (cur_ir is not RegexCountSetIR { IsDotStar: true } dot) {
                return -1;
            }
            if (window[1] is not IRegexCount { IsStar: true } after) {
                return -1;
            }
            if (dot.IsGreedy)
            {
                opt.Add(window[0]);
                return 0;
            }
            return -1;
        }

            #endregion

        private static int TryReplaceDotStar(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            if (window[0] is not RegexCountSetIR { IsDotStar: true } dot) {
                return -1;
            }
            if (dot.IsGreedy)
            {
                RegexFindComplexIR find = new(false, -1)
                {
                    IsFullyOptimized = true
                };
                opt.Add(find);
                return 0;
            } else
            {
                RegexFindComplexIR find = new(true, -1)
                {
                    IsFullyOptimized = true
                };
                opt.Add(find);
                return 0;
            }
        }

        #region merge find and try-find

        private static int TyrMergeFindRestTwice(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            if (window[0] is not RegexFindComplexIR { CommandCount: -1 } find_1 ||
                window[1] is not RegexFindComplexIR find_2) {
                return -1;
            }
            RegexFindComplexIR find_try = new(find_1.IsNearest && find_2.IsNearest, find_2.CommandCount)
            {
                IsFullyOptimized = true
            };
            opt.Add(find_try);
            return 0;
        }

        private static int TyrMergeFindRestTry(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            if (window[0] is not RegexFindComplexIR { CommandCount: -1 } _find ||
                window[1] is not RegexTryIR _try) {
                return -1;
            }
            RegexFindComplexIR find_try = new(_find.IsNearest, _try.CommandCount)
            {
                IsFullyOptimized = true
            };
            opt.Add(find_try);
            return find_try.CommandCount;
        }

            #endregion


        #region expand once

        private static void TryExpandComplex(
                            Span<RegexIR> complex,
                            List<RegexIR> opt,
                            bool is_once
        )
        {
            if (!is_once) {
                opt.Add(new RegexTryIR(complex.Length));
            }

            opt.AddRange(complex);
        }

        private static int TryExpandOnce(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            RegexIR ir = window[0];
            if (ir is IRegexCount { Min: 1, Max: 1 })
            {
                string pattern;
                switch (ir.CommandMask & RegexIRCommand.Count)
                {
                    case RegexIRCommand.Count_Complex:
                        RegexCountComplexIR complex = Unsafe.As<RegexCountComplexIR>(ir);
                        return complex.CommandCount;
                    case RegexIRCommand.Count_Seq:
                        RegexCountSeqIR seq = Unsafe.As<RegexCountSeqIR>(ir);
                        pattern = seq.Pattern;
                        opt.Add(new RegexMatchSeqIR(pattern));
                        return 0;
                    case RegexIRCommand.Count_Set:
                        RegexCountSetIR set = Unsafe.As<RegexCountSetIR>(ir);
                        pattern = set.CharSet;
                        opt.Add(new RegexMatchSetIR(pattern));
                        return 0;
                    default:
                        return -1;
                }
            } else if (ir is IRegexCount { Min: 0, Max: 1 })
            {
                string pattern;
                switch (ir.CommandMask & RegexIRCommand.Count)
                {
                    case RegexIRCommand.Count_Complex:
                        RegexCountComplexIR complex = Unsafe.As<RegexCountComplexIR>(ir);
                        return complex.CommandCount;
                    case RegexIRCommand.Count_Seq:
                        RegexCountSeqIR seq = Unsafe.As<RegexCountSeqIR>(ir);
                        pattern = seq.Pattern;
                        opt.Add(new RegexTryIR(1));
                        opt.Add(new RegexMatchSeqIR(pattern));
                        return 0;
                    case RegexIRCommand.Count_Set:
                        opt.Add(new RegexTryIR(1));
                        RegexCountSetIR set = Unsafe.As<RegexCountSetIR>(ir);
                        pattern = set.CharSet;
                        opt.Add(new RegexMatchSetIR(pattern));
                        return 0;
                    default:
                        return -1;
                }
            } else
            {
                return -1;
            }
        }

            #endregion


        #region expand plus

        private static void TryExpandComplexPlus(
                            bool is_greedy,
                            Span<RegexIR> complex,
                            List<RegexIR> opt
        )
        {
            ScanAndOptimizeIR(complex, out List<RegexIR>? part_opt);
            opt.AddRange(part_opt);
            RegexCountComplexIR c_ir = new(0, -1, is_greedy, part_opt.Count)
            {
                IsFullyOptimized = true
            };
            opt.Add(c_ir);
            opt.AddRange(part_opt);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="window">
        ///
        /// </param>
        /// <param name="opt">
        ///
        /// </param>
        /// <returns>
        ///   Negative if cannot opt, 0 if match successfully, positive if meets an IRegexComplex
        ///   interface, need to call TryExpandComplexPlus
        /// </returns>
        private static int TryExpandPlus(
                           Span<RegexIR> window,
                           List<RegexIR> opt
        )
        {
            /* a+
             * convert to
             * aa*
             */

            RegexIR ir = window[0];
            if (ir is not IRegexCount { IsPlus: true } count) {
                return -1;
            }
            bool is_greedy = count.IsGreedy;
            string pattern;
            switch (ir.CommandMask & RegexIRCommand.Count)
            {
                case RegexIRCommand.Count_Complex:
                    RegexCountComplexIR complex = Unsafe.As<RegexCountComplexIR>(ir);
                    return complex.CommandCount;
                case RegexIRCommand.Count_Seq:
                    RegexCountSeqIR seq = Unsafe.As<RegexCountSeqIR>(ir);
                    pattern = seq.Pattern;
                    opt.Add(new RegexMatchSeqIR(pattern));
                    opt.Add(new RegexCountSeqIR(0, -1, is_greedy, pattern));
                    return 0;
                case RegexIRCommand.Count_Set:
                    RegexCountSeqIR set = Unsafe.As<RegexCountSeqIR>(ir);
                    pattern = set.Pattern;
                    opt.Add(new RegexMatchSeqIR(pattern));
                    opt.Add(new RegexCountSeqIR(0, -1, is_greedy, pattern));
                    return 0;
                default:
                    return -1;
            }
        }

        #endregion
    }
}
