//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipelineMarshal.cs"
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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public static class PipelineMarshal
    {

        public static IEnumerable<PipelineNode> GetNodeSortedEnumeration(
                                                PipelineScheduler scheduler )
        {
            ArgumentNullException.ThrowIfNull( scheduler );
            return new PipelineNodeEnumerator( scheduler )
            {
                _count = scheduler._allNode.Count,
            };
        }

        private class PipelineNodeEnumerator : ICollection<PipelineNode>
        {

            private readonly List<List<PipelineNode>> _mt, _c;

            public int _count;

            public PipelineNodeEnumerator( PipelineScheduler scheduler )
            {
                if( !scheduler.TryGetAllNode( out _mt, out _c ) ) {
                    throw new InvalidOperationException( "Binded scheduler is not sorted." );
                }
            }

            public bool IsReadOnly => true;

            public int Count => _count;

            public IEnumerator<PipelineNode> GetEnumerator()
            {
                for( int i = 1; i < _mt.Count; i++ )
                {
                    foreach( PipelineNode node in _mt[ i ] ) {
                        yield return node;
                    }
                    foreach( PipelineNode node in _c[ i ] ) {
                        yield return node;
                    }
                }
            }

            void ICollection<PipelineNode>.Add( PipelineNode item )
            {
                throw new NotSupportedException();
            }

            void ICollection<PipelineNode>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<PipelineNode>.Contains( PipelineNode item )
            {
                throw new NotSupportedException();
            }

            void ICollection<PipelineNode>.CopyTo( PipelineNode[] array, int arrayIndex )
            {
                throw new NotSupportedException();
            }

            bool ICollection<PipelineNode>.Remove( PipelineNode item )
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

    }
}
