//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepCircle.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ��Software��), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ��AS IS��, 
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
    public class StepCircle : StepConic, IExpConvertableFrom<StepCircle>
    {

        private StepAxis2Placement _position;

        public StepCircle( StepModel model ) : base( model ) { }

        public StepCircle( StepModel model, StepAxis2Placement position, float radius )
            : base( model )
        {
            Position = position;
            Radius = radius;
        }

        public float Radius { get; set; }

        public StepAxis2Placement Position
        {
            get { return _position; }
            set
            {
                if( value == null ) {
                    throw new ArgumentNullException();
                }

                _position = value;
            }
        }

        public override StepItemType ItemType => StepItemType.Circle;

        public void FromInstance( StepCircle entity )
        {
            base.FromInstance( entity );
            this._position = entity.Position;
        }

        internal static StepCircle FromSyntax( StepModel binder, ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            StepCircle circle = new StepCircle( binder );
            enumerator.AssertEnumeratorCount( 3 );
            circle.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            circle.Position = binder[ enumerator[ 1 ] ] as StepAxis2Placement;
            circle.Radius = ( enumerator[ 2 ]as ExpRealNumberNode )!.Value;
            return circle;
        }

    }
}

