// -----------------------------------------------------------------------------
// file="GlManagedApiSuppressionFilter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Omits raw managed forwards for commands that have safer handwritten public APIs.
    /// </summary>
    internal sealed class GlManagedApiSuppressionFilter : IGlApiFilter
    {

        private static readonly HashSet<string> _suppressedCommands = new HashSet<string>(
            new[]
            {
                "glCreateShader",
                "glShaderSource",
            },
            StringComparer.Ordinal);

        public IReadOnlyList<ManagedMethodDefinition> Apply(
            GlCommandDefinition command,
            IReadOnlyList<ManagedMethodDefinition> methods,
            GlEquivalentTypeMapper typeMapper
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(methods);
            ArgumentNullException.ThrowIfNull(typeMapper);

            return _suppressedCommands.Contains(command.Name)
                ? Array.Empty<ManagedMethodDefinition>()
                : methods;
        }

    }
}
