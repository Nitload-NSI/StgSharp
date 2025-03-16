//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Blueprint.cs"
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.PipeLine
{
    /// <summary>
    /// A sets of complex logic designed for running in multi threads environment. Warning: do not
    /// run a <see cref="BlueprintScheduler" /> instance in multi threads more than once.
    /// </summary>
    public partial class BlueprintScheduler : IConvertableToBlueprintNode
    {

        private List<PipeLineNode>[] _concurrentNodes, _mainThreadNodes;

        private BeginningNode beginNode;
        private Dictionary<string, PipeLineNode> allNode;
        private EndingNode endNode;

        private Func<bool> terminate;
        private List<PipeLineNode> globalNodeList;
        private List<PipeLineNode> mainThreadNodeList;

        private SemaphoreSlim runningStat;

        internal int currentGlobalNodeIndex;
        internal int currentNativeNodeIndex;

        public BlueprintScheduler()
        {
            allNode = new Dictionary<string, PipeLineNode>();
            globalNodeList = new List<PipeLineNode>();
            mainThreadNodeList = new List<PipeLineNode>();
            runningStat = new SemaphoreSlim( 1, 1 );
            beginNode = new BeginningNode( this );
            endNode = new EndingNode( this );
        }

        public BlueprintNodeOperation Operation
        {
            get => ExecuteMain;
        }

        public bool IsRunning
        {
            get => runningStat.CurrentCount == 0;
        }

        public IEnumerable<string> InputInterfacesName
        {
            get { return beginNode.OutputInterfaces.Keys; }
        }

        public IEnumerable<string> OutputInterfacesName
        {
            get { return beginNode.OutputInterfaces.Keys; }
        }

        public PipeLineNode BeginLayer => beginNode;

        public PipeLineNode EndLayer => endNode;

        /**/

        internal SemaphoreSlim RunningStat
        {
            get => runningStat;
        }
        /**/

        public void AddNode( PipeLineNode node, bool isNative )
        {
            _ = node ?? throw new ArgumentNullException( nameof( node ) );
            node.IsNative = isNative;
            allNode.Add( node.Name, node );
        }

        public void ExecuteMain(
                            in Dictionary<string, PipelineConnector> input,
                            in Dictionary<string, PipelineConnector> output )
        {
            PipeLineRunner.Run( this );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public (string name, PipeLineConnectorArgs arg)[] GetOutput()
        {
            return endNode.OutputData;
        }

        public void Init()
        {
            foreach( KeyValuePair<string, PipeLineNode> item in allNode ) {
                if( item.Value.IsNative ) {
                    mainThreadNodeList.Add( item.Value );
                } else {
                    globalNodeList.Add( item.Value );
                }
            }
            mainThreadNodeList.Sort();
            globalNodeList.Sort();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void SetInput(
                            params (string name, PipeLineConnectorArgs arg)[] parameters )
        {
            beginNode.InputData = parameters;
        }

        public void Terminate()
        {
            Interlocked.Exchange( ref currentGlobalNodeIndex, int.MaxValue );
            Interlocked.Exchange( ref currentNativeNodeIndex, int.MaxValue );
        }

        internal bool RequestNexGlobalNode( out PipeLineNode node )
        {
            if( currentGlobalNodeIndex < globalNodeList.Count ) {
                node = globalNodeList[ currentGlobalNodeIndex ];
                Interlocked.Increment( ref currentGlobalNodeIndex );
                return true;
            }
            node = default!;
            return false;
        }

        internal bool RequestNextNativeNode( out PipeLineNode node )
        {
            if( currentNativeNodeIndex < mainThreadNodeList.Count ) {
                node = mainThreadNodeList[ currentNativeNodeIndex ];
                Interlocked.Increment( ref currentNativeNodeIndex );
                return true;
            }
            node = default!;
            return false;
        }

        ~BlueprintScheduler()
        {
            runningStat.Dispose();
        }

    }
}
