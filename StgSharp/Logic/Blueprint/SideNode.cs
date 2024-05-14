using StgSharp.Logic;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    internal sealed class BeginningNode:BlueprintNode
    {
        public BeginningNode():base(name : "BeginningNode",null,
            true, ["default"], ["default"]
            ) { }

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
                    OutputInterfaces[parameter.name].Value = parameter.arg;
                }
            }
        }

        public new void SetCertainOutputPort(string name, BlueprintPipeline pipeline)
        {
            if (outputInterfaces.ContainsKey(name))
            {
                outputInterfaces[name] = pipeline;
            }
            outputInterfaces.Add(name,pipeline);
        }

        public new void Run()
        {
            BlueprintPipeline.SkipAll(outputInterfaces);
        }
    }

    internal sealed class EndingNode : BlueprintNode
    {
        public EndingNode():base(name:"EndingNode",
            operation: (
                in Dictionary<string , BlueprintPipeline> input,
                in Dictionary<string, BlueprintPipeline> output
                ) => 
            {
            },
            true, ["default"], ["default"]
            ) { }

        internal (string name, BlueprintPipelineArgs arg)[] OutputData
        {
            get 
            { 
                return InputInterfaces.Select(
                key => (key.Key, key.Value.Value) ).
                ToArray();
            }
        }

        public new void SetCertainInputPort(string name, BlueprintPipeline pipeline)
        {
            if (inputInterfaces.ContainsKey(name))
            {
                inputInterfaces[name] = pipeline;
            }
            inputInterfaces.Add(name, pipeline);
        }

        public new void Run() 
        {
            BlueprintPipeline.WaitAll(inputInterfaces);
        }
    }
}
