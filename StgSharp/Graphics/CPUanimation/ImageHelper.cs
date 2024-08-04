using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Graphics
{
    public static class ImageHelper
    {
        public static void Resize(Image i, int width, int height, ImageMovement move)
        {
            if (i == null)
            {
                throw new ArgumentNullException(nameof(i));
            }
            Array.Resize(ref i.data, width * height * i.PixelSize);
            i.Height = height;
            i.Width = width;
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
