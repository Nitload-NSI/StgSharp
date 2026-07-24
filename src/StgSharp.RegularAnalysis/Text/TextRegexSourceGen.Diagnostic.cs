// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.Diagnostic"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        private static readonly DiagnosticDescriptor CanNotBeRecordDiag = new(id:"NGRA004",
                                                                              title:$"Containing type cannot be record",
                                                                              messageFormat:$"Containing type: {{0}} cannot be record",
                                                                              defaultSeverity:DiagnosticSeverity.Error,
                                                                              category:"NGRA.TextRegex",
                                                                              isEnabledByDefault:true);

        private static readonly DiagnosticDescriptor CompileFailDiag = new(id:"NGRA005",
                                                                           title:"Unexpected exception when compiling",
                                                                           messageFormat:"Unexpected exception handles when compiling expression \"{0}\":{1}",
                                                                           defaultSeverity:DiagnosticSeverity.Error,
                                                                           category:"NGRA.TextRegex",
                                                                           isEnabledByDefault:true);

        private static readonly DiagnosticDescriptor IncorrectContainingTypeDiag = new(id:"NGRA005",
                                                                                       title:$"Containing type can only be struct or class",
                                                                                       messageFormat:$"Containing type: {{0}} is a {{1}}, but not class or struct",
                                                                                       defaultSeverity:DiagnosticSeverity.Error,
                                                                                       category:"NGRA.TextRegex",
                                                                                       isEnabledByDefault:true);

        private static readonly DiagnosticDescriptor IncorrectReturnTypeDiag = new(id:"NGRA003",
                                                                                   title:"Text regex binding must be partial",
                                                                                   messageFormat:"Method {0} must be partial to receive generated code",
                                                                                   defaultSeverity:DiagnosticSeverity.Error,
                                                                                   category:"NGRA.TextRegex",
                                                                                   isEnabledByDefault:true);

        private static readonly DiagnosticDescriptor NotParameterLessDiag = new(id:"NGRA002",
                                                                                title:"Text regex binding must not have any parameter",
                                                                                messageFormat:"Method {0} must not have any parameter",
                                                                                defaultSeverity:DiagnosticSeverity.Error,
                                                                                category:"NGRA.TextRegex",
                                                                                isEnabledByDefault:true);

        private static readonly DiagnosticDescriptor NotPartialDiag = new(id:"NGRA001",
                                                                          title:$"Returning type must be {typeof(TextRegex).FullName}",
                                                                          messageFormat:$"Method {{0}} must return {typeof(TextRegex).FullName}",
                                                                          defaultSeverity:DiagnosticSeverity.Error,
                                                                          category:"NGRA.TextRegex",
                                                                          isEnabledByDefault:true);

    }
}
