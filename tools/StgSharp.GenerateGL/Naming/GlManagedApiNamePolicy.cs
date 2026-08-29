// -----------------------------------------------------------------------------
// file="GlManagedApiNamePolicy"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StgSharp.GenerateGL.Analysis;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Naming
{
    /// <summary>
    ///   Translates native overload families into stable, semantic C# method names.
    /// </summary>
    internal sealed class GlManagedApiNamePolicy
    {

        private static readonly Regex _getVertexAttributeModePattern = new Regex(
            @"^glGetVertexAttrib(?<mode>I|L)(?:i|ui|d)v$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex _vertexAttributeModePattern = new Regex(
            @"^glVertexAttrib(?<mode>I|L|P)[1-4]",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly Dictionary<string, FamilyEntry> _familiesByCommand;

        public GlManagedApiNamePolicy(
            GlFunctionFamilyAnalysis analysis
        )
        {
            ArgumentNullException.ThrowIfNull(analysis);
            _familiesByCommand = analysis.Families
                .SelectMany(
                    family => family.Members.Select(
                        member => new KeyValuePair<string, FamilyEntry>(
                            member.Command.Name,
                            new FamilyEntry(family, member.NameShape))))
                .ToDictionary(
                    value => value.Key,
                    value => value.Value,
                    StringComparer.Ordinal);
        }

        public string ReadMethodName(
            GlCommandDefinition command,
            string currentName
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentException.ThrowIfNullOrWhiteSpace(currentName);

            string rawName = CSharpIdentifierPolicy.NormalizeMemberName(
                RemoveGlPrefix(command.Name));
            string normalizedCurrentName = CSharpIdentifierPolicy.NormalizeMemberName(
                currentName);
            if (!string.Equals(normalizedCurrentName, rawName, StringComparison.Ordinal))
            {
                return normalizedCurrentName;
            }

            string? specialName = ReadSpecialMethodName(command.Name);
            if (specialName is not null)
            {
                return specialName;
            }

            if (!_familiesByCommand.TryGetValue(
                    command.Name,
                    out FamilyEntry? entry))
            {
                return rawName;
            }

            string familyName = ReadSemanticFamilyName(entry.Family.Name);
            if (entry.Family.ShapeNamePolicy == GlFunctionShapeNamePolicy.MustRetain &&
                entry.Shape?.Shape is not null)
            {
                familyName += entry.Shape.Shape;
            }

            return CSharpIdentifierPolicy.NormalizeMemberName(familyName);
        }

        public static string ReadNativeMethodName(
            GlCommandDefinition command
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            return CSharpIdentifierPolicy.NormalizeMemberName(RemoveGlPrefix(command.Name));
        }

        public string ReadDocumentationPage(
            GlCommandDefinition command
        )
        {
            ArgumentNullException.ThrowIfNull(command);

            string? specialPage = ReadSpecialDocumentationPage(command.Name);
            if (specialPage is not null)
            {
                return specialPage;
            }

            if (!_familiesByCommand.TryGetValue(
                    command.Name,
                    out FamilyEntry? entry))
            {
                return command.Name;
            }

            string familyName = entry.Family.Name
                .Replace("FrameBuffer", "Framebuffer", StringComparison.Ordinal)
                .Replace("RenderBuffer", "Renderbuffer", StringComparison.Ordinal)
                .Replace("InternalFormat", "Internalformat", StringComparison.Ordinal)
                .Replace("Attribute", "Attrib", StringComparison.Ordinal)
                .Replace("Fragment", "Frag", StringComparison.Ordinal);
            return $"gl{ReadDocumentationFamilyName(familyName)}";
        }

        private static string? ReadSpecialMethodName(
            string commandName
        )
        {
            if (commandName == "glClearBufferfi")
            {
                return "ClearBuffer";
            }

            if (commandName == "glClearNamedFramebufferfi")
            {
                return "ClearNamedFrameBuffer";
            }

            if (commandName == "glGetVertexAttribPointerv")
            {
                return "GetVertexAttributePointer";
            }

            if (commandName.StartsWith("glVertexAttrib4N", StringComparison.Ordinal))
            {
                return "VertexAttributeNormalized";
            }

            Match getMatch = _getVertexAttributeModePattern.Match(commandName);
            if (getMatch.Success)
            {
                return getMatch.Groups["mode"].Value == "I"
                    ? "GetVertexAttributeInteger"
                    : "GetVertexAttributeDouble";
            }

            if (commandName is "glVertexAttribIFormat" or "glVertexArrayAttribIFormat")
            {
                return CSharpIdentifierPolicy.NormalizeMemberName(
                    RemoveGlPrefix(commandName).Replace(
                        "AttribIFormat",
                        "AttributeIntegerFormat",
                        StringComparison.Ordinal));
            }

            if (commandName is "glVertexAttribLFormat" or "glVertexArrayAttribLFormat")
            {
                return CSharpIdentifierPolicy.NormalizeMemberName(
                    RemoveGlPrefix(commandName).Replace(
                        "AttribLFormat",
                        "AttributeDoubleFormat",
                        StringComparison.Ordinal));
            }

            if (commandName == "glVertexAttribIPointer")
            {
                return "VertexAttributeIntegerPointer";
            }

            if (commandName == "glVertexAttribLPointer")
            {
                return "VertexAttributeDoublePointer";
            }

            Match vertexMatch = _vertexAttributeModePattern.Match(commandName);
            if (!vertexMatch.Success)
            {
                return null;
            }

            return vertexMatch.Groups["mode"].Value switch
            {
                "I" => "VertexAttributeInteger",
                "L" => "VertexAttributeDouble",
                "P" => $"VertexAttributePacked{ReadVertexAttributeWidth(commandName, 'P')}",
                _ => null,
            };
        }

        private static string ReadSemanticFamilyName(
            string familyName
        )
        {
            return familyName switch
            {
                "VertexAttrib" or "VertexAttribute" => "VertexAttribute",
                "VertexAttrib4N" or "VertexAttribute4N" => "VertexAttributeNormalized",
                "VertexAttribI" or "VertexAttributeI" => "VertexAttributeInteger",
                "VertexAttribL" or "VertexAttributeL" => "VertexAttributeDouble",
                "VertexAttribP" or "VertexAttributeP" => "VertexAttributePacked",
                "GetVertexAttrib" or "GetVertexAttribute" => "GetVertexAttribute",
                "GetVertexAttribI" or "GetVertexAttributeI" => "GetVertexAttributeInteger",
                "GetnUniform" => "GetUniform",
                "SamplerParameterI" => "SamplerParameterInteger",
                "GetSamplerParameterI" => "GetSamplerParameterInteger",
                "TexParameter" => "TextureParameter",
                "TexParameterI" => "TextureParameterInteger",
                "GetTexParameter" => "GetTextureParameter",
                "GetTexParameterI" => "GetTextureParameterInteger",
                "GetTexLevelParameter" => "GetTextureLevelParameter",
                "TextureParameter" => "NamedTextureParameter",
                "TextureParameterI" => "NamedTextureParameterInteger",
                "GetTextureParameter" => "GetNamedTextureParameter",
                "GetTextureParameterI" => "GetNamedTextureParameterInteger",
                "GetTextureLevelParameter" => "GetNamedTextureLevelParameter",
                _ => familyName,
            };
        }

        private static string ReadDocumentationFamilyName(
            string familyName
        )
        {
            return familyName switch
            {
                "VertexAttrib4N" or "VertexAttribI" or "VertexAttribL" or
                    "VertexAttribP" => "VertexAttrib",
                "GetVertexAttribI" => "GetVertexAttrib",
                "SamplerParameterI" => "SamplerParameter",
                "GetSamplerParameterI" => "GetSamplerParameter",
                "TexParameterI" => "TexParameter",
                "GetTexParameterI" => "GetTexParameter",
                "TextureParameterI" => "TextureParameter",
                "GetTextureParameterI" => "GetTextureParameter",
                _ => familyName,
            };
        }

        private static string? ReadSpecialDocumentationPage(
            string commandName
        )
        {
            if (commandName.StartsWith("glProgramUniformMatrix", StringComparison.Ordinal))
            {
                return "glProgramUniform";
            }

            if (commandName.StartsWith("glUniformMatrix", StringComparison.Ordinal))
            {
                return "glUniform";
            }

            if (commandName is "glVertexAttribIPointer" or "glVertexAttribLPointer")
            {
                return "glVertexAttribPointer";
            }

            if (commandName is "glVertexAttribIFormat" or "glVertexAttribLFormat" or
                "glVertexArrayAttribFormat" or "glVertexArrayAttribIFormat" or
                "glVertexArrayAttribLFormat")
            {
                return "glVertexAttribFormat";
            }

            if (commandName == "glClearBufferfi")
            {
                return "glClearBuffer";
            }

            if (commandName == "glClearNamedFramebufferfi")
            {
                return "glClearNamedFramebuffer";
            }

            if (commandName is "glGetBooleanv" or "glGetDoublev" or
                "glGetFloatv" or "glGetIntegerv" or "glGetInteger64v" or
                "glGetBooleani_v" or "glGetDoublei_v" or "glGetFloati_v" or
                "glGetIntegeri_v" or "glGetInteger64i_v")
            {
                return "glGet";
            }

            if (commandName == "glGetVertexAttribPointerv")
            {
                return commandName;
            }

            if (commandName.StartsWith("glGetVertexAttrib", StringComparison.Ordinal))
            {
                return "glGetVertexAttrib";
            }

            if (commandName.StartsWith("glVertexAttrib", StringComparison.Ordinal) &&
                commandName.Length > "glVertexAttrib".Length &&
                char.IsDigit(commandName["glVertexAttrib".Length]) ||
                commandName.StartsWith("glVertexAttribI", StringComparison.Ordinal) &&
                commandName != "glVertexAttribIFormat" &&
                commandName != "glVertexAttribIPointer" ||
                commandName.StartsWith("glVertexAttribL", StringComparison.Ordinal) &&
                commandName != "glVertexAttribLFormat" &&
                commandName != "glVertexAttribLPointer" ||
                commandName.StartsWith("glVertexAttribP", StringComparison.Ordinal))
            {
                return "glVertexAttrib";
            }

            return null;
        }

        private static char ReadVertexAttributeWidth(
            string commandName,
            char mode
        )
        {
            int index = commandName.IndexOf(mode, "glVertexAttrib".Length) + 1;
            return commandName[index];
        }

        private static string RemoveGlPrefix(
            string name
        )
        {
            return name.StartsWith("gl", StringComparison.Ordinal) && name.Length > 2
                ? name[2..]
                : name;
        }

        private sealed record FamilyEntry(
            GlFunctionFamily Family,
            GlFunctionNameShape? Shape
        );

    }
}
