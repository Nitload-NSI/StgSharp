using System.Collections.Immutable;
using System.Linq;

namespace StgSharp.UserInterface.Generator;

internal static class PanelStateIncrementalGeneratorStateSymbols
{
    public static PanelStateSymbolPartAnalysis Analyze(
                       PanelStateIncrementalGeneratorTypeTarget target
    )
    {
        ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> members = target.MarkedMembers
            .Where(static member => PanelStateIncrementalGeneratorKnownAttributes.IsStateSymbolAttribute(member.AttributeMetadataName))
            .ToImmutableArray();

        return new PanelStateSymbolPartAnalysis(
            target.TypeDisplayName,
            members);
    }
}