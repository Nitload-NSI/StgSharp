//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepUncertainElement.cs"
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
using StgSharp;
using StgSharp.PipeLine;
using StgSharp.Script;

using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace StgSharp.Modeling.Step
{
    public class StepUninitializedEntity : StepEntityBase, IConvertableToPipelineNode
    {

        internal const string InName = "in", OutName = "out";

        private ExpSyntaxNode data;

        public StepUninitializedEntity( StepModel model, int id )
        {
            Context = model;
            Id = id;
        }

        public override StepItemType ItemType => StepItemType.Unknown;

        internal static HashSet<string> UnsupportedItemTypes { get; } = new HashSet<string>();

        public static StepUninitializedEntity FromSyntax( StepModel model, ExpSyntaxNode node )
        {
            if( node is not ExpBinaryOperatorNode assign ) {
                throw new ExpInvalidSyntaxException( "Not an assign expression " );
            }
            StepEntityInstanceNode? idNode = assign.Left as StepEntityInstanceNode;
            ExpElementInitializingNode? initNode = assign.Right as ExpElementInitializingNode;
            StepUninitializedEntity entity = new StepUninitializedEntity( model, idNode.Id );
            entity.data = initNode;
            return entity;
        }

        public void Init( StepModel model, ExpSyntaxNode param )
        {
            if( param is ExpFunctionCallingNode caller )
            {
                string entityName = caller.FunctionCaller.Name;
                ExpSyntaxNode p = caller.Right;
                ExpNodeNextEnumerator e = p.ToEnumerator();
            } else
            {
                throw new ExpInvalidSyntaxException(
                    "Assigned syntax node is not a function caller." );
            }
        }

        internal void ConvertToAccurate()
        {
            if( data is ExpFunctionCallingNode initNode )
            {
                string name = initNode.FunctionCaller.Name;
                StepEntityBase? entity = name switch
                {
                    ExpressStepUtil.AdvancedFaceText => StepAdvancedFace.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.Axis2Placement2DText => StepAxis2Placement2D.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.Axis2Placement3DText => StepAxis2Placement3D.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.BSplineCurveWithKnotsText => StepBSplineCurveWithKnots.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.CartesianPointText => StepCartesianPoint.FromSyntax(
                        initNode.Right ),

                    ExpressStepUtil.CircleText => StepCircle.FromSyntax( Context, initNode.Right ),

                    ExpressStepUtil.CylindricalSurfaceText => StepCylindricalSurface.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.DirectionText => StepDirection.FromSyntax( initNode.Right ),

                    ExpressStepUtil.EdgeCurveText => StepEdgeCurve.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.EdgeLoopText => StepEdgeLoop.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.EllipseText => StepEllipse.FromSyntax( Context,
                                                                           initNode.Right ),

                    ExpressStepUtil.FaceBoundText => StepFaceBound.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.FaceOuterBoundText => StepFaceOuterBound.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.LineText => StepLine.FromSyntax( Context, initNode.Right ),

                    ExpressStepUtil.OrientedEdgeText => StepOrientedEdge.FromSyntax(
                        Context, initNode.Right ),

                    ExpressStepUtil.PlaneText => StepPlane.FromSyntax( Context, initNode.Right ),

                    ExpressStepUtil.VectorText => StepVector.FromSyntax( Context, initNode.Right ),

                    ExpressStepUtil.VertexPointText => StepVertexPoint.FromSyntax(
                        Context, initNode.Right ),

                    _ => null
                };
                if( entity is null )
                {
                    UnsupportedItemTypes.Add( name );
                    return;
                }
                Context.ReplaceEntity( this, entity );
            } else
            {
                throw new ExpInvalidSyntaxException( "Name of entity is required." );
            }
        }

        void IConvertableToPipelineNode.NodeMain(
                                        in Dictionary<string, PipelineNodeImport> input,
                                        in Dictionary<string, PipelineNodeExport> output )
        {
            PipelineNodeExport.SkipAll( output );
        }

        IEnumerable<string> IConvertableToPipelineNode.InputInterfacesName => [InName];

        PipelineNodeOperation IConvertableToPipelineNode.Operation => ( this as IConvertableToPipelineNode ).NodeMain;

        IEnumerable<string> IConvertableToPipelineNode.OutputInterfacesName => [OutName];

    }
}