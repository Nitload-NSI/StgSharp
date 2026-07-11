// -----------------------------------------------------------------------------
// file="CompileDepthState"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public interface ICompileDepthState
    {

        string CurrentState { get; set; }

    }

    public class CompileDepthMark
    {

        internal CompileDepthMark(
                 int operatorBegin,
                 int operandBegin,
                 int usage
        )
        {
            OperatorBegin = operatorBegin;
            OperandBegin = operandBegin;
            Usage = usage;
        }

        internal CompileDepthMark(
                 int operatorBegin,
                 int operandBegin,
                 int usage,
                 ICompileDepthState stateCache
        )
        {
            OperatorBegin = operatorBegin;
            OperandBegin = operandBegin;
            Usage = usage;
            StateCache = stateCache;
        }

        public bool IsBeginOfBlock { get; internal set; }

        public ICompileDepthState StateCache { get; internal set; }

        public int OperandBegin { get; internal set; }

        public int OperatorBegin { get; internal set; }

        public int Usage { get; internal set; }

    }
}
