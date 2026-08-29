// -----------------------------------------------------------------------------
// file="GlFunctionNameFactorizer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using StgSharp.GenerateGL.Naming;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Analysis
{
    /// <summary>
    ///   Factors command-name alternatives by their common prefix and interprets only the
    ///   remaining suffix as overload axes.
    /// </summary>
    internal static class GlFunctionNameFactorizer
    {

        private static readonly string[] _typeSuffixes =
        {
            "ui64",
            "i64",
            "ui",
            "ub",
            "us",
            "b",
            "s",
            "i",
            "f",
            "d",
            "h",
            "x",
        };

        public static Dictionary<string, GlFunctionNameShape> Factor(
            GlRegistryModel registry,
            IReadOnlyList<GlCommandDefinition> commands
        )
        {
            ArgumentNullException.ThrowIfNull(registry);
            ArgumentNullException.ThrowIfNull(commands);

            string[] vendorSuffixes = ReadVendorSuffixes(registry);
            NameEntry[] entries = commands
                .Select(
                    command => new NameEntry(
                        command,
                        command.Name,
                        ReadCoreName(command.Name, vendorSuffixes)))
                .ToArray();
            TrieNode root = new TrieNode(string.Empty);
            for (int index = 0; index < entries.Length; index++)
            {
                Add(root, entries[index].CoreName, index);
            }

            List<FactorCandidate>[] candidatesByEntry = Enumerable.Range(0, entries.Length)
                .Select(_ => new List<FactorCandidate>())
                .ToArray();
            CollectCandidates(root, entries, candidatesByEntry);

            Dictionary<string, GlFunctionNameShape> result = new Dictionary<string, GlFunctionNameShape>(
                StringComparer.Ordinal);
            for (int index = 0; index < entries.Length; index++)
            {
                FactorCandidate? candidate = candidatesByEntry[index]
                    .OrderByDescending(value => value.DistinctVariantCount)
                    .ThenByDescending(value => value.MemberCount)
                    .ThenBy(value => value.FamilyName.Length)
                    .FirstOrDefault();
                if (candidate is null ||
                    !candidate.Shapes.TryGetValue(index, out GlFunctionNameShape? shape))
                {
                    continue;
                }

                result.TryAdd(entries[index].CommandName, shape);
            }

            AddFixedVectorPairs(result, commands);
            return result;
        }

        private static void AddFixedVectorPairs(
            Dictionary<string, GlFunctionNameShape> shapes,
            IReadOnlyList<GlCommandDefinition> commands
        )
        {
            Dictionary<string, GlCommandDefinition> commandsByName = commands.ToDictionary(
                value => value.Name,
                StringComparer.Ordinal);
            foreach (GlCommandDefinition vector in commands)
            {
                if (!vector.Name.EndsWith('v') ||
                    shapes.ContainsKey(vector.Name) ||
                    vector.Parameters.Count == 0 ||
                    !int.TryParse(vector.Parameters[^1].Length, out int componentCount) ||
                    componentCount < 2)
                {
                    continue;
                }

                string scalarName = vector.Name[..^1];
                if (shapes.ContainsKey(scalarName) ||
                    !commandsByName.TryGetValue(
                        scalarName,
                        out GlCommandDefinition? scalar) ||
                    !IsFixedVectorPair(scalar, vector, componentCount))
                {
                    continue;
                }

                string familyName = CSharpIdentifierPolicy.NormalizeMemberName(
                    scalarName.StartsWith("gl", StringComparison.Ordinal) &&
                    scalarName.Length > 2
                        ? scalarName[2..]
                        : scalarName);
                shapes.Add(
                    scalarName,
                    new GlFunctionNameShape(
                        familyName,
                        null,
                        null,
                        null,
                        string.Empty,
                        false));
                shapes.Add(
                    vector.Name,
                    new GlFunctionNameShape(
                        familyName,
                        null,
                        null,
                        null,
                        string.Empty,
                        true));
            }
        }

        private static bool IsFixedVectorPair(
            GlCommandDefinition scalar,
            GlCommandDefinition vector,
            int componentCount
        )
        {
            if (!string.Equals(
                    scalar.Return.TypeName,
                    vector.Return.TypeName,
                    StringComparison.Ordinal) ||
                scalar.Parameters.Count != vector.Parameters.Count - 1 + componentCount)
            {
                return false;
            }

            int prefixCount = vector.Parameters.Count - 1;
            for (int index = 0; index < prefixCount; index++)
            {
                if (!HaveEquivalentScalarTypes(
                        scalar.Parameters[index].TypeName,
                        vector.Parameters[index].TypeName) ||
                    scalar.Parameters[index].DeclarationText.Contains('*', StringComparison.Ordinal) !=
                    vector.Parameters[index].DeclarationText.Contains('*', StringComparison.Ordinal))
                {
                    return false;
                }
            }

            GlParameterDefinition vectorValue = vector.Parameters[^1];
            if (!vectorValue.DeclarationText.Contains('*', StringComparison.Ordinal))
            {
                return false;
            }

            for (int index = prefixCount; index < scalar.Parameters.Count; index++)
            {
                if (!HaveEquivalentScalarTypes(
                        scalar.Parameters[index].TypeName,
                        vectorValue.TypeName) ||
                    scalar.Parameters[index].DeclarationText.Contains('*', StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool HaveEquivalentScalarTypes(
            string? left,
            string? right
        )
        {
            if (string.Equals(left, right, StringComparison.Ordinal))
            {
                return true;
            }

            return left is "GLint" or "GLsizei" &&
                   right is "GLint" or "GLsizei";
        }

        private static List<int> CollectCandidates(
            TrieNode node,
            IReadOnlyList<NameEntry> entries,
            IReadOnlyList<List<FactorCandidate>> candidatesByEntry
        )
        {
            List<int> descendants = new List<int>(node.Terminals);
            foreach (TrieNode child in node.Children.Values)
            {
                descendants.AddRange(CollectCandidates(child, entries, candidatesByEntry));
            }

            if (node.Prefix.Length == 0 || descendants.Count < 2)
            {
                return descendants;
            }

            Dictionary<int, GlFunctionNameShape> shapes = new Dictionary<int, GlFunctionNameShape>();
            foreach (int index in descendants)
            {
                string coreName = entries[index].CoreName;
                if (coreName.Length <= node.Prefix.Length ||
                    !TryParseVariantSuffix(
                        coreName[node.Prefix.Length..],
                        node.Prefix,
                        entries[index].Command,
                        out GlFunctionNameShape? shape))
                {
                    continue;
                }

                shapes.Add(index, shape!);
            }

            int distinctVariantCount = shapes.Keys
                .Select(index => entries[index].CoreName)
                .Distinct(StringComparer.Ordinal)
                .Count();
            if (distinctVariantCount < 2)
            {
                return descendants;
            }

            FactorCandidate candidate = new FactorCandidate(
                node.Prefix,
                shapes,
                distinctVariantCount);
            foreach (int index in shapes.Keys)
            {
                candidatesByEntry[index].Add(candidate);
            }

            return descendants;
        }

        private static bool TryParseVariantSuffix(
            string suffix,
            string familyName,
            GlCommandDefinition command,
            out GlFunctionNameShape? shape
        )
        {
            bool isVectorForm = suffix.EndsWith('v');
            if (isVectorForm)
            {
                suffix = suffix[..^1];
            }

            string? typeSuffix = null;
            foreach (string candidate in _typeSuffixes)
            {
                if (suffix.EndsWith(candidate, StringComparison.Ordinal))
                {
                    typeSuffix = candidate;
                    suffix = suffix[..^candidate.Length];
                    break;
                }
            }

            if (typeSuffix is null ||
                !IsTypeSuffixConsistent(command, typeSuffix) ||
                !TryParseShape(
                    suffix,
                    out string? shapeText,
                    out int? columns,
                    out int? rows))
            {
                shape = null;
                return false;
            }

            shape = new GlFunctionNameShape(
                CSharpIdentifierPolicy.NormalizeMemberName(familyName),
                shapeText,
                columns,
                rows,
                typeSuffix,
                isVectorForm);
            return true;
        }

        private static bool IsTypeSuffixConsistent(
            GlCommandDefinition command,
            string typeSuffix
        )
        {
            string? glType = typeSuffix switch
            {
                "b" => "GLbyte",
                "ub" => "GLubyte",
                "s" => "GLshort",
                "us" => "GLushort",
                "i" => "GLint",
                "ui" => "GLuint",
                "i64" => "GLint64",
                "ui64" => "GLuint64",
                "f" => "GLfloat",
                "d" => "GLdouble",
                "h" => "GLhalf",
                "x" => "GLfixed",
                _ => null,
            };
            if (glType is null)
            {
                return false;
            }

            return string.Equals(command.Return.TypeName, glType, StringComparison.Ordinal) ||
                   command.Parameters.Any(
                       value => string.Equals(
                           value.TypeName,
                           glType,
                           StringComparison.Ordinal));
        }

        private static bool TryParseShape(
            string value,
            out string? shape,
            out int? columns,
            out int? rows
        )
        {
            if (value.Length == 0)
            {
                shape = null;
                columns = null;
                rows = null;
                return true;
            }

            if (value.Length == 1 && value[0] is >= '1' and <= '4')
            {
                shape = value;
                columns = value[0] - '0';
                rows = 1;
                return true;
            }

            if (value.Length == 3 &&
                value[0] is >= '2' and <= '4' &&
                value[1] == 'x' &&
                value[2] is >= '2' and <= '4')
            {
                shape = value;
                columns = value[0] - '0';
                rows = value[2] - '0';
                return true;
            }

            shape = null;
            columns = null;
            rows = null;
            return false;
        }

        private static void Add(
            TrieNode root,
            string value,
            int entryIndex
        )
        {
            TrieNode current = root;
            foreach (char character in value)
            {
                if (!current.Children.TryGetValue(character, out TrieNode? child))
                {
                    child = new TrieNode(current.Prefix + character);
                    current.Children.Add(character, child);
                }

                current = child;
            }

            current.Terminals.Add(entryIndex);
        }

        private static string ReadCoreName(
            string commandName,
            IReadOnlyList<string> vendorSuffixes
        )
        {
            string name = commandName.StartsWith("gl", StringComparison.Ordinal) &&
                          commandName.Length > 2
                ? commandName[2..]
                : commandName;
            foreach (string vendorSuffix in vendorSuffixes)
            {
                if (name.Length > vendorSuffix.Length && name.EndsWith(
                        vendorSuffix,
                        StringComparison.Ordinal))
                {
                    return name[..^vendorSuffix.Length];
                }
            }

            return name;
        }

        private static string[] ReadVendorSuffixes(
            GlRegistryModel registry
        )
        {
            return registry.Extensions
                .Select(value => value.Name.Split('_', StringSplitOptions.RemoveEmptyEntries))
                .Where(value => value.Length > 2 && value[0] == "GL")
                .Select(value => value[1])
                .Where(value => value.All(character => char.IsUpper(character) || char.IsDigit(character)))
                .Distinct(StringComparer.Ordinal)
                .OrderByDescending(value => value.Length)
                .ThenBy(value => value, StringComparer.Ordinal)
                .ToArray();
        }

        private sealed record NameEntry(
            GlCommandDefinition Command,
            string CommandName,
            string CoreName
        );

        private sealed record FactorCandidate(
            string FamilyName,
            IReadOnlyDictionary<int, GlFunctionNameShape> Shapes,
            int DistinctVariantCount
        )
        {
            public int MemberCount => Shapes.Count;
        }

        private sealed class TrieNode
        {

            public TrieNode(
                string prefix
            )
            {
                Prefix = prefix;
            }

            public string Prefix { get; }

            public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();

            public List<int> Terminals { get; } = new List<int>();

        }

    }
}
