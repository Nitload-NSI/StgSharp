// -----------------------------------------------------------------------------
// file="CSharpAbiTypePolicy"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace StgSharp.GenerateGL.Generation
{
    /// <summary>
    ///   Defines C ABI primitives and external Khronos platform types that cannot
    ///   be inferred from gl.xml.
    /// </summary>
    internal static class CSharpAbiTypePolicy
    {

        private static readonly Dictionary<string, string> _types = new Dictionary<string, string>(
            StringComparer.Ordinal)
        {
            ["void"] = "void",
            ["char"] = "byte",
            ["signed char"] = "sbyte",
            ["unsigned char"] = "byte",
            ["short"] = "short",
            ["short int"] = "short",
            ["signed short"] = "short",
            ["signed short int"] = "short",
            ["unsigned short"] = "ushort",
            ["unsigned short int"] = "ushort",
            ["int"] = "int",
            ["signed"] = "int",
            ["signed int"] = "int",
            ["unsigned"] = "uint",
            ["unsigned int"] = "uint",
            ["float"] = "float",
            ["double"] = "double",
            ["khronos_int8_t"] = "sbyte",
            ["khronos_uint8_t"] = "byte",
            ["khronos_int16_t"] = "short",
            ["khronos_uint16_t"] = "ushort",
            ["khronos_int32_t"] = "int",
            ["khronos_uint32_t"] = "uint",
            ["khronos_int64_t"] = "long",
            ["khronos_uint64_t"] = "ulong",
            ["khronos_intptr_t"] = "nint",
            ["khronos_uintptr_t"] = "nuint",
            ["khronos_ssize_t"] = "nint",
            ["khronos_usize_t"] = "nuint",
            ["khronos_float_t"] = "float",
        };

        public static bool TryResolve(
                                      string typeName,
                                      out string equivalentType
        )
        {
            return _types.TryGetValue(typeName, out equivalentType!);
        }

    }
}
