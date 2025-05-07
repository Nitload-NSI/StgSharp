//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IConvertableToPipelineNode.cs"
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
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.PipeLine
{
    public delegate void PipelineNodeOperation(
                         in Dictionary<string, PipelineNodeInPort> input,
                         in Dictionary<string, PipelineNodeOutPort> output );

    public interface IConvertableToPipelineNode
    {

        public IEnumerable<string> InputPortName { get; }

        public IEnumerable<string> OutputPortName { get; }

        PipelineNodeOperation Operation { get; }

        void NodeMain(
             in Dictionary<string, PipelineNodeInPort> input,
             in Dictionary<string, PipelineNodeOutPort> output );

    }

    internal class DefaultConvertableToBlueprintNode : IConvertableToPipelineNode
    {

        private string[] _input;
        private string[] _output;
        private PipelineNodeOperation _execution;

        public DefaultConvertableToBlueprintNode(
               PipelineNodeOperation execution,
               string[] input,
               string[] output )
        {
            _input = input;
            _output = output;
            _execution = execution;
        }

        public IEnumerable<string> InputPortName => _input;

        public IEnumerable<string> OutputPortName => _output;

        public PipelineNodeOperation Operation => _execution;

        public void NodeMain(
                    in Dictionary<string, PipelineNodeInPort> input,
                    in Dictionary<string, PipelineNodeOutPort> output )
        {
            _execution.Invoke( input, output );
        }

    }
}
