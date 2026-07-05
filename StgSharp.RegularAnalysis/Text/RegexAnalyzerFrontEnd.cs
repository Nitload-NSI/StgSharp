//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexAnalyzer"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    public record TextRegexSource
    {

        internal TextRegexSource(
                 string pattern,
                 AbstractSyntaxTree<RegexAstNode, RegexElementLabel> ast,
                 List<RegexIR> base_ir,
                 List<RegexIR> ir,
                 Exception? analyseException = null
        )
        {
            Pattern = pattern;
            Ast = ast;
            IRs = ir;
            BaseIR = base_ir;
            AnalyseException = analyseException;
        }

        public Exception? AnalyseException { get; set; }

        public string Pattern { get; }

        internal AbstractSyntaxTree<RegexAstNode, RegexElementLabel> Ast { get; }

        internal List<RegexIR> BaseIR { get; }

        internal List<RegexIR> IRs { get; }

    }

    public static partial class RegexAnalyzerFrontEnd
    {

        public static TextRegexSource Analyze(
                                      string regex
        )
        {
            // RegexAnalyzer interpreter = new();
            // interpreter._source = regex;
            try
            {
                AbstractSyntaxTree<RegexAstNode, RegexElementLabel> tree = GenerateAST(regex);
                OptimizeTree(tree);
                List<RegexIR> base_ir = GenerateIR(tree);
                ScanAndOptimizeIR(base_ir, out List<RegexIR> opt_ir);
                return new TextRegexSource(regex, tree, base_ir, opt_ir);
            }
            catch (Exception ex)
            {
                return new TextRegexSource(regex, null, null, null, ex);
            }
        }

    }
}
