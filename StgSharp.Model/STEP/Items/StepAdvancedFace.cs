//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepAdvancedFace.cs"
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

using System.Linq;

namespace StgSharp.Model.Step
{
    public class StepAdvancedFace : StepFaceSurface, IExpConvertableFrom<StepAdvancedFace>
    {

        public StepAdvancedFace( StepModel model ) : base( model ) { }

        public override StepItemType ItemType => StepItemType.AdvancedFace;

        public void FromInstance( StepAdvancedFace entity )
        {
            base.FromInstance( entity );
        }

        internal static StepAdvancedFace FromSyntax( StepModel binder, ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            StepAdvancedFace face = new StepAdvancedFace( binder );
            enumerator.AssertEnumeratorCount( 4 );
            face.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;

            ExpNodePresidentEnumerator boundsList = enumerator[ 1 ].TupleToPresidentEnumerator();
            face.Bounds.Clear();
            face.Bounds
                .AddRange(
                    Enumerable.Range( 0, enumerator.Count ).Select( _ => ( StepFaceBound )null ) );
            for( int i = 0; i < boundsList.Count; i++ )
            {
                int j = i; // capture to avoid rebinding
                face.Bounds[ j ] = binder[ boundsList[ j ] ] as StepFaceBound;
            }
            face.FaceGeometry = binder[ enumerator[ 2 ] ] as StepSurface;
            face.SameSense = ( enumerator[ 3 ] as ExpBoolNode )!.Value;

            return face;
        }

    }
}

