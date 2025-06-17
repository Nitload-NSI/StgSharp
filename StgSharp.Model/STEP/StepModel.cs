//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepModel.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
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
using StgSharp.PipeLine;
using StgSharp.Script.Express;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Xml.Linq;

namespace StgSharp.Model.Step
{
    public class StepModel
    {

        private Dictionary<int, StepEntityBase> _nodes
            = new Dictionary<int, StepEntityBase>();

        private PipelineScheduler _builder;

        private PipelineScheduler _initScheduler = new PipelineScheduler();
        private StepInfo _header;

        internal StepModel( StepInfo header )
        {
            _header = header;
            _builder = new PipelineScheduler();
        }

        public StepEntityBase? this[ int id ]
        {
            get
            {
                if( id == 0 ) {
                    return null;
                }
                if( _nodes.TryGetValue( id, out StepEntityBase? node ) )
                {
                    if( node is StepUnsupportedEntity ) {
                        throw new StepEntityUnsupportedException();
                    }
                    return node;
                }
                node = new StepUninitializedEntity( this, id );
                _nodes.Add( id, node );
                return node;
            }
            private set { _nodes[ id ] = value!; }
        }

        internal StepEntityBase? this[ ExpSyntaxNode node ]
        {
            get
            {
                if( node is not StepEntityInstanceNode instance ) {
                    throw new InvalidCastException();
                }
                return this[ instance.Id ];
            }
            set
            {
                if( node is not StepEntityInstanceNode instance ) {
                    throw new InvalidCastException();
                }
                this[ instance.Id ] = value;
            }
        }

        public void BindValue( ExpSyntaxNode node, Action<StepEntityBase> action ) { }

        public StepUninitializedEntity FromInitSequence( StepEntityDefineSequence sequence )
        {
            int id = ( sequence.Expression.Left as StepEntityInstanceNode )!.Id;
            PipelineNode node;
            if( _nodes.TryGetValue( id, out StepEntityBase? e ) && e is StepUninitializedEntity u )
            {
                u.FromSequence( sequence );
                node = _initScheduler.GetNode( u.Label );
            } else
            {
                u = new StepUninitializedEntity( this, id );
                u.FromSequenceUnsafe( sequence );
                _nodes.Add( id, u );
                node = _initScheduler.Create( u.Label, u );
            }

            PipelineNodeInPort port = node.GetInputPort( StepUninitializedEntity.InName );
            if( sequence.Dependencies.Count() == 0 )
            {
                _initScheduler.LinkToInput( port );
            } else
            {
                foreach( int i in sequence.Dependencies )
                {
                    PipelineNode dNode;
                    StepEntityLabel label = new StepEntityLabel( i );
                    if( _nodes.TryGetValue( i, out e ) )
                    {
                        dNode = _initScheduler.GetNode( label );
                    } else
                    {
                        u = new StepUninitializedEntity( this, i );
                        _nodes.Add( i, u );
                        dNode = _initScheduler.Create( u.Label, u );
                    }
                    port.Connect( dNode.GetOutputPort( StepUninitializedEntity.OutName ) );
                }
            }
            return u;
        }

        public void InitEntity()
        {
            if( _initScheduler is null ) {
                return;
            }
            _initScheduler.ArrangeNodes();
            PipeLineRunner.Run( _initScheduler );
            _initScheduler = null!;
        }

        public void ReplaceEntity(
                    StepUninitializedEntity uninitialized,
                    StepEntityBase accurateEntity )
        {
            ArgumentNullException.ThrowIfNull( uninitialized );
            if( uninitialized.Context != this || accurateEntity.Context != this ) {
                throw new InvalidOperationException();
            }
            if( uninitialized.Id != accurateEntity.Id ) {
                throw new InvalidOperationException();
            }
            _nodes[ uninitialized.Id ] = accurateEntity;
        }

        public string[] ShowStructure( int depth )
        {
            _initScheduler.ArrangeNodes();
            IEnumerable<PipelineNode> nodes = PipelineMarshal.GetNodeSortedEnumeration(
                _initScheduler );
            int count = 0;
            List<string> info = [
                @$"Step model root",
                ];
            foreach( PipelineNode node in nodes )
            {
                count++;
                StepUninitializedEntity uncertain = ( node.NodeOperation as StepUninitializedEntity )!;
                string[] strings = uncertain.GetEntityDescription( depth - 1 );
                if( count < nodes.Count() )
                {
                    info.Add( $"  ├-{strings[0]}" );
                    for( int i = 1; i < strings.Length; i++ ) {
                        info.Add( $"  | {strings[i]}" );
                    }
                } else
                {
                    info.Add( $"  └-{strings[0]}" );
                    for( int i = 1; i < strings.Length; i++ ) {
                        info.Add( $"    {strings[i]}" );
                    }
                }
            }
            return info.ToArray();
        }

    }
}
