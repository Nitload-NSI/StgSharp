// -----------------------------------------------------------------------------
// file="Image"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Nitload.Graphics
{
    /// <summary>
    ///   Owns a tightly packed, two-dimensional managed pixel buffer.
    /// </summary>
    /// <typeparam name="TPixel">
    ///   Unmanaged storage layout of one pixel.
    /// </typeparam>
    public sealed class Image<TPixel> where TPixel : unmanaged
    {

        private readonly TPixel[] _pixels;

        public Image(
               int width,
               int height
        ) : this(width, height, new TPixel[GetRequiredPixelCount(width, height)]) { }

        public Image(
               (int width, int height) size
        ) : this(size.width, size.height) { }

        /// <summary>
        ///   Wraps <paramref name="pixels" /> without copying it.
        /// </summary>
        public Image(
               int width,
               int height,
               TPixel[] pixels
        )
        {
            ArgumentNullException.ThrowIfNull(pixels);

            int requiredLength = GetRequiredPixelCount(width, height);
            if (pixels.Length != requiredLength)
            {
                throw new ArgumentException(
                    $"Pixel buffer length must be exactly {requiredLength}, but was {pixels.Length}.",
                    nameof(pixels));
            }

            Width = width;
            Height = height;
            _pixels = pixels;
        }

        public Image(
               (int width, int height) size,
               TPixel[] pixels
        ) : this(size.width, size.height, pixels) { }

        /// <summary>
        ///   Gets a writable byte view over the pixel buffer without copying it.
        /// </summary>
        public Span<byte> Bytes => MemoryMarshal.AsBytes(_pixels.AsSpan());

        public int ByteLength => Bytes.Length;

        public int Height { get; }

        public int PixelCount => _pixels.Length;

        /// <summary>
        ///   Gets a writable typed view over the pixel buffer without copying it.
        /// </summary>
        public Span<TPixel> Pixels => _pixels;

        public ReadOnlySpan<byte> ReadOnlyBytes => MemoryMarshal.AsBytes(_pixels.AsSpan());

        public ReadOnlySpan<TPixel> ReadOnlyPixels => _pixels;

        public (int width, int height) Size => (Width, Height);

        public int Width { get; }

        public static explicit operator Span<byte>(
                                        Image<TPixel> image
        )
        {
            ArgumentNullException.ThrowIfNull(image);
            return image.Bytes;
        }

        private static int GetRequiredPixelCount(
                           int width,
                           int height
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(width);
            ArgumentOutOfRangeException.ThrowIfNegative(height);

            return checked(width * height);
        }

    }
}
