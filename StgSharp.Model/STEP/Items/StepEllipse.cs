//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepEllipse.cs"
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
    public class StepEllipse : StepConic, IExpConvertableFrom<StepEllipse>
    {

        private StepAxis2Placement _position;

        private StepEllipse( StepModel model ) : base( model ) { }

        public StepEllipse(
               StepModel model,
               string name,
               StepAxis2Placement position,
               float semiAxis1,
               float semiAxis2 )
            : base( model )
        {
            Position = position;
            SemiAxis1 = semiAxis1;
            SemiAxis2 = semiAxis2;
        }

        public float SemiAxis1 { get; set; }

        public float SemiAxis2 { get; set; }

        public StepAxis2Placement Position
        {
            get { return _position; }
            set
            {
                ArgumentNullException.ThrowIfNull( value );

                _position = value;
            }
        }

        public override StepItemType ItemType => StepItemType.Ellipse;

        public void FromInstance( StepEllipse entity )
        {
            base.FromInstance( entity );
            SemiAxis1 = entity.SemiAxis1;
            SemiAxis2 = entity.SemiAxis2;
        }

        internal static StepEllipse FromSyntax( StepModel binder, ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            StepEllipse ellipse = new StepEllipse( binder );
            enumerator.AssertEnumeratorCount( 4 );
            ellipse.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            ellipse.Position = binder[ ( enumerator[ 1 ] as StepEntityInstanceNode )!.Id ]as StepAxis2Placement2D;
            ellipse.SemiAxis1 = ( enumerator[ 2 ] as ExpRealNumberNode )!.Value;
            ellipse.SemiAxis2 = ( enumerator[ 3 ] as ExpRealNumberNode )!.Value;
            return ellipse;
        }

    }
}

