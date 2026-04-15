using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace StgSharp.UserInterface.Generator
{
    internal static class PanelStateIncrementalGeneratorKnownAttributes
    {

        public const string PanelStateNodeAttribute = "StgSharp.UserInterface.PanelStateNodeAttribute";

        public const string StateChangeBindingAttribute = "StgSharp.UserInterface.StateChangeBindingAttribute";

        public static bool IsKnown(
                           string? metadataName
        )
        {
            return IsStateSymbolAttribute(metadataName) || metadataName == StateChangeBindingAttribute;
        }

        public static bool IsStateSymbolAttribute(
                           string? metadataName
        )
        {
            return metadataName == PanelStateNodeAttribute;
        }

    }

    internal sealed record PanelStateIncrementalGeneratorMarkedMember(
                           ISymbol Symbol,
                           string AttributeMetadataName,
                           string SymbolName,
                           string SymbolTypeName
    );

    internal sealed record PanelStateIncrementalGeneratorTypeTarget(
                           INamedTypeSymbol Type,
                           string TypeDisplayName,
                           ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> MarkedMembers
    );

    internal readonly record struct PanelStateIncrementalGeneratorInput(
                                    Compilation Compilation,
                                    ImmutableArray<PanelStateIncrementalGeneratorTypeTarget> TypeTargets
    );

    internal sealed record PanelStateIncrementalGeneratorTypeAnalysis(
                           PanelStateIncrementalGeneratorTypeTarget Target,
                           PanelStateSymbolPartAnalysis StateSymbols,
                           StateChangeBindingPartAnalysis StateChangeBindings,
                           StateChangeCommandHandlerPartAnalysis StateChangeCommandHandlers
    );

    internal readonly record struct PanelStateIncrementalGeneratorOutput(
                                    Compilation Compilation,
                                    ImmutableArray<PanelStateIncrementalGeneratorTypeAnalysis> TypeAnalyses
    );

    internal sealed record PanelStateSymbolPartAnalysis(
                           string OwnerTypeName,
                           ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> Members
    );

    internal sealed record StateChangeBindingPartAnalysis(
                           string OwnerTypeName,
                           ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> Members
    );

    internal sealed record StateChangeCommandHandlerPartAnalysis(
                           string OwnerTypeName,
                           ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> Members
    );
}
