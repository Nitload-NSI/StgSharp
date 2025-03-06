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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpNodeNextEnumerator : IEnumerator<ExpNode>
    {

        private ExpNode _begin;
        private ExpNode _current;

        public ExpNodeNextEnumerator( ExpNode token )
        {
            _begin = token;
            _current = token;
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
            _current = _begin;
        }

        object IEnumerator.Current => throw new NotImplementedException();

        ExpNode IEnumerator<ExpNode>.Current => throw new NotImplementedException(
            );

    }
}
