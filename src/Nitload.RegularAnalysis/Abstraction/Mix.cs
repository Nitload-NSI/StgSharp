// -----------------------------------------------------------------------------
// file="Mix"
// Project: ['.] nFramework
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
    public class Mix<TElement, TValue, TEvaluator>(TEvaluator evaluator)
        where TEvaluator: IMixEvaluator<TElement, TValue>
    {

        public TValue Weight { get; private set; } = default!;

        private TEvaluator Evaluator { get; init; } = evaluator;

        public TValue Evaluate(Span<Relation<TElement, TValue>> relations)
        {
            Weight = Evaluator.Evaluate(relations);
            return Weight;
        }

    }

    public sealed class Mix<TElement, TValue>(IMixEvaluator<TElement, TValue> evaluator) : Mix<TElement, TValue, IMixEvaluator<TElement, TValue>>(evaluator) { }

    public interface IMixEvaluator<TElement, TValue>
    {

        public Relation<TElement, TValue, IRelationEvaluator<TElement, TValue>>[] DownstreamPorts { get; }

        TValue Evaluate(Span<Relation<TElement, TValue>> relations);

    }
}
