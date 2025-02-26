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
using System.Text.RegularExpressions;

namespace StgSharp.Script
{
    /// <summary>
    /// Transmit a continuous part of script code to interpreter or compiler. This transmitter is
    /// designed for single-thread-write and single-thread read. Any multi thread operation will
    /// cause undefined behaviour.
    /// </summary>
    public class ScriptSourceTransmitter : IScriptSourceProvider
    {

        private static ScriptSourceTransmitter _empty = new ScriptSourceTransmitter(
            string.Empty, 0, 0 )
        {
            _cache = [],
            _buffer = []
        };

        private string[] _buffer;
        private bool _isWriting = true;
        private int _begin, _index;
        private List<string> _cache;
        private readonly string _completeMark;

        internal ScriptSourceTransmitter( string[] buffer, int start )
        {
            _buffer = buffer;
            _begin = start;
            _isWriting = false;
        }

        public ScriptSourceTransmitter( string endMark, int start )
        {
            _cache = new List<string>();
            _completeMark = endMark;
            _begin = start;
            _index = 0;
        }

        public ScriptSourceTransmitter(
                       string endMark,
                       int start,
                       int capacity )
        {
            _cache = new List<string>( capacity );
            _completeMark = endMark;
            _begin = start;
            _index = 0;
        }

        public bool IsEmpty => _isWriting ?
                ( _cache.Count == 0 ) : ( _buffer.Length == 0 ) || ( _index >=
                                                                     _buffer.Length );

        public bool IsFrozen => !_isWriting;

        public int Location => _begin + _index;

        public static ScriptSourceTransmitter Empty => _empty;

        public void EndWriting()
        {
            if( !_isWriting ) {
                return;
            }
            _isWriting = false;
            _buffer = _cache.ToArray();
            _cache = null!;
        }

        public string ReadLine()
        {
            if( _isWriting ) {
                throw new InvalidOperationException(
                    "Cannot read before all writing operation." );
            }
            if( _index >= _buffer.Length ) {
                return string.Empty;
            }
            string str = _buffer[ _index ];
            _index++;
            return str;
        }

        public string ReadLine( out int position )
        {
            if( _isWriting ) {
                throw new InvalidOperationException(
                    "Cannot read before all writing operation." );
            }
            if( _index >= _buffer.Length ) {
                position = -1;
                return string.Empty;
            }
            position = _begin + _index;
            string str = _buffer[ _index ];
            _index++;
            return str;
        }

        public IScriptSourceProvider Slice( int location, int count )
        {
            ArgumentOutOfRangeException.ThrowIfLessThan( location, _begin );
            ArgumentOutOfRangeException.ThrowIfGreaterThan(
                location + count, _begin + _buffer.Length );
            return new ScriptSourceTransmitter(
                _buffer.AsSpan().Slice( location, count ).ToArray(), location );
        }

        public void WriteLine( string line )
        {
            if( !_isWriting ) {
                return;
            }
            if( string.Equals( _completeMark, line ) ) {
                EndWriting();
                return;
            }
            _cache.Add( line );
        }

    }
}
