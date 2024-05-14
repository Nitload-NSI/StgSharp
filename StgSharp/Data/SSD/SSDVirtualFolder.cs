//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="SSDVirtualFolder.cs"
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
using StgSharp.Data.FileIO;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Data
{
    public class SSDFolder : SSDSerializable, IEnumerable<SSDSegmentHead>
    {
        private Area local;
        private SSDrawHead _head;
        internal int _folderID;
        internal string _name;
        internal Dictionary<int, SSDraw> files;
        internal Dictionary<int, SSDFolder> folders;
        internal Dictionary<string, int> idIndex;

        internal SSDFolder()
        {
            folders = new Dictionary<int, SSDFolder>();
            files = new Dictionary<int, SSDraw>();
        }

        internal SSDFolder(string name)
        {
            if (name == "root")
            {
                throw new ArgumentNullException(
                    "Name cannot be root," +
                    "it is preserved for root file indexer.");
            }
            if (name == "default")
            {
                throw new ArgumentNullException(
                    "Name cannot be default," +
                    "it is preserved for default file id provider.");
            }
            _name = name;
            folders = new Dictionary<int, SSDFolder>();
            files = new Dictionary<int, SSDraw>();
        }

        public override SerializableTypeCode SSDTypeCode => SerializableTypeCode.VirtualFolder;

        /// <summary>
        /// Check out if this virtual folder contains certain file.
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="type">Type code defined as <see cref="SerializableTypeCode"/></param>
        /// <returns>Id of the file in its virtual disc</returns>
        public int FindFileId(string name, SerializableTypeCode type)
        {
            int fileid = 0;
            if (idIndex.TryGetValue($"{name}{Serializer.GetNameTail(type)}"
                , out fileid) && !(type == SerializableTypeCode.VirtualFolder))
            {
                return fileid;
            }
            return 0;
        }



        /// <summary>
        /// Check out if this virtual folder contains certain file.
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <returns>Id of the folder in its virtual disc</returns>
        public int FindFolderId(string name)
        {
            int folderid = 0;
            if (idIndex.TryGetValue($"{name}_f", out folderid))
            {
                return folderid;
            }
            return 0;
        }

        public SSDraw CreateFile(string name, SerializableTypeCode type)
        {
            SSDraw ret = new SSDraw(name + Serializer.GetNameTail(type));
            ret.ID = local.NewID();
            return ret;
        }

        //TODO 虚拟文件夹的GetBytes好难写
        public override byte[] GetBytes()
        {
            List<byte> rawBytes = new List<byte>();
            byte[] size = new byte[5];
            List<int> fileid = new List<int>();
            fileid.Add(4);
            foreach (var item in files)
            {
                fileid.Add(item.Value.ID);
            }
            return rawBytes.ToArray();
        }

        public override byte[] GetBytes(out int length)
        {
            throw new NotImplementedException();
        }

        internal override void BuildFromByteStream(byte[] stream)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var f in files)
            {
                yield return f.Value.Head._head;
            }
            foreach (var f in folders)
            {
                yield return f.Value._head._head;
            }
        }

        IEnumerator<SSDSegmentHead> IEnumerable<SSDSegmentHead>.GetEnumerator()
        {
            foreach (var f in files)
            {
                yield return f.Value.Head._head;
            }
            foreach (var f in folders)
            {
                yield return f.Value._head._head;
            }
        }

    }
}
