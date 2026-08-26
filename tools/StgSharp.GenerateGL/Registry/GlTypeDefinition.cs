// -----------------------------------------------------------------------------
// file="GlTypeDefinition"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.GenerateGL.Registry
{
    internal sealed record GlTypeDefinition(
        string Name,
        string DeclarationText,
        string? Api,
        string? RequiredType,
        string? Category,
        string? Comment,
        GlRegistryElement Element
    );

    internal sealed record GlKindDefinition(
        string Name,
        string Description,
        GlRegistryElement Element
    );
}
