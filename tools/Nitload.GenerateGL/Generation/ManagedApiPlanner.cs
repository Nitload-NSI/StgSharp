// -----------------------------------------------------------------------------
// file="ManagedApiPlanner"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StgSharp.GenerateGL.Analysis;
using StgSharp.GenerateGL.Naming;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    internal sealed class ManagedApiPlanner
    {

        private readonly IReadOnlyList<IGlApiFilter> _filters = Array.AsReadOnly<IGlApiFilter>(
            new IGlApiFilter[]
            {
                new GlManagedApiSuppressionFilter(),
                new GlOverloadApiFilter(),
                new GlGetValueApiFilter(),
                new GlHandleApiFilter(),
            });
        private readonly GlEquivalentTypeMapper _typeMapper;
        private readonly GlEnumTypeMap _enumTypeMap;
        private readonly GlManagedApiNamePolicy _namePolicy;

        public ManagedApiPlanner(
            GlEquivalentTypeMapper typeMapper,
            GlFunctionFamilyAnalysis familyAnalysis,
            GlEnumTypeMap enumTypeMap
        )
        {
            ArgumentNullException.ThrowIfNull(typeMapper);
            ArgumentNullException.ThrowIfNull(familyAnalysis);
            ArgumentNullException.ThrowIfNull(enumTypeMap);
            _typeMapper = typeMapper;
            _enumTypeMap = enumTypeMap;
            _namePolicy = new GlManagedApiNamePolicy(familyAnalysis);
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

                methods = ApplyEnumTypes(command, methods);

                result.AddRange(
                    methods.Select(
                        method => method with
                        {
                            Name = _namePolicy.ReadMethodName(command, method.Name),
                            DocumentationPage = _namePolicy.ReadDocumentationPage(command),
                            Parameters = Array.AsReadOnly(
                                method.Parameters.Select(
                                    parameter => parameter with
                                    {
                                        Name = CSharpIdentifierPolicy.NormalizeParameterName(
                                            parameter.Name),
                                    }).ToArray()),
                        }));
            }

            ResolveNameCollisions(result);
            return Array.AsReadOnly(result.ToArray());
        }

        private ReadOnlyCollection<ManagedMethodDefinition> ApplyEnumTypes(
            GlCommandDefinition command,
            IReadOnlyList<ManagedMethodDefinition> methods
        )
        {
            bool hasEnumReturn = _enumTypeMap.TryReadReturnType(
                command.Return,
                out string enumReturnType);
            return Array.AsReadOnly(
                methods.Select(
                    method => ConvertEnumReturn(method, hasEnumReturn, enumReturnType)).ToArray());
        }

        private ManagedMethodDefinition ConvertEnumReturn(
            ManagedMethodDefinition method,
            bool hasEnumReturn,
            string enumReturnType
        )
        {
            string returnType = string.Empty;
            bool isEnumReturn = hasEnumReturn && TryReplaceNativeEnumType(
                method.ReturnType,
                enumReturnType,
                out returnType);
            return method with
            {
                ReturnType = isEnumReturn ? returnType : method.ReturnType,
                IsEnumReturn = isEnumReturn,
                Parameters = Array.AsReadOnly(
                    method.Parameters.Select(ConvertEnumParameter).ToArray()),
            };
        }

        private ManagedParameterDefinition ConvertEnumParameter(
            ManagedParameterDefinition parameter
        )
        {
            if (parameter.NativeParameter is null ||
                !_enumTypeMap.TryReadParameterType(
                    parameter.NativeParameter,
                    out string enumType) ||
                !TryReplaceNativeEnumType(parameter.Type, enumType, out string convertedType))
            {
                return parameter;
            }

            return parameter with
            {
                Type = convertedType,
                IsEnum = true,
            };
        }

        private static bool TryReplaceNativeEnumType(
            string nativeType,
            string enumType,
            out string convertedType
        )
        {
            const string nativeBaseType = "uint";
            if (!nativeType.StartsWith(nativeBaseType, StringComparison.Ordinal) ||
                nativeType.AsSpan(nativeBaseType.Length).ContainsAnyExcept('*'))
            {
                convertedType = string.Empty;
                return false;
            }

            convertedType = enumType + nativeType[nativeBaseType.Length..];
            return true;
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

        private static void ResolveNameCollisions(
            List<ManagedMethodDefinition> methods
        )
        {
            ILookup<string, int> indicesBySignature = methods
                .Select(
                    (method, index) => new
                    {
                        Index = index,
                        Signature = ReadManagedSignature(method),
                    })
                .ToLookup(value => value.Signature, value => value.Index, StringComparer.Ordinal);
            foreach (IGrouping<string, int> group in indicesBySignature)
            {
                int[] indices = group.ToArray();
                if (indices.Length < 2)
                {
                    continue;
                }

                string[] nativeNames = indices
                    .Select(value => methods[value].NativeCommand.Name)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();
                if (nativeNames.Length != indices.Length)
                {
                    throw new InvalidOperationException(
                        $"Managed overload '{group.Key}' is duplicated by one native command.");
                }

                foreach (int index in indices)
                {
                    ManagedMethodDefinition method = methods[index];
                    methods[index] = method with
                    {
                        Name = GlManagedApiNamePolicy.ReadNativeMethodName(
                            method.NativeCommand),
                    };
                }
            }
        }

        private static string ReadManagedSignature(
            ManagedMethodDefinition method
        )
        {
            return $"{method.Name}({string.Join(",", method.Parameters.Select(value => value.Type))})";
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
