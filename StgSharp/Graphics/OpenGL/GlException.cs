using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public sealed class GlExecutionException : Exception
    {
        public GlExecutionException()
        {
        }

        public GlExecutionException(uint errorCode) : base($"Critical OpenGL error: {errorCode}")
        {
        }

    }

    public sealed class GlArrayFormatException : Exception
    {
        public GlArrayFormatException(PixelChannelLayout layout, string arrayname):
            base($"{arrayname} cannot be used as layout of {layout}")
        {
        }

        internal GlArrayFormatException(string message) : base(message)
        {
        }

        internal GlArrayFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal GlArrayFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public static partial class GlHelper
    {
        public static bool CheckArrayFormat(this Array array, PixelChannelLayout layout)
        {
            return layout switch
            {
                PixelChannelLayout.Byte or PixelChannelLayout.UByte332 or PixelChannelLayout.UByte233Rev
                    => array is byte[] || array is sbyte[],
                PixelChannelLayout.Short => array is short[],
                PixelChannelLayout.UShort => array is ushort[],
                PixelChannelLayout.Int => array is int[],
                PixelChannelLayout.Float => array is float[],
                PixelChannelLayout.UShort4444 => array is ushort[] || array is byte[],
                PixelChannelLayout.UByte => array is byte[] || array is sbyte[],
                PixelChannelLayout.UInt => array is uint[],
                PixelChannelLayout.UShort565 or
                PixelChannelLayout.UShort565Rev or
                PixelChannelLayout.UShort4444Rev or
                PixelChannelLayout.UShort5551 or
                PixelChannelLayout.UShort1555Rev => array is ushort[],
                PixelChannelLayout.UInt8888Rev or
                PixelChannelLayout.UInt1010102 or
                PixelChannelLayout.UInt2101010Rev => array is int[],
                PixelChannelLayout.UInt8888 => array is uint[] or byte[],
                _ => false,
            };
        }

    }
}
