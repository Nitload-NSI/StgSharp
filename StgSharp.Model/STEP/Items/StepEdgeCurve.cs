//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepEdgeCurve.cs"
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
using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;

namespace StgSharp.Model.Step
{
    public class StepEdgeCurve : StepEdge
    {

        private StepCurve _edgeGeometry;

        private StepEdgeCurve() : base() { }

        public StepEdgeCurve(
               StepVertex edgeStart,
               StepVertex edgeEnd,
               StepCurve edgeGeometry,
               bool isSameSense )
            : base( edgeStart, edgeEnd )
        {
            EdgeGeometry = edgeGeometry;
            IsSameSense = isSameSense;
        }

        public bool IsSameSense { get; set; }

        public StepCurve EdgeGeometry
        {
            get { return _edgeGeometry; }
            set
            {
                if( value == null ) {
                    throw new ArgumentNullException();
                }

                _edgeGeometry = value;
            }
        }

        public override StepItemType ItemType => StepItemType.EdgeCurve;

        internal static StepEdgeCurve FromSyntax( StepModel binder, ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            StepEdgeCurve edgeCurve = new StepEdgeCurve();
            enumerator.AssertEnumeratorCount( 5 );
            edgeCurve.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            binder.BindValue( enumerator[ 1 ], v => edgeCurve.EdgeStart = v.AsType<StepVertex>() );
            binder.BindValue( enumerator[ 2 ], v => edgeCurve.EdgeEnd = v.AsType<StepVertex>() );
            binder.BindValue( enumerator[ 3 ],
                              v => edgeCurve.EdgeGeometry = v.AsType<StepCurve>() );
            edgeCurve.IsSameSense = ( enumerator[ 4 ]as ExpBoolNode )!.Value;
            return edgeCurve;
        }

    }
}

