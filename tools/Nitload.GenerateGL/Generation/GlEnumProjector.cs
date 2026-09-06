// -----------------------------------------------------------------------------
// file="GlEnumProjector"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using StgSharp.GenerateGL.Naming;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Projects registry GLenum and GLbitfield parameter groups into C# enum types.
    /// </summary>
    internal static class GlEnumProjector
    {
        private static readonly string[] _coreExtensionSuffixes =
        {
            "ARB",
            "EXT",
            "KHR",
            "NV",
        };

        private static readonly ReadOnlyDictionary<string, string> _managedTypeNameOverrides =
            new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["Buffer"] = "ClearBuffer",
                });

        public static IReadOnlyList<ManagedEnumDefinition> Project(
            GlRegistryProjection projection
        )
        {
            ArgumentNullException.ThrowIfNull(projection);

            Dictionary<string, bool> referencedGroups = ReadReferencedGroups(projection.Commands);
            Dictionary<string, GlEnumDefinition> selectedEnums = projection.Enums
                .Where(value => value.Value is not null)
                .GroupBy(value => value.Name, StringComparer.Ordinal)
                .ToDictionary(
                    value => value.Key,
                    value => value.First(),
                    StringComparer.Ordinal);
            Dictionary<string, List<GlEnumDefinition>> membersByGroup =
                new Dictionary<string, List<GlEnumDefinition>>(StringComparer.Ordinal);
            foreach (GlEnumDefinition value in selectedEnums.Values)
            {
                foreach (string group in SplitGroups(value.Group))
                {
                    if (!referencedGroups.ContainsKey(group))
                    {
                        continue;
                    }

                    if (!membersByGroup.TryGetValue(group, out List<GlEnumDefinition>? members))
                    {
                        members = new List<GlEnumDefinition>();
                        membersByGroup.Add(group, members);
                    }

                    members.Add(value);
                }
            }

            Dictionary<string, EnumBuilder> enumsByManagedName =
                new Dictionary<string, EnumBuilder>(StringComparer.Ordinal);
            foreach ((string nativeGroup, bool isFlags) in referencedGroups.OrderBy(
                         value => value.Key,
                         StringComparer.Ordinal))
            {
                if (!membersByGroup.TryGetValue(nativeGroup, out List<GlEnumDefinition>? members) ||
                    members.Count == 0)
                {
                    continue;
                }

                string managedName = ReadManagedTypeName(nativeGroup);
                if (!enumsByManagedName.TryGetValue(managedName, out EnumBuilder? builder))
                {
                    builder = new EnumBuilder(managedName);
                    enumsByManagedName.Add(managedName, builder);
                }

                builder.AddGroup(nativeGroup, isFlags, members);
            }

            return Array.AsReadOnly(
                enumsByManagedName.Values
                    .OrderBy(value => value.Name, StringComparer.Ordinal)
                    .Select(value => value.Build())
                    .ToArray());
        }

        private static Dictionary<string, bool> ReadReferencedGroups(
            IReadOnlyList<GlCommandDefinition> commands
        )
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>(
                StringComparer.Ordinal);
            foreach (GlCommandDefinition command in commands)
            {
                AddReferencedGroups(
                    result,
                    command.Return.Group,
                    command.Return.TypeName);
                foreach (GlParameterDefinition parameter in command.Parameters)
                {
                    AddReferencedGroups(result, parameter.Group, parameter.TypeName);
                }
            }

            return result;
        }

        private static void AddReferencedGroups(
            Dictionary<string, bool> result,
            string? groupNames,
            string? nativeType
        )
        {
            if (groupNames is null || nativeType is not ("GLenum" or "GLbitfield"))
            {
                return;
            }

            bool isFlags = string.Equals(nativeType, "GLbitfield", StringComparison.Ordinal);
            foreach (string group in SplitGroups(groupNames))
            {
                result[group] = result.TryGetValue(group, out bool previous)
                    ? previous || isFlags
                    : isFlags;
            }
        }

        private static IEnumerable<string> SplitGroups(
            string? value
        )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield break;
            }

            foreach (string item in value.Split(','))
            {
                string group = item.Trim();
                if (group.Length != 0)
                {
                    yield return group;
                }
            }
        }

        private static string ReadManagedTypeName(
            string nativeGroup
        )
        {
            if (_managedTypeNameOverrides.TryGetValue(nativeGroup, out string? overrideName))
            {
                return overrideName;
            }

            string value = nativeGroup;
            foreach (string suffix in _coreExtensionSuffixes)
            {
                if (value.Length > suffix.Length && value.EndsWith(
                        suffix,
                        StringComparison.Ordinal))
                {
                    value = value[..^suffix.Length];
                    break;
                }
            }

            return CSharpIdentifierPolicy.NormalizeMemberName(SanitizeIdentifier(value));
        }

        private static string ReadManagedMemberName(
            string nativeName
        )
        {
            string value = nativeName.StartsWith("GL_", StringComparison.Ordinal)
                ? nativeName[3..]
                : nativeName;
            StringBuilder builder = new StringBuilder(value.Length);
            foreach (string component in value.Split('_', StringSplitOptions.RemoveEmptyEntries))
            {
                if (component.Length == 0)
                {
                    continue;
                }

                if (char.IsLetter(component[0]))
                {
                    builder.Append(char.ToUpperInvariant(component[0]));
                    if (component.Length > 1)
                    {
                        foreach (char character in component.AsSpan(1))
                        {
                            builder.Append(ToAsciiLowerInvariant(character));
                        }
                    }
                }
                else
                {
                    builder.Append(component);
                }
            }

            string result = builder.Length == 0 ? "Value" : builder.ToString();
            return CSharpIdentifierPolicy.NormalizeMemberName(SanitizeIdentifier(result));
        }

        private static char ToAsciiLowerInvariant(
            char value
        )
        {
            return value is >= 'A' and <= 'Z' ? (char)(value + ('a' - 'A')) : value;
        }

        private static string SanitizeIdentifier(
            string value
        )
        {
            StringBuilder builder = new StringBuilder(value.Length + 1);
            if (value.Length == 0 || !char.IsLetter(value[0]) && value[0] != '_')
            {
                builder.Append('_');
            }

            foreach (char character in value)
            {
                builder.Append(char.IsLetterOrDigit(character) || character == '_'
                    ? character
                    : '_');
            }

            return builder.ToString();
        }

        private sealed class EnumBuilder
        {
            private readonly Dictionary<string, ManagedEnumMemberDefinition> _members =
                new Dictionary<string, ManagedEnumMemberDefinition>(StringComparer.Ordinal);
            private readonly HashSet<string> _nativeGroups = new HashSet<string>(
                StringComparer.Ordinal);

            public EnumBuilder(
                string name
            )
            {
                Name = name;
            }

            public bool IsFlags { get; private set; }

            public string Name { get; }

            public void AddGroup(
                string nativeGroup,
                bool isFlags,
                IEnumerable<GlEnumDefinition> members
            )
            {
                _nativeGroups.Add(nativeGroup);
                IsFlags |= isFlags;
                foreach (GlEnumDefinition value in members)
                {
                    string name = ReadManagedMemberName(value.Name);
                    if (string.Equals(name, Name, StringComparison.Ordinal))
                    {
                        name = $"{name}Value";
                    }

                    if (_members.TryGetValue(name, out ManagedEnumMemberDefinition? previous))
                    {
                        if (!string.Equals(
                                previous.NativeValue.Value,
                                value.Value,
                                StringComparison.Ordinal))
                        {
                            throw new InvalidDataException(
                                $"Enum member '{Name}.{name}' maps to conflicting native values " +
                                $"'{previous.NativeValue.Name}' and '{value.Name}'.");
                        }

                        continue;
                    }

                    _members.Add(name, new ManagedEnumMemberDefinition(name, value));
                }
            }

            public ManagedEnumDefinition Build()
            {
                List<ManagedEnumMemberDefinition> members = _members.Values
                    .OrderBy(value => value.Name, StringComparer.Ordinal)
                    .ToList();
                if (IsFlags && members.All(value => value.NativeValue.Value != "0" &&
                                                   value.NativeValue.Value != "0x0" &&
                                                   value.NativeValue.Value != "0x00000000"))
                {
                    members.Insert(
                        0,
                        new ManagedEnumMemberDefinition(
                            "None",
                            new GlEnumDefinition(
                                "GL_NONE",
                                "0",
                                null,
                                null,
                                string.Join(",", _nativeGroups),
                                null,
                                "Synthetic zero value for a generated flags enum.",
                                members[0].NativeValue.Element)));
                }

                return new ManagedEnumDefinition(
                    Name,
                    IsFlags,
                    Array.AsReadOnly(
                        _nativeGroups.OrderBy(value => value, StringComparer.Ordinal).ToArray()),
                    Array.AsReadOnly(members.ToArray()));
            }
        }
    }
}
