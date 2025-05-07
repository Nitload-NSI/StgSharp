//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepAxis2Placement3D.cs"
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
    public class StepAxis2Placement3D : StepAxis2Placement
    {

        private StepDirection _axis;

        private StepAxis2Placement3D() { }

        public StepAxis2Placement3D(
               StepCartesianPoint location,
               StepDirection axis,
               StepDirection refDirection )
        {
            Location = location;
            Axis = axis;
            RefDirection = refDirection;
        }

        public StepDirection Axis
        {
            get { return _axis; }
            set
            {
                if( value == null ) {
                    throw new ArgumentNullException();
                }

                _axis = value;
            }
        }

        public override StepItemType ItemType => StepItemType.AxisPlacement3D;

        internal static StepAxis2Placement3D FromSyntax(
                                             StepModel binder,
                                             ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            StepAxis2Placement3D axis = new StepAxis2Placement3D();
            enumerator.AssertEnumeratorCount( 4 );
            axis.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            binder.BindValue( enumerator[ 1 ],
                              v => axis.Location = v.AsType<StepCartesianPoint>() );
            binder.BindValue( enumerator[ 2 ], v => axis.Axis = v.AsType<StepDirection>() );
            binder.BindValue( enumerator[ 3 ], v => axis.RefDirection = v.AsType<StepDirection>() );
            return axis;
        }

    }
}

