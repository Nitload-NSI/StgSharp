//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BlueprintSchedueler.cs"
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
using StgSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StgSharp.Blueprint
{
    /// <summary>
    /// A sets of complex logic designed for running in multi threads environment. Warning: do not
    /// run a <see cref="BlueprintSchedueler" /> instance in multi threads more than once.
    /// </summary>
    public partial class BlueprintSchedueler : IConvertableToBlueprintNode
    {

        private BeginningNode beginNode;
        private Dictionary<string, BlueprintNode> allNode;
        private EndingNode endNode;

        private Func<bool> terminate;
        private List<BlueprintNode> globalNodeList;
        private List<BlueprintNode> nativeNodeList;

        private SemaphoreSlim runningStat;

        internal int currentGlobalNodeIndex;
        internal int currentNativeNodeIndex;

        public BlueprintSchedueler()
        {
            allNode = new Dictionary<string, BlueprintNode>();
            globalNodeList = new List<BlueprintNode>();
            nativeNodeList = new List<BlueprintNode>();
            runningStat = new SemaphoreSlim( 1, 1 );
            beginNode = new BeginningNode( this );
            endNode = new EndingNode( this );
        }

        public BlueprintNode BeginLayer => beginNode;

        public BlueprintNode EndLayer => endNode;

        public bool IsRunning
        {
            get => runningStat.CurrentCount == 0;
        }

        public ReadOnlySpan<string> InputInterfacesName
        {
            get { return beginNode.OutputInterfaces
                          .Select( p => p.Key )
                          .ToArray(); }
        }

        public ReadOnlySpan<string> OutputInterfacesName
        {
            get { return beginNode.OutputInterfaces
                          .Select( p => p.Key )
                          .ToArray(); }
        }

        /**/

        internal SemaphoreSlim RunningStat
        {
            get => runningStat;
        }
        /**/

        public void AddNode( BlueprintNode node, bool isNative )
        {
            _ = node ?? throw new ArgumentNullException( nameof( node ) );
            node.IsNative = isNative;
            allNode.Add( node.Name, node );
        }


        public void ExecuteMain(
                            in Dictionary<string, BlueprintPipeline> input,
                            in Dictionary<string, BlueprintPipeline> output )
        {
            BlueprintRunner.Run( this );
        }

        public BlueprintNodeOperation Operation
        {
            get => ExecuteMain;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public (string name, BlueprintPipelineArgs arg)[] GetOutput()
        {
            return endNode.OutputData;
        }

        public void Init()
        {
            foreach( KeyValuePair<string, BlueprintNode> item in allNode ) {
                if( item.Value.IsNative ) {
                    nativeNodeList.Add( item.Value );
                } else {
                    globalNodeList.Add( item.Value );
                }
            }
            nativeNodeList.Sort();
            globalNodeList.Sort();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void SetInput(
                            params (string name, BlueprintPipelineArgs arg)[] parameters )
        {
            beginNode.InputData = parameters;
        }

        public void Terminate()
        {
            Interlocked.Exchange( ref currentGlobalNodeIndex, int.MaxValue );
            Interlocked.Exchange( ref currentNativeNodeIndex, int.MaxValue );
        }

        internal bool RequestNexGlobalNode( out BlueprintNode node )
        {
            if( currentGlobalNodeIndex < globalNodeList.Count ) {
                node = globalNodeList[ currentGlobalNodeIndex ];
                Interlocked.Increment( ref currentGlobalNodeIndex );
                return true;
            }
            node = default!;
            return false;
        }

        internal bool RequestNextNativeNode( out BlueprintNode node )
        {
            if( currentNativeNodeIndex < nativeNodeList.Count ) {
                node = nativeNodeList[ currentNativeNodeIndex ];
                Interlocked.Increment( ref currentNativeNodeIndex );
                return true;
            }
            node = default!;
            return false;
        }

        ~BlueprintSchedueler()
        {
            runningStat.Dispose();
        }

    }
}
