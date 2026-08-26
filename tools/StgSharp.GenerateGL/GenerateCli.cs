// -----------------------------------------------------------------------------
// file="GenerateCli"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StgSharp.GenerateGL.Generation;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL
{
    /// <summary>
    ///   Owns the command-line boundary of the future code generation pipeline.
    /// </summary>
    internal static class GenerateCli
    {

        private const int QueryErrorExitCode = 1;
        private const int SuccessExitCode = 0;
        private const int UsageErrorExitCode = 2;

        public static int Generate(
                                   GlRegistryModel registry,
                                   IReadOnlyList<string> arguments,
                                   out GeneratedFileSet? generatedFiles
        )
        {
            ArgumentNullException.ThrowIfNull(registry);
            ArgumentNullException.ThrowIfNull(arguments);
            generatedFiles = null;

            if (arguments.Count == 1 && arguments[0] is "--help" or "-h")
            {
                WriteHelp();
                return SuccessExitCode;
            }

            if (arguments.Count != 0)
            {
                Console.Error.WriteLine(
                    $"error: unknown generate option '{arguments[0]}'.");
                Console.Error.WriteLine("Run 'generate --help' for command information.");
                return UsageErrorExitCode;
            }

            try
            {
                generatedFiles = OpenGlGenerator.Generate(registry);
            }
            catch (InvalidDataException exception)
            {
                Console.Error.WriteLine($"error: {exception.Message}");
                return QueryErrorExitCode;
            }
            catch (InvalidOperationException exception)
            {
                Console.Error.WriteLine($"error: {exception.Message}");
                return QueryErrorExitCode;
            }

            Console.WriteLine(
                $"Generated {generatedFiles.Files.Count} file(s) in memory.");
            Console.WriteLine("Use 'list files' and 'preview <filename>' to inspect them.");
            Console.WriteLine("No files were written to disk.");
            return SuccessExitCode;
        }

        public static int ListFiles(
                                    GeneratedFileSet files,
                                    IReadOnlyList<string> arguments
        )
        {
            ArgumentNullException.ThrowIfNull(files);
            ArgumentNullException.ThrowIfNull(arguments);
            if (arguments.Count != 0)
            {
                Console.Error.WriteLine("error: list files does not accept arguments.");
                return UsageErrorExitCode;
            }

            if (files.Files.Count == 0)
            {
                Console.Error.WriteLine("No generated files are held in this session.");
                Console.Error.WriteLine("Run 'generate' first.");
                return QueryErrorExitCode;
            }

            Console.WriteLine("Generated files (memory only)");
            for (int index = 0; index < files.Files.Count; index++)
            {
                GeneratedFile file = files.Files[index];
                Console.WriteLine(
                    $"  [{index + 1}] {file.Name} ({file.Content.Length} characters)");
            }

            return SuccessExitCode;
        }

        public static int Preview(
                                  GeneratedFileSet files,
                                  IReadOnlyList<string> arguments
        )
        {
            ArgumentNullException.ThrowIfNull(files);
            ArgumentNullException.ThrowIfNull(arguments);
            if (arguments.Count != 1)
            {
                Console.Error.WriteLine("error: preview requires one generated file name.");
                return UsageErrorExitCode;
            }

            if (!files.TryGetFile(arguments[0], out GeneratedFile file))
            {
                Console.Error.WriteLine(
                    $"No generated file named '{arguments[0]}' is held in this session.");
                if (files.Files.Count == 0)
                {
                    Console.Error.WriteLine("Run 'generate' first.");
                }
                else
                {
                    Console.Error.WriteLine(
                        $"Available files: {string.Join(", ", files.Files.Select(value => value.Name))}");
                }

                return QueryErrorExitCode;
            }

            Console.Write(file.Content);
            return SuccessExitCode;
        }

        public static void WriteHelp()
        {
            Console.WriteLine("Generate C# bindings from the loaded OpenGL registry.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  generate");
            Console.WriteLine("  generate --help");
            Console.WriteLine();
            Console.WriteLine("Status:");
            Console.WriteLine("  Output is retained in the current process only.");
            Console.WriteLine("  Running 'generate' does not create or modify disk files.");
            Console.WriteLine();
            Console.WriteLine("Inspection:");
            Console.WriteLine("  list files");
            Console.WriteLine("  preview <filename>");
        }

    }
}
