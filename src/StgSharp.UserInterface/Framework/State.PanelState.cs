// -----------------------------------------------------------------------------
// file="State.PanelState"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace StgSharp.UserInterface
{
    public abstract class PanelState
    {

        protected PanelState(
                  string name
        )
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)) :
                   name;
        }

        public string Name { get; }

    }
}
