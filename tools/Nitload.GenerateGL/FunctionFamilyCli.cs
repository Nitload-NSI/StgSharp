// -----------------------------------------------------------------------------
// file="FunctionFamilyCli"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StgSharp.GenerateGL.Analysis;

namespace StgSharp.GenerateGL
{
    /// <summary>
    ///   Presents the function-family analysis without affecting generation.
    /// </summary>
    internal static class FunctionFamilyCli
    {

        private const int ShowAllResults = int.MaxValue;
        private const int QueryErrorExitCode = 1;
        private const int SuccessExitCode = 0;
        private const int UsageErrorExitCode = 2;

        public static int Execute(
            GlFunctionFamilyAnalysis analysis,
            IReadOnlyList<string> arguments
        )
        {
            ArgumentNullException.ThrowIfNull(analysis);
            ArgumentNullException.ThrowIfNull(arguments);

            if (arguments.Count == 0)
            {
                WriteHelp();
                return SuccessExitCode;
            }

            string command = arguments[0].ToUpperInvariant();
            string[] queryArguments = arguments.Skip(1).ToArray();
            return command switch
            {
                "SUMMARY" => QuerySummary(analysis, queryArguments),
                "LIST" or "LS" => QueryList(analysis, queryArguments),
                "SHOW" or "INFO" => QueryShow(analysis, queryArguments),
                "RELATIONS" or "PAIRS" => QueryRelations(analysis, queryArguments),
                "UNRESOLVED" or "UNCLASSIFIED" => QueryUnclassified(
                    analysis,
                    queryArguments),
                "CONFLICTS" or "CONSTRAINTS" => QueryConstraints(
                    analysis,
                    queryArguments),
                "HELP" or "?" => WriteHelpResult(queryArguments),
                _ => WriteUsageError($"Unknown family command '{arguments[0]}'."),
            };
        }

        public static void WriteHelp()
        {
            Console.WriteLine("Function-family analysis commands:");
            Console.WriteLine("  families summary");
            Console.WriteLine("  families list [filter] [--showcount <n>]");
            Console.WriteLine("  families show <family>");
            Console.WriteLine("  families relations [filter] [--showcount <n>]");
            Console.WriteLine("  families unresolved [filter] [--showcount <n>]");
            Console.WriteLine("  families conflicts [filter] [--showcount <n>]");
        }

        private static int QuerySummary(
            GlFunctionFamilyAnalysis analysis,
            string[] arguments
        )
        {
            if (arguments.Length != 0)
            {
                return WriteUsageError("families summary does not accept arguments.");
            }

            Console.WriteLine("Function-family analysis");
            WriteProperty("families", analysis.Families.Count);
            WriteProperty(
                "classified commands",
                analysis.Families.SelectMany(value => value.Members)
                                  .Select(value => value.Command.Name)
                                  .Distinct(StringComparer.Ordinal)
                                  .Count());
            WriteProperty("unclassified commands", analysis.UnclassifiedCommands.Count);
            WriteProperty("relations", analysis.Relations.Count);
            WriteProperty(
                "registry vector relations",
                analysis.Relations.Count(
                    value => value.Kind == GlFunctionRelationKind.RegistryVectorEquivalent));
            WriteProperty(
                "inferred len=1 relations",
                analysis.Relations.Count(
                    value => value.Kind == GlFunctionRelationKind.InferredSingleValuePointer));
            WriteProperty(
                "verified relations",
                analysis.Relations.Count(
                    value => value.State == GlFunctionRelationState.Verified));
            WriteProperty(
                "relations requiring review",
                analysis.Relations.Count(
                    value => value.State != GlFunctionRelationState.Verified));
            WriteProperty(
                "shape suffix must remain",
                analysis.Families.Count(
                    value => value.ShapeNamePolicy == GlFunctionShapeNamePolicy.MustRetain));
            return SuccessExitCode;
        }

        private static int QueryList(
            GlFunctionFamilyAnalysis analysis,
            IReadOnlyList<string> arguments
        )
        {
            if (!TryReadFilterOptions(
                    arguments,
                    out string? filter,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            GlFunctionFamily[] families = FilterFamilies(analysis, filter).ToArray();
            if (families.Length == 0)
            {
                Console.Error.WriteLine(
                    filter is null
                        ? "No function families were recognized."
                        : $"No function families contain '{filter}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(families.Length, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                GlFunctionFamily family = families[index];
                Console.WriteLine($"[{index + 1}] {family.Name}");
                WriteProperty("members", family.Members.Count);
                WriteProperty(
                    "shapes",
                    JoinDistinct(
                        family.Members.Select(value => value.NameShape?.Shape)));
                WriteProperty(
                    "types",
                    JoinDistinct(
                        family.Members.Select(value => value.NameShape?.TypeSuffix)));
                WriteProperty(
                    "forms",
                    JoinDistinct(
                        family.Members.Where(value => value.NameShape is not null)
                                      .Select(
                                          value => value.NameShape!.IsVectorForm
                                              ? "pointer"
                                              : "scalar")));
                WriteProperty("relations", family.Relations.Count);
                WriteProperty("shape name", ReadShapePolicy(family.ShapeNamePolicy));
            }

            WriteTruncation(families.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryShow(
            GlFunctionFamilyAnalysis analysis,
            string[] arguments
        )
        {
            if (arguments.Length != 1)
            {
                return WriteUsageError("families show requires one exact family name.");
            }

            GlFunctionFamily? family = analysis.Families.FirstOrDefault(
                value => string.Equals(
                    value.Name,
                    arguments[0],
                    StringComparison.OrdinalIgnoreCase));
            if (family is null)
            {
                Console.Error.WriteLine($"No function family named '{arguments[0]}'.");
                Console.Error.WriteLine($"Try: families list {arguments[0]}");
                return QueryErrorExitCode;
            }

            Console.WriteLine($"function family: {family.Name}");
            WriteProperty("members", family.Members.Count);
            WriteProperty("shape name", ReadShapePolicy(family.ShapeNamePolicy));
            for (int index = 0; index < family.Members.Count; index++)
            {
                GlFunctionFamilyMember member = family.Members[index];
                Console.WriteLine($"    [{index + 1}] {member.Command.Name}");
                if (member.NameShape is not null)
                {
                    if (member.NameShape.Shape is not null)
                    {
                        WriteIndentedProperty("shape", member.NameShape.Shape, 8);
                    }

                    WriteIndentedProperty(
                        "type suffix",
                        string.IsNullOrWhiteSpace(member.NameShape.TypeSuffix)
                            ? "-"
                            : member.NameShape.TypeSuffix,
                        8);
                    WriteIndentedProperty(
                        "form",
                        member.NameShape.IsVectorForm ? "pointer" : "scalar",
                        8);
                }

                WriteIndentedProperty(
                    "parameters",
                    ReadParameterList(member.Command),
                    8);
            }

            WriteRelations(family.Relations);
            if (family.Diagnostics.Count != 0)
            {
                Console.WriteLine("  diagnostics:");
                foreach (string diagnostic in family.Diagnostics)
                {
                    Console.WriteLine($"    - {diagnostic}");
                }
            }

            return SuccessExitCode;
        }

        private static int QueryRelations(
            GlFunctionFamilyAnalysis analysis,
            IReadOnlyList<string> arguments
        )
        {
            if (!TryReadFilterOptions(
                    arguments,
                    out string? filter,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            GlFunctionRelation[] relations = analysis.Relations
                .Where(
                    value => filter is null ||
                             value.SourceName.Contains(
                                 filter,
                                 StringComparison.OrdinalIgnoreCase) ||
                             value.TargetName.Contains(
                                 filter,
                                 StringComparison.OrdinalIgnoreCase))
                .OrderBy(value => value.SourceName, StringComparer.Ordinal)
                .ThenBy(value => value.TargetName, StringComparer.Ordinal)
                .ToArray();
            if (relations.Length == 0)
            {
                Console.Error.WriteLine(
                    filter is null
                        ? "No function relations were recognized."
                        : $"No function relations contain '{filter}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(relations.Length, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                WriteRelation(index + 1, relations[index]);
            }

            WriteTruncation(relations.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryUnclassified(
            GlFunctionFamilyAnalysis analysis,
            IReadOnlyList<string> arguments
        )
        {
            if (!TryReadFilterOptions(
                    arguments,
                    out string? filter,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            string[] commands = analysis.UnclassifiedCommands
                .Select(value => value.Name)
                .Where(
                    value => filter is null || value.Contains(
                        filter,
                        StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (commands.Length == 0)
            {
                Console.Error.WriteLine(
                    filter is null
                        ? "Every command belongs to a candidate family."
                        : $"No unclassified commands contain '{filter}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(commands.Length, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                Console.WriteLine($"[{index + 1}] {commands[index]}");
            }

            WriteTruncation(commands.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryConstraints(
            GlFunctionFamilyAnalysis analysis,
            IReadOnlyList<string> arguments
        )
        {
            if (!TryReadFilterOptions(
                    arguments,
                    out string? filter,
                    out int limit,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            GlFunctionFamily[] families = FilterFamilies(analysis, filter)
                .Where(
                    value => value.ShapeNamePolicy == GlFunctionShapeNamePolicy.MustRetain ||
                             value.Diagnostics.Count != 0 ||
                             value.Relations.Any(
                                 relation => relation.State != GlFunctionRelationState.Verified))
                .ToArray();
            if (families.Length == 0)
            {
                Console.Error.WriteLine(
                    filter is null
                        ? "No family constraints require review."
                        : $"No constrained families contain '{filter}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(families.Length, limit);
            for (int index = 0; index < displayedCount; index++)
            {
                GlFunctionFamily family = families[index];
                Console.WriteLine($"[{index + 1}] {family.Name}");
                if (family.ShapeNamePolicy == GlFunctionShapeNamePolicy.MustRetain)
                {
                    WriteProperty(
                        "constraint",
                        "shape suffix must remain because normalized signatures collide");
                }

                foreach (string diagnostic in family.Diagnostics)
                {
                    WriteProperty("diagnostic", diagnostic);
                }
            }

            WriteTruncation(families.Length, displayedCount);
            return SuccessExitCode;
        }

        private static IEnumerable<GlFunctionFamily> FilterFamilies(
            GlFunctionFamilyAnalysis analysis,
            string? filter
        )
        {
            return analysis.Families.Where(
                value => filter is null || value.Name.Contains(
                    filter,
                    StringComparison.OrdinalIgnoreCase));
        }

        private static bool TryReadFilterOptions(
            IReadOnlyList<string> arguments,
            out string? filter,
            out int limit,
            out string? error
        )
        {
            filter = null;
            limit = ShowAllResults;
            int index = 0;
            if (index < arguments.Count && !arguments[index].StartsWith(
                    "--",
                    StringComparison.Ordinal))
            {
                filter = arguments[index++];
            }

            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if ((argument == "--showcount" || argument == "--limit") &&
                    index < arguments.Count && int.TryParse(
                        arguments[index++],
                        NumberStyles.None,
                        CultureInfo.InvariantCulture,
                        out limit) && limit > 0)
                {
                    continue;
                }

                error = $"Unknown or invalid family option '{argument}'.";
                return false;
            }

            error = null;
            return true;
        }

        private static string ReadParameterList(
            Registry.GlCommandDefinition command
        )
        {
            return string.Join(
                ", ",
                command.Parameters.Select(value => value.DeclarationText));
        }

        private static void WriteRelations(
            IReadOnlyList<GlFunctionRelation> relations
        )
        {
            if (relations.Count == 0)
            {
                return;
            }

            Console.WriteLine("  relations:");
            for (int index = 0; index < relations.Count; index++)
            {
                WriteRelation(index + 1, relations[index], 4);
            }
        }

        private static void WriteRelation(
            int index,
            GlFunctionRelation relation,
            int indentation = 0
        )
        {
            string prefix = new string(' ', indentation);
            Console.WriteLine(
                $"{prefix}[{index}] {relation.SourceName} -> {relation.TargetName}");
            WriteIndentedProperty(
                "source",
                relation.Kind == GlFunctionRelationKind.RegistryVectorEquivalent
                    ? "gl.xml vecequiv"
                    : "inferred len=1 pair",
                indentation + 4);
            WriteIndentedProperty(
                "state",
                ReadRelationState(relation.State),
                indentation + 4);
            WriteIndentedProperty("detail", relation.Detail, indentation + 4);
        }

        private static string ReadShapePolicy(
            GlFunctionShapeNamePolicy policy
        )
        {
            return policy switch
            {
                GlFunctionShapeNamePolicy.NotApplicable => "not applicable",
                GlFunctionShapeNamePolicy.CanRemove => "may be removed",
                GlFunctionShapeNamePolicy.MustRetain => "must remain",
                _ => throw new ArgumentOutOfRangeException(nameof(policy)),
            };
        }

        private static string ReadRelationState(
            GlFunctionRelationState state
        )
        {
            return state switch
            {
                GlFunctionRelationState.Verified => "verified",
                GlFunctionRelationState.MissingTarget => "missing target",
                GlFunctionRelationState.Rejected => "rejected",
                _ => throw new ArgumentOutOfRangeException(nameof(state)),
            };
        }

        private static string JoinDistinct(
            IEnumerable<string?> values
        )
        {
            string[] distinct = values.Where(value => !string.IsNullOrWhiteSpace(value))
                                      .Select(value => value!)
                                      .Distinct(StringComparer.Ordinal)
                                      .OrderBy(value => value, StringComparer.Ordinal)
                                      .ToArray();
            return distinct.Length == 0 ? "-" : string.Join(", ", distinct);
        }

        private static int WriteHelpResult(
            string[] arguments
        )
        {
            if (arguments.Length != 0)
            {
                return WriteUsageError("families help does not accept arguments.");
            }

            WriteHelp();
            return SuccessExitCode;
        }

        private static int WriteUsageError(
            string? error
        )
        {
            Console.Error.WriteLine($"error: {error}");
            return UsageErrorExitCode;
        }

        private static void WriteProperty(
            string name,
            object value
        )
        {
            WriteIndentedProperty(name, value, 2);
        }

        private static void WriteIndentedProperty(
            string name,
            object value,
            int indentation
        )
        {
            Console.WriteLine($"{new string(' ', indentation)}{name}: {value}");
        }

        private static void WriteTruncation(
            int totalCount,
            int displayedCount
        )
        {
            if (displayedCount != totalCount)
            {
                Console.WriteLine($"... {totalCount - displayedCount} more result(s)");
            }
        }

    }
}
