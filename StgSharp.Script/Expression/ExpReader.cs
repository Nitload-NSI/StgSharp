//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpReader.cs"
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
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace StgSharp.Script.Expression
{
    public class ExpFileReader : IDisposable, IScriptSourceProvider
    {

        private bool disposedValue;
        private FileStream _expFileStream;
        private int _location;
        private MemoryMappedFile _expMemoryMappedFile;
        private MemoryMappedViewStream _expMemoryMappedStream;

        private SemaphoreSlim readLineSemaphore = new SemaphoreSlim( 1, 1 );
        private StreamReader _memoryReader;

        public ExpFileReader( string path )
        {
            if( !File.Exists( path ) ) {
                throw new FileNotFoundException();
            }
            _expFileStream = new FileStream(
                path, FileMode.Open, FileAccess.Read );
            _expMemoryMappedFile = MemoryMappedFile.CreateFromFile(
                _expFileStream, null, 0, MemoryMappedFileAccess.Read,
                HandleInheritability.None, true );
            _expMemoryMappedStream = _expMemoryMappedFile.CreateViewStream(
                offset: 0, size: 0, access: MemoryMappedFileAccess.Read );
            _memoryReader = new StreamReader( _expMemoryMappedStream );
        }

        public int Location => _location;

        // ~ExpFileReader()
        // {
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }

        public string ReadLine()
        {
            readLineSemaphore.Wait();
            string line = _memoryReader.ReadLine();
            _location++;
            readLineSemaphore.Release();
            return line;
        }

        public void ResetToBegin()
        {
            readLineSemaphore.Wait();
            _expMemoryMappedStream.Seek( 0, SeekOrigin.Begin );
            _location = 0;
            readLineSemaphore.Release();
        }

        protected virtual void Dispose( bool disposing )
        {
            if( !disposedValue ) {
                if( disposing ) {
                    _expFileStream.Dispose();
                    _expMemoryMappedFile.Dispose();
                    _expMemoryMappedStream.Dispose();
                    _memoryReader.Dispose();
                }
                disposedValue = true;
            }
        }

    }
}
