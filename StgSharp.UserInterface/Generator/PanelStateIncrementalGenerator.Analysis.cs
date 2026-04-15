using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace StgSharp.UserInterface.Generator;

internal static class PanelStateIncrementalGeneratorAnalysis
{
    public static PanelStateIncrementalGeneratorOutput Analyze(
                       PanelStateIncrementalGeneratorInput input
    )
    {
        if (input.TypeTargets.IsDefaultOrEmpty)
        {
            return new PanelStateIncrementalGeneratorOutput(
                input.Compilation,
                ImmutableArray<PanelStateIncrementalGeneratorTypeAnalysis>.Empty);
        }

        HashSet<string> emittedTypes = new(StringComparer.Ordinal);
        ImmutableArray<PanelStateIncrementalGeneratorTypeAnalysis>.Builder builder =
            ImmutableArray.CreateBuilder<PanelStateIncrementalGeneratorTypeAnalysis>();

        foreach (PanelStateIncrementalGeneratorTypeTarget target in input.TypeTargets
                                                                     .OrderBy(static item => item.TypeDisplayName, StringComparer.Ordinal))
        {
            if (!emittedTypes.Add(target.TypeDisplayName))
            {
                continue;
            }

            PanelStateSymbolPartAnalysis stateSymbols = PanelStateIncrementalGeneratorStateSymbols.Analyze(target);
            StateChangeBindingPartAnalysis stateChangeBindings = PanelStateIncrementalGeneratorStateChangeBindings.Analyze(target);

            builder.Add(
                new PanelStateIncrementalGeneratorTypeAnalysis(
                    target,
                    stateSymbols,
                    stateChangeBindings,
                    PanelStateIncrementalGeneratorStateCommandHandlers.Analyze(
                        target,
                        stateSymbols,
                        stateChangeBindings)));
        }

        return new PanelStateIncrementalGeneratorOutput(
            input.Compilation,
            builder.ToImmutable());
    }
}