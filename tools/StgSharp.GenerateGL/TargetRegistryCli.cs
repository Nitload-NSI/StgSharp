// -----------------------------------------------------------------------------
// file="TargetRegistryCli"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL
{
    internal static class TargetRegistryCli
    {

        private const int SuccessExitCode = 0;
        private const int UsageErrorExitCode = 2;

        public static int Execute(
            GlRegistryProjection projection,
            IReadOnlyList<string> arguments
        )
        {
            ArgumentNullException.ThrowIfNull(projection);
            ArgumentNullException.ThrowIfNull(arguments);

            if (arguments.Count > 1 || arguments.Count == 1 && !string.Equals(
                    arguments[0],
                    "summary",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("error: target accepts only the 'summary' command.");
                return UsageErrorExitCode;
            }

            Console.WriteLine("Registry target projection");
            WriteProperty("api", projection.Api);
            WriteProperty("profile", projection.Profile);
            WriteProperty("maximum version", projection.MaximumVersion);
            WriteProperty("features", projection.Features.Count);
            WriteProperty("commands", projection.Commands.Count);
            WriteProperty("enums", projection.Enums.Count);
            WriteProperty("extensions", 0);
            WriteProperty("source commands", projection.Source.Commands.Count);
            WriteProperty("source extensions", projection.Source.Extensions.Count);
            return SuccessExitCode;
        }

        private static void WriteProperty(
            string name,
            object value
        )
        {
            Console.WriteLine($"  {name}: {value}");
        }

    }
}
