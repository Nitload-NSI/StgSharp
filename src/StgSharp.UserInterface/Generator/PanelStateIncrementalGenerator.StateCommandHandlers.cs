// -----------------------------------------------------------------------------
// file="PanelStateIncrementalGenerator.StateCommandHandlers"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace StgSharp.UserInterface.Generator
{
    internal static class PanelStateIncrementalGeneratorStateCommandHandlers
    {

        public static StateChangeCommandHandlerPartAnalysis Analyze(
                                                            PanelStateIncrementalGeneratorTypeTarget target,
                                                            PanelStateSymbolPartAnalysis stateSymbols,
                                                            StateChangeBindingPartAnalysis stateChangeBindings
        )
        {
            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> members = MergeMembers(
                stateChangeBindings.Members,
                stateSymbols.Members);

            return new StateChangeCommandHandlerPartAnalysis(
                target.TypeDisplayName,
                members);
        }

        private static void AddMembers(
                            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember>.Builder builder,
                            HashSet<string> seen,
                            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> members
        )
        {
            foreach (PanelStateIncrementalGeneratorMarkedMember member in members)
            {
                string key = member.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!seen.Add(key))
                {
                    continue;
                }

                builder.Add(member);
            }
        }

        private static ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> MergeMembers(
                                                                                  ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> primaryMembers,
                                                                                  ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> secondaryMembers
        )
        {
            if (primaryMembers.IsDefaultOrEmpty && secondaryMembers.IsDefaultOrEmpty) {
                return ImmutableArray<PanelStateIncrementalGeneratorMarkedMember>.Empty;
            }

            HashSet<string> seen = new(StringComparer.Ordinal);
            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember>.Builder builder =
                ImmutableArray.CreateBuilder<PanelStateIncrementalGeneratorMarkedMember>();

            AddMembers(builder, seen, primaryMembers);
            AddMembers(builder, seen, secondaryMembers);

            return builder.ToImmutable();
        }

    }
}