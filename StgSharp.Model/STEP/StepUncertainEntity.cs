//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepUncertainEntity"
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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

using StgSharp;

using StgSharp.Model.Step;
using StgSharp.PipeLine;
using StgSharp.Script;

using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace StgSharp.Model.Step
{
    public partial class StepUninitializedEntity : StepEntityBase, IConvertableToPipelineNode
    {

        internal const string InName = "in", OutName = "out";

        private ExpSyntaxNode _data;

        internal StepUninitializedEntity(StepModel model, int id)
        {
            Context = model;
            Id = id;
        }

        internal StepUninitializedEntity(StepModel model, StepEntityLabel label)
        {
            Context = model;
            Label = label;
        }

        internal StepUninitializedEntity(
                 StepModel model,
                 int id,
                 ExpSyntaxNode initialization,
                 IEnumerable<int> dependency)
        {
            Context = model;
            Id = id;
            _data = initialization;
            if (dependency == null)
            {
                Dependencies = [];
            } else
            {
                Dependencies = dependency;
            }
        }

        public override StepItemType ItemType => StepItemType.Unknown;

        internal static HashSet<string> UnsupportedItemTypes { get; } = new HashSet<string>();

        internal StepEntityBase? ConvertToAccurate()
        {
            /*
            StringBuilder builder = new StringBuilder();
            if( Dependencies is null || _data is null )
            {
                builder.Append( "---------------------------" );
                builder.Append( "************************" );
                Console.WriteLine(
                    $"Building entity {Id} from thread {Environment.CurrentManagedThreadId} with dependency:{builder.ToString()} " );
            } else
            {
                foreach( int item in Dependencies ) {
                    builder.Append( $"{item}," );
                }
            }
            return null;
            /**/
            if (_data is ExpFunctionCallingNode initNode)
            {
                string name = initNode.FunctionCaller.Name;
                StepEntityBase? entity = ConvertSimpleEntity(Context, initNode);
                if (entity is null)
                {
                    UnsupportedItemTypes.Add(name);
                    return null;
                }
                return entity;
            } else if (_data is StepComplexEntityNode complex)
            {
                /**/
                ExpNodeTempEnumerator enumerator = new ExpNodeTempEnumerator(complex.Right, true);
                StepRepresentationItem entity = null!;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is not ExpFunctionCallingNode init) {
                        throw new InvalidCastException();
                    }
                    StepRepresentationItem? e = ConvertSimpleEntity(Context, init);
                    if (e is null)
                    {
                        // throw new NotImplementedException( $"Unknown entity type in sequence {Id}" );
                        return null;
                    }
                    if (entity is null)
                    {
                        entity = e;
                    } else
                    {
                        entity = StepDataParser.Implement(entity, e);
                    }
                }
                return entity;
                /**/
                return null;
            } else
            {
                throw new ExpInvalidSyntaxException("Name of entity is required.");
            }
        }

        internal void FromSequence(StepEntityDefineSequence sequence)
        {
            ExpSyntaxNode node = sequence.Expression;
            if (node is not ExpBinaryOperatorNode assign) {
                throw new ExpInvalidSyntaxException("Not an assign expression ");
            }
            StepEntityInstanceNode? idNode = assign.Left as StepEntityInstanceNode;
            if (idNode!.Id != this.Id) {
                throw new InvalidOperationException();
            }
            FromSequenceUnsafe(sequence);
        }

        internal void FromSequenceUnsafe(StepEntityDefineSequence sequence)
        {
            ExpSyntaxNode assign = (sequence.Expression as ExpBinaryOperatorNode)!;
            ExpSyntaxNode initNode;
            if ((assign.Right is ExpFunctionCallingNode f))
            {
                initNode = assign.Right;
                ExpEntityTypeName = f.FunctionCaller.Name;
            } else if ((assign.Right is StepComplexEntityNode c))
            {
                initNode = assign.Right;
                ExpNodeTempEnumerator enumerator = new ExpNodeTempEnumerator(c.Right, false);
                StringBuilder sb = new StringBuilder();
                sb.Append("Complex entity initialization: ");
                int count = 0;
                while (enumerator.MoveNext())
                {
                    if (count > 0) {
                        sb.Append(" <-> ");
                    }
                    f = (enumerator.Current as ExpFunctionCallingNode)!;
                    sb.Append($"{f.FunctionCaller.Name}");
                    count++;
                }
                ExpEntityTypeName = sb.ToString();
            } else
            {
                throw new ExpInvalidSyntaxException(
                    "Entity init or complex entity define is required.");
            }
            if (_data is not null) {
                return;
            }
            _data = initNode!;
            Dependencies = sequence.Dependencies ?? [];
        }

        void IConvertableToPipelineNode.NodeMain(
                                        in Dictionary<string, PipelineNodeInPort> input,
                                        in Dictionary<string, PipelineNodeOutPort> output)
        {
            try
            {
                StepEntityBase? entity = ConvertToAccurate();
                if (entity is not null)
                {
                    entity.FromInstance(this);
                    Context.ReplaceEntity(this, entity);
                }
            }
            catch (StepEntityUnsupportedException) { }
            catch (Exception ex)
            {
                throw new Exception($"Step decoder crashed when building entity {Id}", ex);
            }

            // Context.ReplaceEntity( this, entity );
            PipelineNodeOutPort.SkipAll(output);
        }

        IEnumerable<string> IConvertableToPipelineNode.InputPortName => [ InName ];

        PipelineNodeOperation IConvertableToPipelineNode.Operation => (this as IConvertableToPipelineNode).NodeMain;

        IEnumerable<string> IConvertableToPipelineNode.OutputPortName => [ OutName ];

    }
}