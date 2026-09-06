// -----------------------------------------------------------------------------
// file="GlRegistryProjection"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace StgSharp.GenerateGL.Registry
{
    /// <summary>
    ///   Represents one API/profile target selected from the complete registry.
    /// </summary>
    internal sealed record GlRegistryProjection(
        GlRegistryModel Source,
        string Api,
        string Profile,
        string MaximumVersion,
        IReadOnlyList<GlFeatureDefinition> Features,
        IReadOnlyList<GlCommandDefinition> Commands,
        IReadOnlyList<GlEnumDefinition> Enums,
        IReadOnlyDictionary<string, string> CommandIntroducedVersions,
        IReadOnlyDictionary<string, string> EnumIntroducedVersions
    );
}
