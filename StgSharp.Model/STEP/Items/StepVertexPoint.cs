//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepVertexPoint.cs"
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
    public class StepVertexPoint : StepVertex
    {

        private StepCartesianPoint _location;

        private StepVertexPoint() : base( string.Empty ) { }

        public StepVertexPoint( string name, StepCartesianPoint location )
            : base( name )
        {
            Location = location;
        }

        public StepCartesianPoint Location
        {
            get { return _location; }
            set
            {
                if( value == null ) {
                    throw new ArgumentNullException();
                }

                _location = value;
            }
        }

        public override StepItemType ItemType => StepItemType.VertexPoint;

        internal static StepVertexPoint CreateFromSyntaxList(
                                        StepBinder binder,
                                        ExpNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            StepVertexPoint vertex = new StepVertexPoint();
            enumerator.AssertEnumeratorCount( 2 );
            vertex.Name = enumerator.Values[ 0 ].CodeConvertTemplate;
            binder.BindValue(
                enumerator.Values[ 1 ], v => vertex.Location = v.AsType<StepCartesianPoint>() );
            return vertex;
        }

        internal override IEnumerable<StepRepresentationItem> GetReferencedItems()
        {
            yield return Location;
        }

    }
}

