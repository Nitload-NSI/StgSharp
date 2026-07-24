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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            IncrementalValuesProvider<TextRegexBinding> provider = context.SyntaxProvider
                                                                          .ForAttributeWithMetadataName<TextRegexBinding>(
                TextRegexTypeName,
                static(
                _,
                _
                ) => true, BindTextRegexCandidate);

            IncrementalValuesProvider<TextRegexBinding> match = RegisterDiagnosticBypass(context, provider);

            IncrementalValuesProvider<TextRegexBinding> compile = match.Select(CompileRegexMethodSource);

            IncrementalValuesProvider<TextRegexBinding> compile_success = RegisterDiagnosticBypass(context, compile);

            context.RegisterSourceOutput(compile_success, GenerateRegexSourceFile);
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
                bind.AddDiagnostic(NotPartialDiag, method_decl.GetLocation(), method_decl.GetText());
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
                    bind.AddDiagnostic(CanNotBeRecordDiag, method_decl.GetLocation(), method_symbol.Name);
                }
                if (contain.TypeKind is not TypeKind.Class or TypeKind.Struct) {
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
                                        TextRegexBinding source,
                                        CancellationToken ct
        )
        {
            ct.ThrowIfCancellationRequested();
            TextRegexBinding result = source.Clone();
            string pattern = result.Source.Pattern;
            TextRegexSource analyze_result = RegexAnalyzer.Analyze(pattern);
            if (analyze_result.AnalyseException is not null)
            {
                result.AddDiagnostic(CompileFailDiag, null, analyze_result.AnalyseException.Message);
            } else
            {
                SequenceEmitter<string> method_source = GenerateSource(analyze_result);
                result.GeneratedSource = method_source;
            }

            return result;
        }

        private static IncrementalValuesProvider<TextRegexBinding> RegisterDiagnosticBypass(
                                                                   IncrementalGeneratorInitializationContext context,
                                                                   IncrementalValuesProvider<TextRegexBinding> bindings
        )
        {
            IncrementalValuesProvider<Diagnostic> error_path = bindings.Where(static binding => !binding.IsValid)
                                                                       .SelectMany(static(
                                                                                   binding,
                                                                                   _
                                                                       ) => binding.RegexDiagnostic);
            IncrementalValuesProvider<TextRegexBinding> correct_path = bindings.Where(static binding => binding.IsValid);

            context.RegisterSourceOutput(error_path, (
                                                     c,
                                                     diag
            ) => c.ReportDiagnostic(diag));

            return correct_path;
        }

    }
}
