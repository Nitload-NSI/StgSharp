// -----------------------------------------------------------------------------
// file="Relation"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.RegularAnalysis.Abstraction
{
    public class Relation<TElement, TValue, TEvaluator>(
                 TEvaluator evaluator
    ) where TEvaluator : IRelationEvaluator<TElement, TValue>
    {

        public TEvaluator Evaluator { get; init; } = evaluator;

        public TValue Weight { get; private set; } = default!;

        public TValue Evaluate(
                      ReadOnlySpan<TElement> sequence
        )
        {
            Weight = Evaluate(sequence);
            return Weight;
        }

    }

    public class Relation<TElement, TValue>(
                 IRelationEvaluator<TElement, TValue> evaluator
    ) : Relation<TElement, TValue, IRelationEvaluator<TElement, TValue>>(evaluator) { }

    public interface IRelationEvaluator<TElement, TValue>
    {

        TValue Evaluate(
               ReadOnlySpan<TElement> sequence
        );

    }
}
