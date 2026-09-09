// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenSpecial"
// Project: Nitload
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
                            in ExitLabel exit_label
        )
        {
            // A special rule owns the entire matched subtree. Returning false leaves emission
            // to the general path; returning true prevents that subtree from being emitted twice.
            return node.Label switch
            {
                RegexElementLabel.CONCAT => TryGenerateFindConcat(node, sc, func_list, context, exit_label),
                _ => false,
            };
        }

        #region FIND concatenation

        private static bool TryGenerateFindConcat(
                            RegexAstNode node,
                            in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list,
                            in SourceGenContext context,
                            in ExitLabel exit_label
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
                GenerateMethodSource(right, sc, func_list, context, exit_label, true);
                return true;
            }

            if (IsNamedFind(node))
            {
                if (!RegexAstNode.IsNullOrEmpty(right))
                {
                    // TODO(NGRA): Emit named FIND retry and capture the text preceding the
                    // successful anchor candidate.
                    GenerateMethodSource(right, sc, func_list, context, exit_label, true);
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

            if (RegexAstNode.IsNullOrEmpty(node) || node.Label != RegexElementLabel.CONCAT) {
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
