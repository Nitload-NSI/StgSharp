// -----------------------------------------------------------------------------
// file="RegexAnalyzer.PrefixEmit"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    public static partial class RegexAnalyzer
    {

        internal static List<RegexIR> EmitPrefix(
                                      List<RegexIR> irList,
                                      ref RegexInfo info
        )
        {
            List<RegexIR> origin = irList;
            List<RegexIR> result = new(origin.Count);
            Span<RegexIR> origin_span = CollectionsMarshal.AsSpan(origin);

            int region_name = 1;
            EmitPrefix(origin_span, result, 0, 0);
            int single_line_count = FixLineObservation(result);
            info.RegionCount = region_name - 1;
            info.SingleLineResultCount = single_line_count;
            return result;

            void EmitPrefix(
                 ReadOnlySpan<RegexIR> region,
                 List<RegexIR> result,
                 int currentRegion,
                 int lineBase
            )
            {
                ReadOnlySpan<RegexIR> emit_window;
                int index = 0;
                while (index < region.Length)
                {
                    emit_window = region[index..];

                    int emitResult;
                    if ((emitResult =
                         TryEmitGroupFindPack(emit_window, result, currentRegion, lineBase)) > 0)
                    {
                        index += emitResult;
                    } else if ((emitResult =
                                TryEmitTryGroupTail(emit_window, result, currentRegion,
                                                    lineBase)) > 0)
                    {
                        index += emitResult;
                    } else if ((emitResult =
                                TryEmitAltChain(emit_window, result, currentRegion, lineBase)) > 0)
                    {
                        index += emitResult;
                    } else if ((emitResult =
                                TryEmitAltSuccessTail(emit_window, result, currentRegion,
                                                      lineBase)) > 0)
                    {
                        index += emitResult;
                    } else if ((emitResult =
                                TryEmitAltFailTail(emit_window, result, currentRegion,
                                                   lineBase)) > 0)
                    {
                        index += emitResult;
                    } else if ((emitResult =
                                TryEmitComplexIR(emit_window, result, currentRegion, lineBase)) > 0)
                    {
                        index += emitResult;
                    } else if ((emitResult =
                                TryEmitTryIR(emit_window, result, currentRegion, lineBase)) > 0)
                    {
                        index += emitResult;
                    } else
                    {
                        result.Add(CloneIR(emit_window[0], currentRegion));
                        index++;
                    }
                }
            }

            int TryEmitTryGroupTail(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase
            )
            {
                if (origin.Length < 5 || origin[0] is not RegexTryIR _try) {
                    return -1;
                }

                int tailOffset = _try.CommandCount + 1;
                if (origin.Length < tailOffset + 3 ||
                    origin[tailOffset] is not RegexConditionIR { SuccessCase: 2, FailCase: 1 } condition ||
                    origin[tailOffset + 1] is not RegexPopIR { IsSuccess: false } pop ||
                    origin[tailOffset + 2] is not RegexPackIR { Condition: true } pack) {
                    return -1;
                }

                int tryOutputIndex = result.Count;
                int emitCount = TryEmitTryIR(origin, result, currentRegion, lineBase,
                                             foldSingleInstructionTry:false);
                if (emitCount <= 0 || result[tryOutputIndex] is not RegexTryIR emittedTry) {
                    return -1;
                }

                _ = condition;
                int regionId = emittedTry.Prefix.WorkingRegion;

                RegexIR prefixedPop = CloneIR(pop, regionId);
                ApplyPrefix(prefixedPop, regionId,
                            RegexIRPrefixMask.Prev_Fail | RegexIRPrefixMask.Observe_Region,
                            regionId);

                RegexIR prefixedPack = CloneIR(pack, regionId);
                ApplyPrefix(prefixedPack, regionId,
                            RegexIRPrefixMask.Prev_Success | RegexIRPrefixMask.Observe_Region,
                            regionId);

                result.Add(prefixedPop);
                result.Add(prefixedPack);
                return emitCount + 3;
            }

            int TryEmitAltChain(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase
            )
            {
                if (origin.Length < 3 ||
                    origin[0] is not RegexConditionIR { SuccessCase: 1, FailCase: 2 } ||
                    origin[1] is not RegexPopIR { IsSuccess: true } ||
                    origin[2] is not RegexTryIR) {
                    return -1;
                }

                int observeRegion = currentRegion;
                RegexIRPrefixMask observeMask = RegexIRPrefixMask.Observe_Region;
                int observeSource = currentRegion;
                if (result.Count > 0)
                {
                    RegexIR previous = result[^1];
                    observeRegion = GetObservedRegion(previous, currentRegion);
                    GetObservation(previous, result, lineBase, currentRegion, out observeMask,
                                   out observeSource);
                    MarkPopResult(previous, RegexIRPrefixMask.Pop_Success);
                }

                RegexIRPrefixMask mask = RegexIRPrefixMask.Prev_Fail | observeMask;
                int emit_count = TryEmitTryIR(origin[2..], result, currentRegion, lineBase, mask,
                                              observeSource, observeRegion);
                return emit_count < 0 ? -1 : emit_count + 2;
            }

            int TryEmitAltSuccessTail(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase
            )
            {
                if (origin.Length < 2 ||
                    origin[0] is not RegexConditionIR { SuccessCase: 1, FailCase: 2 } ||
                    origin[1] is not RegexPopIR { IsSuccess: true } ||
                    result.Count == 0) {
                    return -1;
                }

                RegexIR previous = result[^1];
                MarkPopResult(previous, RegexIRPrefixMask.Pop_Success);
                return 2;
            }

            int TryEmitAltFailTail(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase
            )
            {
                if (origin.Length < 1 ||
                    origin[0] is not RegexPopIR { IsSuccess: false } pop ||
                    result.Count == 0) {
                    return -1;
                }

                RegexIR previous = result[^1];
                if ((previous.Prefix.Mask & RegexIRPrefixMask.Pop_Success) == 0) {
                    return -1;
                }

                GetObservation(previous, result, lineBase, currentRegion,
                               out RegexIRPrefixMask observeMask, out int observeSource);
                RegexIR prefixedPop = CloneIR(pop, currentRegion);
                ApplyPrefix(prefixedPop, currentRegion, RegexIRPrefixMask.Prev_Fail | observeMask,
                            observeSource);
                result.Add(prefixedPop);
                return 1;
            }

            int TryEmitComplexIR(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase
            )
            {
                if (origin.Length == 0) {
                    return -1;
                }

                if (origin[0] is RegexCountComplexIR count)
                {
                    int commandCount = count.CommandCount;
                    if (origin.Length < commandCount + 1) {
                        return -1;
                    }

                    List<RegexIR> sub_ir = new(commandCount);
                    EmitPrefix(origin.Slice(1, commandCount), sub_ir, currentRegion,
                               lineBase + result.Count + 1);

                    RegexCountComplexIR newCount = new(count.Min, count.Max, count.IsGreedy,
                                                       sub_ir.Count)
                    {
                        IsFullyOptimized = count.IsFullyOptimized,
                        Prefix =
                        new RegexIRPrefix(
                            count.Prefix.WorkingRegion == 0 ?
                            currentRegion :
                            count.Prefix.WorkingRegion,
                            count.Prefix.Mask,
                            count.Prefix.ObserveSource)
                    };

                    result.Add(newCount);
                    result.AddRange(sub_ir);
                    return commandCount + 1;
                }

                if (origin[0] is RegexFindComplexIR { CommandCount: > 0 } find)
                {
                    int commandCount = find.CommandCount;
                    if (origin.Length < commandCount + 1) {
                        return -1;
                    }

                    List<RegexIR> sub_ir = new(commandCount);
                    EmitPrefix(origin.Slice(1, commandCount), sub_ir, currentRegion,
                               lineBase + result.Count + 1);

                    RegexFindComplexIR newFind = new(find.IsNearest, sub_ir.Count)
                    {
                        IsFullyOptimized = find.IsFullyOptimized,
                        Prefix =
                        new RegexIRPrefix(
                            find.Prefix.WorkingRegion == 0 ?
                            currentRegion :
                            find.Prefix.WorkingRegion,
                            find.Prefix.Mask,
                            find.Prefix.ObserveSource)
                    };

                    result.Add(newFind);
                    result.AddRange(sub_ir);
                    return commandCount + 1;
                }

                return -1;
            }

            // convert TRY into region
            int TryEmitTryIR(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase,
                RegexIRPrefixMask mask = 0,
                int observeSource = 0,
                int reuseRegion = 0,
                bool foldSingleInstructionTry = true
            )
            {
                if (origin.Length == 0 || origin[0] is not RegexTryIR _try) {
                    return -1;
                }

                int commandCount = _try.CommandCount;
                if (origin.Length < commandCount + 1) {
                    return -1;
                }

                int regionId = reuseRegion == 0 ? region_name : reuseRegion;
                if (reuseRegion == 0) {
                    region_name++;
                }

                if (foldSingleInstructionTry &&
                    commandCount == 1 &&
                    IsFoldableSingleTryInstruction(origin[1]))
                {
                    RegexIR folded = CloneIR(origin[1], regionId);
                    int foldedObserveSource =
                        (mask & RegexIRPrefixMask.ObserveMethod) != 0 ?
                        observeSource :
                        _try.Prefix.ObserveSource;
                    folded.Prefix = new RegexIRPrefix(
                        regionId,
                        folded.Prefix.Mask | _try.Prefix.Mask | mask,
                        foldedObserveSource);
                    result.Add(folded);
                    return commandCount + 1;
                }

                List<RegexIR> sub_ir = new(commandCount);
                ReadOnlySpan<RegexIR> try_region = origin.Slice(1, commandCount);
                EmitPrefix(try_region, sub_ir, regionId, lineBase + result.Count + 1);

                RegexTryIR newTry = new(sub_ir.Count)
                {
                    IsFullyOptimized = _try.IsFullyOptimized,
                    Prefix = new RegexIRPrefix(regionId, _try.Prefix.Mask | mask, observeSource)
                };
                result.Add(newTry);
                result.AddRange(sub_ir);
                return commandCount + 1;
            }

            int TryEmitGroupFindPack(
                ReadOnlySpan<RegexIR> origin,
                List<RegexIR> result,
                int currentRegion,
                int lineBase
            )
            {
                if (origin.Length < 5 ||
                    origin[0] is not RegexTryIR { CommandCount: 1 } _try ||
                    origin[1] is not RegexFindComplexIR find ||
                    origin[2] is not RegexConditionIR { SuccessCase: 2, FailCase: 1 } ||
                    origin[3] is not RegexPopIR { IsSuccess: false } ||
                    origin[4] is not RegexPackIR { Condition: true } pack) {
                    return -1;
                }

                int regionId = region_name;
                region_name++;

                RegexIRPrefixMask findMask = find.Prefix.Mask | _try.Prefix.Mask;
                int findObserveSource =
                    (find.Prefix.Mask & RegexIRPrefixMask.ObserveMethod) != 0 ?
                    find.Prefix.ObserveSource :
                    _try.Prefix.ObserveSource;

                RegexFindComplexIR newFind = new(find.IsNearest, find.CommandCount)
                {
                    IsFullyOptimized = find.IsFullyOptimized,
                    Prefix = new RegexIRPrefix(regionId, findMask, findObserveSource)
                };
                RegexPackIR newPack = new(pack.Condition, pack.GroupName)
                {
                    Prefix =
                    new RegexIRPrefix(
                        regionId,
                        pack.Prefix.Mask | RegexIRPrefixMask.Prev_Success | RegexIRPrefixMask.Observe_Line,
                        -1)
                };

                result.Add(newFind);
                result.Add(newPack);
                return 5;
            }

            static int GetObservedRegion(
                       RegexIR ir,
                       int currentRegion
            )
            {
                return ir.Prefix.WorkingRegion == 0 ? currentRegion : ir.Prefix.WorkingRegion;
            }

            static void GetObservation(
                        RegexIR ir,
                        List<RegexIR> result,
                        int lineBase,
                        int currentRegion,
                        out RegexIRPrefixMask observeMask,
                        out int observeSource
            )
            {
                if (IsLineObservable(ir))
                {
                    observeMask = RegexIRPrefixMask.Observe_Line;
                    observeSource = -1;
                } else
                {
                    observeMask = RegexIRPrefixMask.Observe_Region;
                    observeSource = GetObservedRegion(ir, currentRegion);
                }
            }

            static int FixLineObservation(
                       List<RegexIR> result
            )
            {
                // Observe_Line uses relative offsets while nested regions are emitted.
                // Positive offsets point to larger line numbers; this pass resolves them
                // into absolute IR line indexes after the final instruction order is known.
                HashSet<int> single_required = [];
                for (int i = 0; i < result.Count; i++)
                {
                    RegexIRPrefix prefix = result[i].Prefix;
                    if ((prefix.Mask & RegexIRPrefixMask.Observe_Line) == 0)
                    {
                        continue;
                    }

                    int observeSource = i + prefix.ObserveSource;
                    RegexIRPrefix prefix_new = new RegexIRPrefix(
                        prefix.WorkingRegion,
                        prefix.Mask,
                        observeSource);
                    result[i].Prefix = prefix_new;
                    _ = single_required.Add(prefix_new.ObserveSource);
                }
                return single_required.Count;
            }

            static bool IsFoldableSingleTryInstruction(
                        RegexIR ir
            )
            {
                return IsLineObservable(ir) &&
                       (ir.Prefix.Mask & RegexIRPrefixMask.ExeCondition) == 0 &&
                       (ir.Prefix.Mask & RegexIRPrefixMask.ObserveMethod) == 0;
            }

            static bool IsLineObservable(
                        RegexIR ir
            )
            {
                return ir is
                    RegexMatchSeqIR or
                    RegexMatchSetIR or
                    RegexCountSeqIR or
                    RegexCountSetIR or
                    RegexFindSeqIR or
                    //RegexFindSetIR or
                    RegexFindComplexIR;
            }

            static void ApplyPrefix(
                        RegexIR ir,
                        int workingRegion,
                        RegexIRPrefixMask mask,
                        int observeSource
            )
            {
                ir.Prefix = new RegexIRPrefix(
                    workingRegion,
                    ir.Prefix.Mask | mask,
                    observeSource);
            }

            // Only annotate the pop result of an already emitted instruction.
            // The working region and observe source belong to the execution guard
            // and must stay untouched, otherwise a region id leaks into ObserveSource.
            static void MarkPopResult(
                        RegexIR ir,
                        RegexIRPrefixMask popMask
            )
            {
                ir.Prefix = new RegexIRPrefix(
                    ir.Prefix.WorkingRegion,
                    ir.Prefix.Mask | popMask,
                    ir.Prefix.ObserveSource);
            }

            static RegexIR CloneIR(
                           RegexIR ir,
                           int currentRegion
            )
            {
                RegexIR clone = ir switch
                {
                    RegexFindComplexIR find => new RegexFindComplexIR(find.IsNearest, find.CommandCount)
                    {
                        IsFullyOptimized = find.IsFullyOptimized
                    },
                    RegexFindSeqIR find => new RegexFindSeqIR(find.Pattern, find.IsFirst),
                    RegexMatchSeqIR match => new RegexMatchSeqIR(match.Pattern),
                    RegexMatchSetIR match => new RegexMatchSetIR(match.Charset),
                    RegexConditionIR condition => new RegexConditionIR(condition.SuccessCase, condition.FailCase),
                    RegexPackIR pack => new RegexPackIR(pack.Condition, pack.GroupName),
                    RegexCountComplexIR count => new RegexCountComplexIR(count.Min, count.Max,
                                                                         count.IsGreedy,
                                                                         count.CommandCount)
                    {
                        IsFullyOptimized = count.IsFullyOptimized
                    },
                    RegexCountSeqIR count => new RegexCountSeqIR(count.Min, count.Max,
                                                                 count.IsGreedy, count.Pattern),
                    RegexCountSetIR count => new RegexCountSetIR(count.Min, count.Max,
                                                                 count.IsGreedy, count.CharSet),
                    RegexTryIR tryIR => new RegexTryIR(tryIR.CommandCount)
                    {
                        IsFullyOptimized = tryIR.IsFullyOptimized
                    },
                    RegexPopIR pop => new RegexPopIR(pop.IsSuccess),
                    _ => throw new NotSupportedException($"Unsupported IR type: {ir.GetType().Name}")
                };

                clone.Prefix = new RegexIRPrefix(
                    ir.Prefix.WorkingRegion == 0 ? currentRegion : ir.Prefix.WorkingRegion,
                    ir.Prefix.Mask,
                    ir.Prefix.ObserveSource);
                return clone;
            }
        }

    }
}
