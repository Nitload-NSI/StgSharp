// -----------------------------------------------------------------------------
// file="GlHandleApiFilter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Replaces registry object-class GLuint values with the managed GlHandle type.
    /// </summary>
    internal sealed class GlHandleApiFilter : IGlApiFilter
    {

        public IReadOnlyList<ManagedMethodDefinition> Apply(
            GlCommandDefinition command,
            IReadOnlyList<ManagedMethodDefinition> methods,
            GlEquivalentTypeMapper typeMapper
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(methods);
            ArgumentNullException.ThrowIfNull(typeMapper);

            bool wrapsReturn = string.Equals(
                                   command.Return.TypeName,
                                   "GLuint",
                                   StringComparison.Ordinal) &&
                               command.Return.ObjectClass is not null;
            return Array.AsReadOnly(
                methods.Select(
                    method => ConvertMethod(method, wrapsReturn)).ToArray());
        }

        private static ManagedMethodDefinition ConvertMethod(
            ManagedMethodDefinition method,
            bool wrapsReturn
        )
        {
            ManagedParameterDefinition[] converted = method.Parameters.Select(
                ConvertParameter).ToArray();
            if (method.Invocation is not DirectInvocation direct ||
                !converted.Any(value => value.IsHandleSpan))
            {
                return method with
                {
                    ReturnType = wrapsReturn ? "GlHandle" : method.ReturnType,
                    Parameters = Array.AsReadOnly(converted),
                    WrapHandleReturn = wrapsReturn,
                };
            }

            Dictionary<string, int> spanByLengthName = new Dictionary<string, int>(
                StringComparer.Ordinal);
            for (int index = 0; index < converted.Length; index++)
            {
                ManagedParameterDefinition parameter = converted[index];
                string? length = parameter.NativeParameter?.Length;
                if (parameter.IsHandleSpan &&
                    length is not null &&
                    IsIdentifier(length))
                {
                    spanByLengthName.TryAdd(length, index);
                }
            }

            Dictionary<int, int> oldToNew = new Dictionary<int, int>();
            List<ManagedParameterDefinition> parameters = new List<ManagedParameterDefinition>();
            for (int oldIndex = 0; oldIndex < converted.Length; oldIndex++)
            {
                ManagedParameterDefinition parameter = converted[oldIndex];
                if (spanByLengthName.ContainsKey(parameter.Name))
                {
                    continue;
                }

                oldToNew.Add(oldIndex, parameters.Count);
                parameters.Add(parameter);
            }

            List<ManagedArgument> arguments = new List<ManagedArgument>(direct.Arguments.Count);
            foreach (ManagedArgument argument in direct.Arguments)
            {
                if (argument is not ParameterArgument sourceArgument)
                {
                    arguments.Add(argument);
                    continue;
                }

                ManagedParameterDefinition sourceParameter = converted[sourceArgument.ParameterIndex];
                if (spanByLengthName.TryGetValue(
                        sourceParameter.Name,
                        out int spanOldIndex))
                {
                    arguments.Add(new SpanLengthArgument(oldToNew[spanOldIndex]));
                }
                else
                {
                    arguments.Add(
                        new ParameterArgument(oldToNew[sourceArgument.ParameterIndex]));
                }
            }

            return method with
            {
                ReturnType = wrapsReturn ? "GlHandle" : method.ReturnType,
                Parameters = Array.AsReadOnly(parameters.ToArray()),
                Invocation = new DirectInvocation(Array.AsReadOnly(arguments.ToArray())),
                WrapHandleReturn = wrapsReturn,
            };
        }

        private static ManagedParameterDefinition ConvertParameter(
            ManagedParameterDefinition parameter
        )
        {
            GlParameterDefinition? source = parameter.NativeParameter;
            if (source is null ||
                source.ObjectClass is null ||
                !string.Equals(source.TypeName, "GLuint", StringComparison.Ordinal))
            {
                return parameter;
            }

            int pointerDepth = source.DeclarationText.Count(value => value == '*');
            bool isSpan = pointerDepth == 1 && source.Length is not null;
            string type;
            if (isSpan)
            {
                type = source.DeclarationText.Contains("const", StringComparison.Ordinal)
                    ? "ReadOnlySpan<GlHandle>"
                    : "Span<GlHandle>";
            }
            else
            {
                type = pointerDepth == 0
                    ? "GlHandle"
                    : $"GlHandle{new string('*', pointerDepth)}";
            }

            return parameter with
            {
                Type = type,
                IsHandle = true,
                IsHandleSpan = isSpan,
            };
        }

        private static bool IsIdentifier(
            string value
        )
        {
            if (value.Length == 0 ||
                !char.IsLetter(value[0]) && value[0] != '_')
            {
                return false;
            }

            return value.Skip(1).All(character => char.IsLetterOrDigit(character) || character == '_');
        }

    }
}
