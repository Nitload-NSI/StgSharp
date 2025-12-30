//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFucntionSourceManager"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
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

        public int TextureUnitCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => textureUnitCountGetter;
        }

        private ConcurrentQueue<TextureUnit> UnusedTextureImageUint
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = [];

        internal void ReturnAvailableUnit(IEnumerable<TextureUnit> source)
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

        internal int TryGetAvailableTextureUnit(int count, out IEnumerable<TextureUnit> result)
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