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

using StgSharp.Modeling.Step;

using StgSharp.Script;
using StgSharp.Script.Express;

using System.Linq;

namespace StgSharp.Modeling.Step
{
    public class StepAdvancedFace : StepFaceSurface
    {

        private StepAdvancedFace() : base( string.Empty ) { }

        public StepAdvancedFace( string name ) : base( name ) { }

        public override StepItemType ItemType => StepItemType.AdvancedFace;

        internal static StepAdvancedFace CreateFromSyntaxList(
                                         StepModel binder,
                                         ExpNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            StepAdvancedFace face = new StepAdvancedFace();
            enumerator.AssertEnumeratorCount( 4 );
            face.Name = ( enumerator.Values[ 0 ]as ExpStringNode )!.Value;

            ExpNodeNextEnumerator boundsList = enumerator.Values[ 1 ].AsEnumerator();
            face.Bounds.Clear();
            face.Bounds
                .AddRange(
                    Enumerable.Range( 0, enumerator.Values.Count )
                              .Select( _ => ( StepFaceBound )null ) );
            for( int i = 0; i < enumerator.Values.Count; i++ )
            {
                int j = i; // capture to avoid rebinding
                binder.BindValue(
                    enumerator.Values[ j ], v => face.Bounds[ j ] = v.AsType<StepFaceBound>() );
            }
            binder.BindValue(
                enumerator.Values[ 2 ], v => face.FaceGeometry = v.AsType<StepSurface>() );
            face.SameSense = ( enumerator.Values[ 3 ] as ExpBoolNode )!.Value;

            return face;
        }

    }
}

