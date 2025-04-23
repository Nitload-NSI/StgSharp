//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipelineScheduler.cs"
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
using StgSharp.Stg;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace StgSharp.PipeLine
{
    /// <summary>
    ///   A sets of complex logic designed for running in multi threads environment. Warning: do not
    ///   run a <see cref="PipelineScheduler" /> instance in multi threads more than once.
    /// </summary>
    public partial class PipelineScheduler : IConvertableToPipelineNode
    {

        private BeginningNode _beginning;
        private Dictionary<string, PipelineNode> allNode;
        private EndingNode _ending;

        private List<PipelineNode>
            _concurrentNodes = new List<PipelineNode>(), 
            _mainThreadNodes = new List<PipelineNode>();

        private List<PipelineNode> globalNodeList;
        private List<PipelineNode> mainThreadNodeList;

        private SemaphoreSlim runningStat;

        internal int currentGlobalNodeIndex;
        internal int currentNativeNodeIndex;

        public PipelineScheduler()
        {
            allNode = new Dictionary<string, PipelineNode>();
            globalNodeList = new List<PipelineNode>();
            mainThreadNodeList = new List<PipelineNode>();
            runningStat = new SemaphoreSlim( 1, 1 );
            _beginning = new BeginningNode( this );
            _ending = new EndingNode( this );
        }

        public bool IsRunning
        {
            get => runningStat.CurrentCount == 0;
        }

        public IEnumerable<string> InputInterfacesName
        {
            get { return _beginning.OutputPorts.Keys; }
        }

        public IEnumerable<string> OutputInterfacesName
        {
            get { return _beginning.OutputPorts.Keys; }
        }

        public PipelineNode BeginLayer => _beginning;

        public PipelineNode EndLayer => _ending;

        public PipelineNodeOperation Operation
        {
            get => ( this as IConvertableToPipelineNode ).NodeMain;
        }

        /**/

        internal SemaphoreSlim RunningStat
        {
            get => runningStat;
        }
        /**/

        public void AddNode( PipelineNode node, bool isNative )
        {
            ArgumentNullException.ThrowIfNull( node );
            node.IsNative = isNative;
            allNode.Add( node.Name, node );
        }

        public void ArrangeNodeOrder()
        {
            LinkedList<PipelineNode> _cache = new LinkedList<PipelineNode>();
            foreach( PipelineNode node in _beginning.Next )
            {
                if( node.IsNative )
                {
                    _mainThreadNodes.Add( node );
                } else
                {
                    _concurrentNodes.Add( node );
                }
                foreach( PipelineNode item in node.Next )
                {
                    item.Previous.Remove( node );
                    _cache.AddLast( item );
                }
            }
            while( _cache.Count > 0 )
            {
                LinkedListNode<PipelineNode> begin = _cache.First!,current = begin,zero;
                do
                {
                    PipelineNode node = current.Value;
                    if( current.Value.Previous.Count == 0 )
                    {
                        zero = current;
                        current = current.Next!;
                        if( node.IsNative )
                        {
                            _mainThreadNodes.Add( node );
                        } else
                        {
                            _concurrentNodes.Add( node );
                        }
                        foreach( PipelineNode item in node.Next )
                        {
                            item.Previous.Remove( node );
                            _cache.AddLast( item );
                        }
                        _cache.Remove( zero );
                    } else
                    {
                        current = current.Next!;
                    }
                } while (current.Next != begin);
            }
        }

        public PipelineNode Create( [NotNull]IConvertableToPipelineNode body, string name )
        {
            PipelineNode node = new PipelineNode( name, body, false );
            return node;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void SetInput( params (string name, IPipeLineConnectionPayload arg)[] parameters )
        {
            _beginning.SetInputData( parameters );
        }

        public void Terminate()
        {
            Interlocked.Exchange( ref currentGlobalNodeIndex, int.MaxValue );
            Interlocked.Exchange( ref currentNativeNodeIndex, int.MaxValue );
        }

        internal bool RequestNexGlobalNode( out PipelineNode node )
        {
            if( currentGlobalNodeIndex < globalNodeList.Count )
            {
                node = globalNodeList[ currentGlobalNodeIndex ];
                Interlocked.Increment( ref currentGlobalNodeIndex );
                return true;
            }
            node = default!;
            return false;
        }

        internal bool RequestNextNativeNode( out PipelineNode node )
        {
            if( currentNativeNodeIndex < mainThreadNodeList.Count )
            {
                node = mainThreadNodeList[ currentNativeNodeIndex ];
                Interlocked.Increment( ref currentNativeNodeIndex );
                return true;
            }
            node = default!;
            return false;
        }

        void IConvertableToPipelineNode.NodeMain(
                                        in Dictionary<string, PipelineNodeImport> input,
                                        in Dictionary<string, PipelineNodeExport> output )
        {
            PipeLineRunner.Run( this );
            PipelineNodeExport.SkipAll( output );
        }

        ~PipelineScheduler()
        {
            runningStat.Dispose();
        }

    }
}
