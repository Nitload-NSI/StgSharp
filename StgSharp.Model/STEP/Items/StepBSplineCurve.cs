//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepBSplineCurve.cs"
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

using System;

using System.Collections.Generic;
using System.Linq;

namespace StgSharp.Modeling.Step
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

;

    public abstract class StepBSplineCurve : StepBoundedCurve
    {

        private const string CIRCULAR_ARC = "CIRCULAR_ARC";
        private const string ELLIPTIC_ARC = "ELLIPTIC_ARC";
        private const string HYPERBOLIC_ARC = "HYPERBOLIC_ARC";
        private const string PARABOLIC_ARC = "PARABOLIC_ARC";
        private const string POLYLINE_FORM = "POLYLINE_FORM";
        private const string UNSPECIFIED = "UNSPECIFIED";

        public StepBSplineCurve( string name, IEnumerable<StepCartesianPoint> controlPoints )
            : base( name )
        {
            ControlPointsList.AddRange( controlPoints );
        }

        public StepBSplineCurve( string name, params StepCartesianPoint[] controlPoints )
            : this( name, ( IEnumerable<StepCartesianPoint> )controlPoints ) { }

        public bool ClosedCurve { get; set; }

        public bool SelfIntersect { get; set; }

        public int Degree { get; set; }

        public List<StepCartesianPoint> ControlPointsList { get; } = new List<StepCartesianPoint>();

        public StepBSplineCurveForm CurveForm { get; set; } = StepBSplineCurveForm.Unspecified;

        internal override IEnumerable<StepRepresentationItem> GetReferencedItems()
        {
            return ControlPointsList;
        }

        protected static string GetCurveFormString( StepBSplineCurveForm form )
        {
            switch( form )
            {
                case StepBSplineCurveForm.Polyline:
                    return POLYLINE_FORM;
                case StepBSplineCurveForm.CircularArc:
                    return CIRCULAR_ARC;
                case StepBSplineCurveForm.EllipticalArc:
                    return ELLIPTIC_ARC;
                case StepBSplineCurveForm.ParabolicArc:
                    return PARABOLIC_ARC;
                case StepBSplineCurveForm.HyperbolicArc:
                    return HYPERBOLIC_ARC;
                case StepBSplineCurveForm.Unspecified:
                    return UNSPECIFIED;
            }

            throw new NotImplementedException();
        }

        protected static StepBSplineCurveForm ParseCurveForm( string enumerationValue )
        {
            switch( enumerationValue.ToUpperInvariant() )
            {
                case POLYLINE_FORM:
                    return StepBSplineCurveForm.Polyline;
                case CIRCULAR_ARC:
                    return StepBSplineCurveForm.CircularArc;
                case ELLIPTIC_ARC:
                    return StepBSplineCurveForm.EllipticalArc;
                case PARABOLIC_ARC:
                    return StepBSplineCurveForm.ParabolicArc;
                case HYPERBOLIC_ARC:
                    return StepBSplineCurveForm.HyperbolicArc;
                default:
                    return StepBSplineCurveForm.Unspecified;
            }
        }

    }
}

