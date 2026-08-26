// -----------------------------------------------------------------------------
// file="GlApiDefinition"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace StgSharp.GenerateGL.Registry
{
    internal sealed record GlFeatureDefinition(
        string Name,
        string Api,
        string Number,
        string? Protect,
        string? Comment,
        IReadOnlyList<GlRequirementBlock> Requirements,
        GlRegistryElement Element
    );

    internal sealed record GlExtensionDefinition(
        string Name,
        string? SupportedApis,
        string? Protect,
        string? Comment,
        IReadOnlyList<GlRequirementBlock> Requirements,
        GlRegistryElement Element
    );

    internal sealed record GlRequirementBlock(
        GlRequirementOperation Operation,
        string? Api,
        string? Profile,
        string? Comment,
        IReadOnlyList<GlRegistryReference> References,
        GlRegistryElement Element
    );

    internal sealed record GlRegistryReference(
        GlRegistryReferenceKind Kind,
        string Name,
        string? Comment,
        GlRegistryElement Element
    );

    internal enum GlRequirementOperation
    {
        Require,
        Remove,
    }

    internal enum GlRegistryReferenceKind
    {
        Type,
        Enum,
        Command,
    }
}
