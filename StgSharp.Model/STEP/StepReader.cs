//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepReader.cs"
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
using StgSharp;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    public partial class StepReader : IDisposable
    {

        private const string fileLabel = "ISO-10303-21;";

        private bool disposedValue;
        private FileStream _fileStream;
        private int _dataBeginLine;
        private long _headStart = -1, _headEnd = -1, _dataStart = -1, _size;
        private MemoryMappedFile _memoryFile;

        private StepReader() { }

        private StepReader( FileStream stream, long size )
        {
            _fileStream = stream;
            _size = size;
            _memoryFile = MemoryMappedFile.CreateFromFile(
                stream, mapName: null, size, MemoryMappedFileAccess.Read, HandleInheritability.None,
                leaveOpen: true );
        }

        // ~StepReader()
        // {
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }

        public static bool FromFile( string path, out StepReader reader )
        {
            FileInfo info = new FileInfo( path );
            if( !info.Exists )
            {
                reader = null!;
                return false;
            }
            FileStream stream = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096,
                useAsync: true );
            byte[] fileHead = new byte[40];
            stream.Read( fileHead, 0, fileLabel.Length );
            if( !Encoding.UTF8.GetString( fileHead ).Contains( fileLabel ) ) {
                throw new InvalidDataException( "The given file is not a STEP file" );
            }
            reader = new StepReader( stream, info.Length );
            reader.LocateSection();
            return  reader != null;
        }

        private void LocateSection()
        {
            long startPos = 0;
            byte[] buffer = new byte[1024];
            Span<byte> bufferSpan = buffer, 
                headerSec = Encoding.UTF8.GetBytes( "HEADER;" ),
                endsecSec = Encoding.UTF8.GetBytes( "ENDSEC;" ),
                dataSec = Encoding.UTF8.GetBytes( "DATA;" );
            _fileStream.Position = 0;
            for( ; startPos < _size; startPos += 1000 )
            {
                _fileStream.Read( buffer );
                if( _headStart == -1 )
                {
                    long pos = bufferSpan.IndexOf( headerSec );
                    if( pos >= 0 ) {
                        _headStart = startPos + pos;
                    }
                }
                if( _headEnd == -1 )
                {
                    long pos = bufferSpan.IndexOf( endsecSec );
                    if( pos >= 0 ) {
                        _headEnd = startPos + pos;
                    }
                }
                if( _dataStart == -1 )
                {
                    long pos = bufferSpan.IndexOf( dataSec );
                    if( pos >= 0 ) {
                        _dataStart = startPos + pos;
                    }
                } else
                {
                    break;
                }
                _fileStream.Seek( 1000, SeekOrigin.Current );
            }
        }

        private string ReadToken( StreamReader reader, ref long posCache )
        {
            StringBuilder token = new StringBuilder( 100 );
            while( token.Length == 0 || token[ token.Length - 1 ] != ';' )
            {
                string subString = reader.ReadLine()!;
                if( subString == null )
                {
                    break;
                }
                token.Append( subString.TrimStart() );
            }
            posCache += token.Length;
            return token.ToString();
        }

        private protected virtual void Dispose( bool disposing )
        {
            if( !disposedValue )
            {
                if( disposing )
                {
                    _fileStream.Dispose();
                    _memoryFile.Dispose();
                }

                disposedValue = true;
            }
        }

    }
}
