//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepFaceBound.cs"
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

namespace StgSharp.Modeling.Step
{
    public class StepFaceBound : StepTopologicalRepresentationItem
    {

        protected StepFaceBound() : base( string.Empty ) { }

        public StepFaceBound( string name, StepLoop bound, bool orientation )
            : base( name )
        {
            Bound = bound;
            Orientation = orientation;
        }

        public bool Orientation { get; set; }

        public override StepItemType ItemType => StepItemType.FaceBound;

        public StepLoop Bound { get; set; }

        internal static StepFaceBound CreateFromSyntaxList(
                                      StepModel binder,
                                      ExpSyntaxNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            enumerator.AssertEnumeratorCount( 3 );
            StepFaceBound faceBound = new StepFaceBound();
            faceBound.Name = ( enumerator.Values[ 0 ]as ExpStringNode )!.Value;
            binder.BindValue( enumerator.Values[ 1 ], v => faceBound.Bound = v.AsType<StepLoop>() );
            faceBound.Orientation = ( enumerator.Values[ 2 ]as ExpBoolNode )!.Value;
            return faceBound;
        }

    }
}

