//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepItemType.cs"
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

namespace StgSharp.Modeling.Step
{
    public enum StepItemType
    {

        AdvancedFace,
        AxisPlacement2D,
        AxisPlacement3D,
        BSplineCurveWithKnots,
        CartesianPoint,
        Circle,
        CylindricalSurface,
        Direction,
        EdgeCurve,
        EdgeLoop,
        Ellipse,
        FaceBound,
        FaceOuterBound,
        Line,
        OrientedEdge,
        Plane,
        Vector,
        VertexPoint

    }

    internal static class StepItemTypeExtensions
    {

        public const string AdvancedFaceText = "ADVANCED_FACE";
        public const string Axis2Placement2DText = "AXIS2_PLACEMENT_2D";
        public const string Axis2Placement3DText = "AXIS2_PLACEMENT_3D";
        public const string BSplineCurveWithKnotsText = "B_SPLINE_CURVE_WITH_KNOTS";
        public const string CartesianPointText = "CARTESIAN_POINT";
        public const string CircleText = "CIRCLE";
        public const string CylindricalSurfaceText = "CYLINDRICAL_SURFACE";
        public const string DirectionText = "DIRECTION";
        public const string EdgeCurveText = "EDGE_CURVE";
        public const string EdgeLoopText = "EDGE_LOOP";
        public const string EllipseText = "ELLIPSE";
        public const string FaceBoundText = "FACE_BOUND";
        public const string FaceOuterBoundText = "FACE_OUTER_BOUND";
        public const string LineText = "LINE";
        public const string OrientedEdgeText = "ORIENTED_EDGE";
        public const string PlaneText = "PLANE";
        public const string VectorText = "VECTOR";
        public const string VertexPointText = "VERTEX_POINT";

        public static string GetItemTypeString( this StepItemType type )
        {
            switch( type )
            {
                case StepItemType.AdvancedFace:
                    return AdvancedFaceText;
                case StepItemType.AxisPlacement2D:
                    return Axis2Placement2DText;
                case StepItemType.AxisPlacement3D:
                    return Axis2Placement3DText;
                case StepItemType.BSplineCurveWithKnots:
                    return BSplineCurveWithKnotsText;
                case StepItemType.CartesianPoint:
                    return CartesianPointText;
                case StepItemType.Circle:
                    return CircleText;
                case StepItemType.CylindricalSurface:
                    return CylindricalSurfaceText;
                case StepItemType.Direction:
                    return DirectionText;
                case StepItemType.EdgeCurve:
                    return EdgeCurveText;
                case StepItemType.EdgeLoop:
                    return EdgeLoopText;
                case StepItemType.Ellipse:
                    return EllipseText;
                case StepItemType.FaceBound:
                    return FaceBoundText;
                case StepItemType.FaceOuterBound:
                    return FaceOuterBoundText;
                case StepItemType.Line:
                    return LineText;
                case StepItemType.OrientedEdge:
                    return OrientedEdgeText;
                case StepItemType.Plane:
                    return PlaneText;
                case StepItemType.Vector:
                    return VectorText;
                case StepItemType.VertexPoint:
                    return VertexPointText;
                default:
                    throw new InvalidOperationException( $"Unexpected item type {type}" );
            }
        }

    }
}

