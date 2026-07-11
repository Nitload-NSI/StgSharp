// -----------------------------------------------------------------------------
// file="RegexIR.Abstraction"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal interface IRegexComplex
    {

        bool IsFullyOptimized { get; set; }

        int CommandCount { get; set; }

    }

    internal interface IRegexSequence
    {

        string Pattern { get; }

    }

    internal interface IRegexCount
    {

        bool IsGreedy { get; }

        bool IsStar { get; }

        bool IsPlus { get; }

        int Min { get; }

        int Max { get; }

    }

    internal abstract class RegexIR
    {

        protected RegexIR(RegexIRCommand command)
        {
            CommandMask = command;
        }

        public bool SingleLineResultRequired { get; set; }

        public int SingleLineResultIndex { get; set; }

        public RegexIRPrefix Prefix { get; set; }

        internal RegexIRCommand CommandMask { get; init; }

        internal string AsCommand(int lineNum)
        {
            return $"[{lineNum:D3}] {Prefix}, {AsCommand()}";
        }

        protected abstract string AsCommand();

    }
}
