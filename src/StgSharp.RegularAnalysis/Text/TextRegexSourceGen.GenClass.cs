// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using StgSharp.RegularAnalysis.Abstraction;
using System;
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
                             bool is_find = false

        )
        {
            RegexAstNode root = ast;
            switch (root.Label & (RegexElementLabel.VAST_OPERATOR))
            {
                case RegexElementLabel.CONCAT:
                    GenerateConcat(root, sc, func_list);
                    break;
                case RegexElementLabel.COUNT:
                    break;
                case RegexElementLabel.GROUP_BEGIN:
                    break;
                case RegexElementLabel.ALT:
                    break;
            }
        }

        #region supporting method

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

        #region verb generate

        private static void GenerateAlt(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list
        ) { }

        private static void GenerateConcat(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list
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
                    GenerateMethodSource(right, sc, func_list, true);
                } else if (IsNamedFind(node))
                {
                    if (right is not null)
                    {
                        // TODO named find
                        GenerateMethodSource(right, sc, func_list, true);
                    } else
                    {
                        // TODO pack rest of string to group
                    }
                } else
                {
                    GenerateMethodSource(left, sc, func_list);
                    if (right is not null) {
                        GenerateMethodSource(right, sc, func_list);
                    }
                }
            }
            if (right is not null) {
                GenerateMethodSource(right, sc, func_list);
            }
        }

        private static void GenerateCount(
                            RegexAstNode node,
                            [NotNull] in SequenceEmitter<string> sc,
                            in List<SequenceEmitter<string>> func_list
        )
        {
            RegexAstNode value = node.Left ?? node.Right;
            if ((value.Label & RegexElementLabel.SEQUENCE) == 0)
            {
                GenerateMethodSource(value, sc, func_list);
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
