//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepBSplineCurveWithKnots"
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
    public enum StepKnotType
    {

        UniformKnots,
        QuasiUniformKnots,
        PiecewiseBezierKnots,
        Unspecified

    }

;

    public class StepBSplineCurveWithKnots : StepBSplineCurve, IExpConvertableFrom<StepBSplineCurveWithKnots>
    {

        private const string PIECEWISE_BEZIER_KNOTS = "PIECEWISE_BEZIER_KNOTS";
        private const string QUASI_UNIFORM_KNOTS = "QUASI_UNIFORM_KNOTS";

        private const string UNIFORM_KNOTS = "UNIFORM_KNOTS";
        private const string UNSPECIFIED = "UNSPECIFIED";

        public StepBSplineCurveWithKnots(StepModel model, IEnumerable<StepCartesianPoint> controlPoints)
            : base(model, controlPoints) { }

        public StepBSplineCurveWithKnots(StepModel model, params StepCartesianPoint[] controlPoints)
            : base(model, controlPoints) { }

        public List<int> KnotMultiplicities { get; } = new List<int>();

        public List<double> Knots { get; } = new List<double>();

        public override StepItemType ItemType => StepItemType.BSplineCurveWithKnots;

        public StepKnotType KnotSpec { get; set; } = StepKnotType.Unspecified;

        public void FromInstance(StepBSplineCurveWithKnots entity)
        {
            base.FromInstance(entity);
            KnotSpec = entity.KnotSpec;
            KnotMultiplicities.Clear();
            KnotMultiplicities.AddRange(entity.KnotMultiplicities);
            Knots.Clear();
            Knots.AddRange(entity.Knots);
        }

        internal static StepBSplineCurveWithKnots FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = syntaxList .ToPresidentEnumerator();
            enumerator.AssertEnumeratorCount(9);
            ExpNodePresidentEnumerator controlPointsList = enumerator[2].TupleToPresidentEnumerator(
                );

            StepBSplineCurveWithKnots spline = new StepBSplineCurveWithKnots(
                binder, new StepCartesianPoint[controlPointsList.Count]);
            spline.Name = (enumerator[0]as ExpStringNode)!.Value;
            spline.Degree = (enumerator[1] as ExpIntNode)!.Value;

            for (int i = 0; i < controlPointsList.Count; i++)
            {
                int j = i; // capture to avoid rebinding
                spline.ControlPointsList[j] = binder[controlPointsList[j]]as StepCartesianPoint;
            }

            spline.CurveForm = ParseCurveForm(enumerator[3].CodeConvertTemplate);
            spline.ClosedCurve = (enumerator[4] as ExpBoolNode)!.Value;
            spline.SelfIntersect = (enumerator[5]as ExpBoolNode)!.Value;

            ExpNodePresidentEnumerator knotMultiplicitiesList = enumerator[6].TupleToPresidentEnumerator(
                );
            spline.KnotMultiplicities.Clear();
            for (int i = 0; i < knotMultiplicitiesList.Count; i++) {
                spline.KnotMultiplicities.Add((knotMultiplicitiesList[i]as ExpIntNode)!.Value);
            }

            ExpNodePresidentEnumerator knotsList = enumerator[7].TupleToPresidentEnumerator();
            spline.Knots.Clear();
            for (int i = 0; i < knotsList.Count; i++) {
                spline.Knots.Add((knotsList[i]as ExpRealNumberNode)!.Value);
            }

            spline.KnotSpec = ParseKnotSpec(enumerator[8].CodeConvertTemplate);

            return spline;
        }

        private static string GetKnotSpec(StepKnotType spec)
        {
            return spec switch
            {
                StepKnotType.UniformKnots => UNIFORM_KNOTS,
                StepKnotType.QuasiUniformKnots => QUASI_UNIFORM_KNOTS,
                StepKnotType.PiecewiseBezierKnots => PIECEWISE_BEZIER_KNOTS,
                StepKnotType.Unspecified => UNSPECIFIED,
                _ => throw new NotImplementedException(),
            };
        }

        private static StepKnotType ParseKnotSpec(string enumerationValue)
        {
            return enumerationValue.ToUpperInvariant() switch
            {
                UNIFORM_KNOTS => StepKnotType.UniformKnots,
                QUASI_UNIFORM_KNOTS => StepKnotType.QuasiUniformKnots,
                PIECEWISE_BEZIER_KNOTS => StepKnotType.PiecewiseBezierKnots,
                _ => StepKnotType.Unspecified,
            };
        }

    }
}

