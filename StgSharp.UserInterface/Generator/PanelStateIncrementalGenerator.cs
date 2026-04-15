using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace StgSharp.UserInterface.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class PanelStateIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<PanelStateIncrementalGeneratorTypeTarget> typeTargets = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => PanelStateIncrementalGeneratorPipeline.IsCandidate(node),
                static (syntaxContext, cancellationToken) =>
                    PanelStateIncrementalGeneratorPipeline.Transform(syntaxContext, cancellationToken))
            .Where(static target => target is not null)
            .Select(static (target, _) => target!);

        IncrementalValueProvider<ImmutableArray<PanelStateIncrementalGeneratorTypeTarget>> collectedTypeTargets = typeTargets.Collect();

        IncrementalValueProvider<PanelStateIncrementalGeneratorInput> generatorInput = context.CompilationProvider
            .Combine(collectedTypeTargets)
            .Select(
                static (pair, _) => new PanelStateIncrementalGeneratorInput(
                    pair.Left,
                    pair.Right));

        IncrementalValueProvider<PanelStateIncrementalGeneratorOutput> generatorOutput = generatorInput
            .Select(
                static (input, _) => PanelStateIncrementalGeneratorAnalysis.Analyze(input));

        context.RegisterSourceOutput(
            generatorOutput,
            static (productionContext, source) =>
                PanelStateIncrementalGeneratorEmitter.Emit(productionContext, source));
    }
}