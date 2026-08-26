// -----------------------------------------------------------------------------
// file="GlEnumDefinition"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace StgSharp.GenerateGL.Registry
{
    internal sealed record GlEnumDefinition(
        string Name,
        string? Value,
        string? Api,
        string? TypeSuffix,
        string? Group,
        string? Alias,
        string? Comment,
        GlRegistryElement Element
    );

    internal sealed record GlEnumBlock(
        string? Namespace,
        string? Group,
        string? Type,
        string? Start,
        string? End,
        string? Vendor,
        string? Comment,
        IReadOnlyList<GlEnumDefinition> Enums,
        IReadOnlyList<GlUnusedEnumRange> UnusedRanges,
        GlRegistryElement Element
    );

    internal sealed record GlEnumGroup(
        string Name,
        string? Comment,
        IReadOnlyList<GlNamedReference> Members,
        GlRegistryElement Element
    );

    internal sealed record GlUnusedEnumRange(
        string Start,
        string? End,
        string? Vendor,
        string? Comment,
        GlRegistryElement Element
    );
}
