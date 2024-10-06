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
    public class SSDFolder : ISSDSerializable
    {

        internal Dictionary<string, SSDFolder> folders;
        internal int _folderID;
        internal string _name;

        internal SSDFolder()
        {
            folders = new Dictionary<string, SSDFolder>();
        }

        internal SSDFolder( string name )
        {
            if( name == "root" ) {
                throw new ArgumentException(
                    "Name cannot be root," + "it is preserved for root file indexer." );
            }
            if( name == "default" ) {
                throw new ArgumentException(
                    "Name cannot be default," + "it is preserved for default file id provider." );
            }
            _name = name;
            folders = new Dictionary<string, SSDFolder>();
        }

        public SerializableTypeCode SSDTypeCode => SerializableTypeCode.VirtualFolder;

        public void FromBytes( byte[] stream )
        {
            throw new NotImplementedException();
        }

        //TODO 虚拟文件夹的GetBytes好难写
        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

    }
}
