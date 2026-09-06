// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenContext"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.Text;
using Nitload.Common.PipeLine;
using Nitload.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        /// <summary>
        ///   Tracks variable naming and declaration demand while generating one match method.
        /// </summary>
        /// <remarks>
        ///   Method level state is limited to the active cursor and the result of the most recent
        ///   instruction. Everything else is either a region slot, a line slot, a capture slot, or
        ///   a block scoped scratch variable. Scratch indices remain unique while rented and are
        ///   recycled after their generated lexical scope has closed.
        /// </remarks>
        internal sealed class SourceGenContext
        {

            public const string __input_span = "input";

            public const string _remain_span = "__remain";

            private int _exit_alt_count;

            private int _scratch_scope_count;

            private readonly Stack<int> _free_scratch_scope_indices = [];

            public string RemainSpan { get; } = _remain_span;

            public string FirstOfRemain { get; } = $@"{_remain_span}[0]";

            public string InputString { get; } = __input_span;

            public SequenceEmitter<string> DeclareGlobalFields()
            {
                SequenceEmitter<string> sc = new();

                return sc.AppendLine($@"{ROS_char} {RemainSpan};");
            }

            public string RequestExitLabel()
            {
                string code = $@"_exit_alt_{_exit_alt_count}";
                _exit_alt_count++;
                return code;
            }

            public int RentScratchScopeIndex()
            {
                if (_free_scratch_scope_indices.TryPop(out int recycled_index)) {
                    return recycled_index;
                }

                int index = _scratch_scope_count;
                _scratch_scope_count++;
                return index;
            }

            public void ReturnScratchScopeIndex(
                        int index
            )
            {
                if ((uint)index >= (uint)_scratch_scope_count) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (_free_scratch_scope_indices.Contains(index)) {
                    throw new InvalidOperationException($"Scratch scope {index} was returned twice.");
                }

                _free_scratch_scope_indices.Push(index);
            }

        }

    }
}
