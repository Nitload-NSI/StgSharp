using System.Collections.Immutable;
using System.Linq;

namespace StgSharp.UserInterface.Generator;

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