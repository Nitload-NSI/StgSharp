//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeNextEnumerator.cs"
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public static class ExpNodeNextEnumeratorExtensions
    {

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ExpNodeNextEnumerator AsEnumerator( this ExpNode token )
        {
            return new ExpNodeNextEnumerator( token );
        }

    }

    public class ExpNodeNextEnumerator : IEnumerator<ExpNode>
    {

        private ExpNode _current;

        private List<ExpNode> _begin = new List<ExpNode>();

        public ExpNodeNextEnumerator( ExpNode token )
        {
            _begin[ 0 ] = token;
            _current = token;
            ExpNode t = token;
            while( !ReferenceEquals( t.Next, token ) && t.Next != null )
            {
                t = t.Next;
                _begin.Add( t );
            }
        }

        public ExpNode this[ int index ]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfNegative( index );
                ArgumentOutOfRangeException.ThrowIfGreaterThan( index, _begin.Count );
                return _begin[ index ];
            }
        }

        public List<ExpNode> Values => _begin;

        public void AssertEnumeratorCount( int count )
        {
            if( _begin.Count != count ) {
                throw new InvalidOperationException(
                    $"Enumerator count mismatch. Expected {count}, but got {_begin.Count}." );
            }
        }

        void IDisposable.Dispose()
        {
            return;
        }

        bool IEnumerator.MoveNext()
        {
            ExpNode next = _current.Next;
            if( ReferenceEquals( next, _begin ) || next == null ) {
                return false;
            }
            _current = next;
            return true;
        }

        void IEnumerator.Reset()
        {
            _current = _begin[ 0 ];
        }

        object IEnumerator.Current => _current;

        ExpNode IEnumerator<ExpNode>.Current => _current;

    }
}
