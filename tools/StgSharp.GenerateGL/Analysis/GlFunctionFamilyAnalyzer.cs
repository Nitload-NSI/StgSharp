// -----------------------------------------------------------------------------
// file="GlFunctionFamilyAnalyzer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StgSharp.GenerateGL.Naming;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Analysis
{
    /// <summary>
    ///   Builds inspectable function-family candidates without changing generation.
    /// </summary>
    internal static class GlFunctionFamilyAnalyzer
    {

        public static GlFunctionFamilyAnalysis Analyze(
            GlRegistryModel registry
        )
        {
            ArgumentNullException.ThrowIfNull(registry);
            return Analyze(registry, registry.Commands);
        }

        public static GlFunctionFamilyAnalysis Analyze(
            GlRegistryProjection projection
        )
        {
            ArgumentNullException.ThrowIfNull(projection);
            return Analyze(projection.Source, projection.Commands);
        }

        private static GlFunctionFamilyAnalysis Analyze(
            GlRegistryModel registry,
            IReadOnlyList<GlCommandDefinition> commands
        )
        {
            Dictionary<string, IReadOnlyList<GlCommandDefinition>> commandsByName = commands
                .GroupBy(value => value.Name, StringComparer.Ordinal)
                .ToDictionary(
                    value => value.Key,
                    value => (IReadOnlyList<GlCommandDefinition>)Array.AsReadOnly(
                        value.ToArray()),
                    StringComparer.Ordinal);

            Dictionary<string, GlFunctionNameShape> shapes =
                GlFunctionNameFactorizer.Factor(registry, commands);
            Dictionary<string, FamilyBuilder> builders = new Dictionary<string, FamilyBuilder>(
                StringComparer.Ordinal);
            List<GlFunctionRelation> relations = new List<GlFunctionRelation>();

            foreach (GlCommandDefinition command in commands)
            {
                if (shapes.TryGetValue(command.Name, out GlFunctionNameShape? shape))
                {
                    ReadBuilder(builders, shape.FamilyName).Add(command, shape);
                }
            }

            AddRegistryRelations(commands, commandsByName, shapes, builders, relations);
            AddSingleValuePointerRelations(commandsByName, shapes, builders, relations);

            GlFunctionFamily[] families = builders.Values
                .Where(value => value.Members.Count > 1 || value.Relations.Count != 0)
                .Select(value => value.Build())
                .OrderBy(value => value.Name, StringComparer.Ordinal)
                .ToArray();
            HashSet<string> classifiedNames = families
                .SelectMany(value => value.Members)
                .Select(value => value.Command.Name)
                .ToHashSet(StringComparer.Ordinal);
            GlCommandDefinition[] unclassified = commands
                .Where(value => !classifiedNames.Contains(value.Name))
                .OrderBy(value => value.Name, StringComparer.Ordinal)
                .ToArray();

            return new GlFunctionFamilyAnalysis(
                Freeze(families),
                Freeze(unclassified),
                Freeze(relations));
        }

        private static void AddRegistryRelations(
            IReadOnlyList<GlCommandDefinition> commands,
            Dictionary<string, IReadOnlyList<GlCommandDefinition>> commandsByName,
            Dictionary<string, GlFunctionNameShape> shapes,
            IDictionary<string, FamilyBuilder> builders,
            List<GlFunctionRelation> relations
        )
        {
            foreach (GlCommandDefinition command in commands)
            {
                foreach (GlNamedReference reference in command.VectorEquivalents)
                {
                    bool exists = commandsByName.TryGetValue(
                        reference.Name,
                        out IReadOnlyList<GlCommandDefinition>? targets);
                    GlFunctionRelation relation = new GlFunctionRelation(
                        command.Name,
                        reference.Name,
                        GlFunctionRelationKind.RegistryVectorEquivalent,
                        exists
                            ? GlFunctionRelationState.Verified
                            : GlFunctionRelationState.MissingTarget,
                        exists
                            ? "declared by gl.xml <vecequiv>"
                            : "gl.xml <vecequiv> target is not registered");
                    relations.Add(relation);

                    string familyName = ReadRelationFamilyName(
                        command.Name,
                        reference.Name,
                        shapes);
                    FamilyBuilder builder = ReadBuilder(builders, familyName);
                    builder.Add(
                        command,
                        shapes.TryGetValue(command.Name, out GlFunctionNameShape? sourceShape)
                            ? sourceShape
                            : null);
                    builder.Relations.Add(relation);
                    if (!exists)
                    {
                        builder.Diagnostics.Add(
                            $"Missing vector-equivalent target '{reference.Name}'.");
                        continue;
                    }

                    foreach (GlCommandDefinition target in targets!)
                    {
                        builder.Add(
                            target,
                            shapes.TryGetValue(target.Name, out GlFunctionNameShape? targetShape)
                                ? targetShape
                                : null);
                    }
                }
            }
        }

        private static void AddSingleValuePointerRelations(
            Dictionary<string, IReadOnlyList<GlCommandDefinition>> commandsByName,
            Dictionary<string, GlFunctionNameShape> shapes,
            IDictionary<string, FamilyBuilder> builders,
            List<GlFunctionRelation> relations
        )
        {
            HashSet<string> existingPairs = relations
                .Select(value => $"{value.SourceName}\0{value.TargetName}")
                .ToHashSet(StringComparer.Ordinal);
            foreach ((string commandName, GlFunctionNameShape pointerShape) in shapes)
            {
                if (!pointerShape.IsVectorForm ||
                    !commandsByName.TryGetValue(
                        commandName,
                        out IReadOnlyList<GlCommandDefinition>? pointerCommands))
                {
                    continue;
                }

                string scalarName = commandName[..^1];
                if (!shapes.TryGetValue(scalarName, out GlFunctionNameShape? scalarShape) ||
                    !commandsByName.TryGetValue(
                        scalarName,
                        out IReadOnlyList<GlCommandDefinition>? scalarCommands))
                {
                    continue;
                }

                foreach (GlCommandDefinition pointer in pointerCommands)
                {
                    if (pointer.Parameters.Count == 0 ||
                        !string.Equals(pointer.Parameters[^1].Length, "1", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    foreach (GlCommandDefinition scalar in scalarCommands)
                    {
                        if (existingPairs.Contains($"{scalar.Name}\0{pointer.Name}"))
                        {
                            continue;
                        }

                        bool valid = IsSingleValuePointerPair(scalar, pointer, out string detail);
                        GlFunctionRelation relation = new GlFunctionRelation(
                            scalar.Name,
                            pointer.Name,
                            GlFunctionRelationKind.InferredSingleValuePointer,
                            valid
                                ? GlFunctionRelationState.Verified
                                : GlFunctionRelationState.Rejected,
                            detail);
                        relations.Add(relation);

                        FamilyBuilder builder = ReadBuilder(builders, pointerShape.FamilyName);
                        builder.Add(scalar, scalarShape);
                        builder.Add(pointer, pointerShape);
                        builder.Relations.Add(relation);
                        if (!valid)
                        {
                            builder.Diagnostics.Add(
                                $"Rejected scalar/pointer pair {scalar.Name} -> {pointer.Name}: " +
                                detail);
                        }
                    }
                }
            }
        }

        private static bool IsSingleValuePointerPair(
            GlCommandDefinition scalar,
            GlCommandDefinition pointer,
            out string detail
        )
        {
            if (!string.Equals(
                    scalar.Return.TypeName,
                    pointer.Return.TypeName,
                    StringComparison.Ordinal) ||
                scalar.Parameters.Count != pointer.Parameters.Count)
            {
                detail = "return type or parameter count differs";
                return false;
            }

            int lastIndex = scalar.Parameters.Count - 1;
            for (int index = 0; index < lastIndex; index++)
            {
                if (!HaveEquivalentParameterTypes(
                        scalar.Parameters[index],
                        pointer.Parameters[index]))
                {
                    detail = $"parameter {index + 1} differs";
                    return false;
                }
            }

            GlParameterDefinition scalarValue = scalar.Parameters[lastIndex];
            GlParameterDefinition pointerValue = pointer.Parameters[lastIndex];
            if (!string.Equals(
                    scalarValue.TypeName,
                    pointerValue.TypeName,
                    StringComparison.Ordinal) ||
                scalarValue.DeclarationText.Contains('*', StringComparison.Ordinal) ||
                !pointerValue.DeclarationText.Contains('*', StringComparison.Ordinal) ||
                !pointerValue.DeclarationText.TrimStart().StartsWith(
                    "const ",
                    StringComparison.Ordinal) ||
                !string.Equals(pointerValue.Length, "1", StringComparison.Ordinal))
            {
                detail = "last parameter is not a matching T / const T* len=1 pair";
                return false;
            }

            detail = "verified T / const T* len=1 pair";
            return true;
        }

        private static bool HaveEquivalentParameterTypes(
            GlParameterDefinition left,
            GlParameterDefinition right
        )
        {
            return string.Equals(left.TypeName, right.TypeName, StringComparison.Ordinal) &&
                   left.DeclarationText.Contains('*', StringComparison.Ordinal) ==
                   right.DeclarationText.Contains('*', StringComparison.Ordinal);
        }

        private static string ReadRelationFamilyName(
            string sourceName,
            string targetName,
            Dictionary<string, GlFunctionNameShape> shapes
        )
        {
            if (shapes.TryGetValue(sourceName, out GlFunctionNameShape? sourceShape))
            {
                return sourceShape.FamilyName;
            }

            if (shapes.TryGetValue(targetName, out GlFunctionNameShape? targetShape))
            {
                return targetShape.FamilyName;
            }

            string source = RemoveGlPrefix(sourceName);
            string target = RemoveGlPrefix(targetName);
            if (target.EndsWith('v') &&
                string.Equals(source, target[..^1], StringComparison.Ordinal))
            {
                return CSharpIdentifierPolicy.NormalizeMemberName(source);
            }

            int length = 0;
            int maximum = Math.Min(source.Length, target.Length);
            while (length < maximum && source[length] == target[length])
            {
                length++;
            }

            string familyName = length == 0 ? source : source[..length].TrimEnd('_');
            return CSharpIdentifierPolicy.NormalizeMemberName(familyName);
        }

        private static string RemoveGlPrefix(
            string name
        )
        {
            return name.StartsWith("gl", StringComparison.Ordinal) && name.Length > 2
                ? name[2..]
                : name;
        }

        private static FamilyBuilder ReadBuilder(
            IDictionary<string, FamilyBuilder> builders,
            string familyName
        )
        {
            if (!builders.TryGetValue(familyName, out FamilyBuilder? builder))
            {
                builder = new FamilyBuilder(familyName);
                builders.Add(familyName, builder);
            }

            return builder;
        }

        private static ReadOnlyCollection<T> Freeze<T>(
            IEnumerable<T> values
        )
        {
            return Array.AsReadOnly(values.ToArray());
        }

        private sealed class FamilyBuilder
        {

            private readonly Dictionary<string, GlFunctionFamilyMember> _members = new Dictionary<string, GlFunctionFamilyMember>(
                StringComparer.Ordinal);

            public FamilyBuilder(
                string name
            )
            {
                Name = name;
            }

            public string Name { get; }

            public Dictionary<string, GlFunctionFamilyMember>.ValueCollection Members =>
                _members.Values;

            public List<GlFunctionRelation> Relations { get; } = new List<GlFunctionRelation>();

            public List<string> Diagnostics { get; } = new List<string>();

            public void Add(
                GlCommandDefinition command,
                GlFunctionNameShape? shape
            )
            {
                _members.TryAdd(
                    command.Name,
                    new GlFunctionFamilyMember(command, shape));
            }

            public GlFunctionFamily Build()
            {
                GlFunctionFamilyMember[] members = _members.Values
                    .OrderBy(value => value.Command.Name, StringComparer.Ordinal)
                    .ToArray();
                string[] diagnostics = Diagnostics
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(value => value, StringComparer.Ordinal)
                    .ToArray();
                return new GlFunctionFamily(
                    Name,
                    Freeze(members),
                    Freeze(Relations.Distinct().ToArray()),
                    Freeze(diagnostics),
                    ReadShapeNamePolicy(members));
            }

            private static GlFunctionShapeNamePolicy ReadShapeNamePolicy(
                IReadOnlyCollection<GlFunctionFamilyMember> members
            )
            {
                GlFunctionFamilyMember[] scalarMembers = members
                    .Where(
                        value => value.NameShape is
                            { IsVectorForm: false, Shape: not null })
                    .ToArray();
                if (scalarMembers.Select(value => value.NameShape!.Shape)
                                 .Distinct(StringComparer.Ordinal)
                                 .Count() < 2)
                {
                    return GlFunctionShapeNamePolicy.NotApplicable;
                }

                bool collision = scalarMembers
                    .GroupBy(ReadNativeSignature, StringComparer.Ordinal)
                    .Any(group => group.Select(value => value.NameShape!.Shape)
                                       .Distinct(StringComparer.Ordinal)
                                       .Count() > 1);
                return collision
                    ? GlFunctionShapeNamePolicy.MustRetain
                    : GlFunctionShapeNamePolicy.CanRemove;
            }

            private static string ReadNativeSignature(
                GlFunctionFamilyMember member
            )
            {
                GlCommandDefinition command = member.Command;
                return $"{command.Return.TypeName}:" + string.Join(
                    ",",
                    command.Parameters.Select(
                        value => $"{value.TypeName}:{value.DeclarationText.Contains('*', StringComparison.Ordinal)}"));
            }

        }

    }
}
