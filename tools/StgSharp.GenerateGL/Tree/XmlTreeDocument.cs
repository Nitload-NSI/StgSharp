// -----------------------------------------------------------------------------
// file="XmlTreeDocument"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Xml.Linq;

namespace StgSharp.GenerateGL.Tree
{
    /// <summary>
    ///   Holds the original XML text alongside its parsed tree.
    /// </summary>
    internal sealed record XmlTreeDocument(
        string SourcePath,
        string SourceText,
        XDocument Tree
    );
}
