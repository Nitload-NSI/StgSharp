using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Logic
{
    public interface IConvertableToBlueprintNode
    {
#pragma warning disable CA1819 
        public string[] InputInterfacesName { get; }
#pragma warning restore CA1819 

#pragma warning disable CA1819
        public string[] OutputInterfacesName { get; }
#pragma warning restore CA1819

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExecuteMain(
        in Dictionary<string, BlueprintPipeline> input,
        in Dictionary<string, BlueprintPipeline> output);
    }

    internal class DefaultConvertableToBlueprintNode : IConvertableToBlueprintNode
    {
        private string[] _input;
        private string[] _output;
        private BlueprintNodeOperation _execution;

        string[] IConvertableToBlueprintNode.InputInterfacesName => _input;

        string[] IConvertableToBlueprintNode.OutputInterfacesName => _output;

        void IConvertableToBlueprintNode.ExecuteMain(in Dictionary<string, BlueprintPipeline> input, in Dictionary<string, BlueprintPipeline> output)
        {
            _execution(in input, in output);
        }

        public DefaultConvertableToBlueprintNode(BlueprintNodeOperation execution, string[] input, string[] output)
        {
            _input = input;
            _output = output;
            _execution = execution;
        }
    }

    public delegate void BlueprintNodeOperation(in Dictionary<string, BlueprintPipeline> input, in Dictionary<string, BlueprintPipeline> output);
}
