//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepRepresentationItem_FromTypedParameter.cs"
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

using System.Collections.Generic;
using System.Diagnostics;

namespace StgSharp.Modeling.Step
{
    public abstract partial class StepRepresentationItem
    {

        internal static HashSet<string> UnsupportedItemTypes { get; } = new HashSet<string>();

        internal static StepRepresentationItem FromTypedParameter(
                                               StepModel binder,
                                               ExpNode itemSyntax )
        {
            StepRepresentationItem item = null!;
            if( itemSyntax is ExpElementInitializingNode initNode )
            {
                string name = initNode.EqualityTypeConvert.Name;
                switch( name )
                {
                    case StepItemTypeExtensions.AdvancedFaceText:
                        item = StepAdvancedFace.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.Axis2Placement2DText:
                        item = StepAxis2Placement2D.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.Axis2Placement3DText:
                        item = StepAxis2Placement3D.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.BSplineCurveWithKnotsText:
                        item = StepBSplineCurveWithKnots.CreateFromSyntaxList(
                            binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.CartesianPointText:
                        item = StepCartesianPoint.CreateFromSyntaxList( initNode.Right );
                        break;
                    case StepItemTypeExtensions.CircleText:
                        item = StepCircle.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.CylindricalSurfaceText:
                        item = StepCylindricalSurface.CreateFromSyntaxList( binder,
                                                                            initNode.Right );
                        break;
                    case StepItemTypeExtensions.DirectionText:
                        item = StepDirection.CreateFromSyntaxList( initNode.Right );
                        break;
                    case StepItemTypeExtensions.EdgeCurveText:
                        item = StepEdgeCurve.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.EdgeLoopText:
                        item = StepEdgeLoop.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.EllipseText:
                        item = StepEllipse.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.FaceBoundText:
                        item = StepFaceBound.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.FaceOuterBoundText:
                        item = StepFaceOuterBound.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.LineText:
                        item = StepLine.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.OrientedEdgeText:
                        item = StepOrientedEdge.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.PlaneText:
                        item = StepPlane.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.VectorText:
                        item = StepVector.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    case StepItemTypeExtensions.VertexPointText:
                        item = StepVertexPoint.CreateFromSyntaxList( binder, initNode.Right );
                        break;
                    default:
                        if( UnsupportedItemTypes.Add( name ) ) {
                            Debug.WriteLine(
                                $"Unsupported item {name} at {initNode.Line}, {initNode.Column}" );
                        }
                        break;
                }
            } else
            {
                // TODO:
            }

            return item;
        }

    }
}

