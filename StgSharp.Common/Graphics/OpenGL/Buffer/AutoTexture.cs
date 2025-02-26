//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="AutoTexture.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------

using StgSharp.Commom.Collections;
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
    /// A collection of a sets of OpenGL textures. The collection can automatically select one of
    /// the  texture with minimum costs to upgrade pixels to Buffer. Usually <see
    /// cref="AutoTextureGL" /> is used for <see cref="IImageProvider" /> loading, but not for
    /// target of  <see cref="OpenGLFunction.FrameBufferTexture2d(FrameBufferTarget, uint,
    /// Texture2DTarget, GlHandle, int)" />.
    /// </summary>
    public sealed class AutoTextureGL
    {

        private TextureProperty[] propertyCache;
        private BidirectionalDictionary<Image, int> imageIndexMap = new BidirectionalDictionary<Image, int>(
            );
        private Dictionary<Image, int> imageUpdateIndex = new Dictionary<Image, int>(
            );

        private int _size, _currentIndex;
        private LinkedList<int> usedTextureIndex;
        private Queue<int> unusedTextureUnitIndex;
        private TextureGL texturePackage;

        private TextureProperty _textureProperty;

        private AutoTextureGL( TextureGL texture )
        {
            texturePackage = texture;
            _size = texture.Count;
            imageIndexMap = new BidirectionalDictionary<Image, int>();
            unusedTextureUnitIndex = new Queue<int>(
                Enumerable.Range( 0, _size ) );
            usedTextureIndex = new LinkedList<int>();
            propertyCache = new TextureProperty[_size];
        }

        public AutoTextureGL( int textureObjectSize, [NotNull]glRender binding )
        {
            this._size = textureObjectSize;
            imageIndexMap = new BidirectionalDictionary<Image, int>();
            texturePackage = new TextureGL( textureObjectSize, binding );
            unusedTextureUnitIndex = new Queue<int>(
                Enumerable.Range( 0, _size ) );
            usedTextureIndex = new LinkedList<int>();
            propertyCache = new TextureProperty[_size];
        }

        public TextureProperty DefaultTextureProperty
        {
            get => _textureProperty;
            set => _textureProperty = value;
        }

        private OpenGLFunction GL
        {
            get => texturePackage.GL;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void SelectTextureAndBind( Image i )
        {
            TextureProperty property = _textureProperty;

            SelectTextureAndBind( i, property );
        }

        public void SelectTextureAndBind(
            [NotNull]Image i,
            TextureProperty property )
        {
            //usable texture with image match
            if( imageIndexMap.TryGetValue( i, out int index ) ) {
                //prop setting match
                if( propertyCache[ index ] == property ) {
                    _currentIndex = index;                          //GL.Assert(true);
                    texturePackage.Bind2D( index );                   //GL.Assert(true);
                    if( i.PixelUpdateCount != imageUpdateIndex[ i ] ) {
                        texturePackage.LoadTexture( index, i );       //GL.Assert(true);
                    }
                } else {
                    _currentIndex = index;                          //GL.Assert(true);
                    texturePackage.Bind2D( index );                   //GL.Assert(true);
                    texturePackage.Set2dProperty( index, property );  //GL.Assert(true);
                    if( i.PixelUpdateCount != imageUpdateIndex[ i ] ) {
                        texturePackage.LoadTexture( index, i );       //GL.Assert(true);
                    }
                }
            } else {
                if( unusedTextureUnitIndex.TryDequeue( out int newIndex ) ) {
                    //has extra unused texture object
                    imageIndexMap.Add( i, index );
                    texturePackage.Bind2D( index );
                    texturePackage.Set2dProperty( index, property );
                    texturePackage.LoadTexture( index, i );
                    usedTextureIndex.AddLast( newIndex );
                    propertyCache[ newIndex ] = property;
                    imageUpdateIndex[ i ] = i.PixelUpdateCount;
                } else {
                    //recycle from current
                    int reusedIndex = usedTextureIndex.First!.Value;
                    usedTextureIndex.RemoveFirst();
                    usedTextureIndex.AddLast( reusedIndex );
                    imageIndexMap[ i ] = reusedIndex;
                    propertyCache[ newIndex ] = property;
                    imageUpdateIndex[ i ] = i.PixelUpdateCount;
                }
            }
        }

        public static AutoTextureGL ToAuto( [NotNull]TextureGL texture )
        {
            if( texture.IndexOfCurrentTexture != -1 ) {
                throw new InvalidOperationException(
                    "Texture to be converted has been editted." );
            }
            return new AutoTextureGL( texture );
        }

    }
}
