//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepUncertainEntity.Parse"
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
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    public partial class StepUninitializedEntity
    {

        private static StepRepresentationItem ConvertSimpleEntity(StepModel context, ExpFunctionCallingNode calling)
        {
            string name = calling.FunctionCaller.Name;
            return name switch
            {
                ExpressStepUtil.AdvancedFaceText => StepAdvancedFace.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.Axis2Placement2DText => StepAxis2Placement2D.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.Axis2Placement3DText => StepAxis2Placement3D.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.BSplineCurveWithKnotsText => StepBSplineCurveWithKnots.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.CartesianPointText => StepCartesianPoint.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.CircleText => StepCircle.FromSyntax(context, calling.Right),
                ExpressStepUtil.CylindricalSurfaceText => StepCylindricalSurface.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.DirectionText => StepDirection.FromSyntax(context, calling.Right),
                ExpressStepUtil.EdgeCurveText => StepEdgeCurve.FromSyntax(context, calling.Right),
                ExpressStepUtil.EdgeLoopText => StepEdgeLoop.FromSyntax(context, calling.Right),
                ExpressStepUtil.EllipseText => StepEllipse.FromSyntax(context, calling.Right),
                ExpressStepUtil.FaceBoundText => StepFaceBound.FromSyntax(context, calling.Right),
                ExpressStepUtil.FaceOuterBoundText => StepFaceOuterBound.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.LineText => StepLine.FromSyntax(context, calling.Right),
                ExpressStepUtil.OrientedEdgeText => StepOrientedEdge.FromSyntax(
                    context, calling.Right),
                ExpressStepUtil.PlaneText => StepPlane.FromSyntax(context, calling.Right),
                ExpressStepUtil.VectorText => StepVector.FromSyntax(context, calling.Right),
                ExpressStepUtil.VertexPointText => StepVertexPoint.FromSyntax(
                    context, calling.Right),
                _ => throw new StepEntityUnsupportedException()
            };
        }

    }
}
