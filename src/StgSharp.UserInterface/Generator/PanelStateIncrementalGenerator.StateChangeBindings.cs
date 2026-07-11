// -----------------------------------------------------------------------------
// file="PanelStateIncrementalGenerator.StateChangeBindings"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Linq;

namespace StgSharp.UserInterface.Generator
{
    internal static class PanelStateIncrementalGeneratorStateChangeBindings
    {

        public static StateChangeBindingPartAnalysis Analyze(
                                                     PanelStateIncrementalGeneratorTypeTarget target
        )
        {
            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> members = target.MarkedMembers
                                                                                       .Where(static member => member.AttributeMetadataName == PanelStateIncrementalGeneratorKnownAttributes.StateChangeBindingAttribute)
                                                                                       .ToImmutableArray();

            return new StateChangeBindingPartAnalysis(
                target.TypeDisplayName,
                members);
        }

    }
}