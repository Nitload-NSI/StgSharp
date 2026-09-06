// -----------------------------------------------------------------------------
// file="GlRegistryModel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StgSharp.GenerateGL.Registry
{
    /// <summary>
    ///   Contains the structural OpenGL registry model without managed type mapping.
    /// </summary>
    internal sealed class GlRegistryModel
    {

        public GlRegistryModel(
                               string sourcePath,
                               string? comment,
                               IEnumerable<GlTypeDefinition> types,
                               IEnumerable<GlKindDefinition> kinds,
                               IEnumerable<GlEnumGroup> enumGroups,
                               IEnumerable<GlEnumBlock> enumBlocks,
                               IEnumerable<GlCommandBlock> commandBlocks,
                               IEnumerable<GlFeatureDefinition> features,
                               IEnumerable<GlExtensionDefinition> extensions,
                               GlRegistryElement element
        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);
            ArgumentNullException.ThrowIfNull(types);
            ArgumentNullException.ThrowIfNull(kinds);
            ArgumentNullException.ThrowIfNull(enumGroups);
            ArgumentNullException.ThrowIfNull(enumBlocks);
            ArgumentNullException.ThrowIfNull(commandBlocks);
            ArgumentNullException.ThrowIfNull(features);
            ArgumentNullException.ThrowIfNull(extensions);
            ArgumentNullException.ThrowIfNull(element);

            SourcePath = sourcePath;
            Comment = comment;
            Types = Freeze(types);
            Kinds = Freeze(kinds);
            EnumGroups = Freeze(enumGroups);
            EnumBlocks = Freeze(enumBlocks);
            CommandBlocks = Freeze(commandBlocks);
            Commands = Freeze(CommandBlocks.SelectMany(value => value.Commands));
            Features = Freeze(features);
            Extensions = Freeze(extensions);
            Element = element;

            TypesByName = CreateIndex(Types, value => value.Name);
            KindsByName = CreateIndex(Kinds, value => value.Name);
            EnumGroupsByName = CreateIndex(EnumGroups, value => value.Name);
            EnumsByName = CreateIndex(
                EnumBlocks.SelectMany(value => value.Enums),
                value => value.Name);
            CommandsByName = CreateIndex(Commands, value => value.Name);
            FeaturesByName = CreateIndex(Features, value => value.Name);
            ExtensionsByName = CreateIndex(Extensions, value => value.Name);
        }

        public string SourcePath { get; }

        public string? Comment { get; }

        public IReadOnlyList<GlTypeDefinition> Types { get; }

        public IReadOnlyList<GlKindDefinition> Kinds { get; }

        public IReadOnlyList<GlEnumGroup> EnumGroups { get; }

        public IReadOnlyList<GlEnumBlock> EnumBlocks { get; }

        public IReadOnlyList<GlCommandBlock> CommandBlocks { get; }

        public IReadOnlyList<GlCommandDefinition> Commands { get; }

        public IReadOnlyList<GlFeatureDefinition> Features { get; }

        public IReadOnlyList<GlExtensionDefinition> Extensions { get; }

        public GlRegistryElement Element { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlTypeDefinition>> TypesByName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlKindDefinition>> KindsByName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlEnumGroup>> EnumGroupsByName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlEnumDefinition>> EnumsByName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlCommandDefinition>> CommandsByName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlFeatureDefinition>> FeaturesByName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<GlExtensionDefinition>> ExtensionsByName { get; }

        private static ReadOnlyDictionary<string, IReadOnlyList<T>> CreateIndex<T>(
            IEnumerable<T> values,
            Func<T, string> readName
        )
        {
            Dictionary<string, IReadOnlyList<T>> index = values.GroupBy(
                                                                      readName,
                                                                      StringComparer.Ordinal)
                                                                  .ToDictionary(
                                                                      value => value.Key,
                                                                      value => (IReadOnlyList<T>)Freeze(value),
                                                                      StringComparer.Ordinal);
            return new ReadOnlyDictionary<string, IReadOnlyList<T>>(index);
        }

        private static ReadOnlyCollection<T> Freeze<T>(
                                                        IEnumerable<T> values
        )
        {
            return Array.AsReadOnly(values.ToArray());
        }

    }
}
