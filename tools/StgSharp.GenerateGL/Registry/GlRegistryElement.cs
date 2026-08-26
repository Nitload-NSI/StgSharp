// -----------------------------------------------------------------------------
// file="GlRegistryElement"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace StgSharp.GenerateGL.Registry
{
    /// <summary>
    ///   Preserves the XML representation shared by every registry model item.
    /// </summary>
    internal sealed record GlRegistryElement(
        GlSourceLocation Location,
        string RawXml,
        IReadOnlyDictionary<string, string> Attributes
    );

    internal readonly record struct GlSourceLocation(
        int LineNumber,
        int LinePosition
    );

    internal sealed record GlNamedReference(
        string Name,
        GlRegistryElement Element
    );
}
