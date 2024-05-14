//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="SSDraw.cs"
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
using StgSharp.Graphics.ShaderEdit;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace StgSharp.Data.FileIO
{
    public class SSDraw
    {

        public const string FileType = "ssdraw";
        public const int SegmentCapacity = 16 * (StgSharp.ssdSegmentLength - 1);

        private Area home;
        private int previousHash;
        private List<SSDSegment> segments;

        private SSDrawHead _head;
        private SSDSegment currentSegment;
        private string _name;

        internal SSDraw()
        {
            segments = new List<SSDSegment>();
        }

        public SSDraw(string name)
        {
            _name = name;
            segments = new List<SSDSegment>();
        }

        public int ID
        {
            get => _head._head.globalID;
            set => _head._head.globalID = value;
        }

        internal SSDrawHead Head => _head;

        public SSDSegment[] GetSegments()
        {
            SSDSegment[] ret = segments.ToArray();
            Array.Resize(ref ret, ret.Length + 1);
            ret[ret.Length - 1] = Head.Data;
            return ret;
        }

        public void CreateRawFile(string outputFolder, bool rewrite)
        {
            string route = (outputFolder == string.Empty) ? ($"{_name}.{FileType}") :
                ($"{outputFolder} \\ {_name}.{FileType}");
            if (File.Exists(route) && !rewrite)
            {
                throw new Exception("This SSDRAW has already existed.");
            }
            using (File.Create(route)) { }
            using (File.Open(route, FileMode.Truncate)) { }
            FileStream f = File.Open(route, FileMode.Open);
            foreach (SSDSegment _head in segments)
            {
                f.Write(_head.GetBytes(), 0, 18 * 16);
            }
            f.Close();
        }

        
        public static SSDraw readRaw(string route)
        {
            string[] routeFolders = route.Split('\\');
            if (!routeFolders.Last().Contains($".{FileType}"))
            {
                throw new Exception($"File to open is not a {FileType} file.");
            }
            byte[] dataStream= new byte[16 * StgSharp.ssdSegmentLength];
            SSDraw ret = new SSDraw();
            using (FileStream stream = File.Open(route, FileMode.Open))
            {
                int offset = 0;
                offset += stream.Read(dataStream,offset,16);
                ret._head = new SSDrawHead(dataStream);
                while (offset < stream.Length)
                {
                    offset += stream.Read(dataStream, offset, 16);
                    ret.segments.Add(SSDSegment.FromStream(dataStream));
                }
            }
            return ret;
        }

        public void WriteRaw(byte[] dataStream)
        {
            previousHash = BuildRawHead(0);
            currentSegment = new SSDSegment(previousHash);
            int t = 0;
            for (int i = 0; i < dataStream.Length; i++, t++)
            {
                if (t == SegmentCapacity)
                {
                    currentSegment.GetDataHash();
                    currentSegment.Head.Size = 256;
                    currentSegment.Head.globalID = i;
                    segments.Add(currentSegment);
                    previousHash = currentSegment.GetHashCode();
                    currentSegment = new SSDSegment(previousHash);
                    t = 0;
                }
                currentSegment[i] = dataStream[i];
            }
            currentSegment.FillRandomBytes(t);
            currentSegment.GetDataHash();
            currentSegment.Head.Size = t;
            segments.Add(currentSegment);

        }

        private int BuildRawHead(int beginID)
        {
            SSDrawHead rawHead = new SSDrawHead(_name);
            _head = rawHead;
            return rawHead.GetHashCode();
        }

    }


    public unsafe class SSDrawHead : IEnumerable<(int, int)>
    {

        private SSDSegment segmentData;
        private string name;

        internal SSDrawHead(byte[] data)
        {
            if (data.Length != 16* StgSharp.ssdSegmentLength)
            {
                throw new ArgumentException();
            }
            segmentData = new SSDSegment();
            fixed (byte* bptr = data)
            {
                segmentData.Head.data = *(M128*)bptr;
                for (int i = 1; i < StgSharp.ssdSegmentLength; i++)
                {
                    segmentData.WriteData<M128>(i - 1,*(M128*)(bptr + 16 * i));
                }
            }
        }

        internal SSDrawHead(string name)
        {
            int hash;
            this.name = name;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(name));

                hash = BitConverter.ToInt32(bytes, 0);
            }
            _head.previousHash = hash;
        }

        public SSDrawHead()
        {
        }

        internal SSDSegment Data
        {
            get => segmentData;
        }

        public (int begin, int end) this[byte index]
        {
            get => segmentData.ReadData<(int, int)>(index);
            set => segmentData.WriteData<(int, int)>(index, value);
        }

        public unsafe string Name
        {
            get
            {
                byte[] bytes = new byte[15 * 16];
                fixed (byte* bptr = segmentData.Data)
                {
                    Marshal.Copy(
                        source: (IntPtr)bptr,
                        destination: bytes,
                        256, 15 * 16);
                }
                return Encoding.UTF8.GetString(bytes);
            }
            set
            {
                byte[] namecode = Encoding.UTF8.GetBytes(value);
                fixed (byte* bptr = segmentData.Data)
                {
                    Marshal.Copy(
                        source: namecode,
                        startIndex: 0,
                        destination: (IntPtr)(bptr + 256),
                        length: 15 * 16);
                }
            }
        }

        public byte TypeCode
        {
            get => _head.selfDefine_1;
            internal set => _head.selfDefine_1 = value;
        }

        internal ref SSDSegmentHead _head
        {
            get => ref segmentData.Head;
        }

        internal byte pairCount
        {
            get => (byte)(_head.size / 8);
            set
            {
                if (value > 32)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _head.size = (byte)(value * 8);
            }
        }

        public unsafe byte[] GetBytes()
        {
            return segmentData.GetBytes();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < pairCount; i++)
            {
                yield return segmentData.ReadData<(int, int)>(i);
            }
        }

        IEnumerator<(int, int)> IEnumerable<(int, int)>.GetEnumerator()
        {
            for (int i = 0; i < pairCount; i++)
            {
                yield return segmentData.ReadData<(int, int)>(i);
            }
        }



    }


}
