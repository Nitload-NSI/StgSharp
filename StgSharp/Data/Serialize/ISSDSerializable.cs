using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Data
{

    public abstract class SSDSerializable
    {
        public abstract SerializableTypeCode SSDTypeCode { get; }

        public abstract byte[] GetBytes();

        public abstract byte[] GetBytes(out int length);

        internal abstract void BuildFromByteStream(byte[] stream);
    }

    public enum SerializableTypeCode
    {
        Unknown = 0,
        IndexTable = 1,
        PixelImage = 2,
        VectorImage = 3,
        Audio = 4,
        Shader = 5,
        Text = 6,
        RawData = 7,
        VirtualFolder = 8,

    }

    public static partial class Serializer
    {
        public static byte[] Serialize(SSDSerializable serializableObject)
        {
            return serializableObject.GetBytes();
        }

        public static T Deserialize<T>(byte[] stream)where T : SSDSerializable
        {
            T serializableObject = (T)Activator.CreateInstance<T>();
            serializableObject.BuildFromByteStream(stream);
            return serializableObject;
        }

        public static string GetNameTail(SerializableTypeCode typeCode)
        {
            switch (typeCode)
            {
                case SerializableTypeCode.IndexTable:
                    return "_idt";
                case SerializableTypeCode.PixelImage:
                    return "_pim";
                case SerializableTypeCode.VectorImage:
                    return "_vim";
                case SerializableTypeCode.Audio:
                    return "_wav";
                case SerializableTypeCode.Shader:
                    return "_gls";
                case SerializableTypeCode.Text:
                    return "_txt";
                case SerializableTypeCode.RawData:
                    return "_raw";
                case SerializableTypeCode.VirtualFolder:
                    return "_fod";
                default:
                    throw new ArgumentException("Unknown file type");
            }
        }
    }
}