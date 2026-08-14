// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenContext"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.Text;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
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
        private sealed class SourceGenContext
        {

            /// <summary>
            ///   Name of the match method parameter holding the input sequence.
            /// </summary>
            public const string _inputSpan = "text";

            /// <summary>
            ///   Result of the instruction emitted immediately before the current one.
            /// </summary>
            public const string _stepResult = "__ok";

            private readonly Dictionary<string, int> _groupIndex = [];
            private readonly HashSet<int> _observedLines = [];

            private int _anonymousGroup;

            private int _count_idx = 0;
            private int _scratchIndex;
            private readonly SortedSet<int> _lineSlots = [];
            private readonly SortedSet<int> _regionSlots = [];

            public string RemainingSpan { get; } = "__remaining_span";

            public string InputSpan { get; } = _inputSpan;

            public string StepResult { get; } = _stepResult;

            // region id -> entry count from first to last occurrence, filled once per pattern
            public int[] RegionExtent { get; set; } = [];

            public string[] BeginRegion()
            {
                return [
                    "do",
                    "{"
                    ];
            }

            public string[] CloseRegion()
            {
                return [
                    "} while(true);"
                    ];
            }

            /// <summary>
            ///   Emits declarations for every slot requested during body generation.
            /// </summary>
            /// <remarks>
            ///   Must be called after the method body has been generated, but the produced lines
            ///   have to be placed before it.
            /// </remarks>
            public SequenceEmitter<string> EmitDeclarationsTo(
                                           SequenceEmitter<string> emitter
            )
            {
                if (emitter is null) {
                    return null!;
                }

                _ = emitter.AppendLine($@"{ROS_char} {RemainingSpan} = {_inputSpan};")
                           .AppendLine($@"bool {_stepResult} = true;");

                foreach (int region in _regionSlots) {
                    _ = emitter.AppendLine($@"int {RegionAnchor(region)} = 0;")
                               .AppendLine($@"bool {RegionResult(region)} = false;");
                }

                foreach (int line in _lineSlots) {
                    _ = emitter.AppendLine($@"bool {LineResult(line)} = false;");
                }

                foreach (int group in _groupIndex.Values) {
                    _ = emitter.AppendLine($@"int __g{group}_begin = -1;")
                               .AppendLine($@"int __g{group}_end = -1;");
                }

                return emitter;
            }

            public string GetVarGroupBegin(
                          string name
            )
            {
                return $"__g{ResolveGroup(name)}_begin";
            }

            public string GetVarGroupEnd(
                          string name
            )
            {
                return $"__g{ResolveGroup(name)}_end";
            }

            /// <summary>
            ///   Indicates whether the instruction at line <paramref name="line" /> has to persist
            ///   its result into a line slot.
            /// </summary>
            public bool IsLineObserved(
                        int line
            )
            {
                return _observedLines.Contains(line);
            }

            /// <summary>
            ///   Name of the slot holding the match result of the instruction at line <paramref
            ///   name="line" />. Only lines actually referenced by an <see
            ///   cref="RegexIRPrefixMask.Observe_Line" /> prefix get a slot.
            /// </summary>
            public string LineResult(
                          int line
            )
            {
                _ = _lineSlots.Add(line);
                return $"__l{line}_ok";
            }

            public string MakeRemainOffset(
                          string offset_name
            )
            {
                return @$"{RemainingSpan} = {RemainingSpan}[{offset_name}..];";
            }

            public string MakeRemainOffset(
                          int offset
            )
            {
                return @$"{RemainingSpan} = {RemainingSpan}[{offset}..];";
            }

            /// <summary>
            ///   Records that some prefix observes the result of the instruction at line <paramref
            ///   name="line" />.
            /// </summary>
            public void MarkLineObserved(
                        int line
            )
            {
                _ = _observedLines.Add(line);
            }

            /// <summary>
            ///   Allocates a fresh block scoped scratch variable name.
            /// </summary>
            /// <param name="tag">
            ///   Short role marker, for example <c> n </c> for a counter or <c> ch </c> for a
            ///   character.
            /// </param>
            public string NewScratch(
                          string tag
            )
            {
                return $"__{tag}{_scratchIndex++}";
            }

            /// <summary>
            ///   Name of the slot holding the cursor value captured when region <paramref
            ///   name="region" /> was entered.
            /// </summary>
            public string RegionAnchor(
                          int region
            )
            {
                _ = _regionSlots.Add(region);
                return $"__r{region}_begin";
            }

            /// <summary>
            ///   Name of the slot holding the match result of region <paramref name="region" />.
            /// </summary>
            public string RegionResult(
                          int region
            )
            {
                _ = _regionSlots.Add(region);
                return $"__r{region}_ok";
            }

            public string RequireNewCounter()
            {
                string counter_name = $"__count{_count_idx}";
                _count_idx++;
                return counter_name;
            }

            private int ResolveGroup(
                        string name
            )
            {
                string key = string.IsNullOrEmpty(name) ? $"#{_anonymousGroup++}" : name;
                if (_groupIndex.TryGetValue(key, out int index)) {
                    return index;
                }
                index = _groupIndex.Count;
                _groupIndex[key] = index;
                return index;
            }

        }

    }
}
