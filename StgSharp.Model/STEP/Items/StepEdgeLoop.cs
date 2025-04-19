//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepEdgeLoop.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using StgSharp.Modeling.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System.Collections.Generic;
using System.Linq;

namespace StgSharp.Modeling.Step
{
    public class StepEdgeLoop : StepLoop
    {

        public StepEdgeLoop( string name, IEnumerable<StepOrientedEdge> edgeList )
            : base( name )
        {
            EdgeList = new List<StepOrientedEdge>( edgeList );
        }

        public StepEdgeLoop( string name, params StepOrientedEdge[] edgeList )
            : this( name, ( IEnumerable<StepOrientedEdge> )edgeList ) { }

        public List<StepOrientedEdge> EdgeList { get; private set; }

        public override StepItemType ItemType => StepItemType.EdgeLoop;

        internal static StepEdgeLoop CreateFromSyntaxList(
                                     StepModel binder,
                                     ExpSyntaxNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            enumerator.AssertEnumeratorCount( 2 );
            ExpNodeNextEnumerator edgeSyntaxList = enumerator.Values[ 1 ].ToEnumerator();
            StepEdgeLoop edgeLoop = new StepEdgeLoop(
                string.Empty, new StepOrientedEdge[edgeSyntaxList.Values.Count] );
            edgeLoop.Name = ( enumerator.Values[ 0 ]as ExpStringNode )!.Value;
            for( int i = 0; i < edgeSyntaxList.Values.Count; i++ )
            {
                int j = i; // capture to avoid rebinding
                binder.BindValue(
                    edgeSyntaxList.Values[ j ],
                    v => edgeLoop.EdgeList[ j ] = v.AsType<StepOrientedEdge>() );
            }

            return edgeLoop;
        }

        internal override IEnumerable<StepRepresentationItem> GetReferencedItems()
        {
            return EdgeList;
        }

    }
}

