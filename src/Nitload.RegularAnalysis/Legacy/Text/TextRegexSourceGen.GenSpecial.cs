// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenSpecial"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.RegularAnalysis.Abstraction;

namespace Nitload.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        private static bool TryGenerateSpecialSource(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context,
                            in ExitLabel local_fail,
                            ref ExitLabel backtrack
        )
        {
            // A special rule owns the entire matched subtree. Returning false leaves emission
            // to the general path; returning true prevents that subtree from being emitted twice.
            return node.Label switch
            {
                RegexElementLabel.CONCAT => TryGenerateFindConcat(node, sc, func_list, context,
                                                                  local_fail, ref backtrack),
                _ => false,
            };
        }

        #region ALT candidate rules

        private static bool TryGetSimpleAltNode(
                            RegexAstNode node,
                            out RegexAstNode simple
        )
        {
            simple = node;
            while (simple.Label == RegexElementLabel.CONCAT)
            {
                bool left_empty = RegexAstNode.IsNullOrEmpty(simple.Left);
                bool right_empty = RegexAstNode.IsNullOrEmpty(simple.Right);
                if (left_empty == right_empty) {
                    return false;
                }
                simple = left_empty ? simple.Right : simple.Left;
            }

            // Inspect transparent wrappers without changing ownership or the candidate Next link.
            // COUNT and GROUP are never peeled: repetition and capture semantics must survive.
            return simple.Label is RegexElementLabel.UNIT or RegexElementLabel.UNIT_SET or
                                   RegexElementLabel.UNIT_SPAN;
        }

        private static void GenerateAltCandidate(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context,
                            in ExitLabel success
        )
        {
            if (TryGetSimpleAltNode(node, out RegexAstNode simple)) {
                node = simple;
            }

            switch (node.Label)
            {
                case RegexElementLabel.UNIT:
                    GenerateAltUnit(node, sc, context, success);
                    break;
                case RegexElementLabel.UNIT_SET:
                    GenerateAltUnitSet(node, sc, context, success);
                    break;
                case RegexElementLabel.UNIT_SPAN:
                    GenerateAltUnitSpan(node, sc, context, success);
                    break;
                case RegexElementLabel.CONCAT when RegexAstNode.IsNullOrEmpty(node.Left) &&
                                                   RegexAstNode.IsNullOrEmpty(node.Right):
                case RegexElementLabel.EPSILON:

                    // An empty expression succeeds without consuming input.
                    _ = sc.AppendLine($"goto {success.GetLabelName()};");
                    break;
                case RegexElementLabel.CONCAT:
                case RegexElementLabel.COUNT:
                case RegexElementLabel.GROUP_BEGIN:
                case RegexElementLabel.ALT:
                    GenerateAltOperator(node, sc, func_list, context, success);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported ALT candidate: {node.Label}.");
            }
        }

        private static void GenerateAltUnit(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel success
        )
        {
            string literal = node.Value;
            if (literal.Length != 1) {
                throw new InvalidOperationException("ALT UNIT requires one decoded character.");
            }

            _ = sc.AppendLine($"if (!{context.RemainSpan}.IsEmpty && {context.RemainSpan}[0] == {FormatCharLiteral(literal[0])})")
                  .AppendLine("{")
                  .AppendLine($"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                  .AppendLine($"goto {success.GetLabelName()};")
                  .AppendLine("}");
        }

        private static void GenerateAltUnitSet(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel success
        )
        {
            RegexCharSetPayload payload = node.PayloadAs<RegexCharSetPayload>();
            _ = sc.AppendLine($"if (!{context.RemainSpan}.IsEmpty &&")
                  .AppendLine(payload.Accept ? "    (" : "    !(");
            if (payload.Set.Count == 0) {
                _ = sc.AppendLine("        false");
            }
            for (int i = 0; i < payload.Set.Count; i++)
            {
                string suffix = i + 1 < payload.Set.Count ? " ||" : string.Empty;
                _ = sc.AppendLine($"        {GenerateCharSetExpression(payload.Set[i])}{suffix}");
            }
            _ = sc.AppendLine("    ))")
                  .AppendLine("{")
                  .AppendLine($"{context.RemainSpan} = {context.RemainSpan}.Slice(1);")
                  .AppendLine($"goto {success.GetLabelName()};")
                  .AppendLine("}");
        }

        private static void GenerateAltUnitSpan(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in SourceGenContext context,
                            in ExitLabel success
        )
        {
            string literal = node.Value;
            if (literal.Length == 0) {
                throw new InvalidOperationException("ALT UNIT_SPAN requires a non-empty literal.");
            }
            string predicate = $"{context.RemainSpan}.{nameof(MemoryExtensions.StartsWith)}({FormatStringLiteral(literal)})";
            _ = sc.AppendLine($"if ({predicate})")
                  .AppendLine("{")
                  .AppendLine($"{context.RemainSpan} = {context.RemainSpan}.Slice({literal.Length});")
                  .AppendLine($"goto {success.GetLabelName()};")
                  .AppendLine("}");
        }

        private static void GenerateAltOperator(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context,
                            in ExitLabel success
        )
        {
            ExitLabel failure = context.RequestExitLabel(ExitKind.AltNext);
            string entry = $"__alt_entry_{context.RequestScratchScopeIndex()}";
            _ = sc.AppendLine($"{ROS_char} {entry} = {context.RemainSpan};");
            // Keep branch-local retries isolated from both siblings and the caller's path.
            ExitLabel branch_backtrack = failure;
            GenerateMethodSource(node, sc, func_list, context, failure, ref branch_backtrack);
            _ = sc.AppendLine($"goto {success.GetLabelName()};")
                  .AppendLine($"{failure.GetLabelName()}:")
                  .AppendLine($"{context.RemainSpan} = {entry};");

            // TODO(NGRA): Restore capture state once GROUP emission supports captures.
            // Retrying an already successful candidate after a later continuation fails is separate.
        }

            #endregion

        #region FIND concatenation

        private static bool TryGenerateFindConcat(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context,
                            in ExitLabel local_fail,
                            ref ExitLabel backtrack
        )
        {
            RegexAstNode left = node.Left;
            RegexAstNode right = node.Right;
            if (RegexAstNode.IsNullOrEmpty(left)) {
                return false;
            }

            // Existing FIND dispatch scaffold; candidate search and retries are not implemented.
            // TODO(NGRA): Use the COUNT payload's IsGreedy flag to order FIND candidates.
            if (RegexAstNode.IsDotStar(left) && !RegexAstNode.IsNullOrEmpty(right))
            {
                // TODO(NGRA): Emit the unnamed FIND candidate loop and retry the right subtree
                // until it succeeds or no anchor candidate remains.
                GenerateMethodSource(right, sc, func_list, context, local_fail, ref backtrack, true);
                return true;
            }

            if (IsNamedFind(node))
            {
                if (!RegexAstNode.IsNullOrEmpty(right))
                {
                    // TODO(NGRA): Emit named FIND retry and capture the text preceding the
                    // successful anchor candidate.
                    GenerateMethodSource(right, sc, func_list, context, local_fail, ref backtrack, true);
                } else
                {
                    // TODO(NGRA): Capture the remaining input for a terminal named dot-star.
                }
                return true;
            }

            return false;
        }

        private static bool IsNamedFind(
                            RegexAstNode node
        )
        {
            // (?<name>.*)abc

            if (RegexAstNode.IsNullOrEmpty(node) || node.Label != RegexElementLabel.CONCAT)
            {
                return false;
            }
            RegexAstNode group = node.Left;
            if (RegexAstNode.IsNullOrEmpty(group) ||
                group.Payload is not RegexGroupPayload { IsCapturing: true } config ||
                string.IsNullOrEmpty(config.Name)) {
                return false;
            }

            bool left_empty = RegexAstNode.IsNullOrEmpty(group.Left);
            bool right_empty = RegexAstNode.IsNullOrEmpty(group.Right);
            return left_empty != right_empty &&
                   RegexAstNode.IsDotStar(left_empty ? group.Right : group.Left);
        }

        #endregion

        #region Repeated capture rules

        // TODO(NGRA): Recognize and implement repeated captures such as (?<name>.*){m,n}
        // and (?<name>.*a){m,n}. Keep these distinct from (?<name>.*)a FIND concatenation.
        // A later repetition may need to reopen an earlier repetition's inner search, restoring
        // its cursor, candidate position, and capture state. Do not claim these subtrees in
        // TryGenerateSpecialSource until the complete retry/rollback strategy is implemented.
        // An escaped star is a literal and must not be classified as dot-star repetition.

        #endregion
    }
}
