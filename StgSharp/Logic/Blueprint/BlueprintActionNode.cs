using StgSharp.Logic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharpDebug.Logic
{
    public class BlueprintActionNode : IConvertableToBlueprintNode
    {
        private Action<Dictionary<string, BlueprintPipeline>, Dictionary<string, BlueprintPipeline>> mainAction;
        private string[] inPorts;
        private string[] outPorts;

        internal BlueprintActionNode()
        {
            
        }

        public string[] InputInterfacesName => inPorts;

        public string[] OutputInterfacesName => outPorts;

        public void ExecuteMain(in Dictionary<string, BlueprintPipeline> input, in Dictionary<string, BlueprintPipeline> output)
        {
            mainAction?.Invoke(input, output);
        }
    }
}
