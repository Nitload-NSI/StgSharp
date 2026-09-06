// -----------------------------------------------------------------------------
// file="GlCommandDefinition"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace StgSharp.GenerateGL.Registry
{
    internal sealed record GlCommandBlock(
        string? Namespace,
        IReadOnlyList<GlCommandDefinition> Commands,
        GlRegistryElement Element
    );

    internal sealed record GlCommandDefinition(
        string Name,
        GlReturnDefinition Return,
        IReadOnlyList<GlParameterDefinition> Parameters,
        IReadOnlyList<GlNamedReference> Aliases,
        IReadOnlyList<GlNamedReference> VectorEquivalents,
        IReadOnlyList<GlCommandProtocol> Protocols,
        string? Comment,
        GlRegistryElement Element
    );

    internal sealed record GlReturnDefinition(
        string DeclarationText,
        string? TypeName,
        string? Group,
        string? Kind,
        string? ObjectClass,
        GlRegistryElement Element
    );

    internal sealed record GlParameterDefinition(
        string Name,
        string DeclarationText,
        string? TypeName,
        string? Group,
        string? Kind,
        string? ObjectClass,
        string? Length,
        GlRegistryElement Element
    );

    internal sealed record GlCommandProtocol(
        string Type,
        string Opcode,
        string? Name,
        string? Comment,
        GlRegistryElement Element
    );
}
