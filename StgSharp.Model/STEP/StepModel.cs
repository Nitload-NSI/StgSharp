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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Modeling.Step
{
    public class StepModel
    {

        private BlueprintScheduler _builder;
        private Dictionary<int, StepUncertainEntity> _nodes 
            = new Dictionary<int, StepUncertainEntity>();
        private StepInfo _header;

        internal StepModel( StepInfo header )
        {
            _header = header;
            _builder = new BlueprintScheduler();
        }

        public StepUncertainEntity this[ int id ]
        {
            get
            {
                if( _nodes.TryGetValue( id, out StepUncertainEntity? node ) ) {
                    return node;
                }
                node = StepUncertainEntity.Create( id );
                return node;
            }
        }

        public void AddUncertainEntity( int id, StepUncertainEntity entity )
        {
            if( _nodes.ContainsKey( id ) ) {
                throw new InvalidOperationException(
                    "Attempt to add entity with same id more than once" );
            }
            _nodes.Add( id, entity );
        }

        public void OrganizeNode()
        {
            foreach( KeyValuePair<int, StepUncertainEntity> node in _nodes ) { }
        }

    }
}
