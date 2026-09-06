// -----------------------------------------------------------------------------
// file="GlEnumTypeMap"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Resolves unambiguous scalar GLenum and GLbitfield groups to generated C# types.
    /// </summary>
    internal sealed class GlEnumTypeMap
    {
        private readonly Dictionary<string, string> _typesByNativeGroup;

        public GlEnumTypeMap(
            IEnumerable<ManagedEnumDefinition> definitions
        )
        {
            ArgumentNullException.ThrowIfNull(definitions);

            _typesByNativeGroup = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (ManagedEnumDefinition definition in definitions)
            {
                foreach (string nativeGroup in definition.NativeGroups)
                {
                    if (_typesByNativeGroup.TryGetValue(nativeGroup, out string? previous) &&
                        !string.Equals(previous, definition.Name, StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException(
                            $"Native enum group '{nativeGroup}' maps to both " +
                            $"'{previous}' and '{definition.Name}'.");
                    }

                    _typesByNativeGroup[nativeGroup] = definition.Name;
                }
            }
        }

        public bool TryReadParameterType(
            GlParameterDefinition parameter,
            out string managedType
        )
        {
            ArgumentNullException.ThrowIfNull(parameter);
            if (!IsEnumDeclaration(parameter.TypeName, parameter.Group))
            {
                managedType = string.Empty;
                return false;
            }

            return TryReadGroupType(parameter.Group!, out managedType);
        }

        public bool TryReadReturnType(
            GlReturnDefinition value,
            out string managedType
        )
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!IsEnumDeclaration(value.TypeName, value.Group))
            {
                managedType = string.Empty;
                return false;
            }

            return TryReadGroupType(value.Group!, out managedType);
        }

        private static bool IsEnumDeclaration(
            string? nativeType,
            string? group
        )
        {
            return group is not null &&
                   nativeType is "GLenum" or "GLbitfield";
        }

        private bool TryReadGroupType(
            string groups,
            out string managedType
        )
        {
            string[] nativeGroups = groups.Split(',')
                .Select(value => value.Trim())
                .Where(value => value.Length != 0)
                .ToArray();
            if (nativeGroups.Length == 0)
            {
                managedType = string.Empty;
                return false;
            }

            HashSet<string> candidates = new HashSet<string>(StringComparer.Ordinal);
            foreach (string nativeGroup in nativeGroups)
            {
                if (!_typesByNativeGroup.TryGetValue(nativeGroup, out string? candidate))
                {
                    managedType = string.Empty;
                    return false;
                }

                candidates.Add(candidate);
            }

            if (candidates.Count == 1)
            {
                managedType = candidates.First();
                return true;
            }

            managedType = string.Empty;
            return false;
        }
    }
}
