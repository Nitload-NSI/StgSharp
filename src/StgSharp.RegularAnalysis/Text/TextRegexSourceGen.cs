// -----------------------------------------------------------------------------
// file="TextRegexSourceGen"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StgSharp.RegularAnalysis.Abstraction;
using System.Collections.Generic;
using System.Threading;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen : IIncrementalGenerator
    {

        private static readonly string TextRegexTypeName = typeof(TextRegexAttribute).FullName ??
            string.Empty;

        public void Initialize(
                    IncrementalGeneratorInitializationContext context
        )
        {
            IncrementalValuesProvider<TextRegexBinding> candidates =

                //
                context.SyntaxProvider
                       .ForAttributeWithMetadataName<TextRegexBinding>(
                TextRegexTypeName,
                static(
                _,
                _
                ) => true, BindTextRegexCandidate);

            IncrementalValuesProvider<TextRegexBinding> validCandidates =
                RegisterDiagnosticBypass(context, candidates);

            IncrementalValuesProvider<TextRegexBinding> analyzed =
                validCandidates.Select(AnalyzeRegexMethod);

            IncrementalValuesProvider<TextRegexBinding> validAnalyzed =
                RegisterDiagnosticBypass(context, analyzed);

            IncrementalValuesProvider<TextRegexBinding> generated =
                validAnalyzed.Select(CompileRegexMethodSource);

            IncrementalValuesProvider<TextRegexBinding> validGenerated =
                RegisterDiagnosticBypass(context, generated);
            context.RegisterSourceOutput(validGenerated, GenerateRegexSourceFile);
        }

        private static TextRegexBinding AnalyzeRegexMethod(
                                        TextRegexBinding binding,
                                        CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!binding.IsValid) {
                return binding;
            }

            TextRegexBinding result = binding.Clone();
            string pattern = result.Source.Pattern;
            TextRegexSource analyzedSource = RegexAnalyzer.Analyze(pattern);
            result.AnalyzedSource = analyzedSource;
            if (analyzedSource.AnalyseException is not null) {
                result.AddDiagnostic(CompileFailDiag, result.Source.PatternDefineLocation, pattern,
                                     analyzedSource.AnalyseException.Message);
            }

            return result;
        }

        private static TextRegexBinding BindTextRegexCandidate(
                                        GeneratorAttributeSyntaxContext context,
                                        CancellationToken ct
        )
        {
            ct.ThrowIfCancellationRequested();

            if (context.TargetNode is not MethodDeclarationSyntax method_decl ||
                context.TargetSymbol is not IMethodSymbol method_symbol) {
                return TextRegexBinding.Invalid();
            }

            /*
             *return node is MethodDeclarationSyntax method
             *    && method_decl.Modifiers.Any(SyntaxKind.PartialKeyword)
             *    && method_decl.Modifiers.Any(SyntaxKind.StaticKeyword)
             *    && method_decl.ParameterList.Parameters.Count == 0
             *    && method_decl.Body is null
             *    && method_decl.ExpressionBody is null
             *    && method_decl.SemicolonToken.IsKind(SyntaxKind.SemicolonToken);
             */

            TextRegexBinding bind = new();
            if (!method_decl.Modifiers.Any(SyntaxKind.PartialKeyword)) {
                bind.AddDiagnostic(NotPartialDiag, method_decl.GetLocation(), method_symbol.Name);
            }
            if (method_decl.ParameterList.Parameters.Count != 0) {
                bind.AddDiagnostic(NotParameterLessDiag, method_decl.GetLocation(), method_decl.GetText());
            }


            INamedTypeSymbol? expected_return_type =
                context.SemanticModel.Compilation
                                     .GetTypeByMetadataName(TextRegexTypeName
                    );
            ITypeSymbol ret_value = method_symbol.ReturnType;

            if (!SymbolEqualityComparer.Default.Equals(expected_return_type, ret_value)) {
                bind.AddDiagnostic(IncorrectReturnTypeDiag, method_decl.ReturnType.GetLocation(), method_symbol.Name);
            }

            AttributeData attribute = context.Attributes[0];

            INamedTypeSymbol contain = method_symbol.ContainingType;
            INamespaceSymbol namespace_symbol = contain.ContainingNamespace;
            TypeSource type_source = null!;

            while (contain is not null)
            {
                if (contain.IsRecord) {
                    bind.AddDiagnostic(CanNotBeRecordDiag, method_decl.GetLocation(), contain.Name);
                }
                if (contain.TypeKind is not (TypeKind.Class or TypeKind.Struct)) {
                    bind.AddDiagnostic(IncorrectContainingTypeDiag, method_decl.GetLocation(),
                                       method_symbol.Name, contain.TypeKind);
                }
                if (!bind.IsValid) {
                    return bind;
                }
                string display = contain.ToDisplayString(TypeSourceFormat);
                TypeSource t = new(contain.IsStatic, display, contain.Name, contain.TypeKind,
                                   contain.DeclaredAccessibility, type_source);
                type_source = t;
                contain = contain.ContainingType!;
            }
            Source s = new()
            {
                MethodName = method_symbol.Name,
                NameSpaceName = namespace_symbol.ToDisplayString(),
                PatternDefineLocation = method_decl.GetLocation(),
                IsMethodStatic = method_decl.Modifiers.Any(SyntaxKind.StaticKeyword),
                ContainingType = type_source,
                MethodAccessibility = method_symbol.DeclaredAccessibility,
            };
            foreach (KeyValuePair<string, TypedConstant> item in attribute.NamedArguments)
            {
                if (item.Key == nameof(TextRegexAttribute.Pattern)) {
                    s.Pattern = item.Value.Value as string ?? string.Empty;
                }
            }

            bind.Source = s.AsRecord();
            return bind;
        }

        private static TextRegexBinding CompileRegexMethodSource(
                                        TextRegexBinding binding,
                                        CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            TextRegexBinding result = binding.Clone();
            RegexAstNode root = result.AnalyzedSource!.Ast.Root;
            SourceGenContext generationContext = new();
            SequenceEmitter<string> methodSource = new();
            List<SequenceEmitter<string>> localFunctions = [];

            GenerateMethodSource(root, methodSource, localFunctions, generationContext);
            foreach (SequenceEmitter<string> localFunction in localFunctions) {
                _ = methodSource.AppendLine()
                                .AppendLine()
                                .AppendLine("// local method for matching")
                                .Append(localFunction);
            }

            result.SourceContext = generationContext;
            result.GeneratedSource = methodSource;
            return result;
        }

        private static IncrementalValuesProvider<TextRegexBinding> RegisterDiagnosticBypass(
                                                                   IncrementalGeneratorInitializationContext context,
                                                                   IncrementalValuesProvider<TextRegexBinding> bindings
        )
        {
            IncrementalValuesProvider<Diagnostic> errorPath =
                bindings.Where(static binding => !binding.IsValid)
                        .SelectMany(static(
                                    binding,
                                    _
                        ) => binding.RegexDiagnostic);
            context.RegisterSourceOutput(errorPath, static(
                                                    productionContext,
                                                    diagnostic
            ) => productionContext.ReportDiagnostic(diagnostic));

            return bindings.Where(static binding => binding.IsValid);
        }

    }
}
