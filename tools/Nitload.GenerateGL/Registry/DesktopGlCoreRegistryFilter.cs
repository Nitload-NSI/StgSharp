// -----------------------------------------------------------------------------
// file="DesktopGlCoreRegistryFilter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace StgSharp.GenerateGL.Registry
{
    /// <summary>
    ///   Selects the active desktop OpenGL core API without extension requirements.
    /// </summary>
    internal static class DesktopGlCoreRegistryFilter
    {

        public const string ApiName = "gl";
        public const string CoreProfile = "core";
        public const string LatestVersion = "4.6";

        public static GlRegistryProjection Apply(
            GlRegistryModel registry,
            string maximumVersion = LatestVersion
        )
        {
            ArgumentNullException.ThrowIfNull(registry);
            Version maximum = ReadVersion(maximumVersion);

            GlFeatureDefinition[] features = registry.Features
                .Where(
                    value => string.Equals(value.Api, ApiName, StringComparison.Ordinal) &&
                             ReadVersion(value.Number) <= maximum)
                .ToArray();
            Dictionary<string, GlCommandDefinition> commandDefinitions = ReadCommandDefinitions(
                registry);
            Dictionary<string, GlEnumDefinition> enumDefinitions = ReadEnumDefinitions(registry);
            SymbolSelection commands = new SymbolSelection();
            SymbolSelection enums = new SymbolSelection();

            foreach (GlFeatureDefinition feature in features)
            {
                foreach (GlRequirementBlock block in feature.Requirements.Where(AppliesToTarget))
                {
                    foreach (GlRegistryReference reference in block.References)
                    {
                        SymbolSelection? selection = reference.Kind switch
                        {
                            GlRegistryReferenceKind.Command => commands,
                            GlRegistryReferenceKind.Enum => enums,
                            GlRegistryReferenceKind.Type => null,
                            _ => throw new InvalidDataException(
                                $"Unknown registry reference kind '{reference.Kind}'."),
                        };
                        if (selection is null)
                        {
                            continue;
                        }

                        if (block.Operation == GlRequirementOperation.Require)
                        {
                            selection.Require(reference.Name, feature.Number);
                        }
                        else
                        {
                            selection.Remove(reference.Name);
                        }
                    }
                }
            }

            GlCommandDefinition[] selectedCommands = commands.ReadActiveNames()
                .Select(
                    name => ReadDefinition(
                        commandDefinitions,
                        name,
                        "command"))
                .ToArray();
            GlEnumDefinition[] selectedEnums = enums.ReadActiveNames()
                .Select(
                    name => ReadDefinition(
                        enumDefinitions,
                        name,
                        "enum"))
                .ToArray();

            return new GlRegistryProjection(
                registry,
                ApiName,
                CoreProfile,
                maximumVersion,
                Freeze(features),
                Freeze(selectedCommands),
                Freeze(selectedEnums),
                commands.FreezeIntroducedVersions(),
                enums.FreezeIntroducedVersions());
        }

        private static bool AppliesToTarget(
            GlRequirementBlock block
        )
        {
            bool matchesApi = block.Api is null || string.Equals(
                block.Api,
                ApiName,
                StringComparison.Ordinal);
            bool matchesProfile = block.Profile is null || string.Equals(
                block.Profile,
                CoreProfile,
                StringComparison.Ordinal);
            return matchesApi && matchesProfile;
        }

        private static Dictionary<string, GlCommandDefinition> ReadCommandDefinitions(
            GlRegistryModel registry
        )
        {
            Dictionary<string, GlCommandDefinition> result = new Dictionary<string, GlCommandDefinition>(
                StringComparer.Ordinal);
            foreach (GlCommandDefinition command in registry.Commands)
            {
                if (!result.TryAdd(command.Name, command))
                {
                    throw new InvalidDataException(
                        $"Command '{command.Name}' has more than one definition.");
                }
            }

            return result;
        }

        private static Dictionary<string, GlEnumDefinition> ReadEnumDefinitions(
            GlRegistryModel registry
        )
        {
            Dictionary<string, GlEnumDefinition> result = new Dictionary<string, GlEnumDefinition>(
                StringComparer.Ordinal);
            foreach (GlEnumDefinition value in registry.EnumBlocks.SelectMany(
                         block => block.Enums).Where(IsDesktopDefinition))
            {
                if (!result.TryGetValue(value.Name, out GlEnumDefinition? previous))
                {
                    result.Add(value.Name, value);
                    continue;
                }

                if (previous.Value is not null && value.Value is not null && !string.Equals(
                        previous.Value,
                        value.Value,
                        StringComparison.Ordinal))
                {
                    throw new InvalidDataException(
                        $"Enum '{value.Name}' has conflicting desktop values " +
                        $"'{previous.Value}' and '{value.Value}'.");
                }

                if (previous.Value is null && value.Value is not null)
                {
                    result[value.Name] = value;
                }
            }

            return result;
        }

        private static bool IsDesktopDefinition(
            GlEnumDefinition value
        )
        {
            return value.Api is null || string.Equals(
                value.Api,
                ApiName,
                StringComparison.Ordinal);
        }

        private static T ReadDefinition<T>(
            IReadOnlyDictionary<string, T> definitions,
            string name,
            string kind
        )
        {
            if (!definitions.TryGetValue(name, out T? definition))
            {
                throw new InvalidDataException(
                    $"Desktop OpenGL core references undefined {kind} '{name}'.");
            }

            return definition;
        }

        private static Version ReadVersion(
            string value
        )
        {
            if (!Version.TryParse(value, out Version? version))
            {
                throw new InvalidDataException(
                    $"OpenGL feature version '{value}' is not a valid version.");
            }

            return version;
        }

        private static ReadOnlyCollection<T> Freeze<T>(
            IEnumerable<T> values
        )
        {
            return Array.AsReadOnly(values.ToArray());
        }

        private sealed class SymbolSelection
        {

            private readonly HashSet<string> _active = new HashSet<string>(StringComparer.Ordinal);
            private readonly Dictionary<string, string> _introducedVersions = new Dictionary<string, string>(
                StringComparer.Ordinal);
            private readonly List<string> _order = new List<string>();

            public void Require(
                string name,
                string version
            )
            {
                _active.Add(name);
                if (_introducedVersions.TryAdd(name, version))
                {
                    _order.Add(name);
                }
            }

            public void Remove(
                string name
            )
            {
                _active.Remove(name);
            }

            public IEnumerable<string> ReadActiveNames()
            {
                return _order.Where(_active.Contains);
            }

            public ReadOnlyDictionary<string, string> FreezeIntroducedVersions()
            {
                return new ReadOnlyDictionary<string, string>(
                    new Dictionary<string, string>(
                        _introducedVersions,
                        StringComparer.Ordinal));
            }

        }

    }
}
