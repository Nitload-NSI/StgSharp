// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Nitload.RegularAnalysis.Abstraction;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nitload.RegularAnalysis.Text
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
                    default:

                        // TODO(NGRA): Report an unsupported operator instead of silently emitting
                        // an incomplete matcher.
                        break;
                }
            } else
            {
                op = root.Label & (RegexElementLabel.SEQUENCE);
                switch (op)
                {
                    case RegexElementLabel.UNIT:

                        // TODO(NGRA): Emit the remaining-span length guard, literal comparison,
                        // cursor advance, and failure transfer for one character.
                        break;
                    case RegexElementLabel.UNIT_SET:

                        // TODO(NGRA): Emit the remaining-span length guard, charset predicate,
                        // cursor advance, and failure transfer.
                        break;
                    case RegexElementLabel.UNIT_SPAN:

                        // TODO(NGRA): Emit StartsWith for the complete literal span and advance by
                        // the literal length; integrate is_find candidate retry semantics.
                        break;
                    default:

                        // TODO(NGRA): Diagnose malformed leaf nodes instead of generating no code.
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

        private static string FormatStringLiteral(
                              ReadOnlySpan<char> value
        )
        {
            return SyntaxFactory.Literal(value.ToString()).Text;
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
            string remain_span = SourceGenContext._remain_span;
            string accept = set.Accept ? string.Empty : "!";
            switch (set.Type)
            {
                case RegexCharSetType.Single:
                    return $"{accept}({FormatStringLiteral(set.Value)}.{nameof(MemoryExtensions.Contains)}({remain_span}[0]))";
                case RegexCharSetType.Range:
                    return $"{accept}({remain_span}[0] >= {FormatCharLiteral(set.Value[0])} && {remain_span}[0] <= {FormatCharLiteral(set.Value[1])})";
                case RegexCharSetType.Set:
                    switch (set.Value[0])
                    {
                        case 's':
                            return $"{accept}char.IsWhiteSpace({remain_span}[0])";
                        case 'w':
                            return $"{accept}{nameof(TextRegex.IsCharWord)}({remain_span}[0])";
                        case 'd':
                            return $"{accept}char.IsDigit({remain_span}[0])";
                        default:
                            break;
                    }
                    goto default;
                default:

                    // TODO(NGRA): Unsupported/empty charset rules must become an analyzer
                    // diagnostic; returning an empty expression produces invalid generated C#.
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

            // TODO(NGRA): Validate the normalized CONCAT/GROUP/COUNT shape with Empty sentinels
            // before dereferencing children; this helper currently assumes every edge exists.
            if ((group.Label & RegexElementLabel.GROUP_BEGIN) == 0)
            {
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
            string exit_label = context.RequestExitLabel();

            // TODO(NGRA): Walk every normalized ALT candidate exactly once. The final traversal
            // shape and failure continuation are not wired yet.
            while (RegexAstNode.IsNullOrEmpty(current))
            {
                RegexElementLabel op = current.Label & RegexElementLabel.SEQUENCE;
                if (op != 0)
                {
                    string code = current.SourceCode;
                    switch (op)
                    {
                        case RegexElementLabel.UNIT:

                            // TODO(NGRA): Guard against an empty remaining span before indexing it.
                            _ = sc.AppendLine(@$"if({context.RemainSpan}[0] == {FormatCharLiteral(code[0])})")
                                  .AppendLine("{")
                                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                                  .AppendLine($@"goto {exit_label};")
                                  .AppendLine("}");
                            break;
                        case RegexElementLabel.UNIT_SET:

                            // TODO(NGRA): Guard the remaining span, handle empty/Any sets, and
                            // validate negated-set composition before emitting this predicate.
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

                            // TODO(NGRA): Advance by code.Length rather than one character and
                            // preserve the ALT rollback cursor on failure.
                            _ = sc.AppendLine(@$"if({context.RemainSpan}.{MemoryExtensions.StartsWith}({FormatStringLiteral(code)}))")
                                  .AppendLine("{")
                                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                                  .AppendLine($@"goto {exit_label};")
                                  .AppendLine("}");

                            break;
                        default:

                            // TODO(NGRA): Route unsupported ALT candidates through the recursive
                            // matcher or report a source-generation diagnostic.
                            break;
                    }

                    // TODO(NGRA): Advance to the next ALT candidate after a scalar candidate
                    // misses; the current sequence branch does not update current.
                } else
                {
                    GenerateMethodSource(current, sc, func_list, context);
                    current = current.Next;
                }
            }

            // TODO(NGRA): Emit the all-alternatives-failed continuation before the shared success
            // label, including restoration of the entry cursor and capture state.
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

            // TODO(NGRA): Select the owned group body using Empty-sentinel checks, snapshot its
            // start cursor, and restore it when the child matcher fails.
            if (left is not null)
            {
                GenerateMethodSource(left, sc, func_list, context);
            } else
            if (right is not null) {
                GenerateMethodSource(right, sc, func_list, context);
            }

            string group_name = config.Name;

            // TODO(NGRA): Allocate numeric/named capture slots and emit the successful capture
            // span. Non-capturing groups still need a rollback scope but no stored capture.
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
                    // TODO(NGRA): Emit the unnamed FIND candidate loop and retry the right subtree
                    // until it succeeds or no anchor candidate remains.
                    GenerateMethodSource(right, sc, func_list, context, true);
                } else if (IsNamedFind(node))
                {
                    if (right is not null)
                    {
                        // TODO(NGRA): Emit named FIND retry and capture the text preceding the
                        // successful anchor candidate.
                        GenerateMethodSource(right, sc, func_list, context, true);
                    } else
                    {
                        // TODO(NGRA): Capture the remaining input for a terminal named dot-star.
                    }
                } else
                {
                    GenerateMethodSource(left, sc, func_list, context);
                    if (right is not null) {
                        GenerateMethodSource(right, sc, func_list, context);
                    }
                }
            }

            // TODO(NGRA): Normalize ownership so the right subtree is emitted exactly once. It is
            // currently reachable both inside the ordinary CONCAT branch and here.
            if (right is not null)
            {
                GenerateMethodSource(right, sc, func_list, context);
            }
        }

        private const int flattened_unit_count_threshold = 32;

        private static void GenerateUnitCount(
                            RegexAstNode value,
                            int min,
                            int max,
                            bool is_greedy,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context
        )
        {
            if (min < 0 || (max >= 0 && max < min)) {
                throw new InvalidOperationException($"Invalid COUNT range: {min}..{max}.");
            }

            string literal = value.Value;
            if (literal.Length != 1) {
                throw new InvalidOperationException(
                    $"COUNT(UNIT) requires one decoded character, but received {literal.Length}.");
            }

            char expected = literal[0];
            string expected_literal = FormatCharLiteral(expected);
            string remain_span = context.RemainSpan;
            int scratch_scope_index = context.RentScratchScopeIndex();
            string required_rest = $"__required_rest_{scratch_scope_index}";
            string required_matched = $"__required_matched_{scratch_scope_index}";
            string required_chunk_index = $"__required_chunk_index_{scratch_scope_index}";
            string additional_count = $"__additional_count_{scratch_scope_index}";

            // TODO(NGRA): Move ownership of this work region to the enclosing CONCAT or match
            // method. Until then, break demonstrates the local failure path but cannot prevent a
            // later sibling node from being emitted and executed.
            _ = sc.AppendLine("do").AppendLine("{");

            if (min <= flattened_unit_count_threshold)
            {
                if (min > 0)
                {
                    string required_prefix = new(expected, min);
                    _ = sc.AppendLine($"if (!{remain_span}.{nameof(MemoryExtensions.StartsWith)}({FormatStringLiteral(required_prefix)}))")
                          .AppendLine("{")
                          .AppendLine("break;")
                          .AppendLine("}")
                          .AppendLine($"{remain_span} = {remain_span}.Slice({min});");
                }
            } else
            {
                // Keep large lower bounds vectorizable without emitting a giant literal. Validate
                // repeated threshold-sized chunks against a local span and commit the cursor only
                // after every chunk has matched.
                string required_chunk = new(expected, flattened_unit_count_threshold);
                int required_chunk_count = min / flattened_unit_count_threshold;
                int required_remainder_length = min % flattened_unit_count_threshold;

                _ = sc.AppendLine($"if ({remain_span}.Length < {min})")
                      .AppendLine("{")
                      .AppendLine("break;")
                      .AppendLine("}")
                      .AppendLine($"{ROS_char} {required_rest} = {remain_span};")
                      .AppendLine($"bool {required_matched} = true;")
                      .AppendLine($"for (int {required_chunk_index} = 0; {required_chunk_index} < {required_chunk_count}; {required_chunk_index}++)")
                      .AppendLine("{")
                      .AppendLine($"if (!{required_rest}.{nameof(MemoryExtensions.StartsWith)}({FormatStringLiteral(required_chunk)}))")
                      .AppendLine("{")
                      .AppendLine($"{required_matched} = false;")
                      .AppendLine("break;")
                      .AppendLine("}")
                      .AppendLine($"{required_rest} = {required_rest}.Slice({flattened_unit_count_threshold});")
                      .AppendLine("}");

                if (required_remainder_length > 0)
                {
                    string required_remainder = new(expected, required_remainder_length);
                    _ = sc.AppendLine($"if ({required_matched} && !{required_rest}.{nameof(MemoryExtensions.StartsWith)}({FormatStringLiteral(required_remainder)}))")
                          .AppendLine("{")
                          .AppendLine($"{required_matched} = false;")
                          .AppendLine("}");
                }

                _ = sc.AppendLine($"if (!{required_matched})")
                      .AppendLine("{")
                      .AppendLine("break;")
                      .AppendLine("}")
                      .AppendLine($"{remain_span} = {remain_span}.Slice({min});");
            }

            int additional_limit = max < 0 ? -1 : max - min;
            if (is_greedy && additional_limit != 0)
            {
                if (additional_limit < 0)
                {
                    _ = sc.AppendLine($"while (!{remain_span}.IsEmpty && {remain_span}[0] == {expected_literal})")
                          .AppendLine("{")
                          .AppendLine($"{remain_span} = {remain_span}.Slice(1);")
                          .AppendLine("}");
                } else
                {
                    _ = sc.AppendLine($"for (int {additional_count} = 0; {additional_count} < {additional_limit} && !{remain_span}.IsEmpty && {remain_span}[0] == {expected_literal}; {additional_count}++)")
                          .AppendLine("{")
                          .AppendLine($"{remain_span} = {remain_span}.Slice(1);")
                          .AppendLine("}");
                }
            } else if (!is_greedy && additional_limit != 0)
            {
                // Lazy COUNT deliberately stops at Min. Its optional tail is consumed only when
                // a future continuation attempt fails and requests another repetition.
                _ = sc.AppendLine("// TODO(NGRA): Retry the lazy COUNT tail from its continuation.");
            }

            _ = sc.AppendLine("} while (false);");
            context.ReturnScratchScopeIndex(scratch_scope_index);
        }

        private static void GenerateUnitSetCount(
                            RegexAstNode value,
                            int min,
                            int max,
                            bool is_greedy,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context
        )
        {
            if (min < 0 || (max >= 0 && max < min)) {
                throw new InvalidOperationException($"Invalid COUNT range: {min}..{max}.");
            }

            string literal = value.Value;
            if (literal.Length != 1) {
                throw new InvalidOperationException(
                    $"COUNT(UNIT) requires one decoded character, but received {literal.Length}.");
            }

            RegexCharSetPayload payload = value.PayloadAs<RegexCharSetPayload>();

            IReadOnlyList<RegexCharSet> charset = payload.Set;
            string remain_span = context.RemainSpan;
            int scratch_scope_index = context.RentScratchScopeIndex();
            string required_rest = $"__required_rest_{scratch_scope_index}";
            string required_matched = $"__required_matched_{scratch_scope_index}";
            string required_chunk_index = $"__required_chunk_index_{scratch_scope_index}";
            string additional_count = $"__additional_count_{scratch_scope_index}";


            // TODO(NGRA): Move ownership of this work region to the enclosing CONCAT or match
            // method. Until then, break demonstrates the local failure path but cannot prevent a
            // later sibling node from being emitted and executed.
            bool accept = payload.Accept;
            RegexCharSet _set = charset[0];
            _ = sc.AppendLine($"{ROS_char} {required_matched} = {remain_span}")
                  .AppendLine($"int {additional_count} = 0;")
                  .AppendLine()
                  .AppendLine() //TODO loop head
                  .AppendLine($"if({(accept ? string.Empty : "!(")} {GenerateCharSetExpression(_set)} ||");
            for (int i = 1; i < charset.Count - 1; i++)
            {
                _set = charset[i];
                _ = sc.AppendLine($"    {GenerateCharSetExpression(_set)} ||");
            }
            _set = charset[^1];
            _ = sc.AppendLine($"    {GenerateCharSetExpression(_set)} ){(accept ? string.Empty : ')')}")
                  .AppendLine("{")
                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                  .AppendLine($@"break;")
                  .AppendLine("}");
        }

        private static void GenerateUnitSpanCount(
                            RegexAstNode value,
                            int min,
                            int max,
                            bool is_greedy,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context
        )
        {
            if (min < 0 || (max >= 0 && max < min)) {
                throw new InvalidOperationException($"Invalid COUNT range: {min}..{max}.");
            }

            string literal = value.Value;
            if (literal.Length == 0) {
                throw new InvalidOperationException(
                    "COUNT(UNIT_SPAN) requires a non-empty literal.");
            }

            string expected_literal = FormatStringLiteral(literal);
            string remain_span = context.RemainSpan;
            int scratch_scope_index = context.RentScratchScopeIndex();
            string required_rest = $"__required_rest_{scratch_scope_index}";
            string required_matched = $"__required_matched_{scratch_scope_index}";
            string required_chunk_index = $"__required_chunk_index_{scratch_scope_index}";
            string required_count = $"__required_count_{scratch_scope_index}";
            string additional_count = $"__additional_count_{scratch_scope_index}";
            string starts_with = $"{nameof(MemoryExtensions.StartsWith)}";

            // TODO(NGRA): Move ownership of this work region to the enclosing CONCAT or match
            // method. Until then, break demonstrates the local failure path but cannot prevent a
            // later sibling node from being emitted and executed.
            _ = sc.AppendLine("do").AppendLine("{");

            if (literal.Length > flattened_unit_count_threshold)
            {
                // One repetition is already larger than the flattening threshold. Match the
                // original literal directly and keep all cursor mutations local until Min has
                // succeeded; the common tail below remains responsible for Min..Max.
                _ = sc.AppendLine($"int {required_count} = 0;")
                      .AppendLine($"{ROS_char} {required_rest} = {remain_span};")
                      .AppendLine($"for (; {required_count} < {min} && {required_rest}.{starts_with}({expected_literal}); {required_count}++)")
                      .AppendLine("{")
                      .AppendLine($"{required_rest} = {required_rest}.Slice({literal.Length});")
                      .AppendLine("}")
                      .AppendLine($"if ({required_count} != {min})")
                      .AppendLine("{")
                      .AppendLine("break;")
                      .AppendLine("}")
                      .AppendLine($"{remain_span} = {required_rest};");
            } else if (min <= flattened_unit_count_threshold / literal.Length)
            {
                if (min > 0)
                {
                    Span<char> required_prefix_span = stackalloc char[literal.Length * min];
                    for (int i = 0; i < min; i++) {
                        literal.AsSpan().CopyTo(required_prefix_span[(i * literal.Length)..]);
                    }
                    string required_prefix = required_prefix_span.ToString();
                    _ = sc.AppendLine($"if (!{remain_span}.{starts_with}({FormatStringLiteral(required_prefix)}))")
                          .AppendLine("{")
                          .AppendLine("break;")
                          .AppendLine("}")
                          .AppendLine($"{remain_span} = {remain_span}.Slice({min * literal.Length});");
                }
            } else
            {
                // Keep large lower bounds vectorizable without emitting a giant literal. Validate
                // repeated threshold-sized chunks against a local span and commit the cursor only
                // after every chunk has matched.
                int count = flattened_unit_count_threshold / literal.Length;
                Span<char> required_prefix_span = stackalloc char[literal.Length * count];
                for (int i = 0; i < count; i++) {
                    literal.AsSpan().CopyTo(required_prefix_span[(i * literal.Length)..]);
                }
                string required_chunk = required_prefix_span.ToString();
                int required_chunk_count = min / count;
                int required_remainder_length = min % count;

                _ = sc.AppendLine($"{ROS_char} {required_rest} = {remain_span};")
                      .AppendLine($"bool {required_matched} = true;")
                      .AppendLine($"for (int {required_chunk_index} = 0; {required_chunk_index} < {required_chunk_count}; {required_chunk_index}++)")
                      .AppendLine("{")
                      .AppendLine($"if (!{required_rest}.{starts_with}({FormatStringLiteral(required_chunk)}))")
                      .AppendLine("{")
                      .AppendLine($"{required_matched} = false;")
                      .AppendLine("break;")
                      .AppendLine("}")
                      .AppendLine($"{required_rest} = {required_rest}.Slice({count * literal.Length});")
                      .AppendLine("}");

                if (required_remainder_length > 0)
                {
                    Span<char> required_remainder_span = stackalloc char[literal.Length * required_remainder_length];
                    for (int i = 0; i < required_remainder_length; i++) {
                        literal.AsSpan().CopyTo(required_remainder_span[(i * literal.Length)..]);
                    }
                    string required_remainder = required_remainder_span.ToString();
                    _ = sc.AppendLine($"if ({required_matched} && !{required_rest}.{starts_with}({FormatStringLiteral(required_remainder)}))")
                          .AppendLine("{")
                          .AppendLine($"{required_matched} = false;")
                          .AppendLine("}")
                          .AppendLine($"if ({required_matched})")
                          .AppendLine("{")
                          .AppendLine($"{required_rest} = {required_rest}.Slice({literal.Length * required_remainder_length});")
                          .AppendLine("}");
                }

                _ = sc.AppendLine($"if (!{required_matched})")
                      .AppendLine("{")
                      .AppendLine("break;")
                      .AppendLine("}")
                      .AppendLine($"{remain_span} = {required_rest};");
            }

            int additional_limit = max < 0 ? -1 : max - min;
            if (is_greedy && additional_limit != 0)
            {
                if (additional_limit < 0)
                {
                    _ = sc.AppendLine($"while ({remain_span}.{starts_with}({expected_literal}))")
                          .AppendLine("{")
                          .AppendLine($"{remain_span} = {remain_span}.Slice({literal.Length});")
                          .AppendLine("}");
                } else
                {
                    _ = sc.AppendLine($"for (int {additional_count} = 0; {additional_count} < {additional_limit} && {remain_span}.{starts_with}({expected_literal}); {additional_count}++)")
                          .AppendLine("{")
                          .AppendLine($"{remain_span} = {remain_span}.Slice({literal.Length});")
                          .AppendLine("}");
                }
            } else if (!is_greedy && additional_limit != 0)
            {
                // Lazy COUNT deliberately stops at Min. Its optional tail is consumed only when
                // a future continuation attempt fails and requests another repetition.
                _ = sc.AppendLine("// TODO(NGRA): Retry the lazy COUNT tail from its continuation.");
            }

            _ = sc.AppendLine("} while (false);");
            context.ReturnScratchScopeIndex(scratch_scope_index);
        }

        private static void GenerateCount(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context
        )
        {
            RegexAstNode value = RegexAstNode.IsNullOrEmpty(node.Left) ? node.Right : node.Left;
            if (RegexAstNode.IsNullOrEmpty(value)) {
                throw new InvalidOperationException("COUNT requires an operand.");
            }
            RegexCountPayload payload = node.PayloadAs<RegexCountPayload>();
            int min = payload.Min;
            int max = payload.Max;
            bool is_greedy = payload.IsGreedy;
            string exit_label = context.RequestExitLabel();
            if ((value.Label & RegexElementLabel.SEQUENCE) == 0)
            {
                // TODO(NGRA): Emit min/max repetition, zero-width progress protection, greedy vs.
                // lazy ordering, and per-attempt rollback for a complex COUNT body.
                GenerateMethodSource(value, sc, func_list, context);
            } else
            {
                switch (value.Label & RegexElementLabel.SEQUENCE)
                {
                    case RegexElementLabel.UNIT:
                        GenerateUnitCount(value, min, max, is_greedy, sc, context);
                        break;
                    case RegexElementLabel.UNIT_SET:

                        // TODO(NGRA): Emit a bounded/unbounded charset counting loop, preferably
                        // using IndexOfAnyExcept where the charset shape permits it.
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        GenerateUnitSpanCount(value, min, max, is_greedy, sc, context);
                        break;
                    default:

                        // TODO(NGRA): Diagnose malformed COUNT children instead of emitting an
                        // empty branch.
                        break;
                }
            }

            // TODO(NGRA): Connect COUNT success/failure continuations to the surrounding rollback
            // scope; the label is currently only a placeholder.
            _ = sc.AppendLine($@"{exit_label}: {{ }}");
        }

        #endregion
    }
}
