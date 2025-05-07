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

using StgSharp.Model.Step;
using StgSharp.PipeLine;
using StgSharp.Script;

using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace StgSharp.Model.Step
{
    public class StepUninitializedEntity : StepEntityBase, IConvertableToPipelineNode
    {

        internal const string InName = "in", OutName = "out";

        private ExpSyntaxNode _data;
        private IEnumerable<int> _dependency;

        internal StepUninitializedEntity( StepModel model, int id )
        {
            Context = model;
            Id = id;
        }

        internal StepUninitializedEntity( StepModel model, StepEntityLabel label )
        {
            Context = model;
            Label = label;
        }

        internal StepUninitializedEntity(
                 StepModel model,
                 int id,
                 ExpSyntaxNode initialization,
                 IEnumerable<int> dependency )
        {
            Context = model;
            Id = id;
            _data = initialization;
            if( dependency == null )
            {
                _dependency = [];
            } else
            {
                _dependency = dependency;
            }
        }

        public IEnumerable<int> Dependencies => _dependency;

        public override StepItemType ItemType => StepItemType.Unknown;

        internal static HashSet<string> UnsupportedItemTypes { get; } = new HashSet<string>();

        internal static StepEntityBase? ConvertSimpleEntity(
                                        StepModel context,
                                        ExpFunctionCallingNode calling )
        {
            string name = calling.FunctionCaller.Name;
            return name switch
            {
                ExpressStepUtil.AdvancedFaceText => StepAdvancedFace.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.Axis2Placement2DText => StepAxis2Placement2D.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.Axis2Placement3DText => StepAxis2Placement3D.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.BSplineCurveWithKnotsText => StepBSplineCurveWithKnots.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.CartesianPointText => StepCartesianPoint.FromSyntax( calling.Right ),
                ExpressStepUtil.CircleText => StepCircle.FromSyntax( context, calling.Right ),
                ExpressStepUtil.CylindricalSurfaceText => StepCylindricalSurface.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.DirectionText => StepDirection.FromSyntax( calling.Right ),
                ExpressStepUtil.EdgeCurveText => StepEdgeCurve.FromSyntax( context, calling.Right ),
                ExpressStepUtil.EdgeLoopText => StepEdgeLoop.FromSyntax( context, calling.Right ),
                ExpressStepUtil.EllipseText => StepEllipse.FromSyntax( context, calling.Right ),
                ExpressStepUtil.FaceBoundText => StepFaceBound.FromSyntax( context, calling.Right ),
                ExpressStepUtil.FaceOuterBoundText => StepFaceOuterBound.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.LineText => StepLine.FromSyntax( context, calling.Right ),
                ExpressStepUtil.OrientedEdgeText => StepOrientedEdge.FromSyntax(
                    context, calling.Right ),
                ExpressStepUtil.PlaneText => StepPlane.FromSyntax( context, calling.Right ),
                ExpressStepUtil.VectorText => StepVector.FromSyntax( context, calling.Right ),
                ExpressStepUtil.VertexPointText => StepVertexPoint.FromSyntax(
                    context, calling.Right ),
                _ => null
            };
        }

        internal StepEntityBase? ConvertToAccurate()
        {
            /*
            StringBuilder builder = new StringBuilder();
            if( _dependency is null )
            {
                builder.Append( "---------------------------" );
                if( _data is null ) {
                    builder.Append( "************************" );
                }
            } else
            {
                foreach( int item in Dependencies ) {
                    builder.Append( $"{item}," );
                }
            }
            Console.WriteLine(
                $"Building entity {Id} from thread {Environment.CurrentManagedThreadId} with dependency:{builder.ToString()} " );
            return null;
            /**/
            if( _data is ExpFunctionCallingNode initNode )
            {
                string name = initNode.FunctionCaller.Name;
                StepEntityBase? entity = ConvertSimpleEntity( Context, initNode );
                if( entity is null )
                {
                    UnsupportedItemTypes.Add( name );
                    return null;
                }
                return entity;
            } else if( _data is StepComplexEntityNode complex )
            {
                ExpNodeTempEnumerator enumerator = new ExpNodeTempEnumerator( complex.Right );
                StepEntityBase entity = null!;
                while( enumerator.MoveNext() )
                {
                    if( enumerator.Current is not ExpFunctionCallingNode init ) {
                        throw new InvalidCastException();
                    }
                    StepEntityBase? e = ConvertSimpleEntity( Context, init );
                    if( e is null )
                    {
                        //throw new NotImplementedException( $"Unknown entity type in sequence {Id}" );
                        return null;
                    }
                    if( entity is null )
                    {
                        entity = e;
                    } else
                    {
                        Type current = entity.GetType(), newType = e.GetType();
                        if( newType.IsAssignableFrom( current ) )
                        {
                            e.ImplementFrom( entity );
                            entity = e;
                        } else if( current.IsAssignableFrom( newType ) )
                        {
                            entity.ImplementFrom( e );
                        } else
                        {
                            throw new NotImplementedException(
                                $"Not supported dependency relationship between {current.Name} ans {newType.Name}" );
                        }
                    }
                }
                return entity;
            } else
            {
                throw new ExpInvalidSyntaxException( "Name of entity is required." );
            }
        }

        internal void FromSequence( StepEntityDefineSequence sequence )
        {
            ExpSyntaxNode node = sequence.Expression;
            if( node is not ExpBinaryOperatorNode assign ) {
                throw new ExpInvalidSyntaxException( "Not an assign expression " );
            }
            StepEntityInstanceNode? idNode = assign.Left as StepEntityInstanceNode;
            ExpFunctionCallingNode initNode = ( assign.Right as ExpFunctionCallingNode )!;
            if( idNode!.Id != this.Id ) {
                throw new InvalidOperationException();
            }
            if( _data is not null ) {
                return;
            }
            _data = initNode;
            _dependency = sequence.Dependencies ?? [];
        }

        internal void FromSequenceUnsafe( StepEntityDefineSequence sequence )
        {
            ExpSyntaxNode assign = ( sequence.Expression as ExpBinaryOperatorNode )!;
            ExpFunctionCallingNode initNode = ( assign.Right as ExpFunctionCallingNode )!;
            if( _data is not null ) {
                return;
            }
            _data = initNode!;
            _dependency = sequence.Dependencies ?? [];
        }

        void IConvertableToPipelineNode.NodeMain(
                                        in Dictionary<string, PipelineNodeInPort> input,
                                        in Dictionary<string, PipelineNodeOutPort> output )
        {
            StepEntityBase? entity = ConvertToAccurate();
            if( entity is null )
            {
                //throw new InvalidOperationException();
            }

            //Context.ReplaceEntity( this, entity );
            PipelineNodeOutPort.SkipAll( output );
        }

        IEnumerable<string> IConvertableToPipelineNode.InputPortName => [InName];

        PipelineNodeOperation IConvertableToPipelineNode.Operation => ( this as IConvertableToPipelineNode ).NodeMain;

        IEnumerable<string> IConvertableToPipelineNode.OutputPortName => [OutName];

    }
}