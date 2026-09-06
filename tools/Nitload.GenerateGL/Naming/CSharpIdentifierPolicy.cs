// -----------------------------------------------------------------------------
// file="CSharpIdentifierPolicy"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Text.RegularExpressions;

namespace StgSharp.GenerateGL.Naming
{
    internal static class CSharpIdentifierPolicy
    {

        private static readonly Regex _attributeWord = new Regex(
            @"Attrib(?!ute)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex _attributeParameterWord = new Regex(
            @"attrib(?!ute)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex _fragmentWord = new Regex(
            @"Frag(?!ment)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string NormalizeMemberName(
            string value
        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            string result = value.Replace(
                                     "Framebuffer",
                                     "FrameBuffer",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "Renderbuffer",
                                     "RenderBuffer",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "Internalformat",
                                     "InternalFormat",
                                     StringComparison.Ordinal);
            result = _attributeWord.Replace(result, "Attribute");
            return _fragmentWord.Replace(result, "Fragment");
        }

        public static string NormalizeParameterName(
            string value
        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            string result = value.Replace(
                                     "Framebuffer",
                                     "FrameBuffer",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "framebuffer",
                                     "frameBuffer",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "Renderbuffer",
                                     "RenderBuffer",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "renderbuffer",
                                     "renderBuffer",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "Internalformat",
                                     "InternalFormat",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "internalformat",
                                     "internalFormat",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "attribindex",
                                     "attributeIndex",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "bindingindex",
                                     "bindingIndex",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "relativeoffset",
                                     "relativeOffset",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "pname",
                                     "pName",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "xoffset",
                                     "xOffset",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "yoffset",
                                     "yOffset",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "zoffset",
                                     "zOffset",
                                     StringComparison.Ordinal)
                                 .Replace(
                                     "vaobj",
                                     "vertexArray",
                                     StringComparison.Ordinal);
            return _attributeParameterWord.Replace(result, "attribute");
        }

    }
}
