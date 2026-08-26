// -----------------------------------------------------------------------------
// file="RegistryCli"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL
{
    /// <summary>
    ///   Presents the parsed registry model as the primary CLI surface.
    /// </summary>
    internal static class RegistryCli
    {

        private const int DefaultFindLimit = 10;
        private const int DefaultListLimit = 20;
        private const int QueryErrorExitCode = 1;
        private const int SuccessExitCode = 0;
        private const int UsageErrorExitCode = 2;

        public static int Execute(
                                  GlRegistryModel registry,
                                  string command,
                                  IReadOnlyList<string> arguments
        )
        {
            ArgumentNullException.ThrowIfNull(registry);
            return command switch
            {
                "SUMMARY" or "REGISTRY" or "MODEL" => QuerySummary(
                    registry,
                    arguments),
                "LIST" or "LS" => QueryList(registry, arguments),
                "FIND" or "SEARCH" => QueryFind(registry, arguments),
                "SHOW" or "INFO" => QueryShow(registry, arguments),
                _ => WriteUsageError($"Unknown registry command '{command}'."),
            };
        }

        private static int QuerySummary(
                                        GlRegistryModel registry,
                                        IReadOnlyCollection<string> arguments
        )
        {
            if (!ExpectNoArguments("summary", arguments))
            {
                return UsageErrorExitCode;
            }

            GlRequirementBlock[] requirements = registry.Features.SelectMany(
                                                                  value => value.Requirements)
                                                              .Concat(
                                                                  registry.Extensions.SelectMany(
                                                                      value => value.Requirements))
                                                              .ToArray();
            Console.WriteLine("OpenGL registry");
            WriteProperty("source", registry.SourcePath);
            WriteProperty("types", registry.Types.Count);
            WriteProperty("kinds", registry.Kinds.Count);
            WriteProperty("enum groups", registry.EnumGroups.Count);
            WriteProperty("enum blocks", registry.EnumBlocks.Count);
            WriteProperty(
                "enums",
                registry.EnumBlocks.Sum(value => value.Enums.Count));
            WriteProperty(
                "unused enum ranges",
                registry.EnumBlocks.Sum(value => value.UnusedRanges.Count));
            WriteProperty("commands", registry.Commands.Count);
            WriteProperty(
                "parameters",
                registry.Commands.Sum(value => value.Parameters.Count));
            WriteProperty("features", registry.Features.Count);
            WriteProperty("extensions", registry.Extensions.Count);
            WriteProperty(
                "require blocks",
                requirements.Count(
                    value => value.Operation == GlRequirementOperation.Require));
            WriteProperty(
                "remove blocks",
                requirements.Count(
                    value => value.Operation == GlRequirementOperation.Remove));
            WriteProperty(
                "named references",
                requirements.Sum(value => value.References.Count));
            return SuccessExitCode;
        }

        private static int QueryList(
                                     GlRegistryModel registry,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadListOptions(
                    arguments,
                    out RegistryObjectKind kind,
                    out string? filter,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            RegistrySearchResult[] values = Enumerate(registry, kind)
                                            .Where(
                                                value => filter is null || value.Name.Contains(
                                                    filter,
                                                    StringComparison.OrdinalIgnoreCase))
                                            .ToArray();
            if (values.Length == 0)
            {
                Console.Error.WriteLine(
                    filter is null
                        ? $"No {ReadKindName(kind, true)} are registered."
                        : $"No {ReadKindName(kind, true)} contain '{filter}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(values.Length, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                WriteCompactResult(index + 1, values[index]);
            }

            WriteTruncation(values.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryFind(
                                     GlRegistryModel registry,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadFindOptions(
                    arguments,
                    out string text,
                    out bool exact,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            RegistrySearchResult[] values = Enumerate(registry)
                                            .Where(
                                                value => exact
                                                    ? string.Equals(
                                                        value.Name,
                                                        text,
                                                        StringComparison.OrdinalIgnoreCase)
                                                    : value.Name.Contains(
                                                        text,
                                                        StringComparison.OrdinalIgnoreCase))
                                            .OrderBy(
                                                value => value.Name,
                                                StringComparer.Ordinal)
                                            .ThenBy(value => value.Kind)
                                            .ToArray();
            if (values.Length == 0)
            {
                Console.Error.WriteLine($"No registry names match '{text}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(values.Length, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                WriteCompactResult(index + 1, values[index]);
            }

            WriteTruncation(values.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryShow(
                                     GlRegistryModel registry,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadShowOptions(
                    arguments,
                    out RegistryObjectKind kind,
                    out string name,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            RegistrySearchResult[] values = Enumerate(registry, kind)
                                            .Where(
                                                value => string.Equals(
                                                    value.Name,
                                                    name,
                                                    StringComparison.Ordinal))
                                            .ToArray();
            if (values.Length == 0)
            {
                Console.Error.WriteLine(
                    $"No {ReadKindName(kind, false)} named '{name}' is registered.");
                Console.Error.WriteLine($"Try: find {name}");
                return QueryErrorExitCode;
            }

            for (int index = 0; index < values.Length; index++)
            {
                if (index != 0)
                {
                    Console.WriteLine();
                }

                if (values.Length > 1)
                {
                    Console.WriteLine($"[{index + 1}]");
                }

                WriteDetailedResult(values[index], limit);
            }

            return SuccessExitCode;
        }

        private static IEnumerable<RegistrySearchResult> Enumerate(
                                                                    GlRegistryModel registry
        )
        {
            foreach (RegistryObjectKind kind in Enum.GetValues<RegistryObjectKind>())
            {
                foreach (RegistrySearchResult value in Enumerate(registry, kind))
                {
                    yield return value;
                }
            }
        }

        private static IEnumerable<RegistrySearchResult> Enumerate(
                                                                    GlRegistryModel registry,
                                                                    RegistryObjectKind kind
        )
        {
            return kind switch
            {
                RegistryObjectKind.Type => registry.Types.Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                RegistryObjectKind.Kind => registry.Kinds.Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                RegistryObjectKind.EnumGroup => registry.EnumGroups.Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                RegistryObjectKind.Enum => registry.EnumBlocks.SelectMany(
                    value => value.Enums).Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                RegistryObjectKind.Command => registry.Commands.Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                RegistryObjectKind.Feature => registry.Features.Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                RegistryObjectKind.Extension => registry.Extensions.Select(
                    value => new RegistrySearchResult(
                        kind,
                        value.Name,
                        value,
                        value.Element)),
                _ => throw new ArgumentOutOfRangeException(nameof(kind)),
            };
        }

        private static void WriteCompactResult(
                                               int index,
                                               RegistrySearchResult result
        )
        {
            Console.WriteLine(
                $"[{index}] {ReadKindName(result.Kind, false)}: {result.Name}");
            switch (result.Value)
            {
                case GlTypeDefinition type:
                    WriteProperty("declaration", type.DeclarationText);
                    break;

                case GlKindDefinition kind:
                    WriteProperty("description", kind.Description);
                    break;

                case GlEnumGroup group:
                    WriteProperty("members", group.Members.Count);
                    break;

                case GlEnumDefinition enumValue:
                    WriteOptionalProperty("value", enumValue.Value);
                    WriteOptionalProperty("alias", enumValue.Alias);
                    WriteOptionalProperty("group", enumValue.Group);
                    break;

                case GlCommandDefinition command:
                    WriteProperty("declaration", ReadCommandDeclaration(command));
                    break;

                case GlFeatureDefinition feature:
                    WriteProperty("api", feature.Api);
                    WriteProperty("version", feature.Number);
                    break;

                case GlExtensionDefinition extension:
                    WriteOptionalProperty("supported", extension.SupportedApis);
                    break;
            }

            WriteProperty("line", result.Element.Location.LineNumber);
        }

        private static void WriteDetailedResult(
                                                RegistrySearchResult result,
                                                int referenceLimit
        )
        {
            switch (result.Value)
            {
                case GlTypeDefinition type:
                    WriteType(type);
                    break;

                case GlKindDefinition kind:
                    WriteKind(kind);
                    break;

                case GlEnumGroup group:
                    WriteEnumGroup(group, referenceLimit);
                    break;

                case GlEnumDefinition enumValue:
                    WriteEnum(enumValue);
                    break;

                case GlCommandDefinition command:
                    WriteCommand(command);
                    break;

                case GlFeatureDefinition feature:
                    Console.WriteLine($"feature: {feature.Name}");
                    WriteProperty("api", feature.Api);
                    WriteProperty("version", feature.Number);
                    WriteOptionalProperty("protect", feature.Protect);
                    WriteOptionalProperty("comment", feature.Comment);
                    WriteProperty("line", feature.Element.Location.LineNumber);
                    WriteRequirements(feature.Requirements, referenceLimit);
                    break;

                case GlExtensionDefinition extension:
                    Console.WriteLine($"extension: {extension.Name}");
                    WriteOptionalProperty("supported", extension.SupportedApis);
                    WriteOptionalProperty("protect", extension.Protect);
                    WriteOptionalProperty("comment", extension.Comment);
                    WriteProperty("line", extension.Element.Location.LineNumber);
                    WriteRequirements(extension.Requirements, referenceLimit);
                    break;
            }
        }

        private static void WriteType(
                                      GlTypeDefinition value
        )
        {
            Console.WriteLine($"type: {value.Name}");
            WriteProperty("declaration", value.DeclarationText);
            WriteOptionalProperty("api", value.Api);
            WriteOptionalProperty("requires", value.RequiredType);
            WriteOptionalProperty("category", value.Category);
            WriteOptionalProperty("comment", value.Comment);
            WriteProperty("line", value.Element.Location.LineNumber);
        }

        private static void WriteKind(
                                      GlKindDefinition value
        )
        {
            Console.WriteLine($"kind: {value.Name}");
            WriteProperty("description", value.Description);
            WriteProperty("line", value.Element.Location.LineNumber);
        }

        private static void WriteEnumGroup(
                                           GlEnumGroup value,
                                           int limit
        )
        {
            Console.WriteLine($"enum group: {value.Name}");
            WriteOptionalProperty("comment", value.Comment);
            WriteProperty("line", value.Element.Location.LineNumber);
            WriteProperty("members", value.Members.Count);
            int displayedCount = Math.Min(value.Members.Count, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                Console.WriteLine($"    [{index + 1}] {value.Members[index].Name}");
            }

            WriteIndentedTruncation(value.Members.Count, displayedCount, 4);
        }

        private static void WriteEnum(
                                      GlEnumDefinition value
        )
        {
            Console.WriteLine($"enum: {value.Name}");
            WriteOptionalProperty("value", value.Value);
            WriteOptionalProperty("api", value.Api);
            WriteOptionalProperty("type suffix", value.TypeSuffix);
            WriteOptionalProperty("group", value.Group);
            WriteOptionalProperty("alias", value.Alias);
            WriteOptionalProperty("comment", value.Comment);
            WriteProperty("line", value.Element.Location.LineNumber);
        }

        private static void WriteCommand(
                                         GlCommandDefinition value
        )
        {
            Console.WriteLine($"command: {value.Name}");
            WriteProperty("declaration", ReadCommandDeclaration(value));
            WriteProperty("line", value.Element.Location.LineNumber);
            WriteProperty("return declaration", value.Return.DeclarationText);
            WriteOptionalProperty("return type", value.Return.TypeName);
            WriteOptionalProperty("return group", value.Return.Group);
            WriteOptionalProperty("return kind", value.Return.Kind);
            WriteOptionalProperty("return class", value.Return.ObjectClass);
            WriteOptionalProperty("comment", value.Comment);
            WriteProperty("parameters", value.Parameters.Count);
            for (int index = 0; index < value.Parameters.Count; index++)
            {
                GlParameterDefinition parameter = value.Parameters[index];
                Console.WriteLine($"    [{index + 1}] {parameter.Name}");
                WriteIndentedProperty("declaration", parameter.DeclarationText, 6);
                WriteIndentedOptionalProperty("type", parameter.TypeName, 6);
                WriteIndentedOptionalProperty("group", parameter.Group, 6);
                WriteIndentedOptionalProperty("kind", parameter.Kind, 6);
                WriteIndentedOptionalProperty("class", parameter.ObjectClass, 6);
                WriteIndentedOptionalProperty("length", parameter.Length, 6);
                WriteIndentedProperty("line", parameter.Element.Location.LineNumber, 6);
            }

            WriteNamedReferences("aliases", value.Aliases);
            WriteNamedReferences("vector equivalents", value.VectorEquivalents);
            if (value.Protocols.Count != 0)
            {
                WriteProperty("protocols", value.Protocols.Count);
                foreach (GlCommandProtocol protocol in value.Protocols)
                {
                    Console.WriteLine($"    {protocol.Type}: {protocol.Opcode}");
                    WriteIndentedOptionalProperty("name", protocol.Name, 6);
                    WriteIndentedOptionalProperty("comment", protocol.Comment, 6);
                }
            }
        }

        private static void WriteRequirements(
                                              IReadOnlyList<GlRequirementBlock> requirements,
                                              int referenceLimit
        )
        {
            WriteProperty("requirement blocks", requirements.Count);
            for (int index = 0; index < requirements.Count; index++)
            {
                GlRequirementBlock block = requirements[index];
                Console.WriteLine(
                    $"    [{index + 1}] " +
                    $"{(block.Operation == GlRequirementOperation.Require ? "require" : "remove")}");
                WriteIndentedOptionalProperty("api", block.Api, 6);
                WriteIndentedOptionalProperty("profile", block.Profile, 6);
                WriteIndentedOptionalProperty("comment", block.Comment, 6);
                WriteIndentedProperty("references", block.References.Count, 6);
                int displayedCount = Math.Min(block.References.Count, referenceLimit);
                for (int referenceIndex = 0;
                     referenceIndex < displayedCount;
                     referenceIndex++)
                {
                    GlRegistryReference reference = block.References[referenceIndex];
                    Console.WriteLine(
                        $"        {ReadReferenceKind(reference.Kind)}: {reference.Name}");
                }

                WriteIndentedTruncation(block.References.Count, displayedCount, 8);
            }
        }

        private static void WriteNamedReferences(
                                                 string name,
                                                 IReadOnlyList<GlNamedReference> values
        )
        {
            if (values.Count == 0)
            {
                return;
            }

            WriteProperty(name, values.Count);
            foreach (GlNamedReference value in values)
            {
                Console.WriteLine($"    - {value.Name}");
            }
        }

        private static string ReadCommandDeclaration(
                                                     GlCommandDefinition command
        )
        {
            string returnType = command.Return.DeclarationText.Replace(
                command.Name,
                string.Empty,
                StringComparison.Ordinal).Trim();
            return $"{returnType} {command.Name}(" +
                   string.Join(", ", command.Parameters.Select(value => value.DeclarationText)) +
                   ")";
        }

        private static bool TryReadListOptions(
                                               IReadOnlyList<string> arguments,
                                               out RegistryObjectKind kind,
                                               out string? filter,
                                               out int limit,
                                               out string? error
        )
        {
            kind = default;
            if (arguments.Count == 0 || !TryReadKind(arguments[0], out kind))
            {
                filter = null;
                limit = 0;
                error = "list requires a registry category: types, kinds, groups, enums, " +
                        "commands, features, or extensions.";
                return false;
            }

            filter = null;
            limit = DefaultListLimit;
            int index = 1;
            if (index < arguments.Count && !arguments[index].StartsWith(
                    "--",
                    StringComparison.Ordinal))
            {
                filter = arguments[index++];
            }

            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if (argument == "--limit" && TryReadPositiveInt(
                        arguments,
                        ref index,
                        out limit))
                {
                    continue;
                }

                error = $"Unknown or invalid list option '{argument}'.";
                return false;
            }

            error = null;
            return true;
        }

        private static bool TryReadFindOptions(
                                               IReadOnlyList<string> arguments,
                                               out string text,
                                               out bool exact,
                                               out int limit,
                                               out string? error
        )
        {
            if (arguments.Count == 0 || arguments[0].StartsWith(
                    "--",
                    StringComparison.Ordinal))
            {
                text = string.Empty;
                exact = false;
                limit = 0;
                error = "find requires a registry name or partial name.";
                return false;
            }

            text = arguments[0];
            exact = false;
            limit = DefaultFindLimit;
            int index = 1;
            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if (argument == "--exact")
                {
                    exact = true;
                    continue;
                }

                if (argument == "--limit" && TryReadPositiveInt(
                        arguments,
                        ref index,
                        out limit))
                {
                    continue;
                }

                error = $"Unknown or invalid find option '{argument}'.";
                return false;
            }

            error = null;
            return true;
        }

        private static bool TryReadShowOptions(
                                               IReadOnlyList<string> arguments,
                                               out RegistryObjectKind kind,
                                               out string name,
                                               out int limit,
                                               out string? error
        )
        {
            kind = default;
            if (arguments.Count < 2 || !TryReadKind(arguments[0], out kind))
            {
                name = string.Empty;
                limit = 0;
                error = "show requires a registry category and exact name.";
                return false;
            }

            name = arguments[1];
            limit = DefaultListLimit;
            int index = 2;
            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if (argument == "--limit" && TryReadPositiveInt(
                        arguments,
                        ref index,
                        out limit))
                {
                    continue;
                }

                error = $"Unknown or invalid show option '{argument}'.";
                return false;
            }

            error = null;
            return true;
        }

        private static bool TryReadKind(
                                        string value,
                                        out RegistryObjectKind kind
        )
        {
            kind = value.ToUpperInvariant() switch
            {
                "TYPE" or "TYPES" => RegistryObjectKind.Type,
                "KIND" or "KINDS" => RegistryObjectKind.Kind,
                "GROUP" or "GROUPS" or "ENUMGROUP" or "ENUMGROUPS" =>
                    RegistryObjectKind.EnumGroup,
                "ENUM" or "ENUMS" => RegistryObjectKind.Enum,
                "COMMAND" or "COMMANDS" or "FUNCTION" or "FUNCTIONS" =>
                    RegistryObjectKind.Command,
                "FEATURE" or "FEATURES" or "VERSION" or "VERSIONS" =>
                    RegistryObjectKind.Feature,
                "EXTENSION" or "EXTENSIONS" => RegistryObjectKind.Extension,
                _ => default,
            };
            return value.ToUpperInvariant() is
                "TYPE" or "TYPES" or
                "KIND" or "KINDS" or
                "GROUP" or "GROUPS" or "ENUMGROUP" or "ENUMGROUPS" or
                "ENUM" or "ENUMS" or
                "COMMAND" or "COMMANDS" or "FUNCTION" or "FUNCTIONS" or
                "FEATURE" or "FEATURES" or "VERSION" or "VERSIONS" or
                "EXTENSION" or "EXTENSIONS";
        }

        private static string ReadKindName(
                                           RegistryObjectKind kind,
                                           bool plural
        )
        {
            string name = kind switch
            {
                RegistryObjectKind.Type => "type",
                RegistryObjectKind.Kind => "kind",
                RegistryObjectKind.EnumGroup => "enum group",
                RegistryObjectKind.Enum => "enum",
                RegistryObjectKind.Command => "command",
                RegistryObjectKind.Feature => "feature",
                RegistryObjectKind.Extension => "extension",
                _ => throw new ArgumentOutOfRangeException(nameof(kind)),
            };
            return plural ? $"{name}s" : name;
        }

        private static string ReadReferenceKind(
                                                GlRegistryReferenceKind kind
        )
        {
            return kind switch
            {
                GlRegistryReferenceKind.Type => "type",
                GlRegistryReferenceKind.Enum => "enum",
                GlRegistryReferenceKind.Command => "command",
                _ => throw new ArgumentOutOfRangeException(nameof(kind)),
            };
        }

        private static bool TryReadPositiveInt(
                                               IReadOnlyList<string> arguments,
                                               ref int index,
                                               out int value
        )
        {
            if (index >= arguments.Count)
            {
                value = 0;
                return false;
            }

            return int.TryParse(
                       arguments[index++],
                       NumberStyles.None,
                       CultureInfo.InvariantCulture,
                       out value) && value > 0;
        }

        private static bool ExpectNoArguments(
                                              string command,
                                              IReadOnlyCollection<string> arguments
        )
        {
            if (arguments.Count == 0)
            {
                return true;
            }

            Console.Error.WriteLine($"error: {command} does not accept arguments.");
            return false;
        }

        private static int WriteUsageError(
                                           string? error
        )
        {
            Console.Error.WriteLine($"error: {error}");
            return UsageErrorExitCode;
        }

        private static void WriteTruncation(
                                            int totalCount,
                                            int displayedCount
        )
        {
            if (displayedCount != totalCount)
            {
                Console.WriteLine($"... {totalCount - displayedCount} more result(s)");
            }
        }

        private static void WriteIndentedTruncation(
                                                    int totalCount,
                                                    int displayedCount,
                                                    int indentation
        )
        {
            if (displayedCount != totalCount)
            {
                Console.WriteLine(
                    $"{new string(' ', indentation)}... " +
                    $"{totalCount - displayedCount} more item(s)");
            }
        }

        private static void WriteProperty(
                                          string name,
                                          object value
        )
        {
            WriteIndentedProperty(name, value, 2);
        }

        private static void WriteOptionalProperty(
                                                  string name,
                                                  string? value
        )
        {
            WriteIndentedOptionalProperty(name, value, 2);
        }

        private static void WriteIndentedProperty(
                                                  string name,
                                                  object value,
                                                  int indentation
        )
        {
            Console.WriteLine($"{new string(' ', indentation)}{name}: {value}");
        }

        private static void WriteIndentedOptionalProperty(
                                                          string name,
                                                          string? value,
                                                          int indentation
        )
        {
            if (value is not null)
            {
                WriteIndentedProperty(name, value, indentation);
            }
        }

        private sealed record RegistrySearchResult(
            RegistryObjectKind Kind,
            string Name,
            object Value,
            GlRegistryElement Element
        );

        private enum RegistryObjectKind
        {
            Type,
            Kind,
            EnumGroup,
            Enum,
            Command,
            Feature,
            Extension,
        }

    }
}
