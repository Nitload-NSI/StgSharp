// -----------------------------------------------------------------------------
// file="OpenGLFunction.Context"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        private static readonly Dictionary<IntPtr, OpenGLFunction> contextToGLMap = [];
        private static readonly ThreadLocal<OpenGLFunction> _currentGL = new();
        private OpenglContext* _context;

        private readonly Lazy<int> _attachmentColorRange;

        private OpenGLFunction()
        {
            textureUnitCountGetter = glConst.MAX_TEXTURE_IMAGE_UNITS;
            UnusedTextureImageUint = [];
            _attachmentColorRange = new Lazy<int>(
                () => GetMaskInteger(glConst.MAX_COLOR_ATTACHMENTS));
        }

        public int AttachmentColorRange => _attachmentColorRange.Value;

        public static OpenGLFunction CurrentGL
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _currentGL.Value!;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set => _currentGL.Value = value;
        }

        internal IntPtr ContextHandle => (IntPtr)_context;

        protected ref OpenglContext Context
        {
            get
            {
                #if DEBUG
                ArgumentNullException.ThrowIfNull(_context, nameof(_context));
                #endif
                return ref (*_context);
            }
        }

        internal static OpenGLFunction BuildGlFunctionPackage(
                                       IntPtr handle
        )
        {
            if (contextToGLMap.TryGetValue(handle, out OpenGLFunction pack)) {
                return pack;
            }
            pack = new OpenGLFunction
            {
                _context = (OpenglContext*)handle
            };
            contextToGLMap.Add(handle, pack);
            return pack;
        }

        ~OpenGLFunction()
        {
            _ = contextToGLMap.Remove(ContextHandle);
        }

    }
}
