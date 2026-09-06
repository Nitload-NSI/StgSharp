// -----------------------------------------------------------------------------
// file="GlEquivalentTypeMapper"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Resolves OpenGL typedef declarations recursively to equivalent C# ABI types.
    /// </summary>
    internal sealed class GlEquivalentTypeMapper
    {

        private readonly Dictionary<string, string> _resolvedTypes = new Dictionary<string, string>(
            StringComparer.Ordinal);
        private readonly GlRegistryModel _registry;

        public GlEquivalentTypeMapper(
                                      GlRegistryModel registry
        )
        {
            ArgumentNullException.ThrowIfNull(registry);
            _registry = registry;
        }

        public string ReadReturnType(
                                     GlCommandDefinition command
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            return ReadDeclarationType(
                command.Return.TypeName,
                command.Return.DeclarationText,
                command.Name);
        }

        public string ReadParameterType(
                                        GlParameterDefinition parameter
        )
        {
            ArgumentNullException.ThrowIfNull(parameter);
            return ReadDeclarationType(
                parameter.TypeName,
                parameter.DeclarationText,
                parameter.Name);
        }

        private string ReadDeclarationType(
                                           string? typeName,
                                           string declaration,
                                           string identifier
        )
        {
            if (typeName is null)
            {
                return ResolveTypeExpression(RemoveIdentifier(declaration, identifier));
            }

            string equivalentType = ResolveNamedType(
                typeName,
                new HashSet<string>(StringComparer.Ordinal));
            int pointerDepth = declaration.Count(value => value == '*');
            if (declaration.Contains('[', StringComparison.Ordinal))
            {
                pointerDepth++;
            }

            return equivalentType + new string('*', pointerDepth);
        }

        private string ResolveNamedType(
                                        string typeName,
                                        HashSet<string> resolving
        )
        {
            if (CSharpAbiTypePolicy.TryResolve(typeName, out string abiType))
            {
                return abiType;
            }

            if (_resolvedTypes.TryGetValue(typeName, out string? resolvedType))
            {
                return resolvedType;
            }

            if (!resolving.Add(typeName))
            {
                throw new InvalidDataException(
                    $"OpenGL type alias cycle contains '{typeName}'.");
            }

            if (!_registry.TypesByName.TryGetValue(
                    typeName,
                    out IReadOnlyList<GlTypeDefinition>? definitions) ||
                definitions.Count != 1)
            {
                throw new InvalidDataException(
                    $"OpenGL type '{typeName}' does not have one registry definition.");
            }

            GlTypeDefinition definition = definitions[0];
            string declaration = NormalizeWhitespace(definition.DeclarationText);
            string value;
            if (TryReadCallbackType(typeName, declaration, resolving, out string callbackType))
            {
                value = callbackType;
            }
            else
            {
                MatchCollection aliases = Regex.Matches(
                    declaration,
                    $@"typedef\s+(?<target>.+?)\s*\b{Regex.Escape(typeName)}\b\s*;",
                    RegexOptions.CultureInvariant);
                if (aliases.Count == 1)
                {
                    value = ResolveTypeExpression(
                        aliases[0].Groups["target"].Value,
                        resolving);
                }
                else if (declaration == $"{typeName};" || declaration.StartsWith(
                             "struct ",
                             StringComparison.Ordinal))
                {
                    value = "void";
                }
                else if (aliases.Count > 1)
                {
                    throw new InvalidDataException(
                        $"OpenGL type '{typeName}' has platform-dependent typedefs and " +
                        "requires an explicit target policy.");
                }
                else
                {
                    throw new InvalidDataException(
                        $"Cannot read typedef declaration '{definition.DeclarationText}'.");
                }
            }

            resolving.Remove(typeName);
            _resolvedTypes.Add(typeName, value);
            return value;
        }

        private bool TryReadCallbackType(
                                         string typeName,
                                         string declaration,
                                         HashSet<string> resolving,
                                         out string type
        )
        {
            Match match = Regex.Match(
                declaration,
                $@"^typedef\s+(?<return>.+?)\s*\(\s*\*\s*{Regex.Escape(typeName)}\s*\)" +
                @"\s*\((?<parameters>.*)\)\s*;$",
                RegexOptions.CultureInvariant);
            if (!match.Success)
            {
                type = string.Empty;
                return false;
            }

            List<string> types = new List<string>();
            string parameters = match.Groups["parameters"].Value.Trim();
            if (parameters.Length != 0 && parameters != "void")
            {
                foreach (string parameter in parameters.Split(','))
                {
                    types.Add(ResolveTypeExpression(RemoveLastIdentifier(parameter), resolving));
                }
            }

            types.Add(ResolveTypeExpression(match.Groups["return"].Value, resolving));
            type = $"delegate* unmanaged<{string.Join(", ", types)}>";
            return true;
        }

        private string ResolveTypeExpression(
                                             string expression,
                                             HashSet<string>? resolving = null
        )
        {
            resolving ??= new HashSet<string>(StringComparer.Ordinal);
            string value = Regex.Replace(
                NormalizeWhitespace(expression),
                @"\b(?:const|volatile|restrict)\b",
                string.Empty,
                RegexOptions.CultureInvariant).Trim();
            int pointerDepth = value.Count(character => character == '*');
            value = value.Replace("*", string.Empty, StringComparison.Ordinal).Trim();
            if (value.StartsWith("struct ", StringComparison.Ordinal))
            {
                value = "void";
            }

            string equivalentType = ResolveNamedType(value, resolving);
            return equivalentType + new string('*', pointerDepth);
        }

        private static string RemoveIdentifier(
                                               string declaration,
                                               string identifier
        )
        {
            int index = declaration.LastIndexOf(identifier, StringComparison.Ordinal);
            return index < 0
                ? declaration
                : declaration.Remove(index, identifier.Length);
        }

        private static string RemoveLastIdentifier(
                                                   string declaration
        )
        {
            Match match = Regex.Match(
                declaration.Trim(),
                @"^(?<type>.+?)(?<name>[A-Za-z_][A-Za-z0-9_]*)$",
                RegexOptions.CultureInvariant);
            return match.Success ? match.Groups["type"].Value.Trim() : declaration.Trim();
        }

        private static string NormalizeWhitespace(
                                                  string value
        )
        {
            return Regex.Replace(
                value,
                @"\s+",
                " ",
                RegexOptions.CultureInvariant).Trim();
        }

    }
}
