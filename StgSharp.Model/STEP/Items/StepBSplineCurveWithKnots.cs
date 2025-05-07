//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepBSplineCurveWithKnots.cs"
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
using System.Linq;

namespace StgSharp.Model.Step
{
    public enum StepKnotType
    {

        UniformKnots,
        QuasiUniformKnots,
        PiecewiseBezierKnots,
        Unspecified

    }

;

    public class StepBSplineCurveWithKnots : StepBSplineCurve
    {

        private const string PIECEWISE_BEZIER_KNOTS = "PIECEWISE_BEZIER_KNOTS";
        private const string QUASI_UNIFORM_KNOTS = "QUASI_UNIFORM_KNOTS";

        private const string UNIFORM_KNOTS = "UNIFORM_KNOTS";
        private const string UNSPECIFIED = "UNSPECIFIED";

        public StepBSplineCurveWithKnots( IEnumerable<StepCartesianPoint> controlPoints )
            : base( controlPoints ) { }

        public StepBSplineCurveWithKnots( params StepCartesianPoint[] controlPoints )
            : base( controlPoints ) { }

        public List<int> KnotMultiplicities { get; } = new List<int>();

        public List<double> Knots { get; } = new List<double>();

        public override StepItemType ItemType => StepItemType.BSplineCurveWithKnots;

        public StepKnotType KnotSpec { get; set; } = StepKnotType.Unspecified;

        internal static StepBSplineCurveWithKnots FromSyntax(
                                                  StepModel binder,
                                                  ExpSyntaxNode syntaxList )
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator( syntaxList );
            enumerator.AssertEnumeratorCount( 9 );
            ExpNodePresidentEnumerator controlPointsList = enumerator[ 2 ].ToPresidentEnumerator();

            StepBSplineCurveWithKnots spline = new StepBSplineCurveWithKnots(
                new StepCartesianPoint[controlPointsList.Count] );
            spline.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            spline.Degree = ( enumerator[ 1 ] as ExpIntNode )!.Value;

            for( int i = 0; i < controlPointsList.Count; i++ )
            {
                int j = i; // capture to avoid rebinding
                binder.BindValue(
                    controlPointsList[ j ],
                    v => spline.ControlPointsList[ j ] = v.AsType<StepCartesianPoint>() );
            }

            spline.CurveForm = ParseCurveForm( enumerator[ 3 ].CodeConvertTemplate );
            spline.ClosedCurve = ( enumerator[ 4 ] as ExpBoolNode )!.Value;
            spline.SelfIntersect = ( enumerator[ 5 ]as ExpBoolNode )!.Value;

            ExpNodePresidentEnumerator knotMultiplicitiesList = enumerator[ 6 ].ToPresidentEnumerator(
                );
            spline.KnotMultiplicities.Clear();
            for( int i = 0; i < knotMultiplicitiesList.Count; i++ ) {
                spline.KnotMultiplicities.Add( ( knotMultiplicitiesList[ i ]as ExpIntNode )!.Value );
            }

            ExpNodePresidentEnumerator knotslist = enumerator[ 7 ].ToPresidentEnumerator();
            spline.Knots.Clear();
            for( int i = 0; i < knotslist.Count; i++ ) {
                spline.Knots.Add( ( knotslist[ i ]as ExpRealNumberNode )!.Value );
            }

            spline.KnotSpec = ParseKnotSpec( ( enumerator[ 8 ]as ExpStringNode )!.Value );

            return spline;
        }

        private static string GetKnotSpec( StepKnotType spec )
        {
            switch( spec )
            {
                case StepKnotType.UniformKnots:
                    return UNIFORM_KNOTS;
                case StepKnotType.QuasiUniformKnots:
                    return QUASI_UNIFORM_KNOTS;
                case StepKnotType.PiecewiseBezierKnots:
                    return PIECEWISE_BEZIER_KNOTS;
                case StepKnotType.Unspecified:
                    return UNSPECIFIED;
            }

            throw new NotImplementedException();
        }

        private static StepKnotType ParseKnotSpec( string enumerationValue )
        {
            switch( enumerationValue.ToUpperInvariant() )
            {
                case UNIFORM_KNOTS:
                    return StepKnotType.UniformKnots;
                case QUASI_UNIFORM_KNOTS:
                    return StepKnotType.QuasiUniformKnots;
                case PIECEWISE_BEZIER_KNOTS:
                    return StepKnotType.PiecewiseBezierKnots;
                default:
                    return StepKnotType.Unspecified;
            }
        }

    }
}

