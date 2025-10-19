//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ImageHelper"
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StgSharp.Graphics
{
    public static class ImageHelper
    {

        public static void Resize(Image i, int width, int height, ImageMovement move)
        {
            if (i == null) {
                throw new ArgumentNullException(nameof(i));
            }
            if (i.Size == (width, height)) {
                return;
            }
            if (!Enum.IsDefined(typeof(ImageMovement), move)) {
                throw new InvalidEnumArgumentException(nameof(move));
            }
            byte[] oldData = i.Data;
            i.Data = new byte[width * height * i.PixelSize];
            i.Height = height;
            i.Width = width;
            i.PixelUpdateCount++;
            switch (move)
            {
                case ImageMovement.Discard:
                    break;
                case ImageMovement.Top:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }

    [Flags]
    public enum ImageMovement
    {
#pragma warning disable CA1008 
        Discard = 0,
        #pragma warning restore CA1008 
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        TopLeft = Top | Left,
        BottomLeft = Bottom | Left,
        TopRight = Top | Right,
        BottomRight = Bottom | Right,

    }
}
