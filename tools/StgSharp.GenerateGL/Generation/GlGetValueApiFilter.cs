// -----------------------------------------------------------------------------
// file="GlGetValueApiFilter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Replaces desktop state query pointer families with typed Span overloads.
    /// </summary>
    internal sealed class GlGetValueApiFilter : IGlApiFilter
    {

        private static readonly Regex _pattern = new Regex(
            @"^glGet(?<type>Boolean|Double|Float|Integer64|Integer)(?<indexed>i_)?v$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public IReadOnlyList<ManagedMethodDefinition> Apply(
            GlCommandDefinition command,
            IReadOnlyList<ManagedMethodDefinition> methods,
            GlEquivalentTypeMapper typeMapper
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(methods);
            ArgumentNullException.ThrowIfNull(typeMapper);

            Match match = _pattern.Match(command.Name);
            if (!match.Success)
            {
                return methods;
            }

            bool isIndexed = match.Groups["indexed"].Success;
            int prefixCount = isIndexed ? 2 : 1;
            if (typeMapper.ReadReturnType(command) != "void" ||
                command.Parameters.Count != prefixCount + 1)
            {
                throw new InvalidOperationException(
                    $"State query '{command.Name}' does not match its recognized shape.");
            }

            GlParameterDefinition output = command.Parameters[^1];
            string elementType = ReadElementType(match.Groups["type"].Value);
            string expectedLength = $"COMPSIZE({command.Parameters[0].Name})";
            if (output.DeclarationText.Contains("const", StringComparison.Ordinal) ||
                typeMapper.ReadParameterType(output) != $"{elementType}*" ||
                !string.Equals(output.Length, expectedLength, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"State query '{command.Name}' has inconsistent output metadata.");
            }

            List<ManagedParameterDefinition> parameters = new List<ManagedParameterDefinition>();
            List<ManagedArgument> arguments = new List<ManagedArgument>();
            for (int index = 0; index < prefixCount; index++)
            {
                GlParameterDefinition source = command.Parameters[index];
                parameters.Add(
                    new ManagedParameterDefinition(
                        typeMapper.ReadParameterType(source),
                        source.Name,
                        source));
                arguments.Add(new ParameterArgument(index));
            }

            parameters.Add(
                new ManagedParameterDefinition(
                    $"Span<{elementType}>",
                    "values",
                    output));
            return Array.AsReadOnly(
                new[]
                {
                    new ManagedMethodDefinition(
                        "GetValue",
                        "void",
                        Array.AsReadOnly(parameters.ToArray()),
                        command,
                        new QuerySpanInvocation(
                            Array.AsReadOnly(arguments.ToArray()),
                            parameters.Count - 1,
                            elementType)),
                });
        }

        private static string ReadElementType(
            string name
        )
        {
            return name switch
            {
                "Boolean" => "byte",
                "Double" => "double",
                "Float" => "float",
                "Integer" => "int",
                "Integer64" => "long",
                _ => throw new InvalidOperationException(
                    $"Unsupported state query component type '{name}'."),
            };
        }

    }
}
