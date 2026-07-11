// -----------------------------------------------------------------------------
// file="ISimpleStateMachine"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.State
{
    public interface ISimpleStateMachine<TStateNode, TStateLabel, TStateContext>
        where TStateNode : SimpleStateMachineNode<TStateNode, TStateLabel, TStateContext>
        where TStateLabel : unmanaged, IEquatable<TStateLabel>
        where TStateContext : StateContext
    {

        TStateNode this[
                   TStateLabel label
        ] { get; }

        ref readonly TStateNode GetCurrentStateNode();

        void Reset();

        bool TryMoveNext(
             TStateContext ctx
        );

    }
}
