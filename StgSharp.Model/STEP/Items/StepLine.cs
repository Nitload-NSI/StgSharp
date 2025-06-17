//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepLine.cs"
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
using StgSharp.Math;
using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;

namespace StgSharp.Model.Step
{
    public class StepLine : StepCurve, IExpConvertableFrom<StepLine>
    {

        private StepCartesianPoint _point;
        private StepVector _vector;

        private StepLine( StepModel model ) : base( model ) { }

        public StepLine( StepModel model, StepCartesianPoint point, StepVector vector )
            : base( model )
        {
            Point = point;
            Vector = vector;
        }

        public StepCartesianPoint Point
        {
            get { return _point; }
            set
            {
                ArgumentNullException.ThrowIfNull( value );
                _point = value;
            }
        }

        public override StepItemType ItemType => StepItemType.Line;

        public StepVector Vector
        {
            get { return _vector; }
            set
            {
                ArgumentNullException.ThrowIfNull( value );
                _vector = value;
            }
        }

        public void FromInstance( StepLine entity )
        {
            base.FromInstance( entity );
            Point = entity.Point;
            Vector = entity.Vector;
        }

        public static StepLine FromPoints(
                               StepModel model,
                               float x1,
                               float y1,
                               float z1,
                               float x2,
                               float y2,
                               float z2 )
        {
            StepCartesianPoint start = new StepCartesianPoint( model, x1, y1, z1 );
            float dx = x2 - x1;
            float dy = y2 - y1;
            float dz = z2 - z1;
            float length = Scaler.Sqrt( dx * dx + dy * dy + dz * dz );
            float dxn = dx / length;
            float dyn = dy / length;
            float dzn = dz / length;
            StepVector vector = new StepVector(
                model, new StepDirection( model, dxn, dyn, dzn ), length );
            return new StepLine( model, start, vector );
        }

        internal static StepLine FromSyntax( StepModel binder, ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            StepLine line = new StepLine( binder );
            enumerator.AssertEnumeratorCount( 3 );
            line.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            line.Point = binder[ enumerator[ 1 ] ] as StepCartesianPoint;
            line.Vector = binder[ enumerator[ 2 ] ] as StepVector;
            return line;
        }

    }
}

