// -----------------------------------------------------------------------------
// file="glApiManager"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.Internal;

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Nitload.Graphics.OpenGL
{
    /// <summary>
    ///   Compatibility loader retained until the context subsystem is replaced.
    /// </summary>
    public static partial class glManager
    {

        private const uint GlVersion = 0x1F02;

        private static bool core10;
        private static bool core11;
        private static bool core12;
        private static bool core13;
        private static bool core14;
        private static bool core15;
        private static bool core20;
        private static bool core21;
        private static bool core30;
        private static bool core31;
        private static bool core32;
        private static bool core33;
        private static bool core40;
        private static bool core41;
        private static bool core42;
        private static bool core43;
        private static bool core44;
        private static bool core45;
        private static bool core46;

        /// <summary>
        ///   Loads the core OpenGL entry points for the current GLFW context.
        /// </summary>
        public static unsafe void LoadOpenGLApiTo(
                                  IntPtr contextHandle
        )
        {
            if (contextHandle == IntPtr.Zero) {
                throw new ArgumentException("A context table is required.", nameof(contextHandle));
            }

            OpenglContext* context = (OpenglContext*)contextHandle;
            if (context->glGetString != null) {
                return;
            }

            delegate* <ReadOnlySpan<byte>, IntPtr> loader = &GraphicFramework.glfwGetProcAddress;
            IntPtr getString = GraphicFramework.glfwGetProcAddress("glGetString"u8);
            if (getString == IntPtr.Zero) {
                throw new NotSupportedException("Cannot load glGetString from the current context.");
            }

            context->glGetString = (delegate* unmanaged[Cdecl]<uint, byte*>)getString;
            byte* versionPointer = context->glGetString(GlVersion);
            if (versionPointer == null) {
                throw new NotSupportedException("The current context did not report an OpenGL version.");
            }

            string versionText = Marshal.PtrToStringAnsi((nint)versionPointer) ?? string.Empty;
            if (!TryReadVersion(versionText, out int majorVersion, out int minorVersion)) {
                throw new NotSupportedException($"Cannot parse OpenGL version '{versionText}'.");
            }

            CheckCoreVersion(majorVersion, minorVersion);

            _ = LoadGLcore10((glContextShadow*)context, loader) &&
                LoadGLcore11((glContextShadow*)context, loader) &&
                LoadGLcore12((glContextShadow*)context, loader) &&
                LoadGLcore13((glContextShadow*)context, loader) &&
                LoadGLcore14((glContextShadow*)context, loader) &&
                LoadGLcore15((glContextShadow*)context, loader) &&
                LoadGLcore20((glContextShadow*)context, loader) &&
                LoadGLcore21((glContextShadow*)context, loader) &&
                LoadGLcore30((glContextShadow*)context, loader) &&
                LoadGLcore31((glContextShadow*)context, loader) &&
                LoadGLcore32((glContextShadow*)context, loader) &&
                LoadGLcore33((glContextShadow*)context, loader) &&
                LoadGLcore40((glContextShadow*)context, loader) &&
                LoadGLcore41((glContextShadow*)context, loader) &&
                LoadGLcore42((glContextShadow*)context, loader) &&
                LoadGLcore43((glContextShadow*)context, loader) &&
                LoadGLcore44((glContextShadow*)context, loader) &&
                LoadGLcore45((glContextShadow*)context, loader) &&
                LoadGLcore46((glContextShadow*)context, loader);
        }

        internal static void CheckCoreVersion(
                             int majorVersion,
                             int minorVersion
        )
        {
            if (majorVersion < 3) {
                throw new InvalidOperationException(
                    "StgSharp does not support OpenGL immediate mode.");
            }

            core10 = IsAtLeast(majorVersion, minorVersion, 1, 0);
            core11 = IsAtLeast(majorVersion, minorVersion, 1, 1);
            core12 = IsAtLeast(majorVersion, minorVersion, 1, 2);
            core13 = IsAtLeast(majorVersion, minorVersion, 1, 3);
            core14 = IsAtLeast(majorVersion, minorVersion, 1, 4);
            core15 = IsAtLeast(majorVersion, minorVersion, 1, 5);
            core20 = IsAtLeast(majorVersion, minorVersion, 2, 0);
            core21 = IsAtLeast(majorVersion, minorVersion, 2, 1);
            core30 = IsAtLeast(majorVersion, minorVersion, 3, 0);
            core31 = IsAtLeast(majorVersion, minorVersion, 3, 1);
            core32 = IsAtLeast(majorVersion, minorVersion, 3, 2);
            core33 = IsAtLeast(majorVersion, minorVersion, 3, 3);
            core40 = IsAtLeast(majorVersion, minorVersion, 4, 0);
            core41 = IsAtLeast(majorVersion, minorVersion, 4, 1);
            core42 = IsAtLeast(majorVersion, minorVersion, 4, 2);
            core43 = IsAtLeast(majorVersion, minorVersion, 4, 3);
            core44 = IsAtLeast(majorVersion, minorVersion, 4, 4);
            core45 = IsAtLeast(majorVersion, minorVersion, 4, 5);
            core46 = IsAtLeast(majorVersion, minorVersion, 4, 6);
        }

        private static bool IsAtLeast(
                            int actualMajor,
                            int actualMinor,
                            int requiredMajor,
                            int requiredMinor
        )
        {
            return actualMajor > requiredMajor ||
                   (actualMajor == requiredMajor && actualMinor >= requiredMinor);
        }

        private static bool TryReadVersion(
                            ReadOnlySpan<char> text,
                            out int major,
                            out int minor
        )
        {
            major = 0;
            minor = 0;

            int majorStart = 0;
            while (majorStart < text.Length && !char.IsAsciiDigit(text[majorStart])) {
                majorStart++;
            }
            if (majorStart == text.Length) {
                return false;
            }

            int dot = text[majorStart..].IndexOf('.');
            if (dot < 1) {
                return false;
            }
            dot += majorStart;

            int minorEnd = dot + 1;
            while (minorEnd < text.Length && char.IsAsciiDigit(text[minorEnd])) {
                minorEnd++;
            }

            return minorEnd > dot + 1 &&
                   int.TryParse(text[majorStart..dot], NumberStyles.None,
                                CultureInfo.InvariantCulture,
                                out major) &&
                   int.TryParse(text[(dot + 1)..minorEnd], NumberStyles.None,
                                CultureInfo.InvariantCulture, out minor);
        }

    }
}
