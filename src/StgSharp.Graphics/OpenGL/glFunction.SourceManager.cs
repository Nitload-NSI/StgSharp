// -----------------------------------------------------------------------------
// file="glFunction.SourceManager"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public partial class OpenGLFunction
    {

        private readonly int textureUnitCountGetter;

        public int TextureUnitCount => textureUnitCountGetter;

        private ConcurrentQueue<TextureUnit> UnusedTextureImageUint
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = [];

        internal void ReturnAvailableUnit(
                      IEnumerable<TextureUnit> source
        )
        {
            int v = glConst.TEXTURE0 + TextureUnitCount;
            foreach (TextureUnit unit in source)
            {
                if ((int)unit > v) {
                    throw new InvalidOperationException(
                        "Source contains texture unit beyond context support.");
                }
            }
        }

        internal int TryGetAvailableTextureUnit(
                     int count,
                     out IEnumerable<TextureUnit> result
        )
        {
            result = new Queue<TextureUnit>();
            for (int i = 0; i < count; i++)
            {
                if (UnusedTextureImageUint.TryDequeue(out TextureUnit unit))
                {
                    (result as Queue<TextureUnit>)!.Enqueue(unit);
                } else
                {
                    return i;
                }
            }
            return count;
        }

        private ConcurrentQueue<TextureUnit> InitializeUnusedTextureImageUint()
        {
            ConcurrentQueue<TextureUnit> result = new ConcurrentQueue<TextureUnit>();
            for (int index = 0; index < TextureUnitCount; index++) {
                result.Enqueue((TextureUnit)(glConst.TEXTURE0 + index));
            }
            return result;
        }

    }
}