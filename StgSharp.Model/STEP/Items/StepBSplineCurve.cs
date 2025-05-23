//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepBSplineCurve.cs"
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

using System;

using System.Collections.Generic;
using System.Linq;

namespace StgSharp.Model.Step
{
    public enum StepBSplineCurveForm
    {

        Polyline,
        CircularArc,
        EllipticalArc,
        ParabolicArc,
        HyperbolicArc,
        Unspecified

    }

    public static class StepExpEnumValue
    {

        internal const string CIRCULAR_ARC = "CIRCULAR_ARC";
        internal const string ELLIPTIC_ARC = "ELLIPTIC_ARC";
        internal const string HYPERBOLIC_ARC = "HYPERBOLIC_ARC";
        internal const string PARABOLIC_ARC = "PARABOLIC_ARC";
        internal const string POLYLINE_FORM = "POLYLINE_FORM";
        internal const string UNSPECIFIED = "UNSPECIFIED";

        internal static HashSet<string> AllPossibleValue { get; } = [
                CIRCULAR_ARC,
                ELLIPTIC_ARC,
                HYPERBOLIC_ARC,
                PARABOLIC_ARC,
                POLYLINE_FORM,
                UNSPECIFIED,
            ];

    }

    public abstract class StepBSplineCurve : StepBoundedCurve
    {

        public StepBSplineCurve( IEnumerable<StepCartesianPoint> controlPoints )
        {
            ControlPointsList.AddRange( controlPoints );
        }

        public StepBSplineCurve( params StepCartesianPoint[] controlPoints )
            : this( ( IEnumerable<StepCartesianPoint> )controlPoints ) { }

        public bool ClosedCurve { get; set; }

        public bool SelfIntersect { get; set; }

        public int Degree { get; set; }

        public List<StepCartesianPoint> ControlPointsList { get; } = new List<StepCartesianPoint>();

        public StepBSplineCurveForm CurveForm { get; set; } = StepBSplineCurveForm.Unspecified;

        protected static string GetCurveFormString( StepBSplineCurveForm form )
        {
            switch( form )
            {
                case StepBSplineCurveForm.Polyline:
                    return StepExpEnumValue.POLYLINE_FORM;
                case StepBSplineCurveForm.CircularArc:
                    return StepExpEnumValue.CIRCULAR_ARC;
                case StepBSplineCurveForm.EllipticalArc:
                    return StepExpEnumValue.ELLIPTIC_ARC;
                case StepBSplineCurveForm.ParabolicArc:
                    return StepExpEnumValue.PARABOLIC_ARC;
                case StepBSplineCurveForm.HyperbolicArc:
                    return StepExpEnumValue.HYPERBOLIC_ARC;
                case StepBSplineCurveForm.Unspecified:
                    return StepExpEnumValue.UNSPECIFIED;
            }

            throw new NotImplementedException();
        }

        protected static StepBSplineCurveForm ParseCurveForm( string enumerationValue )
        {
            switch( enumerationValue.ToUpperInvariant() )
            {
                case StepExpEnumValue.POLYLINE_FORM:
                    return StepBSplineCurveForm.Polyline;
                case StepExpEnumValue.CIRCULAR_ARC:
                    return StepBSplineCurveForm.CircularArc;
                case StepExpEnumValue.ELLIPTIC_ARC:
                    return StepBSplineCurveForm.EllipticalArc;
                case StepExpEnumValue.PARABOLIC_ARC:
                    return StepBSplineCurveForm.ParabolicArc;
                case StepExpEnumValue.HYPERBOLIC_ARC:
                    return StepBSplineCurveForm.HyperbolicArc;
                default:
                    return StepBSplineCurveForm.Unspecified;
            }
        }

    }
}

