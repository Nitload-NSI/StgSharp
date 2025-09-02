//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpCodeGenerator.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;

namespace StgSharp.Generator
{
    [Generator]
    public partial class MainGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var declaration =
                context.SyntaxProvider.CreateSyntaxProvider(
                    predicate: static (s,_) => s is MethodDeclarationSyntax,
                    transform: static (cds,_) => (MethodDeclarationSyntax)cds.Node
                    ).Where(static m=>m!=null);

            context.RegisterSourceOutput(declaration, BpNodeSyntaxReceiver.BuildNode);

            // Add multi-version framework source generation
            var frameworkUsages = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (s, _) => IsFrameworkUsage(s),
                transform: static (ctx, _) => ExtractFrameworkUsage(ctx)
            ).Where(static usage => usage != null);

            context.RegisterSourceOutput(frameworkUsages, GenerateFrameworkProxy);
        }

        private static bool IsFrameworkUsage(SyntaxNode node)
        {
            // Detect World.* calls or other framework API usage
            if (node is InvocationExpressionSyntax invocation)
            {
                var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
                if (memberAccess?.Expression is IdentifierNameSyntax identifier)
                {
                    return identifier.Identifier.ValueText == "World";
                }
            }
            return false;
        }

        private static FrameworkUsage? ExtractFrameworkUsage(GeneratorSyntaxContext context)
        {
            if (context.Node is InvocationExpressionSyntax invocation)
            {
                var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
                if (memberAccess != null)
                {
                    return new FrameworkUsage
                    {
                        MethodName = memberAccess.Name.Identifier.ValueText,
                        Node = invocation
                    };
                }
            }
            return null;
        }

        private static void GenerateFrameworkProxy(SourceProductionContext context, FrameworkUsage? usage)
        {
            if (usage == null) return;

            var source = $$"""
// Auto-generated framework proxy code
using StgSharp.SDK;

namespace StgSharp.Generated
{
    public static class WorldProxy
    {
        public static void {{usage.MethodName}}()
        {
            World.{{usage.MethodName}}();
        }
    }
}
""";

            context.AddSource("WorldProxy.g.cs", source);
        }
    }

    internal class FrameworkUsage
    {
        public string MethodName { get; set; } = string.Empty;
        public SyntaxNode? Node { get; set; }
    }
}
