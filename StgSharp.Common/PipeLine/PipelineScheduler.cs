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
        private bool _isAvailable = false;
        private EndingNode _ending;

        private int mtLevel, cLevel,mtIndex, cIndex;
        private List<List<PipelineNode>> _concurrentNodes, _mainThreadNodes ;
        private SemaphoreSlim runningStat,_concurrentLock = new SemaphoreSlim( 1, 1 );
        internal Dictionary<PipelineNodeLabel, PipelineNode> _allNode;

        public PipelineScheduler()
        {
            _allNode = new Dictionary<PipelineNodeLabel, PipelineNode>();
            runningStat = new SemaphoreSlim( 1, 1 );
            _beginning = new BeginningNode( this );
            _ending = new EndingNode( this );
        }
        /**/

        public PipelineNode this[ PipelineNodeLabel label ]
        {
            get { return _allNode[ label ]; }
        }

        public bool IsRunning
        {
            get => runningStat.CurrentCount == 0;
        }

        public IEnumerable<string> InputPortName
        {
            get => _beginning.InputPorts.Keys;
        }

        public IEnumerable<string> OutputPortName
        {
            get => _beginning.OutputPorts.Keys;
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

        public void AddNode( PipelineNode node, bool isNative )
        {
            ArgumentNullException.ThrowIfNull( node );
            node.IsNative = isNative;
            _allNode.TryAdd( node.Label, node );
        }

        public void ArrangeNodes()
        {
            if( _isAvailable ) {
                return;
            }
            int maxLevel = 1;
            Queue<PipelineNode> completedNodes = new Queue<PipelineNode>();
            foreach( PipelineNode item in _beginning.Next )
            {
                item.TryIncreaseLevelTo( 1 );
                completedNodes.Enqueue( item );
            }

            //calc level of nodes O(V)
            while( completedNodes.Count > 0 )
            {
                PipelineNode u = completedNodes.Dequeue();
                foreach( PipelineNode v in u.Next )
                {
                    if( !v.TryIncreaseLevelTo( u.Level + 1 ) ) {
                        throw new InvalidOperationException( "Ring in DAG detected." );
                    }
                    if( v.RemainingIncreaseCount == 0 )
                    {
                        completedNodes.Enqueue( v );
                        maxLevel = int.Max( maxLevel, v.Level );
                    }
                }
            }

            //bucket sort of nodes, O(E)
            _concurrentNodes = new List<List<PipelineNode>>( maxLevel + 1 );
            _mainThreadNodes = new List<List<PipelineNode>>( maxLevel + 1 );
            for( int i = 0; i <= maxLevel; i++ )
            {
                _concurrentNodes.Add( [] );
                _mainThreadNodes.Add( [] );
            }
            foreach( PipelineNode item in _allNode.Values )
            {
                if( item.IsNative )
                {
                    _mainThreadNodes[ item.Level ].Add( item );
                } else
                {
                    _concurrentNodes[ item.Level ].Add( item );
                }
            }
            foreach( List<PipelineNode> item in _concurrentNodes ) {
                item.TrimExcess();
            }
            foreach( List<PipelineNode> item in _mainThreadNodes ) {
                item.TrimExcess();
            }
            _isAvailable = true;
        }

        public PipelineNode Create(
                            PipelineNodeLabel label,
                            [NotNull] IConvertableToPipelineNode body )
        {
            if( _allNode.TryGetValue( label, out PipelineNode? node ) ) {
                throw new InvalidOperationException( "Label has exist in scheduler." );
            }
            node = new PipelineNode( label, body, false );
            _allNode.Add( label, node );
            return node;
        }

        public PipelineNode GetNode( PipelineNodeLabel label )
        {
            if( _allNode.TryGetValue( label, out PipelineNode? node ) )
            {
                return node;
            } else
            {
                throw new KeyNotFoundException();
            }
        }

        public void LinkToInput( PipelineNodeInPort port )
        {
            ArgumentNullException.ThrowIfNull( port );
            PipelineNodeOutPort p = _beginning.DefaultOut;
            p.Connect( port );
        }

        public void LinkToInput( PipelineNodeInPort port, string inputPortName )
        {
            ArgumentNullException.ThrowIfNull( port );
            PipelineNodeOutPort p = _beginning.DefineCertainOutputPort( inputPortName );
            p.Connect( port );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void SetInput( params (string label, IPipeLineConnectionPayload arg)[] parameters )
        {
            _beginning.SetInputData( parameters );
        }

        public void Terminate()
        {
            _concurrentLock.Wait();
            (mtLevel, cLevel, mtIndex, cIndex) = (int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            _concurrentLock.Release();
        }

        public bool TryGetAllNode(
                    out List<List<PipelineNode>> mainThreadNodes,
                    out List<List<PipelineNode>> concurrentNodes )
        {
            if( _isAvailable )
            {
                mainThreadNodes = _mainThreadNodes;
                concurrentNodes = _concurrentNodes;
                return true;
            }
            mainThreadNodes = null;
            concurrentNodes = null;
            return false;
        }

        internal bool RequestNexConcurrentNode( out PipelineNode node )
        {
            int index, level;
            if( cLevel >= _concurrentNodes.Count )
            {
                node = null!;
                return false;
            }
            _concurrentLock.Wait();
            while( cIndex >= _concurrentNodes[ cLevel ].Count )
            {
                Interlocked.Increment( ref cLevel );
                Interlocked.Exchange( ref cIndex, 0 );
                if( cLevel >= _concurrentNodes.Count )
                {
                    node = null!;
                    return false;
                }
            }
            index = cIndex;
            level = cLevel;
            Interlocked.Increment( ref cIndex );
            _concurrentLock.Release();
            node = _concurrentNodes[ level ][ index ];
            return true;
        }

        internal bool RequestNextMainThreadNode( out PipelineNode node )
        {
            if( mtLevel >= _mainThreadNodes.Count )
            {
                node = null!;
                return false;
            }
            while( mtIndex >= _mainThreadNodes[ cLevel ].Count )
            {
                Interlocked.Increment( ref mtLevel );
                Interlocked.Exchange( ref mtIndex, 0 );
                if( mtLevel >= _mainThreadNodes.Count )
                {
                    node = null!;
                    return false;
                }
            }
            node = _mainThreadNodes[ mtLevel ][ mtIndex ];
            return true;
        }

        internal void ResetIndex()
        {
            (mtLevel, cLevel, mtIndex, cIndex) = (0, 0, 0, 0);
            _concurrentLock.Release();
        }

        void IConvertableToPipelineNode.NodeMain(
                                        in Dictionary<string, PipelineNodeInPort> input,
                                        in Dictionary<string, PipelineNodeOutPort> output )
        {
            PipeLineRunner.Run( this );
            PipelineNodeOutPort.SkipAll( output );
        }

        ~PipelineScheduler()
        {
            runningStat.Dispose();
        }

    }
}
