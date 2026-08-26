// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        internal static void GenerateMethodSource(
                             RegexAstNode ast,
                             in SequenceEmitter<string> sc,
                             in List<SequenceEmitter<string>> func_list,
                             in SourceGenContext context,
                             bool is_find = false

        )
        {
            RegexAstNode root = ast;
            if (root.Label == RegexElementLabel.EPSILON) {
                return;
            }
            RegexElementLabel op = root.Label & (RegexElementLabel.VAST_OPERATOR);
            if (op != 0)
            {
                switch (op)
                {
                    case RegexElementLabel.CONCAT:
                        GenerateConcat(root, sc, func_list, context);
                        break;
                    case RegexElementLabel.COUNT:
                        GenerateCount(root, sc, func_list, context);
                        break;
                    case RegexElementLabel.GROUP_BEGIN:
                        GenerateGroup(root, sc, func_list, context);
                        break;
                    case RegexElementLabel.ALT:
                        GenerateAlt(root, sc, func_list, context);
                        break;
                }
            } else
            {
                op = root.Label & (RegexElementLabel.SEQUENCE);
                switch (op)
                {
                    case RegexElementLabel.UNIT:
                        break;
                    case RegexElementLabel.UNIT_SET:
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        break;
                }
            }
        }

        #region supporting method

        private static string FormatStringLiteral(
                              string value
        )
        {
            return SyntaxFactory.Literal(value).Text;
        }

        private static string FormatCharLiteral(
                              char value
        )
        {
            return SyntaxFactory.Literal(value).Text;
        }

        private static string GenerateCharSetExpression(
                              RegexCharSet set
        )
        {
            string remainSpan = SourceGenContext._remain_span;
            string accept = set.Accept ? string.Empty : "!";
            switch (set.Type)
            {
                case RegexCharSetType.Single:
                    return $"{accept}({FormatStringLiteral(set.Value)}.{nameof(MemoryExtensions.Contains)}({remainSpan}[0]))";
                case RegexCharSetType.Range:
                    return $"{accept}({remainSpan}[0] >= {FormatCharLiteral(set.Value[0])} && {remainSpan}[0] <= {FormatCharLiteral(set.Value[1])})";
                case RegexCharSetType.Set:
                    switch (set.Value[0])
                    {
                        case 's':
                            return $"{accept}char.IsWhiteSpace({remainSpan}[0])";
                        case 'w':
                            return $"{accept}{nameof(TextRegex.IsCharWord)}({remainSpan}[0])";
                        case 'd':
                            return $"{accept}char.IsDigit({remainSpan}[0])";
                        default:
                            break;
                    }
                    goto default;
                default:
                    return string.Empty;
            }
        }

        private static bool IsNamedFind(
                            RegexAstNode node
        )
        {
            // (?<name>.*)abc

            if (node is null)
            {
                return false;
            }
            if ((node.Label & RegexElementLabel.CONCAT) != 0) {
                return false;
            }
            RegexAstNode group = node.Left;
            if ((group.Label & RegexElementLabel.GROUP_BEGIN) == 0) {
                return false;
            }
            RegexAstNode count = group.Right;

            // .*
            return (count.Label & RegexElementLabel.COUNT) != 0 &&
                   (count.Right.Label & RegexElementLabel.UNIT_SINGLE) != 0 &&
                   count.Right.Source.Source == ".";
        }

            #endregion

        #region basic verb generate

        private static void GenerateAlt(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context
        )
        {
            RegexAstNode current = node.Right;
            string exit_label = context.RequestExitAlt();
            while (RegexAstNode.IsNullOrEmpty(current))
            {
                RegexElementLabel op = current.Label & RegexElementLabel.SEQUENCE;
                if (op != 0)
                {
                    string code = current.SourceCode;
                    switch (op)
                    {
                        case RegexElementLabel.UNIT:
                            _ = sc.AppendLine(@$"if({context.RemainSpan}[0] == {FormatCharLiteral(code[0])})")
                                  .AppendLine("{")
                                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                                  .AppendLine($@"goto {exit_label};")
                                  .AppendLine("}");
                            break;
                        case RegexElementLabel.UNIT_SET:
                            RegexCharSetPayload payload = current.PayloadAs<RegexCharSetPayload>();
                            IReadOnlyList<RegexCharSet> split = payload.Set;
                            bool accept = payload.Accept;
                            RegexCharSet _set = split[0];
                            _ = sc.AppendLine($"if({(accept ? string.Empty : "!(")} {GenerateCharSetExpression(_set)} ||");
                            for (int i = 1; i < split.Count - 1; i++)
                            {
                                _set = split[i];
                                _ = sc.AppendLine($"    {GenerateCharSetExpression(_set)} ||");
                            }
                            _set = split[^1];
                            _ = sc.AppendLine($"    {GenerateCharSetExpression(_set)} ){(accept ? string.Empty : ')')}")
                                  .AppendLine("{")
                                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                                  .AppendLine($@"goto {exit_label};")
                                  .AppendLine("}");
                            break;
                        case RegexElementLabel.UNIT_SPAN:
                            _ = sc.AppendLine(@$"if({context.RemainSpan}.{MemoryExtensions.StartsWith}({FormatStringLiteral(code)}))")
                                  .AppendLine("{")
                                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                                  .AppendLine($@"goto {exit_label};")
                                  .AppendLine("}");

                            break;
                        default:
                            break;
                    }
                } else
                {
                    GenerateMethodSource(current, sc, func_list, context);
                    current = current.Next;
                }
            }
            _ = sc.AppendLine($@"{exit_label}: {{ }}");
        }

        private static void GenerateGroup(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context
        )
        {
            RegexAstNode left = node.Left;
            RegexAstNode right = node.Right;

            RegexGroupPayload config = node.PayloadAs<RegexGroupPayload>();

            if (left is not null)
            {
                GenerateMethodSource(left, sc, func_list, context);
            } else
            if (right is not null) {
                GenerateMethodSource(right, sc, func_list, context);
            }

            string group_name = config.Name;
            throw new NotImplementedException();
        }

        private static void GenerateConcat(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context
        )
        {
            RegexAstNode left = node.Left;
            RegexAstNode right = node.Right;

            if (left is not null)
            {
                // find
                if ((left.Label & RegexElementLabel.COUNT) == RegexElementLabel.COUNT &&
                    (left.Right.Label & RegexElementLabel.UNIT_SINGLE) == RegexElementLabel.UNIT_SINGLE &&
                    left.Right.Source.Source == "." &&
                    right is not null)
                {
                    // TODO use find
                    GenerateMethodSource(right, sc, func_list, context, true);
                } else if (IsNamedFind(node))
                {
                    if (right is not null)
                    {
                        // TODO named find
                        GenerateMethodSource(right, sc, func_list, context, true);
                    } else
                    {
                        // TODO pack rest of string to group
                    }
                } else
                {
                    GenerateMethodSource(left, sc, func_list, context);
                    if (right is not null) {
                        GenerateMethodSource(right, sc, func_list, context);
                    }
                }
            }
            if (right is not null) {
                GenerateMethodSource(right, sc, func_list, context);
            }
        }

        private static void GenerateCount(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context
        )
        {
            RegexAstNode value = node.Left ?? node.Right;
            if ((value.Label & RegexElementLabel.SEQUENCE) == 0)
            {
                GenerateMethodSource(value, sc, func_list, context);
            } else
            {
                RegexCountPayload payload = value.PayloadAs<RegexCountPayload>();
                int min = payload.Min;
                int max = payload.Max;
                bool is_greedy = payload.IsGreedy;
                switch (value.Label & RegexElementLabel.SEQUENCE)
                {
                    case RegexElementLabel.UNIT:
                        break;
                    case RegexElementLabel.UNIT_SET:
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        break;
                }
            }
        }

        #endregion
    }
}
