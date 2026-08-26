// -----------------------------------------------------------------------------
// file="Program"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;

namespace StgSharp.GenerateGL
{
    internal static class Program
    {

        private static int Main(
                           string[] args
        )
        {
            try
            {
                return XmlCli.Run(args);
            }
            catch (IOException exception)
            {
                return WriteError(exception);
            }
            catch (XmlException exception)
            {
                return WriteError(exception);
            }
            catch (InvalidDataException exception)
            {
                return WriteError(exception);
            }
            catch (UnauthorizedAccessException exception)
            {
                return WriteError(exception);
            }
            catch (ArgumentException exception)
            {
                return WriteError(exception);
            }
            catch (NotSupportedException exception)
            {
                return WriteError(exception);
            }
        }

        private static int WriteError(
                           Exception exception
        )
        {
            Console.Error.WriteLine($"error: {exception.Message}");
            return XmlCli.LoadErrorExitCode;
        }

    }
}
