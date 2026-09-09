// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Nitload.RegularAnalysis.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace Nitload.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        internal static void GenerateMethodSource(
                             RegexAstNode ast,
                             in SequenceEmitter<string> sc,
                             in List<SequenceEmitter<string>> func_list,
                             in SourceGenContext context,
                             in ExitLabel exit_label = default,
                             bool is_find = false
        )
        {
            RegexAstNode root = ast;
            if (root.Label == RegexElementLabel.EPSILON) {
                return;
            }
            if (TryGenerateSpecialSource(root, sc, func_list, context, exit_label)) {
                return;
            }
            RegexElementLabel op = root.Label & (RegexElementLabel.VAST_OPERATOR);
            if (op != 0)
            {
                switch (op)
                {
                    case RegexElementLabel.CONCAT:
                        GenerateConcat(root, sc, func_list, context, exit_label);
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
                        GenerateUnit(root, sc, context, exit_label);
                        break;
                    case RegexElementLabel.UNIT_SET:
                        GenerateUnitSet(root, sc, context, exit_label);
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        GenerateUnitSpan(root, sc, context, exit_label);
                        break;
                    default:
                        break;
                }

                // TODO(NGRA): Integrate is_find candidate search and continuation retries.
                // Leaf emission currently matches only at the active cursor.
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
                    if (set.Value.Length != 2) {
                        throw new InvalidOperationException("A charset range requires two endpoints.");
                    }
                    return $"{accept}({remain_span}[0] >= {FormatCharLiteral(set.Value[0])} && {remain_span}[0] <= {FormatCharLiteral(set.Value[1])})";
                case RegexCharSetType.Set:
                    switch (set.Value)
                    {
                        case "s":
                            return $"{accept}char.IsWhiteSpace({remain_span}[0])";
                        case "w":
                            return $"{accept}{Text_Regex}.{nameof(TextRegex.IsCharWord)}({remain_span}[0])";
                        case "d":
                            return $"{accept}char.IsDigit({remain_span}[0])";
                        default:
                            break;
                    }
                    goto default;
                case RegexCharSetType.Any:
                    return set.Accept ? "true" : "false";
                default:
                    throw new NotSupportedException($"Unsupported charset rule: {set.Type} ({set.Value}).");
            }
        }

            #endregion

        #region UNIT direct matching

        private static void GenerateUnit(
                            RegexAstNode value,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel exit
        )
        {
            string literal = value.Value;
            if (literal.Length != 1) {
                throw new InvalidOperationException(
                    $"UNIT requires one decoded character, but received {literal.Length}.");
            }

            string remain_span = context.RemainSpan;
            _ = sc.AppendLine($"if ({remain_span}.IsEmpty || {remain_span}[0] != {FormatCharLiteral(literal[0])})")
                  .AppendLine("{")
                  .AppendLine($"goto {exit.GetLabelName()};")
                  .AppendLine("}")
                  .AppendLine($"{remain_span} = {remain_span}.Slice(1);");
        }

        private static void GenerateUnitSet(
                            RegexAstNode value,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel exit
        )
        {
            RegexCharSetPayload payload = value.PayloadAs<RegexCharSetPayload>();
            string[] rules = new string[payload.Set.Count];
            for (int i = 0; i < rules.Length; i++) {
                rules[i] = GenerateCharSetExpression(payload.Set[i]);
            }

            // Combine rule-level complements before applying the enclosing charset complement.
            // An empty union accepts nothing; its complement accepts any available character.
            string predicate = rules.Length == 0 ? "false" : string.Join(" || ", rules);
            string mismatch = payload.Accept ? $"!({predicate})" : $"({predicate})";
            string remain_span = context.RemainSpan;
            _ = sc.AppendLine($"if ({remain_span}.IsEmpty || {mismatch})")
                  .AppendLine("{")
                  .AppendLine($"goto {exit.GetLabelName()};")
                  .AppendLine("}")
                  .AppendLine($"{remain_span} = {remain_span}.Slice(1);");
        }

        private static void GenerateUnitSpan(
                            RegexAstNode value,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel exit
        )
        {
            string literal = value.Value;
            if (literal.Length == 0) {
                throw new InvalidOperationException("UNIT_SPAN requires a non-empty literal.");
            }

            // StartsWith also rejects input shorter than the decoded literal.
            string remain_span = context.RemainSpan;
            _ = sc.AppendLine($"if (!{remain_span}.{nameof(MemoryExtensions.StartsWith)}({FormatStringLiteral(literal)}))")
                  .AppendLine("{")
                  .AppendLine($"goto {exit.GetLabelName()};")
                  .AppendLine("}")
                  .AppendLine($"{remain_span} = {remain_span}.Slice({literal.Length});");
        }

            #endregion

        #region UNIT count matching

        private const int flattened_unit_count_threshold = 32;

        private static void GenerateUnitCount(
                            RegexAstNode value,
                            int min,
                            int max,
                            bool is_greedy,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel exit
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

            // TODO(NGRA-NAMING): Allocate required_rest, required_matched, required_chunk_index,
            // and additional_count through the exclusive-name pool, including loop-local names.
            int scratch_scope_index = context.RequestScratchScopeIndex();
            string exit_label_node = exit.GetLabelName();
            string required_rest = $"__required_rest_{scratch_scope_index}";
            string required_matched = $"__required_matched_{scratch_scope_index}";
            string required_chunk_index = $"__required_chunk_index_{scratch_scope_index}";
            string additional_count = $"__additional_count_{scratch_scope_index}";

            // TODO(NGRA): Replace this local COUNT exit with the inherited failure ExitLabel.
            // The temporary goto preserves the former local exit but still lets siblings execute.

            if (min <= flattened_unit_count_threshold)
            {
                if (min > 0)
                {
                    string required_prefix = new(expected, min);
                    _ = sc.AppendLine($"if (!{remain_span}.{nameof(MemoryExtensions.StartsWith)}({FormatStringLiteral(required_prefix)}))")
                          .AppendLine("{")
                          .AppendLine($"goto {exit_label_node};")
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
                      .AppendLine($"goto {exit_label_node};")
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
                      .AppendLine($"goto {exit_label_node};")
                      .AppendLine("}")
                      .AppendLine($"{remain_span} = {remain_span}.Slice({min});");
            }

            int additional_limit = max < 0 ? -1 : max - min;
            if (is_greedy && additional_limit != 0)
            {
                _ = additional_limit < 0 ?
                    sc.AppendLine($"while (!{remain_span}.IsEmpty && {remain_span}[0] == {expected_literal})")
                      .AppendLine("{")
                      .AppendLine($"{remain_span} = {remain_span}.Slice(1);")
                      .AppendLine("}") :
                    sc.AppendLine($"for (int {additional_count} = 0; {additional_count} < {additional_limit} && !{remain_span}.IsEmpty && {remain_span}[0] == {expected_literal}; {additional_count}++)")
                      .AppendLine("{")
                      .AppendLine($"{remain_span} = {remain_span}.Slice(1);")
                      .AppendLine("}");
            } else if (!is_greedy && additional_limit != 0)
            {
                // Lazy COUNT deliberately stops at Min. Its optional tail is consumed only when
                // a future continuation attempt fails and requests another repetition.
                _ = sc.AppendLine("// TODO(NGRA): Retry the lazy COUNT tail from its continuation.");
            }
        }

        private static void GenerateUnitSetCount(
                            RegexAstNode value,
                            int min,
                            int max,
                            bool is_greedy,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel exit_label
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

            // TODO(NGRA-NAMING): Allocate required_matched and additional_count through the
            // exclusive-name pool. Reserve required_rest/required_chunk_index if emitted later.
            int scratch_scope_index = context.RequestScratchScopeIndex();
            string required_rest = $"__required_rest_{scratch_scope_index}";
            string required_matched = $"__required_matched_{scratch_scope_index}";
            string required_chunk_index = $"__required_chunk_index_{scratch_scope_index}";
            string additional_count = $"__additional_count_{scratch_scope_index}";


            // TODO(NGRA): Route a failed minimum check through the inherited failure ExitLabel.
            // The break below only stops charset scanning and must remain local to that loop.
            bool accept = payload.Accept;
            RegexCharSet _set = charset[0];
            _ = sc.AppendLine($"{ROS_char} {required_matched} = {remain_span}")
                  .AppendLine($"int {additional_count} = 0;")
                  .AppendLine($"for(; {additional_count} < {max}; {additional_count} ++)")
                  .AppendLine("{");
            if (charset.Count == 1)
            {
                // single charset rule
                _ = sc.AppendLine($"if({(accept ? string.Empty : "!(")} {GenerateCharSetExpression(_set)})");
            } else
            {
                // multi charset rule
                _ = sc.AppendLine($"if({(accept ? string.Empty : "!(")} {GenerateCharSetExpression(_set)} ||");
                for (int i = 1; i < charset.Count - 1; i++)
                {
                    _set = charset[i];
                    _ = sc.AppendLine($"    {GenerateCharSetExpression(_set)} ||");
                }
                _set = charset[^1];
                _ = sc.AppendLine($"    {GenerateCharSetExpression(_set)} ){(accept ? string.Empty : ')')}");
            }
            _ = sc .AppendLine("{")
                   .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                   .AppendLine("}")
                   .AppendLine("else")
                   .AppendLine("{")
                   .AppendLine($@"break;")
                   .AppendLine("}")
                   .AppendLine("}")
                   .AppendLine($"if({additional_count} < {min})")
                   .AppendLine("{")
                   .AppendLine("}")
                   .AppendLine("else")
                   .AppendLine("{")
                   .AppendLine("}");
        }

        private static void GenerateUnitSpanCount(
                            RegexAstNode value,
                            int min,
                            int max,
                            bool is_greedy,
                            [NotNull] in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel exit
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

            // TODO(NGRA-NAMING): Allocate required_rest, required_matched, required_chunk_index,
            // required_count, and additional_count through the exclusive-name pool. Flattened
            // sibling COUNT nodes share the declaration space even after their exit labels.
            int scratch_scope_index = context.RequestScratchScopeIndex();
            string exit_label_node = exit.GetLabelName();
            string required_rest = $"__required_rest_{scratch_scope_index}";
            string required_matched = $"__required_matched_{scratch_scope_index}";
            string required_chunk_index = $"__required_chunk_index_{scratch_scope_index}";
            string required_count = $"__required_count_{scratch_scope_index}";
            string additional_count = $"__additional_count_{scratch_scope_index}";
            string starts_with = $"{nameof(MemoryExtensions.StartsWith)}";

            // TODO(NGRA): Replace this local COUNT exit with the inherited failure ExitLabel.
            // The temporary goto preserves the former local exit but still lets siblings execute.

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
                      .AppendLine($"goto {exit_label_node};")
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
                          .AppendLine($"goto {exit_label_node};")
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
                      .AppendLine($"goto {exit_label_node};")
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
        }

            #endregion

        #region UNIT find matching

        // TODO(NGRA): Add UNIT, UNIT_SET, and UNIT_SPAN candidate search emitters here.
        // Wire is_find dispatch and continuation retries when FIND matching is implemented.

        #endregion

        #region Operator generation

        private static void GenerateAlt(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context
        )
        {
            RegexAstNode current = node.Right;

            // TODO(NGRA-NAMING): Reserve exclusive names for future ALT cursor/capture snapshots;
            // their lifetime extends through retry labels, not just the candidate's emitted block.
            ExitLabel exit_label = context.RequestExitLabel(ExitMode.Goto);

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
                            string literal = FormatStringLiteral(code);
                            _ = sc.AppendLine(@$"if({context.RemainSpan}.{MemoryExtensions.StartsWith}({literal}))")
                                  .AppendLine("{")
                                  .AppendLine($@"{context.RemainSpan} = {context.RemainSpan}.Slice({code.Length});")
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

            // TODO(NGRA-NAMING): Reserve exclusive names for generated group snapshots and capture
            // temporaries. A capture's user-provided name is not a unique generated local name.

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
                            in SourceGenContext context,
                            in ExitLabel exit_label = default
        )
        {
            if (!RegexAstNode.IsNullOrEmpty(node.Left)) {
                GenerateMethodSource(node.Left, sc, func_list, context, exit_label);
            }
            if (!RegexAstNode.IsNullOrEmpty(node.Right)) {
                GenerateMethodSource(node.Right, sc, func_list, context, exit_label);
            }
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

            // TODO(NGRA-NAMING): Reserve exclusive names for future repetition/rollback state
            // across complex COUNT retries; recursive children must use the same method pool.
            ExitLabel exit_label = context.RequestExitLabel(ExitMode.Goto);
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
                        GenerateUnitCount(value, min, max, is_greedy, sc, context, exit_label);
                        break;
                    case RegexElementLabel.UNIT_SET:
                        GenerateUnitSetCount(value, min, max, is_greedy, sc, context, exit_label);
                        break;
                    case RegexElementLabel.UNIT_SPAN:
                        GenerateUnitSpanCount(value, min, max, is_greedy, sc, context, exit_label);
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
