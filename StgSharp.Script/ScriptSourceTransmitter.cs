//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ScriptSourceTransmitter.cs"
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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Script
{
    public class ScriptSourceTransmitter : IScriptSourceProvider
    {

        private bool _isWriting = true;
        private ConcurrentQueue<string> cache = new ConcurrentQueue<string>();
        private int _location;
        private readonly string _completeMark;

        public ScriptSourceTransmitter( string endMark )
        {
            _completeMark = endMark;
        }

        public int Location => throw new NotImplementedException();

        public string ReadLine()
        {
            cache.TryDequeue( out string line );
            _location++;
            return line;
        }

        public void WriteLine( string line )
        {
            if( !_isWriting ) {
                return;
            }
            if( string.Equals( _completeMark, line ) ) {
                _isWriting = false;
            }
            cache.Enqueue( line );
        }

    }
}
