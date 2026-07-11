// -----------------------------------------------------------------------------
// file="IConvertableToPipelineNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.PipeLine
{
    public delegate void PipelineNodeOperation(
                         in Dictionary<string, PipelineNodeInPort> input,
                         in Dictionary<string, PipelineNodeOutPort> output
    );

    public interface IConvertableToPipelineNode
    {

        public IEnumerable<string> InputPortName { get; }

        public IEnumerable<string> OutputPortName { get; }

        PipelineNodeOperation Operation { get; }

        void NodeMain(
             in Dictionary<string, PipelineNodeInPort> input,
             in Dictionary<string, PipelineNodeOutPort> output
        );

    }

    internal class DefaultConvertableToBlueprintNode : IConvertableToPipelineNode
    {

        private string[] _input;
        private string[] _output;
        private PipelineNodeOperation _execution;

        public DefaultConvertableToBlueprintNode(
               PipelineNodeOperation execution,
               string[] input,
               string[] output
        )
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
                    in Dictionary<string, PipelineNodeOutPort> output
        )
        {
            _execution.Invoke(input, output);
        }

    }
}
