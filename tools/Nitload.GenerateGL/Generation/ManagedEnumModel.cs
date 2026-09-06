// -----------------------------------------------------------------------------
// file="ManagedEnumModel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    internal sealed record ManagedEnumDefinition(
        string Name,
        bool IsFlags,
        IReadOnlyList<string> NativeGroups,
        IReadOnlyList<ManagedEnumMemberDefinition> Members
    );

    internal sealed record ManagedEnumMemberDefinition(
        string Name,
        GlEnumDefinition NativeValue
    );
}
