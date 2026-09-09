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
        ///   a scratch variable. Scratch indices remain unique throughout the generated method;
        ///   flattened regions cannot release their names for reuse by later nodes.
        /// </remarks>
        internal sealed class SourceGenContext
        {

            public const string __input_span = "input";

            public const string _remain_span = "__remain";

            private int _max_exit_lable;

            private int _scratch_scope_count;

            public string RemainSpan { get; } = _remain_span;

            public string FirstOfRemain { get; } = $@"{_remain_span}[0]";

            public string InputString { get; } = __input_span;

            public SequenceEmitter<string> DeclareGlobalFields()
            {
                SequenceEmitter<string> sc = new();

                return sc.AppendLine($@"{ROS_char} {RemainSpan};");
            }

            public ExitLabel RequestExitLabel(
                             ExitMode exitMode
            )
            {
                int index = _max_exit_lable;
                _max_exit_lable++;
                return new(exitMode, index);
            }

            public int RequestScratchScopeIndex()
            {
                // TODO(NGRA-NAMING): Integrate scratch names with the method-wide exclusive-name
                // allocator. Until then, reserve each suffix permanently for this method.
                int index = _scratch_scope_count;
                _scratch_scope_count++;
                return index;
            }

        }

    }
}
