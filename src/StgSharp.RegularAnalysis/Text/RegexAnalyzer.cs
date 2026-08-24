// -----------------------------------------------------------------------------
// file="RegexAnalyzer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                 Exception? analyseException = null

        )
        {
            Pattern = pattern;
            Ast = ast;
            AnalyseException = analyseException;
        }

        public Exception? AnalyseException { get; set; }

        public string Pattern { get; }

        internal AbstractSyntaxTree<RegexAstNode, RegexElementLabel> Ast { get; }

    }

    public static partial class RegexAnalyzer
    {

        public static TextRegexSource Analyze(
                                      string regex
        )
        {
#pragma warning disable CA1031
            // RegexAnalyzer interpreter = new();
            // interpreter._source = regex;
            AbstractSyntaxTree<RegexAstNode, RegexElementLabel> tree;
            try
            {
                tree = GenerateAST(regex);
                OptimizeTree(tree);
                return new TextRegexSource(regex, tree, null);
            }
            catch (Exception ex)
            {
                return new TextRegexSource(regex, null, ex);
            }
            /*
            // List<RegexIR> base_ir;
            try
            {
                // base_ir = GenerateIR(tree, ref info);
            }
            catch (Exception ex)
            {
                return new TextRegexSource(regex, require_ast ? tree : null!, null, null, info, ex);
            }
            try
            {
                // ScanAndOptimizeIR(base_ir, out List<RegexIR> opt_ir);
                // List<RegexIR> prefix_ir = EmitPrefix(opt_ir, ref info);
                return new TextRegexSource(regex, require_ast ? tree : null!,
                                           require_origin_ir ? base_ir : null!, prefix_ir, info);
            }
            catch (Exception ex)
            {
                return new TextRegexSource(regex, require_ast ? tree : null!,
                                           require_origin_ir ? base_ir : null!, null!, info, ex);
            }
            /**/
#pragma warning restore CA1031 
        }

    }
}
