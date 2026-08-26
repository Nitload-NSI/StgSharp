// -----------------------------------------------------------------------------
// file="XmlTreeReader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace StgSharp.GenerateGL.Tree
{
    /// <summary>
    ///   Loads XML as a generic tree without interpreting OpenGL semantics.
    /// </summary>
    internal static class XmlTreeReader
    {

        public static XmlTreeDocument Load(
                                      string path
        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            string sourcePath = Path.GetFullPath(path);
            string sourceText = File.ReadAllText(sourcePath);
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = false,
                IgnoreWhitespace = false,
                XmlResolver = null,
            };

            XDocument tree;
            using (StringReader textReader = new StringReader(sourceText))
            using (XmlReader reader = XmlReader.Create(textReader, settings, sourcePath))
            {
                tree = XDocument.Load(
                    reader,
                    LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
            }

            if (tree.Root is null)
            {
                throw new InvalidDataException(
                    $"XML document '{sourcePath}' does not contain a root element.");
            }

            return new XmlTreeDocument(sourcePath, sourceText, tree);
        }

    }
}
