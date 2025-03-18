//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpPostProcessor.cs"
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpPostProcessor
    {

        private readonly string _interfaceName /*= typeof(  )/**/;
        private readonly string _sourcePath;

        public string Process( string filePath )
        {
            Dictionary<string, string> factoryMethods = new Dictionary<string, string>(
                );

            FileStream file = File.OpenRead( filePath );
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
                File.ReadAllText( filePath ) );

            CSharpCompilation compilation = CSharpCompilation.Create(
                "Analysis" )
                    .AddSyntaxTrees( syntaxTree )
                    .AddReferences(
                        MetadataReference.CreateFromFile(
                            typeof( object ).Assembly.Location ) );

            INamedTypeSymbol? interfaceSymbol = compilation.GetTypeByMetadataName(
                _interfaceName );

            if( interfaceSymbol == null ) {
                throw new InvalidOperationException(
                    $"Interface '{_interfaceName}' not found." );
            }

            SemanticModel semanticModel = compilation.GetSemanticModel(
                syntaxTree );
            SyntaxNode root = syntaxTree.GetRoot();

            IEnumerable<ClassDeclarationSyntax> classDeclarations = root.DescendantNodes(
                )
                    .OfType<ClassDeclarationSyntax>()
                    .Where(
                        cls => cls.BaseList?.Types.Any(
                                t => SymbolEqualityComparer.Default
                                        .Equals(
                                            semanticModel.GetTypeInfo( t.Type )
                                                                .Type,
                                            interfaceSymbol ) ) ??
                            false );

            foreach( ClassDeclarationSyntax classDeclaration in classDeclarations ) {
                string className = classDeclaration.Identifier.Text;
                IMethodSymbol? factoryMethod = interfaceSymbol.GetMembers()
                        .OfType<IMethodSymbol>()
                        .FirstOrDefault(
                            m => m.IsStatic && m.Name == "CreateInstance" );

                if( factoryMethod != null ) {
                    factoryMethods[ className ] = $"() => {interfaceSymbol.Name}.CreateInstance()";
                }
            }

            string code = "var factoryMethods = new Dictionary<string, Func<object>>\n{\n";
            code += string.Join(
                ",\n",
                factoryMethods.Select(
                    kvp => $"    {{ \"{kvp.Key}\", {kvp.Value} }}" ) );
            code += "\n};";

            return code;
        }

    }
}
