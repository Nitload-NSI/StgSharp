// -----------------------------------------------------------------------------
// file="RegexIRGenerator"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal sealed class RegexIRGenerator
    {

        private readonly SequenceEmitter<RegexIR> _stream = new();

        public int Count => _stream.Count;

        public RegexIRGenerator EmitCondition(
                                int success,
                                int fail
        )
        {
            return Emit(new RegexConditionIR(success, fail));
        }

        public RegexIRGenerator EmitCountComplex(
                                int min,
                                int max,
                                bool isGreedy,
                                int operation
        )
        {
            return Emit(new RegexCountComplexIR(min, max, isGreedy, operation));
        }

        public RegexIRGenerator EmitCountSeq(
                                int min,
                                int max,
                                bool isGreedy,
                                string seq
        )
        {
            return Emit(new RegexCountSeqIR(min, max, isGreedy, seq));
        }

        public RegexIRGenerator EmitCountSet(
                                int min,
                                int max,
                                bool isGreedy,
                                string set
        )
        {
            return Emit(new RegexCountSetIR(min, max, isGreedy, set));
        }

        public RegexIRGenerator EmitIRStream(
                                RegexIRGenerator generator
        )
        {
            ArgumentNullException.ThrowIfNull(generator);

            _ = _stream.Append(generator._stream);
            return this;
        }

        public RegexIRGenerator EmitMatchSeq(
                                string unit
        )
        {
            return Emit(new RegexMatchSeqIR(unit));
        }

        public RegexIRGenerator EmitMatchSet(
                                string unit
        )
        {
            return Emit(new RegexMatchSetIR(unit));
        }

        public RegexIRGenerator EmitPack(
                                bool condition,
                                string name = ""
        )
        {
            return Emit(new RegexPackIR(condition, name));
        }

        public RegexIRGenerator EmitPop(
                                bool isSuccess
        )
        {
            return Emit(new RegexPopIR(isSuccess));
        }

        public RegexIRGenerator EmitTry(
                                int count
        )
        {
            return Emit(new RegexTryIR(count));
        }

        public SequenceEmitter<RegexIR>.Enumerator GetEnumerator()
        {
            return _stream.GetEnumerator();
        }

        public List<RegexIR> ToList()
        {
            return _stream.ToList();
        }

        private RegexIRGenerator Emit(
                                 RegexIR ir
        )
        {
            _ = _stream.Append(ir);
            return this;
        }

    }
}
