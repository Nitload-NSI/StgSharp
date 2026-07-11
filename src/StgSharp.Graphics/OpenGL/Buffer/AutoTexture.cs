// -----------------------------------------------------------------------------
// file="AutoTexture"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;
using StgSharp.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    /// <summary>
    ///   A collection of a sets of OpenGL textures. The collection can automatically select one of
    ///   the  texture with minimum costs to upgrade pixels to BufferHandle. Usually <see ///  
    ///   cref="AutoTextureGL" /> is used for <see cref="IImageProvider" /> loading, but not for
    ///   target of  <see cref="OpenGLFunction.FrameBufferTexture2d(FrameBufferTarget, uint, ///  
    ///   Texture2DTarget, GlHandle, int)" />.
    /// </summary>
    public sealed class AutoTextureGL
    {

        private TextureProperty[] propertyCache;
        private BidirectionalDictionary<Image, int> imageIndexMap = new BidirectionalDictionary<Image, int>(
            );
        private Dictionary<Image, int> imageUpdateIndex = new Dictionary<Image, int>();

        private int _size, _currentIndex;
        private LinkedList<int> usedTextureIndex;
        private Queue<int> unusedTextureUnitIndex;
        private TextureGL texturePackage;

        private TextureProperty _textureProperty;

        private AutoTextureGL(
                TextureGL texture
        )
        {
            texturePackage = texture;
            _size = texture.Count;
            imageIndexMap = new BidirectionalDictionary<Image, int>();
            unusedTextureUnitIndex = new Queue<int>(Enumerable.Range(0, _size));
            usedTextureIndex = new LinkedList<int>();
            propertyCache = new TextureProperty[_size];
        }

        public AutoTextureGL(
               int textureObjectSize,
               [NotNull]glRender binding
        )
        {
            this._size = textureObjectSize;
            imageIndexMap = new BidirectionalDictionary<Image, int>();
            texturePackage = new TextureGL(textureObjectSize, binding);
            unusedTextureUnitIndex = new Queue<int>(Enumerable.Range(0, _size));
            usedTextureIndex = new LinkedList<int>();
            propertyCache = new TextureProperty[_size];
        }

        public TextureProperty DefaultTextureProperty
        {
            get => _textureProperty;
            set => _textureProperty = value;
        }

        private OpenGLFunction GL => texturePackage.GL;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SelectTextureAndBind(
                    Image i
        )
        {
            TextureProperty property = _textureProperty;

            SelectTextureAndBind(i, property);
        }

        public void SelectTextureAndBind(
                    [NotNull]Image i,
                    TextureProperty property
        )
        {
            // usable texture with image match
            if (imageIndexMap.TryGetValue(i, out int index))
            {
                // prop setting match
                if (propertyCache[index] == property)
                {
                    _currentIndex = index;                          //GL.Assert(true);
                    texturePackage.Bind2D(index);                   //GL.Assert(true);
                    if (i.PixelUpdateCount != imageUpdateIndex[i])
                    {
                        texturePackage.LoadTexture(index, i);       //GL.Assert(true);
                    }
                } else
                {
                    _currentIndex = index;                          //GL.Assert(true);
                    texturePackage.Bind2D(index);                   //GL.Assert(true);
                    texturePackage.Set2dProperty(index, property);  //GL.Assert(true);
                    if (i.PixelUpdateCount != imageUpdateIndex[i])
                    {
                        texturePackage.LoadTexture(index, i);       //GL.Assert(true);
                    }
                }
            } else
            {
                if (unusedTextureUnitIndex.TryDequeue(out int newIndex))
                {
                    // has extra unused texture object
                    imageIndexMap.Add(i, index);
                    texturePackage.Bind2D(index);
                    texturePackage.Set2dProperty(index, property);
                    texturePackage.LoadTexture(index, i);
                    usedTextureIndex.AddLast(newIndex);
                    propertyCache[newIndex] = property;
                    imageUpdateIndex[i] = i.PixelUpdateCount;
                } else
                {
                    // recycle from current
                    int reusedIndex = usedTextureIndex.First!.Value;
                    usedTextureIndex.RemoveFirst();
                    usedTextureIndex.AddLast(reusedIndex);
                    imageIndexMap[i] = reusedIndex;
                    propertyCache[newIndex] = property;
                    imageUpdateIndex[i] = i.PixelUpdateCount;
                }
            }
        }

        public static AutoTextureGL ToAuto(
                                    [NotNull]TextureGL texture
        )
        {
            if (texture.IndexOfCurrentTexture != -1) {
                throw new InvalidOperationException("Texture to be converted has been editted.");
            }
            return new AutoTextureGL(texture);
        }

    }
}
