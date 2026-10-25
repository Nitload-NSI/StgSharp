// -----------------------------------------------------------------------------
// file="PropertyMatchEngine"
// Project: ['.] nFramework
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
    public class PropertyMatchEngine<T>
    {

        private readonly List<Mix<T, PropertyMatchState>> _nodes = [];
        private readonly List<Relation<T, PropertyMatchState, PropertyEvaluator<T>>> _relations = [];

    }

    public class FrozenPropertyMatchEngine<T>
    {

        private readonly Mix<T, PropertyMatchState>[] _nodes;
        private readonly Relation<T, PropertyMatchState, PropertyEvaluator<T>>[] _relations;

    }
}
