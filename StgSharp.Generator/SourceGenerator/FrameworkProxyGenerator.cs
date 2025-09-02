using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace StgSharp.Generator
{
    /// <summary>
    /// Source generator for multi-version framework architecture
    /// Generates proxy code for framework API calls during development
    /// </summary>
    [Generator]
    public class FrameworkProxyGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Look for compilation attributes that indicate build mode
            var buildMode = context.CompilationProvider.Select((c, _) => GetBuildMode(c));

            // Find World API usages
            var worldUsages = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (s, _) => IsWorldApiUsage(s),
                transform: static (ctx, _) => ExtractWorldUsage(ctx)
            ).Where(static usage => usage != null);

            var combined = worldUsages.Combine(buildMode);

            context.RegisterSourceOutput(combined, GenerateProxyCode);
        }

        private static BuildMode GetBuildMode(Compilation compilation)
        {
            // Check for conditional compilation symbols
            var preprocessorSymbols = compilation.SyntaxTrees
                .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<DefineDirectiveTriviaSyntax>())
                .Select(define => define.Name.ValueText)
                .ToList();

            if (preprocessorSymbols.Contains("STGSHARP_RELEASE_BUILD"))
                return BuildMode.Release;
            
            if (preprocessorSymbols.Contains("STGSHARP_DEVELOPMENT"))
                return BuildMode.Development;

            // Default: check for DEBUG symbol
            if (compilation.Options.OptimizationLevel == OptimizationLevel.Debug)
                return BuildMode.Development;

            return BuildMode.Release;
        }

        private static bool IsWorldApiUsage(SyntaxNode node)
        {
            return node is InvocationExpressionSyntax invocation &&
                   invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                   memberAccess.Expression is IdentifierNameSyntax identifier &&
                   identifier.Identifier.ValueText == "World";
        }

        private static WorldApiUsage? ExtractWorldUsage(GeneratorSyntaxContext context)
        {
            if (context.Node is InvocationExpressionSyntax invocation &&
                invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                var methodName = memberAccess.Name.Identifier.ValueText;
                var arguments = invocation.ArgumentList.Arguments.Select(arg => arg.ToString()).ToArray();

                return new WorldApiUsage
                {
                    MethodName = methodName,
                    Arguments = arguments,
                    Location = invocation.GetLocation()
                };
            }
            return null;
        }

        private static void GenerateProxyCode(SourceProductionContext context, 
            (WorldApiUsage? Usage, BuildMode Mode) input)
        {
            var (usage, mode) = input;
            if (usage == null) return;

            if (mode == BuildMode.Development)
            {
                // Development mode: direct calls to framework
                GenerateDevelopmentProxy(context, usage);
            }
            else
            {
                // Release mode: SDK calls for dynamic loading
                GenerateReleaseProxy(context, usage);
            }
        }

        private static void GenerateDevelopmentProxy(SourceProductionContext context, WorldApiUsage usage)
        {
            var argumentsStr = string.Join(", ", usage.Arguments);
            var source = $$"""
// Auto-generated development proxy for {{usage.MethodName}}
using StgSharp;

namespace StgSharp.Generated
{
    public static partial class WorldProxy
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void {{usage.MethodName}}({{GetParameterSignature(usage)}})
        {
            // Direct call to framework in development mode
            StgSharp.World.{{usage.MethodName}}({{argumentsStr}});
        }
    }
}
""";

            context.AddSource($"WorldProxy_{usage.MethodName}_Dev.g.cs", source);
        }

        private static void GenerateReleaseProxy(SourceProductionContext context, WorldApiUsage usage)
        {
            var argumentsStr = string.Join(", ", usage.Arguments);
            var source = $$"""
// Auto-generated release proxy for {{usage.MethodName}}
using StgSharp.SDK;

namespace StgSharp.Generated
{
    public static partial class WorldProxy
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void {{usage.MethodName}}({{GetParameterSignature(usage)}})
        {
            // Dynamic call via SDK in release mode
            World.{{usage.MethodName}}({{argumentsStr}});
        }
    }
}
""";

            context.AddSource($"WorldProxy_{usage.MethodName}_Release.g.cs", source);
        }

        private static string GetParameterSignature(WorldApiUsage usage)
        {
            // This would need more sophisticated parameter type detection
            // For now, assume common patterns
            return usage.MethodName switch
            {
                "LogInfo" => "string message",
                "LogError" => "System.Exception exception",
                _ => ""
            };
        }
    }

    internal class WorldApiUsage
    {
        public string MethodName { get; set; } = string.Empty;
        public string[] Arguments { get; set; } = new string[0];
        public Location? Location { get; set; }
    }

    internal enum BuildMode
    {
        Development,
        Release
    }
}