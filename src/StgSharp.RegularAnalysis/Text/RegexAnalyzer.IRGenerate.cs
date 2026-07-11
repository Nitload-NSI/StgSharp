// -----------------------------------------------------------------------------
// file="RegexAnalyzer.IRGenerate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StgSharp.RegularAnalysis.Text
{
    public partial class RegexAnalyzer
    {

        internal static List<RegexIR> GenerateIR(
                                      AbstractSyntaxTree<RegexAstNode, RegexElementLabel> tree,
                                      ref RegexInfo info
        )
        {
            List<RegexIR> list = GenerateIRRecursion(tree.Root, 0, out int length).ToList();
            info.MinPredictLength = length;
            return list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EmitCount(
                            int curDepth,
                            RegexIRGenerator ir,
                            int min,
                            int max,
                            bool isGreedy,
                            RegexAstNode right,
                            out int minLength
        )
        {
            bool is_operator = !RegexAstNode.IsNullOrEmpty(right) &&
                               (right.Source.Flag & RegexElementLabel.OPERATOR) != 0;

            if (is_operator)        // seq to be count is complex
            {
                RegexIRGenerator right_ir = GenerateIRRecursion(right, curDepth + 1, out minLength);
                _ = ir.EmitCountComplex(min, max, isGreedy, right_ir.Count);
                _ = ir.EmitIRStream(right_ir);
            } else
            {
                RegexElementLabel count_source_mask = right.Source.Flag;
                switch (count_source_mask)
                {
                    case RegexElementLabel.UNIT:
                        minLength = 1;
                        _ = ir.EmitCountSeq(min, max, isGreedy, right.Value);
                        break;
                    case RegexElementLabel.UNIT_SET:
                        minLength = 1;
                        _ = ir.EmitCountSet(min, max, isGreedy, right.Value);
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        minLength = 1;
                        _ = ir.EmitCountSeq(min, max, isGreedy, right.Value);
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected regex element type for count operator: {count_source_mask}");
                }
            }
        }

        private static RegexIRGenerator GenerateIRRecursion(
                                        RegexAstNode node,
                                        int depth,
                                        out int minLength
        )
        {
#pragma warning disable IDE0010
            if (depth > 1024)
            {
                throw new OverflowException("Regex parse exceeds maximum recursion depth");
            }
            int curDepth = depth;
            RegexAstNode left = node.Left;
            RegexAstNode right = node.Right;
            RegexIRGenerator leftIR;
            RegexIRGenerator rightIR;

            Token<RegexElementLabel> token = node.Source;

            switch (node.Source.Flag)
            {
                case RegexElementLabel.GROUP_BEGIN:
                    string group_source = token.Value;

                    // process group type
                    bool is_group = group_source.Length == 1;           // simple group: (pattern)
                    string group_name =
                        group_source.StartsWith(@"(?<", StringComparison.Ordinal) ?
                        group_source[3..^1] :                   /* named group (?<name>) */
                        string.Empty;                           /* unnamed group (?:) */

                    RegexIRGenerator groupIR = new();
                    rightIR = GenerateIRRecursion(right, curDepth + 1, out minLength);
                    _ = groupIR.EmitTry(rightIR.Count)
                               .EmitIRStream(rightIR)
                               .EmitCondition(2, 1)
                               .EmitPop(false)
                               .EmitPack(true, group_name);
                    return groupIR;
                case RegexElementLabel.COUNT:
                    string count_source = token.Value;
                    bool isGreedy = !(count_source.Length > 1 && count_source.EndsWith('?'));
                    RegexAstNode seq = right;
                    ReadOnlySpan<char> count_source_span = count_source;
                    RegexIRGenerator count_ir = new();
                    int length_once;
                    if (count_source_span[0] == '*')              // infinite count
                    {
                        EmitCount(curDepth, count_ir, 0, -1, isGreedy, seq, out _);
                        minLength = 0;
                    } else if (count_source_span[0] == '+')       // at least one
                    {
                        EmitCount(curDepth, count_ir, 1, -1, isGreedy, seq, out length_once);
                        minLength = length_once;
                    } else if (count_source_span[0] == '?')       // zero or one
                    {
                        EmitCount(curDepth, count_ir, 0, 1, isGreedy, seq, out _);
                        minLength = 0;
                    } else if (count_source_span[0] == '{')        // zero or one, non-greedy
                    {
                        int comma_pos = count_source_span.IndexOf(',');
                        if (comma_pos == -1)
                        {
                            if (int.TryParse(count_source[1..(isGreedy ? ^2 : ^1)], out int count))
                            {
                                EmitCount(curDepth, count_ir, count, count, isGreedy, seq,
                                          out length_once);
                                minLength = length_once * count;
                            } else
                            {
                                throw new InvalidCastException($"Invalid count format: {count_source}");
                            }
                        } else
                        {
                            if (int.TryParse(count_source[1..comma_pos], out int min) &&
                                int.TryParse(count_source[(comma_pos + 1)..(isGreedy ? ^1 : ^2)], out int max))
                            {
                                EmitCount(curDepth, count_ir, min, max, isGreedy, seq,
                                          out length_once);
                                minLength = length_once * min;
                            } else
                            {
                                throw new InvalidCastException($"Invalid count format: {count_source}");
                            }
                        }
                    } else
                    {
                        throw new InvalidOperationException();
                    }   // unknown count format

                    return count_ir;
                case RegexElementLabel.CONCAT:
                    RegexIRGenerator concat = new();

                    // process left node
                    int left_length = 0;
                    if (RegexAstNode.IsNullOrEmpty(left))
                    {
                        // do nothing
                    } else if ((left.Source.Flag & RegexElementLabel.SEQUENCE) != 0)
                    {
                        switch (left.Source.Flag)
                        {
                            case RegexElementLabel.UNIT_SPAN:
                                left_length = left.Value.Length;
                                _ = concat.EmitMatchSeq(left.Value);
                                break;
                            case RegexElementLabel.UNIT:
                                left_length = 1;
                                _ = concat.EmitMatchSeq(left.Value);
                                break;
                            case RegexElementLabel.UNIT_SET:
                                left_length = 1;
                                _ = concat.EmitMatchSet(left.Value);
                                break;
                            default:
                                break;
                        }
                    } else if ((left.Source.Flag & RegexElementLabel.VAST_OPERATOR) != 0)
                    {
                        leftIR = GenerateIRRecursion(left, curDepth + 1, out left_length);
                        _ = concat.EmitIRStream(leftIR);
                    }

                    // process right node
                    int right_length = 0;
                    if (RegexAstNode.IsNullOrEmpty(right))
                    {
                        // do nothing
                    } else if ((right.Source.Flag & RegexElementLabel.SEQUENCE) != 0)
                    {
                        switch (right.Source.Flag)
                        {
                            case RegexElementLabel.UNIT_SPAN:
                                right_length = right.Value.Length;
                                _ = concat.EmitMatchSeq(right.Value);
                                break;
                            case RegexElementLabel.UNIT:
                                right_length = 1;
                                _ = concat.EmitMatchSeq(right.Value);
                                break;
                            case RegexElementLabel.UNIT_SET:
                                right_length = 1;
                                _ = concat.EmitMatchSet(right.Value);
                                break;
                            default:
                                break;
                        }
                    } else if ((right.Source.Flag & RegexElementLabel.VAST_OPERATOR) != 0)
                    {
                        rightIR = GenerateIRRecursion(right, curDepth + 1, out right_length);
                        _ = concat.EmitIRStream(rightIR);
                    }
                    minLength = left_length + right_length;
                    return concat;
                case RegexElementLabel.ALT:
                    RegexIRGenerator alt = new();
                    List<RegexIRGenerator> casesIRList = [];
                    RegexAstNode cur_node = right;
                    int ir_count = 0;
                    int min_case_length = int.MaxValue;
                    while (!RegexAstNode.IsNullOrEmpty(cur_node))
                    {
                        RegexIRGenerator _case = GenerateIRRecursion(cur_node, curDepth + 1, out int case_length);
                        min_case_length = int.Min(case_length, min_case_length);
                        casesIRList.Add(_case);
                        ir_count += _case.Count;
                        cur_node = cur_node.Next;
                    }
                    int case_count = casesIRList.Count;
                    while (case_count != 0)
                    {
                        RegexIRGenerator cur_ir = casesIRList[^case_count];
                        ir_count -= cur_ir.Count;
                        case_count--;
                        _ = alt.EmitTry(cur_ir.Count)
                               .EmitIRStream(cur_ir)
                               .EmitCondition(1/* need to calc here */, 2)
                               .EmitPop(true);
                    }
                    _ = alt.EmitPop(false);
                    minLength = min_case_length;
                    return alt;
                case RegexElementLabel.UNIT_SPAN:
                    RegexIRGenerator unit = new();
                    _ = unit.EmitMatchSeq(node.Value);
                    minLength = node.Value.Length;
                    return unit;
                case RegexElementLabel.UNIT:
                    RegexIRGenerator unit_seq = new();
                    _ = unit_seq.EmitMatchSeq(node.Value);
                    minLength = 1;
                    return unit_seq;
                case RegexElementLabel.UNIT_SET:
                    RegexIRGenerator unit_set = new();
                    _ = unit_set.EmitMatchSet(node.Value);
                    minLength = 1;
                    return unit_set;
                default:
                    minLength = 0;
                    return null!;
            }
#pragma warning restore IDE0010


            // info.MinPredictLength = ComputeMinLength(tree.Root);

            static int ComputeMinLength(
                       RegexAstNode node
            )
            {
                if (node.EqualityTypeConvert is RegexElementLabel.UNIT_SPAN or
                    RegexElementLabel.UNIT)
                {
                    return node.Value.Length;
                } else if (node.EqualityTypeConvert == RegexElementLabel.CONCAT)
                {
                    return ComputeMinLength(node.Left) + ComputeMinLength(node.Right);
                } else if (node.EqualityTypeConvert == RegexElementLabel.ALT)
                {
                    RegexAstNode case_0 = node.Right;
                    int length = int.MaxValue;
                    do
                    {
                        length = int.Min(ComputeMinLength(case_0), length);
                        case_0 = case_0.Next;
                    } while (!RegexAstNode.IsNullOrEmpty(case_0));
                    return length;
                } else if (node.EqualityTypeConvert == RegexElementLabel.COUNT)
                {
                    string source = node.Source.Value;
                    int single_count = ComputeMinLength(node.Right);
                    return 0;
                } else
                {
                    return 0;
                }
            }
        }

    }
}
