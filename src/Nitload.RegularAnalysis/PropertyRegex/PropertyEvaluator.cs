// -----------------------------------------------------------------------------
// file="PropertyEvaluator"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.RegularAnalysis.PropertyRegex
{
    internal abstract class PropertyEvaluator<T> : IRelationEvaluator<T, PropertyMatchState>
    {

        public abstract PropertyMatchState Evaluate(
                                           ReadOnlySpan<T> sequence
        );

    }
}
