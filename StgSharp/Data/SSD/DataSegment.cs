//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="DataSegment.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Data
{
    [StructLayout(LayoutKind.Explicit, Size = StgSharp.ssdSegmentLength * 16)]
    public unsafe struct SSDSegment : IComparable<int>, IComparable<SSDSegment>
    {

        [FieldOffset(16)] public fixed byte Data[16 * (StgSharp.ssdSegmentLength - 1)];
        [FieldOffset(12)] public int DataHash;
        [FieldOffset(8)] public int PreviousHash;
        [FieldOffset(0 * 16)] public SSDSegmentHead Head;

        public SSDSegment(int previousHash)
        {
            this.PreviousHash = previousHash;
        }

        public static SSDSegment FromStream(byte[] stream)
        {
            if (stream.Length != 16 * StgSharp.ssdSegmentLength)
            {
                throw new ArgumentException();
            }
            SSDSegment ret = new SSDSegment();
            fixed (byte* bptr = stream)
            {
                ret.Head.data = *(M128*)bptr;
                for (int i = 1; i < StgSharp.ssdSegmentLength; i++)
                {
                    ret.WriteData<M128>(i - 1, *(M128*)(bptr + 16 * i));
                }
            }
            return ret;
        }

        public byte this[int index]
        {
            get
            {
                if (index > (16 * (StgSharp.ssdSegmentLength - 1)) - 1)
                {
                    throw new IndexOutOfRangeException();
                }
                return Data[index];
            }

            set
            {
                if (index > (16 * (StgSharp.ssdSegmentLength - 1)) - 1)
                {
                    throw new IndexOutOfRangeException();
                }
                Data[index] = value;
            }
        }

        public int ID
        {
            get { return Head.globalID; }
            set { Head.globalID = value; }
        }

        public short Size => Head.size;

        public void FillRandomBytes(int begin)
        {
            if (begin > 255)
            {
                return;
            }
            for (int i = begin; i < 255; i++)
            {
                Data[i] = Scaler.Random<byte>(); //TODO: 使用STGSHARP内部随机数工具处理它
            }
        }

        public byte[] GetBytes()
        {
            byte[] ret = new byte[16 * StgSharp.ssdSegmentLength];
            fixed (M128* mptr = &Head.data)
            {
                fixed (byte* aptr = ret)
                {
                    Vector4* vsptr = (Vector4*)mptr;
                    Vector4* vtptr = (Vector4*)aptr;
                    for (int i = 0; i < 18; i++)
                    {
                        *(vtptr + i) = *(vsptr + i);
                    }
                }
            }

            return ret;
        }

        public int GetDataHash()
        {
            Vector4 v = Vector4.One;
            fixed (M128* mptr = &Head.data)
            {
                Vector4* vptr = (Vector4*)mptr;
                for (int i = 0; i < StgSharp.ssdSegmentLength - 1; i += 1)
                {
                    v += (*vptr) * 31;
                }
            }
            M128 m = new M128(v);
            DataHash = m.vec.GetHashCode();
            return DataHash;
        }

        public override int GetHashCode()
        {
            int ret = 17;
            fixed (M128* mptr = &Head.data)
            {
                Vector4* vptr = (Vector4*)mptr;
                for (int i = 0; i < StgSharp.ssdSegmentLength; i++)
                {
                    ret = (ret + (*vptr).GetHashCode()) * 31;
                }
            }
            return ret;
        }

        public unsafe T ReadData<T>(int index) where T : struct
        {
            fixed (byte* bptr = Data)
            {
                T* tptr = (T*)bptr;
                return *(tptr + index);
            }
        }

        public unsafe void WriteData<T>(int index, T value) where T : struct
        {
            fixed (byte* bptr = Data)
            {
                T* tptr = (T*)bptr;
                *(tptr + index) = value;
            }
        }

        int IComparable<int>.CompareTo(int other)
        {
            return ID.CompareTo(other);
        }

        int IComparable<SSDSegment>.CompareTo(SSDSegment other)
        {
            return ID.CompareTo(other.ID);
        }

    }

}
