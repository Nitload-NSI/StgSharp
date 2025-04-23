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
using StgSharp.Modeling.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;

using System.Collections.Generic;
using System.Linq;

namespace StgSharp.Modeling.Step
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

        public StepBSplineCurveWithKnots(
               string name,
               IEnumerable<StepCartesianPoint> controlPoints )
            : base( name, controlPoints ) { }

        public StepBSplineCurveWithKnots( string name, params StepCartesianPoint[] controlPoints )
            : base( name, controlPoints ) { }

        public List<int> KnotMultiplicities { get; } = new List<int>();

        public List<double> Knots { get; } = new List<double>();

        public override StepItemType ItemType => StepItemType.BSplineCurveWithKnots;

        public StepKnotType KnotSpec { get; set; } = StepKnotType.Unspecified;

        internal static StepBSplineCurveWithKnots CreateFromSyntaxList(
                                                  StepModel binder,
                                                  ExpSyntaxNode syntaxList )
        {
            ExpNodeNextEnumerator enumerator = new ExpNodeNextEnumerator( syntaxList );
            enumerator.AssertEnumeratorCount( 9 );
            ExpNodeNextEnumerator controlPointsList = enumerator.Values[ 2 ].ToEnumerator();

            StepBSplineCurveWithKnots spline = new StepBSplineCurveWithKnots(
                string.Empty, new StepCartesianPoint[controlPointsList.Values.Count] );
            spline.Name = ( enumerator.Values[ 0 ]as ExpStringNode )!.Value;
            spline.Degree = ( enumerator.Values[ 1 ] as ExpIntNode )!.Value;

            for( int i = 0; i < controlPointsList.Values.Count; i++ )
            {
                int j = i; // capture to avoid rebinding
                binder.BindValue(
                    controlPointsList.Values[ j ],
                    v => spline.ControlPointsList[ j ] = v.AsType<StepCartesianPoint>() );
            }

            spline.CurveForm = ParseCurveForm( enumerator.Values[ 3 ].CodeConvertTemplate );
            spline.ClosedCurve = ( enumerator.Values[ 4 ] as ExpBoolNode )!.Value;
            spline.SelfIntersect = ( enumerator.Values[ 5 ]as ExpBoolNode )!.Value;

            ExpNodeNextEnumerator knotMultiplicitiesList = enumerator.Values[ 6 ].ToEnumerator();
            spline.KnotMultiplicities.Clear();
            for( int i = 0; i < knotMultiplicitiesList.Values.Count; i++ ) {
                spline.KnotMultiplicities
                      .Add( ( knotMultiplicitiesList.Values[ i ]as ExpIntNode )!.Value );
            }

            ExpNodeNextEnumerator knotslist = enumerator.Values[ 7 ].ToEnumerator();
            spline.Knots.Clear();
            for( int i = 0; i < knotslist.Values.Count; i++ ) {
                spline.Knots.Add( ( knotslist.Values[ i ]as ExpRealNumberNode )!.Value );
            }

            spline.KnotSpec = ParseKnotSpec( ( enumerator.Values[ 8 ]as ExpStringNode )!.Value );

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

