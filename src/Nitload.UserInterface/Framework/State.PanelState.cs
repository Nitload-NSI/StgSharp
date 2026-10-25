// -----------------------------------------------------------------------------
// file="State.PanelState"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Nitload.UserInterface
{
    public abstract class PanelState
    {

        protected PanelState(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)) :
                   name;
        }

        public string Name { get; }

    }
}
