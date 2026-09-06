// -----------------------------------------------------------------------------
// file="GeneratedFileSet"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StgSharp.GenerateGL.Generation
{
    internal sealed record GeneratedFile(
        string Name,
        string Content
    );

    internal sealed class GeneratedFileSet
    {

        public static GeneratedFileSet Empty { get; } = new GeneratedFileSet(
            Array.Empty<GeneratedFile>());

        private readonly ReadOnlyDictionary<string, GeneratedFile> _filesByName;

        public GeneratedFileSet(
                                IEnumerable<GeneratedFile> files
        )
        {
            ArgumentNullException.ThrowIfNull(files);

            ReadOnlyCollection<GeneratedFile> values = Array.AsReadOnly(files.ToArray());
            Dictionary<string, GeneratedFile> index = new Dictionary<string, GeneratedFile>(
                StringComparer.OrdinalIgnoreCase);
            foreach (GeneratedFile file in values)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(file.Name);
                if (!index.TryAdd(file.Name, file))
                {
                    throw new InvalidOperationException(
                        $"Generated file name '{file.Name}' is duplicated.");
                }
            }

            Files = values;
            _filesByName = new ReadOnlyDictionary<string, GeneratedFile>(index);
        }

        public IReadOnlyList<GeneratedFile> Files { get; }

        public bool TryGetFile(
                               string name,
                               out GeneratedFile file
        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            return _filesByName.TryGetValue(name, out file!);
        }

    }
}
