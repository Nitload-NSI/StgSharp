// -----------------------------------------------------------------------------
// file="CrossFramePipelineNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.PipeLine;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public class CrossFrameOperation : IConvertableToPipelineNode
    {

        private static string[] _inputName = ["TaskIn"];
        private static string[] _outputName = ["TaskOut"];
        private Action _mainExecution;
        private int currentCount;
        private readonly int cycleCount;

        public CrossFrameOperation(
               Action startup,
               int cycleCount
        )
        {
            MainExecution = startup;
            this.cycleCount = (cycleCount > 1) ?
                              cycleCount :
                              throw new ArgumentException(
                        $"{$"The {typeof(CrossFrameOperation).Name} crosses only one cycle, "}{$"please use a {typeof(PipelineNode).Name} instead."}");
            this.cycleCount = cycleCount;
            currentCount = cycleCount;
        }

        public Action MainExecution
        {
            get => _mainExecution;
            private protected set => _mainExecution = value;
        }

        public IEnumerable<string> InputPortName => _inputName;

        public IEnumerable<string> OutputPortName => _outputName;

        public PipelineNodeOperation Operation => NodeMain;

        public void NodeMain(
                    in Dictionary<string, PipelineNodeInPort> input,
                    in Dictionary<string, PipelineNodeOutPort> output
        )
        {
            if (currentCount < cycleCount)
            {
                Interlocked.Increment(ref currentCount);
            } else
            {
                Interlocked.Exchange(ref currentCount, 0);
                Task.Run(MainExecution);
            }

            // Console.WriteLine(currentCount);
            PipelineNodeOutPort.SkipAll(output);
        }

    }
}
