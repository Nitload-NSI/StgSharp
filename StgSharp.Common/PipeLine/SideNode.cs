//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="SideNode.cs"
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    internal sealed class BeginningNode : PipelineNode
    {

        public const string OutName = "default";

        private PipelineScheduler _bp;

        public BeginningNode( PipelineScheduler bp )
            : base(
            DefaultOperation, new PipelineNodeStringLabel( "BeginningNode" ), true, ["default"],
            [OutName] )
        {
            _bp = bp;
        }

        public PipelineNodeOutPort DefaultOut => OutputPorts[ OutName ];

        public override void Run()
        {
            _bp.RunningStat.Wait();
            PipelineNodeOutPort.SkipAll( _output );
        }

        internal void SetInputData( IEnumerable<(string, IPipeLineConnectionPayload)> data )
        {
            if( data is null ) {
                return;
            }
            foreach( (string, IPipeLineConnectionPayload) item in data ) {
                OutputPorts[ item.Item1 ].TransmitValue( item.Item2 );
            }
        }

    }

    internal sealed class EndingNode : PipelineNode
    {

        private PipelineScheduler _bp;

        public EndingNode( PipelineScheduler bp )
            : base(
            DefaultOperation, name: new PipelineNodeStringLabel( "EndingNode" ), true, ["default"],
            ["default"] )
        {
            _bp = bp;
        }

        public override void Run()
        {
            PipelineNodeInPort.WaitAll( _input );
            _bp.ResetIndex();
            _bp.RunningStat.Release();
        }

    }
}