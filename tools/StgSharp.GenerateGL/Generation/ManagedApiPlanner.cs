// -----------------------------------------------------------------------------
// file="ManagedApiPlanner"
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
    internal sealed class ManagedApiPlanner
    {

        private readonly IReadOnlyList<IGlApiFilter> _filters = Array.AsReadOnly<IGlApiFilter>(
            new IGlApiFilter[]
            {
                new GlOverloadApiFilter(),
                new GlHandleApiFilter(),
            });
        private readonly GlEquivalentTypeMapper _typeMapper;

        public ManagedApiPlanner(
            GlEquivalentTypeMapper typeMapper
        )
        {
            ArgumentNullException.ThrowIfNull(typeMapper);
            _typeMapper = typeMapper;
        }

        public IReadOnlyList<ManagedMethodDefinition> Plan(
            IReadOnlyList<GlCommandDefinition> commands
        )
        {
            ArgumentNullException.ThrowIfNull(commands);

            List<ManagedMethodDefinition> result = new List<ManagedMethodDefinition>();
            foreach (GlCommandDefinition command in commands)
            {
                IReadOnlyList<ManagedMethodDefinition> methods = Array.AsReadOnly(
                    new[]
                    {
                        CreateRawMethod(command),
                    });
                foreach (IGlApiFilter filter in _filters)
                {
                    methods = filter.Apply(command, methods, _typeMapper);
                }

                result.AddRange(methods);
            }

            return Array.AsReadOnly(result.ToArray());
        }

        public static void Validate(
            IEnumerable<ManagedMethodDefinition> methods
        )
        {
            ArgumentNullException.ThrowIfNull(methods);

            Dictionary<string, ManagedMethodDefinition> signatures = new Dictionary<string, ManagedMethodDefinition>(
                StringComparer.Ordinal);
            foreach (ManagedMethodDefinition method in methods)
            {
                string signature = $"{method.Name}({string.Join(",", method.Parameters.Select(value => value.Type))})";
                if (signatures.TryGetValue(signature, out ManagedMethodDefinition? previous))
                {
                    throw new InvalidOperationException(
                        $"Managed overload '{signature}' is produced by both " +
                        $"'{previous.NativeCommand.Name}' and '{method.NativeCommand.Name}'.");
                }

                signatures.Add(signature, method);
            }
        }

        private ManagedMethodDefinition CreateRawMethod(
            GlCommandDefinition command
        )
        {
            List<ManagedParameterDefinition> parameters = new List<ManagedParameterDefinition>(
                command.Parameters.Count);
            List<ManagedArgument> arguments = new List<ManagedArgument>(
                command.Parameters.Count);
            for (int index = 0; index < command.Parameters.Count; index++)
            {
                GlParameterDefinition source = command.Parameters[index];
                parameters.Add(
                    new ManagedParameterDefinition(
                        _typeMapper.ReadParameterType(source),
                        source.Name,
                        source));
                arguments.Add(new ParameterArgument(index));
            }

            return new ManagedMethodDefinition(
                ReadMethodName(command.Name),
                _typeMapper.ReadReturnType(command),
                Array.AsReadOnly(parameters.ToArray()),
                command,
                new DirectInvocation(Array.AsReadOnly(arguments.ToArray())));
        }

        private static string ReadMethodName(
            string commandName
        )
        {
            return commandName.StartsWith("gl", StringComparison.Ordinal) &&
                   commandName.Length > 2
                ? commandName[2..]
                : commandName;
        }

    }
}
