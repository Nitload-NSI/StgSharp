// -----------------------------------------------------------------------------
// file="IGlApiFilter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    internal interface IGlApiFilter
    {

        IReadOnlyList<ManagedMethodDefinition> Apply(
            GlCommandDefinition command,
            IReadOnlyList<ManagedMethodDefinition> methods,
            GlEquivalentTypeMapper typeMapper
        );

    }
}
