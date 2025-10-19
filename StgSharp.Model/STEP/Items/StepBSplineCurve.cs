//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepBSplineCurve"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

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

        internal static HashSet<string> AllPossibleValue { get; } = [ CIRCULAR_ARC, ELLIPTIC_ARC, HYPERBOLIC_ARC, PARABOLIC_ARC, POLYLINE_FORM, UNSPECIFIED, ];

    }

    public class StepBSplineCurve : StepBoundedCurve, IExpConvertableFrom<StepBSplineCurve>
    {

        public StepBSplineCurve(StepModel model, params StepCartesianPoint[] controlPoints)
            : this(model, (IEnumerable<StepCartesianPoint>)controlPoints) { }

        public StepBSplineCurve(StepModel model, IEnumerable<StepCartesianPoint> controlPoints)
            : base(model)
        {
            ControlPointsList.AddRange(controlPoints);
        }

        public bool ClosedCurve { get; set; }

        public bool SelfIntersect { get; set; }

        public int Degree { get; set; }

        public List<StepCartesianPoint> ControlPointsList { get; private set; } 
            = new List<StepCartesianPoint>();

        public StepBSplineCurveForm CurveForm { get; set; } = StepBSplineCurveForm.Unspecified;

        public void FromInstance(StepBSplineCurve entity)
        {
            base.FromInstance(entity);
            this.Degree = entity.Degree;
            this.ClosedCurve = entity.ClosedCurve;
            this.ControlPointsList = entity.ControlPointsList;
            this.SelfIntersect = entity.SelfIntersect;
        }

        public override bool IsConvertableTo(string entityName)
        {
            return base.IsConvertableTo(entityName) || entityName == "StepBSplineCurve";
        }

        protected static string GetCurveFormString(StepBSplineCurveForm form)
        {
            return form switch
            {
                StepBSplineCurveForm.Polyline => StepExpEnumValue.POLYLINE_FORM,
                StepBSplineCurveForm.CircularArc => StepExpEnumValue.CIRCULAR_ARC,
                StepBSplineCurveForm.EllipticalArc => StepExpEnumValue.ELLIPTIC_ARC,
                StepBSplineCurveForm.ParabolicArc => StepExpEnumValue.PARABOLIC_ARC,
                StepBSplineCurveForm.HyperbolicArc => StepExpEnumValue.HYPERBOLIC_ARC,
                StepBSplineCurveForm.Unspecified => StepExpEnumValue.UNSPECIFIED,
                _ => throw new NotImplementedException(),
            };
        }

        protected static StepBSplineCurveForm ParseCurveForm(string enumerationValue)
        {
            return enumerationValue.ToUpperInvariant() switch
            {
                StepExpEnumValue.POLYLINE_FORM => StepBSplineCurveForm.Polyline,
                StepExpEnumValue.CIRCULAR_ARC => StepBSplineCurveForm.CircularArc,
                StepExpEnumValue.ELLIPTIC_ARC => StepBSplineCurveForm.EllipticalArc,
                StepExpEnumValue.PARABOLIC_ARC => StepBSplineCurveForm.ParabolicArc,
                StepExpEnumValue.HYPERBOLIC_ARC => StepBSplineCurveForm.HyperbolicArc,
                _ => StepBSplineCurveForm.Unspecified,
            };
        }

    }
}

