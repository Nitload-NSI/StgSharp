// -----------------------------------------------------------------------------
// file="SideNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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

        public BeginningNode(
               PipelineScheduler bp
        )
            : base(DefaultOperation, new PipelineNodeStringLabel("BeginningNode"), true,
                   ["default"], [OutName])
        {
            _bp = bp;
        }

        public PipelineNodeOutPort DefaultOut => OutputPorts[OutName];

        public override void Run()
        {
            _bp.RunningStat.Wait();
            PipelineNodeOutPort.SkipAll(_output);
        }

        internal void SetInputData(
                      IEnumerable<(string, IPipeLineConnectionPayload)> data
        )
        {
            if (data is null) {
                return;
            }
            foreach ((string, IPipeLineConnectionPayload) item in data) {
                OutputPorts[item.Item1].TransmitValue(item.Item2);
            }
        }

    }

    internal sealed class EndingNode : PipelineNode
    {

        private PipelineScheduler _bp;

        public EndingNode(
               PipelineScheduler bp
        )
            : base(DefaultOperation, name:new PipelineNodeStringLabel("EndingNode"), true,
                   ["default"], ["default"])
        {
            _bp = bp;
        }

        public override void Run()
        {
            PipelineNodeInPort.WaitAll(_input);
            _bp.ResetIndex();
            _bp.RunningStat.Release();
        }

    }
}