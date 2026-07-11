// -----------------------------------------------------------------------------
// file="SimpleStateMachineNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.State
{
    public abstract class SimpleStateMachineNode<TSelf, TLabel, TStateContext>
        where TSelf : SimpleStateMachineNode<TSelf, TLabel, TStateContext>
        where TLabel : unmanaged, IEquatable<TLabel>
        where TStateContext : StateContext
    {

        private readonly List<int> _jumpTable = [];

        public ReadOnlySpan<int> AvailableNodes => CollectionsMarshal.AsSpan(_jumpTable);

        public abstract bool IsValidateFrom(
                             TSelf former,
                             TStateContext context
        );

        public abstract bool TryTransaction(
                             ReadOnlySpan<TSelf> nodeSpan,
                             TStateContext context,
                             out TLabel label
        );

    }
}
