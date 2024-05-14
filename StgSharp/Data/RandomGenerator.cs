using StgSharp.Data;

using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace StgSharp.Data
{
    public class RandomGenerator
    {


        private readonly byte[] originKey;
        private byte[] currentKey;
        private int cycleCount;
        private SHA512 hash;

        public RandomGenerator():this(DateTime.UtcNow.ToBinary())
        {
            
        }

        public RandomGenerator(object source)
        {
            try
            {
                if (source == null)
                {
                    throw new ArgumentNullException(nameof(source));
                }
                XmlSerializer s = new XmlSerializer(source.GetType());
                using (StringWriter sw = new StringWriter())
                {
                    s.Serialize(sw, source);
                    string data = sw.ToString();
                    originKey = Encoding.UTF8.GetBytes(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
            hash = SHA512.Create();
            originKey = hash.ComputeHash(originKey);
            currentKey = originKey;
            cycleCount = 0;
        }

        public int TotalGeneratedRandom
        {
            get => cycleCount;
        }

        public unsafe int GenRandomInt()
        {
            M128 source = PrivateRand();
            cycleCount++;

            /*
            float d0 =  MathF.Sin(1 / source.Read<float>(0));
            float d1 =  MathF.Sin(1 / source.Read<float>(1));
            float d2 =  MathF.Sin(1 / source.Read<float>(2));
            float d3 =  MathF.Sin(1 / source.Read<float>(3));

            source.Write<short>(0, *( short*)&d3);
            source.Write<short>(2, *((short*)&d2+1));
            source.Write<short>(4, *( short*)&d1);
            source.Write<short>(6, *((short*)&d0+1));

            /**/

            return source.Read<int>(cycleCount % 4);
        }

        public float GenRandomSingle()
        {
            cycleCount++;
            
            while (true)
            {
                M128 source = PrivateRand();
                
                uint
                    temp0 = source.Read<uint>(0),
                    temp1 = source.Read<uint>(2);

                if (temp0 * temp1 != 0)
                {
                    float ret = (temp0 % temp1) * 1.0f / temp1;
                    if (ret < 0.335f)
                    {
                        ret = ret * (ret * -0.7811934f + 1.4489113f);
                    }
                    else 
                    {
                        ret = ret * (ret * -0.1158368f + 1.0580600f) + 0.0590584f;
                    }
                    if (ret > 1)
                    {
                        continue;
                    }
                    return ret;
                }

            }

        }

        public byte GenRandomInt8()
        {
            cycleCount++;
            if (cycleCount % 64 == 0)
            {
                PrivateRand();
            }
            return (byte)(currentKey[ cycleCount % 64 ]);
        }

        private unsafe M128 PrivateRand()
        {
            currentKey = hash.ComputeHash(currentKey);
            M128 ret;
            fixed (byte* bptr = currentKey)
            {
                switch ((cycleCount % 16) / 4) 
                {
                    case 0:
                        ret= *((M128*)bptr + 0);break;
                    case 1:
                        ret= *((M128*)bptr + 1);break;
                    case 2:
                        ret= *((M128*)bptr + 2);break;
                    case 3:
                        ret= *((M128*)bptr + 3);break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            //Console.WriteLine(ret.ToString(16));
            return ret;
        }


        public void Reset()
        {
            currentKey = originKey;
            cycleCount = 0;
        }

    }
}
