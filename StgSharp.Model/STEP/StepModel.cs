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
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace StgSharp.Modeling.Step
{
    public class StepModel
    {

        private Dictionary<int, StepEntityBase> _nodes 
            = new Dictionary<int, StepEntityBase>();

        private PipelineScheduler _builder;
        private StepInfo _header;

        internal StepModel( StepInfo header )
        {
            _header = header;
            _builder = new PipelineScheduler();
        }

        public StepEntityBase this[ int id ]
        {
            get
            {
                if( _nodes.TryGetValue( id, out StepEntityBase? node ) ) {
                    return node;
                }
                node = new StepUninitializedEntity( this, id );
                _nodes.Add( id, node );
                return node;
            }
        }

        public void AddUncertainEntity( int id, StepUninitializedEntity entity )
        {
            if( _nodes.ContainsKey( id ) ) {
                throw new InvalidOperationException(
                    "Attempt to add entity with same id more than once" );
            }
            _nodes.Add( id, entity );
        }

        public void BindValue<T>( ExpSyntaxNode node, Action<T> action ) { }

        public void OrganizeNode()
        {
            PipelineScheduler ps = new PipelineScheduler();
            foreach( StepEntityBase node in _nodes.Values )
            {
                if( node is StepUninitializedEntity uncertain ) {
                    uncertain.ConvertToAccurate();
                }
            }
        }

        public void ReplaceEntity(
                    StepUninitializedEntity uninitialized,
                    StepEntityBase accurateEntity )
        {
            ArgumentNullException.ThrowIfNull( uninitialized );
            if( uninitialized. Context != this || accurateEntity.Context != this ) {
                throw new InvalidOperationException();
            }
            if( uninitialized.Id != accurateEntity.Id ) {
                throw new InvalidOperationException();
            }
            _nodes[ uninitialized.Id ] = accurateEntity;
        }

    }
}
