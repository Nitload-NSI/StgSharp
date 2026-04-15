using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace StgSharp.UserInterface.Generator
{
    internal static class PanelStateIncrementalGeneratorPipeline
    {

        public static bool IsCandidate(
                           SyntaxNode node
        )
        {
            return node is TypeDeclarationSyntax typeDeclaration && HasAttributedMembers(typeDeclaration);
        }

        public static PanelStateIncrementalGeneratorTypeTarget? Transform(
                                                GeneratorSyntaxContext context,
                                                CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (context.Node is not TypeDeclarationSyntax typeDeclaration) {
                return null;
            }

            if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration, cancellationToken) is not INamedTypeSymbol typeSymbol)
            {
                return null;
            }

            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> markedMembers = CollectMarkedMembers(typeSymbol);
            if (markedMembers.IsDefaultOrEmpty)
            {
                return null;
            }

            return new PanelStateIncrementalGeneratorTypeTarget(
                typeSymbol,
                typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                markedMembers);
        }

        private static bool HasAttributedMembers(
                            TypeDeclarationSyntax typeDeclaration
        )
        {
            foreach (MemberDeclarationSyntax member in typeDeclaration.Members)
            {
                switch (member)
                {
                    case FieldDeclarationSyntax { AttributeLists.Count: > 0 }:
                    case PropertyDeclarationSyntax { AttributeLists.Count: > 0 }:
                    case MethodDeclarationSyntax { AttributeLists.Count: > 0 }:
                        return true;
                }
            }

            return false;
        }

        private static ImmutableArray<PanelStateIncrementalGeneratorMarkedMember> CollectMarkedMembers(
                              INamedTypeSymbol typeSymbol
        )
        {
            ImmutableArray<PanelStateIncrementalGeneratorMarkedMember>.Builder builder =
                ImmutableArray.CreateBuilder<PanelStateIncrementalGeneratorMarkedMember>();

            foreach (ISymbol member in typeSymbol.GetMembers())
            {
                foreach (AttributeData attributeData in member.GetAttributes())
                {
                    string? metadataName = attributeData.AttributeClass?.ToDisplayString();
                    if (!PanelStateIncrementalGeneratorKnownAttributes.IsKnown(metadataName))
                    {
                        continue;
                    }

                    builder.Add(
                        new PanelStateIncrementalGeneratorMarkedMember(
                            member,
                            metadataName!,
                            member.Name,
                            GetSymbolTypeName(member)));
                    break;
                }
            }

            return builder.ToImmutable();
        }

        private static string GetSymbolTypeName(
                              ISymbol symbol
        )
        {
            return symbol switch
            {
                IFieldSymbol fieldSymbol => fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                IPropertySymbol propertySymbol => propertySymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                IMethodSymbol methodSymbol => methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                _ => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            };
        }

    }
}
