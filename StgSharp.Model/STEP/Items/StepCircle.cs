//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepCircle.cs"
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
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;

namespace StgSharp.Modeling.Items
{
    public class StepCircle : StepConic
    {

        private StepAxis2Placement _position;

        private StepCircle() : base( string.Empty ) { }

        public StepCircle( string label, StepAxis2Placement position, float radius )
            : base( label )
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

        internal static StepCircle CreateFromSyntaxList( StepBinder binder, ExpNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            StepCircle circle = new StepCircle();
            enumerator.AssertEnumeratorCount( 3 );
            circle.Name = enumerator.Values[ 0 ].CodeConvertTemplate;
            binder.BindValue(
                enumerator.Values[ 1 ], v => circle.Position = v.AsType<StepAxis2Placement>() );
            circle.Radius = ( enumerator.Values[ 2 ]as ExpRealNumberNode )!.Value;
            return circle;
        }

        internal override IEnumerable<StepRepresentationItem> GetReferencedItems()
        {
            yield return Position;
        }

    }
}

