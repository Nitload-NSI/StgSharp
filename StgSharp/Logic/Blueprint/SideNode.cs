using StgSharp.Logic;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    internal sealed class BeginningNode : BlueprintNode
    {
        private Blueprint _bp;

        public BeginningNode( Blueprint bp ) : base(name: "BeginningNode",true, ["default"], ["default"]
            )
        {
            _bp = bp;
        }

        internal (string name, BlueprintPipelineArgs arg)[] InputData
        {
            set
            {
                if (value == null)
                {
                    return;
                }
                foreach (var parameter in value)
                {
                    OutputInterfaces[parameter.name].Args = parameter.arg;
                }
            }
        }

        public new void SetCertainOutputPort(string name, BlueprintPipeline pipeline)
        {
            if (_outputInterfaces.ContainsKey(name))
            {
                _outputInterfaces[name] = pipeline;
            }
            _outputInterfaces.Add(name, pipeline);
        }

        public override void Run()
        {
            _bp.RunningStat.Wait();
            BlueprintPipeline.SkipAll(_outputInterfaces);
        }
    }

    internal sealed class EndingNode : BlueprintNode
    {
        private Blueprint _bp;

        public EndingNode(Blueprint bp) : base(name: "EndingNode",true, ["default"], ["default"]
            )
        {
            _bp = bp;
        }

        internal (string name, BlueprintPipelineArgs arg)[] OutputData
        {
            get
            {
                return InputInterfaces.Select(
                key => (key.Key, key.Value.Args)).
                ToArray();
            }
        }

        public new void SetCertainInputPort(string name, BlueprintPipeline pipeline)
        {
            if (_inputInterfaces.ContainsKey(name))
            {
                _inputInterfaces[name] = pipeline;
            }
            _inputInterfaces.Add(name, pipeline);
        }

        public override void Run()
        {
            BlueprintPipeline.WaitAll(_inputInterfaces);
            Interlocked.Exchange(ref _bp.currentGlobalNodeIndex, 0);
            Interlocked.Exchange(ref _bp.currentNativeNodeIndex, 0);
            _bp.RunningStat.Release();
        }
    }
}