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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public static class ExpNodeNextEnumeratorExtensions
    {

        public static ExpNodeNextEnumerator AsCollectionEnumerator( this ExpSyntaxNode token )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( token );
            IExpElementSource type = token.EqualityTypeConvert;
            while( enumerator.MoveNext() )
            {
                if( enumerator. Current != type ) {
                    throw new InvalidCastException();
                }
            }
            return enumerator;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ExpNodeNextEnumerator ToEnumerator( this ExpSyntaxNode token )
        {
            return new ExpNodeNextEnumerator( token );
        }

    }

    public class ExpNodeNextEnumerator : IEnumerator<ExpSyntaxNode>
    {

        private ExpSyntaxNode _current;

        private List<ExpSyntaxNode> _begin = new List<ExpSyntaxNode>();

        public ExpNodeNextEnumerator( ExpSyntaxNode token )
        {
            _begin[ 0 ] = token;
            _current = token;
            ExpSyntaxNode t = token;
            while( !ReferenceEquals( t.Next, token ) && t.Next != null )
            {
                t = t.Next;
                _begin.Add( t );
            }
        }

        public ExpSyntaxNode this[ int index ]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfNegative( index );
                ArgumentOutOfRangeException.ThrowIfGreaterThan( index, _begin.Count );
                return _begin[ index ];
            }
        }

        public ExpSyntaxNode Current => _current;

        public List<ExpSyntaxNode> Values => _begin;

        public void AssertEnumeratorCount( int count )
        {
            if( _begin.Count != count ) {
                throw new InvalidOperationException(
                    $"Enumerator count mismatch. Expected {count}, but got {_begin.Count}." );
            }
        }

        public void AssertEnumeratorCount( int min, int max )
        {
            if( _begin.Count < min || _begin.Count > max ) {
                throw new InvalidOperationException(
                    $"Enumerator count mismatch. Expected between {min} and {max}, but got {_begin.Count}." );
            }
        }

        public bool MoveNext()
        {
            ExpSyntaxNode next = _current.Next;
            if( ReferenceEquals( next, _begin ) || next == null ) {
                return false;
            }
            _current = next;
            return true;
        }

        void IDisposable.Dispose()
        {
            return;
        }

        void IEnumerator.Reset()
        {
            _current = _begin[ 0 ];
        }

        object IEnumerator.Current => _current;

    }
}
