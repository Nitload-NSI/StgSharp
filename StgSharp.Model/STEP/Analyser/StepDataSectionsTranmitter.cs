//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepDataSectionsTranmitter.cs"
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
using StgSharp.Script;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Modeling.Step
{
    public class StepDataSectionTransmitter : IScriptSourceProvider
    {

        private const string EndDataText = "ENDSEC;";

        private bool _isEmpty;
        private ConcurrentQueue<string> _cache = new ConcurrentQueue<string>();
        private int _line = 0;

        public bool IsEmpty => _isEmpty;

        public bool IsWriting { get; private set; } = true;

        public int Location => _line;

        public void EndWriting()
        {
            _cache.Enqueue( EndDataText );
            IsWriting = false;
        }

        public string ReadLine()
        {
            _cache.TryDequeue( out string? line );
            while( !IsEmpty )
            {
                if( _cache.TryDequeue( out line ) )
                {
                    if( line == EndDataText )
                    {
                        _isEmpty = true;
                        return string.Empty;
                    }
                    _line++;
                    return line;
                }
            }
            return string.Empty;
        }

        public string ReadLine( out int position )
        {
            string ret = ReadLine();
            position = _line;
            return ret;
        }

        public void WriteLine( string line )
        {
            if( line == EndDataText ) {
                EndWriting();
            }
            _cache.Enqueue( line );
        }

        IScriptSourceProvider IScriptSourceProvider.Slice( int location, int count )
        {
            throw new NotSupportedException();
        }

    }
}
