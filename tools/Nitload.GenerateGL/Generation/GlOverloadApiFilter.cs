// -----------------------------------------------------------------------------
// file="GlOverloadApiFilter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Replaces verified C-style command families with managed overloads.
    /// </summary>
    internal sealed class GlOverloadApiFilter : IGlApiFilter
    {

        private static readonly Regex _matrixUniformPattern = new Regex(
            @"^gl(?<family>ProgramUniform|Uniform)Matrix(?<columns>[2-4])" +
            @"(?:x(?<rows>[2-4]))?(?<type>f|d)v$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex _uniformPattern = new Regex(
            @"^gl(?<family>ProgramUniform|Uniform)(?<width>[1-4])" +
            @"(?<type>ui|f|i|d)(?<vector>v?)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex _vertexAttribPattern = new Regex(
            @"^glVertexAttrib(?<width>[1-4])(?<type>d|f|s)(?<vector>v?)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex _redundantVertexAttributeVectorPattern = new Regex(
            @"^glVertexAttrib(?:I[1-4](?:i|ui)|L[1-4]d|P[1-4]ui|4Nub)v$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly string[] _components = { "X", "Y", "Z", "W" };

        public IReadOnlyList<ManagedMethodDefinition> Apply(
            GlCommandDefinition command,
            IReadOnlyList<ManagedMethodDefinition> methods,
            GlEquivalentTypeMapper typeMapper
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(methods);
            ArgumentNullException.ThrowIfNull(typeMapper);

            if (_redundantVertexAttributeVectorPattern.IsMatch(command.Name))
            {
                return Array.Empty<ManagedMethodDefinition>();
            }

            Match matrixMatch = _matrixUniformPattern.Match(command.Name);
            if (matrixMatch.Success)
            {
                return RewriteMatrixUniform(command, matrixMatch, typeMapper);
            }

            Match uniformMatch = _uniformPattern.Match(command.Name);
            if (uniformMatch.Success)
            {
                return RewriteUniform(command, uniformMatch, typeMapper);
            }

            Match vertexAttribMatch = _vertexAttribPattern.Match(command.Name);
            if (vertexAttribMatch.Success)
            {
                return vertexAttribMatch.Groups["vector"].Value.Length == 0
                    ? Array.AsReadOnly(
                        new[]
                        {
                            methods[0] with { Name = "VertexAttrib" },
                        })
                    : Array.Empty<ManagedMethodDefinition>();
            }

            return methods;
        }

        private static ReadOnlyCollection<ManagedMethodDefinition> RewriteUniform(
            GlCommandDefinition command,
            Match match,
            GlEquivalentTypeMapper typeMapper
        )
        {
            int width = int.Parse(
                match.Groups["width"].Value,
                CultureInfo.InvariantCulture);
            string componentType = ReadComponentType(match.Groups["type"].Value);
            bool isVector = match.Groups["vector"].Value.Length != 0;
            int prefixCount = match.Groups["family"].Value == "ProgramUniform" ? 2 : 1;
            string methodName = match.Groups["family"].Value;

            ValidateUniformSignature(
                command,
                width,
                componentType,
                prefixCount,
                isVector,
                typeMapper);

            List<ManagedParameterDefinition> prefix = ReadNativeParameters(
                command,
                prefixCount,
                typeMapper);
            List<ManagedArgument> prefixArguments = ReadParameterArguments(prefix.Count);
            if (isVector)
            {
                if (width is 2 or 3)
                {
                    return Array.AsReadOnly(Array.Empty<ManagedMethodDefinition>());
                }

                GlParameterDefinition valueParameter = command.Parameters[^1];
                string spanElementType = width == 1
                    ? componentType
                    : $"Vec{width}<{componentType}>";
                List<ManagedParameterDefinition> parameters = new List<ManagedParameterDefinition>(prefix)
                {
                    new ManagedParameterDefinition(
                        $"ReadOnlySpan<{spanElementType}>",
                        "values",
                        valueParameter),
                };
                ManagedInvocation invocation = width == 1
                    ? new PinnedSpanInvocation(
                        prefixArguments,
                        parameters.Count - 1,
                        componentType)
                    : new PinnedVectorSpanInvocation(
                        prefixArguments,
                        parameters.Count - 1,
                        componentType,
                        spanElementType);
                return Array.AsReadOnly(
                    new[]
                    {
                        new ManagedMethodDefinition(
                            methodName,
                            "void",
                            Array.AsReadOnly(parameters.ToArray()),
                            command,
                            invocation),
                    });
            }

            List<ManagedMethodDefinition> methods = new List<ManagedMethodDefinition>();
            List<ManagedParameterDefinition> scalarParameters = ReadNativeParameters(
                command,
                command.Parameters.Count,
                typeMapper);
            methods.Add(
                new ManagedMethodDefinition(
                    methodName,
                    "void",
                    Array.AsReadOnly(scalarParameters.ToArray()),
                    command,
                    new DirectInvocation(ReadParameterArguments(scalarParameters.Count))));

            if (width > 1)
            {
                List<ManagedParameterDefinition> vectorParameters = new List<ManagedParameterDefinition>(prefix)
                {
                    new ManagedParameterDefinition(
                        $"Vec{width}<{componentType}>",
                        "value"),
                };
                List<ManagedArgument> vectorArguments = new List<ManagedArgument>(prefixArguments);
                for (int index = 0; index < width; index++)
                {
                    vectorArguments.Add(
                        new VectorComponentArgument(
                            vectorParameters.Count - 1,
                            _components[index]));
                }

                methods.Add(
                    new ManagedMethodDefinition(
                        methodName,
                        "void",
                        Array.AsReadOnly(vectorParameters.ToArray()),
                        command,
                        new DirectInvocation(Array.AsReadOnly(vectorArguments.ToArray()))));
            }

            return Array.AsReadOnly(methods.ToArray());
        }

        private static IReadOnlyList<ManagedMethodDefinition> RewriteMatrixUniform(
            GlCommandDefinition command,
            Match match,
            GlEquivalentTypeMapper typeMapper
        )
        {
            int columns = int.Parse(
                match.Groups["columns"].Value,
                CultureInfo.InvariantCulture);
            int rows = match.Groups["rows"].Success
                ? int.Parse(
                    match.Groups["rows"].Value,
                    CultureInfo.InvariantCulture)
                : columns;
            string componentType = ReadComponentType(match.Groups["type"].Value);
            int prefixCount = match.Groups["family"].Value == "ProgramUniform" ? 2 : 1;
            string methodName = match.Groups["family"].Value;

            ValidateMatrixSignature(
                command,
                columns,
                rows,
                componentType,
                prefixCount,
                typeMapper);
            if (columns != 4 || rows != 4 || componentType != "float")
            {
                return Array.Empty<ManagedMethodDefinition>();
            }

            List<ManagedParameterDefinition> prefix = ReadNativeParameters(
                command,
                prefixCount,
                typeMapper);
            List<ManagedArgument> prefixArguments = ReadParameterArguments(prefix.Count);
            GlParameterDefinition transposeSource = command.Parameters[prefixCount + 1];
            GlParameterDefinition valueSource = command.Parameters[prefixCount + 2];

            List<ManagedParameterDefinition> singleParameters = new List<ManagedParameterDefinition>(prefix)
            {
                new ManagedParameterDefinition("bool", "transpose", transposeSource),
                new ManagedParameterDefinition("GraphicsMatrix", "value", valueSource),
            };
            List<ManagedParameterDefinition> spanParameters = new List<ManagedParameterDefinition>(prefix)
            {
                new ManagedParameterDefinition("bool", "transpose", transposeSource),
                new ManagedParameterDefinition(
                    "ReadOnlySpan<GraphicsMatrix>",
                    "values",
                    valueSource),
            };

            return Array.AsReadOnly(
                new[]
                {
                    new ManagedMethodDefinition(
                        methodName,
                        "void",
                        Array.AsReadOnly(singleParameters.ToArray()),
                        command,
                        new Matrix4Invocation(
                            prefixArguments,
                            singleParameters.Count - 2,
                            singleParameters.Count - 1,
                            false)),
                    new ManagedMethodDefinition(
                        methodName,
                        "void",
                        Array.AsReadOnly(spanParameters.ToArray()),
                        command,
                        new Matrix4Invocation(
                            prefixArguments,
                            spanParameters.Count - 2,
                            spanParameters.Count - 1,
                            true)),
                });
        }

        private static void ValidateUniformSignature(
            GlCommandDefinition command,
            int width,
            string componentType,
            int prefixCount,
            bool isVector,
            GlEquivalentTypeMapper typeMapper
        )
        {
            int expectedCount = prefixCount + (isVector ? 2 : width);
            if (command.Parameters.Count != expectedCount)
            {
                throw new InvalidOperationException(
                    $"Uniform command '{command.Name}' does not match its recognized shape.");
            }

            if (isVector)
            {
                GlParameterDefinition count = command.Parameters[prefixCount];
                GlParameterDefinition value = command.Parameters[prefixCount + 1];
                string expectedLength = $"{count.Name}*{width}";
                string expectedKind = $"Vector{width}";
                if (typeMapper.ReadParameterType(value) != $"{componentType}*" ||
                    !string.Equals(value.Length, expectedLength, StringComparison.Ordinal) &&
                    !(width == 1 && string.Equals(
                        value.Length,
                        count.Name,
                        StringComparison.Ordinal)) ||
                    !string.Equals(value.Kind, expectedKind, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Uniform command '{command.Name}' has inconsistent vector metadata.");
                }

                return;
            }

            for (int index = prefixCount; index < command.Parameters.Count; index++)
            {
                if (typeMapper.ReadParameterType(command.Parameters[index]) != componentType)
                {
                    throw new InvalidOperationException(
                        $"Uniform command '{command.Name}' has inconsistent component types.");
                }
            }
        }

        private static void ValidateMatrixSignature(
            GlCommandDefinition command,
            int columns,
            int rows,
            string componentType,
            int prefixCount,
            GlEquivalentTypeMapper typeMapper
        )
        {
            if (command.Parameters.Count != prefixCount + 3)
            {
                throw new InvalidOperationException(
                    $"Matrix command '{command.Name}' does not match its recognized shape.");
            }

            GlParameterDefinition count = command.Parameters[prefixCount];
            GlParameterDefinition value = command.Parameters[prefixCount + 2];
            if (typeMapper.ReadParameterType(value) != $"{componentType}*" ||
                !string.Equals(
                    value.Kind,
                    $"Matrix{columns}x{rows}",
                    StringComparison.Ordinal) ||
                !string.Equals(
                    value.Length,
                    $"{count.Name}*{columns * rows}",
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Matrix command '{command.Name}' has inconsistent matrix metadata.");
            }
        }

        private static List<ManagedParameterDefinition> ReadNativeParameters(
            GlCommandDefinition command,
            int count,
            GlEquivalentTypeMapper typeMapper
        )
        {
            List<ManagedParameterDefinition> parameters = new List<ManagedParameterDefinition>(count);
            for (int index = 0; index < count; index++)
            {
                GlParameterDefinition source = command.Parameters[index];
                parameters.Add(
                    new ManagedParameterDefinition(
                        typeMapper.ReadParameterType(source),
                        source.Name,
                        source));
            }

            return parameters;
        }

        private static List<ManagedArgument> ReadParameterArguments(
            int count
        )
        {
            List<ManagedArgument> arguments = new List<ManagedArgument>(count);
            for (int index = 0; index < count; index++)
            {
                arguments.Add(new ParameterArgument(index));
            }

            return arguments;
        }

        private static string ReadComponentType(
            string suffix
        )
        {
            return suffix switch
            {
                "f" => "float",
                "i" => "int",
                "ui" => "uint",
                "d" => "double",
                _ => throw new InvalidOperationException(
                    $"Unsupported uniform component suffix '{suffix}'."),
            };
        }

    }
}
