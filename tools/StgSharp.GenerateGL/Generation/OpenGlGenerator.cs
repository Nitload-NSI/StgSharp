// -----------------------------------------------------------------------------
// file="OpenGlGenerator"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using StgSharp.GenerateGL.Analysis;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Produces the first in-memory desktop OpenGL binding files.
    /// </summary>
    internal static class OpenGlGenerator
    {

        private static readonly HashSet<string> _csharpKeywords = new HashSet<string>(
            new[]
            {
                "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
                "char", "checked", "class", "const", "continue", "decimal", "default",
                "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
                "false", "finally", "fixed", "float", "for", "foreach", "goto", "if",
                "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
                "namespace", "new", "null", "object", "operator", "out", "override",
                "params", "private", "protected", "public", "readonly", "ref", "return",
                "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string",
                "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
                "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
                "volatile", "while",
            },
            StringComparer.Ordinal);

        public static GeneratedFileSet Generate(
                                                GlRegistryProjection projection
        )
        {
            ArgumentNullException.ThrowIfNull(projection);

            IReadOnlyList<GlCommandGroup> commandGroups = ReadCoreCommandGroups(projection);
            IReadOnlyList<GlCommandDefinition> commands = Array.AsReadOnly(
                commandGroups.SelectMany(value => value.Commands).ToArray());
            GlEquivalentTypeMapper typeMapper = new GlEquivalentTypeMapper(projection.Source);
            GlFunctionFamilyAnalysis familyAnalysis = GlFunctionFamilyAnalyzer.Analyze(projection);
            ManagedApiPlanner apiPlanner = new ManagedApiPlanner(typeMapper, familyAnalysis);
            IReadOnlyList<ManagedMethodDefinition> managedMethods = apiPlanner.Plan(commands);
            Dictionary<string, IReadOnlyList<ManagedMethodDefinition>> methodsByCommand =
                managedMethods.GroupBy(value => value.NativeCommand.Name, StringComparer.Ordinal)
                              .ToDictionary(
                                  value => value.Key,
                                  value => (IReadOnlyList<ManagedMethodDefinition>)Array.AsReadOnly(
                                      value.ToArray()),
                                  StringComparer.Ordinal);
            List<ManagedCommandGroup> managedGroups = commandGroups.Select(
                group => new ManagedCommandGroup(
                    group,
                    Array.AsReadOnly(
                        group.Commands.SelectMany(
                            command => methodsByCommand.TryGetValue(
                                command.Name,
                                out IReadOnlyList<ManagedMethodDefinition>? methods)
                                    ? methods
                                    : Array.Empty<ManagedMethodDefinition>())
                             .ToArray()))).ToList();
            ManagedApiPlanner.Validate(
                managedGroups.SelectMany(value => value.Methods));
            List<GeneratedFile> files = new List<GeneratedFile>
            {
                new GeneratedFile("glconst.cs", GenerateConstants(projection.Enums)),
                new GeneratedFile(
                    "OpenglContext.g.cs",
                    GenerateContext(commands, typeMapper)),
                new GeneratedFile(
                    "glContextShadow.g.cs",
                    GenerateContextShadow(commands)),
            };
            for (int index = 0; index < managedGroups.Count; index++)
            {
                ManagedCommandGroup managedGroup = managedGroups[index];
                GlCommandGroup group = managedGroup.Source;
                files.Add(
                    new GeneratedFile(
                        group.FileName,
                        GenerateFunctions(
                            managedGroup.Methods,
                            group.Description,
                            index == 0)));
            }

            return new GeneratedFileSet(files);
        }

        private static ReadOnlyCollection<GlCommandGroup> ReadCoreCommandGroups(
                                                                                GlRegistryProjection projection
        )
        {
            List<GlCommandDefinition> foundation = new List<GlCommandDefinition>();
            Dictionary<string, List<GlCommandDefinition>> versionGroups = new Dictionary<string, List<GlCommandDefinition>>(
                StringComparer.Ordinal);
            List<string> versionOrder = new List<string>();
            foreach (GlCommandDefinition command in projection.Commands)
            {
                if (!projection.CommandIntroducedVersions.TryGetValue(
                        command.Name,
                        out string? version))
                {
                    throw new InvalidDataException(
                        $"Command '{command.Name}' has no introduction version.");
                }

                if (IsFoundationVersion(version))
                {
                    foundation.Add(command);
                    continue;
                }

                if (!versionGroups.TryGetValue(
                        version,
                        out List<GlCommandDefinition>? commands))
                {
                    commands = new List<GlCommandDefinition>();
                    versionGroups.Add(version, commands);
                    versionOrder.Add(version);
                }

                commands.Add(command);
            }

            List<GlCommandGroup> groups = new List<GlCommandGroup>
            {
                new GlCommandGroup(
                    "OpenGLFunction.Base.g.cs",
                    "Desktop OpenGL forwarding methods introduced through 3.2 and retained by the target core profile",
                    Array.AsReadOnly(foundation.ToArray())),
            };
            foreach (string version in versionOrder)
            {
                groups.Add(
                    new GlCommandGroup(
                        $"OpenGLFunction.v{version.Replace('.', '_')}.g.cs",
                        $"Desktop OpenGL {version} core-profile forwarding methods",
                        Array.AsReadOnly(versionGroups[version].ToArray())));
            }

            return Array.AsReadOnly(groups.ToArray());
        }

        private static bool IsFoundationVersion(
                                                string version
        )
        {
            string[] components = version.Split('.');
            if (components.Length != 2 ||
                !int.TryParse(
                    components[0],
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out int major) ||
                !int.TryParse(
                    components[1],
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out int minor))
            {
                throw new InvalidDataException(
                    $"OpenGL feature version '{version}' is not a major.minor number.");
            }

            return major < 3 || major == 3 && minor <= 2;
        }

        private static string GenerateConstants(
                                                IReadOnlyList<GlEnumDefinition> enums
        )
        {
            StringBuilder builder = new StringBuilder(512 * 1024);
            AppendGeneratedHeader(builder, "Desktop OpenGL constants");
            builder.Append("namespace StgSharp.Graphics.OpenGL\n");
            builder.Append("{\n");
            builder.Append("    public static class glConst\n");
            builder.Append("    {\n\n");

            Dictionary<string, GlEnumDefinition> emitted = new Dictionary<string, GlEnumDefinition>(
                StringComparer.Ordinal);
            foreach (GlEnumDefinition value in enums)
            {
                if (value.Value is null)
                {
                    continue;
                }

                if (emitted.TryGetValue(value.Name, out GlEnumDefinition? previous))
                {
                    if (!string.Equals(previous.Value, value.Value, StringComparison.Ordinal))
                    {
                        throw new InvalidDataException(
                            $"Desktop enum '{value.Name}' has conflicting values " +
                            $"'{previous.Value}' and '{value.Value}'.");
                    }

                    continue;
                }

                emitted.Add(value.Name, value);
                ConstantExpression expression = ReadConstantExpression(value);
                builder.Append("        public const ");
                builder.Append(expression.Type);
                builder.Append(' ');
                builder.Append(ReadConstantName(value.Name));
                builder.Append(" = ");
                builder.Append(expression.Literal);
                builder.Append(";\n");
            }

            builder.Append("\n    }\n");
            builder.Append("}\n");
            return builder.ToString();
        }

        private static ConstantExpression ReadConstantExpression(
                                                                 GlEnumDefinition value
        )
        {
            string source = value.Value!;
            bool isHex = source.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
            BigInteger number = isHex
                ? BigInteger.Parse(
                    $"0{source[2..]}",
                    NumberStyles.AllowHexSpecifier,
                    CultureInfo.InvariantCulture)
                : BigInteger.Parse(source, NumberStyles.Integer, CultureInfo.InvariantCulture);

            if (string.Equals(value.TypeSuffix, "ull", StringComparison.OrdinalIgnoreCase))
            {
                EnsureRange(number, BigInteger.Zero, ulong.MaxValue, value);
                return new ConstantExpression("ulong", ReadLiteral(source, isHex, "UL"));
            }

            if (string.Equals(value.TypeSuffix, "u", StringComparison.OrdinalIgnoreCase))
            {
                EnsureRange(number, BigInteger.Zero, uint.MaxValue, value);
                return new ConstantExpression("uint", ReadLiteral(source, isHex, "u"));
            }

            if (number >= int.MinValue && number <= int.MaxValue)
            {
                return new ConstantExpression("int", source);
            }

            if (number >= BigInteger.Zero && number <= uint.MaxValue)
            {
                return new ConstantExpression("uint", ReadLiteral(source, isHex, "u"));
            }

            if (number >= long.MinValue && number <= long.MaxValue)
            {
                return new ConstantExpression("long", ReadLiteral(source, isHex, "L"));
            }

            if (number >= BigInteger.Zero && number <= ulong.MaxValue)
            {
                return new ConstantExpression("ulong", ReadLiteral(source, isHex, "UL"));
            }

            throw new InvalidDataException(
                $"Enum '{value.Name}' value '{source}' does not fit a C# integral type.");
        }

        private static void EnsureRange(
                                        BigInteger value,
                                        BigInteger minimum,
                                        BigInteger maximum,
                                        GlEnumDefinition definition
        )
        {
            if (value < minimum || value > maximum)
            {
                throw new InvalidDataException(
                    $"Enum '{definition.Name}' value '{definition.Value}' is outside its " +
                    "declared numeric suffix range.");
            }
        }

        private static string ReadLiteral(
                                          string source,
                                          bool isHex,
                                          string suffix
        )
        {
            return isHex ? $"{source}{suffix}" : $"{source}{suffix}";
        }

        private static string ReadConstantName(
                                               string name
        )
        {
            string value = name.StartsWith("GL_", StringComparison.Ordinal)
                ? name[3..]
                : name;
            StringBuilder result = new StringBuilder(value.Length + 1);
            if (value.Length == 0 || char.IsDigit(value[0]))
            {
                result.Append('_');
            }

            foreach (char character in value)
            {
                result.Append(char.IsLetterOrDigit(character) || character == '_'
                    ? character
                    : '_');
            }

            return result.ToString();
        }

        private static string GenerateContext(
                                              IReadOnlyList<GlCommandDefinition> commands,
                                              GlEquivalentTypeMapper typeMapper
        )
        {
            StringBuilder builder = new StringBuilder(128 * 1024);
            AppendGeneratedHeader(builder, "Desktop OpenGL core function pointer table");
            builder.Append("namespace StgSharp.Graphics.OpenGL\n");
            builder.Append("{\n");
            builder.Append("    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]\n");
            builder.Append("    public unsafe partial struct OpenglContext\n");
            builder.Append("    {\n");
            foreach (GlCommandDefinition command in OrderCommands(commands))
            {
                builder.Append("        internal delegate* unmanaged<");
                foreach (GlParameterDefinition parameter in command.Parameters)
                {
                    builder.Append(typeMapper.ReadParameterType(parameter));
                    builder.Append(", ");
                }

                builder.Append(typeMapper.ReadReturnType(command));
                builder.Append("> ");
                builder.Append(command.Name);
                builder.Append(";\n");
            }

            builder.Append("    }\n");
            builder.Append("}\n");
            return builder.ToString();
        }

        private static string GenerateContextShadow(
                                                    IReadOnlyList<GlCommandDefinition> commands
        )
        {
            StringBuilder builder = new StringBuilder(64 * 1024);
            AppendGeneratedHeader(builder, "Native-address shadow of the OpenGL function pointer table");
            builder.Append("namespace StgSharp.Graphics.OpenGL\n");
            builder.Append("{\n");
            builder.Append("    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]\n");
            builder.Append("    internal struct glContextShadow\n");
            builder.Append("    {\n");
            foreach (GlCommandDefinition command in OrderCommands(commands))
            {
                builder.Append("        internal nint ");
                builder.Append(command.Name);
                builder.Append(";\n");
            }

            builder.Append("    }\n");
            builder.Append("}\n");
            return builder.ToString();
        }

        private static IEnumerable<GlCommandDefinition> OrderCommands(
                                                                       IReadOnlyList<GlCommandDefinition> commands
        )
        {
            return commands.OrderBy(
                value => value.Name,
                StringComparer.Ordinal);
        }

        private static string GenerateFunctions(
                                                IReadOnlyList<ManagedMethodDefinition> methods,
                                                string description,
                                                bool includeContextState
        )
        {
            StringBuilder builder = new StringBuilder(512 * 1024);
            AppendGeneratedHeader(builder, description);
            builder.Append("using StgSharp.Mathematics.Graphics;\n\n");
            builder.Append("using System;\n");
            builder.Append("using System.Runtime.CompilerServices;\n\n");
            builder.Append("namespace StgSharp.Graphics.OpenGL\n");
            builder.Append("{\n");
            builder.Append("    public unsafe partial class OpenGLFunction\n");
            builder.Append("    {\n");
            if (includeContextState)
            {
                builder.Append("\n        private readonly OpenglContext* __context;\n");
                builder.Append("\n        internal OpenGLFunction(OpenglContext* context)\n");
                builder.Append("        {\n");
                builder.Append("            __context = context;\n");
                builder.Append("        }\n");
            }

            foreach (ManagedMethodDefinition method in methods)
            {
                AppendMethodDocumentation(builder, method);
                builder.Append("\n        [MethodImpl(MethodImplOptions.AggressiveInlining)]\n");
                builder.Append("        public ");
                builder.Append(method.ReturnType);
                builder.Append(' ');
                builder.Append(method.Name);
                builder.Append('(');
                for (int index = 0; index < method.Parameters.Count; index++)
                {
                    if (index != 0)
                    {
                        builder.Append(", ");
                    }

                    ManagedParameterDefinition parameter = method.Parameters[index];
                    builder.Append(parameter.Type);
                    builder.Append(' ');
                    builder.Append(EscapeIdentifier(parameter.Name));
                }

                builder.Append(")\n");
                builder.Append("        {\n");
                AppendInvocation(builder, method);
                builder.Append("        }\n");
            }

            builder.Append("\n    }\n");
            builder.Append("}\n");
            return builder.ToString();
        }

        private static void AppendMethodDocumentation(
            StringBuilder builder,
            ManagedMethodDefinition method
        )
        {
            string declaration = ReadNativeDeclaration(method.NativeCommand);
            string page = method.DocumentationPage ?? method.NativeCommand.Name;
            builder.Append("\n        /// <summary>\n");
            builder.Append("        /// <para>Forwards to the native OpenGL entry point.</para>\n");
            builder.Append("        /// <para><c>");
            builder.Append(EscapeXml(declaration));
            builder.Append("</c></para>\n");
            builder.Append("        /// <para>See <see href=\"https://docs.gl/gl4/");
            builder.Append(page);
            builder.Append("\">docs.gl</see>.</para>\n");
            builder.Append("        /// </summary>");
        }

        private static string ReadNativeDeclaration(
            GlCommandDefinition command
        )
        {
            return $"{command.Return.DeclarationText}(" +
                   string.Join(", ", command.Parameters.Select(value => value.DeclarationText)) +
                   ");";
        }

        private static string EscapeXml(
            string value
        )
        {
            return value.Replace("&", "&amp;", StringComparison.Ordinal)
                        .Replace("<", "&lt;", StringComparison.Ordinal)
                        .Replace(">", "&gt;", StringComparison.Ordinal)
                        .Replace("\"", "&quot;", StringComparison.Ordinal)
                        .Replace("'", "&apos;", StringComparison.Ordinal);
        }

        private static void AppendInvocation(
            StringBuilder builder,
            ManagedMethodDefinition method
        )
        {
            switch (method.Invocation)
            {
                case DirectInvocation direct:
                    AppendDirectInvocation(builder, method, direct);
                    break;
                case PinnedSpanInvocation pinned:
                    AppendPinnedSpanInvocation(builder, method, pinned);
                    break;
                case PinnedVectorSpanInvocation pinnedVector:
                    AppendPinnedVectorSpanInvocation(builder, method, pinnedVector);
                    break;
                case Matrix4Invocation matrix:
                    AppendMatrix4Invocation(builder, method, matrix);
                    break;
                case QuerySpanInvocation query:
                    AppendQuerySpanInvocation(builder, method, query);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported managed invocation '{method.Invocation.GetType().Name}'.");
            }
        }

        private static void AppendQuerySpanInvocation(
            StringBuilder builder,
            ManagedMethodDefinition method,
            QuerySpanInvocation invocation
        )
        {
            ManagedParameterDefinition span = method.Parameters[invocation.SpanParameterIndex];
            string spanName = EscapeIdentifier(span.Name);
            builder.Append("            fixed (");
            builder.Append(invocation.ElementType);
            builder.Append("* __value = ");
            builder.Append(spanName);
            builder.Append(")\n");
            builder.Append("            {\n");
            builder.Append("                __context->");
            builder.Append(method.NativeCommand.Name);
            builder.Append('(');
            AppendArguments(builder, method, invocation.PrefixArguments);
            if (invocation.PrefixArguments.Count != 0)
            {
                builder.Append(", ");
            }

            builder.Append("__value);\n");
            builder.Append("            }\n");
        }

        private static void AppendDirectInvocation(
            StringBuilder builder,
            ManagedMethodDefinition method,
            DirectInvocation invocation
        )
        {
            List<int> fixedParameters = new List<int>();
            for (int index = 0; index < method.Parameters.Count; index++)
            {
                if (method.Parameters[index].IsHandleSpan)
                {
                    fixedParameters.Add(index);
                }
            }

            int indentation = 12;
            foreach (int parameterIndex in fixedParameters)
            {
                ManagedParameterDefinition parameter = method.Parameters[parameterIndex];
                builder.Append(' ', indentation);
                builder.Append("fixed (GlHandle* __");
                builder.Append(parameter.Name);
                builder.Append(" = ");
                builder.Append(EscapeIdentifier(parameter.Name));
                builder.Append(")\n");
                builder.Append(' ', indentation);
                builder.Append("{\n");
                indentation += 4;
            }

            builder.Append(' ', indentation);
            if (method.WrapHandleReturn)
            {
                builder.Append("return GlHandle.Create(");
            }
            else if (method.ReturnType != "void")
            {
                builder.Append("return ");
            }

            AppendNativeCall(builder, method, invocation.Arguments);
            builder.Append(method.WrapHandleReturn ? ");\n" : ";\n");
            for (int index = fixedParameters.Count - 1; index >= 0; index--)
            {
                indentation -= 4;
                builder.Append(' ', indentation);
                builder.Append("}\n");
            }
        }

        private static void AppendPinnedSpanInvocation(
            StringBuilder builder,
            ManagedMethodDefinition method,
            PinnedSpanInvocation invocation
        )
        {
            ManagedParameterDefinition span = method.Parameters[invocation.SpanParameterIndex];
            builder.Append("            fixed (");
            builder.Append(invocation.ElementType);
            builder.Append("* __value = ");
            builder.Append(EscapeIdentifier(span.Name));
            builder.Append(")\n");
            builder.Append("            {\n");
            builder.Append("                __context->");
            builder.Append(method.NativeCommand.Name);
            builder.Append('(');
            AppendArguments(builder, method, invocation.PrefixArguments);
            if (invocation.PrefixArguments.Count != 0)
            {
                builder.Append(", ");
            }

            builder.Append(EscapeIdentifier(span.Name));
            builder.Append(".Length, __value);\n");
            builder.Append("            }\n");
        }

        private static void AppendPinnedVectorSpanInvocation(
            StringBuilder builder,
            ManagedMethodDefinition method,
            PinnedVectorSpanInvocation invocation
        )
        {
            ManagedParameterDefinition span = method.Parameters[invocation.SpanParameterIndex];
            string spanName = EscapeIdentifier(span.Name);
            builder.Append("            fixed (");
            builder.Append(invocation.VectorType);
            builder.Append("* __vector = ");
            builder.Append(spanName);
            builder.Append(")\n");
            builder.Append("            {\n");
            builder.Append("                __context->");
            builder.Append(method.NativeCommand.Name);
            builder.Append('(');
            AppendArguments(builder, method, invocation.PrefixArguments);
            if (invocation.PrefixArguments.Count != 0)
            {
                builder.Append(", ");
            }

            builder.Append(spanName);
            builder.Append(".Length, (");
            builder.Append(invocation.ComponentType);
            builder.Append("*)__vector);\n");
            builder.Append("            }\n");
        }

        private static void AppendMatrix4Invocation(
            StringBuilder builder,
            ManagedMethodDefinition method,
            Matrix4Invocation invocation
        )
        {
            ManagedParameterDefinition transpose = method.Parameters[invocation.TransposeParameterIndex];
            ManagedParameterDefinition matrix = method.Parameters[invocation.MatrixParameterIndex];
            string transposeName = EscapeIdentifier(transpose.Name);
            string matrixName = EscapeIdentifier(matrix.Name);
            if (invocation.IsSpan)
            {
                builder.Append("            fixed (GraphicsMatrix* __value = ");
                builder.Append(matrixName);
                builder.Append(")\n");
                builder.Append("            {\n");
                builder.Append("                __context->");
            }
            else
            {
                builder.Append("            __context->");
            }

            builder.Append(method.NativeCommand.Name);
            builder.Append('(');
            AppendArguments(builder, method, invocation.PrefixArguments);
            if (invocation.PrefixArguments.Count != 0)
            {
                builder.Append(", ");
            }

            builder.Append(invocation.IsSpan ? $"{matrixName}.Length" : "1");
            builder.Append(", ");
            builder.Append(transposeName);
            builder.Append(" ? (byte)1 : (byte)0, (float*)");
            builder.Append(invocation.IsSpan ? "__value" : $"&{matrixName}");
            builder.Append(");\n");
            if (invocation.IsSpan)
            {
                builder.Append("            }\n");
            }
        }

        private static void AppendNativeCall(
            StringBuilder builder,
            ManagedMethodDefinition method,
            IReadOnlyList<ManagedArgument> arguments
        )
        {
            builder.Append("__context->");
            builder.Append(method.NativeCommand.Name);
            builder.Append('(');
            AppendArguments(builder, method, arguments);
            builder.Append(')');
        }

        private static void AppendArguments(
            StringBuilder builder,
            ManagedMethodDefinition method,
            IReadOnlyList<ManagedArgument> arguments
        )
        {
            for (int index = 0; index < arguments.Count; index++)
            {
                if (index != 0)
                {
                    builder.Append(", ");
                }

                switch (arguments[index])
                {
                    case ParameterArgument parameterArgument:
                        ManagedParameterDefinition parameter = method.Parameters[
                            parameterArgument.ParameterIndex];
                        string parameterName = EscapeIdentifier(parameter.Name);
                        if (!parameter.IsHandle)
                        {
                            builder.Append(parameterName);
                        }
                        else if (parameter.Type.EndsWith('*'))
                        {
                            builder.Append("(uint*)");
                            builder.Append(parameterName);
                        }
                        else if (parameter.IsHandleSpan)
                        {
                            builder.Append("(uint*)__");
                            builder.Append(parameter.Name);
                        }
                        else
                        {
                            builder.Append(parameterName);
                            builder.Append(".Value");
                        }

                        break;
                    case SpanLengthArgument spanLengthArgument:
                        builder.Append(
                            EscapeIdentifier(
                                method.Parameters[spanLengthArgument.ParameterIndex].Name));
                        builder.Append(".Length");
                        break;
                    case VectorComponentArgument componentArgument:
                        builder.Append(
                            EscapeIdentifier(
                                method.Parameters[componentArgument.ParameterIndex].Name));
                        builder.Append('.');
                        builder.Append(componentArgument.Component);
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"Unsupported managed argument '{arguments[index].GetType().Name}'.");
                }
            }
        }

        private static string EscapeIdentifier(
                                               string value
        )
        {
            return _csharpKeywords.Contains(value) ? $"@{value}" : value;
        }

        private static void AppendGeneratedHeader(
                                                  StringBuilder builder,
                                                  string description
        )
        {
            builder.Append("// <auto-generated>\n");
            builder.Append("// Generated by StgSharp.GenerateGL.\n");
            builder.Append("// ");
            builder.Append(description);
            builder.Append(".\n");
            builder.Append("// </auto-generated>\n\n");
        }

        private sealed record ConstantExpression(
            string Type,
            string Literal
        );

        private sealed record GlCommandGroup(
            string FileName,
            string Description,
            IReadOnlyList<GlCommandDefinition> Commands
        );

        private sealed record ManagedCommandGroup(
            GlCommandGroup Source,
            IReadOnlyList<ManagedMethodDefinition> Methods
        );

    }
}
