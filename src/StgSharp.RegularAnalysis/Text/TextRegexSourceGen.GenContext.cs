// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenContext"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.Text;
using StgSharp.PipeLine;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        /// <summary>
        ///   Tracks variable naming and declaration demand while generating one match method.
        /// </summary>
        /// <remarks>
        ///   Method level state is limited to the active cursor and the result of the most recent
        ///   instruction. Everything else is either a region slot, a line slot, a capture slot, or
        ///   a block scoped scratch variable. Scratch names are never reused, so a nested block can
        ///   always declare its own without colliding with an enclosing one.
        /// </remarks>
        internal sealed class SourceGenContext
        {

            public const string __input_span = "input";

            public const string _remain_span = "__remain";

            private int _exit_alt_count;

            public string RemainSpan { get; } = _remain_span;

            public string FirstOfRemain { get; } = $@"{_remain_span}[0]";

            public string InputString { get; } = __input_span;

            public SequenceEmitter<string> DeclareGlobalFields()
            {
                SequenceEmitter<string> sc = new();

                return sc.AppendLine($@"{ROS_char} {RemainSpan};");
            }

            public string RequestExitAlt()
            {
                string code = $@"_exit_alt_{_exit_alt_count}";
                _exit_alt_count++;
                return code;
            }

        }

    }
}
