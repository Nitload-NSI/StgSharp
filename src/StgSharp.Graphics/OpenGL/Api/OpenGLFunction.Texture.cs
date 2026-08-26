// -----------------------------------------------------------------------------
// file="OpenGLFunction.Texture"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ActiveTextureUnit(
                    uint type
        )
        {
            _context->glActiveTexture(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenTextures(
                                 int count
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            GlHandle[] h = new GlHandle[count];
            if (h.Length == 0) {
                return h;
            }

            fixed (GlHandle* hptr = h) {
                _context->glGenTextures(count, (uint*)hptr);
            }
            return h;
        }

        /// <summary>Delete texture object handles.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteTextures(
                    ReadOnlySpan<GlHandle> textures
        )
        {
            fixed (GlHandle* texturesPtr = textures) {
                Context.glDeleteTextures(textures.Length, (uint*)texturesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void GetTextureImage<T>(
                           Texture2DTarget target,
                           int level,
                           FrameBufferChannel channel,
                           PixelChannelLayout channelLayout,
                           Span<T> destination
        ) where T : unmanaged
        {
            fixed (T* tptr = destination) {
                _context->glGetTexImage((uint)target, level, (uint)channel, (uint)channelLayout,
                                        tptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureLevelProperty(
                    uint textureTypeMask,
                    int level,
                    uint propertyMask,
                    out int propertyValue
        )
        {
            fixed (int* iptr = &propertyValue) {
                _context->glGetTexLevelParameteriv(textureTypeMask, level, propertyMask, iptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadPixels<T>(
                    (int X, int Y) beginPosition,
                    (int width, int height) size,
                    FrameBufferChannel format,
                    PixelChannelLayout dataType,
                    Span<T> destination
        ) where T : unmanaged
        {
            fixed (T* destinationPtr = destination) {
                _context->glReadPixels(beginPosition.X, beginPosition.Y, size.width, size.height,
                                       (uint)format, (uint)dataType, destinationPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureImage2d(
                    Texture2DTarget target,
                    int level,
                    int internalFormat,
                    int width,
                    int height,
                    FrameBufferChannel format,
                    PixelChannelLayout type
        )
        {
            _context->glTexImage2D((uint)target, level, internalFormat, width, height, 0,
                                   (uint)format, (uint)type, (void*)0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureImage2d<T>(
                    Texture2DTarget target,
                    int level,
                    int internalFormat,
                    int width,
                    int height,
                    FrameBufferChannel format,
                    PixelChannelLayout type,
                    ReadOnlySpan<T> pixels
        ) where T : unmanaged
        {
            fixed (T* pixelPtr = pixels) {
                _context->glTexImage2D((uint)target, level, internalFormat, width, height, 0,
                                       (uint)format, (uint)type, pixelPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameter(
                    int target,
                    uint pname,
                    int param
        )
        {
            _context->glTexParameteri((uint)target, pname, param);
        }

    }
}
