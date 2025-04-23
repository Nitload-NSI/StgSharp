//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepOrientedEdge.cs"
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

using System;
using System.Collections.Generic;

namespace StgSharp.Modeling.Step
{
    public class StepOrientedEdge : StepEdge
    {

        private StepEdge _edgeElement;

        private StepOrientedEdge() { }

        public StepOrientedEdge(
               string name,
               StepVertex edgeStart,
               StepVertex edgeEnd,
               StepEdge edgeElement,
               bool orientation )
            : base( name, edgeStart, edgeEnd )
        {
            EdgeElement = edgeElement;
            Orientation = orientation;
        }

        public bool Orientation { get; set; }

        public StepEdge EdgeElement
        {
            get { return _edgeElement; }
            set
            {
                if( value == null ) {
                    throw new ArgumentNullException();
                }

                _edgeElement = value;
            }
        }

        public override StepItemType ItemType => StepItemType.OrientedEdge;

        internal static StepOrientedEdge CreateFromSyntaxList(
                                         StepModel binder,
                                         ExpSyntaxNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            StepOrientedEdge orientedEdge = new StepOrientedEdge();
            enumerator.AssertEnumeratorCount( 5 );
            orientedEdge.Name = ( enumerator.Values[ 0 ]as ExpStringNode )!.Value;
            binder.BindValue(
                enumerator.Values[ 1 ], v => orientedEdge.EdgeStart = v.AsType<StepVertex>() );
            binder.BindValue(
                enumerator.Values[ 2 ], v => orientedEdge.EdgeEnd = v.AsType<StepVertex>() );
            binder.BindValue(
                enumerator.Values[ 3 ], v => orientedEdge.EdgeElement = v.AsType<StepEdge>() );
            orientedEdge.Orientation = ( enumerator.Values[ 4 ]as ExpBoolNode )!.Value;
            return orientedEdge;
        }

    }
}

