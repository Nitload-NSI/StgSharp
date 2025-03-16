//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BpNodeGenerator.cs"
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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using StgSharp.PipeLine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StgSharp.Generator
{
    internal class BpNodeSyntaxReceiver : ISyntaxReceiver
    {

        private static BpNodeSyntaxReceiver receiver = new BpNodeSyntaxReceiver(
            );
        private static readonly string _indexTypeName = typeof( PartialDictionary<string, object> ).Name;

        public BpNodeSyntaxReceiver() { }

        public BpNodeSyntaxReceiver( SyntaxNode node )
        {
            OnVisitSyntaxNode( node );
        }

        public List<MethodDeclarationSyntax> ValidMethod
        {
            get;
            private set;
        } = new List<MethodDeclarationSyntax>();

        public List<MethodDeclarationSyntax> InvalidMethod
        {
            get;
            private set;
        } = new List<MethodDeclarationSyntax>();

        public static void BuildNode(
                                   SourceProductionContext context,
                                   MethodDeclarationSyntax mds )
        {
            if( mds.AttributeLists
                    .SelectMany( list => list.Attributes )
                    .Any(
                        syntax => syntax.Name.ToString() ==
                                  typeof( BlueprintNodeExecutionAttribute ).Name ) ) {
                SeparatedSyntaxList<ParameterSyntax> paramList = mds.ParameterList.Parameters;
                if( mds.Modifiers.Any( SyntaxKind.StaticKeyword ) && paramList.Count ==
                    2 && paramList[ 0 ].Type!.ToString() == _indexTypeName && paramList[
                    1 ].Type!.ToString() ==
                    _indexTypeName ) {
                    string methodName = mds.Identifier.Text;
                    SourceText sourceText = SourceText.From(
                        string.Empty, encoding: Encoding.UTF8 );
                    context.AddSource( "GeneratedBlueprintNode.cs",
                                       sourceText );
                } else {
                    Diagnostic diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            id: "SSE0001",
                            title: "Invalid Blueprint Node Method",
                            messageFormat: "Blueprint Node Method must be static and have two parameters of type PartialDictionary<string, object>",
                            category: "Blueprint Node",
                            DiagnosticSeverity.Error,
                            isEnabledByDefault: true ),
                        mds.GetLocation(), mds.Identifier.Text );

                    context.ReportDiagnostic( diagnostic );
                }
            }
        }

        public void OnVisitSyntaxNode( SyntaxNode syntaxNode )
        {
            if( syntaxNode is MethodDeclarationSyntax mds && mds.AttributeLists
                    .SelectMany( list => list.Attributes )
                    .Any(
                        syntax => syntax.Name.ToString() ==
                                  typeof( BlueprintNodeExecutionAttribute ).Name ) ) {
                SeparatedSyntaxList<ParameterSyntax> paramList = mds.ParameterList.Parameters;
                if( mds.Modifiers.Any( SyntaxKind.StaticKeyword ) && paramList.Count ==
                    2 && paramList[ 0 ].Type!.ToString() == _indexTypeName && paramList[
                    1 ].Type!.ToString() ==
                    _indexTypeName ) {
                    ValidMethod.Add( mds );
                } else {
                    InvalidMethod.Add( mds );
                }
            }
        }

    }
}
