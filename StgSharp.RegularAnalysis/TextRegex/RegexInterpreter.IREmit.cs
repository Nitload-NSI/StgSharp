//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexInterpreter.IREmit"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
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
    public partial class RegexInterpreter
    {

        internal static List<RegexIR> GenerateIR(AbstractSyntaxTree<RegexAstNode, RegexElementLabel> tree)
        {
            return GenerateIRRecursion(tree.Root, 0).ToList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EmitCount(
                            int curDepth,
                            RegexIRGenerator ir,
                            int min,
                            int max,
                            bool isGreedy,
                            RegexAstNode right)
        {
            bool isRightOperator = !RegexAstNode.IsNullOrEmpty(right) &&
                                   (right.Source.Flag & RegexElementLabel.OPERATOR) != 0;

            if (isRightOperator)        // seq to be count is complex
            {
                ir.EmitCount(min, max, isGreedy, 1);
                RegexIRGenerator rightIR = GenerateIRRecursion(right, curDepth + 1);
                ir.EmitTry(rightIR.Count);
                ir.EmitIRStream(rightIR);
            } else
            {
                RegexElementLabel count_source_mask = right.Source.Flag;
                switch (count_source_mask)
                {
                    case RegexElementLabel.UNIT:
                        ir.EmitCountSeq(min, max, isGreedy, right.Source.Value);
                        break;
                    case RegexElementLabel.UNIT_SET:
                        ir.EmitCountSet(min, max, isGreedy, right.Source.Value);
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        ir.EmitCountSeq(min, max, isGreedy, right.Source.Value);
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected regex element type for count operator: {count_source_mask}");
                }
            }
        }

        private static RegexIRGenerator GenerateIRRecursion(RegexAstNode node, int depth)
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
                        group_source[3..^2] :                   /* named group (?<name>) */
                        string.Empty;                           /* unnamed group (?:) */

                    RegexIRGenerator groupIR = new();
                    rightIR = GenerateIRRecursion(right, curDepth + 1);
                    groupIR.EmitTry(rightIR.Count);
                    groupIR.EmitIRStream(rightIR);
                    groupIR.EmitCondition(2, 1);
                    /*generate pack ir here*/
                    return groupIR;
                case RegexElementLabel.COUNT:
                    string count_source = token.Value;
                    bool isGreedy = !(count_source.Length > 1 && count_source.EndsWith('?'));
                    int seq_mask = (RegexAstNode.IsNullOrEmpty(left) ? 0 : 2) + (RegexAstNode.IsNullOrEmpty(right) ?
                                                                                 0 :
                                                                                 1);
                    RegexAstNode seq = seq_mask switch
                    {
                        // no seq, invalid count operator
                        0 => throw new InvalidOperationException("Count operator must have a sequence to operate on"),
                        1 => right,// right seq
                        2 => left,// left seq
                        // both sides, invalid count operator
                        3 => throw new InvalidOperationException("Count operator cannot have two sequences to operate on"),
                        _ => throw new InvalidOperationException("Unexpected sequence mask for count operator"),
                    };
                    ReadOnlySpan<char> count_source_span = count_source;
                    RegexIRGenerator countIR = new();

                    if (count_source_span[0] == '*')              // infinite count
                    {
                        EmitCount(curDepth, countIR, 0, int.MaxValue, isGreedy, seq);
                    } else if (count_source_span[0] == '+')       // at least one
                    {
                        EmitCount(curDepth, countIR, 1, int.MaxValue, isGreedy, seq);
                    } else if (count_source_span[0] == '?')       // zero or one
                    {
                        EmitCount(curDepth, countIR, 0, 1, isGreedy, seq);
                    } else if (count_source_span[0] == '{')        // zero or one, non-greedy
                    {
                        int comma_pos = count_source_span.IndexOf(',');
                        if (comma_pos == -1)
                        {
                            if (int.TryParse(count_source[1..(isGreedy ? ^3 : ^2)], out int count))
                            {
                                EmitCount(curDepth, countIR, count, count, isGreedy, seq);
                            } else
                            {
                                throw new InvalidCastException($"Invalid count format: {count_source}");
                            }
                        } else
                        {
                            if (int.TryParse(count_source[1..comma_pos], out int min) &&
                                int.TryParse(count_source[(comma_pos + 1)..(isGreedy ? ^3 : ^2)], out int max))
                            {
                                EmitCount(curDepth, countIR, min, max, isGreedy, seq);
                            } else
                            {
                                throw new InvalidCastException($"Invalid count format: {count_source}");
                            }
                        }
                    } else
                    {
                        throw new InvalidOperationException();
                    }   // unknown count format

                    return countIR;
                case RegexElementLabel.CONCAT:
                    RegexIRGenerator concat = new();

                    // process left node
                    if (RegexAstNode.IsNullOrEmpty(left))
                    {
                        // do nothing
                    } else if ((left.Source.Flag & RegexElementLabel.SEQUENCE) != 0)
                    {
                        switch (left.Source.Flag)
                        {
                            case RegexElementLabel.UNIT_SPAN:
                                goto case RegexElementLabel.UNIT;
                            case RegexElementLabel.UNIT:
                                concat.EmitMatchSeq(left.Source.Value);
                                break;
                            case RegexElementLabel.UNIT_SET:
                                concat.EmitMatchSet(left.Source.Value);
                                break;
                            default:
                                break;
                        }
                    } else if ((left.Source.Flag & RegexElementLabel.VAST_OPERATOR) != 0)
                    {
                        leftIR = GenerateIRRecursion(left, curDepth + 1);
                        concat.EmitIRStream(leftIR);
                    }

                    // process right node
                    if (RegexAstNode.IsNullOrEmpty(right))
                    {
                        // do nothing
                    } else if ((right.Source.Flag & RegexElementLabel.SEQUENCE) != 0)
                    {
                        switch (right.Source.Flag)
                        {
                            case RegexElementLabel.UNIT_SPAN:
                                goto case RegexElementLabel.UNIT;
                            case RegexElementLabel.UNIT:
                                concat.EmitMatchSeq(right.Source.Value);
                                break;
                            case RegexElementLabel.UNIT_SET:
                                concat.EmitMatchSet(right.Source.Value);
                                break;
                            default:
                                break;
                        }
                    } else if ((right.Source.Flag & RegexElementLabel.VAST_OPERATOR) != 0)
                    {
                        rightIR = GenerateIRRecursion(right, curDepth + 1);
                        concat.EmitIRStream(rightIR);
                    }
                    return concat;
                case RegexElementLabel.ALT:
                    RegexIRGenerator alt = new();
                    List<RegexIRGenerator> casesIRList = [];
                    RegexAstNode cur_node = right;
                    int ir_count = 0;
                    while (!RegexAstNode.IsNullOrEmpty(cur_node))
                    {
                        RegexIRGenerator _case = GenerateIRRecursion(cur_node, curDepth + 1);
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
                        alt.EmitTry(cur_ir.Count);
                        alt.EmitIRStream(cur_ir);
                        alt.EmitCondition(1/* need to calc here */, 2);
                        alt.EmitPop(true);
                    }
                    alt.EmitPop(false);
                    return alt;
                case RegexElementLabel.UNIT_SPAN:
                    goto case RegexElementLabel.UNIT;
                case RegexElementLabel.UNIT:
                    RegexIRGenerator unit_seq = new();
                    unit_seq.EmitMatchSeq(node.Source.Value);
                    return unit_seq;
                case RegexElementLabel.UNIT_SET:
                    RegexIRGenerator unit_set = new();
                    unit_set.EmitMatchSet(node.Source.Value);
                    return unit_set;
                default:
                    return null!;
            }
#pragma warning restore IDE0010
        }

    }
}
