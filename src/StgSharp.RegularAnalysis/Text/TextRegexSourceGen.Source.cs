// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.Source"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        private readonly record struct SourceRecord(

                                       bool IsClassStatic,
                                       bool IsMethodStatic,
                                       string Pattern,
                                       string MethodName,
                                       string NamespaceName,
                                       Accessibility MethodAccessibility,
                                       TypeSource ContainingType,
                                       Location PatternDefineLocation
        );

        private sealed record TypeSource(
                              bool IsStatic,
                              string TypeDisplayName,
                              string TypeName,
                              TypeKind DeclType,
                              Accessibility Accessibility,
                              TypeSource? InnerType
        );

        private struct Source
        {

            public Accessibility MethodAccessibility { get; set; }

            public bool IsClassStatic { get; set; }

            public bool IsMethodStatic { get; set; }

            public Location PatternDefineLocation { get; set; }

            public string NameSpaceName { get; set; }

            public string Pattern { get; set; }

            public string MethodName { get; set; }

            public TypeSource ContainingType { get; set; }

            public readonly SourceRecord AsRecord()
            {
                return new SourceRecord(IsClassStatic, IsMethodStatic, Pattern, MethodName,
                                        NameSpaceName, MethodAccessibility, ContainingType,
                                        PatternDefineLocation);
            }

        }

        private class TextRegexBinding
        {

            private readonly List<Diagnostic> _diagnostics = [];

            private TextRegexBinding(
                    List<Diagnostic> diagnostics,
                    SequenceEmitter<string> generatedSource,
                    SourceRecord source
            )
            {
                _diagnostics = diagnostics ?? [];
                GeneratedSource = generatedSource ?? new();
                Source = source;
            }

            public TextRegexBinding()
            {
                _diagnostics = [];
            }

            public bool IsValid { get; private set; } = true;

            public IReadOnlyList<Diagnostic> RegexDiagnostic => _diagnostics;

            public SequenceEmitter<string> GeneratedSource { get; set; }

            public SourceRecord Source { get; set; }

            public void AddDiagnostic(
                        Diagnostic diag
            )
            {
                _diagnostics.Add(diag);
                IsValid = IsValid && (diag.DefaultSeverity != DiagnosticSeverity.Error);
            }

            public void AddDiagnostic(
                        DiagnosticDescriptor descriptor,
                        Location? location,
                        params object[] messageArgs
            )
            {
                Diagnostic diag = Diagnostic.Create(descriptor, location, messageArgs);
                _diagnostics.Add(diag);
                IsValid = IsValid && (diag.Severity != DiagnosticSeverity.Error);
            }

            public TextRegexBinding Clone()
            {
                SequenceEmitter<string> code = GeneratedSource.Clone();
                List<Diagnostic> diag = new(_diagnostics.Count);
                diag.AddRange(this._diagnostics);
                TextRegexBinding binding = new(diag, code, Source with { });
                return binding;
            }

            public static TextRegexBinding Invalid()
            {
                return new TextRegexBinding
                {
                    IsValid = false
                };
            }

        }

    }
}
