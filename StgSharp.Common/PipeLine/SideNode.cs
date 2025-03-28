﻿//-----------------------------------------------------------------------
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    internal sealed class BeginningNode : PipeLineNode
    {

        private BlueprintScheduler _bp;

        public BeginningNode( BlueprintScheduler bp )
            : base(
            DefaultOperation, name: "BeginningNode", true, ["default"],
            ["default"] )
        {
            _bp = bp;
        }

        internal (string name, PipeLineConnectorArgs arg)[] InputData
        {
            set
            {
                if( value == null ) {
                    return;
                }
                foreach( (string name, PipeLineConnectorArgs arg) parameter in value ) {
                    OutputInterfaces[ parameter.name ].Args = parameter.arg;
                }
            }
        }

        public override void Run()
        {
            _bp.RunningStat.Wait();
            PipelineConnector.SkipAll( _outputInterfaces );
        }

        public new void SetCertainOutputPort(
                                string name,
                                PipelineConnector pipeline )
        {
            if( _outputInterfaces.ContainsKey( name ) ) {
                _outputInterfaces[ name ] = pipeline;
            }
            _outputInterfaces.Add( name, pipeline );
        }

    }

    internal sealed class EndingNode : PipeLineNode
    {

        private BlueprintScheduler _bp;

        public EndingNode( BlueprintScheduler bp )
            : base(
            DefaultOperation, name: "EndingNode", true, ["default"],
            ["default"] )
        {
            _bp = bp;
        }

        internal (string name, PipeLineConnectorArgs arg)[] OutputData
        {
            get { return InputInterfaces.Select(
                      key => (key.Key, key.Value.Args) )
                          .ToArray(); }
        }

        public override void Run()
        {
            PipelineConnector.WaitAll( _inputInterfaces );
            Interlocked.Exchange( ref _bp.currentGlobalNodeIndex, 0 );
            Interlocked.Exchange( ref _bp.currentNativeNodeIndex, 0 );
            _bp.RunningStat.Release();
        }

        public new void SetCertainInputPort(
                                string name,
                                PipelineConnector pipeline )
        {
            if( _inputInterfaces.ContainsKey( name ) ) {
                _inputInterfaces[ name ] = pipeline;
            }
            _inputInterfaces.Add( name, pipeline );
        }

    }
}